//csharp Function/function_QuanLi/KhoTruyVanQuanLi.cs
// This file contains the low-level ADO logic used by AdoKhoTruyVan.
using System;
using System.Collections.Generic;
using DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_QuanLi {
    public class KhoTruyVanQuanLi {
        // Existing methods kept as-is
        public List<string> TaiThongBao() {
            var list = new List<string>();
            try {
                int pending = AdoNetHelper.ExecuteScalar<int>("SELECT COUNT(1) FROM DonHang WHERE TrangThai = N'Đang xử lý'");
                if (pending > 0) list.Add($"Có {pending} đơn hàng đang xử lý.");
                int low = AdoNetHelper.ExecuteScalar<int>("SELECT COUNT(1) FROM NguyenLieu WHERE SoLuongTon < ISNULL(NguongCanhBao,0)");
                if (low > 0) list.Add($"Có {low} nguyên liệu dưới ngưỡng cảnh báo.");
                list.Add("Hệ thống hoạt động bình thường.");
            }
            catch (Exception ex) {
                list.Clear(); list.Add("Không thể tải thông báo: " + ex.Message);
            }
            return list;
        }

        public List<object> TaiDuLieuNhanVien(int thang) {
            var result = new List<object>();
            try {
                string sql = @"
                    SELECT nv.TenNV, COUNT(dh.MaDH) AS SoDon, ISNULL(SUM(dh.TongTien),0) AS TongDoanhThu
                    FROM NhanVien nv
                    LEFT JOIN DonHang dh ON nv.MaNv = dh.MaNv AND (@month = 0 OR MONTH(dh.NgayLap) = @month)
                    GROUP BY nv.TenNV
                    ORDER BY TongDoanhThu DESC";
                var rows = AdoNetHelper.QueryList(sql, r => new NhanVienDto {
                    TenNV = r.IsDBNull(0) ? string.Empty : r.GetString(0),
                    SoDon = r.IsDBNull(1) ? 0 : r.GetInt32(1),
                    TongDoanhThu = r.IsDBNull(2) ? 0m : r.GetDecimal(2),
                    HieuSuat = ""
                }, new Dictionary<string, object> { ["@month"] = thang });

                // Map simple performance label
                foreach (var nv in rows) {
                    if (nv.TongDoanhThu > 1000000) nv.HieuSuat = "Xuất Sắc";
                    else if (nv.TongDoanhThu > 500000) nv.HieuSuat = "Tốt";
                    else nv.HieuSuat = "Cần Cải Thiện";
                    result.Add(nv);
                }
            }
            catch (Exception) { }
            return result;
        }

        public List<object> TaiDuLieuHoaDon(string timKiem, string trangThai) {
            var result = new List<object>();
            try {
                string sql = @"
                    SELECT dh.MaDH, COALESCE(kh.TenKH, N'Khách vãng lai') AS TenKH, dh.NgayLap, ISNULL(dh.TongTien,0) AS TongTien, dh.TrangThai
                    FROM DonHang dh
                    LEFT JOIN KhachHang kh ON dh.MaKH = kh.MaKH
                    WHERE (@tim = '' OR dh.MaDH = @timInt OR LOWER(kh.TenKH) LIKE '%' + @tim + '%')
                      AND (@status = 'Tất cả' OR dh.TrangThai = @status)
                    ORDER BY dh.NgayLap DESC";

                int timInt = 0;
                if (!string.IsNullOrWhiteSpace(timKiem) && int.TryParse(timKiem, out var tmp)) timInt = tmp;
                var rows = AdoNetHelper.QueryList(sql, r => new HoaDonDto {
                    MaDH = r.IsDBNull(0) ? 0 : r.GetInt32(0),
                    TenKH = r.IsDBNull(1) ? string.Empty : r.GetString(1),
                    NgayLap = r.IsDBNull(2) ? (DateTime?)null : r.GetDateTime(2),
                    TongTien = r.IsDBNull(3) ? 0m : r.GetDecimal(3),
                    TrangThai = r.IsDBNull(4) ? string.Empty : r.GetString(4)
                }, new Dictionary<string, object> { ["@tim"] = timKiem?.ToLower() ?? "", ["@timInt"] = timInt, ["@status"] = trangThai });

                foreach (var h in rows) result.Add(h);
            }
            catch (Exception) { }
            return result;
        }

        public List<object> TaiDuLieuTonKho(string timKiem) {
            var result = new List<object>();
            try {
                string sql = @"SELECT MaNL, TenNL, DonViTinh, ISNULL(SoLuongTon,0) AS SoLuongTon, ISNULL(NguongCanhBao,0) AS NguongCanhBao FROM NguyenLieu";
                var rows = AdoNetHelper.QueryList(sql, r => new TonKhoDto {
                    MaNl = r.IsDBNull(0) ? 0 : r.GetInt32(0),
                    TenNl = r.IsDBNull(1) ? string.Empty : r.GetString(1),
                    DonViTinh = r.IsDBNull(2) ? string.Empty : r.GetString(2),
                    SoLuongTon = r.IsDBNull(3) ? 0m : r.GetDecimal(3),
                    NguongCanhBao = r.IsDBNull(4) ? 0m : r.GetDecimal(4)
                });

                foreach (var nl in rows) {
                    // compute status
                    if (nl.SoLuongTon <= 0) nl.TinhTrang = "Hết hàng";
                    else if (nl.SoLuongTon <= nl.NguongCanhBao) nl.TinhTrang = "Thiếu thốn";
                    else nl.TinhTrang = "Dồi dào";
                    if (!string.IsNullOrWhiteSpace(timKiem) && !nl.TenNl.ToLower().Contains(timKiem.ToLower())) continue;
                    result.Add(nl);
                }
            }
            catch (Exception) { }
            return result;
        }

        public List<object> TaiDuLieuSanPham(string timKiem) {
            var result = new List<object>();
            try {
                string sql = @"SELECT MaSP, TenSP, LoaiSP, DonGia, DonVi, TrangThai FROM SanPham";
                var rows = AdoNetHelper.QueryList(sql, r => new SanPhamDto {
                    MaSp = r.IsDBNull(0) ? 0 : r.GetInt32(0),
                    TenSp = r.IsDBNull(1) ? string.Empty : r.GetString(1),
                    Loai = r.IsDBNull(2) ? string.Empty : r.GetString(2),
                    DonGia = r.IsDBNull(3) ? 0m : r.GetDecimal(3),
                    DonVi = r.IsDBNull(4) ? string.Empty : r.GetString(4),
                    TrangThai = r.IsDBNull(5) ? string.Empty : r.GetString(5)
                });

                foreach (var sp in rows) {
                    if (!string.IsNullOrWhiteSpace(timKiem) && !sp.TenSp.ToLower().Contains(timKiem.ToLower())) continue;
                    result.Add(sp);
                }
            }
            catch (Exception) { }
            return result;
        }

        public List<object> TaiDuLieuKhuyenMai(string timKiem, string trangThai) {
            var result = new List<object>();
            try {
                string sql = @"SELECT MaKM, TenKM, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, TrangThai FROM KhuyenMai";
                var rows = AdoNetHelper.QueryList(sql, r => new KhuyenMaiDto {
                    MaKm = r.IsDBNull(0) ? 0 : r.GetInt32(0),
                    TenKM = r.IsDBNull(1) ? string.Empty : r.GetString(1),
                    Loai = r.IsDBNull(2) ? string.Empty : r.GetString(2),
                    GiaTri = r.IsDBNull(3) ? 0m : r.GetDecimal(3),
                    NgayBatDau = r.IsDBNull(4) ? DateTime.MinValue : r.GetDateTime(4),
                    NgayKetThuc = r.IsDBNull(5) ? DateTime.MinValue : r.GetDateTime(5),
                    TrangThai = r.IsDBNull(6) ? string.Empty : r.GetString(6)
                });

                foreach (var km in rows) {
                    if (!string.IsNullOrWhiteSpace(timKiem) && !km.TenKM.ToLower().Contains(timKiem.ToLower())) continue;
                    if (trangThai != "Tất cả" && !string.IsNullOrWhiteSpace(trangThai) && km.TrangThai != trangThai) continue;
                    result.Add(km);
                }
            }
            catch (Exception) { }
            return result;
        }
    }
}