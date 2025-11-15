using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main {

    /// Đại diện cho một món hàng trong giỏ.
    /// Chỉ lưu trữ dữ liệu, không có logic.

    public class GioHangItem {
        // Mã sản phẩm (khóa tham chiếu đến bảng SanPham)
        public int MaSp { get; set; }
        // Tên sản phẩm để hiển thị lên UI
        public string TenSp { get; set; }
        // Số lượng đơn vị của món trong giỏ
        public int SoLuong { get; set; }
        // Giá gốc của sản phẩm (chưa áp dụng khuyến mãi)
        public decimal DonGiaGoc { get; set; } // Giá gốc (chưa KM)
        // Thành tiền = SoLuong * DonGiaGoc (giữ sẵn để tránh tính lại nhiều lần)
        public decimal ThanhTienGoc { get; set; } // SoLuong * DonGiaGoc
    }


    /// Quản lý logic nghiệp vụ của giỏ hàng.
    /// Không tương tác trực tiếp với UI (ListView).

    public class GioHang {
        // Danh sách các món đang có trong giỏ
        private List<GioHangItem> _items;

        // Dịch vụ để kiểm tra tồn kho / giá bán / logic liên quan đơn hàng
        private readonly DichVuDonHang _dichVuDonHang;

        // Constructor: nhận DichVuDonHang từ MainForm để tái sử dụng logic kiểm kho / giá
        public GioHang(DichVuDonHang dichVu) {
            _items = new List<GioHangItem>(); // Khởi tạo list rỗng
            _dichVuDonHang = dichVu; // Lưu tham chiếu đến dịch vụ kiểm kho
        }


        /// Thêm một sản phẩm vào giỏ hoặc tăng số lượng.
        /// Trả về (Success, Message): Success=true khi thành công, false + thông báo khi thất bại.

        public (bool Success, string Message) ThemMon(SanPham sp) {
            // Tìm xem sản phẩm đã tồn tại trong giỏ chưa bằng cách so sánh MaSp
            GioHangItem itemCoSan = null;
            foreach (var item in _items) {
                if (item.MaSp == sp.MaSp) {
                    itemCoSan = item; // nếu tìm thấy thì gán itemCoSan
                    break;
                }
            }

            if (itemCoSan != null) {
                // Nếu đã có trong giỏ -> dự kiến tăng số lượng lên +1
                int soLuongMoi = itemCoSan.SoLuong + 1;

                // Kiểm tra kho thực tế: gọi DichVuDonHang.KiemTraSoLuongTonThucTe để biết có đủ nguyên liệu cho số lượng mới không
                var kiemTra = _dichVuDonHang.KiemTraSoLuongTonThucTe(sp.MaSp, soLuongMoi);
                if (kiemTra.DuHang == false) {
                    // Nếu không đủ nguyên liệu, trả về false kèm thông báo từ dịch vụ
                    return (false, kiemTra.ThongBao);
                }

                // Nếu đủ -> cập nhật số lượng và thành tiền gốc cho item trong giỏ
                itemCoSan.SoLuong = soLuongMoi;
                itemCoSan.ThanhTienGoc = soLuongMoi * itemCoSan.DonGiaGoc;
            }
            else {
                // Nếu chưa có trong giỏ -> kiểm tra kho với 1 đơn vị trước khi thêm
                var kiemTra = _dichVuDonHang.KiemTraSoLuongTonThucTe(sp.MaSp, 1);
                if (kiemTra.DuHang == false) {
                    // Không đủ nguyên liệu cho 1 món -> trả về lỗi
                    return (false, kiemTra.ThongBao);
                }

                // Nếu đủ -> tạo GioHangItem mới với số lượng = 1 và thêm vào danh sách
                _items.Add(new GioHangItem {
                    MaSp = sp.MaSp,
                    TenSp = sp.TenSp,
                    SoLuong = 1,
                    DonGiaGoc = sp.DonGia, // lưu giá gốc
                    ThanhTienGoc = sp.DonGia // thành tiền ban đầu = 1 * DonGia
                });
            }

            // Trả về thành công nếu vượt qua các kiểm tra
            return (true, "OK");
        }


        /// Giảm số lượng của một món (giảm 1).
        /// Trả về true nếu món bị xóa (sau khi giảm = 0), false nếu chỉ giảm.

        public bool GiamSoLuong(int maSp) {
            // Tìm item theo mã
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
                // Nếu số lượng > 1 -> giảm 1 và cập nhật thành tiền
                itemCanGiam.SoLuong--;
                itemCanGiam.ThanhTienGoc = itemCanGiam.SoLuong * itemCanGiam.DonGiaGoc;
                return false; // chỉ giảm, không xóa
            }
            else {
                // Nếu số lượng hiện tại = 1 -> xóa item khỏi giỏ
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


        /// Lấy tổng tiền (chưa tính khuyến mãi).

        public decimal LayTongTienGoc() {
            decimal tong = 0;
            // Cộng dồn thành tiền gốc của từng item
            foreach (var item in _items) {
                tong += item.ThanhTienGoc;
            }
            return tong;
        }


        /// Đếm số lượng món trong giỏ.

        public int LaySoLuongMon() {
            return _items.Count;
        }
    }
}