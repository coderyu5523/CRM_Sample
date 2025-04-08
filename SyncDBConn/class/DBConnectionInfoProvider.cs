using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDBConn
{
    public class DBConnectionInfoProvider
    {
        private readonly string _localServer;
        private readonly string _proxyServer;
        private readonly Dictionary<string, string> _connectionInfoCache;

        public DBConnectionInfoProvider(string sourceDBServer, DBConnInfo dbConnInfo)
        {
            _connectionInfoCache = new Dictionary<string, string>();
            LoadAllConnectionInfo(dbConnInfo.GetProxyConnectionString());
            _proxyServer = dbConnInfo.GetProxyConnectionString();
            _localServer = GetConnectionInfo(sourceDBServer);
        }

        public DBConnectionInfoProvider(string proxyConnectionString,string localConnectionString , DBConnInfo dbConnInfo)
        {
            _connectionInfoCache = new Dictionary<string, string>();
            LoadAllConnectionInfo(dbConnInfo.GetProxyConnectionString());
            _proxyServer = proxyConnectionString;
        }

        public string ProxyServer() 
        {
            return _proxyServer;
        }
        public string LocalServer()
        {
            return _localServer;
        }

        // 모든 접속 정보를 로드하여 캐시에 저장
        private void LoadAllConnectionInfo(string _connectionString)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT co_cd,dbip,dbname,port,id,pw FROM CRMConnInfoTable with(nolock)", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string co_cd = reader["co_cd"].ToString();
                            string dbip = reader["dbip"].ToString();
                            string dbname = reader["dbname"].ToString();
                            string port = reader["port"].ToString();
                            string id = reader["id"].ToString();
                            string pw = reader["pw"].ToString();
                            // 접속 정보를 캐시에 저장
                            string connectionString = Setting(dbip, id, pw, dbname, port);

                            _connectionInfoCache[co_cd] = connectionString;

                        }
                    }
                }
            }
        }


        public string Setting(string ip,string id, string password, string dbName,string port)
        {
            string dbConn = "SERVER=" + ip + "," + port + ";" +
                            "DATABASE=" + dbName + ";" +
                            "UID=" + id + ";" +
                            "PWD=" + password + ";" +
                            "Connection Timeout=10";
            return dbConn;
        }

        public string GetConnectionInfo(string countryCode)
        {
            if (_connectionInfoCache.ContainsKey(countryCode))
            {
                return _connectionInfoCache[countryCode];
            }
            else
            {
                throw new KeyNotFoundException($"출발지 국가 코드 '{countryCode}'에 대한 연결 정보를 찾을 수 없습니다.");
            }
        }


    }


}
