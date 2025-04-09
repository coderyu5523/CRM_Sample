using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using SyncsCRMData;

namespace SyncCRMData
{
    public class MyJobListener : IJobListener
    {
        private Action<string> _jobExecutedCallBack;

        private readonly Action<int, string, DateTime, string, string, TimeSpan?, string, string, string, string, string, string> _jobExecutedCallback;
        private readonly LogManager _logManager; // LogManager 인스턴스 추가

        public MyJobListener(Action<int, string, DateTime, string, string, TimeSpan?, string, string, string, string, string, string> jobExecutedCallback)
        {
            _jobExecutedCallback = jobExecutedCallback;
            _logManager = new LogManager(); // LogManager 인스턴스 초기화
        }
    

    public string Name => "MyJobListener";
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken)
        {
            var jobData = context.JobDetail.JobDataMap;

            // 정수형 필드 TaskId와 ScheduleId 가져오기
            int taskId = jobData.GetInt("TaskId"); // TaskId는 정수형
            string taskName = jobData.GetString("TaskName"); // TaskName은 문자열
            DateTime startTime = DateTime.Now; // 현재 시간을 기록
            string status = "Running";
            // 스케줄 타입, 인터벌, 특정 시간 등 가져오기
            string scheduleType = jobData.GetString("ScheduleType"); // 문자열
            TimeSpan? interval = jobData.ContainsKey("Interval") && jobData["Interval"] != null
                ? (TimeSpan?)TimeSpan.Parse(jobData.GetString("Interval"))
                : null; // TimeSpan으로 변환
            DateTime? specificTime = jobData.ContainsKey("SpecificTime") && jobData["SpecificTime"] != null
                ? (DateTime?)DateTime.Parse(jobData.GetString("SpecificTime"))
                : null; // Nullable DateTime 변환

            // SourceDB, TargetDB, SyncDirection 가져오기
            string sourceDB = jobData.GetString("SourceDB");
            string targetDB = jobData.GetString("TargetDB");
            string referenceTables = jobData.GetString("ReferenceTables");
            string procedureName = jobData.GetString("ProcedureName");
            string syncDirection = jobData.GetString("SyncDirection");
            string targettable = jobData.GetString("TargetTable");
            // 로그 정보를 생성
            string logInfo = $"{taskId}, {taskName},{startTime}, {status}, {scheduleType}, {interval}, {sourceDB}, {targetDB}, {referenceTables}, {procedureName},{syncDirection},{targettable}";

            // LogManager를 사용해 로그 파일에 저장
            _logManager.SaveLogToFile(logInfo);

            _jobExecutedCallback?.Invoke(taskId, taskName, startTime, "Running", scheduleType, interval, sourceDB, targetDB, referenceTables, procedureName, syncDirection, targettable);
            return Task.CompletedTask;
        }
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken)
        {
            // 작업이 취소되거나 중복 실행될 경우 처리 (스킵된 경우)
            var jobData = context.JobDetail.JobDataMap;
            int taskId = jobData.GetInt("TaskId");
            string taskName = jobData.GetString("TaskName");

            Console.WriteLine($"Task {taskId} ({taskName}) 중복 실행으로 스킵됨.");

            // 추가로 스킵된 작업을 로그에 저장하거나 필요한 작업 수행
            return Task.CompletedTask;
        }
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            var jobData = context.JobDetail.JobDataMap;


            // 정수형 필드 TaskId와 ScheduleId 가져오기
            int taskId = jobData.GetInt("TaskId"); // TaskId는 정수형
            string taskName = jobData.GetString("TaskName"); // TaskName은 문자열
            DateTime endTime = DateTime.Now; // 현재 시간을 기록

            // 스케줄 타입, 인터벌, 특정 시간 등 가져오기
            string scheduleType = jobData.GetString("ScheduleType"); // 문자열
            TimeSpan? interval = jobData.ContainsKey("Interval") && jobData["Interval"] != null
                ? (TimeSpan?)TimeSpan.Parse(jobData.GetString("Interval"))
                : null; // TimeSpan으로 변환

            // SourceDB, TargetDB, SyncDirection 가져오기
            string sourceDB = jobData.GetString("SourceDB");
            string targetDB = jobData.GetString("TargetDB");
            string referenceTables = jobData.GetString("ReferenceTables");
            string procedureName = jobData.GetString("ProcedureName");
            string syncDirection = jobData.GetString("SyncDirection");
            string targettable = jobData.GetString("TargetTable");

            string status = jobException == null ? "Success" : $"Failed ({jobException.Message})";

            // 로그 정보를 생성
            string logInfo = $"{taskId}, {taskName},{endTime}, {status}, {scheduleType}, {interval}, {sourceDB}, {targetDB}, {referenceTables}, {procedureName},{syncDirection},{targettable}";

            // LogManager를 사용해 로그 파일에 저장
            _logManager.SaveLogToFile(logInfo);

            _jobExecutedCallback?.Invoke(taskId, taskName, endTime, status, scheduleType, interval, sourceDB, targetDB, referenceTables, procedureName, syncDirection, targettable);
            return Task.CompletedTask;
        }
    }
    }
