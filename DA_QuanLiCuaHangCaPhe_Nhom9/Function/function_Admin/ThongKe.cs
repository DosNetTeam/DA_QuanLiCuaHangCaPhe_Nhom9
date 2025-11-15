using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin {
    // --- CÁC LỚP CHỨA KẾT QUẢ TRUY VẤN (DTOs) ---
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


    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho chức năng Thống Kê (Tổng Quan, Doanh Thu).

    public class ThongKe {
        public List<DuLieuTongQuan> TaiDuLieuTongQuan() {
            var overviewData = new List<DuLieuTongQuan>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Lấy dữ liệu thô
                    var allNhanVien = db.NhanViens.ToList();
                    var allSanPham = db.SanPhams.ToList();
                    var allDonHang = db.DonHangs.ToList();
                    var allNguyenLieu = db.NguyenLieus.ToList();
                    var allKhachHang = db.KhachHangs.ToList();

                    // 1. Tổng NV
                    int tongNhanVien = allNhanVien.Count;

                    // 2. Tổng SP
                    int tongSanPham = 0;
                    foreach (var sp in allSanPham) {
                        if (sp.TrangThai == "Con ban" || sp.TrangThai == "Còn bán") {
                            tongSanPham++;
                        }
                    }

                    // 3. Tổng ĐH
                    int tongDonHang = allDonHang.Count;

                    // 4. ĐH Hôm nay
                    int donHangHomNay = 0;
                    foreach (var dh in allDonHang) {
                        if (dh.NgayLap.HasValue && dh.NgayLap.Value.Date == DateTime.Today) {
                            donHangHomNay++;
                        }
                    }

                    // 5. Tổng NL
                    int tongNguyenLieu = allNguyenLieu.Count;

                    // 6. Tổng KH
                    int tongKhachHang = allKhachHang.Count;

                    // Thêm vào danh sách
                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số nhân viên", Count = tongNhanVien, Details = "Nhân viên đang làm việc" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số sản phẩm", Count = tongSanPham, Details = "Sản phẩm đang bán" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số đơn hàng", Count = tongDonHang, Details = "Tất cả đơn hàng" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Đơn hàng hôm nay", Count = donHangHomNay, Details = DateTime.Today.ToString("dd/MM/yyyy") });
                    overviewData.Add(new DuLieuTongQuan { Category = "Nguyên liệu trong kho", Count = tongNguyenLieu, Details = "Tổng số loại nguyên liệu" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Khách hàng", Count = tongKhachHang, Details = "Tổng số khách hàng" });
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải dữ liệu tổng quan: {ex.Message}");
            }
            return overviewData;
        }

        public List<DuLieuDoanhThu> TaiDuLieuDoanhThu() {
            var revenueData = new List<DuLieuDoanhThu>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var donHangsHopLe = new List<DonHang>();
                    foreach (var dh in db.DonHangs.ToList()) {
                        if (dh.TongTien != null && dh.NgayLap != null) {
                            donHangsHopLe.Add(dh);
                        }
                    }

                    Dictionary<string, List<DonHang>> groups = new Dictionary<string, List<DonHang>>();
                    foreach (var dh in donHangsHopLe) {
                        if (dh.NgayLap.HasValue) {
                            string key = dh.NgayLap.Value.Month.ToString("00") + "/" + dh.NgayLap.Value.Year.ToString();
                            if (!groups.ContainsKey(key)) {
                                groups[key] = new List<DonHang>();
                            }
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

                    // Sắp xếp và lấy 12
                    tempData.Sort((a, b) => string.Compare(b.Thang, a.Thang));

                    int count = 0;
                    foreach (var item in tempData) {
                        revenueData.Add(item);
                        count++;
                        if (count >= 12) break;
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải dữ liệu doanh thu: {ex.Message}");
            }
            return revenueData;
        }
    }
}