using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main; // Thêm
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class ThemKhachHangMoi : Form {
        private string _soDienThoai;
        private readonly KhoTruyVanMainForm _khoTruyVan;

        public ThemKhachHangMoi(string sdt) {
            InitializeComponent();
            _soDienThoai = sdt;
            _khoTruyVan = new KhoTruyVanMainForm();
        }

        private void ThemKhachHangMoi_Load(object sender, EventArgs e) {
            txtSDT.Text = _soDienThoai;
            txtSDT.Enabled = false;
            if (cbLoaiKH != null) {
                cbLoaiKH.Items.Clear();
                cbLoaiKH.Items.Add("Thuong");
                cbLoaiKH.Items.Add("VIP");
                cbLoaiKH.SelectedIndex = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // *** ĐÃ THAY ĐỔI: Gọi KhoTruyVan ***
        private void btnSave_Click(object sender, EventArgs e) {
            if (txtTenKH.Text.Trim() == "") {
                MessageBox.Show("Vui lòng nhập Tên khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                var khachHangMoi = new KhachHang {
                    TenKh = txtTenKH.Text.Trim(), // Thêm .Trim()
                    SoDienThoai = txtSDT.Text,
                    DiaChi = txtDiaChi.Text.Trim(), // Thêm .Trim()
                    LoaiKh = cbLoaiKH.SelectedItem.ToString()
                };

                // *** THAY ĐỔI: Gọi KhoTruyVan ***
                bool success = _khoTruyVan.ThemKhachHangMoi(khachHangMoi);

                if (success) {
                    MessageBox.Show("Thêm khách hàng mới thành công!", "Thông báo");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else {
                    MessageBox.Show("Lỗi khi lưu khách hàng. Vui lòng thử lại.");
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lưu khách hàng: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}