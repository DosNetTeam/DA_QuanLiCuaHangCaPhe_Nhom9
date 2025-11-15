//csharp DataAccess/AdoNetHelper.cs
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess {
    /// <summary>
    /// ADO.NET helper chung — lightweight, thread-safe usage (static).
    /// Lấy connection string từ appsettings.json (DefaultConnection ưu tiên).
    /// </summary>
    public static class AdoNetHelper {
        public static string GetConnectionString() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();

            var conn = config.GetConnectionString("DefaultConnection")
                       ?? config.GetConnectionString("MyDatabase");

            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException("Không tìm thấy chuỗi kết nối 'DefaultConnection' hoặc 'MyDatabase' trong appsettings.json");

            return conn;
        }

        public static SqlConnection CreateConnection() {
            return new SqlConnection(GetConnectionString());
        }

        public static T ExecuteScalar<T>(string sql, Dictionary<string, object>? parameters = null) {
            using var conn = CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            AddParameters(cmd, parameters);
            conn.Open();
            var result = cmd.ExecuteScalar();
            if (result == null || result == DBNull.Value) return default!;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static int ExecuteNonQuery(string sql, Dictionary<string, object>? parameters = null) {
            using var conn = CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            AddParameters(cmd, parameters);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public static DataTable ExecuteDataTable(string sql, Dictionary<string, object>? parameters = null) {
            using var conn = CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            AddParameters(cmd, parameters);
            using var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public static List<T> QueryList<T>(string sql, Func<SqlDataReader, T> map, Dictionary<string, object>? parameters = null) {
            var list = new List<T>();
            using var conn = CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            AddParameters(cmd, parameters);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read()) {
                list.Add(map(rdr));
            }
            return list;
        }

        public static (SqlConnection conn, SqlTransaction tx) BeginTransaction(IsolationLevel isolation = IsolationLevel.ReadCommitted) {
            var conn = CreateConnection();
            conn.Open();
            var tx = conn.BeginTransaction(isolation);
            return (conn, tx);
        }

        public static int ExecuteNonQuery(SqlTransaction tx, string sql, Dictionary<string, object>? parameters = null) {
            using var cmd = tx.Connection!.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            AddParameters(cmd as SqlCommand, parameters);
            return cmd.ExecuteNonQuery();
        }

        private static void AddParameters(SqlCommand? cmd, Dictionary<string, object>? parameters) {
            if (cmd == null || parameters == null) return;
            foreach (var kv in parameters) {
                var paramName = kv.Key.StartsWith("@") ? kv.Key : "@" + kv.Key;
                if (!cmd.Parameters.Contains(paramName))
                    cmd.Parameters.AddWithValue(paramName, kv.Value ?? DBNull.Value);
                else
                    cmd.Parameters[paramName].Value = kv.Value ?? DBNull.Value;
            }
        }
    }
}