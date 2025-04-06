using Quartz.Impl.Matchers;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Quartz.Logging.OperationName;
using System.Numerics;
using System.Security.Policy;

namespace SyncCRMData.SyncMonitoring
{
    public class ScheduleStats
    {
        public int TotalJobsExecuted { get; private set; }
        public int TotalJobsFailed { get; private set; }
        public string LastExecutedJob { get; private set; }

        public void JobExecuted(string jobName)
        {
            TotalJobsExecuted++;
            LastExecutedJob = jobName;
        }

        public void JobFailed(string jobName)
        {
            TotalJobsFailed++;
            LastExecutedJob = jobName;
        }

        public void ResetStats()
        {
            TotalJobsExecuted = 0;
            TotalJobsFailed = 0;
            LastExecutedJob = string.Empty;
        }

        /// <summary>
        /// Quartz.NET에서는 스케줄러에 등록된 작업(Job)과 트리거(Trigger)를 조회할 수 있는 API를 제공합니다. 
        /// 이를 통해 현재 스케줄러에 올라간 작업의 상태나 등록된 작업 목록을 확인할 수 있습니다.
        /// 현재 스케줄러에 등록된 모든 Job 및 Trigger 조회
        /// </summary>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static async Task CheckSchedulerStatus(IScheduler scheduler)
        {
            // 스케줄러에서 등록된 모든 Job의 Key 목록을 가져옴
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            Console.WriteLine("현재 등록된 작업 목록:");
            foreach (var jobKey in jobKeys)
            {
                Console.WriteLine($"- Job: {jobKey.Name}, Group: {jobKey.Group}");

                // Job에 연결된 Trigger 정보 가져오기
                var triggers = await scheduler.GetTriggersOfJob(jobKey);
                foreach (var trigger in triggers)
                {
                    var triggerState = await scheduler.GetTriggerState(trigger.Key);
                    Console.WriteLine($"  - Trigger: {trigger.Key.Name}, 상태: {triggerState}");
                }
            }
        }
        ///
        ///        Quartz.NET에서는 트리거의 상태를 조회할 수 있습니다.트리거 상태는 다음과 같은 값을 가질 수 있습니다:

        //None: 트리거가 존재하지 않음.
        //Normal: 정상적으로 스케줄된 상태.
        //Paused: 트리거가 일시 중지된 상태.
        //Complete: 트리거가 완료된 상태.
        //Error: 트리거 실행 중 오류가 발생한 상태.
        //Blocked: 작업이 이미 실행 중인 상태.
        //Waiting: 트리거가 대기 중인 상태.
        public static async Task GetJobStatus(IScheduler scheduler, JobKey jobKey)
        {
            // Job에 연결된 모든 트리거 조회
            var triggers = await scheduler.GetTriggersOfJob(jobKey);

            foreach (var trigger in triggers)
            {
                var triggerState = await scheduler.GetTriggerState(trigger.Key);
                Console.WriteLine($"Job: {jobKey.Name}, Trigger: {trigger.Key.Name}, 상태: {triggerState}");
            }
        }
        /*스케줄러 전체 상태 조회 (Job, Trigger 등)
        스케줄러에서 **작업(Job)**과 **트리거(Trigger)**의 상세 상태를 확인할 수 있는 전체 상태 조회 코드를 작성할 수 있습니다. 
        이 코드는 스케줄러에 등록된 모든 작업과 그에 대한 트리거 상태를 조회합니다.
            전체 스케줄러 상태 확인 코드
        */
        public async Task CheckSchedulerDetails(IScheduler scheduler)
        {
            // 등록된 모든 Job 조회
            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            Console.WriteLine("스케줄러 상태 확인:");
            foreach (var jobKey in jobKeys)
            {
                Console.WriteLine($"Job: {jobKey.Name}, Group: {jobKey.Group}");

                // Job에 연결된 트리거 상태 확인
                var triggers = await scheduler.GetTriggersOfJob(jobKey);
                foreach (var trigger in triggers)
                {
                    var triggerState = await scheduler.GetTriggerState(trigger.Key);
                    Console.WriteLine($"  - Trigger: {trigger.Key.Name}, 상태: {triggerState}");
                }
            }
        }

        /*
         * Job 및 Trigger의 상세 정보 조회
            각 작업의 자세한 정보를 조회하려면, 특정 작업에 연결된 JobDataMap이나 Trigger 정보를 가져올 수 있습니다.
          */
        public async Task GetJobAndTriggerDetails(IScheduler scheduler, JobKey jobKey)
        {
            // Job에 연결된 트리거 정보 및 상태 조회
            var jobDetail = await scheduler.GetJobDetail(jobKey);
            var triggers = await scheduler.GetTriggersOfJob(jobKey);

            Console.WriteLine($"Job: {jobKey.Name}");
            Console.WriteLine($"Description: {jobDetail.Description}");
            Console.WriteLine($"Job Data:");

            foreach (var data in jobDetail.JobDataMap)
            {
                Console.WriteLine($"  {data.Key}: {data.Value}");
            }

            foreach (var trigger in triggers)
            {
                var triggerState = await scheduler.GetTriggerState(trigger.Key);
                Console.WriteLine($"Trigger: {trigger.Key.Name}, 상태: {triggerState}");
            }
        }






    }
}
