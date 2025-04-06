using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncScheduleManager
{
    public class SyncSchedule
    {
        public int ScheduleId { get; set; }         // 스케줄 ID
        public int TaskId { get; set; }             // 연관된 Task ID
        public string SrtDate { get; set; }
        public string ScheduleType { get; set; }    // 스케줄 타입 (OneTime, Recurring, Daily, Weekly)
        public DateTime? SpecificTime { get; set; } // 한 번 수행 또는 일별 수행 시의 지정된 시간
        public TimeSpan? Interval { get; set; }     // 되풀이 수행 시의 주기 (예: 1시간 간격)
        public DayOfWeek? WeekDay { get; set; }     // 주별 수행 시의 요일
    }
}
