using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDBConn
{
    public class SyncTaskJob
    {
        public int taskId { get; set; }
        public string taskName { get; set; }
        public List<string> referenceTables { get; set; }

        public string targetTable { get; set; } // 타겟 테이블
        public string procedureName { get; set; }
        public string sourceDB { get; set; }
        public string targetDB { get; set; }
        public string syncDirection { get; set; } // 동기화 방향 S1,S2,S3
        public bool isActive { get; set; } // 활성화 여부
        public string scheduleType { get; set; } // 스케줄 타입

    }
}
