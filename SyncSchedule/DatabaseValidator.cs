using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSchedule
{
    public class DatabaseValidator
    {
        private readonly string _connectionString;

        public DatabaseValidator(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 테이블 존재 여부와 Primary Key 확인
        public bool TableExistsAndHasPrimaryKey(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // 테이블 존재 여부 확인
                string tableQuery = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";
                using (SqlCommand tableCommand = new SqlCommand(tableQuery, connection))
                {
                    tableCommand.Parameters.AddWithValue("@TableName", tableName);

                    int tableCount = (int)tableCommand.ExecuteScalar();
                    if (tableCount == 0)
                    {
                        // 테이블이 존재하지 않음
                        return false;
                    }
                }

                // Primary Key 존재 여부 확인
                string pkQuery = @"
                SELECT COUNT(*)
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
                JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KU
                ON TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
                WHERE TC.TABLE_NAME = @TableName AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY'";

                using (SqlCommand pkCommand = new SqlCommand(pkQuery, connection))
                {
                    pkCommand.Parameters.AddWithValue("@TableName", tableName);

                    int pkCount = (int)pkCommand.ExecuteScalar();
                    if (pkCount == 0)
                    {
                        // 테이블에는 Primary Key가 없음
                        return false;
                    }
                }
            }

            // 테이블이 존재하고 Primary Key가 있음
            return true;
        }

        // 프로시저 존재 여부 확인
        public bool ProcedureExists(string procedureName, string connstr)
        {
            using (SqlConnection connection = new SqlConnection(connstr))
            {
                connection.Open();

                string procedureQuery = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = @ProcedureName AND ROUTINE_TYPE = 'PROCEDURE'";
                using (SqlCommand command = new SqlCommand(procedureQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProcedureName", procedureName);

                    int procedureCount = (int)command.ExecuteScalar();
                    return procedureCount > 0;
                }
            }
        }
    }

}
