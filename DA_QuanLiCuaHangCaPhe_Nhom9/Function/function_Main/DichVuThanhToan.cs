using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main {
    // --- DTO cho ThanhToan_Load ---
    public class ThongTinDonHangDayDu {
        public DonHang DonHang { get; set; }
        public List<ChiTietDonHang> ChiTiet { get; set; }
        public Models.ThanhToan ThanhToan { get; set; }
        public List<SanPham> SanPhams { get; set; } // Danh sách SP để lấy tên
    }

    // --- DTO cho HienThiBillPreview (chỉ dùng trong ThanhToan.cs) ---
    public class DuLieuHoaDonPreview {
        public string TenMon { get; set; }
        public string SoLuong { get; set; }
        public string DonGia { get; set; }
        public string ThanhTien { get; set; }
    }

    // --- DTO cho ChonDonHangCho_Load ---
    public class DuLieuDonHangCho {
        public int MaDh { get; set; }
        public string TenKH { get; set; }
        public DateTime? NgayLap { get; set; }
        public decimal TongTien { get; set; }
    }

    /// <summary>
    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho các chức năng Thanh Toán và Quản lý Đơn Chờ.
    /// (ĐÃ VIẾT LẠI BẰNG FOREACH, KHÔNG LINQ)
    /// </summary>
    public class DichVuThanhToan {
        // --- LOGIC CHO THANHTOAN.CS ---

        /// <summary>
        /// Tải tất cả thông tin cần thiết cho Form ThanhToan
        /// (Viết lại bằng foreach)
        /// </summary>
        public ThongTinDonHangDayDu TaiThongTinThanhToan(int maDonHangChon) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var ketQua = new ThongTinDonHangDayDu();
                    ketQua.ChiTiet = new List<ChiTietDonHang>();

                    // 1. Tải Đơn Hàng
                    foreach (var dh in db.DonHangs.ToList()) {
                        if (dh.MaDh == maDonHangChon) {
                            ketQua.DonHang = dh;
                            break;
                        }
                    }

                    // 2. Tải Chi Tiết Đơn Hàng
                    foreach (var ct in db.ChiTietDonHangs.ToList()) {
                        if (ct.MaDh == maDonHangChon) {
                            ketQua.ChiTiet.Add(ct);
                        }
                    }

                    // 3. Tải Thanh Toán
                    foreach (var tt in db.ThanhToans.ToList()) {
                        if (tt.MaDh == maDonHangChon && tt.TrangThai == "Chưa thanh toán") {
                            ketQua.ThanhToan = tt;
                            break;
                        }
                    }

                    // 4. Tải Sản Phẩm (để lấy tên)
                    ketQua.SanPhams = db.SanPhams.ToList();

                    if (ketQua.DonHang == null || ketQua.ThanhToan == null) {
                        return null; // Lỗi không tìm thấy
                    }
                    return ketQua;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải thông tin thanh toán: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Cập nhật trạng thái DonHang và ThanhToan thành "Đã thanh toán"
        /// </summary>
        public bool XacNhanThanhToan(int maDonHang, string hinhThuc) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    DonHang donHang = null;
                    foreach (var dh in db.DonHangs) {
                        if (dh.MaDh == maDonHang) { donHang = dh; break; }
                    }

                    Models.ThanhToan thanhToan = null;
                    foreach (var tt in db.ThanhToans) {
                        if (tt.MaDh == maDonHang && tt.TrangThai == "Chưa thanh toán") {
                            thanhToan = tt; break;
                        }
                    }

                    if (donHang == null || thanhToan == null) {
                        return false; // Lỗi
                    }

                    donHang.TrangThai = "Đã thanh toán";
                    thanhToan.TrangThai = "Đã thanh toán";
                    thanhToan.HinhThuc = hinhThuc;

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

        /// <summary>
        /// Tải danh sách các đơn hàng đang ở trạng thái "Đang xử lý"
        /// (Viết lại bằng foreach)
        /// </summary>
        public List<DuLieuDonHangCho> TaiDanhSachDonHangCho() {
            var ketQua = new List<DuLieuDonHangCho>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var allDonHang = db.DonHangs.ToList();
                    var allKhachHang = db.KhachHangs.ToList();

                    var donHangCho = new List<DonHang>();

                    foreach (var dh in allDonHang) {
                        if (dh.TrangThai == "Đang xử lý") {
                            donHangCho.Add(dh);
                        }
                    }

                    donHangCho.Sort((a, b) => a.NgayLap.GetValueOrDefault().CompareTo(b.NgayLap.GetValueOrDefault()));

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

        /// <summary>
        /// Lấy thông tin chi tiết của 1 đơn hàng chờ (để tính lại tiền giảm giá)
        /// </summary>
        public DonHang LayChiTietDonHangGoc(int maDH) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    DonHang donHang = null;
                    foreach (var dh in db.DonHangs.ToList()) {
                        if (dh.MaDh == maDH) {
                            donHang = dh;
                            donHang.ChiTietDonHangs = new List<ChiTietDonHang>();
                            foreach (var ct in db.ChiTietDonHangs.ToList()) {
                                if (ct.MaDh == maDH) {
                                    donHang.ChiTietDonHangs.Add(ct);
                                }
                            }
                            return donHang;
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải chi tiết đơn hàng: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Hủy đơn hàng và Hoàn kho nguyên liệu
        /// (Viết lại bằng foreach)
        /// </summary>
        public bool HuyDonHangCho(int maDHCanHuy) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    using (var transaction = db.Database.BeginTransaction()) {
                        try {
                            DonHang donHang = null;
                            foreach (var dh in db.DonHangs) {
                                if (dh.MaDh == maDHCanHuy) { donHang = dh; break; }
                            }

                            Models.ThanhToan thanhToan = null;
                            foreach (var tt in db.ThanhToans) {
                                if (tt.MaDh == maDHCanHuy && tt.TrangThai == "Chưa thanh toán") {
                                    thanhToan = tt; break;
                                }
                            }

                            var chiTiet = new List<ChiTietDonHang>();
                            foreach (var ct in db.ChiTietDonHangs.ToList()) {
                                if (ct.MaDh == maDHCanHuy) { chiTiet.Add(ct); }
                            }

                            if (donHang == null || thanhToan == null) {
                                transaction.Rollback();
                                return false; // Không tìm thấy
                            }

                            // --- HOÀN KHO ---
                            var allCongThuc = db.DinhLuongs.ToList();
                            var allNguyenLieu = db.NguyenLieus.ToList();

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
                                            decimal luongCanCong = nguyenLieuCan.SoLuongCan * monAn.SoLuong;
                                            nguyenLieuTrongKho.SoLuongTon += luongCanCong;
                                            db.Update(nguyenLieuTrongKho);
                                        }
                                    }
                                }
                            }

                            // --- CẬP NHẬT TRẠNG THÁI ---
                            donHang.TrangThai = "Đã huỷ";
                            thanhToan.TrangThai = "Đã huỷ";
                            db.Update(donHang);
                            db.Update(thanhToan);

                            db.SaveChanges();
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex_inner) {
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