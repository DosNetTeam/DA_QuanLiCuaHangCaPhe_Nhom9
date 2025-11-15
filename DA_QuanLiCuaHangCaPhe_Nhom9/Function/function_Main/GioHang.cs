using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main {

    /// Đại diện cho một món hàng trong giỏ.
    /// Chỉ lưu trữ dữ liệu, không có logic.

    public class GioHangItem {
        // Mã sản phẩm (primary key tham chiếu đến bảng SanPham)
        public int MaSp { get; set; }

        // Tên hiển thị của sản phẩm (để dễ render UI)
        public string TenSp { get; set; }

        // Số lượng hiện có trong giỏ
        public int SoLuong { get; set; }

        // Giá gốc của sản phẩm (trước khi áp dụng khuyến mãi)
        public decimal DonGiaGoc { get; set; } // Giá gốc (chưa KM)

        // Thành tiền tính theo DonGiaGoc * SoLuong (lưu để không phải tính lại nhiều lần)
        public decimal ThanhTienGoc { get; set; } // SoLuong * DonGiaGoc
    }


    /// Quản lý logic nghiệp vụ của giỏ hàng.
    /// Không tương tác trực tiếp với UI (ListView).

    public class GioHang {
        // Danh sách các món đang có trong giỏ
        private List<GioHangItem> _items;

        // Dịch vụ dùng để kiểm tra tồn kho / giá bán khi thay đổi giỏ
        private readonly DichVuDonHang _dichVuDonHang;

        // Constructor nhận DichVuDonHang để thực hiện kiểm tra nghiệp vụ
        public GioHang(DichVuDonHang dichVu) {
            _items = new List<GioHangItem>(); // Khởi tạo list rỗng
            _dichVuDonHang = dichVu; // Lưu tham chiếu đến dịch vụ kiểm kho
        }


        /// Thêm một sản phẩm vào giỏ hoặc tăng số lượng.

        /// <returns>Một tuple (bool Success, string Message). 
        /// True nếu thành công, False và kèm thông báo lỗi nếu thất bại.
        /// </returns>
        public (bool Success, string Message) ThemMon(SanPham sp) {
            // Tìm xem đã có item này trong giỏ chưa (so sánh theo MaSp)
            GioHangItem itemCoSan = null;
            foreach (var item in _items) {
                if (item.MaSp == sp.MaSp) {
                    itemCoSan = item; // nếu tìm thấy thì gán itemCoSan
                    break;
                }
            }

            if (itemCoSan != null) {
                // Nếu đã có, dự kiến tăng 1 đơn vị
                int soLuongMoi = itemCoSan.SoLuong + 1;

                // Kiểm tra kho thực tế qua DichVuDonHang
                var kiemTra = _dichVuDonHang.KiemTraSoLuongTonThucTe(sp.MaSp, soLuongMoi);
                if (kiemTra.DuHang == false) {
                    // Không đủ nguyên liệu -> trả về lỗi để UI hiển thị
                    return (false, kiemTra.ThongBao);
                }

                // Cập nhật số lượng và thành tiền của item đã có
                itemCoSan.SoLuong = soLuongMoi;
                itemCoSan.ThanhTienGoc = soLuongMoi * itemCoSan.DonGiaGoc;
            }
            else {
                // Nếu chưa có item trong giỏ -> kiểm tra kho cho 1 món
                var kiemTra = _dichVuDonHang.KiemTraSoLuongTonThucTe(sp.MaSp, 1);
                if (kiemTra.DuHang == false) {
                    // Không có đủ nguyên liệu cho 1 món -> trả về lỗi
                    return (false, kiemTra.ThongBao);
                }

                // Tạo GioHangItem mới và add vào danh sách
                _items.Add(new GioHangItem {
                    MaSp = sp.MaSp,
                    TenSp = sp.TenSp,
                    SoLuong = 1,
                    DonGiaGoc = sp.DonGia, // Lưu giá gốc từ model SanPham
                    ThanhTienGoc = sp.DonGia // initial thành tiền = 1 * DonGia
                });
            }

            // Trả về thành công nếu vượt qua các kiểm tra
            return (true, "OK");
        }


        /// Giảm số lượng của một món.

        /// <returns>True nếu món bị xóa (SL=0), False nếu chỉ giảm.</returns>
        public bool GiamSoLuong(int maSp) {
            // Tìm item trong danh sách theo mã
            GioHangItem itemCanGiam = null;
            foreach (var item in _items) {
                if (item.MaSp == maSp) {
                    itemCanGiam = item;
                    break;
                }
            }

            // Nếu không tìm thấy, không làm gì và trả false
            if (itemCanGiam == null) return false;

            if (itemCanGiam.SoLuong > 1) {
                // Giảm số lượng và cập nhật thành tiền
                itemCanGiam.SoLuong--;
                itemCanGiam.ThanhTienGoc = itemCanGiam.SoLuong * itemCanGiam.DonGiaGoc;
                return false; // Chỉ giảm, không xóa
            }
            else {
                // Nếu số lượng chỉ có 1 -> xóa hoàn toàn item khỏi giỏ
                _items.Remove(itemCanGiam);
                return true; // đã xóa
            }
        }


        /// Xóa hoàn toàn một món khỏi giỏ, bất kể số lượng.

        public void XoaMon(int maSp) {
            // Tìm item tương ứng và remove khỏi danh sách nếu tồn tại
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


        /// Xóa sạch giỏ hàng.

        public void XoaTatCa() {
            // Clear danh sách item
            _items.Clear();
        }


        /// Lấy danh sách tất cả các món trong giỏ để hiển thị.

        public List<GioHangItem> LayTatCaMon() {
            // Trả về tham chiếu list (read-only handling ở caller nếu cần)
            return _items;
        }


        /// Lấy tổng tiền (chưa tính KM).

        public decimal LayTongTienGoc() {
            decimal tong = 0;
            // Cộng dồn thành tiền gốc của từng item
            foreach (var item in _items) {
                tong += item.ThanhTienGoc;
            }
            return tong;
        }


        /// Đếm số lượng mục (khác với tổng số lượng đơn vị) trong giỏ.

        public int LaySoLuongMon() {
            return _items.Count;
        }
    }
}