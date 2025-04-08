using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SyncScheduleManager
{
    public class TaskScheduleManager
    {
        public static List<SyncTask> LoadTasks() 
        {
            if (File.Exists("SyncTask.json"))
            {
                var jsonString = File.ReadAllText("SyncTask.json");
                return JsonSerializer.Deserialize<List<SyncTask>>(jsonString);
            }
            return new List<SyncTask>();
        }

        public static List<SyncSchedule> LoadSchedules() 
        {
            if (File.Exists("ProxyServerInfo.json"))
            {
                var jsonString = File.ReadAllText("ProxyServerInfo.json");
                return JsonSerializer.Deserialize<List<SyncSchedule>>(jsonString);
            }
            return new List<SyncSchedule>();
        }

        public static List<CombinedTaskSchedule> CombineTaskAndSchedule()
        {
            var tasks = TaskFileManager.LoadTasks();
            var schedules = ScheduleFileManager.LoadSchedules();

            var combinedList = from task in tasks
                               where task.IsActive
                               join schedule in schedules on task.TaskId equals schedule.TaskId
                               select new CombinedTaskSchedule
                               {
                                   Task = task,
                                   Schedule = schedule
                               };

            return combinedList.ToList();


        }
    }
}
