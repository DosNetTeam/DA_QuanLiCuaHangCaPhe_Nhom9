//csharp DataAccess/KhachHangRepository.cs
using System;
using System.Collections.Generic;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess
{
    public static class KhachHangRepository
    {
        public static KhachHang? FindByPhone(string sdt)
        {
            string sql = "SELECT MaKH, TenKH, SoDienThoai, DiaChi, LoaiKH FROM KhachHang WHERE SoDienThoai = @sdt";
            var p = new Dictionary<string, object> { ["@sdt"] = sdt };
            var list = AdoNetHelper.QueryList(sql, r =>
            {
                var kh = new KhachHang();
                kh.MaKh = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                kh.TenKh = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                kh.SoDienThoai = r.IsDBNull(2) ? null : r.GetString(2);
                kh.DiaChi = r.IsDBNull(3) ? null : r.GetString(3);
                kh.LoaiKh = r.IsDBNull(4) ? null : r.GetString(4);
                return kh;
            }, p);
            return list.Count > 0 ? list[0] : null;
        }

        public static bool Insert(KhachHang kh)
        {
            string sql = "INSERT INTO KhachHang (TenKH, SoDienThoai, DiaChi, LoaiKH) VALUES (@TenKH, @SoDienThoai, @DiaChi, @LoaiKH)";
            var p = new Dictionary<string, object>
            {
                ["@TenKH"] = kh.TenKh,
                ["@SoDienThoai"] = string.IsNullOrWhiteSpace(kh.SoDienThoai) ? DBNull.Value : (object)kh.SoDienThoai,
                ["@DiaChi"] = string.IsNullOrWhiteSpace(kh.DiaChi) ? DBNull.Value : (object)kh.DiaChi,
                ["@LoaiKH"] = string.IsNullOrWhiteSpace(kh.LoaiKh) ? "Thuong" : (object)kh.LoaiKh
            };
            int rows = AdoNetHelper.ExecuteNonQuery(sql, p);
            return rows > 0;
        }
    }
}