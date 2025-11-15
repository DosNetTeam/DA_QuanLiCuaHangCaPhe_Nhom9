
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class ChonDonHangCho : Form {
        public int MaDonHangDaChon { get; private set; }

        // *** THAY ĐỔI: Khai báo Dịch Vụ ***
        private readonly DichVuThanhToan _dichVuThanhToan;

        public ChonDonHangCho() {
            InitializeComponent();

            // *** THAY ĐỔI: Khởi tạo Dịch Vụ ***
            _dichVuThanhToan = new DichVuThanhToan();
        }

        private void ChonDonHangCho_Load(object sender, EventArgs e) {
            TaiDanhSachDonHangCho();
        }

        private void btnTaiLai_Click(object sender, EventArgs e) {
            TaiDanhSachDonHangCho();
        }

        // *** ĐÃ THAY ĐỔI: Gọi DichVuThanhToan ***
        private void btnChonThanhToan_Click(object sender, EventArgs e) {
            if (lvDonHangCho.SelectedItems.Count == 0) {
                MessageBox.Show("Vui lòng chọn một đơn hàng để thanh toán.");
                return;
            }

            ListViewItem itemDaChon = lvDonHangCho.SelectedItems[0];
            int maDHCanThanhToan = (int)itemDaChon.Tag;

            // 1. Gán MaDonHang (Giữ nguyên)
            this.MaDonHangDaChon = maDHCanThanhToan;

            // 2. Tính toán tiền gốc và tiền giảm (*** THAY ĐỔI: Gọi Dịch Vụ ***)
            decimal tongGoc = 0;
            decimal soTienGiam = 0;
            decimal thanhTienCuoi = 0;

            try {
                // Gọi Dịch Vụ để lấy chi tiết
                var donHang = _dichVuThanhToan.LayChiTietDonHangGoc(maDHCanThanhToan);

                if (donHang != null && donHang.ChiTietDonHangs != null) {
                    // Tính tổng gốc (dùng foreach)
                    foreach (var ct in donHang.ChiTietDonHangs) {
                        tongGoc += (ct.DonGia * ct.SoLuong);
                    }
                    thanhTienCuoi = donHang.TongTien ?? tongGoc;
                    soTienGiam = tongGoc - thanhTienCuoi;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải lại chi tiết đơn hàng: " + ex.Message);
            }

            // 3. Mở Form ThanhToan (Giữ nguyên)
            ThanhToan frmThanhToan = new ThanhToan(maDHCanThanhToan, tongGoc, soTienGiam);
            var result = frmThanhToan.ShowDialog();

            // 4. Xử lý kết quả
            if (result == DialogResult.OK) {
                TaiDanhSachDonHangCho(); // Tải lại
            }
        }

        // *** ĐÃ THAY ĐỔI: Gọi DichVuThanhToan ***
        private void TaiDanhSachDonHangCho() {
            lvDonHangCho.Items.Clear();
            try {
                // 1. Gọi Dịch Vụ
                var donHangCho = _dichVuThanhToan.TaiDanhSachDonHangCho();

                // 2. Lặp qua kết quả và điền vào ListView
                foreach (var dh in donHangCho) {
                    ListViewItem lvi = new ListViewItem(dh.MaDh.ToString());
                    lvi.Tag = dh.MaDh;
                    lvi.SubItems.Add(dh.TenKH);
                    lvi.SubItems.Add(dh.NgayLap.HasValue ? dh.NgayLap.Value.ToString("HH:mm dd/MM/yy") : "N/A");
                    lvi.SubItems.Add(dh.TongTien.ToString("N0") + " đ");

                    lvDonHangCho.Items.Add(lvi);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải danh sách đơn chờ: " + ex.Message);
            }
        }

        // *** ĐÃ THAY ĐỔI: Gọi DichVuThanhToan ***
        private void btnHuyDonCho_Click(object sender, EventArgs e) {
            if (lvDonHangCho.SelectedItems.Count == 0) {
                MessageBox.Show("Vui lòng chọn một đơn hàng để HỦY.");
                return;
            }

            ListViewItem itemDaChon = lvDonHangCho.SelectedItems[0];
            int maDHCanHuy = (int)itemDaChon.Tag;

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn HỦY đơn hàng [MaDH: {maDHCanHuy}] không?\n\nHÀNH ĐỘNG NÀY SẼ HOÀN TRẢ NGUYÊN LIỆU VỀ KHO.",
                "Xác nhận Hủy Đơn",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.No) {

                return;
            }

            // 4. Bắt đầu quá trình HỦY (*** THAY ĐỔI ***)
            try {
                // Gọi Dịch Vụ
                bool success = _dichVuThanhToan.HuyDonHangCho(maDHCanHuy);

                if (success) {
                    MessageBox.Show($"Đã hủy thành công đơn hàng {maDHCanHuy}. \nĐã hoàn trả nguyên liệu về kho.", "Thông báo");
                    TaiDanhSachDonHangCho();

                }
                else {
                    MessageBox.Show("Lỗi: Không tìm thấy đơn hàng hoặc thanh toán để hủy. Giao dịch đã được Rollback.", "Lỗi Hủy Đơn");
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi nghiêm trọng khi hủy đơn hàng: " + ex.Message);
            }
        }

        // (Giữ nguyên)
        private void lvDonHangCho_MouseDoubleClick(object sender, MouseEventArgs e) {
            btnChonThanhToan_Click(sender, e);
        }
    }
}