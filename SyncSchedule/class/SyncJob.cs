using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using SyncDBConn;
using SyncLibrary;
using SyncScheduleManager;


namespace SyncSchedule
{
    // 동일 작업 중복 실행 방지
    [DisallowConcurrentExecution]
    public class SyncJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("1111111111");
                int taskId = context.MergedJobDataMap.GetInt("TaskId"); // 작업 ID
                string taskName = context.MergedJobDataMap.GetString("TaskName");
                string scheduleType = context.MergedJobDataMap.GetString("ScheduleType");
                string sourceDB = context.MergedJobDataMap.GetString("SourceDB"); // 원본 DB
                string targetDB = context.MergedJobDataMap.GetString("TargetDB"); // 대상 DB
                string SyncDirection = context.MergedJobDataMap.GetString("SyncDirection"); // S1, S2, S3

                string procedureName = context.MergedJobDataMap.GetString("ProcedureName"); // 호출할 프로시저명
                string referenceTables = context.MergedJobDataMap.GetString("ReferenceTables"); // 참조 테이블

                var combinedTaskSchedules = TaskScheduleManager.CombineTaskAndSchedule();

                // Find the specific task based on TaskId
                var task = combinedTaskSchedules.FirstOrDefault(t => t.Task.TaskId == taskId);

                if (task == null)
                {
                    Console.WriteLine($"Task {taskId} not found.");
                    return;
                }
                // Check if the task is active
                if (!task.Task.IsActive)
                {
                    Console.WriteLine($"Task {taskId} is not active and will not be executed.");
                    return;
                }
                // SyncTaskJob 객체 생성
                var syncTaskJob = new SyncTaskJob
                {

                    taskId = context.MergedJobDataMap.GetInt("TaskId"),
                    taskName = context.MergedJobDataMap.GetString("TaskName"),
                    scheduleType = context.MergedJobDataMap.GetString("ScheduleType"),
                    sourceDB = context.MergedJobDataMap.GetString("SourceDB"),
                    targetDB = context.MergedJobDataMap.GetString("TargetDB"),
                    syncDirection = context.MergedJobDataMap.GetString("SyncDirection"),
                    procedureName = context.MergedJobDataMap.GetString("ProcedureName"),
                    referenceTables = context.MergedJobDataMap.GetString("ReferenceTables").Split(',').ToList()
                };

                // 작업 구분에 따라 적절한 DataSyncProcessor 선택 (예시)
                IDataSyncProcessor selectedProcessor = null;

                //DBConnInfo.
                DBConnInfo dBConnInfo = new DBConnInfo();
                ProxyServerInfo proxyServerInfo = new ProxyServerInfo();

                ProxyServerInfo serverInfo = new ProxyServerInfo();
                serverInfo = ProxyServerInfoManager.LoadServerInfo();
                if (serverInfo == null)
                {
                    return;
                }

                dBConnInfo.proxyDbIp = serverInfo.ServerIP;
                dBConnInfo.proxyDbId = serverInfo.dbid;
                dBConnInfo.proxyDbPw = serverInfo.dbpwd;
                dBConnInfo.proxyDbName = serverInfo.dbname;
                dBConnInfo.proxyDbPort = serverInfo.dbport;

                DBConnectionInfoProvider dbConnectionInfoProvider = new DBConnectionInfoProvider(syncTaskJob.sourceDB, dBConnInfo);
                SqlLogger _logger = new SqlLogger(dbConnectionInfoProvider);


                // Logger로 동기화 작업 시작 로그 남기기
                _logger.LogInformation($"Task {taskId} ({taskName}) 동기화 작업을 시작합니다.");

                if (SyncDirection.StartsWith("S2"))
                {
                    //selectedProcessor = new DataSyncLogProcessor_Unidirection(_logger, dbConnectionInfoProvider, syncTaskJob);

                }
                else if (SyncDirection.StartsWith("S1"))
                {
                    //selectedProcessor = new DataSyncLogProcessor_Bidirection(_logger, dbConnectionInfoProvider, syncTaskJob);
                }

                else if (SyncDirection.StartsWith("S3"))
                {
                    selectedProcessor = new DataSyncLogProcessor_Update(_logger, dbConnectionInfoProvider, syncTaskJob);
                }
                if (selectedProcessor != null)
                {
                    // SyncManager에 선택된 프로세서 주입 후 실행
                    SyncManager syncManager = new SyncManager(selectedProcessor, _logger);
                    //syncManager.SetDataSyncProcessor(selectedProcessor);
                    try
                    {

                        // 동기화 작업 시작

                        // Job 시작 로그 추가
                        Console.WriteLine($"Task {taskId} ({taskName}) started at {DateTime.Now}");
                        await syncManager.StartSync();
                        // Logger로 동기화 작업 완료 로그 남기기
                        _logger.LogInformation($"Task {taskId} ({taskName}) 동기화 작업이 완료되었습니다.");
                        // Job 완료 로그 추가
                        Console.WriteLine($"Task {taskId} ({taskName}) completed at {DateTime.Now}");
                    }
                    catch (Exception ex)
                    {


                        throw new JobExecutionException(ex)
                        {
                            RefireImmediately = false
                        }; // 반드시 이 예외를 던져야 리스너가 인식

                        //throw jobExecutionException;
                        _logger.LogError($"SyncJob - Task {taskId} ({taskName}) 동기화 중 오류 발생: {ex.Message}", ex);

                    }
                }
                else
                {
                    _logger.LogWarning($"Task {taskId} ({taskName})의 동기화 방향이 올바르지 않습니다.");
                }

            }
            catch (Exception ex)
            {
                // 예외 발생 시 DB에 기록하거나 다른 후속 작업 수행
                throw new JobExecutionException(ex);
                Console.WriteLine($"SyncJob - Task 실행 중 예외 발생: {ex.Message}");
            }
        }
    }    
}
