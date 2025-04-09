using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SyncDBConn;

namespace SyncLibrary
{
    public class SqlLogger : ILogger
    {
        private readonly string _connectionString;

        public SqlLogger(DBConnectionInfoProvider dBConnectionInfoProvider)
        {
            _connectionString = dBConnectionInfoProvider.ProxyServer();
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // 로그 레벨에 따라 기록할지 여부를 결정
            return logLevel >= LogLevel.Information;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);

            try
            {
                // SQL Server에 로그 기록
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string logCommand = "INSERT INTO CRMDataSync_OperationLog (OperationDetails, OperationDate, LogLevel, SqlQuery) VALUES (@OperationDetails, GETDATE(), @LogLevel, @SqlQuery)";
                    using (SqlCommand command = new SqlCommand(logCommand, connection))

                    {
                        command.Parameters.AddWithValue("@OperationDetails", message);
                        command.Parameters.AddWithValue("@LogLevel", logLevel.ToString());
                        command.Parameters.AddWithValue("@SqlQuery", exception?.Message ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // 예외 처리 로직 (예: 로그 파일에 기록)
                Console.WriteLine($"Error logging to SQL Server: {ex.Message}");
            }
        }
        public void LogError(string message, string sqlQuery = null)
        {
            // 로그 레벨을 Error로 설정하여 로그 기록
            Log(LogLevel.Error, new EventId(), message, null, (state, exception) => state.ToString());
        }   
    }
}