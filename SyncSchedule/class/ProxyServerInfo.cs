using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSchedule
{
    public class ProxyServerInfo
    {
        public string serverIp { get; set; } // db 아이피
        public string dbId { get; set; } // db 아이디
        public string dbPw { get; set; } // db 비밀번호
        public string dbName { get; set; } // db 이름
        public string dbPort { get; set; } // 접근 포트
    }
}
