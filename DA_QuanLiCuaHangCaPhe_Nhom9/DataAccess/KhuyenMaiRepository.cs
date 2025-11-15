//csharp DataAccess/KhuyenMaiRepository.cs
using System;
using System.Collections.Generic;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess
{
    public static class KhuyenMaiRepository
    {
        public static List<KhuyenMai> GetAll()
        {
            string sql = @"SELECT MaKM, TenKM, MoTa, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, TrangThai
                           FROM KhuyenMai
                           ORDER BY NgayBatDau DESC, TenKM";
            return AdoNetHelper.QueryList(sql, r =>
            {
                var km = new KhuyenMai();
                km.MaKm = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                km.TenKm = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                km.MoTa = r.IsDBNull(2) ? null : r.GetString(2);
                km.LoaiKm = r.IsDBNull(3) ? null : r.GetString(3);
                km.GiaTri = r.IsDBNull(4) ? 0m : r.GetDecimal(4);
                km.NgayBatDau = r.IsDBNull(5) ? DateOnly.FromDateTime(DateTime.MinValue) : DateOnly.FromDateTime(r.GetDateTime(5));
                km.NgayKetThuc = r.IsDBNull(6) ? DateOnly.FromDateTime(DateTime.MinValue) : DateOnly.FromDateTime(r.GetDateTime(6));
                km.TrangThai = r.IsDBNull(7) ? null : r.GetString(7);
                return km;
            });
        }

        public static List<KhuyenMai> GetActiveByType(string loai)
        {
            string sql = @"SELECT MaKM, TenKM, MoTa, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, TrangThai
                           FROM KhuyenMai
                           WHERE LoaiKM = @loai AND TrangThai = N'?ang áp d?ng'
                           ORDER BY GiaTri DESC";
            var p = new Dictionary<string, object> { ["@loai"] = loai };
            return AdoNetHelper.QueryList(sql, r =>
            {
                var km = new KhuyenMai();
                km.MaKm = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                km.TenKm = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                km.MoTa = r.IsDBNull(2) ? null : r.GetString(2);
                km.LoaiKm = r.IsDBNull(3) ? null : r.GetString(3);
                km.GiaTri = r.IsDBNull(4) ? 0m : r.GetDecimal(4);
                km.NgayBatDau = r.IsDBNull(5) ? DateOnly.FromDateTime(DateTime.MinValue) : DateOnly.FromDateTime(r.GetDateTime(5));
                km.NgayKetThuc = r.IsDBNull(6) ? DateOnly.FromDateTime(DateTime.MinValue) : DateOnly.FromDateTime(r.GetDateTime(6));
                km.TrangThai = r.IsDBNull(7) ? null : r.GetString(7);
                return km;
            }, p);
        }
    }
}