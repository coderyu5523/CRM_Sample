using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SyncScheduleManager;

namespace SyncScheduleManager
{
    public class TaskFileManager
    {
        private static string baseDirectory = @"C:\Sync_CRMData";
        private static readonly string taskFilePath = Path.Combine(baseDirectory, "config", "sync_tasks.json");

        // SyncTask 데이터를 파일로 저장하는 메서드
        public static void SaveTask(SyncTask task)
        {
            Console.WriteLine("1");
            List<SyncTask> tasks = new List<SyncTask>();
            Console.WriteLine("2");


            // 파일이 존재하면 기존 파일 읽어오기
            if (File.Exists(taskFilePath))
            {
                Console.WriteLine("3");

                var existingJson = File.ReadAllText(taskFilePath);

                Console.WriteLine("4");

                tasks = JsonSerializer.Deserialize<List<SyncTask>>(existingJson);

                Console.WriteLine("5");

            }

            // 기존 TaskId와 동일한 Task가 있는지 확인
            var existingTask = tasks.FirstOrDefault(t => t.TaskId == task.TaskId);

            if (existingTask != null)
            {
                // 기존 TaskId가 존재하면 업데이트
                existingTask.TaskName = task.TaskName;
                existingTask.ReferenceTables = task.ReferenceTables;
                existingTask.TargetTable = task.TargetTable;
                existingTask.ProcedureName = task.ProcedureName;
                existingTask.SourceDB = task.SourceDB;
                existingTask.TargetDB = task.TargetDB;
                existingTask.SyncDirection = task.SyncDirection;
                existingTask.IsActive = task.IsActive;
            }
            else
            {
                // TaskId가 존재하지 않으면 새로 추가
                tasks.Add(task);
            }

            // 업데이트된 리스트를 다시 파일에 저장
            var updatedJson = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(taskFilePath, updatedJson);
        }

        // 여러 개의 SyncTask 저장
        public static void SaveTasks(List<SyncTask> newTasks)
        {
            List<SyncTask> tasks = new List<SyncTask>();

            // 파일이 존재하면 기존 파일 읽어오기
            if (File.Exists(taskFilePath))
            {
                var existingJson = File.ReadAllText(taskFilePath);
                tasks = JsonSerializer.Deserialize<List<SyncTask>>(existingJson);
            }

            // 새로운 Task 목록을 처리
            foreach (var newTask in newTasks)
            {
                var existingTask = tasks.FirstOrDefault(t => t.TaskId == newTask.TaskId);

                if (existingTask != null)
                {
                    // 기존 TaskId가 존재하면 업데이트
                    existingTask.TaskName = newTask.TaskName;
                    existingTask.ReferenceTables = newTask.ReferenceTables;
                    existingTask.TargetTable = newTask.TargetTable;
                    existingTask.ProcedureName = newTask.ProcedureName;
                    existingTask.SourceDB = newTask.SourceDB;
                    existingTask.TargetDB = newTask.TargetDB;
                    existingTask.SyncDirection = newTask.SyncDirection;
                    existingTask.IsActive = newTask.IsActive;
                }
                else
                {
                    // TaskId가 존재하지 않으면 새로 추가
                    tasks.Add(newTask);
                }
            }

            // 업데이트된 리스트를 다시 파일에 저장
            var updatedJson = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(taskFilePath, updatedJson);
        }
        // SyncTask 데이터를 파일에서 불러오는 메서드
        public static SyncTask LoadTask()
        {
            if (File.Exists(taskFilePath))
            {
                var jsonString = File.ReadAllText(taskFilePath);
                return JsonSerializer.Deserialize<SyncTask>(jsonString);
            }
            return null;
        }

        // 여러 개의 SyncTask 불러오기
        public static List<SyncTask> LoadTasks()
        {
            if (File.Exists(taskFilePath))
            {
                var jsonString = File.ReadAllText(taskFilePath);
                return JsonSerializer.Deserialize<List<SyncTask>>(jsonString);
            }
            return new List<SyncTask>();
        }



        // 작업 데이터를 파일에서 불러와 SourceDB로 필터링하는 메서드
        public static List<SyncTask> LoadTasks(string sourceDB)
        {
            // 프로젝트 내 Data 폴더에 sync_tasks.json이 있는 경우

            if (File.Exists(taskFilePath))
            {
                var jsonString = File.ReadAllText(taskFilePath);
                var allTasks = JsonSerializer.Deserialize<List<SyncTask>>(jsonString);

                // 선택된 SourceDB와 일치하는 작업들만 필터링
                return allTasks.Where(t => t.SourceDB == sourceDB).ToList();
            }
            return new List<SyncTask>();
        }
    }
}
