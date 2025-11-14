using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
// Thêm global:: để tránh lỗi

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main {

    /// Lớp này dùng để trả về nhiều danh sách
    /// từ hàm LayDuLieuSanPham()

    public class DuLieuSanPham {
        public List<SanPham> TatCaSanPham { get; set; }
        public List<DinhLuong> AllDinhLuong { get; set; }
        public List<NguyenLieu> AllNguyenLieu { get; set; }
    }


    /// Dùng để chuyển thông tin giỏ hàng từ
    /// MainForm sang KhoTruyVan

    public class ChiTietGioHang {
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }



    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho các nhu cầu của MainForm.
    /// (ĐÃ VIẾT LẠI BẰNG FOREACH)

    public class KhoTruyVanMainForm {

        /// Lấy danh sách tên các Loại Sản Phẩm (dùng foreach)

        public List<string> TaiLoaiSanPham() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Lấy tất cả sản phẩm từ DB (underlying ToList tải ra bộ nhớ)
                    var tatCaSanPham = db.SanPhams.ToList();
                    var cacLoaiSPTam = new List<string>();

                    // Thu thập các LoaiSp không null/empty
                    foreach (var sp in tatCaSanPham) {
                        if (sp.LoaiSp != null && sp.LoaiSp != "") {
                            cacLoaiSPTam.Add(sp.LoaiSp);
                        }
                    }

                    // Loại bỏ trùng lặp theo cách thủ công
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


        /// Lấy tất cả dữ liệu thô cần thiết để hiển thị sản phẩm (dùng foreach)

        public DuLieuSanPham LayDuLieuSanPham() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Tải toàn bộ bảng cần thiết một lần để giảm số lần truy vấn
                    var tatCaSanPham_raw = db.SanPhams.ToList();
                    var tatCaNguyenLieu_raw = db.NguyenLieus.ToList();
                    var allDinhLuong = db.DinhLuongs.ToList();

                    // Lọc sản phẩm đang kinh doanh
                    var tatCaSanPham_filter = new List<SanPham>();
                    var allNguyenLieu_filter = new List<NguyenLieu>();

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


        /// Tìm một khách hàng bằng SĐT (dùng foreach)

        public KhachHang SearchKhachHangBySDT(string sdt) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var tatCaKhachHang = db.KhachHangs.ToList();

                    // Duyệt từng khách hàng và so sánh số điện thoại
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


        /// Lấy KM Hóa Đơn tốt nhất (Logic foreach gốc của bạn)

        public KhuyenMai LayKhuyenMaiHoaDon() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                    var allKM = db.KhuyenMais.ToList();
                    KhuyenMai kmHoaDon = null;

                    // Duyệt tất cả khuyến mãi, chọn KM loại HoaDon đang áp dụng và có giá trị lớn nhất
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


        /// Lưu đơn hàng tạm, trừ kho (Logic gốc từ ThucHienLuuTam, dùng foreach)

        /// <returns>MaDH mới, hoặc -1 nếu lỗi</returns>
        public int LuuDonHangTam(List<ChiTietGioHang> gioHang, decimal tongTien, int maNV, int? maKH) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    using (var transaction = db.Database.BeginTransaction()) {
                        try {
                            // Bước 1: Tạo DonHang mới (chưa lưu vào DB cho đến SaveChanges)
                            var donHangMoi = new DonHang {
                                NgayLap = DateTime.Now,
                                MaNv = maNV,
                                TrangThai = "Đang xử lý",
                                TongTien = tongTien,
                                MaKh = maKH
                            };

                            // Bước 2: Tạo danh sách ChiTietDonHang dựa trên input gioHang
                            var listChiTiet = new List<ChiTietDonHang>();
                            foreach (var item in gioHang) {
                                var chiTiet = new ChiTietDonHang {
                                    MaDhNavigation = donHangMoi, // Gắn navigation để EF hiểu quan hệ
                                    MaSp = item.MaSP,
                                    SoLuong = item.SoLuong,
                                    DonGia = item.DonGia
                                };
                                listChiTiet.Add(chiTiet);
                            }

                            // Bước 3: Tạo record ThanhToan liên quan (chưa thanh toán)
                            var thanhToanMoi = new Models.ThanhToan {
                                MaDhNavigation = donHangMoi, // Gán navigation
                                HinhThuc = null,
                                SoTien = tongTien,
                                TrangThai = "Chưa thanh toán"
                            };

                            // Bước 4: Add vào DbContext (EF sẽ xử lý insert theo quan hệ)
                            db.DonHangs.Add(donHangMoi);
                            db.ChiTietDonHangs.AddRange(listChiTiet);
                            db.ThanhToans.Add(thanhToanMoi);

                            // Bước 5: Trừ kho: tải công thức và nguyên liệu 1 lần rồi xử lý
                            var allCongThuc = db.DinhLuongs.ToList();
                            var allNguyenLieu = db.NguyenLieus.ToList(); // Tải 1 lần

                            foreach (var monAn in listChiTiet) {
                                var congThuc_filter = new List<DinhLuong>();
                                // Tìm các DinhLuong phù hợp với MaSp
                                foreach (var dl in allCongThuc) {
                                    if (dl.MaSp == monAn.MaSp) {
                                        congThuc_filter.Add(dl);
                                    }
                                }

                                if (congThuc_filter.Count > 0) {
                                    foreach (var nguyenLieuCan in congThuc_filter) {
                                        NguyenLieu nguyenLieuTrongKho = null;
                                        // Tìm nguyên liệu tương ứng trong kho
                                        foreach (var nl in allNguyenLieu) // Tìm trong list đã tải
                                        {
                                            if (nl.MaNl == nguyenLieuCan.MaNl) {
                                                nguyenLieuTrongKho = nl;
                                                break;
                                            }
                                        }

                                        if (nguyenLieuTrongKho != null) {
                                            // Trừ số lượng cần thiết (SoLuongCan * số lượng món)
                                            decimal luongCanTru = nguyenLieuCan.SoLuongCan * monAn.SoLuong;
                                            nguyenLieuTrongKho.SoLuongTon -= luongCanTru;
                                            db.Update(nguyenLieuTrongKho); // Đánh dấu entity đã thay đổi
                                        }
                                    }
                                }
                            }

                            // Bước 6: Lưu CSDL và commit transaction
                            db.SaveChanges();
                            transaction.Commit(); // Hoàn tất giao dịch

                            // Bước 7: Sau SaveChanges, donHangMoi.MaDh được EF gán -> trả về MaDH mới
                            return donHangMoi.MaDh;
                        }
                        catch (Exception ex_inner) {
                            Console.WriteLine("Lỗi trong transaction LuuDonHangTam: " + ex_inner.Message);
                            transaction.Rollback(); // Hoàn tác nếu có lỗi
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


        /// Thêm một khách hàng mới vào CSDL.
        /// Trả về true nếu thành công, false nếu thất bại.

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