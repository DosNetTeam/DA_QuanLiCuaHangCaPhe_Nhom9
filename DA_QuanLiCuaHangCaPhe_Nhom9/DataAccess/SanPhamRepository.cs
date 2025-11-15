//csharp DataAccess/SanPhamRepository.cs
using System;
using System.Collections.Generic;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using Microsoft.Data.SqlClient;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess
{
    public static class SanPhamRepository
    {
        public static List<SanPham> GetAllActive()
        {
            string sql = "SELECT MaSP, TenSP, LoaiSP, DonGia, DonVi, TrangThai FROM SanPham";
            return AdoNetHelper.QueryList(sql, r =>
            {
                var sp = new SanPham();
                sp.MaSp = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                sp.TenSp = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                sp.LoaiSp = r.IsDBNull(2) ? string.Empty : r.GetString(2);
                sp.DonGia = r.IsDBNull(3) ? 0m : r.GetDecimal(3);
                sp.DonVi = r.IsDBNull(4) ? string.Empty : r.GetString(4);
                sp.TrangThai = r.IsDBNull(5) ? string.Empty : r.GetString(5);
                return sp;
            });
        }

        public static SanPham? GetById(int maSp)
        {
            string sql = "SELECT MaSP, TenSP, LoaiSP, DonGia, DonVi, TrangThai FROM SanPham WHERE MaSP = @MaSP";
            var p = new Dictionary<string, object> { ["@MaSP"] = maSp };
            var list = AdoNetHelper.QueryList(sql, r =>
            {
                var sp = new SanPham();
                sp.MaSp = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                sp.TenSp = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                sp.LoaiSp = r.IsDBNull(2) ? string.Empty : r.GetString(2);
                sp.DonGia = r.IsDBNull(3) ? 0m : r.GetDecimal(3);
                sp.DonVi = r.IsDBNull(4) ? string.Empty : r.GetString(4);
                sp.TrangThai = r.IsDBNull(5) ? string.Empty : r.GetString(5);
                return sp;
            }, p);
            return list.Count > 0 ? list[0] : null;
        }
    }
}