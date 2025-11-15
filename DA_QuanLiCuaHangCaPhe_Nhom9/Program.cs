using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_QuanLi;
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main;
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Login;
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

                    // Register IKhoTruyVan with runtime detection: prefer EF if database reachable, otherwise ADO
                    services.AddSingleton<IKhoTruyVan>(sp => {
                        try {
                            using var scope = sp.CreateScope();
                            var db = scope.ServiceProvider.GetRequiredService<DataSqlContext>();
                            // Try to connect using EF
                            if (db.Database.CanConnect()) {
                                return new EfKhoTruyVan();
                            }
                        }
                        catch {
                            // ignore and fallback to ADO
                        }
                        return new AdoKhoTruyVan();
                    });

                    // Register other services used by forms
                    services.AddSingleton<KhoTruyVanDangNhap>();
                    services.AddTransient<KhoTruyVanMainForm>();
                    services.AddTransient<DichVuDonHang>();
                    services.AddTransient<GioHang>(sp => new GioHang(sp.GetRequiredService<DichVuDonHang>()));

                })
                .Build();

            ServiceProvider = host.Services;
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            ApplicationConfiguration.Initialize();

            // Start the application using DI-resolved Loginform so forms can receive IKhoTruyVan from container
            var login = ServiceProvider.GetRequiredService<Loginform>();
            Application.Run(login);
        }
    }
}