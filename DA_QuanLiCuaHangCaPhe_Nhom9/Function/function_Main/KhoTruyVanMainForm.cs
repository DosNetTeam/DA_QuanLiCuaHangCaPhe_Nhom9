using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main {

    /// Container trả về nhiều danh sách cần cho MainForm

    public class DuLieuSanPham {
        public List<SanPham> TatCaSanPham { get; set; }    // Sản phẩm đang kinh doanh
        public List<DinhLuong> AllDinhLuong { get; set; } // Toàn bộ công thức
        public List<NguyenLieu> AllNguyenLieu { get; set; } // Nguyên liệu đang kinh doanh
    }


    /// DTO chuyển chi tiết giỏ hàng từ UI xuống repository để lưu

    public class ChiTietGioHang {
        public int MaSP { get; set; }   // mã sản phẩm
        public int SoLuong { get; set; } // số lượng
        public decimal DonGia { get; set; } // giá gốc lưu vào CSDL
    }


    /// Lớp chịu trách nhiệm truy vấn CSDL cho MainForm (tách repository khỏi UI).
    /// Tất cả truy vấn viết bằng foreach để tránh LINQ ở đây.

    public class KhoTruyVanMainForm {


        /// Lấy danh sách tên các loại sản phẩm hiện có trong DB
        /// - Tải toàn bộ SanPhams
        /// - Thu thập LoaiSp khác null/empty
        /// - Loại bỏ trùng lặp bằng Contains

        public List<string> TaiLoaiSanPham() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var tatCaSanPham = db.SanPhams.ToList(); // load toàn bộ sản phẩm
                    var cacLoaiSPTam = new List<string>();

                    // Thu thập các LoaiSp có giá trị
                    foreach (var sp in tatCaSanPham) {
                        if (sp.LoaiSp != null && sp.LoaiSp != "") {
                            cacLoaiSPTam.Add(sp.LoaiSp);
                        }
                    }

                    // Loại bỏ trùng lặp thủ công
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
                // Log lỗi và trả list rỗng để UI không crash
                Console.WriteLine("Lỗi khi tải loại sản phẩm: " + ex.InnerException?.Message ?? ex.Message);
                return new List<string>();
            }
        }


        /// Lấy dữ liệu thô cần thiết để hiển thị nút sản phẩm trên MainForm
        /// - TatCaSanPham: những SP có TrangThai == "Còn bán"
        /// - AllDinhLuong: tất cả DinhLuongs (công thức) dùng kiểm kho
        /// - AllNguyenLieu: nguyên liệu có TrangThai == "Đang kinh doanh"

        public DuLieuSanPham LayDuLieuSanPham() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Tải toàn bộ dữ liệu cần cho việc build UI và kiểm kho
                    var tatCaSanPham_raw = db.SanPhams.ToList();
                    var tatCaNguyenLieu_raw = db.NguyenLieus.ToList();
                    var allDinhLuong = db.DinhLuongs.ToList();

                    var tatCaSanPham_filter = new List<SanPham>();
                    var allNguyenLieu_filter = new List<NguyenLieu>();

                    // Lọc chỉ những SP đang kinh doanh
                    foreach (var sp in tatCaSanPham_raw) {
                        if (sp.TrangThai == "Còn bán") {
                            tatCaSanPham_filter.Add(sp);
                        }
                    }

                    // Lọc nguyên liệu đang kinh doanh
                    foreach (var nl in tatCaNguyenLieu_raw) {
                        if (nl.TrangThai == "Đang kinh doanh") {
                            allNguyenLieu_filter.Add(nl);
                        }
                    }

                    // Trả về container chứa 3 danh sách để MainForm dùng
                    return new DuLieuSanPham {
                        TatCaSanPham = tatCaSanPham_filter,
                        AllDinhLuong = allDinhLuong,
                        AllNguyenLieu = allNguyenLieu_filter
                    };
                }
            }
            catch (Exception ex) {
                // Nếu lỗi, trả về container rỗng để UI xử lý an toàn
                Console.WriteLine("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message);
                return new DuLieuSanPham {
                    TatCaSanPham = new List<SanPham>(),
                    AllDinhLuong = new List<DinhLuong>(),
                    AllNguyenLieu = new List<NguyenLieu>()
                };
            }
        }


        /// Tìm KhachHang theo SĐT
        /// - Tải toàn bộ KhachHangs và so sánh SĐT bằng foreach
        /// - Trả về entity KhachHang nếu tìm thấy, null nếu không

        public KhachHang SearchKhachHangBySDT(string sdt) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var tatCaKhachHang = db.KhachHangs.ToList();

                    foreach (var kh in tatCaKhachHang) {
                        if (kh.SoDienThoai == sdt) {
                            return kh; // trả về khi tìm thấy
                        }
                    }
                    return null; // không tìm thấy
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tìm khách hàng: " + ex.Message);
                return null;
            }
        }


        /// Lấy khuyến mãi loại HoaDon áp dụng hiện tại có GiaTri lớn nhất
        /// - Dùng DateOnly để so sánh ngày
        /// - Nếu không có KM phù hợp -> trả về null

        public KhuyenMai LayKhuyenMaiHoaDon() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                    var allKM = db.KhuyenMais.ToList();
                    KhuyenMai kmHoaDon = null;

                    // Duyệt qua tất cả khuyến mãi, chọn KM phù hợp và có giá trị lớn nhất
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
                    return kmHoaDon;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi lấy KM hóa đơn: " + ex.Message);
                return null;
            }
        }


        /// Lưu đơn hàng tạm:
        /// - Tạo DonHang (TrangThai = "Đang xử lý")
        /// - Tạo ChiTietDonHang, ThanhToan (Chưa thanh toán)
        /// - Trừ kho nguyên liệu theo DinhLuongs
        /// - Lưu trong transaction để đảm bảo toàn vẹn (rollback khi lỗi)
        /// Trả về MaDh mới nếu thành công, -1 nếu lỗi.

        public int LuuDonHangTam(List<ChiTietGioHang> gioHang, decimal tongTien, int maNV, int? maKH) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    using (var transaction = db.Database.BeginTransaction()) {
                        try {
                            // Bước 1: Khởi tạo DonHang mới với thông tin cơ bản
                            var donHangMoi = new DonHang {
                                NgayLap = DateTime.Now,
                                MaNv = maNV,
                                TrangThai = "Đang xử lý",
                                TongTien = tongTien,
                                MaKh = maKH
                            };

                            // Bước 2: Chuyển chi tiết giỏ hàng sang ChiTietDonHang entity, gán MaDhNavigation để EF hiểu mối quan hệ
                            var listChiTiet = new List<ChiTietDonHang>();
                            foreach (var item in gioHang) {
                                var chiTiet = new ChiTietDonHang {
                                    MaDhNavigation = donHangMoi, // navigation object
                                    MaSp = item.MaSP,
                                    SoLuong = item.SoLuong,
                                    DonGia = item.DonGia
                                };
                                listChiTiet.Add(chiTiet);
                            }

                            // Bước 3: Tạo record ThanhToan liên quan (chưa thanh toán)
                            var thanhToanMoi = new Models.ThanhToan {
                                MaDhNavigation = donHangMoi,
                                HinhThuc = null,
                                SoTien = tongTien,
                                TrangThai = "Chưa thanh toán"
                            };

                            // Bước 4: Đăng ký các entity vào DbContext
                            db.DonHangs.Add(donHangMoi);
                            db.ChiTietDonHangs.AddRange(listChiTiet);
                            db.ThanhToans.Add(thanhToanMoi);

                            // Bước 5: Trừ kho nguyên liệu
                            var allCongThuc = db.DinhLuongs.ToList();  // tải công thức 1 lần
                            var allNguyenLieu = db.NguyenLieus.ToList(); // tải nguyên liệu 1 lần

                            foreach (var monAn in listChiTiet) {
                                // Tìm công thức của món (Danh sách DinhLuong có MaSp = monAn.MaSp)
                                var congThuc_filter = new List<DinhLuong>();
                                foreach (var dl in allCongThuc) {
                                    if (dl.MaSp == monAn.MaSp) {
                                        congThuc_filter.Add(dl);
                                    }
                                }

                                if (congThuc_filter.Count > 0) {
                                    // Với mỗi dòng công thức, tìm nguyên liệu tương ứng và trừ SoLuongTon
                                    foreach (var nguyenLieuCan in congThuc_filter) {
                                        NguyenLieu nguyenLieuTrongKho = null;
                                        foreach (var nl in allNguyenLieu) {
                                            if (nl.MaNl == nguyenLieuCan.MaNl) {
                                                nguyenLieuTrongKho = nl;
                                                break;
                                            }
                                        }

                                        if (nguyenLieuTrongKho != null) {
                                            // Lượng cần trừ = SoLuongCan (của công thức) * số lượng món trong đơn
                                            decimal luongCanTru = nguyenLieuCan.SoLuongCan * monAn.SoLuong;
                                            nguyenLieuTrongKho.SoLuongTon -= luongCanTru;
                                            db.Update(nguyenLieuTrongKho); // mark as modified
                                        }
                                    }
                                }
                            }

                            // Bước 6: Lưu thay đổi và commit transaction
                            db.SaveChanges();
                            transaction.Commit();

                            // Bước 7: Trả về MaDh do EF gán sau SaveChanges
                            return donHangMoi.MaDh;
                        }
                        catch (Exception ex_inner) {
                            // Nếu xảy ra lỗi trong transaction -> rollback và trả -1
                            Console.WriteLine("Lỗi trong transaction LuuDonHangTam: " + ex_inner.Message);
                            transaction.Rollback();
                            return -1;
                        }
                    }
                }
            }
            catch (Exception ex) {
                // Lỗi không mong muốn khi mở DB/transaction -> log và trả -1
                Console.WriteLine("Lỗi khi lưu tạm đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
                return -1;
            }
        }


        /// Thêm một khách hàng mới vào CSDL.
        /// Trả về true nếu thành công, false nếu thất bại.

        public bool ThemKhachHangMoi(KhachHang khachHangMoi) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    db.KhachHangs.Add(khachHangMoi); // thêm entity mới
                    db.SaveChanges(); // lưu vào DB
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