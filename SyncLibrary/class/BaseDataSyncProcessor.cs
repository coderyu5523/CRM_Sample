using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SyncDBConn;

namespace SyncLibrary
{
    // 데이터 동기화 클래스가 반드시 구현해야 하는 인터페이스
    public interface IDataSyncProcessor
    {
        Task ProcessLogsAsync();
    }


    // 데이터 동기화 추상 클래스 
    public abstract class BaseDataSyncProcessor : IDataSyncProcessor 
    {
        protected static XmlToSQLScript xmlToSQLScript = new XmlToSQLScript();
        protected readonly string proxyConnectionString;    
        protected string localConnectionString;
        private readonly SyncTaskJob _syncTaskJob;
        DBConnectionInfoProvider dBConnectionInfoProvider;

        protected int maxRetryAttempts = 3;
        protected int retryDelayMilliseconds = 2000;
        protected readonly SqlLogger _logger;

        public BaseDataSyncProcessor(SqlLogger logger, DBConnectionInfoProvider dbConnectionInfo, SyncTaskJob syncTaskJob)
        {
            _logger = logger;
            dBConnectionInfoProvider = dbConnectionInfo;
            _syncTaskJob = syncTaskJob;
            this.proxyConnectionString = dBConnectionInfoProvider.ProxyServer();
            this.localConnectionString = dBConnectionInfoProvider.LocalServer();
        }

        // 추상 메서드 구현
        public abstract Task ProcessLogsAsync();

        // 쿼리 재실행 메서드
        protected async Task ExecuteQueryWithRetriesAsync(SqlConnection connection, string queryText, SqlTransaction transaction)
        {
            int retryCount = 0;

            while (retryCount < maxRetryAttempts)
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(queryText, connection, transaction))
                    {
                        await command.ExecuteNonQueryAsync();
                    }

                    return;
                }
                catch (Exception ex)
                {
                    retryCount++;

                    if (retryCount >= maxRetryAttempts)
                    {
                        Console.WriteLine($"Base-ExecuteQueryWithRetriesAsync 쿼리 실행 실패: {ex.Message}");
                        throw new Exception($"쿼리 실행 실패: {ex.Message}");
                    }
                    _logger.LogError($"SQL 오류 발생: {ex.Message}", ex.ToString());
                    Console.WriteLine($"재시도... 시도 횟수 {retryCount}");
                    await Task.Delay(retryDelayMilliseconds);
                }
            }
        }

        // 쿼리 실행 후 성공적으로 처리된 로그 ID를 기록
        protected void ProcessSuccess(string queryText, int logId, List<int> processedLogIds, Action<string> onSqlExecuted)
        {
            onSqlExecuted?.Invoke(queryText);
            _logger.LogInformation($"LogID-{logId} 처리 완료: {DateTime.Now}");

            processedLogIds.Add(logId);
        }

        //  변경 유형(Insert, Update)에 따라 SQL 쿼리 생성
        protected string GenerateQueryText(string changeType, string changeDetails, string tableName, List<string> primaryKeys, Dictionary<string, (string DataType, int? MaxLength, int? Precision, int? Scale)> fieldTypes)
        {
            if (changeType.ToUpper() == "I")
            {
                return xmlToSQLScript.GenerateInsertSql(changeDetails, tableName, fieldTypes);
            }
            else if (changeType.ToUpper() == "U")
            {
                return xmlToSQLScript.GenerateUpdateSql(changeDetails, tableName, primaryKeys, fieldTypes);
            }
            return string.Empty;
        }


        // 데이터베이스에서 필드 타입과 기본 키를 가져오는 메서드
        protected (Dictionary<string, (string DataType, int? MaxLength, int? Precision, int? Scale)>, List<string>) GetFieldTypesAndPrimaryKeyFromDatabase(string tableName, string connstr)
        {
            var fieldTypes = new Dictionary<string, (string DataType, int? MaxLength, int? Precision, int? Scale)>();
            List<string> primaryKeys = new List<string>();

            using (SqlConnection connection = new SqlConnection(connstr))
            {
                connection.Open();

                string columnQuery = "SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
                using (SqlCommand columnCommand = new SqlCommand(columnQuery, connection))
                {
                    columnCommand.Parameters.AddWithValue("@TableName", tableName);

                    using (SqlDataReader reader = columnCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string columnName = reader["COLUMN_NAME"].ToString();
                            string dataType = reader["DATA_TYPE"].ToString();
                            int? maxLength = reader.IsDBNull(reader.GetOrdinal("CHARACTER_MAXIMUM_LENGTH"))
                                ? (int?)null
                                : reader.GetInt32(reader.GetOrdinal("CHARACTER_MAXIMUM_LENGTH"));
                            int? precision = reader.IsDBNull(reader.GetOrdinal("NUMERIC_PRECISION"))
                                            ? (int?)null
                                            : reader.GetByte(reader.GetOrdinal("NUMERIC_PRECISION"));
                            int? scale = reader.IsDBNull(reader.GetOrdinal("NUMERIC_SCALE"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("NUMERIC_SCALE"));

                            fieldTypes[columnName] = (dataType, maxLength, precision, scale);
                        }
                    }
                }

                if (fieldTypes.Count == 0)
                {
                    throw new InvalidOperationException($"테이블 '{tableName}'에 대한 열 정보를 찾을 수 없습니다.");
                }

                string pkQuery = @"
                    SELECT column_name
                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
                    JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KU
                    ON TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
                    WHERE TC.TABLE_NAME = @TableName AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY'";

                using (SqlCommand pkCommand = new SqlCommand(pkQuery, connection))
                {
                    pkCommand.Parameters.AddWithValue("@TableName", tableName);

                    using (SqlDataReader reader = pkCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            primaryKeys.Add(reader["COLUMN_NAME"].ToString());
                        }

                        if (primaryKeys.Count == 0)
                        {
                            Console.WriteLine($"테이블 '{tableName}'에 기본 키가 존재하지 않습니다.");
                            throw new InvalidOperationException($"테이블 '{tableName}'에 기본 키가 존재하지 않습니다.");
                        }
                    }
                }
            }

            return (fieldTypes, primaryKeys);
        }

        // 쿼리의 안전성을 검사하는 메서드, delete * 금지
        protected bool IsQuerySafe(string queryText)
        {
            string lowerQuery = queryText.ToLower();
            return !(lowerQuery.Contains("delete *"));
        }

        // 업데이트 상태 로깅
        protected void UpdateStatus(string message)
        {
            _logger.LogInformation(message);
        }

        // 로그 기록 메서드
        protected void LogOperation(string message, string sqlQuery = null)
        {
            _logger.LogInformation(message, sqlQuery);
        }

        // 오류 로그 기록 메서드
        protected void LogError(string message, string sqlQuery = null)
        {
            _logger.LogError(message, sqlQuery);
        }

        // 로그를 읽어오는 메서드
        protected DataTable LoadLogs(int batchSize)
        {
            DataTable logData = new DataTable();
            try
            {

                using (SqlConnection connection = new SqlConnection(localConnectionString))
                {
                    connection.Open();

                    string referenceTablesCondition = string.Join(",", _syncTaskJob.ReferenceTables.Select(table => $"'{table}'"));

                    string query = $@"
                                    SELECT TOP (@BatchSize) LogId, TableName, ChangeType, ChangeDetails 
                                    FROM CRMDataSync_ChangeLog with(nolock)
                                    WHERE Processed = 0 
                                    AND TableName IN ({referenceTablesCondition}) Order by logid";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BatchSize", batchSize);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(logData);
                        }
                    }
                }

                return logData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRMDataSync_ChangeLog Read ERROR: {ex.Message}");
                throw new Exception($"CRMDataSync_ChangeLog Read ERROR: {ex.Message}");
            }
        }

        // 로그 처리 완료 후 Processed = 1 로 업데이트
        public bool MarkLogsAsProcessed(List<int> logIds, string connectionString)
        {
            if (logIds == null || logIds.Count == 0)
            {
                throw new Exception("No log IDs provided to update.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE CRMDataSync_ChangeLog SET Processed = 1 WHERE LogId IN (" + string.Join(",", logIds) + ")";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Base-MarkLogsAsProcessed: {ex.Message}");
                throw new Exception($"Failed to update log status: {ex.Message}");
            }
        }

        // 연결 상태 확인용 메서드
        protected bool IsConnectionActive(SqlConnection connection)
        {
            try
            {
                connection.Open();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
    
}
