using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
// Thêm global:: để tránh lỗi

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

                    foreach (var sp in tatCaSanPham) {
                        if (sp.LoaiSp != null && sp.LoaiSp != "") {
                            cacLoaiSPTam.Add(sp.LoaiSp);
                        }
                    }

                    var ketQua = new List<string>();
                    foreach (var loai in cacLoaiSPTam) {
                        if (!ketQua.Contains(loai)) {
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
                    var allDinhLuong = db.DinhLuongs.ToList();

                    var tatCaSanPham_filter = new List<SanPham>();
                    var allNguyenLieu_filter = new List<NguyenLieu>();

                    foreach (var sp in tatCaSanPham_raw) {
                        if (sp.TrangThai == "Còn bán") {
                            tatCaSanPham_filter.Add(sp);
                        }
                    }

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

                    foreach (var kh in tatCaKhachHang) {
                        if (kh.SoDienThoai == sdt) {
                            return kh;
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
                using (DataSqlContext db = new DataSqlContext()) {
                    using (var transaction = db.Database.BeginTransaction()) {
                        try {
                            // Bước 1: Tạo DonHang
                            var donHangMoi = new DonHang {
                                NgayLap = DateTime.Now,
                                MaNv = maNV,
                                TrangThai = "Đang xử lý",
                                TongTien = tongTien,
                                MaKh = maKH
                            };

                            // Bước 2: Tạo List ChiTietDonHang
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

                            // Bước 3: Tạo ThanhToan
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
                            var allCongThuc = db.DinhLuongs.ToList();
                            var allNguyenLieu = db.NguyenLieus.ToList(); // Tải 1 lần

                            foreach (var monAn in listChiTiet) {
                                var congThuc_filter = new List<DinhLuong>();
                                foreach (var dl in allCongThuc) {
                                    if (dl.MaSp == monAn.MaSp) {
                                        congThuc_filter.Add(dl);
                                    }
                                }

                                if (congThuc_filter.Count > 0) {
                                    foreach (var nguyenLieuCan in congThuc_filter) {
                                        NguyenLieu nguyenLieuTrongKho = null;
                                        foreach (var nl in allNguyenLieu) // Tìm trong list đã tải
                                        {
                                            if (nl.MaNl == nguyenLieuCan.MaNl) {
                                                nguyenLieuTrongKho = nl;
                                                break;
                                            }
                                        }

                                        if (nguyenLieuTrongKho != null) {
                                            decimal luongCanTru = nguyenLieuCan.SoLuongCan * monAn.SoLuong;
                                            nguyenLieuTrongKho.SoLuongTon -= luongCanTru;
                                            db.Update(nguyenLieuTrongKho); // Đánh dấu đã sửa
                                        }
                                    }
                                }
                            }

                            // Bước 6: Lưu CSDL
                            db.SaveChanges();
                            transaction.Commit(); // Hoàn tất giao dịch

                            // Bước 7: Trả về MaDH mới
                            return donHangMoi.MaDh;
                        }
                        catch (Exception ex_inner) {
                            Console.WriteLine("Lỗi trong transaction LuuDonHangTam: " + ex_inner.Message);
                            transaction.Rollback(); // Hoàn tác
                            return -1;
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi lưu tạm đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
                return -1; // Báo lỗi
            }
        }

        /// <summary>
        /// Thêm một khách hàng mới vào CSDL.
        /// Trả về true nếu thành công, false nếu thất bại.
        /// </summary>
        public bool ThemKhachHangMoi(KhachHang khachHangMoi) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    db.KhachHangs.Add(khachHangMoi);
                    db.SaveChanges(); // Lưu vào CSDL
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi lưu khách hàng: " + ex.InnerException?.Message ?? ex.Message);
                return false;
            }
        }
    }
}