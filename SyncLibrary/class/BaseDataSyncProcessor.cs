using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncDBConn;

namespace SyncLibrary
{
    // 데이터 동기화 클래스가 반드시 구현해야 하는 인터페이스
    public interface IDataSyncProcessor
    {
        Task ProcessLogsAsync();
    }


    // 데이터 동기화 추상 클래스 
    public abstract class BaseDataSyncProcessor : IDataSyncProcessor 
    {
        protected static XmlToSQLScript xmlToSQLScript = new XmlToSQLScript();
        protected readonly string proxyConnectionString;    
        protected string localConnectionString;
        private readonly SyncTaskJob _syncTaskJob;
        DBConnectionInfoProvider dBConnectionInfoProvider;

        protected int maxRetryAttepmts = 3;
        protected int retryDelayMilliseconds = 2000;
        protected readonly SqlLogger _logger;

        public BaseDataSyncProcessor(SqlLogger logger, DBConnectionInfoProvider dbConnectionInfo, SyncTaskJob syncTaskJob)
        {
            _logger = logger;
            dBConnectionInfoProvider = dbConnectionInfo;
            _syncTaskJob = syncTaskJob;
            this.proxyConnectionString = dBConnectionInfoProvider.ProxyServer();
            this.localConnectionString = dBConnectionInfoProvider.LocalServer();
        }
    }
    
}
