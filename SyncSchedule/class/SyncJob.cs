using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using SyncDBConn;
using SyncScheduleManager;

namespace SyncSchedule
{
    // 동일 작업 중복 실행 방지
    [DisallowConcurrentExecution]
    public class SyncJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            int taskId = context.JobDetail.JobDataMap.GetInt("TaskId");
            string taskName = context.JobDetail.JobDataMap.GetString("TaskName");
            string scheduleType = context.JobDetail.JobDataMap.GetString("ScheduleType");
            string sourceDB = context.JobDetail.JobDataMap.GetString("SourceDB");
            string targetDB = context.JobDetail.JobDataMap.GetString("TargetDB");
            string syncDirection = context.JobDetail.JobDataMap.GetString("SyncDirection"); // 동기화 방향 S1,S2,S3

            string procedureName = context.JobDetail.JobDataMap.GetString("ProcedureName"); // 호출할 프로시저명
            string referenceTables = context.JobDetail.JobDataMap.GetString("ReferenceTables"); // 참조 테이블

            var combinedTaskSchedules = TaskScheduleManager.CombineTaskAndSchedule(); // Task에 매칭되는 Schedule을 가져옴

            var task = combinedTaskSchedules.FirstOrDefault(t => t.Task.TaskId == taskId); // 태스크 객체 저장

            if (task == null) 
            {
                Console.WriteLine($"Task with ID {taskId} not found.");
                return;
            }
            if(!task.Task.IsActive)
            {
                Console.WriteLine($"Task with ID {taskId} is not active.");
                return;
            }
            
            var syncTask = new SyncTaskJob
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

            IDataSyncProcessor selectedProcessor = null;

        }
    }
   
}
