using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Dapper.Contrib.Extensions;

namespace FeederAnalysis.Business
{
    public class SQLHelper
    {
        public static string SERVER_NAME = "";
        public static string USERNAME_DB = "";
        public static string PASSWORD_DB = "";
        public static string DATABASE = "";
        public static int TIME_OUT = 3;
        public static bool IS_CONNECTED = true;
        public static string CONNECTION_STRINGS = "";

        public static string ConnectString()
        {
            SQLHelper.CONNECTION_STRINGS = string.Format("Server={0};Database={1};User Id={2};Password = {3}; ;Connection Timeout={4}", (object)SQLHelper.SERVER_NAME, (object)SQLHelper.DATABASE, (object)SQLHelper.USERNAME_DB, (object)SQLHelper.PASSWORD_DB, (object)SQLHelper.TIME_OUT);
            return SQLHelper.CONNECTION_STRINGS;
        }

        public static long Insert<T>(T entityToInsert) where T : class
        {
            using (SqlConnection connection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                connection.Open();
                long num;
                try
                {
                    num = connection.Insert<T>(entityToInsert);
                }
                catch (Exception ex)
                {
                    num = 0L;
                    Console.WriteLine(ex.Message);
                }
                return num;
            }
        }

        public static bool Update<T>(T entityToUpdate) where T : class
        {
            using (SqlConnection connection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                connection.Open();
                bool flag;
                try
                {
                    flag = connection.Update<T>(entityToUpdate);
                }
                catch (Exception ex)
                {
                    flag = false;
                    Console.WriteLine(ex.Message);
                }
                return flag;
            }
        }

        public static bool Delete<T>(T entityToDelete) where T : class
        {
            using (SqlConnection connection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                connection.Open();
                bool flag = false;
                try
                {
                    flag = connection.Delete<T>(entityToDelete);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return flag;
            }
        }

        public static DataTable ExecProcedureDataAsDataTable(
          string ProcedureName,
          object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                DataTable dataTable = new DataTable();
                try
                {
                    SqlConnection cnn = sqlConnection;
                    string sql = ProcedureName;
                    object obj = parametter;
                    CommandType? nullable = new CommandType?(CommandType.StoredProcedure);
                    int? commandTimeout = new int?();
                    CommandType? commandType = nullable;
                    IDataReader reader = cnn.ExecuteReader(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
                    dataTable.Load(reader);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return dataTable;
            }
        }

        public static async Task<DataTable> ExecProcedureDataAsyncAsDataTable(
          string ProcedureName,
          object parametter = null)
        {
            DataTable dataTable = await SQLHelper.WithConnection<DataTable>((Func<IDbConnection, Task<DataTable>>)(async c =>
            {
                IDbConnection cnn = c;
                string sql = ProcedureName;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.StoredProcedure);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                IDataReader reader = await cnn.ExecuteReaderAsync(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
                DataTable table = new DataTable();
                table.Load(reader);
                DataTable dataTable1 = table;
                reader = (IDataReader)null;
                table = (DataTable)null;
                return dataTable1;
            }));
            return dataTable;
        }

        public static DataTable ExecQueryDataAsDataTable(string T_SQL, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = T_SQL;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.Text);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                IDataReader reader = cnn.ExecuteReader(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable;
            }
        }

        public static async Task<DataTable> ExecQueryDataAsyncAsDataTable(
          string T_SQL,
          object parametter = null)
        {
            DataTable dataTable = await SQLHelper.WithConnection<DataTable>((Func<IDbConnection, Task<DataTable>>)(async c =>
            {
                IDbConnection cnn = c;
                string sql = T_SQL;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.Text);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                IDataReader reader = await cnn.ExecuteReaderAsync(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
                DataTable table = new DataTable();
                table.Load(reader);
                DataTable dataTable1 = table;
                reader = (IDataReader)null;
                table = (DataTable)null;
                return dataTable1;
            }));
            return dataTable;
        }

        public static IEnumerable<T> ExecProcedureData<T>(
          string ProcedureName,
          object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = ProcedureName;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.StoredProcedure);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.Query<T>(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
            }
        }

        public static T ExecProcedureDataFistOrDefault<T>(string ProcedureName, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = ProcedureName;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.StoredProcedure);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.QueryFirstOrDefault<T>(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
            }
        }

        public static async Task<IEnumerable<T>> ExecProcedureDataAsync<T>(
          string ProcedureName,
          object parametter = null)
        {
            IEnumerable<T> objs1 = await SQLHelper.WithConnection<IEnumerable<T>>((Func<IDbConnection, Task<IEnumerable<T>>>)(async c =>
            {
                IDbConnection cnn = c;
                string sql = ProcedureName;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.StoredProcedure);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                IEnumerable<T> objs2 = await cnn.QueryAsync<T>(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
                return objs2;
            }));
            return objs1;
        }

        public static T ExecProcedureDataFirstOrDefaultAsync<T>(string ProcedureName, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = ProcedureName;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.StoredProcedure);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.QueryFirstOrDefaultAsync<T>(sql, obj, commandTimeout: commandTimeout, commandType: commandType).Result;
            }
        }

        public static int ExecProcedureNonData(string ProcedureName, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = ProcedureName;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.StoredProcedure);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.Execute(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
            }
        }

        public static int ExecProcedureNonDataAsync(string ProcedureName, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = ProcedureName;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.StoredProcedure);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.ExecuteAsync(sql, obj, commandTimeout: commandTimeout, commandType: commandType).Result;
            }
        }

        public static IEnumerable<T> ExecQueryData<T>(string T_SQL, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = T_SQL;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.Text);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.Query<T>(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
            }
        }

        public static T ExecQueryDataFistOrDefault<T>(string T_SQL, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = T_SQL;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.Text);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.QueryFirstOrDefault<T>(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
            }
        }

        public static async Task<IEnumerable<T>> ExecQueryDataAsync<T>(
          string T_SQL,
          object parametter = null)
        {
            IEnumerable<T> objs1 = await SQLHelper.WithConnection<IEnumerable<T>>((Func<IDbConnection, Task<IEnumerable<T>>>)(async c =>
            {
                IDbConnection cnn = c;
                string sql = T_SQL;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.Text);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                IEnumerable<T> objs2 = await cnn.QueryAsync<T>(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
                return objs2;
            }));
            return objs1;
        }

        public static T ExecQueryDataFirstOrDefaultAsync<T>(string T_SQL, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = T_SQL;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.Text);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.QueryFirstOrDefaultAsync<T>(sql, obj, commandTimeout: commandTimeout, commandType: commandType).Result;
            }
        }

        public static int ExecQueryNonData(string T_SQL, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = T_SQL;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.Text);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.Execute(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
            }
        }

        public static async Task<int> ExecQueryNonDataAsync(string T_SQL, object parametter = null)
        {
            int num1 = await SQLHelper.WithConnection<int>((Func<IDbConnection, Task<int>>)(async c =>
            {
                IDbConnection cnn = c;
                string sql = T_SQL;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.Text);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                int num2 = await cnn.ExecuteAsync(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
                return num2;
            }));
            return num1;
        }

        public static async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            T obj;
            using (SqlConnection connection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                await connection.OpenAsync();
                obj = await getData((IDbConnection)connection);
            }
            return obj;
        }

        public static object ExecProcedureSacalar(string ProcedureName, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = ProcedureName;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.StoredProcedure);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.ExecuteScalar<object>(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
            }
        }

        public static object ExecProcedureSacalarAsync(string ProcedureName, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = ProcedureName;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.StoredProcedure);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.ExecuteScalarAsync<object>(sql, obj, commandTimeout: commandTimeout, commandType: commandType).Result;
            }
        }

        public static object ExecQuerySacalar(string T_SQL, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = T_SQL;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.Text);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.ExecuteScalar<object>(sql, obj, commandTimeout: commandTimeout, commandType: commandType);
            }
        }

        public static object ExecQuerySacalarAsync(string T_SQL, object parametter = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SQLHelper.CONNECTION_STRINGS))
            {
                sqlConnection.Open();
                SqlConnection cnn = sqlConnection;
                string sql = T_SQL;
                object obj = parametter;
                CommandType? nullable = new CommandType?(CommandType.Text);
                int? commandTimeout = new int?();
                CommandType? commandType = nullable;
                return cnn.ExecuteScalarAsync<object>(sql, obj, commandTimeout: commandTimeout, commandType: commandType).Result;
            }
        }
    }
}
