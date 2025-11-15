using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    // Form quản trị (Admin) — chịu trách nhiệm hiển thị và thao tác với:
    // - Thống kê tổng quan, doanh thu
    // - Quản lý nhân viên / tài khoản
    // - Quản lý kho (nguyên liệu)
    // - Quản lý sản phẩm
    // - Quản lý khuyến mãi
    public partial class Admin : Form
    {
        // === THAY ĐỔI: Khai báo 5 Repository mới ===
        // Các trường này là các lớp "KhoTruyVan"/service giúp tách repository ra khỏi UI.
        private readonly KhoSp_Nl _repoKho;               // Truy vấn kho nguyên liệu
        private readonly KhuyenMai_function _repoKM;      // Truy vấn khuyến mãi
        private readonly NhanVien_function _repoNhanVien; // Truy vấn nhân viên / tài khoản / vai trò
        private readonly SanPham_function _repoSanPham;   // Truy vấn sản phẩm
        private readonly ThongKe _repoThongKe;           // Truy vấn thống kê, doanh thu

        // Id nhân viên đang được chọn trong bảng (dùng khi sửa/xóa)
        private int selectedEmployeeId = 0;

        // Constructor của form Admin:
        // - Khởi tạo UI component
        // - Khởi tạo các repository
        // - Đăng ký các sự kiện của DataGridView / Tab / các control
        public Admin()
        {
            InitializeComponent();

            // === THAY ĐỔI: Khởi tạo 5 Repository ===
            _repoKho = new KhoSp_Nl();                // Tạo instance xử lý dữ liệu kho
            _repoKM = new KhuyenMai_function();       // Tạo instance xử lý KM
            _repoNhanVien = new NhanVien_function();  // Tạo instance xử lý NV
            _repoSanPham = new SanPham_function();    // Tạo instance xử lý SP
            _repoThongKe = new ThongKe();            // Tạo instance xử lý thống kê

            // (Các sự kiện giữ nguyên)
            this.StartPosition = FormStartPosition.CenterScreen;           // Đặt form giữa màn hình
            dtgvquanlynhanvien.CellClick += dataGridView1_CellClick;      // Khi click trên DGV NV -> mở chi tiết
            dtgvquanlynhanvien.KeyDown += dataGridView1_KeyDown;         // Khi nhấn phím ở DGV NV
            dgvkho.CellClick += dgvInventory_CellClick;                   // Khi click DGV kho -> mở chi tiết
            dgvkho.KeyDown += dgvInventory_KeyDown;                      // Phím ở DGV kho
            CreateSaveButton();                                          // Placeholder (không làm gì hiện tại)
            pnthongtinnv.Visible = false;                                // Ẩn panel thông tin NV lúc đầu
            dgvSanPham.CellClick += dgvSanPham_CellClick;                // Sự kiện click sản phẩm
            dgvSanPham.KeyDown += dgvSanPham_KeyDown;                    // Sự kiện phím cho sản phẩm
            tabControlKho.SelectedIndexChanged += TabControlKho_SelectedIndexChanged; // Chuyển tab xử lý
            dgvkhuyenmai.CellClick += dgvkhuyenmai_CellClick;            // Click KM -> chi tiết
            dgvkhuyenmai.KeyDown += dgvkhuyenmai_KeyDown;                // Phím ở KM
        }

        // Hàm placeholder cho việc tạo nút lưu (hiện không chứa logic)
        private void CreateSaveButton() { }

        // Sự kiện khi click 1 hàng trong DataGridView Sản phẩm:
        // - Nếu dòng hợp lệ -> hiển thị chi tiết sản phẩm ở panel bên
        private void dgvSanPham_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) { ShowProductDetailInPanel(e.RowIndex); }
        }

        // Sự kiện khi dùng phím (Enter) ở DataGridView Sản phẩm:
        // - Khi nhấn Enter trên hàng hiện tại -> cũng mở chi tiết
        private void dgvSanPham_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvSanPham.CurrentRow != null)
            {
                e.Handled = true;
                ShowProductDetailInPanel(dgvSanPham.CurrentRow.Index);
            }
        }

        // --- LOAD SẢN PHẨM VÀ ĐIỀU CHỈNH GIAO DIỆN ---
        // Lấy dữ liệu sản phẩm từ repository và gán vào dgvSanPham,
        // rồi format cột và tô màu theo trạng thái.
        private void LoadProductData()
        {
            try
            {
                var products = _repoSanPham.TaiDuLieuSanPham(); // Gọi repo lấy danh sách sản phẩm DTO
                dgvSanPham.DataSource = products;               // Gán làm nguồn dữ liệu cho DataGridView

                // Áp style chung cho DataGridView
                StyleDataGridView(dgvSanPham);

                // Đặt tên cột rõ ràng cho người dùng
                dgvSanPham.Columns["MaSp"].HeaderText = "Mã SP";
                dgvSanPham.Columns["TenSp"].HeaderText = "Tên sản phẩm";
                dgvSanPham.Columns["LoaiSp"].HeaderText = "Loại";
                dgvSanPham.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvSanPham.Columns["DonVi"].HeaderText = "Đơn vị";
                dgvSanPham.Columns["TrangThai"].HeaderText = "Trạng thái";

                // Format hiển thị số (Đơn giá)
                dgvSanPham.Columns["DonGia"].DefaultCellStyle.Format = "N0";

                // Duyệt qua từng hàng để tô màu dựa trên cột "TrangThai"
                for (int i = 0; i < dgvSanPham.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvSanPham.Rows[i];
                    if (row.Cells["TrangThai"].Value != null)
                    {
                        string trangThai = row.Cells["TrangThai"].Value.ToString();
                        // Nếu còn bán -> nền xanh nhẹ, chữ xanh đậm
                        if (trangThai == "Còn bán")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(223, 240, 216);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        }
                        // Nếu ngừng bán -> nền đỏ nhạt, chữ đỏ
                        else if (trangThai == "Ngừng bán")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(242, 222, 222);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Báo lỗi người dùng nếu có ngoại lệ khi load dữ liệu
                MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện load Form Admin: khởi tạo dữ liệu tổng quan
        private void Admin_Load(object sender, EventArgs e)
        {
            LoadOverviewData();
        }

        // Khi đổi tab điều khiển chính (bandieukhien) thì tải dữ liệu tương ứng
        private void tabControl1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            switch (bandieukhien.SelectedIndex)
            {
                case 0: LoadOverviewData(); break;
                case 1:
                    LoadEmployeeData();
                    pnthongtinnv.Visible = true; // Luôn hiện panel khi vào tab nhân viên
                    ClearEmployeeForm(); // Reset form khi vào tab
                    break;
                case 2: LoadInventoryData(); gbthongtinnguyenlieu.Visible = false; gbsanpham.Visible = false; break;
                case 3: LoadRevenueData(); break;
                case 4: LoadKhuyenMaiData(); gbkhuyenmai.Visible = false; break;
            }
        }

        // --- THỐNG KÊ TỔNG QUAN ---
        // Gọi _repoThongKe để lấy dữ liệu tổng quan và bind vào DataGridView
        private void LoadOverviewData()
        {
            try
            {
                List<DuLieuTongQuan> overviewData = _repoThongKe.TaiDuLieuTongQuan(); // Gọi repo lấy DTO
                tongquandulieu.DataSource = overviewData;                             // Gán datasource

                // Format DataGridView rồi đổi tên các cột hiển thị
                StyleDataGridView(tongquandulieu);
                tongquandulieu.Columns["Category"].HeaderText = "Danh mục";
                tongquandulieu.Columns["Count"].HeaderText = "Số lượng";
                tongquandulieu.Columns["Details"].HeaderText = "Chi tiết";
            }
            catch (Exception ex)
            {
                // Thông báo lỗi nếu repo ném ngoại lệ
                MessageBox.Show($"Lỗi khi tải dữ liệu tổng quan:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- TẢI DỮ LIỆU NHÂN VIÊN ---
        // Gọi repository nhân viên để nhận danh sách hiển thị
        private void LoadEmployeeData()
        {
            try
            {
                var employees = _repoNhanVien.TaiDuLieuNhanVien(); // Gọi repo trả về list DTO
                dtgvquanlynhanvien.DataSource = employees;         // Bind vào DataGridView

                // Định dạng hiển thị cột
                StyleDataGridView(dtgvquanlynhanvien);
                dtgvquanlynhanvien.Columns["MaNv"].HeaderText = "Mã NV";
                dtgvquanlynhanvien.Columns["TenNv"].HeaderText = "Họ tên";
                dtgvquanlynhanvien.Columns["ChucVu"].HeaderText = "Chức vụ";
                dtgvquanlynhanvien.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dtgvquanlynhanvien.Columns["TaiKhoan"].HeaderText = "Tài khoản";
                dtgvquanlynhanvien.Columns["VaiTro"].HeaderText = "Vai trò";
                dtgvquanlynhanvien.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- TẢI DỮ LIỆU KHO NGUYÊN LIỆU ---
        // Gọi repository kho để lấy danh sách nguyên liệu (đã sắp, tính trạng thái)
        private void LoadInventoryData()
        {
            try
            {
                var sortedList = _repoKho.TaiDuLieuKho(); // Gọi repo trả về DTO danh sách kho
                dgvkho.DataSource = sortedList;           // Bind vào DataGridView kho

                // Áp style và đổi tên cột hiển thị
                StyleDataGridView(dgvkho);
                dgvkho.Columns["MaNl"].HeaderText = "Mã NL";
                dgvkho.Columns["TenNl"].HeaderText = "Tên nguyên liệu";
                dgvkho.Columns["DonViTinh"].HeaderText = "Đơn vị";
                dgvkho.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
                dgvkho.Columns["NguongCanhBao"].HeaderText = "Ngưỡng cảnh báo";
                dgvkho.Columns["TinhTrang"].HeaderText = "Trạng thái";

                // Duyệt từng hàng để tô màu theo tình trạng kho
                for (int i = 0; i < dgvkho.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvkho.Rows[i];
                    if (row.Cells["TinhTrang"].Value != null)
                    {
                        string tinhTrang = row.Cells["TinhTrang"].Value.ToString();
                        if (tinhTrang == "Dồi dào")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(223, 240, 216);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        }
                        else if (tinhTrang == "Cảnh báo")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(156, 87, 0);
                        }
                        else if (tinhTrang == "Hết hàng")
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(183, 28, 28);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- TẢI DỮ LIỆU DOANH THU / THỐNG KÊ ---
        private void LoadRevenueData()
        {
            try
            {
                var sortedData = _repoThongKe.TaiDuLieuDoanhThu(); // Gọi repo trả về DTO doanh thu
                dgvdoanhthu.DataSource = sortedData;

                StyleDataGridView(dgvdoanhthu);
                dgvdoanhthu.Columns["Thang"].HeaderText = "Tháng";
                dgvdoanhthu.Columns["SoDonHang"].HeaderText = "Số đơn hàng";
                dgvdoanhthu.Columns["TongDoanhThu"].HeaderText = "Tổng doanh thu";
                dgvdoanhthu.Columns["DoanhThuTrungBinh"].HeaderText = "TB/Đơn";
                dgvdoanhthu.Columns["DonHangLonNhat"].HeaderText = "Cao nhất";
                dgvdoanhthu.Columns["DonHangNhoNhat"].HeaderText = "Thấp nhất";

                // Format các cột số lớn
                dgvdoanhthu.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
                dgvdoanhthu.Columns["DoanhThuTrungBinh"].DefaultCellStyle.Format = "N0";
                dgvdoanhthu.Columns["DonHangLonNhat"].DefaultCellStyle.Format = "N0";
                dgvdoanhthu.Columns["DonHangNhoNhat"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu doanh thu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // StyleDataGridView: chuẩn hóa giao diện cho tất cả DataGridView sử dụng trong form
        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 181, 246);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // (Các sự kiện DGV CellClick/KeyDown giữ nguyên)
        private void dataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) { ShowEmployeeDetailInPanel(e.RowIndex); }
        }
        private void dataGridView1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dtgvquanlynhanvien.CurrentRow != null) { e.Handled = true; ShowEmployeeDetailInPanel(dtgvquanlynhanvien.CurrentRow.Index); }
        }
        private void dgvInventory_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) { ShowInventoryDetailInPanel(e.RowIndex); }
        }
        private void dgvInventory_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvkho.CurrentRow != null) { e.Handled = true; ShowInventoryDetailInPanel(dgvkho.CurrentRow.Index); }
        }


        // --- HIỂN THỊ CHI TIẾT SẢN PHẨM TRONG PANEL ---
        // Lấy MaSp từ hàng được chọn, gọi repo để lấy chi tiết và gán vào các TextBox trong group sản phẩm
        private void ShowProductDetailInPanel(int rowIndex)
        {
            try
            {
                DataGridViewRow row = dgvSanPham.Rows[rowIndex];
                if (row.Cells["MaSp"].Value == null) return;
                int maSp = Convert.ToInt32(row.Cells["MaSp"].Value);

                SanPham product = _repoSanPham.LayChiTietSanPham(maSp); // Gọi repo lấy chi tiết

                if (product != null)
                {
                    // Gán các trường vào UI
                    txtmasp.Text = product.MaSp.ToString();
                    txttensp.Text = product.TenSp;
                    txtgia.Text = product.DonGia.ToString("N0");
                    txtdongia.Text = product.DonGia.ToString("N0");
                    txtloai.Text = product.LoaiSp ?? "";
                    txtdon_vi.Text = product.DonVi ?? "";
                    txttinhtrangsp.Text = product.TrangThai ?? "Còn kinh doanh";

                    // Thiết lập trạng thái read-only / editable cho các control
                    txtmasp.ReadOnly = true;
                    txttensp.ReadOnly = false;
                    txtloai.ReadOnly = false;
                    txtdongia.ReadOnly = false;
                    txtdon_vi.ReadOnly = false;
                    txtgia.ReadOnly = false;
                    txttinhtrangsp.ReadOnly = false;

                    // Hiển thị nhóm thông tin sản phẩm và focus vào tên để người dùng sửa nhanh
                    gbsanpham.Visible = true;
                    gbsanpham.BringToFront();
                    txttensp.Focus();
                    txttensp.SelectAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị chi tiết sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- HIỂN THỊ CHI TIẾT NHÂN VIÊN TRONG PANEL ---
        // Lấy MaNv từ hàng chọn, gọi repo lấy chi tiết và gán lên các textbox
        private void ShowEmployeeDetailInPanel(int rowIndex)
        {
            try
            {
                DataGridViewRow row = dtgvquanlynhanvien.Rows[rowIndex];
                if (row.Cells["MaNv"].Value == null) return;
                int maNv = Convert.ToInt32(row.Cells["MaNv"].Value);
                selectedEmployeeId = maNv; // Lưu id đang chọn

                var nhanVien = _repoNhanVien.LayChiTietNhanVien(maNv); // Gọi repo

                if (nhanVien != null)
                {
                    // Gán dữ liệu NV lên giao diện
                    txtmanv.Text = nhanVien.MaNv.ToString();
                    txthoten.Text = nhanVien.TenNv;
                    txtsdt.Text = string.IsNullOrEmpty(nhanVien.SoDienThoai) ? "Chưa cập nhật" : nhanVien.SoDienThoai;
                    cb_chucvu.DataSource = new[] { nhanVien.ChucVu };
                    cb_chucvu.SelectedIndex = 0;
                    cb_chucvu.Enabled = false;

                    // Nếu NV có tài khoản, hiển thị thông tin tài khoản
                    if (nhanVien.TaiKhoan != null)
                    {
                        txttentaikhoan.Text = nhanVien.TaiKhoan.TenDangNhap;
                        txtmatkhau.Text = nhanVien.TaiKhoan.MatKhau;
                        if (nhanVien.TaiKhoan.MaVaiTroNavigation != null)
                        {
                            cbvaitro.DataSource = new[] { nhanVien.TaiKhoan.MaVaiTroNavigation };
                            cbvaitro.DisplayMember = "TenVaiTro";
                            cbvaitro.ValueMember = "MaVaiTro";
                            cbvaitro.SelectedIndex = 0;
                            cbvaitro.Enabled = false;
                        }
                        else
                        {
                            cbvaitro.DataSource = null;
                            cbvaitro.Enabled = false;
                        }
                    }
                    else
                    {
                        txttentaikhoan.Text = "Chưa có";
                        txtmatkhau.Text = "";
                        cbvaitro.DataSource = null;
                        cbvaitro.Enabled = false;
                    }

                    txtmatkhau.ReadOnly = true;
                    txtmatkhau.UseSystemPasswordChar = true;
                    txthoten.ReadOnly = false;
                    txtsdt.ReadOnly = false;
                    pnthongtinnv.Visible = true; // Hiện panel thông tin NV
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- ĐỔI MẬT KHẨU (BUTTON) ---
        // Mở một dialog nhỏ để nhập mật khẩu hiện tại / mới / xác nhận rồi gọi repo để đổi
        private void btndoimatkhau_Click(object sender, EventArgs e) // Đổi mật khẩu
        {
            try
            {
                // Kiểm tra đã chọn nhân viên chưa
                if (string.IsNullOrEmpty(txtmanv.Text)) { MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string tenDangNhap = txttentaikhoan.Text;
                if (tenDangNhap == "Chưa có") { MessageBox.Show("Nhân viên này chưa có tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                // Tạo Form prompt tạm để nhập mật khẩu
                using (Form promptForm = new Form())
                {
                    // (Code tạo Form Prompt giữ nguyên)
                    promptForm.Width = 450;
                    promptForm.Height = 250;
                    promptForm.Text = "Đổi mật khẩu";
                    promptForm.StartPosition = FormStartPosition.CenterParent;
                    promptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    promptForm.MaximizeBox = false;
                    promptForm.MinimizeBox = false;
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

                    // Hiển thị dialog và xử lý khi người dùng nhấn Lưu
                    if (promptForm.ShowDialog() == DialogResult.OK)
                    {
                        // Validate nhập
                        if (string.IsNullOrWhiteSpace(txtOldPass.Text)) { MessageBox.Show("Vui lòng nhập mật khẩu hiện tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                        if (string.IsNullOrWhiteSpace(txtNewPass.Text)) { MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                        if (txtNewPass.Text.Length < 3) { MessageBox.Show("Mật khẩu mới phải có ít nhất 3 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                        if (txtNewPass.Text != txtConfirmPass.Text) { MessageBox.Show("Mật khẩu xác nhận không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                        // *** THAY ĐỔI: Gọi _repoNhanVien để đổi mật khẩu ***
                        // Trả về mã: 0 = OK, 2 = sai mật khẩu hiện tại, 3 = không tìm thấy tài khoản, khác = lỗi
                        int ketQua = _repoNhanVien.DoiMatKhau(tenDangNhap, txtOldPass.Text, txtNewPass.Text);

                        // Xử lý kết quả trả về
                        if (ketQua == 0)
                        {
                            MessageBox.Show($"Đổi mật khẩu thành công cho: {tenDangNhap}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtmatkhau.Text = txtNewPass.Text;
                            LoadEmployeeData(); // Reload danh sách NV để cập nhật hiển thị
                        }
                        else if (ketQua == 2)
                        {
                            MessageBox.Show("Mật khẩu hiện tại không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (ketQua == 3)
                        {
                            MessageBox.Show("Không tìm thấy tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Đã xảy ra lỗi khi đổi mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đổi mật khẩu:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Đảm bảo textbox mật khẩu ở trạng thái an toàn (ẩn ký tự)
                if (txtmatkhau != null)
                {
                    txtmatkhau.ReadOnly = true;
                    txtmatkhau.UseSystemPasswordChar = true;
                }
            }
        }

        // --- HIỂN THỊ CHI TIẾT NGUYÊN LIỆU ---
        // Lấy MaNl, gọi repo để lấy thông tin nguyên liệu và gán lên UI
        private void ShowInventoryDetailInPanel(int rowIndex)
        {
            try
            {
                DataGridViewRow row = dgvkho.Rows[rowIndex];
                if (row.Cells["MaNl"].Value == null) return;
                int maNl = Convert.ToInt32(row.Cells["MaNl"].Value);

                NguyenLieu ingredient = _repoKho.LayChiTietNguyenLieu(maNl); // Gọi repo

                if (ingredient != null)
                {
                    // Gán dữ liệu vào các textbox
                    txtma.Text = ingredient.MaNl.ToString();
                    txtten.Text = ingredient.TenNl;
                    txtsoluong.Text = ingredient.SoLuongTon?.ToString() ?? "0";
                    txtdonvi.Text = ingredient.DonViTinh;
                    textBox2.Text = ingredient.TrangThai ?? "Đang kinh doanh";

                    // Thiết lập trạng thái editable/readonly cho form cập nhật
                    txtma.ReadOnly = true;
                    txtten.ReadOnly = true;
                    txtdonvi.ReadOnly = true;
                    txtsoluong.ReadOnly = false;
                    textBox2.ReadOnly = false;
                    txtsoluong.Focus();
                    txtsoluong.SelectAll();

                    // Ghi Tag để biết đang ở chế độ cập nhật (lblcapnhat.Tag != null)
                    lblcapnhat.Tag = maNl;
                    lblcapnhat.Text = "Cập nhật";
                    gbthongtinnguyenlieu.Visible = true;
                    gbthongtinnguyenlieu.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị chi tiết nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- LƯU (CẬP NHẬT HOẶC THÊM) NGUYÊN LIỆU ---
        // Nếu lblcapnhat.Tag != null -> UPDATE, ngược lại -> ADD NEW
        private void lblcapnhat_Click(object sender, EventArgs e) // Lưu (Nguyên liệu)
        {
            try
            {
                // Validate input số lượng
                if (!decimal.TryParse(txtsoluong.Text, out decimal soLuongMoi) || soLuongMoi < 0) { MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtsoluong.Focus(); return; }
                string tinhTrangMoi = textBox2.Text.Trim();

                if (lblcapnhat.Tag != null) // Chế độ UPDATE
                {
                    int maNl = (int)lblcapnhat.Tag;
                    if (MessageBox.Show($"Xác nhận CẬP NHẬT '{txtten.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                    // Gọi repo để cập nhật số lượng và trạng thái
                    bool success = _repoKho.CapNhatNguyenLieu(maNl, soLuongMoi, tinhTrangMoi);
                    if (success) { MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else { MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else // Chế độ ADD NEW
                {
                    // Validate các trường bắt buộc
                    if (string.IsNullOrWhiteSpace(txtten.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtten.Focus(); return; }
                    if (string.IsNullOrWhiteSpace(txtdonvi.Text)) { MessageBox.Show("Vui lòng nhập đơn vị!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtdonvi.Focus(); return; }
                    if (MessageBox.Show($"Xác nhận THÊM MỚI '{txtten.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                    // Gọi repo để thêm nguyên liệu mới
                    var newIngredient = _repoKho.ThemNguyenLieu(txtten.Text.Trim(), txtdonvi.Text.Trim(), soLuongMoi, tinhTrangMoi);
                    if (newIngredient != null) { MessageBox.Show($"Thêm thành công! Mã mới: {newIngredient.MaNl}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else { MessageBox.Show("Thêm mới thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }

                // Reload danh sách kho và dọn form
                LoadInventoryData();
                ClearInventoryForm();
                gbthongtinnguyenlieu.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // (Giữ nguyên) - Hiển thị form thêm mới nguyên liệu, đặt mặc định và focus vào tên
        private void lblthem_Click(object sender, EventArgs e) // Thêm mới (Nguyên liệu)
        {
            ClearInventoryForm();
            txtma.ReadOnly = true;
            txtma.Text = "(Tự động)";
            txtten.ReadOnly = false;
            txtsoluong.ReadOnly = false;
            txtdonvi.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox2.Text = "Đang kinh doanh";
            lblcapnhat.Tag = null;
            lblcapnhat.Text = "Lưu Mới";
            gbthongtinnguyenlieu.Visible = true;
            gbthongtinnguyenlieu.BringToFront();
            txtten.Focus();
        }

        // Xóa dữ liệu trong form nguyên liệu (reset các textbox)
        private void ClearInventoryForm()
        {
            txtma.Clear();
            txtten.Clear();
            txtsoluong.Clear();
            txtdonvi.Clear();
            textBox2.Clear();
            lblcapnhat.Tag = null;
            lblcapnhat.Text = "Cập nhật";
        }

        // --- THÊM SẢN PHẨM ---
        // Validate input và gọi _repoSanPham.ThemSanPham(...) để lưu
        private void btnthem_Click(object sender, EventArgs e) // Thêm Sản Phẩm
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txttensp.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txttensp.Focus(); return; }
                if (string.IsNullOrWhiteSpace(txtdongia.Text) || !decimal.TryParse(txtdongia.Text.Replace(".", "").Replace(",", ""), out decimal donGia)) { MessageBox.Show("Vui lòng nhập đơn giá hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtdongia.Focus(); return; }
                if (MessageBox.Show($"Xác nhận THÊM MỚI '{txttensp.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                // Gọi repo để thêm sản phẩm mới với thông tin từ form
                var newProduct = _repoSanPham.ThemSanPham(
                    txttensp.Text.Trim(),
                    txtloai.Text.Trim(),
                    donGia,
                    txtdon_vi.Text.Trim(),
                    txttinhtrangsp.Text.Trim()
                );

                if (newProduct != null)
                {
                    MessageBox.Show($"Thêm sản phẩm thành công! Mã mới: {newProduct.MaSp}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductData();     // Reload danh sách sản phẩm sau khi thêm
                    gbsanpham.Visible = false;
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- SỬA SẢN PHẨM ---
        // Lấy mã SP từ textbox, validate, gọi repo cập nhật rồi reload
        private void btnsua_Click(object sender, EventArgs e) // Sửa Sản Phẩm
        {
            try
            {
                if (string.IsNullOrEmpty(txtmasp.Text)) { MessageBox.Show("Vui lòng chọn sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                int maSp = Convert.ToInt32(txtmasp.Text);
                if (string.IsNullOrWhiteSpace(txttensp.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txttensp.Focus(); return; }
                if (string.IsNullOrWhiteSpace(txtdongia.Text) || !decimal.TryParse(txtdongia.Text.Replace(".", "").Replace(",", ""), out decimal donGia)) { MessageBox.Show("Vui lòng nhập đơn giá hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtdongia.Focus(); return; }
                if (MessageBox.Show($"Xác nhận CẬP NHẬT '{txttensp.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                // Gọi repo cập nhật sản phẩm
                var updatedProduct = _repoSanPham.CapNhatSanPham(
                    maSp,
                    txttensp.Text.Trim(),
                    txtloai.Text.Trim(),
                    donGia,
                    txtdon_vi.Text.Trim(),
                    txttinhtrangsp.Text.Trim()
                );

                if (updatedProduct != null)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductData();
                    gbsanpham.Visible = false;
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại! Không tìm thấy SP.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- XÓA SẢN PHẨM ---
        // Gọi repo để xóa sản phẩm theo mã, xử lý kết quả trả về
        private void btnxoa_Click(object sender, EventArgs e) // Xóa Sản Phẩm
        {
            try
            {
                if (string.IsNullOrEmpty(txtmasp.Text)) { MessageBox.Show("Vui lòng chọn sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                int maSp = Convert.ToInt32(txtmasp.Text);
                if (MessageBox.Show($"Xác nhận XÓA '{txttensp.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                bool success = _repoSanPham.XoaSanPham(maSp); // Gọi repo

                if (success)
                {
                    MessageBox.Show("Đã xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductData();
                    gbsanpham.Visible = false;
                }
                else
                {
                    MessageBox.Show("Xóa thất bại! Sản phẩm có thể đang được sử dụng trong công thức hoặc đơn hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.InnerException?.Message ?? ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Khi thay đổi tab kho/sản phẩm trong phần quản lý kho
        private void TabControlKho_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                if (tabControlKho.SelectedIndex == 0)
                {
                    LoadInventoryData();
                    gbthongtinnguyenlieu.Visible = false;
                }
                else if (tabControlKho.SelectedIndex == 1)
                {
                    LoadProductData();
                    gbsanpham.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chuyển tab: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- TẢI DỮ LIỆU KHUYẾN MÃI ---
        private void LoadKhuyenMaiData()
        {
            try
            {
                var khuyenMais = _repoKM.TaiDuLieuKhuyenMai(); // Gọi repo trả về DTO KM
                dgvkhuyenmai.DataSource = khuyenMais;

                StyleDataGridView(dgvkhuyenmai);
                dgvkhuyenmai.Columns["MaKm"].HeaderText = "Mã KM";
                dgvkhuyenmai.Columns["TenKm"].HeaderText = "Tên khuyến mãi";
                dgvkhuyenmai.Columns["MoTa"].HeaderText = "Mô tả";
                dgvkhuyenmai.Columns["LoaiKm"].HeaderText = "Loại KM";
                dgvkhuyenmai.Columns["GiaTri"].HeaderText = "Giá trị (%)";
                dgvkhuyenmai.Columns["NgayBatDau"].HeaderText = "Ngày bắt đầu";
                dgvkhuyenmai.Columns["NgayKetThuc"].HeaderText = "Ngày kết thúc";
                dgvkhuyenmai.Columns["TrangThai"].HeaderText = "Trạng thái";

                // Format hiển thị ngày
                dgvkhuyenmai.Columns["NgayBatDau"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvkhuyenmai.Columns["NgayKetThuc"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // (Giữ nguyên) - mở chi tiết khuyến mãi khi click
        private void dgvkhuyenmai_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) { ShowKhuyenMaiDetailInPanel(e.RowIndex); }
        }

        // (Giữ nguyên) - phím Enter ở DGV KM -> mở chi tiết
        private void dgvkhuyenmai_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvkhuyenmai.CurrentRow != null) { e.Handled = true; ShowKhuyenMaiDetailInPanel(dgvkhuyenmai.CurrentRow.Index); }
        }

        // Hiển thị chi tiết khuyến mãi khi chọn 1 hàng
        private void ShowKhuyenMaiDetailInPanel(int rowIndex)
        {
            try
            {
                DataGridViewRow row = dgvkhuyenmai.Rows[rowIndex];
                if (row.Cells["MaKm"].Value == null) return;
                int maKm = Convert.ToInt32(row.Cells["MaKm"].Value);

                var khuyenMai = _repoKM.LayChiTietKhuyenMai(maKm); // Gọi repo lấy Models.KhuyenMai

                if (khuyenMai != null)
                {
                    // Gán lên UI
                    txttenkm.Text = khuyenMai.TenKm;
                    txtmota.Text = khuyenMai.MoTa ?? "";
                    cmbloaikm.SelectedItem = khuyenMai.LoaiKm ?? "HoaDon";
                    txtgiatri.Text = khuyenMai.GiaTri.ToString();
                    txtgtkm.Text = "0";
                    dtpngaybatdau.Value = khuyenMai.NgayBatDau.ToDateTime(TimeOnly.MinValue);
                    dtpngayketthuc.Value = khuyenMai.NgayKetThuc.ToDateTime(TimeOnly.MinValue);
                    cmbtrangthaikm.SelectedItem = khuyenMai.TrangThai ?? "Đang áp dụng";
                    gbkhuyenmai.Visible = true;
                    gbkhuyenmai.BringToFront();
                    txttenkm.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị chi tiết khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- TẠO TÀI KHOẢN CHO NHÂN VIÊN ---
        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            // Nếu đang chọn nhân viên (txtmanv khác '(tạo tự động)' và có số) thì reset form để nhập mới
            if (!string.IsNullOrWhiteSpace(txtmanv.Text) && txtmanv.Text != "(tạo tự động)" && int.TryParse(txtmanv.Text, out _))
            {
                ClearEmployeeForm();
                return;
            }

            // Validate
            if (string.IsNullOrWhiteSpace(txthoten.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txthoten.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txttentaikhoan.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txttentaikhoan.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txtmatkhau.Text) || txtmatkhau.Text.Length < 3)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 3 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmatkhau.Focus(); return;
            }
            if (cbvaitro.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbvaitro.Focus(); return;
            }
            if (cb_chucvu.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn chức vụ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cb_chucvu.Focus(); return;
            }
            // Validate số điện thoại nếu nhập
            if (!string.IsNullOrWhiteSpace(txtsdt.Text))
            {
                string phone = txtsdt.Text.Trim();
                if (phone.Length < 10 || phone.Length > 11 || !phone.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtsdt.Focus(); return;
                }
            }
            // Kiểm tra trùng tên đăng nhập
            var taiKhoans = _repoNhanVien.LayDanhSachTaiKhoan();
            if (taiKhoans.Any(t => t.TenDangNhap == txttentaikhoan.Text.Trim()))
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txttentaikhoan.Focus(); return;
            }
            var selectedVaiTro = (VaiTro)cbvaitro.SelectedItem;
            string chucVu = cb_chucvu.SelectedItem.ToString();
            // Luôn tạo mới nhân viên và tài khoản
            int maNv = _repoNhanVien.ThemNhanVien(txthoten.Text.Trim(), chucVu, txtsdt.Text.Trim());
            bool success = false;
            if (maNv > 0)
            {
                success = _repoNhanVien.TaoTaiKhoan(maNv, txttentaikhoan.Text.Trim(), txtmatkhau.Text, selectedVaiTro.MaVaiTro);
            }
            if (success)
            {
                MessageBox.Show("Tạo tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadEmployeeData();
                ClearEmployeeForm();
            }
            else
            {
                MessageBox.Show("Tạo tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- XÓA TÀI KHOẢN NHÂN VIÊN ---
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtgvquanlynhanvien.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                int maNv = Convert.ToInt32(dtgvquanlynhanvien.SelectedRows[0].Cells["MaNv"].Value);
                string tenNv = dtgvquanlynhanvien.SelectedRows[0].Cells["TenNv"].Value.ToString();
                string taiKhoan = dtgvquanlynhanvien.SelectedRows[0].Cells["TaiKhoan"].Value.ToString();
                if (taiKhoan == "Chưa có") { MessageBox.Show("Nhân viên này chưa có tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                if (MessageBox.Show($"Xác nhận xóa tài khoản của: {tenNv}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                // Gọi repo để xóa tài khoản theo MaNv
                bool success = _repoNhanVien.XoaTaiKhoan(maNv);

                if (success)
                {
                    MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmployeeData(); // Reload để cập nhật UI
                }
                else
                {
                    MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- ĐĂNG XUẤT (Đóng form Admin để quay về Login) ---
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) { this.Close(); }
        }

        // --- THOÁT ỨNG DỤNG ---
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) { Application.Exit(); }
        }

        // (Các hàm trống/placeholder giữ nguyên)
        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void lblthongtinnhanvien_Click(object sender, EventArgs e) { }
        private void txtgtkm_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }

        // --- THÊM KHUYẾN MÃI ---
        private void btnthemkm_Click_1(object sender, EventArgs e) // Thêm KM
        {
            try
            {
                // Validate tên và giá trị khuyến mãi
                if (string.IsNullOrWhiteSpace(txttenkm.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txttenkm.Focus(); return; }
                if (!decimal.TryParse(txtgiatri.Text, out decimal giaTri) || giaTri < 0 || giaTri > 100) { MessageBox.Show("Giá trị phải từ 0-100!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtgiatri.Focus(); return; }
                if (MessageBox.Show($"Xác nhận THÊM MỚI '{txttenkm.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                // Tạo Models.KhuyenMai mới từ dữ liệu form
                var newKhuyenMai = new Models.KhuyenMai
                {
                    TenKm = txttenkm.Text.Trim(),
                    MoTa = txtmota.Text.Trim(),
                    LoaiKm = cmbloaikm.SelectedItem?.ToString() ?? "HoaDon",
                    GiaTri = giaTri,
                    NgayBatDau = DateOnly.FromDateTime(dtpngaybatdau.Value),
                    NgayKetThuc = DateOnly.FromDateTime(dtpngayketthuc.Value),
                    TrangThai = cmbtrangthaikm.SelectedItem?.ToString() ?? "Đang áp dụng"
                };

                // Gọi repo thêm KM
                bool success = _repoKM.ThemKhuyenMai(newKhuyenMai);

                if (success)
                {
                    MessageBox.Show("Thêm khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhuyenMaiData(); // Reload danh sách KM
                    gbkhuyenmai.Visible = false;
                }
                else
                {
                    MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- SỬA KHUYẾN MÃI ---
        private void btnsuakm_Click_1(object sender, EventArgs e) // Sửa KM
        {
            try
            {
                if (dgvkhuyenmai.CurrentRow == null) { MessageBox.Show("Vui lòng chọn khuyến mãi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                int maKm = Convert.ToInt32(dgvkhuyenmai.CurrentRow.Cells["MaKm"].Value);
                if (string.IsNullOrWhiteSpace(txttenkm.Text)) { MessageBox.Show("Vui lòng nhập tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (!decimal.TryParse(txtgiatri.Text, out decimal giaTri) || giaTri < 0 || giaTri > 100) { MessageBox.Show("Giá trị phải từ 0-100!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (MessageBox.Show("Xác nhận cập nhật khuyến mãi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                // Gọi repo cập nhật KM với dữ liệu từ form
                bool success = _repoKM.CapNhatKhuyenMai(
                    maKm,
                    txttenkm.Text.Trim(),
                    txtmota.Text.Trim(),
                    cmbloaikm.SelectedItem?.ToString() ?? "HoaDon",
                    giaTri,
                    DateOnly.FromDateTime(dtpngaybatdau.Value),
                    DateOnly.FromDateTime(dtpngayketthuc.Value),
                    cmbtrangthaikm.SelectedItem?.ToString() ?? "Đang áp dụng"
                );

                if (success)
                {
                    MessageBox.Show("Cập nhật khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhuyenMaiData();
                    gbkhuyenmai.Visible = false;
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- XÓA KHUYẾN MÃI ---
        private void btnxoakm_Click_1(object sender, EventArgs e) // Xóa KM
        {
            try
            {
                if (dgvkhuyenmai.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn khuyến mãi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (MessageBox.Show($"Xác nhận xóa {dgvkhuyenmai.SelectedRows.Count} khuyến mãi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                // Thu thập danh sách MaKm để gửi cho repo
                List<int> maKmList = new List<int>();
                foreach (DataGridViewRow row in dgvkhuyenmai.SelectedRows)
                {
                    maKmList.Add(Convert.ToInt32(row.Cells["MaKm"].Value));
                }

                bool success = _repoKM.XoaKhuyenMai(maKmList); // Gọi repo

                if (success)
                {
                    MessageBox.Show("Xóa khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhuyenMaiData();
                    gbkhuyenmai.Visible = false;
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CẬP NHẬT NHÂN VIÊN ---
        private void button3_Click_1(object sender, EventArgs e) // Sửa NV
        {
            try
            {
                if (string.IsNullOrEmpty(txtmanv.Text) || txtmanv.Text == "(Tự động)")
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int maNv = Convert.ToInt32(txtmanv.Text);
                if (string.IsNullOrWhiteSpace(txthoten.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txthoten.Focus(); return;
                }
                if (string.IsNullOrWhiteSpace(txtsdt.Text))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtsdt.Focus(); return;
                }
                if (cbvaitro.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn vai trò!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbvaitro.Focus(); return;
                }
                if (cb_chucvu.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn chức vụ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cb_chucvu.Focus(); return;
                }
                // Validate số điện thoại
                string phone = txtsdt.Text.Trim();
                if (phone.Length < 10 || phone.Length > 11 || !phone.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtsdt.Focus(); return;
                }
                var selectedVaiTro = (VaiTro)cbvaitro.SelectedItem;
                string chucVu = cb_chucvu.SelectedItem.ToString();
                if (MessageBox.Show($"Xác nhận cập nhật cho: {txthoten.Text}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                // Gọi repo cập nhật nhân viên
                bool success = _repoNhanVien.CapNhatNhanVienFull(
                    maNv,
                    txthoten.Text.Trim(),
                    phone,
                    chucVu,
                    selectedVaiTro.MaVaiTro
                );
                if (success)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmployeeData();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại! (Lỗi: Không tìm thấy NV hoặc Vai trò)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- XÓA NGUYÊN LIỆU ---
        private void button3_Click(object sender, EventArgs e) // Xóa Nguyên Liệu
        {
            try
            {
                if (dgvkho.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn nguyên liệu để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                if (MessageBox.Show($"Bạn có chắc muốn xóa {dgvkho.SelectedRows.Count} nguyên liệu đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                // Gom mã nguyên liệu được chọn vào danh sách
                List<int> maNlList = new List<int>();
                foreach (DataGridViewRow row in dgvkho.SelectedRows)
                {
                    maNlList.Add(Convert.ToInt32(row.Cells["MaNl"].Value));
                }

                // Gọi repo để xóa, repo trả về chuỗi kết quả (có thể chứa thông báo)
                string ketQua = _repoKho.XoaNguyenLieu(maNlList);

                // Hiển thị kết quả trả về và reload danh sách kho
                MessageBox.Show(ketQua, "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadInventoryData();
                gbthongtinnguyenlieu.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi nghiêm trọng khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- LOAD DANH SÁCH VAI TRÒ VÀ CHỨC VỤ ---
        private void LoadVaiTro_ChucVu()
        {
            // Lấy danh sách vai trò từ repo, loại bỏ Chủ cửa hàng
            var vaiTros = _repoNhanVien.LayDanhSachVaiTro()
                .Where(v => !v.TenVaiTro.ToLower().Contains("chủ cửa hàng") && !v.TenVaiTro.ToLower().Contains("chu cua hang"))
                .ToList();
            cbvaitro.DataSource = vaiTros;
            cbvaitro.DisplayMember = "TenVaiTro";
            cbvaitro.ValueMember = "MaVaiTro";
            cbvaitro.SelectedIndexChanged += cbvaitro_SelectedIndexChanged;
        }
        private void cbvaitro_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbvaitro.SelectedItem == null) { cb_chucvu.DataSource = null; return; }
            var selectedVaiTro = (VaiTro)cbvaitro.SelectedItem;
            if (selectedVaiTro.TenVaiTro.ToLower().Contains("quản lý") || selectedVaiTro.TenVaiTro.ToLower().Contains("quan ly"))
            {
                cb_chucvu.DataSource = new[] { "Quản lý" };
                cb_chucvu.Enabled = false;
            }
            else if (selectedVaiTro.TenVaiTro.ToLower().Contains("nhân viên") || selectedVaiTro.TenVaiTro.ToLower().Contains("nhan vien"))
            {
                cb_chucvu.DataSource = new[] { "Thu Ngân", "Oder" };
                cb_chucvu.Enabled = true;
            }
            else
            {
                cb_chucvu.DataSource = null;
                cb_chucvu.Enabled = false;
            }
        }

        // Đảm bảo khai báo hàm ClearEmployeeForm trong Admin.cs
        private void ClearEmployeeForm()
        {
            // Khi reset form hoặc tạo mới, chỉ hiển thị '(tạo tự động)' và luôn ReadOnly
            txtmanv.Text = "(tạo tự động)";
            txtmanv.ReadOnly = true;
            txtmanv.Visible = true;
            txthoten.Text = "";
            txthoten.ReadOnly = false;
            txtsdt.Text = "";
            txtsdt.ReadOnly = false;
            txttentaikhoan.Text = "";
            txttentaikhoan.ReadOnly = false;
            txtmatkhau.Text = "";
            txtmatkhau.ReadOnly = false;
            txtmatkhau.UseSystemPasswordChar = false;
            cb_chucvu.Enabled = true;
            cb_chucvu.DataSource = null;
            cbvaitro.Enabled = true;
            cbvaitro.DataSource = null;
        }

    }
}