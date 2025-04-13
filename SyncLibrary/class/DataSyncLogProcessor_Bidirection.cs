using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncDBConn;

namespace SyncLibrary
{
    public class DataSyncLogProcessor_Bidirection : BaseDataSyncProcessor, IDataSyncProcessor
    {
        private readonly DBConnectionInfoProvider _dbConnectionInfoProvider;
        private readonly SyncTaskJob _syncTaskJob;
        private const int BatchSize = 2000; // 배치로 처리할 로그 수
        private readonly SqlLogger _logger;

        public DataSyncLogProcessor_Bidirection(SqlLogger logger, DBConnectionInfoProvider dbConnectionInfoProvider, SyncTaskJob syncTaskJob)
            : base(logger, dbConnectionInfoProvider, syncTaskJob)
        {
            _logger = logger;
            _dbConnectionInfoProvider = dbConnectionInfoProvider ?? throw new ArgumentNullException(nameof(dbConnectionInfoProvider));
            _syncTaskJob = syncTaskJob;
        }

        // 변경 로그 처리하는 메서드
        public override async Task ProcessLogsAsync()
        {
            DataTable logData = LoadLogs(BatchSize);
            if (logData.Rows.Count == 0)
            {
                LogOperation("No logs to process.");
                return;
            }


            string currentSqlQuery = null;
            var tasks = new List<Task>();
            var rows = logData.AsEnumerable().ToList();
            try
            {

                // 국가 코드에 따른 연결 정보 설정
                var (localConnectionString, remoteConnectionString) = _dbConnectionInfoProvider.GetConnectionInfo(
                    _syncTaskJob.SourceDB,
                    _syncTaskJob.TargetDB);

                // 새로운 DataTable 생성
                DataTable newTable = logData.Clone(); // 구조 복사


                // 각 국가 그룹의 데이터를 처리
                bool success = await ApplyBatchToTempTableAndExecuteProcedureAsync(logData, remoteConnectionString);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Error processing batch: {ex.Message}", currentSqlQuery);
                throw;
                //    //UpdateStatus($"Error: {ex.Message}");
            }
        }

        // 원격DB 연결 후 트랜잭션 시작
        private async Task<bool> ApplyBatchToTempTableAndExecuteProcedureAsync(DataTable logData, string remoteConnectionString)
        {
            using (SqlConnection connection = new SqlConnection(remoteConnectionString))
            {
                if (!IsConnectionActive(connection))
                {
                    UpdateStatus("Destination database connection is inactive. Retrying in next cycle.");
                    return false;
                }
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        List<int> processedLogIds = new List<int>();

                        // 변경 로그를 기반으로 임시 테이블에 데이터 저장
                        bool isTempTableUpdateSuccessful = await ApplyBatchToTempTableAsync(logData, connection, transaction, processedLogIds);

                        // 임시 테이블에 데이터 저장이 성공한 경우에만 저장 프로시저 호출
                        if (isTempTableUpdateSuccessful)
                        {
                            //프로시저가 없는 경우 프로시저 호출은 하지 않는다.
                            if (_syncTaskJob.ProcedureName != "" && _syncTaskJob.ProcedureName != null)
                            {
                                // 프로시저 호출
                                await ExecuteStoredProcedureAsync(connection, transaction);
                            }
                            // 처리된 로그의 상태를 업데이트
                            if (processedLogIds == null || processedLogIds.Count == 0)
                            {
                                throw new Exception("No logs processed. Cannot update log status.");
                            }

                            // 모든 작업이 성공하면 커밋
                            transaction.Commit();
                            UpdateStatus("Batch processed and procedure executed successfully.");

                            bool updateSuccess = MarkLogsAsProcessed(processedLogIds, _dbConnectionInfoProvider.LocalServer());
                            if (!updateSuccess)
                            {
                                UpdateStatus("MarkLogsAsProcessed Error");
                                return false;
                            }
                            return true;
                        }
                        else
                        {
                            //transaction.Rollback();
                            UpdateStatus("Failed to update temp table, rolling back transaction.");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Transaction error: {ex.Message}");
                        UpdateStatus($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        // 변경로그 DB 반영
        private async Task<bool> ApplyBatchToTempTableAsync(DataTable logData, SqlConnection connection, SqlTransaction transaction, List<int> processedLogIds)
        {
            try
            {
                Console.WriteLine("ApplyBatchToTempTableAsync 시작");

                string old_tableName = "";
                //Dictionary<string, string> fieldTypes = null;
                Dictionary<string, (string DataType, int? MaxLength, int? Precision, int? Scale)> fieldTypes = null;
                List<string> primaryKeys = new List<string>();

                foreach (DataRow row in logData.Rows)
                {
                    string tableName = row["TableName"].ToString();
                    string changeType = row["ChangeType"].ToString();
                    string changeDetails = row["ChangeDetails"].ToString();
                    int logId = Convert.ToInt32(row["LogId"]);

                    Console.WriteLine($"테이블: {tableName}, 변경 타입: {changeType}, 로그 ID: {logId}");

                    if (tableName != old_tableName)
                    {
                        old_tableName = tableName;
                        // 필드 타입 및 기본 키 가져오기
                        (fieldTypes, primaryKeys) = GetFieldTypesAndPrimaryKeyFromDatabase(tableName, _dbConnectionInfoProvider.LocalServer());
                        Console.WriteLine($"필드 타입 및 기본 키 갱신 - 테이블: {tableName}");
                    }

                    string queryText = GenerateQueryText(changeType, changeDetails, tableName, primaryKeys, fieldTypes);
                    Console.WriteLine($"{tableName}- 저장쿼리문: {queryText}");

                    if (!IsQuerySafe(queryText))
                    {
                        Console.WriteLine($"안전하지 않은 쿼리 감지: {queryText}");
                        throw new InvalidOperationException("Unsafe query detected");
                    }

                    await ExecuteQueryWithRetriesAsync(connection, queryText, transaction);

                    ProcessSuccess(queryText, logId, processedLogIds, (sql) => queryText = sql);
                    Console.WriteLine($"프로세스 성공 처리 완료 - 로그 ID: {logId}");
                }

                Console.WriteLine("ApplyBatchToTempTableAsync 성공적으로 완료");
                UpdateStatus("Batch processed successfully.");
                return true;
            }
            catch (SqlException sqlEx)
            {
                transaction.Rollback();
                Console.WriteLine($"B-ApplyBatchToTempTableAsync SQL오류: {sqlEx.Message}");
                _logger.LogError($"SQL 오류 발생: {sqlEx.Message}", sqlEx.ToString());

                UpdateStatus($"SQL 오류: {sqlEx.Message}");
                return false;
            }
            catch (InvalidOperationException invEx)
            {
                Console.WriteLine($"B-ApplyBatchToTempTableAsync 유효성 검사 오류 발생: {invEx.Message}");
                transaction.Rollback();
                _logger.LogError($"유효성 검사 오류 발생: {invEx.Message}", invEx.ToString());
                UpdateStatus($"유효성 오류: {invEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"B-ApplyBatchToTempTableAsync 알 수 없는 오류 발생: {ex.Message}");
                transaction.Rollback();
                _logger.LogError($"알 수 없는 오류 발생: {ex.Message}", ex.ToString());
                UpdateStatus($"오류: {ex.Message}");
                return false;
            }
        }

        // 프로시저 호출 
        private async Task ExecuteStoredProcedureAsync(SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                // 프로시저 이름 가져옴
                using (SqlCommand command = new SqlCommand(_syncTaskJob.ProcedureName, connection, transaction))
                {
                    command.CommandType = CommandType.StoredProcedure; // SQL문이 아닌 프로시저임을 명시
                    command.CommandTimeout = 120;
                    string value = "";
                    //command.Parameters.AddWithValue("@wrk_ty", value);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Stored Procedure 실행 중 오류 발생: {ex.Message}", ex.ToString());
                UpdateStatus($"오류: {ex.Message}");
                throw;
            }
        }

    }
}
