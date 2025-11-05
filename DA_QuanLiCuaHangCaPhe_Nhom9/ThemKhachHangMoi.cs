using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    // Đảm bảo tên class này khớp với tên Form của bạn
    public partial class ThemKhachHangMoi : Form {
        // Biến để lưu SĐT được truyền từ MainForm
        private string _soDienThoai;



        public ThemKhachHangMoi(string sdt) {
            InitializeComponent();
            _soDienThoai = sdt;
        }

        private void ThemKhachHangMoi_Load(object sender, EventArgs e) {
            // Tự động điền SĐT vào TextBox
            // (Giả sử TextBox SĐT tên là 'txtSDT')
            txtSDT.Text = _soDienThoai;
            txtSDT.Enabled = false; // Không cho sửa SĐT

            // Thêm các lựa chọn vào ComboBox
            // (Giả sử ComboBox tên là 'cboLoaiKH')
            cbLoaiKH.Items.Add("Thuong");
            cbLoaiKH.Items.Add("VIP");

            // Chọn "Thuong" làm mặc định
            cbLoaiKH.SelectedIndex = 0;
        }

        // Hàm này được gọi khi bấm nút "Hủy"        
        private void btnCancel_Click(object sender, EventArgs e) {
            // Gửi tín hiệu "Cancel" về cho MainForm
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Hàm này được gọi khi bấm nút "Lưu"        
        private void btnSave_Click(object sender, EventArgs e) {
            // 1. Kiểm tra dữ liệu (Tên KH là bắt buộc)

            // .Trim() là hàm dùng để xóa mọi dấu cách ở đầu và cuối
            // Sau đó, ta so sánh xem kết quả có rỗng ("") không.

            if (txtTenKH.Text.Trim() == "") {
                MessageBox.Show("Vui lòng nhập Tên khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Không làm gì cả
            }

            try {
                // 2. Tạo đối tượng KhachHang mới
                var khachHangMoi = new KhachHang {
                    TenKh = txtTenKH.Text,
                    SoDienThoai = txtSDT.Text,

                    // (Giả sử TextBox Địa chỉ tên là 'txtDiaChi')
                    DiaChi = txtDiaChi.Text,

                    // (Giả sử ComboBox tên là 'cbLoaiKH')
                    LoaiKh = cbLoaiKH.SelectedItem.ToString()
                };

                // 3. Mở kết nối CSDL và lưu
                using (DataSqlContext db = new DataSqlContext()) {
                    db.KhachHangs.Add(khachHangMoi);
                    db.SaveChanges(); // Lưu vào CSDL
                }

                // 4. Gửi tín hiệu "OK" (thành công) về cho MainForm
                MessageBox.Show("Thêm khách hàng mới thành công!", "Thông báo");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lưu khách hàng: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}

