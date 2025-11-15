using DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

//csharp Function/function_Admin/ThongKe.cs

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin {
    public class DuLieuTongQuan {
        public string Category { get; set; }
        public int Count { get; set; }
        public string Details { get; set; }
    }

    public class DuLieuDoanhThu {
        public string Thang { get; set; }
        public int SoDonHang { get; set; }
        public decimal TongDoanhThu { get; set; }
        public decimal DoanhThuTrungBinh { get; set; }
        public decimal DonHangLonNhat { get; set; }
        public decimal DonHangNhoNhat { get; set; }
    }

    public class ThongKe {
        public List<DuLieuTongQuan> TaiDuLieuTongQuan() {
            var overviewData = new List<DuLieuTongQuan>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var allNhanVien = db.NhanViens.ToList();
                    var allSanPham = db.SanPhams.ToList();
                    var allDonHang = db.DonHangs.ToList();
                    var allNguyenLieu = db.NguyenLieus.ToList();
                    var allKhachHang = db.KhachHangs.ToList();

                    int tongNhanVien = allNhanVien.Count;

                    int tongSanPham = 0;
                    foreach (var sp in allSanPham) {
                        if (sp.TrangThai == "Con ban" || sp.TrangThai == "Còn bán") tongSanPham++;
                    }

                    int tongDonHang = allDonHang.Count;

                    int donHangHomNay = 0;
                    foreach (var dh in allDonHang) {
                        if (dh.NgayLap.HasValue && dh.NgayLap.Value.Date == DateTime.Today) donHangHomNay++;
                    }

                    int tongNguyenLieu = allNguyenLieu.Count;
                    int tongKhachHang = allKhachHang.Count;

                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số nhân viên", Count = tongNhanVien, Details = "Nhân viên đang làm việc" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số sản phẩm", Count = tongSanPham, Details = "Sản phẩm đang bán" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số đơn hàng", Count = tongDonHang, Details = "Tất cả đơn hàng" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Đơn hàng hôm nay", Count = donHangHomNay, Details = DateTime.Today.ToString("dd/MM/yyyy") });
                    overviewData.Add(new DuLieuTongQuan { Category = "Nguyên liệu trong kho", Count = tongNguyenLieu, Details = "Tổng số loại nguyên liệu" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Khách hàng", Count = tongKhachHang, Details = "Tổng số khách hàng" });
                }
            }
            catch (Exception) {
                // Fallback ADO: use scalar / simple queries
                try {
                    int tongNhanVien = AdoNetHelper.ExecuteScalar<int>("SELECT COUNT(1) FROM NhanVien");
                    int tongSanPham = AdoNetHelper.ExecuteScalar<int>("SELECT COUNT(1) FROM SanPham WHERE TrangThai IN (N'Con ban', N'Còn bán')");
                    int tongDonHang = AdoNetHelper.ExecuteScalar<int>("SELECT COUNT(1) FROM DonHang");
                    int donHangHomNay = AdoNetHelper.ExecuteScalar<int>("SELECT COUNT(1) FROM DonHang WHERE CONVERT(date, NgayLap) = CONVERT(date, GETDATE())");
                    int tongNguyenLieu = AdoNetHelper.ExecuteScalar<int>("SELECT COUNT(1) FROM NguyenLieu");
                    int tongKhachHang = AdoNetHelper.ExecuteScalar<int>("SELECT COUNT(1) FROM KhachHang");

                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số nhân viên", Count = tongNhanVien, Details = "Nhân viên đang làm việc" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số sản phẩm", Count = tongSanPham, Details = "Sản phẩm đang bán" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số đơn hàng", Count = tongDonHang, Details = "Tất cả đơn hàng" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Đơn hàng hôm nay", Count = donHangHomNay, Details = DateTime.Today.ToString("dd/MM/yyyy") });
                    overviewData.Add(new DuLieuTongQuan { Category = "Nguyên liệu trong kho", Count = tongNguyenLieu, Details = "Tổng số loại nguyên liệu" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Khách hàng", Count = tongKhachHang, Details = "Tổng số khách hàng" });
                }
                catch (Exception ex) {
                    Console.WriteLine("Lỗi khi tải dữ liệu tổng quan (ADO fallback): " + ex.Message);
                }
            }
            return overviewData;
        }

        public List<DuLieuDoanhThu> TaiDuLieuDoanhThu() {
            var revenueData = new List<DuLieuDoanhThu>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var donHangsHopLe = new List<DonHang>();
                    foreach (var dh in db.DonHangs.ToList()) {
                        if (dh.TongTien != null && dh.NgayLap != null) donHangsHopLe.Add(dh);
                    }

                    Dictionary<string, List<DonHang>> groups = new Dictionary<string, List<DonHang>>();
                    foreach (var dh in donHangsHopLe) {
                        if (dh.NgayLap.HasValue) {
                            string key = dh.NgayLap.Value.Month.ToString("00") + "/" + dh.NgayLap.Value.Year.ToString();
                            if (!groups.ContainsKey(key)) groups[key] = new List<DonHang>();
                            groups[key].Add(dh);
                        }
                    }

                    List<DuLieuDoanhThu> tempData = new List<DuLieuDoanhThu>();
                    foreach (var group in groups) {
                        int soDonHang = group.Value.Count;
                        decimal tongDoanhThu = 0;
                        decimal maxDoanhThu = 0;
                        decimal minDoanhThu = decimal.MaxValue;

                        foreach (var dh in group.Value) {
                            decimal tien = dh.TongTien ?? 0;
                            tongDoanhThu += tien;
                            if (tien > maxDoanhThu) maxDoanhThu = tien;
                            if (tien < minDoanhThu) minDoanhThu = tien;
                        }

                        decimal trungBinh = soDonHang > 0 ? tongDoanhThu / soDonHang : 0;

                        tempData.Add(new DuLieuDoanhThu {
                            Thang = group.Key,
                            SoDonHang = soDonHang,
                            TongDoanhThu = Math.Round(tongDoanhThu, 0),
                            DoanhThuTrungBinh = Math.Round(trungBinh, 0),
                            DonHangLonNhat = Math.Round(maxDoanhThu, 0),
                            DonHangNhoNhat = (minDoanhThu == decimal.MaxValue) ? 0 : Math.Round(minDoanhThu, 0)
                        });
                    }

                    tempData.Sort((a, b) => string.Compare(b.Thang, a.Thang));

                    int count = 0;
                    foreach (var item in tempData) {
                        revenueData.Add(item);
                        count++;
                        if (count >= 12) break;
                    }
                }
            }
            catch (Exception) {
                // Fallback ADO: aggregate by month/year
                try {
                    string sql = @"SELECT MONTH(NgayLap) AS M, YEAR(NgayLap) AS Y, COUNT(*) AS SoDon, SUM(ISNULL(TongTien,0)) AS TongDoanhThu
                                   FROM DonHang
                                   WHERE NgayLap IS NOT NULL
                                   GROUP BY YEAR(NgayLap), MONTH(NgayLap)
                                   ORDER BY YEAR(NgayLap) DESC, MONTH(NgayLap) DESC";
                    var rows = AdoNetHelper.QueryList(sql, r => new {
                        M = r.GetInt32(0),
                        Y = r.GetInt32(1),
                        SoDon = r.GetInt32(2),
                        TongDoanhThu = r.IsDBNull(3) ? 0m : r.GetDecimal(3)
                    });

                    int added = 0;
                    foreach (var row in rows) {
                        decimal avg = row.SoDon > 0 ? row.TongDoanhThu / row.SoDon : 0;
                        revenueData.Add(new DuLieuDoanhThu {
                            Thang = row.M.ToString("00") + "/" + row.Y.ToString(),
                            SoDonHang = row.SoDon,
                            TongDoanhThu = Math.Round(row.TongDoanhThu, 0),
                            DoanhThuTrungBinh = Math.Round(avg, 0),
                            DonHangLonNhat = 0,
                            DonHangNhoNhat = 0
                        });
                        added++;
                        if (added >= 12) break;
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine("Lỗi khi tải doanh thu (ADO fallback): " + ex.Message);
                }
            }
            return revenueData;
        }
    }
}