//csharp DataAccess/NhanVienRepository.cs
using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess
{
    public static class NhanVienRepository
    {
        /// <summary>
        /// Insert NhanVien and return newly created MaNv (identity).
        /// </summary>
        public static int Insert(string tenNv, string chucVu, string soDienThoai, DateTime ngayVaoLam, string trangThai = "?ang làm vi?c")
        {
            string sql = @"
                INSERT INTO NhanVien (TenNV, ChucVu, SoDienThoai, NgayVaoLam, TrangThai)
                VALUES (@TenNV, @ChucVu, @SoDienThoai, @NgayVaoLam, @TrangThai);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            var p = new Dictionary<string, object>
            {
                ["@TenNV"] = tenNv,
                ["@ChucVu"] = chucVu,
                ["@SoDienThoai"] = string.IsNullOrWhiteSpace(soDienThoai) ? DBNull.Value : (object)soDienThoai,
                ["@NgayVaoLam"] = ngayVaoLam,
                ["@TrangThai"] = trangThai
            };

            return AdoNetHelper.ExecuteScalar<int>(sql, p);
        }
    }
}