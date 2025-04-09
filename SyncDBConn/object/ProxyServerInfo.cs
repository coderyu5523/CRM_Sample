using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDBConn
{
    public class ProxyServerInfo
    {
        public string ServerIP { get; set; }                  // DB서버아이피
        public string dbid { get; set; }             // db id

        public string dbpwd { get; set; }          // db pwd
        public string dbname { get; set; }        // db명
        public string dbport { get; set; }             // 접근포트

    }
}
