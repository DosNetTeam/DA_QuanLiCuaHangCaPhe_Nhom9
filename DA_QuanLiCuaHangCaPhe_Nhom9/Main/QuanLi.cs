using DA_QuanLiCuaHangCaPhe_Nhom9.Function;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using System.Configuration;
using System.Data;
using System.Globalization;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    public partial class QuanLi : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CoffeeDB"]?.ConnectionString;
        private int _currentMaNV = 0;

        public QuanLi(int maNv = 0)
        {
            _currentMaNV = maNv;
            InitializeComponent();

            // Gắn các sự kiện bổ sung
            this.Load += QuanLi_Load;
            txtTimKiemHD.TextChanged += txtTimKiemHD_TextChanged;
            cbTrangThaiHD.SelectedIndexChanged += cbTrangThaiHD_SelectedIndexChanged;
            txtTimKiemKho.TextChanged += txtTimKiemKho_TextChanged;
            btnThemMoiKho.Click += btnThemMoiKho_Click;

            // Gắn sự kiện cho tab Sản Phẩm
            txtTimKiemSP.TextChanged += txtTimKiemSP_TextChanged;
           
            dgvSanPham.CellFormatting += dgvSanPham_CellFormatting;

            // Gắn sự kiện cho tab Khuyến mãi (MỚI)
            txtTimKiemKM.TextChanged += txtTimKiemKM_TextChanged;
            cbLocTrangThaiKM.SelectedIndexChanged += cbLocTrangThaiKM_SelectedIndexChanged;
           
            dgvKhuyenMai.CellFormatting += dgvKhuyenMai_CellFormatting;

            // Gắn CellFormatting để tô màu các cột trạng thái / hiệu suất
            dgvPerformance.CellFormatting += dgvPerformance_CellFormatting;
            dgvHoaDon.CellFormatting += dgvHoaDon_CellFormatting;
            dgvTonKho.CellFormatting += dgvTonKho_CellFormatting;

            // Wire notify button directly (controls are in pnlNotify)
            if (btnSendNotify != null)
                btnSendNotify.Click += btnSendNotify_Click;
        }

        private void QuanLi_Load(object sender, EventArgs e)
        {
            // Cài đặt các ComboBox lọc
            SetupFilters();

            // Tải dữ liệu ban đầu
            LoadData_NhanVien();
            LoadData_HoaDon();
            LoadData_TonKho();
            LoadData_SanPham();
            LoadData_KhuyenMai(); // <-- THÊM MỚI
            LoadNotifications();

            // Ensure notification group stays in the left menu and is usable
            try
            {
                if (grpNotify != null)
                {
                    // Keep group in the designer parent (panelMenu) so input and tab order work
                    grpNotify.Visible = true;
                    grpNotify.BringToFront();
                }
                if (txtNotifyMessage != null)
                {
                    txtNotifyMessage.Enabled = true;
                    txtNotifyMessage.ReadOnly = false;
                }
                if (btnSendNotify != null)
                {
                    btnSendNotify.Enabled = true;
                    // ensure click handler present
                    btnSendNotify.Click -= btnSendNotify_Click;
                    btnSendNotify.Click += btnSendNotify_Click;
                }
            }
            catch { }
        }

        private void PositionNotifyGroup()
        {
            try
            {
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

        // Public helper to get notifications as list of strings
        public List<string> GetNotifications()
        {
            var list = new List<string>();
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    // 1) Employees inactive (no orders in last 30 days)
                    var since = DateTime.Now.AddDays(-30);
                    var inactive = db.NhanViens
                        .Where(nv => !db.DonHangs.Any(dh => dh.MaNv == nv.MaNv && dh.NgayLap >= since))
                        .Select(nv => nv.TenNv)
                        .Take(5)
                        .ToList();

                    foreach (var name in inactive)
                        list.Add($"Nhân viên lâu không hoạt động: {name}");

                    // 2) Unpaid invoices (older than 1 day and ThanhToan status 'Chưa thanh toán')
                    var unpaid = db.ThanhToans
                        .Where(tt => tt.TrangThai == "Chưa thanh toán" && tt.MaDhNavigation.NgayLap <= DateTime.Now.AddDays(-1))
                        .Select(tt => new { tt.MaDh, tt.MaDhNavigation.NgayLap })
                        .Take(5)
                        .ToList();

                    foreach (var u in unpaid)
                        list.Add($"Hóa đơn chưa thanh toán: #{u.MaDh} - {u.NgayLap?.ToString("dd/MM/yy")}");

                    // 3) Low stock items (SoLuongTon <= NguongCanhBao)
                    var low = db.NguyenLieus
                        .Where(nl => nl.SoLuongTon <= (nl.NguongCanhBao ?? 0))
                        .Select(nl => new { nl.TenNl, nl.SoLuongTon })
                        .Take(10)
                        .ToList();

                    foreach (var nl in low)
                        list.Add($"Hàng trong kho còn ít: {nl.TenNl} ({nl.SoLuongTon ?? 0})");
                }
            }
            catch (Exception ex)
            {
                list.Add("Lỗi khi tải thông báo: " + ex.Message);
            }

            if (list.Count == 0)
                list.Add("Không có thông báo mới.");

            return list;
        }

        private void LoadNotifications()
        {
            try
            {
                var notes = GetNotifications();
                // lbNotifications was removed from the UI; keep a no-op placeholder
                // Optionally we could display the latest note in grpNotify.Tag for debugging
                if (grpNotify != null && notes != null && notes.Count > 0)
                {
                    grpNotify.Tag = notes; // store notes for possible later use
                }
            }
            catch { }
        }

        // Cài đặt giá trị ban đầu cho các ComboBox lọc
        private void SetupFilters()
        {
            // Cài đặt cho ComboBox Lọc Tháng
            cbThang.Items.Clear();
            cbThang.Items.Add("Tất cả");
            for (int i = 1; i <= 12; i++)
            {
                cbThang.Items.Add($"Tháng {i}");
            }
            cbThang.SelectedIndex = 0;

            // Cài đặt cho ComboBox Trạng thái Hóa Đơn
            cbTrangThaiHD.Items.Clear();
            cbTrangThaiHD.Items.Add("Tất cả");
            cbTrangThaiHD.Items.Add("Đang xu ly");
            cbTrangThaiHD.Items.Add("Đã thanh toán");
            cbTrangThaiHD.Items.Add("Đã hủy");
            cbTrangThaiHD.SelectedIndex = 0;

            // Cài đặt cho ComboBox Trạng thái Khuyến Mãi (MỚI)
            cbLocTrangThaiKM.Items.Clear();
            cbLocTrangThaiKM.Items.Add("Tất cả");
            cbLocTrangThaiKM.Items.Add("Đang áp dụng");
            cbLocTrangThaiKM.Items.Add("Đã kết thúc");
            cbLocTrangThaiKM.Items.Add("Sắp diễn ra");
            cbLocTrangThaiKM.SelectedIndex = 0;

            // Placeholder text handling (nếu cần)
            if (string.IsNullOrWhiteSpace(txtTimKiemHD.Text))
                txtTimKiemHD.ForeColor = Color.Gray;
            if (string.IsNullOrWhiteSpace(txtTimKiemKho.Text))
                txtTimKiemKho.ForeColor = Color.Gray;
        }

        #region Các hàm tải dữ liệu (Load Data - Dùng EF Core)

        private void LoadData_NhanVien()
        {
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    int selectedMonth = cbThang.SelectedIndex;

                    var query = from nv in db.NhanViens.Where(nv => nv.TrangThai == "Đang làm việc")
                                join dh in db.DonHangs
                                    .Where(d => selectedMonth == 0 || (d.NgayLap.HasValue && d.NgayLap.Value.Month == selectedMonth))
                                    on nv.MaNv equals dh.MaNv into groupDonHang
                                from donHang in groupDonHang.DefaultIfEmpty()
                                group donHang by new { nv.MaNv, nv.TenNv } into g
                                select new
                                {
                                    TenNV = g.Key.TenNv,
                                    SoDon = g.Count(dh => dh != null),
                                    TongDoanhThu = g.Sum(dh => (decimal?)dh.TongTien) ?? 0
                                };

                    var finalData = query
                        .OrderByDescending(x => x.TongDoanhThu)
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.TenNV,
                            x.SoDon,
                            TongDoanhThu = x.TongDoanhThu.ToString("N0", CultureInfo.InvariantCulture) + " đ",
                            HieuSuat = TinhHieuSuat(x.TongDoanhThu)
                        })
                        .ToList();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        private void LoadData_HoaDon()
        {
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    var query = from dh in db.DonHangs
                                join nv in db.NhanViens on dh.MaNv equals nv.MaNv
                                select new
                                {
                                    MaHD = dh.MaDh,
                                    NgayLap = dh.NgayLap,
                                    NhanVien = nv.TenNv,
                                    TongTienRaw = dh.TongTien ?? 0,
                                    TrangThai = dh.TrangThai
                                };

                    string timKiem = txtTimKiemHD.Text?.Trim().ToLower() ?? "";
                    string trangThai = cbTrangThaiHD.SelectedItem?.ToString() ?? "Tất cả";

                    if (!string.IsNullOrEmpty(timKiem) && timKiem != "tim kiem ma hd")
                    {
                        query = query.Where(x => x.MaHD.ToString().ToLower().Contains(timKiem));
                    }

                    if (trangThai != "Tất cả")
                    {
                        query = query.Where(x => x.TrangThai == trangThai);
                    }

                    var finalData = query
                        .OrderByDescending(x => x.NgayLap)
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.MaHD,
                            x.NgayLap,
                            x.NhanVien,
                            TongTien = x.TongTienRaw.ToString("N0", CultureInfo.InvariantCulture) + " đ",
                            x.TrangThai
                        })
                        .ToList();

                    dgvHoaDon.DataSource = finalData;
                    dgvHoaDon.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu hóa đơn: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        private void LoadData_TonKho()
        {
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    var query = from nl in db.NguyenLieus.Where(nl => nl.TrangThai == "Đang kinh doanh")
                                select new
                                {
                                    TenHang = nl.TenNl,
                                    Loai = "Nguyên Liệu",
                                    SoLuongTon = nl.SoLuongTon,
                                    DonVi = nl.DonViTinh,
                                    NguongCanhBao = nl.NguongCanhBao ?? 0
                                };

                    string timKiem = txtTimKiemKho.Text?.Trim().ToLower() ?? "";
                    if (!string.IsNullOrEmpty(timKiem) && timKiem != "tim kiem nguyen lieu")
                    {
                        query = query.Where(x => x.TenHang.ToLower().Contains(timKiem));
                    }

                    var finalData = query
                        .OrderBy(x => x.TenHang)
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.TenHang,
                            x.Loai,
                            SoLuong = $"{x.SoLuongTon} {x.DonVi}",
                            TrangThai = TinhTrangThaiKho(x.SoLuongTon, x.NguongCanhBao)
                        })
                        .ToList();

                    dgvTonKho.DataSource = finalData;
                    dgvTonKho.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu tồn kho: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        private void LoadData_SanPham()
        {
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    var query = from sp in db.SanPhams
                                select new
                                {
                                    sp.MaSp,
                                    sp.TenSp,
                                    sp.LoaiSp,
                                    DonGiaRaw = sp.DonGia,
                                    sp.DonVi,
                                    sp.TrangThai
                                };

                    string timKiem = txtTimKiemSP.Text?.Trim().ToLower() ?? "";
                    if (!string.IsNullOrEmpty(timKiem) && timKiem != "tìm theo tên sản phẩm")
                    {
                        query = query.Where(x => x.TenSp.ToLower().Contains(timKiem));
                    }

                    var finalData = query
                        .OrderBy(x => x.TenSp)
                        .AsEnumerable()
                        .Select(x => new {
                            x.MaSp,
                            TenSP = x.TenSp,
                            Loai = x.LoaiSp,
                            DonGia = x.DonGiaRaw.ToString("N0", CultureInfo.InvariantCulture) + " đ",
                            x.DonVi,
                            x.TrangThai
                        })
                        .ToList();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        // <-- HÀM MỚI -->
        private void LoadData_KhuyenMai()
        {
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    // Giả sử db.KhuyenMais
                    var query = from km in db.KhuyenMais
                                select km;

                    // Lọc theo Trạng thái
                    string trangThai = cbLocTrangThaiKM.SelectedItem?.ToString() ?? "Tất cả";
                    if (trangThai != "Tất cả")
                    {
                        query = query.Where(x => x.TrangThai == trangThai);
                    }

                    // Lọc theo Tên KM
                    string timKiem = txtTimKiemKM.Text?.Trim().ToLower() ?? "";
                    if (!string.IsNullOrEmpty(timKiem) && timKiem != "tìm theo tên km")
                    {
                        query = query.Where(x => x.TenKm.ToLower().Contains(timKiem));
                    }

                    var finalData = query
                        .OrderByDescending(x => x.NgayBatDau)
                        .AsEnumerable() // Chuyển sang client-side để dùng TinhGiaTriKM
                        .Select(x => new {
                            x.MaKm,
                            TenKM = x.TenKm,
                            Loai = x.LoaiKm,
                            GiaTri = TinhGiaTriKM(x.GiaTri, x.LoaiKm), // Helper function
                            BatDau = x.NgayBatDau,
                            KetThuc = x.NgayKetThuc,
                            x.TrangThai
                        })
                        .ToList();

                    dgvKhuyenMai.DataSource = finalData;

                    // Ẩn cột MaKm
                    if (dgvKhuyenMai.Columns["MaKm"] != null)
                        dgvKhuyenMai.Columns["MaKm"].Visible = false;

                    // Đổi tên cột
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu khuyến mãi: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        #endregion

        #region Các hàm xử lý sự kiện (Event Handlers)

        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadData_NhanVien();
        }

        private void txtTimKiemHD_TextChanged(object sender, EventArgs e)
        {
            LoadData_HoaDon();
        }

        private void cbTrangThaiHD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData_HoaDon();
        }

        private void txtTimKiemKho_TextChanged(object sender, EventArgs e)
        {
            LoadData_TonKho();
        }

        private void btnThemMoiKho_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng 'Thêm Mới Kho' đang được phát triển!", "Thông báo");
        }

        private void txtTimKiemSP_TextChanged(object sender, EventArgs e)
        {
            LoadData_SanPham();
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng 'Thêm Sản Phẩm' đang được phát triển!", "Thông báo");
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng 'Sửa Sản Phẩm' đang được phát triển!", "Thông báo");
        }

        // <-- CÁC HÀM MỚI CHO KHUYẾN MÃI -->
        private void txtTimKiemKM_TextChanged(object sender, EventArgs e)
        {
            LoadData_KhuyenMai();
        }

        private void cbLocTrangThaiKM_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData_KhuyenMai();
        }

        private void btnThemKM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng 'Thêm Khuyến Mãi' đang được phát triển!", "Thông báo");
        }

        private void btnSuaKM_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng 'Sửa Khuyến Mãi' đang được phát triển!", "Thông báo");
        }
        // <-- KẾT THÚC THÊM MỚI -->

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            // Confirm navigation to Order screen
            var confirm = MessageBox.Show("Chuyển sang trang Order?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using var db = new DataSqlContext();
                var account = db.TaiKhoans; // dummy account for navigation


                // Open the ordering MainForm. Pass MaNV = 0 (guest/unknown employee).
                var orderForm = new MainForm(_currentMaNV);
                orderForm.StartPosition = FormStartPosition.CenterScreen;
                orderForm.Show();
                // Make sure the new form is visible on top
                orderForm.BringToFront();
                orderForm.Activate();

                // When the order form is closed, show this manager form again
                orderForm.FormClosed += (s2, e2) =>
                {
                    try
                    {
                        this.Show();
                        this.BringToFront();
                        this.Activate();
                        LoadData_NhanVien();
                        LoadData_HoaDon();
                        LoadData_TonKho();
                    }
                    catch { }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chuyển trang: " + ex.Message);
            }
        }

        private void cbThang_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void dgvPerformance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        // Send custom notification about selected employee to Admin/Login admin
        private void btnSendNotify_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure a row is selected
                if (dgvPerformance.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một nhân viên trong bảng để thông báo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var row = dgvPerformance.SelectedRows[0];
                // Assume there is a column 'TenNV' in data source
                var ten = row.Cells["TenNV"].Value?.ToString() ?? "(không tên)";
                var custom = txtNotifyMessage.Text?.Trim();
                if (string.IsNullOrEmpty(custom))
                {
                    MessageBox.Show("Vui lòng nhập nội dung thông báo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var fullMsg = $"[Quản lí] Người được chọn: {ten} - {custom}";

                // Push notification via NotificationCenter (NhanVienInactive type used to target admin)
                var n = new NotificationCenter.Notification { Type = NotificationCenter.NotificationType.NhanVienInactive, Message = fullMsg, Data = ten };
                NotificationCenter.Raise(n);

                MessageBox.Show("Đã gửi thông báo tới Admin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi thông báo: " + ex.Message);
            }
        }

        #endregion

        #region Các hàm tô màu DataGridView (Cell Formatting)

        private void dgvPerformance_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvPerformance.Columns[e.ColumnIndex].Name == "HieuSuat" && e.Value != null)
            {
                string hieuSuat = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (hieuSuat)
                {
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

        private void dgvHoaDon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvHoaDon.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string trangThai = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (trangThai)
                {
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

        private void dgvTonKho_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTonKho.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string trangThai = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (trangThai)
                {
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

        private void dgvSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSanPham.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string trangThai = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (trangThai)
                {
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

        // <-- HÀM TÔ MÀU MỚI -->
        private void dgvKhuyenMai_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvKhuyenMai.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string trangThai = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (trangThai)
                {
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

        #region Các hàm phụ (Helper Functions)

        private string TinhHieuSuat(decimal tongDoanhThu)
        {
            if (tongDoanhThu > 500000)
                return "Xuất Sắc";
            if (tongDoanhThu > 100000)
                return "Tốt";
            return "Cần Cải Thiện";
        }

        private string TinhTrangThaiKho(decimal? soLuong, decimal nguong)
        {
            if (soLuong == null || soLuong == 0)
                return "Hết hàng";
            if (soLuong < nguong)
                return "Cảnh báo";
            return "Dồi dào";
        }

        // <-- HÀM PHỤ MỚI -->
        private string TinhGiaTriKM(decimal? giaTri, string loai)
        {
            if (giaTri == null) return "N/A";

            // G29 dùng để loại bỏ các số 0 không cần thiết (ví dụ: 10.00 -> 10)
            if (loai == "HoaDon" || loai == "SanPham") // Giảm %
            {
                return $"{giaTri.Value.ToString("G29")}%";
            }
            if (loai == "GiamTien") // Nếu có loại giảm tiền trực tiếp
            {
                return $"{giaTri.Value.ToString("N0", CultureInfo.InvariantCulture)} đ";
            }
            return giaTri.Value.ToString("G29");
        }

        #endregion

        private void QuanLi_Load_1(object sender, EventArgs e)
        {

        }

        private void grpNotify_Enter(object sender, EventArgs e)
        {

        }
    }
}