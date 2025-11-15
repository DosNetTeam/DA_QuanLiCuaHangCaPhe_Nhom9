using DA_QuanLiCuaHangCaPhe_Nhom9.Function;
// Thêm namespace của KhoTruyVan
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_QuanLi;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class QuanLi : Form {

        private int _currentMaNV = 0;

        // Khai báo Kho Truy Vấn
        private readonly KhoTruyVanQuanLi _khoTruyVan;

        public QuanLi(int maNv = 0) {
            _currentMaNV = maNv;
            InitializeComponent();

            // Khởi tạo Kho Truy Vấn
            _khoTruyVan = new KhoTruyVanQuanLi();

            // Gắn các sự kiện bổ sung
            this.Load += QuanLi_Load;
            txtTimKiemHD.TextChanged += txtTimKiemHD_TextChanged;
            cbTrangThaiHD.SelectedIndexChanged += cbTrangThaiHD_SelectedIndexChanged;
            txtTimKiemKho.TextChanged += txtTimKiemKho_TextChanged;
            btnThemMoiKho.Click += btnThemMoiKho_Click;

            // Gắn sự kiện cho tab Sản Phẩm
            txtTimKiemSP.TextChanged += txtTimKiemSP_TextChanged;

            dgvSanPham.CellFormatting += dgvSanPham_CellFormatting;

            // Gắn sự kiện cho tab Khuyến mãi
            txtTimKiemKM.TextChanged += txtTimKiemKM_TextChanged;
            cbLocTrangThaiKM.SelectedIndexChanged += cbLocTrangThaiKM_SelectedIndexChanged;

            dgvKhuyenMai.CellFormatting += dgvKhuyenMai_CellFormatting;

            // Gắn CellFormatting
            dgvPerformance.CellFormatting += dgvPerformance_CellFormatting;
            dgvHoaDon.CellFormatting += dgvHoaDon_CellFormatting;
            dgvTonKho.CellFormatting += dgvTonKho_CellFormatting;

            // Wire notify button directly
            if (btnSendNotify != null)
                btnSendNotify.Click += btnSendNotify_Click;
        }

        private void QuanLi_Load(object sender, EventArgs e) {
            SetupFilters();
            LoadData_NhanVien();
            LoadData_HoaDon();
            LoadData_TonKho();
            LoadData_SanPham();
            LoadData_KhuyenMai();
            LoadNotifications();

            // (Code setup UI giữ nguyên)
            try {
                if (grpNotify != null) {
                    grpNotify.Visible = true;
                    grpNotify.BringToFront();
                }
                if (txtNotifyMessage != null) {
                    txtNotifyMessage.Enabled = true;
                    txtNotifyMessage.ReadOnly = false;
                }
                if (btnSendNotify != null) {
                    btnSendNotify.Enabled = true;
                    btnSendNotify.Click -= btnSendNotify_Click;
                    btnSendNotify.Click += btnSendNotify_Click;
                }
            }
            catch { }
        }

        private void PositionNotifyGroup() {

            try {
                if (grpNotify == null || panelContent == null) return;
                int margin = 15;
                int x = Math.Max(margin, panelContent.ClientSize.Width - grpNotify.Width - margin);
                int y = Math.Max(margin, panelContent.ClientSize.Height - grpNotify.Height - margin);
                grpNotify.Location = new Point(x, y);
                grpNotify.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                grpNotify.BringToFront();
            }
            catch { }
        }


        public List<string> GetNotifications() {
            try {
                // Chỉ gọi hàm từ KhoTruyVan
                return _khoTruyVan.TaiThongBao();
            }
            catch (Exception ex) {
                var list = new List<string>();
                list.Add("Lỗi khi tải thông báo: " + ex.Message);
                return list;
            }
        }


        private void LoadNotifications() {
            try {
                // Chỉ gọi hàm GetNotifications đã được refactor
                var notes = GetNotifications();
                if (grpNotify != null && notes != null && notes.Count > 0) {
                    grpNotify.Tag = notes;
                }
            }
            catch { }
        }

        // Cài đặt giá trị ban đầu cho các ComboBox lọc
        private void SetupFilters() {
            // (Code này giữ nguyên)
            cbThang.Items.Clear();
            cbThang.Items.Add("Tất cả");
            for (int i = 1; i <= 12; i++) {
                cbThang.Items.Add($"Tháng {i}");
            }
            cbThang.SelectedIndex = 0;

            cbTrangThaiHD.Items.Clear();
            cbTrangThaiHD.Items.Add("Tất cả");
            cbTrangThaiHD.Items.Add("Đang xu ly");
            cbTrangThaiHD.Items.Add("Đã thanh toán");
            cbTrangThaiHD.Items.Add("Đã hủy");
            cbTrangThaiHD.SelectedIndex = 0;

            cbLocTrangThaiKM.Items.Clear();
            cbLocTrangThaiKM.Items.Add("Tất cả");
            cbLocTrangThaiKM.Items.Add("Đang áp dụng");
            cbLocTrangThaiKM.Items.Add("Đã kết thúc");
            cbLocTrangThaiKM.Items.Add("Sắp diễn ra");
            cbLocTrangThaiKM.SelectedIndex = 0;

            if (string.IsNullOrWhiteSpace(txtTimKiemHD.Text))
                txtTimKiemHD.ForeColor = Color.Gray;
            if (string.IsNullOrWhiteSpace(txtTimKiemKho.Text))
                txtTimKiemKho.ForeColor = Color.Gray;
        }

        #region Các hàm tải dữ liệu (ĐÃ TÁCH CSDL)


        private void LoadData_NhanVien() {
            try {
                int selectedMonth = cbThang.SelectedIndex;

                // 1. Gọi KhoTruyVan để lấy dữ liệu
                var finalData = _khoTruyVan.TaiDuLieuNhanVien(selectedMonth);

                // 2. Gán dữ liệu cho UI
                dgvPerformance.DataSource = finalData;

                if (dgvPerformance.Columns["TenNV"] != null)
                    dgvPerformance.Columns["TenNV"].HeaderText = "Tên Nhân Viên";
                if (dgvPerformance.Columns["SoDon"] != null)
                    dgvPerformance.Columns["SoDon"].HeaderText = "Số Đơn";
                if (dgvPerformance.Columns["TongDoanhThu"] != null)
                    dgvPerformance.Columns["TongDoanhThu"].HeaderText = "Tổng Doanh Thu";
                if (dgvPerformance.Columns["HieuSuat"] != null)
                    dgvPerformance.Columns["HieuSuat"].HeaderText = "Hiệu Suất";

                dgvPerformance.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }


        private void LoadData_HoaDon() {
            try {
                // 1. Lấy tham số từ UI
                string timKiem = txtTimKiemHD.Text?.Trim().ToLower() ?? "";
                string trangThai = cbTrangThaiHD.SelectedItem?.ToString() ?? "Tất cả";
                if (timKiem == "tim kiem ma hd") timKiem = "";

                // 2. Gọi KhoTruyVan
                var finalData = _khoTruyVan.TaiDuLieuHoaDon(timKiem, trangThai);

                // 3. Gán cho UI
                dgvHoaDon.DataSource = finalData;
                dgvHoaDon.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu hóa đơn: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }


        private void LoadData_TonKho() {
            try {
                // 1. Lấy tham số từ UI
                string timKiem = txtTimKiemKho.Text?.Trim().ToLower() ?? "";
                if (timKiem == "tim kiem nguyen lieu") timKiem = "";

                // 2. Gọi KhoTruyVan
                var finalData = _khoTruyVan.TaiDuLieuTonKho(timKiem);

                // 3. Gán cho UI
                dgvTonKho.DataSource = finalData;
                dgvTonKho.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu tồn kho: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }


        private void LoadData_SanPham() {
            try {
                // 1. Lấy tham số từ UI
                string timKiem = txtTimKiemSP.Text?.Trim().ToLower() ?? "";
                if (timKiem == "tìm theo tên sản phẩm") timKiem = "";

                // 2. Gọi KhoTruyVan
                var finalData = _khoTruyVan.TaiDuLieuSanPham(timKiem);

                // 3. Gán cho UI
                dgvSanPham.DataSource = finalData;

                if (dgvSanPham.Columns["MaSp"] != null)
                    dgvSanPham.Columns["MaSp"].Visible = false;
                if (dgvSanPham.Columns["TenSP"] != null)
                    dgvSanPham.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                if (dgvSanPham.Columns["Loai"] != null)
                    dgvSanPham.Columns["Loai"].HeaderText = "Loại";
                if (dgvSanPham.Columns["DonGia"] != null)
                    dgvSanPham.Columns["DonGia"].HeaderText = "Đơn Giá";
                if (dgvSanPham.Columns["DonVi"] != null)
                    dgvSanPham.Columns["DonVi"].HeaderText = "Đơn Vị";
                if (dgvSanPham.Columns["TrangThai"] != null)
                    dgvSanPham.Columns["TrangThai"].HeaderText = "Trạng Thái";

                dgvSanPham.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }


        private void LoadData_KhuyenMai() {
            try {
                // 1. Lấy tham số từ UI
                string trangThai = cbLocTrangThaiKM.SelectedItem?.ToString() ?? "Tất cả";
                string timKiem = txtTimKiemKM.Text?.Trim().ToLower() ?? "";
                if (timKiem == "tìm theo tên km") timKiem = "";

                // 2. Gọi KhoTruyVan
                var finalData = _khoTruyVan.TaiDuLieuKhuyenMai(timKiem, trangThai);

                // 3. Gán cho UI
                dgvKhuyenMai.DataSource = finalData;

                if (dgvKhuyenMai.Columns["MaKm"] != null)
                    dgvKhuyenMai.Columns["MaKm"].Visible = false;
                if (dgvKhuyenMai.Columns["TenKM"] != null)
                    dgvKhuyenMai.Columns["TenKM"].HeaderText = "Tên Khuyến Mãi";
                if (dgvKhuyenMai.Columns["Loai"] != null)
                    dgvKhuyenMai.Columns["Loai"].HeaderText = "Loại";
                if (dgvKhuyenMai.Columns["GiaTri"] != null)
                    dgvKhuyenMai.Columns["GiaTri"].HeaderText = "Giá Trị";
                if (dgvKhuyenMai.Columns["BatDau"] != null)
                    dgvKhuyenMai.Columns["BatDau"].HeaderText = "Bắt Đầu";
                if (dgvKhuyenMai.Columns["KetThuc"] != null)
                    dgvKhuyenMai.Columns["KetThuc"].HeaderText = "Kết Thúc";
                if (dgvKhuyenMai.Columns["TrangThai"] != null)
                    dgvKhuyenMai.Columns["TrangThai"].HeaderText = "Trạng Thái";

                dgvKhuyenMai.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải dữ liệu khuyến mãi: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        #endregion

        #region Các hàm xử lý sự kiện (Event Handlers - Giữ nguyên)

        private void btnLoc_Click(object sender, EventArgs e) {
            LoadData_NhanVien();
        }

        private void txtTimKiemHD_TextChanged(object sender, EventArgs e) {
            LoadData_HoaDon();
        }

        private void cbTrangThaiHD_SelectedIndexChanged(object sender, EventArgs e) {
            LoadData_HoaDon();
        }

        private void txtTimKiemKho_TextChanged(object sender, EventArgs e) {
            LoadData_TonKho();
        }

        private void btnThemMoiKho_Click(object sender, EventArgs e) {
            MessageBox.Show("Chức năng 'Thêm Mới Kho' đang được phát triển!", "Thông báo");
        }

        private void txtTimKiemSP_TextChanged(object sender, EventArgs e) {
            LoadData_SanPham();
        }

        private void btnThemSP_Click(object sender, EventArgs e) {
            MessageBox.Show("Chức năng 'Thêm Sản Phẩm' đang được phát triển!", "Thông báo");
        }

        private void btnSuaSP_Click(object sender, EventArgs e) {
            MessageBox.Show("Chức năng 'Sửa Sản Phẩm' đang được phát triển!", "Thông báo");
        }

        private void txtTimKiemKM_TextChanged(object sender, EventArgs e) {
            LoadData_KhuyenMai();
        }

        private void cbLocTrangThaiKM_SelectedIndexChanged(object sender, EventArgs e) {
            LoadData_KhuyenMai();
        }

        private void btnThemKM_Click(object sender, EventArgs e) {
            MessageBox.Show("Chức năng 'Thêm Khuyến Mãi' đang được phát triển!", "Thông báo");
        }

        private void btnSuaKM_Click(object sender, EventArgs e) {
            MessageBox.Show("Chức năng 'Sửa Khuyến Mãi' đang được phát triển!", "Thông báo");
        }

        private void btnChuyenFormOder_Click(object sender, EventArgs e) {
            var confirm = MessageBox.Show("Chuyển sang trang Order?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try {
                // (Đoạn code này không cần truy cập CSDL, chỉ là khởi tạo để vào form bán hàng hehehe)
                var orderForm = new MainForm(_currentMaNV);
                orderForm.StartPosition = FormStartPosition.CenterScreen;
                orderForm.Show();
                orderForm.BringToFront();
                orderForm.Activate();

                orderForm.FormClosed += (s2, e2) => {
                    try {
                        this.Show();
                        this.BringToFront();
                        this.Activate();
                        // Tải lại dữ liệu khi quay về
                        LoadData_NhanVien();
                        LoadData_HoaDon();
                        LoadData_TonKho();
                    }
                    catch { }
                };
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi chuyển trang: " + ex.Message);
            }
        }

        private void cbThang_SelectedIndexChanged(object sender, EventArgs e) {
            // (Đã có nút Lọc riêng)
        }

        private void dgvPerformance_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            // (Để trống) vì đéo bt nó là cái khung lào để xoá =))))
        }

        private void btnSendNotify_Click(object sender, EventArgs e) {
            try {
                if (dgvPerformance.SelectedRows.Count == 0) {
                    MessageBox.Show("Vui lòng chọn một nhân viên trong bảng để thông báo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var row = dgvPerformance.SelectedRows[0];
                var ten = row.Cells["TenNV"].Value?.ToString() ?? "(không tên)";
                var custom = txtNotifyMessage.Text?.Trim();
                if (string.IsNullOrEmpty(custom)) {
                    MessageBox.Show("Vui lòng nhập nội dung thông báo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var fullMsg = $"[Quản lí] Người được chọn: {ten} - {custom}";

                var n = new NotificationCenter.Notification { Type = NotificationCenter.NotificationType.NhanVienInactive, Message = fullMsg, Data = ten };
                NotificationCenter.Raise(n);

                MessageBox.Show("Đã gửi thông báo tới Admin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi gửi thông báo: " + ex.Message);
            }
        }

        #endregion

        #region Các hàm tô màu DataGridView (Cell Formatting - Giữ nguyên)

        private void dgvPerformance_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (dgvPerformance.Columns[e.ColumnIndex].Name == "HieuSuat" && e.Value != null) {
                string hieuSuat = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (hieuSuat) {
                    case "Xuất Sắc":
                        e.CellStyle.BackColor = Color.FromArgb(223, 240, 216);
                        e.CellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        break;
                    case "Tốt":
                        e.CellStyle.BackColor = Color.FromArgb(252, 248, 227);
                        e.CellStyle.ForeColor = Color.FromArgb(138, 109, 59);
                        break;
                    case "Cần Cải Thiện":
                        e.CellStyle.BackColor = Color.FromArgb(242, 222, 222);
                        e.CellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                        break;
                }
            }
        }

        private void dgvHoaDon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (dgvHoaDon.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null) {
                string trangThai = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (trangThai) {
                    case "Đã thanh toán":
                        e.CellStyle.BackColor = Color.FromArgb(223, 240, 216);
                        e.CellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        break;
                    case "Đang xu ly":
                        e.CellStyle.BackColor = Color.FromArgb(252, 248, 227);
                        e.CellStyle.ForeColor = Color.FromArgb(138, 109, 59);
                        break;
                    case "Đã hủy":
                        e.CellStyle.BackColor = Color.FromArgb(242, 222, 222);
                        e.CellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                        break;
                }
            }
        }

        private void dgvTonKho_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (dgvTonKho.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null) {
                string trangThai = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (trangThai) {
                    case "Dồi dào":
                        e.CellStyle.BackColor = Color.FromArgb(223, 240, 216);
                        e.CellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        break;
                    case "Cảnh báo":
                        e.CellStyle.BackColor = Color.FromArgb(252, 248, 227);
                        e.CellStyle.ForeColor = Color.FromArgb(138, 109, 59);
                        break;
                    case "Hết hàng":
                        e.CellStyle.BackColor = Color.FromArgb(242, 222, 222);
                        e.CellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                        break;
                }
            }
        }

        private void dgvSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (dgvSanPham.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null) {
                string trangThai = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (trangThai) {
                    case "Còn bán":
                        e.CellStyle.BackColor = Color.FromArgb(223, 240, 216); // Green
                        e.CellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        break;
                    case "Hết hàng":
                    case "Ngừng bán":
                        e.CellStyle.BackColor = Color.FromArgb(242, 222, 222); // Red
                        e.CellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                        break;
                }
            }
        }

        private void dgvKhuyenMai_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (dgvKhuyenMai.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null) {
                string trangThai = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (trangThai) {
                    case "Đang áp dụng":
                        e.CellStyle.BackColor = Color.FromArgb(223, 240, 216); // Green
                        e.CellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        break;
                    case "Sắp diễn ra":
                        e.CellStyle.BackColor = Color.FromArgb(252, 248, 227); // Yellow
                        e.CellStyle.ForeColor = Color.FromArgb(138, 109, 59);
                        break;
                    case "Đã kết thúc":
                        e.CellStyle.BackColor = Color.FromArgb(242, 222, 222); // Red
                        e.CellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                        break;
                }
            }
        }

        #endregion

        #region Các hàm phụ (ĐÃ DI CHUYỂN)

        // Các hàm TinhHieuSuat, TinhTrangThaiKho, TinhGiaTriKM
        // đã được di chuyển sang KhoTruyVanQuanLi.cs

        #endregion

        private void QuanLi_Load_1(object sender, EventArgs e) {
            // (Trống) 
        }

        private void grpNotify_Enter(object sender, EventArgs e) {
            // (Trống)
        }

    }
}