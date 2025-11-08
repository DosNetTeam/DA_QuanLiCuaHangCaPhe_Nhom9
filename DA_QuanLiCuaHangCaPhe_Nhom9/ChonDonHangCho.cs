using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class ChonDonHangCho : Form {
        public ChonDonHangCho() {
            InitializeComponent();
        }

        private void ChonDonHangCho_Load(object sender, EventArgs e) {
            TaiDanhSachDonHangCho();
        }

        private void btnTaiLai_Click(object sender, EventArgs e) {
            TaiDanhSachDonHangCho();
        }

        private void btnChonThanhToan_Click(object sender, EventArgs e) {
            // 1. Kiểm tra xem người dùng đã chọn đơn nào chưa
            if (lvDonHangCho.SelectedItems.Count == 0) {
                MessageBox.Show("Vui lòng chọn một đơn hàng để thanh toán.");
                return;
            }

            // 2. Lấy đơn hàng đã chọn
            ListViewItem itemDaChon = lvDonHangCho.SelectedItems[0];

            // Lấy MaDH (số 101, 102,...) mà ta đã lưu trong Tag
            int maDHCanThanhToan = (int)itemDaChon.Tag;

            // 3. Mở Form ThanhToan (frmHoaDonThanhToan)
            //    và truyền MaDH này qua
            // (Chúng ta sẽ sửa Form ThanhToan ở bước sau)
            ThanhToan frmBill = new ThanhToan(maDHCanThanhToan);
            frmBill.ShowDialog();

            // 4. Sau khi form ThanhToan đóng, tải lại danh sách
            // (Vì đơn hàng vừa thanh toán sẽ biến mất khỏi list "Đang xử lý")
            TaiDanhSachDonHangCho();

        }

        private void TaiDanhSachDonHangCho() {
            lvDonHangCho.Items.Clear();

            try {
                // Mở kết nối CSDL
                using (DataSqlContext db = new DataSqlContext()) {
                    // Bước 1: Lấy TẤT CẢ đơn hàng "Đang xử lý"
                    // (Chúng ta không dùng .Include() nữa)
                    var donHangCho = db.DonHangs
                                       .Where(dh => dh.TrangThai == "Đang xử lý")
                                       .OrderBy(dh => dh.NgayLap) // Xếp cái cũ lên trước
                                       .ToList();

                    // Bước 2: Lấy TẤT CẢ khách hàng ra một danh sách tạm
                    // (Đây là cách cơ bản, chúng ta sẽ "join" bằng tay)
                    var allKhachHang = db.KhachHangs.ToList();

                    // Bước 3: Lặp qua từng đơn hàng (trong danh sách donHangCho)
                    foreach (var dh in donHangCho) {
                        string tenKH = "Khách vãng lai";

                        // Bước 3.1: Kiểm tra xem đơn hàng này có MaKH không
                        // (dh.MaKh != null nghĩa là "không phải khách vãng lai")
                        if (dh.MaKh != null) {
                            // Nếu có, lặp qua danh sách allKhachHang để tìm tên
                            foreach (var kh in allKhachHang) {
                                if (kh.MaKh == dh.MaKh) {
                                    tenKH = kh.TenKh;
                                    break; // Dừng tìm khi thấy
                                }
                            }
                        }

                        // Bước 4: Tạo dòng ListView (giống code cũ)
                        ListViewItem lvi = new ListViewItem(dh.MaDh.ToString());

                        // Gán MaDH vào Tag để lát nữa lấy
                        lvi.Tag = dh.MaDh;

                        // Thêm các cột phụ
                        lvi.SubItems.Add(tenKH);

                        // Vì NgayLap có thể null (DateTime?),
                        // chúng ta phải kiểm tra .HasValue
                        if (dh.NgayLap.HasValue) {
                            lvi.SubItems.Add(dh.NgayLap.Value.ToString("HH:mm dd/MM/yy"));
                        }
                        else {
                            lvi.SubItems.Add("N/A"); // Hoặc "" (chuỗi rỗng)
                        }

                        lvi.SubItems.Add(dh.TongTien?.ToString("N0") + " đ");

                        // Thêm dòng này vào ListView
                        lvDonHangCho.Items.Add(lvi);
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải danh sách đơn chờ: " + ex.Message);
            }
        }

    }
}
