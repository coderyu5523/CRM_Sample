using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncScheduleManager
{
    public class SyncTask
    {
        public int TaskId { get; set; }                  // 작업 단위 ID
        public string TaskName { get; set; }             // 작업 단위 명칭
        public List<string> ReferenceTables { get; set; } // 원본 테이블 목록 (여러 개일 수 있음)
        public string TargetTable { get; set; }          // 목적지 테이블
        public string ProcedureName { get; set; }        // 호출할 프로시저명
        public string SourceDB { get; set; }             // 원본 DB
        public string TargetDB { get; set; }             // 대상 DB
        public string SyncDirection { get; set; }        // 동기화 방향 (DB1 -> DB2, DB2 -> DB1, 양방향)
        public bool IsActive { get; set; }               // 작업 활성화 여부
    }
}
