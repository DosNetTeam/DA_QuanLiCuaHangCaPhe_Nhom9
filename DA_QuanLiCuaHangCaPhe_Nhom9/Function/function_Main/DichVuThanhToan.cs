using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main {
    // DTO trả về toàn bộ dữ liệu cần cho form ThanhToan
    public class ThongTinDonHangDayDu {
        public DonHang DonHang { get; set; }                // entity DonHang gốc
        public List<ChiTietDonHang> ChiTiet { get; set; }   // danh sách chi tiết đơn
        public Models.ThanhToan ThanhToan { get; set; }     // record ThanhToan liên quan
        public List<SanPham> SanPhams { get; set; }         // toàn bộ sản phẩm để join tên
    }

    // DTO dùng để hiển thị preview (không thay đổi DB)
    public class DuLieuHoaDonPreview {
        public string TenMon { get; set; }
        public string SoLuong { get; set; }
        public string DonGia { get; set; }
        public string ThanhTien { get; set; }
    }

    // DTO cho danh sách DonHang "Đang xử lý"
    public class DuLieuDonHangCho {
        public int MaDh { get; set; }
        public string TenKH { get; set; }
        public DateTime? NgayLap { get; set; }
        public decimal TongTien { get; set; }
    }


    /// Lớp này chịu trách nhiệm truy vấn CSDL cho ThanhToan và ChonDonHangCho.
    /// Tất cả truy vấn viết bằng foreach để tránh LINQ ở chỗ này.

    public class DichVuThanhToan {
        // --- LOGIC CHO ThanhToan.cs ---

        /// Tải tất cả thông tin cần thiết cho Form ThanhToan
        /// - Tìm DonHang theo maDonHangChon
        /// - Tìm các ChiTietDonHang của đơn đó
        /// - Lấy record ThanhToan có trạng thái "Chưa thanh toán"
        /// - Lấy danh sách tất cả SanPham để map tên khi hiển thị
        /// Trả về null nếu không tìm thấy DonHang hoặc ThanhToan chưa thanh toán.

        public ThongTinDonHangDayDu TaiThongTinThanhToan(int maDonHangChon) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var ketQua = new ThongTinDonHangDayDu();
                    ketQua.ChiTiet = new List<ChiTietDonHang>();

                    // 1. Tải DonHang: lặp qua db.DonHangs.ToList() và so sánh MaDh
                    foreach (var dh in db.DonHangs.ToList()) {
                        if (dh.MaDh == maDonHangChon) {
                            ketQua.DonHang = dh;
                            break; // khi tìm được thoát vòng
                        }
                    }

                    // 2. Tải Chi Tiết Đơn Hàng: lặp toàn bộ ChiTietDonHangs và chọn những mục có MaDh tương ứng
                    foreach (var ct in db.ChiTietDonHangs.ToList()) {
                        if (ct.MaDh == maDonHangChon) {
                            ketQua.ChiTiet.Add(ct);
                        }
                    }

                    // 3. Tải ThanhToan: tìm record ThanhToan có MaDh tương ứng và TrangThai = "Chưa thanh toán"
                    foreach (var tt in db.ThanhToans.ToList()) {
                        if (tt.MaDh == maDonHangChon && tt.TrangThai == "Chưa thanh toán") {
                            ketQua.ThanhToan = tt;
                            break;
                        }
                    }

                    // 4. Lấy danh sách SanPham để phục vụ join tên khi hiển thị chi tiết hóa đơn
                    ketQua.SanPhams = db.SanPhams.ToList();

                    // Nếu thiếu DonHang hoặc ThanhToan chưa thanh toán -> trả null để caller xử lý
                    if (ketQua.DonHang == null || ketQua.ThanhToan == null) {
                        return null;
                    }
                    return ketQua;
                }
            }
            catch (Exception ex) {
                // Log lỗi ra console — không ném để UI bắt và hiển thị message phù hợp
                Console.WriteLine("Lỗi khi tải thông tin thanh toán: " + ex.Message);
                return null;
            }
        }


        /// Xác nhận thanh toán:
        /// - Tìm DonHang và ThanhToan đang chờ
        /// - Cập nhật trạng thái thành "Đã thanh toán"
        /// - Ghi HinhThuc (Tiền mặt / QR)
        /// - Lưu thay đổi và trả về true/false

        public bool XacNhanThanhToan(int maDonHang, string hinhThuc) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Tìm DonHang bằng foreach trên DbSet
                    DonHang donHang = null;
                    foreach (var dh in db.DonHangs) {
                        if (dh.MaDh == maDonHang) { donHang = dh; break; }
                    }

                    // Tìm ThanhToan có trạng thái "Chưa thanh toán"
                    Models.ThanhToan thanhToan = null;
                    foreach (var tt in db.ThanhToans) {
                        if (tt.MaDh == maDonHang && tt.TrangThai == "Chưa thanh toán") {
                            thanhToan = tt; break;
                        }
                    }

                    // Nếu không tìm thấy -> trả false để caller biết
                    if (donHang == null || thanhToan == null) {
                        return false;
                    }

                    // Cập nhật trạng thái và phương thức thanh toán
                    donHang.TrangThai = "Đã thanh toán";
                    thanhToan.TrangThai = "Đã thanh toán";
                    thanhToan.HinhThuc = hinhThuc;

                    // Lưu thay đổi vào DB
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi xác nhận thanh toán: " + ex.Message);
                return false;
            }
        }

        // --- LOGIC CHO CHONDONHANGCHO.CS ---


        /// Tải danh sách DonHang đang ở trạng thái "Đang xử lý"
        /// - Lấy all DonHang, lọc theo TrangThai
        /// - Map MaKh -> TenKH bằng cách join thủ công với KhachHangs
        /// - Trả về list DuLieuDonHangCho đã format để hiển thị

        public List<DuLieuDonHangCho> TaiDanhSachDonHangCho() {
            var ketQua = new List<DuLieuDonHangCho>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var allDonHang = db.DonHangs.ToList();   // tải toàn bộ DonHang
                    var allKhachHang = db.KhachHangs.ToList(); // tải toàn bộ KhachHang

                    var donHangCho = new List<DonHang>();

                    // Lọc DonHang có TrangThai == "Đang xử lý"
                    foreach (var dh in allDonHang) {
                        if (dh.TrangThai == "Đang xử lý") {
                            donHangCho.Add(dh);
                        }
                    }

                    // Sắp theo NgayLap tăng dần (dùng Sort + CompareTo)
                    donHangCho.Sort((a, b) => a.NgayLap.GetValueOrDefault().CompareTo(b.NgayLap.GetValueOrDefault()));

                    // Map từng DonHang -> DuLieuDonHangCho, tìm tên Kh nếu có MaKh
                    foreach (var dh in donHangCho) {
                        string tenKH = "Khách vãng lai";
                        if (dh.MaKh != null) {
                            foreach (var kh in allKhachHang) {
                                if (kh.MaKh == dh.MaKh) {
                                    tenKH = kh.TenKh;
                                    break;
                                }
                            }
                        }

                        ketQua.Add(new DuLieuDonHangCho {
                            MaDh = dh.MaDh,
                            TenKH = tenKH,
                            NgayLap = dh.NgayLap,
                            TongTien = dh.TongTien ?? 0
                        });
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải danh sách đơn chờ: " + ex.Message);
            }
            return ketQua;
        }


        /// Lấy chi tiết DonHang gốc (cần khi tính lại tiền gốc hoặc hiển thị chi tiết)
        /// - Tìm DonHang theo maDH
        /// - Đính kèm ChiTietDonHangs của đơn đó

        public DonHang LayChiTietDonHangGoc(int maDH) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    DonHang donHang = null;
                    // Tìm DonHang bằng foreach
                    foreach (var dh in db.DonHangs.ToList()) {
                        if (dh.MaDh == maDH) {
                            donHang = dh;
                            // Khởi tạo danh sách chi tiết và điền bằng cách lặp qua ChiTietDonHangs
                            donHang.ChiTietDonHangs = new List<ChiTietDonHang>();
                            foreach (var ct in db.ChiTietDonHangs.ToList()) {
                                if (ct.MaDh == maDH) {
                                    donHang.ChiTietDonHangs.Add(ct);
                                }
                            }
                            return donHang; // trả về khi đã lắp chi tiết
                        }
                    }
                    return null; // nếu không tìm thấy
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải chi tiết đơn hàng: " + ex.Message);
                return null;
            }
        }


        /// Hủy đơn hàng chờ và hoàn kho nguyên liệu:
        /// - Tìm DonHang và ThanhToan liên quan
        /// - Lấy chi tiết đơn, tính lượng nguyên liệu cần hoàn trả theo DinhLuongs
        /// - Cộng lại SoLuongTon của các NguyenLieu tương ứng
        /// - Cập nhật trạng thái DonHang/ThanhToan = "Đã huỷ"
        /// - Toàn bộ trong transaction; rollback khi có lỗi

        public bool HuyDonHangCho(int maDHCanHuy) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    using (var transaction = db.Database.BeginTransaction()) {
                        try {
                            // Tìm DonHang
                            DonHang donHang = null;
                            foreach (var dh in db.DonHangs) {
                                if (dh.MaDh == maDHCanHuy) { donHang = dh; break; }
                            }

                            // Tìm ThanhToan đang ở trạng thái "Chưa thanh toán"
                            Models.ThanhToan thanhToan = null;
                            foreach (var tt in db.ThanhToans) {
                                if (tt.MaDh == maDHCanHuy && tt.TrangThai == "Chưa thanh toán") {
                                    thanhToan = tt; break;
                                }
                            }

                            // Lấy chi tiết đơn để dùng khi hoàn kho
                            var chiTiet = new List<ChiTietDonHang>();
                            foreach (var ct in db.ChiTietDonHangs.ToList()) {
                                if (ct.MaDh == maDHCanHuy) { chiTiet.Add(ct); }
                            }

                            // Nếu thiếu DonHang hoặc ThanhToan -> rollback và trả false
                            if (donHang == null || thanhToan == null) {
                                transaction.Rollback();
                                return false;
                            }

                            // --- HOÀN KHO: tải toàn bộ công thức và nguyên liệu 1 lần để xử lý bằng foreach ---
                            var allCongThuc = db.DinhLuongs.ToList();
                            var allNguyenLieu = db.NguyenLieus.ToList();

                            // Với từng món trong chi tiết, tìm công thức (DinhLuong) tương ứng và cộng trả nguyên liệu
                            foreach (var monAn in chiTiet) {
                                var congThucCuaMon = new List<DinhLuong>();
                                foreach (var dl in allCongThuc) {
                                    if (dl.MaSp == monAn.MaSp) { congThucCuaMon.Add(dl); }
                                }

                                if (congThucCuaMon.Count > 0) {
                                    foreach (var nguyenLieuCan in congThucCuaMon) {
                                        NguyenLieu nguyenLieuTrongKho = null;
                                        foreach (var nl in allNguyenLieu) {
                                            if (nl.MaNl == nguyenLieuCan.MaNl) {
                                                nguyenLieuTrongKho = nl;
                                                break;
                                            }
                                        }

                                        if (nguyenLieuTrongKho != null) {
                                            // Lượng cần cộng = SoLuongCan (của công thức) * số lượng món trong đơn
                                            decimal luongCanCong = nguyenLieuCan.SoLuongCan * monAn.SoLuong;
                                            nguyenLieuTrongKho.SoLuongTon += luongCanCong; // cộng trả
                                            db.Update(nguyenLieuTrongKho); // đánh dấu thay đổi
                                        }
                                    }
                                }
                            }

                            // --- CẬP NHẬT TRẠNG THÁI ĐƠN ---
                            donHang.TrangThai = "Đã huỷ";
                            thanhToan.TrangThai = "Đã huỷ";
                            db.Update(donHang);
                            db.Update(thanhToan);

                            // Lưu và commit transaction
                            db.SaveChanges();
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex_inner) {
                            // Log lỗi inner, rollback và trả false
                            Console.WriteLine("Lỗi trong transaction: " + ex_inner.Message);
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi nghiêm trọng khi hủy đơn hàng: " + ex.Message);
                return false;
            }
        }
    }
}