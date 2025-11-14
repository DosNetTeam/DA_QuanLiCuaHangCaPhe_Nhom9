using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main {
    /// <summary>
    /// Đại diện cho một món hàng trong giỏ.
    /// Chỉ lưu trữ dữ liệu, không có logic.
    /// </summary>
    public class GioHangItem {
        public int MaSp { get; set; }
        public string TenSp { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGiaGoc { get; set; } // Giá gốc (chưa KM)
        public decimal ThanhTienGoc { get; set; } // SoLuong * DonGiaGoc
    }

    /// <summary>
    /// Quản lý logic nghiệp vụ của giỏ hàng.
    /// Không tương tác trực tiếp với UI (ListView).
    /// </summary>
    public class GioHang {
        // Danh sách các món đang có trong giỏ
        private List<GioHangItem> _items;

        // Cần Dịch Vụ Đơn Hàng để kiểm tra kho
        private readonly DichVuDonHang _dichVuDonHang;

        public GioHang(DichVuDonHang dichVu) {
            _items = new List<GioHangItem>();
            _dichVuDonHang = dichVu; // Nhận dịch vụ từ MainForm
        }

        /// <summary>
        /// Thêm một sản phẩm vào giỏ hoặc tăng số lượng.
        /// </summary>
        /// <returns>Một tuple (bool Success, string Message). 
        /// True nếu thành công, False và kèm thông báo lỗi nếu thất bại.
        /// </returns>
        public (bool Success, string Message) ThemMon(SanPham sp) {
            // Kiểm tra xem món đã có trong giỏ chưa
            GioHangItem itemCoSan = null;
            foreach (var item in _items) {
                if (item.MaSp == sp.MaSp) {
                    itemCoSan = item;
                    break;
                }
            }

            if (itemCoSan != null) {
                // ----- ĐÃ CÓ, TĂNG SỐ LƯỢNG -----
                int soLuongMoi = itemCoSan.SoLuong + 1;

                // Kiểm tra kho
                var kiemTra = _dichVuDonHang.KiemTraSoLuongTonThucTe(sp.MaSp, soLuongMoi);
                if (kiemTra.DuHang == false) {
                    return (false, kiemTra.ThongBao); // Thất bại, trả về lỗi
                }

                // Cập nhật lại số lượng và thành tiền
                itemCoSan.SoLuong = soLuongMoi;
                itemCoSan.ThanhTienGoc = soLuongMoi * itemCoSan.DonGiaGoc;
            }
            else {
                // ----- CHƯA CÓ, THÊM MỚI -----
                // Kiểm tra kho cho 1 món
                var kiemTra = _dichVuDonHang.KiemTraSoLuongTonThucTe(sp.MaSp, 1);
                if (kiemTra.DuHang == false) {
                    return (false, kiemTra.ThongBao); // Thất bại, trả về lỗi
                }

                // Tạo món mới
                _items.Add(new GioHangItem {
                    MaSp = sp.MaSp,
                    TenSp = sp.TenSp,
                    SoLuong = 1,
                    DonGiaGoc = sp.DonGia, // Lưu giá gốc
                    ThanhTienGoc = sp.DonGia
                });
            }

            return (true, "OK"); // Thành công
        }

        /// <summary>
        /// Giảm số lượng của một món.
        /// </summary>
        /// <returns>True nếu món bị xóa (SL=0), False nếu chỉ giảm.</returns>
        public bool GiamSoLuong(int maSp) {
            GioHangItem itemCanGiam = null;
            foreach (var item in _items) {
                if (item.MaSp == maSp) {
                    itemCanGiam = item;
                    break;
                }
            }

            if (itemCanGiam == null) return false;

            if (itemCanGiam.SoLuong > 1) {
                // Giảm số lượng
                itemCanGiam.SoLuong--;
                itemCanGiam.ThanhTienGoc = itemCanGiam.SoLuong * itemCanGiam.DonGiaGoc;
                return false; // Chỉ giảm
            }
            else {
                // Xóa món
                _items.Remove(itemCanGiam);
                return true; // Đã xóa
            }
        }

        /// <summary>
        /// Xóa hoàn toàn một món khỏi giỏ, bất kể số lượng.
        /// </summary>
        public void XoaMon(int maSp) {
            GioHangItem itemCanXoa = null;
            foreach (var item in _items) {
                if (item.MaSp == maSp) {
                    itemCanXoa = item;
                    break;
                }
            }

            if (itemCanXoa != null) {
                _items.Remove(itemCanXoa);
            }
        }

        /// <summary>
        /// Xóa sạch giỏ hàng.
        /// </summary>
        public void XoaTatCa() {
            _items.Clear();
        }

        /// <summary>
        /// Lấy danh sách tất cả các món trong giỏ để hiển thị.
        /// </summary>
        public List<GioHangItem> LayTatCaMon() {
            return _items;
        }

        /// <summary>
        /// Lấy tổng tiền (chưa tính KM).
        /// </summary>
        public decimal LayTongTienGoc() {
            decimal tong = 0;
            foreach (var item in _items) {
                tong += item.ThanhTienGoc;
            }
            return tong;
        }

        /// <summary>
        /// Đếm số lượng món trong giỏ.
        /// </summary>
        public int LaySoLuongMon() {
            return _items.Count;
        }
    }
}