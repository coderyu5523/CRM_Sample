using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace SyncScheduleManager
{
    public class ScheduleFileManager
    {
        private static readonly string filePath = "sync_schedule.json";

        // SyncSchedule 데이터를 파일로 저장하는 메서드
        public static void SaveSchedule(SyncSchedule schedule)
        {
            List<SyncSchedule> schedules = new List<SyncSchedule>();
            // 기존 파일이 있는지 확인하고, 있으면 불러오기
            if (File.Exists(filePath))
            {
                var existingJson = File.ReadAllText(filePath);
        
                // 기존 스케줄 리스트를 JSON에서 역직렬화
                schedules = JsonSerializer.Deserialize<List<SyncSchedule>>(existingJson);
            }
        
            // 동일한 TaskId가 있는지 확인
            var existingSchedule = schedules.FirstOrDefault(s => s.TaskId == schedule.TaskId);
        
            if (existingSchedule != null)
            {
                // 동일한 TaskId가 있으면 기존 스케줄을 업데이트
                existingSchedule.SrtDate = schedule.SrtDate;
                existingSchedule.ScheduleType = schedule.ScheduleType;
                existingSchedule.SpecificTime = schedule.SpecificTime;
                existingSchedule.Interval = schedule.Interval;
                existingSchedule.WeekDay = schedule.WeekDay;
                // 다른 필드들도 필요한 경우 업데이트
            }
            else
            {
                // TaskId가 중복되지 않으면 새 스케줄 추가
                schedules.Add(schedule);
            }
        
            // 업데이트된 리스트를 다시 JSON으로 직렬화하여 파일에 저장
            var updatedJson = JsonSerializer.Serialize(schedules, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
        
            //var jsonString = JsonSerializer.Serialize(schedule, new JsonSerializerOptions { WriteIndented = true });
            //File.WriteAllText(filePath, jsonString);
        }

        // SyncSchedule 데이터를 파일에서 불러오는 메서드
        public static SyncSchedule LoadSchedule(int taskId)
        {
            if (File.Exists(filePath))
            {
                var jsonString = File.ReadAllText(filePath);
                // JSON 파일이 여러 개의 SyncSchedule을 저장하고 있다고 가정
                var schedules = JsonSerializer.Deserialize<List<SyncSchedule>>(jsonString);
         
                // 특정 TaskId에 해당하는 SyncSchedule을 찾음
                var schedule = schedules?.FirstOrDefault(s => s.TaskId == taskId);
         
                return schedule; // 해당하는 TaskId가 없으면 null 반환
            }
            return null; // 파일이 없으면 null 반환
        }

        public static List<SyncSchedule> LoadSchedules()
        {
            //if (File.Exists(filePath))
            //{
            //    var jsonString = File.ReadAllText(filePath);
            //    // JSON 파일이 여러 개의 SyncSchedule을 저장하고 있다고 가정
            //    var schedules = JsonSerializer.Deserialize<List<SyncSchedule>>(jsonString);
            //
            //    // 특정 TaskId에 해당하는 SyncSchedule을 찾음
            //    var schedule = schedules?.FirstOrDefault(s => s.TaskId == taskId);
            //
            //    return schedules; // 해당하는 TaskId가 없으면 null 반환
            //}
            //return null; // 파일이 없으면 null 반환

            if (File.Exists(filePath))
            {
               var jsonString = File.ReadAllText(filePath);
               //return JsonSerializer.Deserialize< List<SyncSchedule>>(jsonString);
               return JsonSerializer.Deserialize<List<SyncSchedule>>(jsonString);
            }
            return null;
        }

    }
}
