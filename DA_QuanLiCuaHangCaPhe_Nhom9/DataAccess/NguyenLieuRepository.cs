//csharp DataAccess/NguyenLieuRepository.cs
using System;
using System.Collections.Generic;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess
{
    public static class NguyenLieuRepository
    {
        public static List<NguyenLieu> GetAllActive()
        {
            string sql = "SELECT MaNL, TenNL, DonViTinh, SoLuongTon, NguongCanhBao, TrangThai FROM NguyenLieu";
            return AdoNetHelper.QueryList(sql, r =>
            {
                var nl = new NguyenLieu();
                nl.MaNl = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                nl.TenNl = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                nl.DonViTinh = r.IsDBNull(2) ? string.Empty : r.GetString(2);
                nl.SoLuongTon = r.IsDBNull(3) ? 0m : r.GetDecimal(3);
                nl.NguongCanhBao = r.IsDBNull(4) ? 0m : r.GetDecimal(4);
                nl.TrangThai = r.IsDBNull(5) ? string.Empty : r.GetString(5);
                return nl;
            });
        }

        public static NguyenLieu? GetById(int maNl)
        {
            string sql = "SELECT MaNL, TenNL, DonViTinh, SoLuongTon, NguongCanhBao, TrangThai FROM NguyenLieu WHERE MaNL = @MaNL";
            var p = new Dictionary<string, object> { ["@MaNL"] = maNl };
            var list = AdoNetHelper.QueryList(sql, r =>
            {
                var nl = new NguyenLieu();
                nl.MaNl = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                nl.TenNl = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                nl.DonViTinh = r.IsDBNull(2) ? string.Empty : r.GetString(2);
                nl.SoLuongTon = r.IsDBNull(3) ? 0m : r.GetDecimal(3);
                nl.NguongCanhBao = r.IsDBNull(4) ? 0m : r.GetDecimal(4);
                nl.TrangThai = r.IsDBNull(5) ? string.Empty : r.GetString(5);
                return nl;
            }, p);
            return list.Count > 0 ? list[0] : null;
        }

        public static bool UpdateQuantity(int maNl, decimal newQuantity)
        {
            string sql = "UPDATE NguyenLieu SET SoLuongTon = @SoLuongTon WHERE MaNL = @MaNL";
            var p = new Dictionary<string, object> { ["@SoLuongTon"] = newQuantity, ["@MaNL"] = maNl };
            int rows = AdoNetHelper.ExecuteNonQuery(sql, p);
            return rows > 0;
        }
    }
}