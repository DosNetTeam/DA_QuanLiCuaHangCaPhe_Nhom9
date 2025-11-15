//csharp Function/function_Main/DichVuDonHang.cs
using System;
using System.Collections.Generic;
using System.Linq;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main {

    public class DichVuDonHang {

        public enum TrangThaiKho {
            DuHang, SapHet, HetHang
        }

        public class KetQuaKiemKho {
            public bool DuHang { get; set; }
            public string? ThongBao { get; set; } // made nullable to avoid non-nullable warnings
        }

        // Return stock status for a product based on its DinhLuong and current NguyenLieu
        public TrangThaiKho KiemTraDuNguyenLieu(int maSp, List<DinhLuong> allDinhLuong, List<NguyenLieu> allNguyenLieu) {
            try {
                bool anySapHet = false;
                foreach (var dl in allDinhLuong) {
                    if (dl.MaSp != maSp) continue;
                    foreach (var nl in allNguyenLieu) {
                        if (nl.MaNl != dl.MaNl) continue;
                        decimal ton = nl.SoLuongTon ?? 0m;
                        decimal needed = dl.SoLuongCan;
                        if (ton < needed) return TrangThaiKho.HetHang;
                        if (ton < needed * 2) anySapHet = true;
                        break;
                    }
                }
                return anySapHet ? TrangThaiKho.SapHet : TrangThaiKho.DuHang;
            }
            catch {
                return TrangThaiKho.DuHang;
            }
        }

        // Minimal pricing: return given price (placeholder for real pricing logic)
        public decimal GetGiaBan(int maSp, decimal giaGoc) {
            return giaGoc;
        }

        // Return detailed result matching GioHang expectations
        // Now checks real stock using EF when available, otherwise ADO fallback
        public KetQuaKiemKho KiemTraSoLuongTonThucTe(int maSp, int soLuongYeuCau) {
            var res = new KetQuaKiemKho { DuHang = false, ThongBao = null };
            if (soLuongYeuCau <= 0) { res.DuHang = false; res.ThongBao = "Số lượng không hợp lệ"; return res; }

            try {
                // Try EF approach first
                using (var db = new DataSqlContext()) {
                    // Load formula lines for product
                    var dinhluong = db.DinhLuongs.Where(d => d.MaSp == maSp).ToList();
                    if (dinhluong == null || dinhluong.Count == 0) {
                        // No recipe -> assume product does not consume ingredients (e.g., raw product) -> allow
                        res.DuHang = true; return res;
                    }

                    // For each ingredient required, check stock
                    foreach (var dl in dinhluong) {
                        var nl = db.NguyenLieus.FirstOrDefault(n => n.MaNl == dl.MaNl);
                        decimal ton = nl?.SoLuongTon ?? 0m;
                        decimal required = dl.SoLuongCan * soLuongYeuCau;
                        if (ton < required) {
                            res.DuHang = false;
                            res.ThongBao = $"Nguyên liệu '{nl?.TenNl ?? "(#" + dl.MaNl + ")"}' không đủ. Cần {required}, còn {ton}.";
                            return res;
                        }
                    }

                    // All ingredients sufficient
                    res.DuHang = true;
                    return res;
                }
            }
            catch (Exception) {
                // Fallback to ADO queries
                try {
                    string sqlDinhLuong = "SELECT MaNL, SoLuongCan FROM DinhLuong WHERE MaSP = @MaSP";
                    var dls = AdoNetHelper.QueryList(sqlDinhLuong, r => new { MaNl = r.IsDBNull(0) ? 0 : r.GetInt32(0), SoLuongCan = r.IsDBNull(1) ? 0m : r.GetDecimal(1) }, new Dictionary<string, object> { ["@MaSP"] = maSp });
                    if (dls == null || dls.Count == 0) { res.DuHang = true; return res; }

                    foreach (var dl in dls) {
                        var rows = AdoNetHelper.QueryList("SELECT TenNL, ISNULL(SoLuongTon,0) FROM NguyenLieu WHERE MaNL = @MaNL", r => new { Ten = r.IsDBNull(0) ? string.Empty : r.GetString(0), Ton = r.IsDBNull(1) ? 0m : r.GetDecimal(1) }, new Dictionary<string, object> { ["@MaNL"] = dl.MaNl });
                        decimal ton = 0m; string ten = dl.MaNl.ToString();
                        if (rows.Count > 0) { ton = rows[0].Ton; ten = rows[0].Ten; }
                        decimal required = dl.SoLuongCan * soLuongYeuCau;
                        if (ton < required) {
                            res.DuHang = false;
                            res.ThongBao = $"Nguyên liệu '{ten}' không đủ. Cần {required}, còn {ton}.";
                            return res;
                        }
                    }

                    res.DuHang = true;
                    return res;
                }
                catch (Exception ex) {
                    // If fallback also fails, be conservative and disallow
                    res.DuHang = false;
                    res.ThongBao = "Không thể kiểm tra tồn kho: " + ex.Message;
                    return res;
                }
            }
        }
    }
}