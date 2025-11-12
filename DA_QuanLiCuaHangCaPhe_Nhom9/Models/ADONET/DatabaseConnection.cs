using Microsoft.Extensions.Configuration; // Cần thư viện này

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models {
    // Đây là class TĨNH (static), nghĩa là ta có thể gọi nó
    // trực tiếp mà không cần dùng "new"
    public static class DatabaseConnection {
        // Biến này sẽ giữ chuỗi kết nối (chỉ đọc 1 lần)
        private static string? _connectionString;

        // Đây là hàm sẽ cung cấp chuỗi kết nối cho MỌI hàm ADO.NET
        public static string GetConnectionString() {
            // Chỉ đọc file appsettings.json 1 lần duy nhất
            if (_connectionString == null) {
                // Tìm file appsettings.json
                var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                var configuration = configBuilder.Build();

                // Lấy chuỗi kết nối có tên là "MyDatabase"
                _connectionString = configuration.GetConnectionString("MyDatabase");
            }

            // Nếu không tìm thấy, báo lỗi
            if (_connectionString == null) {
                throw new System.Exception("LỖI: Không tìm thấy chuỗi kết nối 'MyDatabase' trong file appsettings.json");
            }

            // Trả về chuỗi kết nối
            return _connectionString;
        }
    }
}