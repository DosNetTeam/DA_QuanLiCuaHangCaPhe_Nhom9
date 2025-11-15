//csharp DataAccess/DichVuThanhToan.cs
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess {
    public class DichVuThanhToan {
        // ADO.NET fallback operations for payment-related workflows

        public bool HuyDonHangCho(int maDHCanHuy) {
            // Fallback ADO: perform explicit operations with transaction
            string sqlCheck = "SELECT COUNT(1) FROM ThanhToan WHERE MaDH = @MaDH AND TrangThai = N'Ch?a thanh toán'";
            var pcheck = new Dictionary<string, object> { ["@MaDH"] = maDHCanHuy };
            int c = AdoNetHelper.ExecuteScalar<int>(sqlCheck, pcheck);
            if (c == 0) return false;

            // Start transaction (returns connection and transaction)
            var t = AdoNetHelper.BeginTransaction();
            var conn = t.conn;
            var tx = t.tx;

            try {
                // Get chi ti?t ??n (MaSP INT, SoLuong DECIMAL)
                string sqlCT = "SELECT MaSP, SoLuong FROM ChiTietDonHang WHERE MaDH = @MaDH";
                var pd = new Dictionary<string, object> { ["@MaDH"] = maDHCanHuy };
                var chiTiet = AdoNetHelper.QueryList(sqlCT, r => (MaSP: r.GetInt32(0), SoLuong: r.GetDecimal(1)), pd);

                // For each item, find DinhLuong and add back quantities
                foreach (var item in chiTiet) {
                    string sqlDl = "SELECT MaNL, SoLuongCan FROM DinhLuong WHERE MaSP = @MaSP";
                    var pdl = new Dictionary<string, object> { ["@MaSP"] = item.MaSP };
                    var dls = AdoNetHelper.QueryList(sqlDl, r => (MaNL: r.GetInt32(0), SoLuongCan: r.GetDecimal(1)), pdl);
                    foreach (var dl in dls) {
                        string sqlUpd = "UPDATE NguyenLieu SET SoLuongTon = SoLuongTon + @SoLuongCong WHERE MaNL = @MaNL";
                        var pu = new Dictionary<string, object> { ["@SoLuongCong"] = dl.SoLuongCan * item.SoLuong, ["@MaNL"] = dl.MaNL };
                        AdoNetHelper.ExecuteNonQuery(tx, sqlUpd, pu);
                    }
                }

                // Update DonHang and ThanhToan statuses
                string sqlUpdDon = "UPDATE DonHang SET TrangThai = N'?ã h?y' WHERE MaDH = @MaDH";
                string sqlUpdTt = "UPDATE ThanhToan SET TrangThai = N'?ã h?y' WHERE MaDH = @MaDH";
                var pu2 = new Dictionary<string, object> { ["@MaDH"] = maDHCanHuy };
                AdoNetHelper.ExecuteNonQuery(tx, sqlUpdDon, pu2);
                AdoNetHelper.ExecuteNonQuery(tx, sqlUpdTt, pu2);

                tx.Commit();
                conn.Close();
                return true;
            }
            catch {
                try { tx.Rollback(); } catch { }
                try { conn.Close(); } catch { }
                return false;
            }
        }
    }
}