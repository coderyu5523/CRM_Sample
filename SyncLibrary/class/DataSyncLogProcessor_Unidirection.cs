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
    public class DataSyncLogProcessor_Unidirection : BaseDataSyncProcessor, IDataSyncProcessor
    {

        private readonly DBConnectionInfoProvider _dbConnectionInfoProvider;
        List<int> processedLogIds = new List<int>(); // 클래스 필드로 선언
        private readonly SyncTaskJob _syncTaskJob;
        private const int BatchSize = 30000; // 배치로 처리할 로그 수
        private SqlLogger _logger; // Logger 인스턴스 추가
        // 진행 상태 및 로그 업데이트를 위한 이벤트
        public event Action<string> StatusUpdated;
        public event Action<string> LogUpdated;

        //연결정보를 받는 경우에는 파라메터로 받는다

        public DataSyncLogProcessor_Unidirection(SqlLogger logger, DBConnectionInfoProvider dbConnectionInfoProvider, SyncTaskJob syncTaskJob) : base(logger, dbConnectionInfoProvider, syncTaskJob)
        {
            _logger = logger;
            _dbConnectionInfoProvider = dbConnectionInfoProvider ?? throw new ArgumentNullException(nameof(dbConnectionInfoProvider));
            _syncTaskJob = syncTaskJob;
        }

        public override async Task ProcessLogsAsync()
        {

            DataTable logData = LoadLogs(BatchSize);
            UpdateStatus($"Process Start - {logData.Rows.Count} 건 - {DateTime.Now}");

            if (logData.Rows.Count == 0)
            {
                UpdateStatus("No logs to process.");
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


                // 비동기 작업 호출
                bool isProcessed = await ApplyBatchToRemoteDatabaseAsync(logData, processedLogIds, (sql) => currentSqlQuery = sql, remoteConnectionString);

                if (isProcessed)
                {
                    // 처리된 로그의 상태를 업데이트
                    MarkLogsAsProcessed(processedLogIds, localConnectionString);
                    //UpdateStatus($"Batch processed successfully for {row["src_nat_cd"].ToString()}.");
                }

                //}));
                //}

                // 모든 작업이 완료될 때까지 대기
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Error processing batch: {ex.Message}", currentSqlQuery);
                throw;
                //UpdateStatus($"Error: {ex.Message}");
            }
        }



        public async void Batch_DataGet()
        {
            await ProcessLogsAsync();
        }


        private async Task<bool> ApplyBatchToRemoteDatabaseAsync(DataTable logData, List<int> processedLogIds, Action<string> onSqlExecuted, string remoteConnectionString)
        {
            try
            {


                using (SqlConnection connection = new SqlConnection(remoteConnectionString))
                {
                    if (!IsConnectionActive(connection))
                    {
                        UpdateStatus("Destination database connection is inactive. Retrying in next cycle.");
                        return false; // 연결이 비활성화된 경우
                    }

                    await connection.OpenAsync();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string old_tableName = "";
                            //Dictionary<string, string> fieldTypes = null;
                            Dictionary<string, (string DataType, int? MaxLength, int? Precision, int? Scale)> fieldTypes = null;
                            List<string> primaryKeys = new List<string>();
                            foreach (DataRow row in logData.Rows)
                            {

                                string tableName = row["TableName"].ToString();
                                string changeType = row["ChangeType"].ToString();
                                string changeDetails = row["ChangeDetails"].ToString();
                                int logId = Convert.ToInt32(row["LogId"]); // 로그 ID 가져오기
                                if (tableName != old_tableName)
                                {
                                    old_tableName = tableName;
                                    (fieldTypes, primaryKeys) = GetFieldTypesAndPrimaryKeyFromDatabase(tableName, localConnectionString);
                                }
                                //string targettable = _syncTaskJob.TargetTable;
                                string queryText = GenerateQueryText(changeType, changeDetails, tableName, primaryKeys, fieldTypes);


                                if (!IsQuerySafe(queryText))
                                {
                                    Console.WriteLine("LogID-" + logId.ToString() + DateTime.Now.ToString() + "쿼리구문오류' - " + queryText);
                                    _logger.LogError($"SQL 오류 발생: {queryText}");
                                    throw new InvalidOperationException("Unsafe query detected");
                                }

                                await ExecuteQueryWithRetriesAsync(connection, queryText, transaction);

                                // SQL 문을 콜백을 통해 전달
                                onSqlExecuted?.Invoke(queryText);
                                Console.WriteLine("LogID-" + logId.ToString() + DateTime.Now.ToString() + "' - " + queryText);
                                // 로그가 성공적으로 처리된 경우 processedLogIds에 추가
                                processedLogIds.Add(logId);
                            }

                            transaction.Commit();

                            return true;
                        }
                        catch (SqlException sqlEx)
                        {
                            _logger.LogError($"SQL 오류 발생: {sqlEx.Message}", sqlEx.ToString());
                            transaction.Rollback();

                            //UpdateStatus($"SQL 오류: {sqlEx.Message}");
                            throw;
                            //return false;
                        }
                        catch (InvalidOperationException invEx)
                        {
                            transaction.Rollback();
                            //_logger.LogError($"유효성 검사 오류 발생: {invEx.Message}", invEx.ToString());
                            //UpdateStatus($"유효성 오류: {invEx.Message}");
                            throw;
                            //return false;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            //_logger.LogError($"알 수 없는 오류 발생: {ex.Message}", ex.ToString());
                            //UpdateStatus($"오류: {ex.Message}");
                            throw;
                            //return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                //_logger.LogError($"알 수 없는 오류 발생: {ex.Message}", ex.ToString());
                //UpdateStatus($"오류: {ex.Message}");
                throw;
                //return false;
            }
        }


    }
}
