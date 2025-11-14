// Thêm namespace của KhoTruyVan
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class Admin : Form {
        // Khai báo Kho Truy Vấn
        private readonly KhoTruyVanAdmin _khoTruyVan;
        private int selectedEmployeeId = 0;

        public Admin() {
            InitializeComponent();

            // Khởi tạo Kho Truy Vấn
            _khoTruyVan = new KhoTruyVanAdmin();

            // (Các cài đặt và sự kiện giữ nguyên)
            this.StartPosition = FormStartPosition.CenterScreen;
            dtgvquanlynhanvien.CellClick += dataGridView1_CellClick;
            dtgvquanlynhanvien.KeyDown += dataGridView1_KeyDown;
            dgvkho.CellClick += dgvInventory_CellClick;
            dgvkho.KeyDown += dgvInventory_KeyDown;
            CreateSaveButton();
            pnthongtinnv.Visible = false;
            dgvSanPham.CellClick += dgvSanPham_CellClick;
            dgvSanPham.KeyDown += dgvSanPham_KeyDown;
            tabControlKho.SelectedIndexChanged += TabControlKho_SelectedIndexChanged;
            dgvkhuyenmai.CellClick += dgvkhuyenmai_CellClick;
            dgvkhuyenmai.KeyDown += dgvkhuyenmai_KeyDown;
        }

        private void CreateSaveButton() {
            // (Để trống)
        }

        // ========== EVENT HANDLER SẢN PHẨM (Giữ nguyên) ==========
        private void dgvSanPham_CellClick(object? sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                ShowProductDetailInPanel(e.RowIndex);
            }
        }

        private void dgvSanPham_KeyDown(object? sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && dgvSanPham.CurrentRow != null) {
                e.Handled = true;
                ShowProductDetailInPanel(dgvSanPham.CurrentRow.Index);
            }
        }

        // ========== HÀM TẢI DỮ LIỆU SẢN PHẨM (ĐÃ TÁCH CSDL) =========
        private void LoadProductData() {
            try {
                // 1. Gọi KhoTruyVan
                var products = _khoTruyVan.TaiDuLieuSanPham();

                // 2. Gán DataSource
                dgvSanPham.DataSource = products;

                // 3. Định dạng cột (Giữ nguyên)
                StyleDataGridView(dgvSanPham);
                dgvSanPham.Columns["MaSp"].HeaderText = "Mã SP";
                dgvSanPham.Columns["TenSp"].HeaderText = "Tên sản phẩm";
                dgvSanPham.Columns["LoaiSp"].HeaderText = "Loại";
                dgvSanPham.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvSanPham.Columns["DonVi"].HeaderText = "Đơn vị";
                dgvSanPham.Columns["TrangThai"].HeaderText = "Trạng thái";
                dgvSanPham.Columns["DonGia"].DefaultCellStyle.Format = "N0";

                // 4. Áp dụng màu (Giữ nguyên)
                for (int i = 0; i < dgvSanPham.Rows.Count; i++) {
                    DataGridViewRow row = dgvSanPham.Rows[i];
                    if (row.Cells["TrangThai"].Value != null) {
                        string trangThai = row.Cells["TrangThai"].Value.ToString();
                        if (trangThai == "Còn bán") {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(223, 240, 216);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        }
                        else if (trangThai == "Ngừng bán") {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(242, 222, 222);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Admin_Load(object sender, EventArgs e) {
            LoadOverviewData();
        }

        // Hàm chuyển Tab (Giữ nguyên logic gọi LoadData)
        private void tabControl1_SelectedIndexChanged(object? sender, EventArgs e) {
            switch (bandieukhien.SelectedIndex) {
                case 0: LoadOverviewData(); break;
                case 1: LoadEmployeeData(); pnthongtinnv.Visible = false; break;
                case 2: LoadInventoryData(); gbthongtinnguyenlieu.Visible = false; gbsanpham.Visible = false; break;
                case 3: LoadRevenueData(); break;
                case 4: LoadKhuyenMaiData(); gbkhuyenmai.Visible = false; break;
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void LoadOverviewData() {
            try {
                // 1. Gọi KhoTruyVan
                List<DuLieuTongQuan> overviewData = _khoTruyVan.TaiDuLieuTongQuan();

                // 2. Gán DataSource
                tongquandulieu.DataSource = overviewData;

                // 3. Định dạng cột
                StyleDataGridView(tongquandulieu);
                tongquandulieu.Columns["Category"].HeaderText = "Danh mục";
                tongquandulieu.Columns["Count"].HeaderText = "Số lượng";
                tongquandulieu.Columns["Details"].HeaderText = "Chi tiết";
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi tải dữ liệu tổng quan:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void LoadEmployeeData() {
            try {
                // 1. Gọi KhoTruyVan
                var employees = _khoTruyVan.TaiDuLieuNhanVien();

                // 2. Gán DataSource
                dtgvquanlynhanvien.DataSource = employees;

                // 3. Định dạng cột
                StyleDataGridView(dtgvquanlynhanvien);
                dtgvquanlynhanvien.Columns["MaNv"].HeaderText = "Mã NV";
                dtgvquanlynhanvien.Columns["TenNv"].HeaderText = "Họ tên";
                dtgvquanlynhanvien.Columns["ChucVu"].HeaderText = "Chức vụ";
                dtgvquanlynhanvien.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dtgvquanlynhanvien.Columns["TaiKhoan"].HeaderText = "Tài khoản";
                dtgvquanlynhanvien.Columns["VaiTro"].HeaderText = "Vai trò";
                dtgvquanlynhanvien.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void LoadInventoryData() {
            try {
                // 1. Gọi KhoTruyVan
                var sortedList = _khoTruyVan.TaiDuLieuKho();

                // 2. Gán DataSource
                dgvkho.DataSource = sortedList;

                // 3. Định dạng cột
                StyleDataGridView(dgvkho);
                dgvkho.Columns["MaNl"].HeaderText = "Mã NL";
                dgvkho.Columns["TenNl"].HeaderText = "Tên nguyên liệu";
                dgvkho.Columns["DonViTinh"].HeaderText = "Đơn vị";
                dgvkho.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
                dgvkho.Columns["NguongCanhBao"].HeaderText = "Ngưỡng cảnh báo";
                dgvkho.Columns["TinhTrang"].HeaderText = "Tình trạng";

                // 4. Áp dụng màu (Giữ nguyên)
                for (int i = 0; i < dgvkho.Rows.Count; i++) {
                    DataGridViewRow row = dgvkho.Rows[i];
                    if (row.Cells["TinhTrang"].Value != null) {
                        string tinhTrang = row.Cells["TinhTrang"].Value.ToString();
                        if (tinhTrang == "Dồi dào") {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(223, 240, 216);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        }
                        else if (tinhTrang == "Thiếu thốn") {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(156, 87, 0);
                        }
                        else if (tinhTrang == "Hết hàng") {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(183, 28, 28);
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void LoadRevenueData() {
            try {
                // 1. Gọi KhoTruyVan
                var sortedData = _khoTruyVan.TaiDuLieuDoanhThu();

                // 2. Gán DataSource
                dgvdoanhthu.DataSource = sortedData;

                // 3. Định dạng cột
                StyleDataGridView(dgvdoanhthu);
                dgvdoanhthu.Columns["Thang"].HeaderText = "Tháng";
                dgvdoanhthu.Columns["SoDonHang"].HeaderText = "Số đơn hàng";
                dgvdoanhthu.Columns["TongDoanhThu"].HeaderText = "Tổng doanh thu";
                dgvdoanhthu.Columns["DoanhThuTrungBinh"].HeaderText = "TB/Đơn";
                dgvdoanhthu.Columns["DonHangLonNhat"].HeaderText = "Cao nhất";
                dgvdoanhthu.Columns["DonHangNhoNhat"].HeaderText = "Thấp nhất";

                dgvdoanhthu.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
                dgvdoanhthu.Columns["DoanhThuTrungBinh"].DefaultCellStyle.Format = "N0";
                dgvdoanhthu.Columns["DonHangLonNhat"].DefaultCellStyle.Format = "N0";
                dgvdoanhthu.Columns["DonHangNhoNhat"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu doanh thu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm Style (Giữ nguyên)
        private void StyleDataGridView(DataGridView dgv) {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 181, 246);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // (Giữ nguyên CellClick và KeyDown cho Nhân Viên)
        private void dataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                ShowEmployeeDetailInPanel(e.RowIndex);
            }
        }

        private void dataGridView1_KeyDown(object? sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && dtgvquanlynhanvien.CurrentRow != null) {
                e.Handled = true;
                ShowEmployeeDetailInPanel(dtgvquanlynhanvien.CurrentRow.Index);
            }
        }

        // (Giữ nguyên CellClick và KeyDown cho Kho)
        private void dgvInventory_CellClick(object? sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                ShowInventoryDetailInPanel(e.RowIndex);
            }
        }

        private void dgvInventory_KeyDown(object? sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && dgvkho.CurrentRow != null) {
                e.Handled = true;
                ShowInventoryDetailInPanel(dgvkho.CurrentRow.Index);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void ShowProductDetailInPanel(int rowIndex) {
            try {
                DataGridViewRow row = dgvSanPham.Rows[rowIndex];
                if (row.Cells["MaSp"].Value == null) return;

                int maSp = Convert.ToInt32(row.Cells["MaSp"].Value);

                // 1. Gọi KhoTruyVan
                SanPham product = _khoTruyVan.LayChiTietSanPham(maSp);

                // 2. Hiển thị UI (Giữ nguyên logic)
                if (product != null) {
                    txtmasp.Text = product.MaSp.ToString();
                    txttensp.Text = product.TenSp;
                    txtgia.Text = product.DonGia.ToString("N0");
                    txtdongia.Text = product.DonGia.ToString("N0");
                    txtloai.Text = product.LoaiSp ?? "";
                    txtdon_vi.Text = product.DonVi ?? "";
                    txttinhtrangsp.Text = product.TrangThai ?? "Còn kinh doanh";

                    txtmasp.ReadOnly = true;
                    txttensp.ReadOnly = false;
                    txtloai.ReadOnly = false;
                    txtdongia.ReadOnly = false;
                    txtdon_vi.ReadOnly = false;
                    txtgia.ReadOnly = false;
                    txttinhtrangsp.ReadOnly = false;

                    gbsanpham.Visible = true;
                    gbsanpham.BringToFront();
                    txttensp.Focus();
                    txttensp.SelectAll();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi hiển thị chi tiết sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void ShowEmployeeDetailInPanel(int rowIndex) {
            try {
                DataGridViewRow row = dtgvquanlynhanvien.Rows[rowIndex];
                if (row.Cells["MaNv"].Value == null) return;

                int maNv = Convert.ToInt32(row.Cells["MaNv"].Value);
                selectedEmployeeId = maNv;

                // 1. Gọi KhoTruyVan
                var nhanVien = _khoTruyVan.LayChiTietNhanVien(maNv);

                // 2. Hiển thị UI (Giữ nguyên logic)
                if (nhanVien != null) {
                    txtmanv.Text = nhanVien.MaNv.ToString();
                    txthoten.Text = nhanVien.TenNv;
                    txtsdt.Text = string.IsNullOrEmpty(nhanVien.SoDienThoai) ? "Chưa cập nhật" : nhanVien.SoDienThoai;
                    txtchucvu.Text = nhanVien.ChucVu;

                    if (nhanVien.TaiKhoan != null) {
                        txttentaikhoan.Text = nhanVien.TaiKhoan.TenDangNhap;
                        txtmatkhau.Text = nhanVien.TaiKhoan.MatKhau;

                        if (nhanVien.TaiKhoan.MaVaiTroNavigation != null) {
                            txtvaitro.Text = nhanVien.TaiKhoan.MaVaiTroNavigation.TenVaiTro;
                            txtvaitro.Tag = nhanVien.TaiKhoan.MaVaiTro;
                        }
                        else {
                            txtvaitro.Text = "N/A";
                            txtvaitro.Tag = null;
                        }
                    }
                    else {
                        txttentaikhoan.Text = "Chưa có";
                        txtmatkhau.Text = "";
                        txtvaitro.Text = "N/A";
                        txtvaitro.Tag = null;
                    }

                    txtmatkhau.ReadOnly = true;
                    txtmatkhau.UseSystemPasswordChar = true;
                    txtchucvu.ReadOnly = false;
                    txtvaitro.ReadOnly = false;
                    pnthongtinnv.Visible = true;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi hiển thị chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void button1_Click(object sender, EventArgs e) // Đổi mật khẩu
        {
            try {
                if (string.IsNullOrEmpty(txtmanv.Text)) {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string tenDangNhap = txttentaikhoan.Text;
                if (tenDangNhap == "Chưa có") {
                    MessageBox.Show("Nhân viên này chưa có tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // (Code tạo Form Prompt giữ nguyên)
                using (Form promptForm = new Form()) {
                    promptForm.Width = 450;
                    promptForm.Height = 250;
                    promptForm.Text = "Đổi mật khẩu";
                    promptForm.StartPosition = FormStartPosition.CenterParent;
                    // ... (Thêm các Controls: Labels, TextBoxes, Buttons y như code gốc)
                    Label lblInfo = new Label() { Text = $"Đổi mật khẩu cho: {txthoten.Text} ({tenDangNhap})", Left = 20, Top = 20, Width = 400, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
                    Label lblOldPass = new Label() { Text = "Mật khẩu hiện tại:", Left = 20, Top = 60, Width = 150 };
                    TextBox txtOldPass = new TextBox() { Left = 180, Top = 57, Width = 230, UseSystemPasswordChar = true };
                    Label lblNewPass = new Label() { Text = "Mật khẩu mới:", Left = 20, Top = 95, Width = 150 };
                    TextBox txtNewPass = new TextBox() { Left = 180, Top = 92, Width = 230, UseSystemPasswordChar = true };
                    Label lblConfirmPass = new Label() { Text = "Xác nhận mật khẩu:", Left = 20, Top = 130, Width = 150 };
                    TextBox txtConfirmPass = new TextBox() { Left = 180, Top = 127, Width = 230, UseSystemPasswordChar = true };
                    Button btnOK = new Button() { Text = "Lưu", Left = 180, Top = 165, Width = 110, Height = 35, BackColor = Color.FromArgb(46, 125, 50), ForeColor = Color.White, DialogResult = DialogResult.OK };
                    Button btnCancel = new Button() { Text = "Hủy", Left = 300, Top = 165, Width = 110, Height = 35, BackColor = Color.FromArgb(211, 47, 47), ForeColor = Color.White, DialogResult = DialogResult.Cancel };
                    promptForm.Controls.Add(lblInfo);
                    promptForm.Controls.Add(lblOldPass);
                    promptForm.Controls.Add(txtOldPass);
                    promptForm.Controls.Add(lblNewPass);
                    promptForm.Controls.Add(txtNewPass);
                    promptForm.Controls.Add(lblConfirmPass);
                    promptForm.Controls.Add(txtConfirmPass);
                    promptForm.Controls.Add(btnOK);
                    promptForm.Controls.Add(btnCancel);
                    promptForm.AcceptButton = btnOK;
                    promptForm.CancelButton = btnCancel;
                    // ... (Kết thúc thêm Controls)

                    if (promptForm.ShowDialog() == DialogResult.OK) {
                        // (Validate input giữ nguyên)
                        if (string.IsNullOrWhiteSpace(txtOldPass.Text)) { MessageBox.Show("Vui lòng nhập mật khẩu hiện tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                        if (string.IsNullOrWhiteSpace(txtNewPass.Text)) { MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                        if (txtNewPass.Text.Length < 3) { MessageBox.Show("Mật khẩu mới phải có ít nhất 3 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                        if (txtNewPass.Text != txtConfirmPass.Text) { MessageBox.Show("Mật khẩu xác nhận không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                        // *** THAY ĐỔI: Gọi KhoTruyVan ***
                        int ketQua = _khoTruyVan.DoiMatKhau(tenDangNhap, txtOldPass.Text, txtNewPass.Text);

                        // Xử lý kết quả
                        if (ketQua == 0) // OK
                        {
                            MessageBox.Show($"Đổi mật khẩu thành công cho: {tenDangNhap}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtmatkhau.Text = txtNewPass.Text;
                            LoadEmployeeData();
                        }
                        else if (ketQua == 2) // Sai MK
                        {
                            MessageBox.Show("Mật khẩu hiện tại không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (ketQua == 3) // Không tìm thấy TK
                        {
                            MessageBox.Show("Không tìm thấy tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else // Lỗi
                        {
                            MessageBox.Show("Đã xảy ra lỗi khi đổi mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi đổi mật khẩu:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                // Luôn khôi phục trạng thái
                if (txtmatkhau != null) {
                    txtmatkhau.ReadOnly = true;
                    txtmatkhau.UseSystemPasswordChar = true;
                }
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void ShowInventoryDetailInPanel(int rowIndex) {
            try {
                DataGridViewRow row = dgvkho.Rows[rowIndex];
                if (row.Cells["MaNl"].Value == null) return;

                int maNl = Convert.ToInt32(row.Cells["MaNl"].Value);

                // 1. Gọi KhoTruyVan
                NguyenLieu ingredient = _khoTruyVan.LayChiTietNguyenLieu(maNl);

                // 2. Hiển thị UI (Giữ nguyên logic)
                if (ingredient != null) {
                    txtma.Text = ingredient.MaNl.ToString();
                    txtten.Text = ingredient.TenNl;
                    txtsoluong.Text = ingredient.SoLuongTon?.ToString() ?? "0";
                    txtdonvi.Text = ingredient.DonViTinh;
                    textBox2.Text = ingredient.TrangThai ?? "Đang kinh doanh";

                    txtma.ReadOnly = true;
                    txtten.ReadOnly = true;
                    txtdonvi.ReadOnly = true;
                    txtsoluong.ReadOnly = false;
                    textBox2.ReadOnly = false;

                    txtsoluong.Focus();
                    txtsoluong.SelectAll();
                    lblcapnhat.Tag = maNl; // Tag để biết là Sửa
                    lblcapnhat.Text = "Cập nhật";

                    gbthongtinnguyenlieu.Visible = true;
                    gbthongtinnguyenlieu.BringToFront();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi hiển thị chi tiết nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void button2_Click(object sender, EventArgs e) // Lưu (Nguyên liệu)
        {
            try {
                // (Validate input giữ nguyên)
                if (!decimal.TryParse(txtsoluong.Text, out decimal soLuongMoi) || soLuongMoi < 0) {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ (lớn hơn hoặc bằng 0)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtsoluong.Focus();
                    return;
                }
                string tinhTrangMoi = textBox2.Text.Trim();

                // KIỂM TRA CHẾ ĐỘ: Update hay Add New
                if (lblcapnhat.Tag != null) {
                    // ============ CHẾ ĐỘ UPDATE ============
                    int maNl = (int)lblcapnhat.Tag;
                    if (MessageBox.Show($"Xác nhận CẬP NHẬT số lượng cho '{txtten.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                    bool success = _khoTruyVan.CapNhatNguyenLieu(maNl, soLuongMoi, tinhTrangMoi);
                    if (success) {
                        MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else {
                        MessageBox.Show("Cập nhật thất bại! Không tìm thấy nguyên liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else {
                    // ============ CHẾ ĐỘ ADD NEW ============
                    if (string.IsNullOrWhiteSpace(txtten.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtten.Focus(); return; }
                    if (string.IsNullOrWhiteSpace(txtdonvi.Text)) { MessageBox.Show("Vui lòng nhập đơn vị!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtdonvi.Focus(); return; }
                    if (MessageBox.Show($"Xác nhận THÊM MỚI '{txtten.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                    var newIngredient = _khoTruyVan.ThemNguyenLieu(txtten.Text.Trim(), txtdonvi.Text.Trim(), soLuongMoi, tinhTrangMoi);
                    if (newIngredient != null) {
                        MessageBox.Show($"Thêm thành công! Mã mới: {newIngredient.MaNl}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else {
                        MessageBox.Show("Thêm mới thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                LoadInventoryData();
                ClearInventoryForm();
                gbthongtinnguyenlieu.Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lưu nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button4: Thêm mới nguyên liệu (Click)
        private void button4_Click(object sender, EventArgs e) {
            ClearInventoryForm();
            txtma.ReadOnly = true;
            txtma.Text = "(Tự động)";
            txtten.ReadOnly = false;
            txtsoluong.ReadOnly = false;
            txtdonvi.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox2.Text = "Đang kinh doanh";
            lblcapnhat.Tag = null; // Tag = null để biết là Thêm mới
            lblcapnhat.Text = "Lưu Mới";
            gbthongtinnguyenlieu.Visible = true;
            gbthongtinnguyenlieu.BringToFront();
            txtten.Focus();
        }

        // Xóa trắng form nguyên liệu
        private void ClearInventoryForm() {
            txtma.Clear();
            txtten.Clear();
            txtsoluong.Clear();
            txtdonvi.Clear();
            textBox2.Clear();
            lblcapnhat.Tag = null;
            lblcapnhat.Text = "Cập nhật";
        }

        // *** ĐÃ TÁCH CSDL ***
        private void btnthem_Click(object sender, EventArgs e) // Thêm Sản Phẩm
        {
            try {
                // (Validate input giữ nguyên)
                if (string.IsNullOrWhiteSpace(txttensp.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txttensp.Focus(); return; }
                if (string.IsNullOrWhiteSpace(txtdongia.Text) || !decimal.TryParse(txtdongia.Text.Replace(".", "").Replace(",", ""), out decimal donGia)) { MessageBox.Show("Vui lòng nhập đơn giá hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtdongia.Focus(); return; }
                if (MessageBox.Show($"Xác nhận THÊM MỚI '{txttensp.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                // 1. Gọi KhoTruyVan
                var newProduct = _khoTruyVan.ThemSanPham(
                    txttensp.Text.Trim(),
                    txtloai.Text.Trim(),
                    donGia,
                    txtdon_vi.Text.Trim(),
                    txttinhtrangsp.Text.Trim()
                );

                // 2. Xử lý kết quả
                if (newProduct != null) {
                    MessageBox.Show($"Thêm sản phẩm thành công! Mã mới: {newProduct.MaSp}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductData();
                    gbsanpham.Visible = false;
                }
                else {
                    MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void btnsua_Click(object sender, EventArgs e) // Sửa Sản Phẩm
        {
            try {
                // (Validate input giữ nguyên)
                if (string.IsNullOrEmpty(txtmasp.Text)) { MessageBox.Show("Vui lòng chọn sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                int maSp = Convert.ToInt32(txtmasp.Text);
                if (string.IsNullOrWhiteSpace(txttensp.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txttensp.Focus(); return; }
                if (string.IsNullOrWhiteSpace(txtdongia.Text) || !decimal.TryParse(txtdongia.Text.Replace(".", "").Replace(",", ""), out decimal donGia)) { MessageBox.Show("Vui lòng nhập đơn giá hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtdongia.Focus(); return; }
                if (MessageBox.Show($"Xác nhận CẬP NHẬT '{txttensp.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                // 1. Gọi KhoTruyVan
                var updatedProduct = _khoTruyVan.CapNhatSanPham(
                    maSp,
                    txttensp.Text.Trim(),
                    txtloai.Text.Trim(),
                    donGia,
                    txtdon_vi.Text.Trim(),
                    txttinhtrangsp.Text.Trim()
                );

                // 2. Xử lý kết quả
                if (updatedProduct != null) {
                    MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductData();
                    gbsanpham.Visible = false;
                }
                else {
                    MessageBox.Show("Cập nhật thất bại! Không tìm thấy sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi cập nhật sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void btnxoa_Click(object sender, EventArgs e) // Xóa Sản Phẩm
        {
            try {
                // (Validate input giữ nguyên)
                if (string.IsNullOrEmpty(txtmasp.Text)) { MessageBox.Show("Vui lòng chọn sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                int maSp = Convert.ToInt32(txtmasp.Text);
                if (MessageBox.Show($"Xác nhận XÓA '{txttensp.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                // 1. Gọi KhoTruyVan
                bool success = _khoTruyVan.XoaSanPham(maSp);

                // 2. Xử lý kết quả
                if (success) {
                    MessageBox.Show("Đã xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductData();
                    gbsanpham.Visible = false;
                }
                else {
                    MessageBox.Show("Xóa thất bại! Không tìm thấy sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm chuyển Tab Kho (Giữ nguyên)
        private void TabControlKho_SelectedIndexChanged(object? sender, EventArgs e) {
            try {
                if (tabControlKho.SelectedIndex == 0) {
                    LoadInventoryData();
                    gbthongtinnguyenlieu.Visible = false;
                }
                else if (tabControlKho.SelectedIndex == 1) {
                    LoadProductData();
                    gbsanpham.Visible = false;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi chuyển tab: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void LoadKhuyenMaiData() {
            try {
                // 1. Gọi KhoTruyVan
                var khuyenMais = _khoTruyVan.TaiDuLieuKhuyenMai();

                // 2. Gán DataSource
                dgvkhuyenmai.DataSource = khuyenMais;

                // 3. Định dạng cột (Giữ nguyên)
                StyleDataGridView(dgvkhuyenmai);
                dgvkhuyenmai.Columns["MaKm"].HeaderText = "Mã KM";
                dgvkhuyenmai.Columns["TenKm"].HeaderText = "Tên khuyến mãi";
                dgvkhuyenmai.Columns["MoTa"].HeaderText = "Mô tả";
                dgvkhuyenmai.Columns["LoaiKm"].HeaderText = "Loại KM";
                dgvkhuyenmai.Columns["GiaTri"].HeaderText = "Giá trị (%)";
                dgvkhuyenmai.Columns["NgayBatDau"].HeaderText = "Ngày bắt đầu";
                dgvkhuyenmai.Columns["NgayKetThuc"].HeaderText = "Ngày kết thúc";
                dgvkhuyenmai.Columns["TrangThai"].HeaderText = "Trạng thái";
                dgvkhuyenmai.Columns["NgayBatDau"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvkhuyenmai.Columns["NgayKetThuc"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // (Giữ nguyên CellClick và KeyDown cho Khuyến Mãi)
        private void dgvkhuyenmai_CellClick(object? sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                ShowKhuyenMaiDetailInPanel(e.RowIndex);
            }
        }

        private void dgvkhuyenmai_KeyDown(object? sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && dgvkhuyenmai.CurrentRow != null) {
                e.Handled = true;
                ShowKhuyenMaiDetailInPanel(dgvkhuyenmai.CurrentRow.Index);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void ShowKhuyenMaiDetailInPanel(int rowIndex) {
            try {
                DataGridViewRow row = dgvkhuyenmai.Rows[rowIndex];
                if (row.Cells["MaKm"].Value == null) return;

                int maKm = Convert.ToInt32(row.Cells["MaKm"].Value);

                // 1. Gọi KhoTruyVan
                var khuyenMai = _khoTruyVan.LayChiTietKhuyenMai(maKm);

                // 2. Hiển thị UI (Giữ nguyên logic)
                if (khuyenMai != null) {
                    txttenkm.Text = khuyenMai.TenKm;
                    txtmota.Text = khuyenMai.MoTa ?? "";
                    cmbloaikm.SelectedItem = khuyenMai.LoaiKm ?? "HoaDon";
                    txtgiatri.Text = khuyenMai.GiaTri.ToString();
                    txtgtkm.Text = "0"; // (Giữ nguyên logic gốc)
                    dtpngaybatdau.Value = khuyenMai.NgayBatDau.ToDateTime(TimeOnly.MinValue);
                    dtpngayketthuc.Value = khuyenMai.NgayKetThuc.ToDateTime(TimeOnly.MinValue);
                    cmbtrangthaikm.SelectedItem = khuyenMai.TrangThai ?? "Đang áp dụng";

                    gbkhuyenmai.Visible = true;
                    gbkhuyenmai.BringToFront();
                    txttenkm.Focus();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi hiển thị chi tiết khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void btnCreateAccount_Click(object sender, EventArgs e) {
            try {
                // (Validate input giữ nguyên)
                if (dtgvquanlynhanvien.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                int maNv = Convert.ToInt32(dtgvquanlynhanvien.SelectedRows[0].Cells["MaNv"].Value);
                string tenNv = dtgvquanlynhanvien.SelectedRows[0].Cells["TenNv"].Value.ToString();
                string taiKhoan = dtgvquanlynhanvien.SelectedRows[0].Cells["TaiKhoan"].Value.ToString();
                if (taiKhoan != "Chưa có") { MessageBox.Show("Nhân viên này đã có tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

                // (Code tạo Form Prompt giữ nguyên)
                using (Form promptForm = new Form()) {
                    promptForm.Width = 450;
                    promptForm.Height = 280;
                    // ... (Thêm các Controls: Labels, TextBoxes, Buttons y như code gốc)
                    Label lblInfo = new Label() { Text = $"Tạo tài khoản cho: {tenNv}", Left = 20, Top = 20, Width = 400, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
                    Label lblUsername = new Label() { Text = "Tên đăng nhập:", Left = 20, Top = 60, Width = 150 };
                    TextBox txtUsername = new TextBox() { Left = 180, Top = 57, Width = 230 };
                    Label lblPassword = new Label() { Text = "Mật khẩu:", Left = 20, Top = 95, Width = 150 };
                    TextBox txtPassword = new TextBox() { Left = 180, Top = 92, Width = 230, UseSystemPasswordChar = true };
                    Label lblRole = new Label() { Text = "Vai trò:", Left = 20, Top = 130, Width = 150 };
                    ComboBox cmbRole = new ComboBox() { Left = 180, Top = 127, Width = 230, DropDownStyle = ComboBoxStyle.DropDownList };

                    // *** THAY ĐỔI: Gọi KhoTruyVan để lấy VaiTro ***
                    cmbRole.DataSource = _khoTruyVan.LayDanhSachVaiTro();
                    cmbRole.DisplayMember = "TenVaiTro";
                    cmbRole.ValueMember = "MaVaiTro";

                    Button btnOK = new Button() { Text = "Tạo", Left = 180, Top = 175, Width = 110, Height = 35, BackColor = Color.FromArgb(46, 125, 50), ForeColor = Color.White, DialogResult = DialogResult.OK };
                    Button btnCancel = new Button() { Text = "Hủy", Left = 300, Top = 175, Width = 110, Height = 35, BackColor = Color.FromArgb(211, 47, 47), ForeColor = Color.White, DialogResult = DialogResult.Cancel };
                    promptForm.Controls.Add(lblInfo);
                    promptForm.Controls.Add(lblUsername);
                    promptForm.Controls.Add(txtUsername);
                    promptForm.Controls.Add(lblPassword);
                    promptForm.Controls.Add(txtPassword);
                    promptForm.Controls.Add(lblRole);
                    promptForm.Controls.Add(cmbRole);
                    promptForm.Controls.Add(btnOK);
                    promptForm.Controls.Add(btnCancel);
                    // ... (Kết thúc thêm Controls)

                    if (promptForm.ShowDialog() == DialogResult.OK) {
                        if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text)) { MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                        // *** THAY ĐỔI: Gọi KhoTruyVan ***
                        bool success = _khoTruyVan.TaoTaiKhoan(maNv, txtUsername.Text.Trim(), txtPassword.Text.Trim(), (int)cmbRole.SelectedValue);

                        if (success) {
                            MessageBox.Show("Tạo tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEmployeeData();
                        }
                        else {
                            MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tạo tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void btnDeleteAccount_Click(object sender, EventArgs e) {
            try {
                // (Validate input giữ nguyên)
                if (dtgvquanlynhanvien.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                int maNv = Convert.ToInt32(dtgvquanlynhanvien.SelectedRows[0].Cells["MaNv"].Value);
                string tenNv = dtgvquanlynhanvien.SelectedRows[0].Cells["TenNv"].Value.ToString();
                string taiKhoan = dtgvquanlynhanvien.SelectedRows[0].Cells["TaiKhoan"].Value.ToString();
                if (taiKhoan == "Chưa có") { MessageBox.Show("Nhân viên này chưa có tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                if (MessageBox.Show($"Xác nhận xóa tài khoản của: {tenNv}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                // *** THAY ĐỔI: Gọi KhoTruyVan ***
                bool success = _khoTruyVan.XoaTaiKhoan(maNv);

                if (success) {
                    MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmployeeData();
                }
                else {
                    MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // (Các hàm Logout/Exit giữ nguyên)
        private void btnLogout_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) {
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) {
                Application.Exit();
            }
        }

        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void lblthongtinnhanvien_Click(object sender, EventArgs e) { }
        private void txtgtkm_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }

        // *** ĐÃ TÁCH CSDL ***
        private void btnthemkm_Click_1(object sender, EventArgs e) // Thêm KM
        {
            try {
                // (Validate input giữ nguyên)
                if (string.IsNullOrWhiteSpace(txttenkm.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txttenkm.Focus(); return; }
                if (!decimal.TryParse(txtgiatri.Text, out decimal giaTri) || giaTri < 0 || giaTri > 100) { MessageBox.Show("Giá trị phải từ 0-100!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtgiatri.Focus(); return; }
                if (MessageBox.Show($"Xác nhận THÊM MỚI '{txttenkm.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                // 1. Tạo đối tượng KM
                var newKhuyenMai = new KhuyenMai {
                    TenKm = txttenkm.Text.Trim(),
                    MoTa = txtmota.Text.Trim(),
                    LoaiKm = cmbloaikm.SelectedItem?.ToString() ?? "HoaDon",
                    GiaTri = giaTri,
                    NgayBatDau = DateOnly.FromDateTime(dtpngaybatdau.Value),
                    NgayKetThuc = DateOnly.FromDateTime(dtpngayketthuc.Value),
                    TrangThai = cmbtrangthaikm.SelectedItem?.ToString() ?? "Đang áp dụng"
                };

                // 2. Gọi KhoTruyVan
                bool success = _khoTruyVan.ThemKhuyenMai(newKhuyenMai);

                // 3. Xử lý kết quả
                if (success) {
                    MessageBox.Show("Thêm khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhuyenMaiData();
                    gbkhuyenmai.Visible = false;
                }
                else {
                    MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi thêm khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void btnsuakm_Click_1(object sender, EventArgs e) // Sửa KM
        {
            try {
                // (Validate input giữ nguyên)
                if (dgvkhuyenmai.CurrentRow == null) { MessageBox.Show("Vui lòng chọn khuyến mãi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                int maKm = Convert.ToInt32(dgvkhuyenmai.CurrentRow.Cells["MaKm"].Value);
                if (string.IsNullOrWhiteSpace(txttenkm.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!decimal.TryParse(txtgiatri.Text, out decimal giaTri) || giaTri < 0 || giaTri > 100) { MessageBox.Show("Giá trị phải từ 0-100!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (MessageBox.Show("Xác nhận cập nhật khuyến mãi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                // 1. Gọi KhoTruyVan
                bool success = _khoTruyVan.CapNhatKhuyenMai(
                    maKm,
                    txttenkm.Text.Trim(),
                    txtmota.Text.Trim(),
                    cmbloaikm.SelectedItem?.ToString() ?? "HoaDon",
                    giaTri,
                    DateOnly.FromDateTime(dtpngaybatdau.Value),
                    DateOnly.FromDateTime(dtpngayketthuc.Value),
                    cmbtrangthaikm.SelectedItem?.ToString() ?? "Đang áp dụng"
                );

                // 2. Xử lý kết quả
                if (success) {
                    MessageBox.Show("Cập nhật khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhuyenMaiData();
                    gbkhuyenmai.Visible = false;
                }
                else {
                    MessageBox.Show("Cập nhật thất bại! Không tìm thấy KM.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi cập nhật khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void btnxoakm_Click_1(object sender, EventArgs e) // Xóa KM
        {
            try {
                // (Validate input giữ nguyên)
                if (dgvkhuyenmai.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn khuyến mãi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (MessageBox.Show($"Xác nhận xóa {dgvkhuyenmai.SelectedRows.Count} khuyến mãi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                // 1. Lấy danh sách MaKm cần xóa
                List<int> maKmList = new List<int>();
                foreach (DataGridViewRow row in dgvkhuyenmai.SelectedRows) {
                    maKmList.Add(Convert.ToInt32(row.Cells["MaKm"].Value));
                }

                // 2. Gọi KhoTruyVan
                bool success = _khoTruyVan.XoaKhuyenMai(maKmList);

                // 3. Xử lý kết quả
                if (success) {
                    MessageBox.Show("Xóa khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhuyenMaiData();
                    gbkhuyenmai.Visible = false;
                }
                else {
                    MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi xóa khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void button3_Click_1(object sender, EventArgs e) // Sửa thông tin NV (Chức vụ/Vai trò)
        {
            try {
                // (Validate input giữ nguyên)
                if (string.IsNullOrEmpty(txtmanv.Text)) { MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                int maNv = Convert.ToInt32(txtmanv.Text);
                string tenDangNhap = txttentaikhoan.Text;
                if (tenDangNhap == "Chưa có") { MessageBox.Show("Nhân viên này chưa có tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (MessageBox.Show($"Xác nhận cập nhật cho: {txthoten.Text}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                // 1. Gọi KhoTruyVan
                bool success = _khoTruyVan.CapNhatNhanVien(
                    maNv,
                    txtchucvu.Text.Trim(),
                    txtvaitro.Text.Trim()
                );

                // 2. Xử lý kết quả
                if (success) {
                    MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmployeeData();
                }
                else {
                    MessageBox.Show("Cập nhật thất bại! (Lỗi: Không tìm thấy NV hoặc Vai trò)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi cập nhật nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *** ĐÃ TÁCH CSDL ***
        private void button3_Click(object sender, EventArgs e) // Xóa Nguyên Liệu
        {
            try {
                // (Validate input giữ nguyên)
                if (dgvkho.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn nguyên liệu để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                if (MessageBox.Show($"Bạn có chắc muốn xóa {dgvkho.SelectedRows.Count} nguyên liệu đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                // 1. Lấy danh sách MaNl cần xóa
                List<int> maNlList = new List<int>();
                foreach (DataGridViewRow row in dgvkho.SelectedRows) {
                    maNlList.Add(Convert.ToInt32(row.Cells["MaNl"].Value));
                }

                // 2. Gọi KhoTruyVan
                string ketQua = _khoTruyVan.XoaNguyenLieu(maNlList);

                // 3. Hiển thị kết quả
                MessageBox.Show(ketQua, "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadInventoryData();
                gbthongtinnguyenlieu.Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show("Đã xảy ra lỗi nghiêm trọng khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}