//csharp DataAccess/TaiKhoanRepository.cs
namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess {
    public static class TaiKhoanRepository {
        public static bool ExistsUsername(string username) {
            string sql = "SELECT COUNT(1) FROM TaiKhoan WHERE TenDangNhap = @username";
            var parameters = new Dictionary<string, object> { ["@username"] = username };
            int count = AdoNetHelper.ExecuteScalar<int>(sql, parameters);
            return count > 0;
        }

        public static bool Create(string tenDangNhap, string matKhau, int maNv, int maVaiTro, bool trangThai = true) {
            string sql = @"INSERT INTO TaiKhoan (TenDangNhap, MatKhau, MaNV, MaVaiTro, TrangThai)
                           VALUES (@TenDangNhap, @MatKhau, @MaNV, @MaVaiTro, @TrangThai)";
            var p = new Dictionary<string, object> {
                ["@TenDangNhap"] = tenDangNhap,
                ["@MatKhau"] = matKhau,
                ["@MaNV"] = maNv,
                ["@MaVaiTro"] = maVaiTro,
                ["@TrangThai"] = trangThai ? 1 : 0
            };
            int rows = AdoNetHelper.ExecuteNonQuery(sql, p);
            return rows > 0;
        }
    }
}