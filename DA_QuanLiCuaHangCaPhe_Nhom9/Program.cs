using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }


        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();

            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) => {
                    // Đọc file appsettings.json
                    builder.SetBasePath(AppContext.BaseDirectory)
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) => {
                    // Lấy chuỗi kết nối từ file appsettings.json
                    string connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                    // Đăng ký DbContext với chuỗi kết nối này
                    services.AddDbContext<DataSqlContext>(options =>
                        options.UseSqlServer(connectionString));

                    // Đăng ký Form chính của bạn (ví dụ tên là Form1)
                    services.AddTransient<Loginform>();
                })
                .Build();

            ServiceProvider = host.Services;
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            ApplicationConfiguration.Initialize();

            Application.Run(new Loginform());
        }
    }
}