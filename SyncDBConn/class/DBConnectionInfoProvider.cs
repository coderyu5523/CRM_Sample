using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDBConn
{
    // 소스DB와 타겟DBㅇ의 연결 정보를 관리하는 클래스
    public class DBConnectionInfoProvider
    {
        private readonly string _localServer; // 소스DB
        private readonly string _proxyServer; // 중계DB
        private readonly Dictionary<string, string> _connectionInfoCache; // co_cd에 따른 연결 정보 캐시

        public DBConnectionInfoProvider(string sourceDBServer, DBConnInfo dbConnInfo)
        {
            _connectionInfoCache = new Dictionary<string, string>();
            LoadAllConnectionInfo(dbConnInfo.GetProxyConnectionString()); // CRMConnInfoTable를 읽어 캐시에 저장
            _proxyServer = dbConnInfo.GetProxyConnectionString(); // 프록시서버 연결 정보 저장
            _localServer = GetConnectionInfo(sourceDBServer); // LoadAllConnectionInfo를 통해 저장된 연결 정보를 저장. 이후에 캐시에서 꺼내서 사용
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

        // 국가 코드에 따른 연결 정보를 반환, kr crm이 국가코드
        public string GetConnectionInfo(string countryCode)
        {
            if (_connectionInfoCache.ContainsKey(countryCode))
            {
                // 해당 국가 코드가 존재할 경우, 캐시에서 반환
                return _connectionInfoCache[countryCode]; // 국가코드에 맞는 DB 연결 문자열 반환
            }
            else
            {
                // 국가 코드가 없을 경우 예외 처리 혹은 기본 값을 반환
                throw new KeyNotFoundException($"출발지 국가 코드 '{countryCode}'에 대한 연결 정보를 찾을 수 없습니다.");
            }
        }

        // kr, crm 등 목적지 데이터 베이스에 따라 연결 문자열을 반환
        public (string SourceConnectionString, string DestinationConnectionString) GetConnectionInfo(string srcNatCd, string desNatCd)
        {
            if (_connectionInfoCache.TryGetValue(srcNatCd, out var sourceConnection) &&
                _connectionInfoCache.TryGetValue(desNatCd, out var destinationConnection))
            {
                return (sourceConnection, destinationConnection);
            }
            throw new Exception($"Connection info not found for source: {srcNatCd} or destination: {desNatCd}");
        }


    }


}
