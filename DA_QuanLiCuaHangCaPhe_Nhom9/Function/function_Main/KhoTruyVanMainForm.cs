using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main {
    /// <summary>
    /// Lớp này dùng để trả về nhiều danh sách
    /// từ hàm LayDuLieuSanPham()
    /// </summary>
    public class DuLieuSanPham {
        public List<SanPham> TatCaSanPham { get; set; }
        public List<DinhLuong> AllDinhLuong { get; set; }
        public List<NguyenLieu> AllNguyenLieu { get; set; }
    }

    /// <summary>
    /// Dùng để chuyển thông tin giỏ hàng từ
    /// MainForm sang KhoTruyVan
    /// </summary>
    public class ChiTietGioHang {
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }


    /// <summary>
    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho các nhu cầu của MainForm.
    /// (ĐÃ VIẾT LẠI BẰNG FOREACH)
    /// </summary>
    public class KhoTruyVanMainForm {
        /// <summary>
        /// Lấy danh sách tên các Loại Sản Phẩm (dùng foreach)
        /// </summary>
        public List<string> TaiLoaiSanPham() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var tatCaSanPham = db.SanPhams.ToList();
                    var cacLoaiSPTam = new List<string>();

                    // Thay thế cho .Select(sp => sp.LoaiSp) và .Where(loai => loai != null && loai != "")
                    foreach (var sp in tatCaSanPham) {
                        if (sp.LoaiSp != null && sp.LoaiSp != "") {
                            cacLoaiSPTam.Add(sp.LoaiSp);
                        }
                    }

                    // Thay thế cho .Distinct()
                    var ketQua = new List<string>();
                    foreach (var loai in cacLoaiSPTam) {
                        if (!ketQua.Contains(loai)) // Nếu chưa có trong danh sách kết quả
                        {
                            ketQua.Add(loai);
                        }
                    }
                    return ketQua;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải loại sản phẩm: " + ex.InnerException?.Message ?? ex.Message);
                return new List<string>();
            }
        }

        /// <summary>
        /// Lấy tất cả dữ liệu thô cần thiết để hiển thị sản phẩm (dùng foreach)
        /// </summary>
        public DuLieuSanPham LayDuLieuSanPham() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var tatCaSanPham_raw = db.SanPhams.ToList();
                    var tatCaNguyenLieu_raw = db.NguyenLieus.ToList();
                    var allDinhLuong = db.DinhLuongs.ToList(); // (Gốc không có filter)

                    var tatCaSanPham_filter = new List<SanPham>();
                    var allNguyenLieu_filter = new List<NguyenLieu>();

                    // Thay thế .Where(sp => sp.TrangThai == "Còn bán")
                    foreach (var sp in tatCaSanPham_raw) {
                        if (sp.TrangThai == "Còn bán") {
                            tatCaSanPham_filter.Add(sp);
                        }
                    }

                    // Thay thế .Where(nl => nl.TrangThai == "Đang kinh doanh")
                    foreach (var nl in tatCaNguyenLieu_raw) {
                        if (nl.TrangThai == "Đang kinh doanh") {
                            allNguyenLieu_filter.Add(nl);
                        }
                    }

                    return new DuLieuSanPham {
                        TatCaSanPham = tatCaSanPham_filter,
                        AllDinhLuong = allDinhLuong,
                        AllNguyenLieu = allNguyenLieu_filter
                    };
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message);
                return new DuLieuSanPham {
                    TatCaSanPham = new List<SanPham>(),
                    AllDinhLuong = new List<DinhLuong>(),
                    AllNguyenLieu = new List<NguyenLieu>()
                };
            }
        }

        /// <summary>
        /// Tìm một khách hàng bằng SĐT (dùng foreach)
        /// </summary>
        public KhachHang SearchKhachHangBySDT(string sdt) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var tatCaKhachHang = db.KhachHangs.ToList();

                    // Thay thế .FirstOrDefault(kh => kh.SoDienThoai == sdt)
                    foreach (var kh in tatCaKhachHang) {
                        if (kh.SoDienThoai == sdt) {
                            return kh; // Tìm thấy, trả về ngay
                        }
                    }
                    return null; // Không tìm thấy
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tìm khách hàng: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Lấy KM Hóa Đơn tốt nhất (Logic foreach gốc của bạn)
        /// </summary>
        public KhuyenMai LayKhuyenMaiHoaDon() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Đây là logic foreach gốc của bạn từ CapNhatTongTien
                    DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                    var allKM = db.KhuyenMais.ToList();
                    KhuyenMai kmHoaDon = null;

                    foreach (KhuyenMai km in allKM) {
                        if (km.LoaiKm == "HoaDon" &&
                            km.TrangThai == "Đang áp dụng" &&
                            km.NgayBatDau <= now &&
                            km.NgayKetThuc >= now) {
                            if (kmHoaDon == null || km.GiaTri > kmHoaDon.GiaTri) {
                                kmHoaDon = km;
                            }
                        }
                    }
                    return kmHoaDon; // Trả về (có thể là null)
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi lấy KM hóa đơn: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Lưu đơn hàng tạm, trừ kho (Logic gốc từ ThucHienLuuTam, dùng foreach)
        /// </summary>
        /// <returns>MaDH mới, hoặc -1 nếu lỗi</returns>
        public int LuuDonHangTam(List<ChiTietGioHang> gioHang, decimal tongTien, int maNV, int? maKH) {
            try {
                // Dùng 1 context cho tất cả thao tác, y như code gốc
                using (DataSqlContext db = new DataSqlContext()) {
                    // Bước 1: Tạo DonHang
                    var donHangMoi = new DonHang {
                        NgayLap = DateTime.Now,
                        MaNv = maNV,
                        TrangThai = "Đang xử lý",
                        TongTien = tongTien,
                        MaKh = maKH
                    };

                    // Bước 2: Tạo List ChiTietDonHang (dùng navigation y như gốc)
                    var listChiTiet = new List<ChiTietDonHang>();
                    foreach (var item in gioHang) {
                        var chiTiet = new ChiTietDonHang {
                            MaDhNavigation = donHangMoi, // Gán navigation
                            MaSp = item.MaSP,
                            SoLuong = item.SoLuong,
                            DonGia = item.DonGia
                        };
                        listChiTiet.Add(chiTiet);
                    }

                    // Bước 3: Tạo ThanhToan (dùng navigation y như gốc)
                    var thanhToanMoi = new Models.ThanhToan {
                        MaDhNavigation = donHangMoi, // Gán navigation
                        HinhThuc = null,
                        SoTien = tongTien,
                        TrangThai = "Chưa thanh toán"
                    };

                    // Bước 4: Add vào DbContext
                    db.DonHangs.Add(donHangMoi);
                    db.ChiTietDonHangs.AddRange(listChiTiet);
                    db.ThanhToans.Add(thanhToanMoi);

                    // Bước 5: Trừ kho (viết lại bằng foreach)
                    foreach (var monAn in listChiTiet) {
                        // Lấy công thức (thay .Where)
                        var congThuc_raw = db.DinhLuongs.ToList();
                        var congThuc_filter = new List<DinhLuong>();
                        foreach (var dl in congThuc_raw) {
                            if (dl.MaSp == monAn.MaSp) {
                                congThuc_filter.Add(dl);
                            }
                        }

                        if (congThuc_filter.Count > 0) {
                            foreach (var nguyenLieuCan in congThuc_filter) {
                                // Lấy NL trong kho (thay .FirstOrDefault)
                                var nguyenLieuKho_raw = db.NguyenLieus.ToList();
                                NguyenLieu nguyenLieuTrongKho = null;
                                foreach (var nl in nguyenLieuKho_raw) {
                                    if (nl.MaNl == nguyenLieuCan.MaNl) {
                                        nguyenLieuTrongKho = nl;
                                        break; // Tìm thấy
                                    }
                                }

                                if (nguyenLieuTrongKho != null) {
                                    decimal luongCanTru = nguyenLieuCan.SoLuongCan * monAn.SoLuong;
                                    nguyenLieuTrongKho.SoLuongTon -= luongCanTru;
                                    db.Update(nguyenLieuTrongKho);
                                }
                            }
                        }
                    }

                    // Bước 6: Lưu CSDL (chỉ 1 lần ở cuối, y như gốc)
                    db.SaveChanges();

                    // Bước 7: Trả về MaDH mới
                    return donHangMoi.MaDh;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi lưu tạm đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
                return -1; // Báo lỗi
            }
        }
    }
}