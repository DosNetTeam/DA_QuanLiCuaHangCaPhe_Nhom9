using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class Admin : Form {
        private int selectedEmployeeId = 0;

        public Admin() {
            InitializeComponent();

            // Đặt hình thức ở giữa
            this.StartPosition = FormStartPosition.CenterScreen;

            // Dang ký sự kiện Load dữ liệu khi form được tải
            dtgvquanlynhanvien.CellClick += dataGridView1_CellClick;
            dtgvquanlynhanvien.KeyDown += dataGridView1_KeyDown;

            // Đăng ký sự kiện cho dgvkho (Nguyên liệu)
            dgvkho.CellClick += dgvInventory_CellClick;
            dgvkho.KeyDown += dgvInventory_KeyDown;

            // Tạo nút Lưu (nếu cần)
            CreateSaveButton();

            // Ẩn panel thông tin nhân viên ban đầu
            pnthongtinnv.Visible = false;

            // ========== THÊM MỚI: Gắn sự kiện cho TabControl thay vì MenuStrip =========
            // XÓA 2 DÒNG NÀY vì không còn MenuStrip
            // nguyênLiệuToolStripMenuItem.Click += NguyênLiệuToolStripMenuItem_Click;
            // sảnPhẩmToolStripMenuItem.Click += SảnPhẩmToolStripMenuItem_Click;

            // Gắn sự kiện cho dgvSanPham (vì đã có sẵn trong Designer)
            dgvSanPham.CellClick += dgvSanPham_CellClick;
            dgvSanPham.KeyDown += dgvSanPham_KeyDown;

            // Gắn sự kiện cho tabControlKho để tải dữ liệu khi chuyển tab
            tabControlKho.SelectedIndexChanged += TabControlKho_SelectedIndexChanged;

            // ========== GẮN SỰ KIỆN CHO TAB KHUYẾN MÃI ==========
            // Gắn sự kiện click vào DataGridView khuyến mãi để hiển thị chi tiết
            dgvkhuyenmai.CellClick += dgvkhuyenmai_CellClick;
            dgvkhuyenmai.KeyDown += dgvkhuyenmai_KeyDown;
        }

        // ========== Hàm CreateSaveButton ==========
        private void CreateSaveButton() {
            // Thực tế không cần button này nữa vì đã có button1 (Đổi mật khẩu)
            // Hàm này chỉ là placeholder, không làm gì cả
            // Bạn có thể xóa dòng gọi CreateSaveButton() trong constructor nếu muốn
        }

        // ========== EVENT HANDLER CHO SẢN PHẨM ==========
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

        // ========== XÓA 2 HÀM XỬ LÝ MENUSTRIP CŨ ==========
        // Không còn MenuStrip, giờ dùng TabControl

        // ========== HÀM TẢI DỮ LIỆU SẢN PHẨM =========
        private void LoadProductData() {
            try {
                DataSqlContext db = new DataSqlContext();

                var products = db.SanPhams
                    .Select(sp => new {
                        sp.MaSp,
                        sp.TenSp,
                        sp.LoaiSp,
                        sp.DonGia,
                        sp.DonVi,
                        sp.TrangThai
                    })
                    .OrderBy(sp => sp.TenSp)
                    .ToList();

                dgvSanPham.DataSource = products;

                // Định dạng cột
                StyleDataGridView(dgvSanPham);
                dgvSanPham.Columns["MaSp"].HeaderText = "Mã SP";
                dgvSanPham.Columns["TenSp"].HeaderText = "Tên sản phẩm";
                dgvSanPham.Columns["LoaiSp"].HeaderText = "Loại";
                dgvSanPham.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvSanPham.Columns["DonVi"].HeaderText = "Đơn vị";
                dgvSanPham.Columns["TrangThai"].HeaderText = "Trạng thái";

                // Định dạng cột Đơn giá
                dgvSanPham.Columns["DonGia"].DefaultCellStyle.Format = "N0";

                // Áp dụng màu sắc theo trạng thái
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

                db.Dispose();
            }
            catch (Exception ex) {
                MessageBox.Show(
                    "Lỗi khi tải dữ liệu sản phẩm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Admin_Load(object sender, EventArgs e) {
            // Làm mới dữ liệu khi form được tải
            LoadOverviewData();
        }

        private void tabControl1_SelectedIndexChanged(object? sender, EventArgs e) {
            // Làm mới dữ liệu khi chuyển tab
            switch (bandieukhien.SelectedIndex) {
                case 0: // Tổng quát
                    LoadOverviewData();
                    break;
                case 1: // Quản Lý Nhân Viên
                    LoadEmployeeData();
                    pnthongtinnv.Visible = false;
                    break;
                case 2: // Quản Lý Kho
                        // Mặc định hiển thị Nguyên liệu khi vào tab
                    LoadInventoryData(); // Tải nguyên liệu (tab đầu tiên của tabControlKho)
                    gbthongtinnguyenlieu.Visible = false; // Ẩn groupBox ban đầu
                    gbsanpham.Visible = false;
                    break;
                case 3: // Doanh thu
                    LoadRevenueData();
                    break;
                case 4: // Khuyến mãi (TAB MỚI)
                    LoadKhuyenMaiData(); // Tải dữ liệu khuyến mãi
                    gbkhuyenmai.Visible = false; // Ẩn form nhập liệu ban đầu
                    break;
            }
        }

        // Làm mới dữ liệu tổng quan từ cơ sở dữ liệu
        private void LoadOverviewData() {
            try {
                DataSqlContext db = new DataSqlContext();

                // Đếm tổng số nhân viên
                int tongNhanVien = db.NhanViens.Count();

                // Đếm tổng số sản phẩm còn bán
                int tongSanPham = db.SanPhams.Count(s => s.TrangThai == "Con ban");

                // Đếm tổng số đơn hàng
                int tongDonHang = db.DonHangs.Count();

                // Đếm đơn hàng hôm nay
                int donHangHomNay = 0;
                List<DonHang> tatCaDonHang = db.DonHangs.ToList();
                for (int i = 0; i < tatCaDonHang.Count; i++) {
                    if (tatCaDonHang[i].NgayLap.HasValue) {
                        if (tatCaDonHang[i].NgayLap.Value.Date == DateTime.Today) {
                            donHangHomNay = donHangHomNay + 1;
                        }
                    }
                }

                // Đếm tổng nguyên liệu
                int tongNguyenLieu = db.NguyenLieus.Count();

                // Đếm tổng khách hàng
                int tongKhachHang = db.KhachHangs.Count();

                // Tạo danh sách overview
                List<object> overviewData = new List<object>();

                overviewData.Add(new {
                    Category = "Tổng số nhân viên",
                    Count = tongNhanVien,
                    Details = "Nhân viên đang làm việc"
                });

                overviewData.Add(new {
                    Category = "Tổng số sản phẩm",
                    Count = tongSanPham,
                    Details = "Sản phẩm đang bán"
                });

                overviewData.Add(new {
                    Category = "Tổng số đơn hàng",
                    Count = tongDonHang,
                    Details = "Tất cả đơn hàng"
                });

                overviewData.Add(new {
                    Category = "Đơn hàng hôm nay",
                    Count = donHangHomNay,
                    Details = DateTime.Today.ToString("dd/MM/yyyy")
                });

                overviewData.Add(new {
                    Category = "Nguyên liệu trong kho",
                    Count = tongNguyenLieu,
                    Details = "Tổng số loại nguyên liệu"
                });

                overviewData.Add(new {
                    Category = "Khách hàng",
                    Count = tongKhachHang,
                    Details = "Tổng số khách hàng"
                });

                tongquandulieu.DataSource = overviewData;

                // Định dạng cột
                StyleDataGridView(tongquandulieu);
                tongquandulieu.Columns["Category"].HeaderText = "Danh mục";
                tongquandulieu.Columns["Count"].HeaderText = "Số lượng";
                tongquandulieu.Columns["Details"].HeaderText = "Chi tiết";
            }
            catch (Exception ex) {
                MessageBox.Show(
                 $"Lỗi khi tải dữ liệu tổng quan:\n{ex.Message}",
                 "Lỗi",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
            }
        }

        // Làm mới dữ liệu nhân viên từ cơ sở dữ liệu
        private void LoadEmployeeData() {
            try {
                DataSqlContext db = new DataSqlContext();

                var employees = db.NhanViens
                   .Select(nv => new {
                       nv.MaNv,
                       nv.TenNv,
                       nv.ChucVu,
                       nv.SoDienThoai,
                       TaiKhoan = nv.TaiKhoan != null ? nv.TaiKhoan.TenDangNhap : "Chưa có",
                       VaiTro = nv.TaiKhoan != null ? nv.TaiKhoan.MaVaiTroNavigation.TenVaiTro : "N/A",
                       TrangThai = nv.TaiKhoan != null ?
                    (nv.TaiKhoan.TrangThai == true ? "Hoạt động" : "Bị khóa") : "N/A"
                   })
                    .ToList();

                dtgvquanlynhanvien.DataSource = employees;

                // Định dạng cột
                StyleDataGridView(dtgvquanlynhanvien);
                dtgvquanlynhanvien.Columns["MaNv"].HeaderText = "Mã NV";
                dtgvquanlynhanvien.Columns["TenNv"].HeaderText = "Họ tên";
                dtgvquanlynhanvien.Columns["ChucVu"].HeaderText = "Chức vụ";
                dtgvquanlynhanvien.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dtgvquanlynhanvien.Columns["TaiKhoan"].HeaderText = "Tài khoản";
                dtgvquanlynhanvien.Columns["VaiTro"].HeaderText = "Vai trò";
                dtgvquanlynhanvien.Columns["TrangThai"].HeaderText = "Trạng thái";

                db.Dispose();
            }
            catch (Exception ex) {
                MessageBox.Show(
                 "Lỗi khi tải dữ liệu nhân viên: " + ex.Message,
                    "Lỗi",
                  MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Làm mới dữ liệu kho từ cơ sở dữ liệu
        private void LoadInventoryData() {
            try {
                DataSqlContext db = new DataSqlContext();

                // Lấy TẤT CẢ nguyên liệu (không lọc TrangThai)
                var inventory = db.NguyenLieus
                    .Select(nl => new {
                        nl.MaNl,
                        nl.TenNl,
                        nl.DonViTinh,
                        SoLuongTon = Math.Round(nl.SoLuongTon != null ? nl.SoLuongTon.Value : 0, 2),
                        NguongCanhBao = Math.Round(nl.NguongCanhBao != null ? nl.NguongCanhBao.Value : 0, 2)
                    })
            .ToList();

                // Tính toán tình trạng
                List<object> inventoryWithStatus = new List<object>();

                for (int i = 0; i < inventory.Count; i++) {
                    var nl = inventory[i];
                    string tinhTrang;

                    // Tính tỷ lệ % so với ngưỡng cảnh báo
                    decimal nguongCanhBao = nl.NguongCanhBao > 0 ? nl.NguongCanhBao : 100;
                    decimal motPhanBa = nguongCanhBao / 3;
                    decimal haiPhanBa = (nguongCanhBao * 2) / 3;

                    // Xác định tình trạng
                    if (nl.SoLuongTon <= 0) {
                        tinhTrang = "Hết hàng";
                    }
                    else if (nl.SoLuongTon <= motPhanBa) {
                        tinhTrang = "Hết hàng";
                    }
                    else if (nl.SoLuongTon <= haiPhanBa) {
                        tinhTrang = "Thiếu thốn";
                    }
                    else {
                        tinhTrang = "Dồi dào";
                    }

                    var item = new {
                        nl.MaNl,
                        nl.TenNl,
                        nl.DonViTinh,
                        nl.SoLuongTon,
                        nl.NguongCanhBao,
                        TinhTrang = tinhTrang
                    };

                    inventoryWithStatus.Add(item);
                }

                // Sắp xếp theo Số lượng tồn
                var sortedList = inventoryWithStatus
              .Cast<dynamic>()
           .OrderBy(x => (decimal)x.SoLuongTon)
                    .ToList();

                dgvkho.DataSource = sortedList;

                // Dinh dạng cột
                StyleDataGridView(dgvkho);
                dgvkho.Columns["MaNl"].HeaderText = "Mã NL";
                dgvkho.Columns["TenNl"].HeaderText = "Tên nguyên liệu";
                dgvkho.Columns["DonViTinh"].HeaderText = "Đơn vị";
                dgvkho.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
                dgvkho.Columns["NguongCanhBao"].HeaderText = "Ngưỡng cảnh báo";
                dgvkho.Columns["TinhTrang"].HeaderText = "Tình trạng";

                // Áp dụng màu sắc
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

                db.Dispose();
            }
            catch (Exception ex) {
                MessageBox.Show(
                    "Lỗi khi tải dữ liệu kho: " + ex.Message,
                    "Lỗi",
                  MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Tải dữ liệu doanh thu từ cơ sở dữ liệu
        private void LoadRevenueData() {
            try {
                DataSqlContext db = new DataSqlContext();

                var donHangs = db.DonHangs
                 .Where(dh => dh.TongTien != null && dh.NgayLap != null)
               .ToList();
                // Nhóm theo tháng và năm
                Dictionary<string, List<DonHang>> groups = new Dictionary<string, List<DonHang>>();

                for (int i = 0; i < donHangs.Count; i++) {
                    DonHang dh = donHangs[i];

                    if (dh.NgayLap.HasValue) {
                        int year = dh.NgayLap.Value.Year;
                        int month = dh.NgayLap.Value.Month;
                        string key = month.ToString("00") + "/" + year.ToString();

                        if (!groups.ContainsKey(key)) {
                            groups[key] = new List<DonHang>();
                        }

                        groups[key].Add(dh);
                    }
                }

                // Tính toán doanh thu
                List<object> revenueData = new List<object>();

                foreach (var group in groups) {
                    string thang = group.Key;
                    List<DonHang> danhSachDonHang = group.Value;

                    int soDonHang = danhSachDonHang.Count;
                    decimal tongDoanhThu = 0;
                    decimal maxDoanhThu = 0;
                    decimal minDoanhThu = decimal.MaxValue;

                    for (int i = 0; i < danhSachDonHang.Count; i++) {
                        decimal tien = danhSachDonHang[i].TongTien.HasValue ? danhSachDonHang[i].TongTien.Value : 0;
                        tongDoanhThu = tongDoanhThu + tien;

                        if (tien > maxDoanhThu) {
                            maxDoanhThu = tien;
                        }

                        if (tien < minDoanhThu) {
                            minDoanhThu = tien;
                        }
                    }

                    decimal trungBinh = soDonHang > 0 ? tongDoanhThu / soDonHang : 0;

                    var item = new {
                        Thang = thang,
                        SoDonHang = soDonHang,
                        TongDoanhThu = Math.Round(tongDoanhThu, 0),
                        DoanhThuTrungBinh = Math.Round(trungBinh, 0),
                        DonHangLonNhat = Math.Round(maxDoanhThu, 0),
                        DonHangNhoNhat = Math.Round(minDoanhThu, 0)
                    };

                    revenueData.Add(item);
                }

                // Sắp xếp giảm dần theo tháng và lấy 12 tháng gần nhất
                var sortedData = revenueData
                    .Cast<dynamic>()
                    .OrderByDescending(x => (string)x.Thang)
                    .Take(12)
              .ToList();

                dgvdoanhthu.DataSource = sortedData;

                // Format columns
                StyleDataGridView(dgvdoanhthu);
                dgvdoanhthu.Columns["Thang"].HeaderText = "Tháng";
                dgvdoanhthu.Columns["SoDonHang"].HeaderText = "Số đơn hàng";
                dgvdoanhthu.Columns["TongDoanhThu"].HeaderText = "Tổng doanh thu";
                dgvdoanhthu.Columns["DoanhThuTrungBinh"].HeaderText = "TB/Đơn";
                dgvdoanhthu.Columns["DonHangLonNhat"].HeaderText = "Cao nhất";
                dgvdoanhthu.Columns["DonHangNhoNhat"].HeaderText = "Thấp nhất";

                // Format currency columns
                dgvdoanhthu.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
                dgvdoanhthu.Columns["DoanhThuTrungBinh"].DefaultCellStyle.Format = "N0";
                dgvdoanhthu.Columns["DonHangLonNhat"].DefaultCellStyle.Format = "N0";
                dgvdoanhthu.Columns["DonHangNhoNhat"].DefaultCellStyle.Format = "N0";

                db.Dispose();
            }
            catch (Exception ex) {
                MessageBox.Show(
                           "Lỗi khi tải dữ liệu doanh thu: " + ex.Message,
                           "Lỗi",
                  MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }
        //Áp dụng kiểu dáng nhất quán cho DataGridViews
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

        // Trình xử lý sự kiện khi người dùng nhấp vào hàng tồn kho (Nguyên Liệu)
        private void dgvInventory_CellClick(object? sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                ShowInventoryDetailInPanel(e.RowIndex);
            }
        }

        // Trình xử lý sự kiện cho điều hướng bàn phím trong lưới kiểm kê
        private void dgvInventory_KeyDown(object? sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && dgvkho.CurrentRow != null) {
                e.Handled = true;
                ShowInventoryDetailInPanel(dgvkho.CurrentRow.Index);
            }
        }

        // ========== HÀM HIỂN THỊ CHI TIẾT SẢN PHẨM ==========
        private void ShowProductDetailInPanel(int rowIndex) {
            try {
                DataGridViewRow row = dgvSanPham.Rows[rowIndex];

                if (row.Cells["MaSp"].Value != null) {
                    int maSp = Convert.ToInt32(row.Cells["MaSp"].Value);

                    DataSqlContext db = new DataSqlContext();

                    SanPham product = db.SanPhams
                            .Where(sp => sp.MaSp == maSp)
                        .FirstOrDefault();

                    if (product != null) {
                        // Hiển thị thông tin sản phẩm
                        txtmasp.Text = product.MaSp.ToString();
                        txttensp.Text = product.TenSp;
                        txtgia.Text = product.DonGia.ToString("N0");
                        txtdongia.Text = product.DonGia.ToString("N0");

                        if (product.LoaiSp != null) {
                            txtloai.Text = product.LoaiSp;
                        }
                        else {
                            txtloai.Text = "";
                        }

                        if (product.DonVi != null) {
                            txtdon_vi.Text = product.DonVi;
                        }
                        else {
                            txtdon_vi.Text = "";
                        }

                        // ========== HIỂN THỊ TRẠNG THÁI VÀO textBox1 (CHO PHÉP SỬA) ==========
                        if (product.TrangThai != null) {
                            txttinhtrangsp.Text = product.TrangThai;
                        }
                        else {
                            txttinhtrangsp.Text = "Còn kinh doanh";
                        }

                        // Cấu hình readonly
                        txtmasp.ReadOnly = true;
                        txttensp.ReadOnly = false;
                        txtloai.ReadOnly = false;
                        txtdongia.ReadOnly = false;
                        txtdon_vi.ReadOnly = false;
                        txtgia.ReadOnly = false;

                        // ========== CHO PHÉP SỬA TRẠNG THÁI ==========
                        txttinhtrangsp.ReadOnly = false;

                        // Hiển thị GroupBox
                        gbsanpham.Visible = true;
                        gbsanpham.BringToFront();

                        // Focus
                        txttensp.Focus();
                        txttensp.SelectAll();
                    }

                    db.Dispose();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(
                    "Lỗi khi hiển thị chi tiết sản phẩm: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Hiển thị chi tiết nhân viên trong panel
        private void ShowEmployeeDetailInPanel(int rowIndex) {
            try {
                DataGridViewRow row = dtgvquanlynhanvien.Rows[rowIndex];

                if (row.Cells["MaNv"].Value != null) {
                    int maNv = Convert.ToInt32(row.Cells["MaNv"].Value);
                    selectedEmployeeId = maNv;

                    DataSqlContext db = new DataSqlContext();

                    var nhanVien = db.NhanViens
                    .Include(nv => nv.TaiKhoan)
                      .ThenInclude(tk => tk.MaVaiTroNavigation)
                      .Where(nv => nv.MaNv == maNv)
                    .FirstOrDefault();

                    if (nhanVien != null) {
                        // Hiển thị thông tin cơ bản
                        txtmanv.Text = nhanVien.MaNv.ToString();
                        txthoten.Text = nhanVien.TenNv;

                        // Hiển thị số điện thoại
                        if (string.IsNullOrEmpty(nhanVien.SoDienThoai)) {
                            txtsdt.Text = "Chưa cập nhật";
                        }
                        else {
                            txtsdt.Text = nhanVien.SoDienThoai;
                        }

                        txtchucvu.Text = nhanVien.ChucVu;

                        // Hiển thị thông tin tài khoản
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

                        // Cấu hình các TextBox
                        txtmatkhau.ReadOnly = true;
                        txtmatkhau.UseSystemPasswordChar = true;
                        txtchucvu.ReadOnly = false;
                        txtvaitro.ReadOnly = false;

                        // Hiển thị panel
                        pnthongtinnv.Visible = true;
                    }

                    db.Dispose();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(
                 "Lỗi khi hiển thị chi tiết: " + ex.Message,
                 "Lỗi",
                       MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        // Button1: Đổi mật khẩu
        private void button1_Click(object sender, EventArgs e) {
            try {
                // Kiểm tra xem có nhân viên nào được chọn không
                if (string.IsNullOrEmpty(txtmanv.Text)) {
                    MessageBox.Show(
                     "Vui lòng chọn nhân viên từ danh sách trước!",
                     "Thông báo",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);
                    return;
                }
                int maNv = Convert.ToInt32(txtmanv.Text);
                string tenDangNhap = txttentaikhoan.Text;

                // Kiểm tra xem nhân viên có tài khoản không
                if (tenDangNhap == "Chưa có") {
                    MessageBox.Show(
                       "Nhân viên này chưa có tài khoản!",
                       "Thông báo",
                     MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                    return;
                }

                // Cho phép chỉnh sửa mật khẩu
                txtmatkhau.ReadOnly = false;
                txtmatkhau.UseSystemPasswordChar = false;
                txtmatkhau.Focus();
                txtmatkhau.SelectAll();

                // Hiển thị hộp thoại nhập mật khẩu mới
                using (Form promptForm = new Form()) {
                    promptForm.Width = 450;
                    promptForm.Height = 250;
                    promptForm.Text = "Đổi mật khẩu";
                    promptForm.StartPosition = FormStartPosition.CenterParent;
                    promptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    promptForm.MaximizeBox = false;
                    promptForm.MinimizeBox = false;

                    Label lblInfo = new Label() {
                        Text = $"Đổi mật khẩu cho: {txthoten.Text} ({tenDangNhap})",
                        Left = 20,
                        Top = 20,
                        Width = 400,
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                    };

                    Label lblOldPass = new Label() {
                        Text = "Mật khẩu hiện tại:",
                        Left = 20,
                        Top = 60,
                        Width = 150
                    };

                    TextBox txtOldPass = new TextBox() {
                        Left = 180,
                        Top = 57,
                        Width = 230,
                        UseSystemPasswordChar = true
                    };

                    Label lblNewPass = new Label() {
                        Text = "Mật khẩu mới:",
                        Left = 20,
                        Top = 95,
                        Width = 150
                    };

                    TextBox txtNewPass = new TextBox() {
                        Left = 180,
                        Top = 92,
                        Width = 230,
                        UseSystemPasswordChar = true
                    };

                    Label lblConfirmPass = new Label() {
                        Text = "Xác nhận mật khẩu:",
                        Left = 20,
                        Top = 130,
                        Width = 150
                    };

                    TextBox txtConfirmPass = new TextBox() {
                        Left = 180,
                        Top = 127,
                        Width = 230,
                        UseSystemPasswordChar = true
                    };

                    Button btnOK = new Button() {
                        Text = "Lưu",
                        Left = 180,
                        Top = 165,
                        Width = 110,
                        Height = 35,
                        BackColor = Color.FromArgb(46, 125, 50),
                        ForeColor = Color.White,
                        DialogResult = DialogResult.OK
                    };

                    Button btnCancel = new Button() {
                        Text = "Hủy",
                        Left = 300,
                        Top = 165,
                        Width = 110,
                        Height = 35,
                        BackColor = Color.FromArgb(211, 47, 47),
                        ForeColor = Color.White,
                        DialogResult = DialogResult.Cancel
                    };

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

                    if (promptForm.ShowDialog() == DialogResult.OK) {
                        // Xác thực đầu vào
                        if (string.IsNullOrWhiteSpace(txtOldPass.Text)) {
                            MessageBox.Show(
                            "Vui lòng nhập mật khẩu hiện tại!",
                            "Cảnh báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(txtNewPass.Text)) {
                            MessageBox.Show(
                             "Vui lòng nhập mật khẩu mới!",
                             "Cảnh báo",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Warning);
                            return;
                        }

                        if (txtNewPass.Text.Length < 3) {
                            MessageBox.Show(
                             "Mật khẩu mới phải có ít nhất 3 ký tự!",
                             "Cảnh báo",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Warning);
                            return;
                        }

                        if (txtNewPass.Text != txtConfirmPass.Text) {
                            MessageBox.Show(
                             "Mật khẩu xác nhận không khớp!",
                             "Cảnh báo",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Warning);
                            return;
                        }

                        // Cập nhật mật khẩu trong database
                        using var db = new DataSqlContext();

                        var taiKhoan = db.TaiKhoans
                             .FirstOrDefault(tk => tk.TenDangNhap == tenDangNhap);

                        if (taiKhoan == null) {
                            MessageBox.Show(
                             "Không tìm thấy tài khoản!",
                             "Lỗi",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Error);
                            return;
                        }

                        // Xác thực mật khẩu hiện tại
                        if (taiKhoan.MatKhau != txtOldPass.Text) {
                            MessageBox.Show(
                             "Mật khẩu hiện tại không đúng!",
                             "Lỗi",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Error);
                            return;
                        }

                        // Đồng ý đổi mật khẩu
                        var confirmResult = MessageBox.Show(
                         $"Xác nhận đổi mật khẩu?\n\n" +
                         $"Tài khoản: {tenDangNhap}\n" +
                         $"Nhân viên: {txthoten.Text}\n\n" +
                         "Mật khẩu mới sẽ được áp dụng ngay lập tức!",
                         "Xác nhận đổi mật khẩu",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Question);

                        if (confirmResult != DialogResult.Yes) {
                            return;
                        }

                        // Cập nhật mật khẩu
                        string oldPassword = taiKhoan.MatKhau;
                        taiKhoan.MatKhau = txtNewPass.Text;
                        db.SaveChanges();

                        MessageBox.Show(
                         $"Đổi mật khẩu thành công!\n\n" +
                          $"Tài khoản: {tenDangNhap}\n" +
                         $"Nhân viên: {txthoten.Text}\n" +
                         $"Mật khẩu cũ: {oldPassword}\n" +
                         $"Mật khẩu mới: {txtNewPass.Text}",
                         "Thành công",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information);

                        // Cập nhật lại TextBox mật khẩu
                        txtmatkhau.Text = txtNewPass.Text;
                        txtmatkhau.ReadOnly = true;
                        txtmatkhau.UseSystemPasswordChar = true;

                        // Làm mới dữ liệu nhân viên
                        LoadEmployeeData();
                    }
                    else {
                        // NGười dùng hủy đổi mật khẩu
                        txtmatkhau.ReadOnly = true;
                        txtmatkhau.UseSystemPasswordChar = true;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(
                 $"Lỗi khi đổi mật khẩu:\n{ex.Message}",
                 "Lỗi",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Error);

                // Khôi phục trạng thái chỉ đọc khi có lỗi
                if (txtmatkhau != null) {
                    txtmatkhau.ReadOnly = true;
                    txtmatkhau.UseSystemPasswordChar = true;
                }
            }
        }

        // Hiển thị chi tiết nguyên liệu trong panel
        private void ShowInventoryDetailInPanel(int rowIndex) {
            try {
                DataGridViewRow row = dgvkho.Rows[rowIndex];

                // Lấy Mã nguyên liệu từ dòng được chọn
                if (row.Cells["MaNl"].Value != null) {
                    int maNl = Convert.ToInt32(row.Cells["MaNl"].Value);

                    // Tải dữ liệu từ database
                    DataSqlContext db = new DataSqlContext();

                    NguyenLieu ingredient = db.NguyenLieus
                   .Where(nl => nl.MaNl == maNl)
                   .FirstOrDefault();

                    if (ingredient != null) {
                        // Hiển thị thông tin vào TextBox
                        txtma.Text = ingredient.MaNl.ToString();
                        txtten.Text = ingredient.TenNl;

                        if (ingredient.SoLuongTon.HasValue) {
                            txtsoluong.Text = ingredient.SoLuongTon.Value.ToString();
                        }
                        else {
                            txtsoluong.Text = "0";
                        }

                        txtdonvi.Text = ingredient.DonViTinh;

                        // ========== HIỂN THỊ TRẠNG THÁI VÀO textBox2 ==========
                        if (ingredient.TrangThai != null) {
                            textBox2.Text = ingredient.TrangThai;
                        }
                        else {
                            textBox2.Text = "Đang kinh doanh";
                        }

                        // Cấu hình readonly
                        txtma.ReadOnly = true;
                        txtten.ReadOnly = true;
                        txtdonvi.ReadOnly = true;
                        txtsoluong.ReadOnly = false;

                        // CHO PHÉP CHỈNH SỬA TRẠNG THÁI
                        textBox2.ReadOnly = false;

                        txtsoluong.Focus();
                        txtsoluong.SelectAll();

                        // Lưu Mã nguyên liệu vào Tag để dùng khi cập nhật
                        lblcapnhat.Tag = maNl;

                        // Hiển thị GroupBox
                        gbthongtinnguyenlieu.Visible = true;
                        gbthongtinnguyenlieu.BringToFront();
                    }

                    db.Dispose();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(
                "Lỗi khi hiển thị chi tiết nguyên liệu: " + ex.Message,
                    "Lỗi",
                 MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Button2: Lưu/Cập nhật nguyên liệu
        private void button2_Click(object sender, EventArgs e) {
            try {
                // Validate số lượng input(xác thực hoặc kiểm tra tính hợp lệ)
                if (string.IsNullOrWhiteSpace(txtsoluong.Text) ||
           !decimal.TryParse(txtsoluong.Text, out decimal soLuongMoi)) {
                    MessageBox.Show(
                    "Vui lòng nhập số lượng hợp lệ!",
                   "Cảnh báo",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Warning);
                    txtsoluong.Focus();
                    return;
                }

                if (soLuongMoi < 0) {
                    MessageBox.Show(
              "Số lượng phải lớn hơn hoặc bằng 0!",
                 "Cảnh báo",
              MessageBoxButtons.OK,
             MessageBoxIcon.Warning);
                    txtsoluong.Focus();
                    return;
                }

                // KIỂM TRA CHẾ ĐỘ: Update hay Add New
                if (lblcapnhat.Tag != null) {
                    // ============ CHẾ ĐỘ UPDATE (CÓ TAG) ============
                    int maNl = (int)lblcapnhat.Tag;
                    string tenNl = txtten.Text;

                    DataSqlContext db = new DataSqlContext();
                    NguyenLieu ingredient = db.NguyenLieus.Find(maNl);

                    if (ingredient == null) {
                        MessageBox.Show(
                 "Không tìm thấy nguyên liệu!",
                "Lỗi",
                 MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
                        return;
                    }

                    // Lấy số lượng cũ
                    decimal soLuongCu = ingredient.SoLuongTon != null ? ingredient.SoLuongTon.Value : 0;

                    // ========== LẤY TRẠNG THÁI MỚI TỪ textBox2 ==========
                    string tinhTrangMoi = textBox2.Text.Trim();

                    // Xác nhận trước khi lưu
                    string confirmMessage = "Xác nhận THAY ĐỔI thông tin nguyên liệu?\n\n";
                    confirmMessage = confirmMessage + "Nguyên liệu: " + tenNl + "\n";
                    confirmMessage = confirmMessage + "Số lượng cũ: " + soLuongCu.ToString() + " " + txtdonvi.Text + "\n";
                    confirmMessage = confirmMessage + "Số lượng mới: " + soLuongMoi.ToString() + " " + txtdonvi.Text;

                    if (!string.IsNullOrEmpty(tinhTrangMoi)) {
                        confirmMessage = confirmMessage + "\nTrạng thái mới: " + tinhTrangMoi;
                    }

                    DialogResult confirmResult = MessageBox.Show(
                confirmMessage,
                    "Xác nhận thay đổi",
                         MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

                    if (confirmResult != DialogResult.Yes) {
                        return;
                    }

                    // Cập nhật dữ liệu
                    ingredient.SoLuongTon = soLuongMoi;

                    // ========== CẬP NHẬT TRẠNG THÁI VÀO DATABASE ==========
                    if (!string.IsNullOrEmpty(tinhTrangMoi)) {
                        ingredient.TrangThai = tinhTrangMoi;
                    }

                    db.SaveChanges();

                    string successMsg = "Cập nhật thành công!\n\n";
                    successMsg = successMsg + "Nguyên liệu: " + tenNl + "\n";
                    successMsg = successMsg + "Số lượng cũ: " + soLuongCu.ToString() + " " + txtdonvi.Text + "\n";
                    successMsg = successMsg + "Số lượng mới: " + ingredient.SoLuongTon.ToString() + " " + txtdonvi.Text;

                    if (!string.IsNullOrEmpty(tinhTrangMoi)) {
                        successMsg = successMsg + "\nTrạng thái: " + tinhTrangMoi;
                    }

                    MessageBox.Show(
                      successMsg,
                     "Thành công",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Information);

                    db.Dispose();
                }
                else {
                    // ============ CHẾ ĐỘ ADD NEW (KHÔNG CÓ TAG) ============
                    if (string.IsNullOrWhiteSpace(txtten.Text)) {
                        MessageBox.Show(
                      "Vui lòng nhập tên nguyên liệu!",
                       "Cảnh báo",
                        MessageBoxButtons.OK,
                       MessageBoxIcon.Warning);
                        txtten.Focus();
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtdonvi.Text)) {
                        MessageBox.Show(
                         "Vui lòng nhập đơn vị (ví dụ: kg, lít, chai)!",
                            "Cảnh báo",
                       MessageBoxButtons.OK,
                             MessageBoxIcon.Warning);
                        txtdonvi.Focus();
                        return;
                    }

                    DialogResult confirmResult = MessageBox.Show(
                        "Xác nhận thêm nguyên liệu mới?\n\n" +
                      "Tên: " + txtten.Text + "\n" +
                       "Số lượng: " + soLuongMoi.ToString() + " " + txtdonvi.Text + "\n" +
                        "Đơn vị: " + txtdonvi.Text,
                      "Xác nhận thêm mới",
                       MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmResult != DialogResult.Yes) {
                        return;
                    }

                    DataSqlContext db = new DataSqlContext();
                    decimal nguongCanhBao = Math.Round(soLuongMoi / 4, 2);

                    // ========== LẤY TRẠNG THÁI MỚI TỪ textBox2 ==========
                    string trangThaiMoi = textBox2.Text.Trim();
                    if (string.IsNullOrEmpty(trangThaiMoi)) {
                        trangThaiMoi = "Đang kinh doanh"; // Mặc định
                    }

                    NguyenLieu newIngredient = new NguyenLieu {
                        TenNl = txtten.Text.Trim(),
                        DonViTinh = txtdonvi.Text.Trim(),
                        SoLuongTon = soLuongMoi,
                        NguongCanhBao = nguongCanhBao,
                        TrangThai = trangThaiMoi // Lưu trạng thái
                    };

                    db.NguyenLieus.Add(newIngredient);
                    db.SaveChanges();

                    MessageBox.Show(
                       "Thêm nguyên liệu mới thành công!\n\n" +
                     "Tên: " + newIngredient.TenNl + "\n" +
                       "Mã nguyên liệu: " + newIngredient.MaNl.ToString() + "\n" +
                       "Số lượng: " + soLuongMoi.ToString() + " " + txtdonvi.Text + "\n" +
                        "Ngưỡng cảnh báo: " + nguongCanhBao.ToString() + " " + txtdonvi.Text + "\n" +
                        "Trạng thái: " + trangThaiMoi,
                      "Thành công",
                       MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    db.Dispose();
                }

                // Làm mới dữ liệu và ẩn form
                LoadInventoryData();
                ClearInventoryForm();
                gbthongtinnguyenlieu.Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show(
            "Lỗi khi lưu nguyên liệu: " + ex.Message,
               "Lỗi",
            MessageBoxButtons.OK,
              MessageBoxIcon.Error);
            }
        }

        // Button4: Thêm mới nguyên liệu
        private void button4_Click(object sender, EventArgs e) {
            try {
                // Xóa form trước
                ClearInventoryForm();

                // Set panel sang chế độ "Add New"
                txtma.ReadOnly = true; // Mã tự động (không cho nhập)
                txtma.Text = "(Tự động)";
                txtten.ReadOnly = false; // Tên cho nhập
                txtsoluong.ReadOnly = false; // Số lượng cho nhập
                txtdonvi.ReadOnly = false; // Đơn vị cho nhập

                // ========== CHO PHÉP NHẬP TRẠNG THÁI ==========
                textBox2.ReadOnly = false;
                textBox2.Text = "Đang kinh doanh"; // Giá trị mặc định

                // Clear button2 tag để báo hiệu đây là "Add New" mode
                lblcapnhat.Tag = null;

                // Đổi text button2 thành "Lưu Mới"
                lblcapnhat.Text = "Lưu Mới";

                // Hiển thị GroupBox1
                gbthongtinnguyenlieu.Visible = true;
                gbthongtinnguyenlieu.BringToFront();

                txtten.Focus();
            }
            catch (Exception ex) {
                MessageBox.Show(
                   "Lỗi: " + ex.Message,
                 "Lỗi",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
        }

        // Xóa trắng form nguyên liệu
        private void ClearInventoryForm() {
            txtma.Clear();
            txtten.Clear();
            txtsoluong.Clear();
            txtdonvi.Clear();

            // Xóa TextBox Tình trạng (textBox2)
            textBox2.Clear();

            lblcapnhat.Tag = null;
            lblcapnhat.Text = "Cập nhật";
        }

        private void btnthem_Click(object sender, EventArgs e) {
            try {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txttensp.Text)) {
                    MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txttensp.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtdongia.Text) || !decimal.TryParse(txtdongia.Text.Replace(".", "").Replace(",", ""), out decimal donGia)) {
                    MessageBox.Show("Vui lòng nhập đơn giá hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtdongia.Focus();
                    return;
                }

                // Xác nhận thêm mới
                DialogResult confirm = MessageBox.Show(
               "Xác nhận thêm sản phẩm mới?\n\n" +
              "Tên: " + txttensp.Text + "\n" +
                "Loại: " + txtloai.Text + "\n" +
               "Đơn giá: " + donGia.ToString("N0") + " đ",
                 "Xác nhận thêm mới",
             MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) {
                    return;
                }

                DataSqlContext db = new DataSqlContext();

                // Lấy trạng thái từ textBox1
                string trangThai = txttinhtrangsp.Text.Trim();
                if (string.IsNullOrEmpty(trangThai)) {
                    trangThai = "Còn bán"; // Mặc định
                }

                SanPham newProduct = new SanPham {
                    TenSp = txttensp.Text.Trim(),
                    LoaiSp = txtloai.Text.Trim(),
                    DonGia = donGia,
                    DonVi = txtdon_vi.Text.Trim(),
                    TrangThai = trangThai
                };

                db.SanPhams.Add(newProduct);
                db.SaveChanges();

                MessageBox.Show(
                "Thêm sản phẩm mới thành công!\n\n" +
                        "Tên: " + newProduct.TenSp + "\n" +
                  "Mã sản phẩm: " + newProduct.MaSp.ToString() + "\n" +
                  "Đơn giá: " + donGia.ToString("N0") + " đ\n" +
                  "Trạng thái: " + trangThai,
                  "Thành công",
                   MessageBoxButtons.OK,
                     MessageBoxIcon.Information);

                // Refresh và ẩn form
                LoadProductData();
                gbsanpham.Visible = false;

                db.Dispose();
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsua_Click(object sender, EventArgs e) {
            try {
                // Kiểm tra xem có chọn sản phẩm không
                if (string.IsNullOrEmpty(txtmasp.Text)) {
                    MessageBox.Show("Vui lòng chọn sản phẩm từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maSp = Convert.ToInt32(txtmasp.Text);

                if (string.IsNullOrWhiteSpace(txttensp.Text)) {
                    MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txttensp.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtdongia.Text) || !decimal.TryParse(txtdongia.Text.Replace(".", "").Replace(",", ""), out decimal donGia)) {
                    MessageBox.Show("Vui lòng nhập đơn giá hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtdongia.Focus();
                    return;
                }

                // Xác nhận cập nhật
                DialogResult confirm = MessageBox.Show(
                  "Xác nhận cập nhật thông tin sản phẩm?\n\n" +
                  "Tên: " + txttensp.Text + "\n" +
                     "Loại: " + txtloai.Text + "\n" +
                    "Đơn giá: " + donGia.ToString("N0") + " đ\n" +
                  "Trạng thái: " + txttinhtrangsp.Text,
                   "Xác nhận cập nhật",
                    MessageBoxButtons.YesNo,
                  MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) {
                    return;
                }

                DataSqlContext db = new DataSqlContext();

                SanPham product = db.SanPhams.Find(maSp);

                if (product == null) {
                    MessageBox.Show("Không tìm thấy sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cập nhật thông tin
                product.TenSp = txttensp.Text.Trim();
                product.LoaiSp = txtloai.Text.Trim();
                product.DonGia = donGia;
                product.DonVi = txtdon_vi.Text.Trim();

                // Cập nhật trạng thái từ textBox1
                string trangThaiMoi = txttinhtrangsp.Text.Trim();
                if (!string.IsNullOrEmpty(trangThaiMoi)) {
                    product.TrangThai = trangThaiMoi;
                }

                db.SaveChanges();

                MessageBox.Show(
                "Cập nhật thành công!\n\n" +
                    "Tên: " + product.TenSp + "\n" +
                   "Đơn giá: " + donGia.ToString("N0") + " đ\n" +
                           "Trạng thái: " + product.TrangThai,
                  "Thành công",
                  MessageBoxButtons.OK,
                MessageBoxIcon.Information);

                // Refresh và ẩn form
                LoadProductData();
                gbsanpham.Visible = false;

                db.Dispose();
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi cập nhật sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa 2 hàm cũ và không cần thiết
        // private void btnxoa_Click(object sender, EventArgs e) { }
        // private void btnxoa_Click_1(object sender, EventArgs e) { }

        // ========== XÓA HÀM btnxoa_Click_1 CŨ VÀ THAY BẰNG CODE MỚI ==========
        private void btnxoa_Click(object sender, EventArgs e) {
    try {
                if (string.IsNullOrEmpty(txtmasp.Text)) {
    MessageBox.Show("Vui lòng chọn sản phẩm từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      return;
        }

        int maSp = Convert.ToInt32(txtmasp.Text);
 string tenSp = txttensp.Text;

                DialogResult confirm = MessageBox.Show(
      "Bạn có chắc muốn XÓA sản phẩm này?\n\n" +
                    "Tên: " + tenSp + "\n" +
    "Mã: " + maSp.ToString(),
           "Xác nhận xóa",
 MessageBoxButtons.YesNo,
         MessageBoxIcon.Warning);

              if (confirm != DialogResult.Yes) {
         return;
   }

         DataSqlContext db = new DataSqlContext();

           SanPham product = db.SanPhams.Find(maSp);

        if (product == null) {
              MessageBox.Show("Không tìm thấy sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return;
            }

       db.SanPhams.Remove(product);
                db.SaveChanges();

    MessageBox.Show("Đã xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

     // Refresh và ẩn form
         LoadProductData();
     gbsanpham.Visible = false;

       db.Dispose();
   }
catch (Exception ex) {
             MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
  }
        }

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

    private void LoadKhuyenMaiData() {
         try {
      DataSqlContext db = new DataSqlContext();

   // Lấy danh sách khuyến mãi từ database (SỬA: Không dùng ToDateTime trong LINQ)
      var khuyenMais = db.KhuyenMais
     .Select(km => new {
       km.MaKm,
        km.TenKm,
           km.MoTa,
           km.LoaiKm,
       km.GiaTri,
               km.NgayBatDau,   // Giữ nguyên DateOnly
           km.NgayKetThuc,// Giữ nguyên DateOnly
           km.TrangThai
        })
          .OrderByDescending(km => km.NgayBatDau)
      .ToList()
      // Chuyển đổi DateOnly sang DateTime SAU KHI lấy dữ liệu từ database
   .Select(km => new {
  km.MaKm,
          km.TenKm,
          km.MoTa,
          km.LoaiKm,
          km.GiaTri,
          NgayBatDau = km.NgayBatDau.ToDateTime(TimeOnly.MinValue),
          NgayKetThuc = km.NgayKetThuc.ToDateTime(TimeOnly.MinValue),
km.TrangThai
    })
      .ToList();

           dgvkhuyenmai.DataSource = khuyenMais;

  // Định dạng cột
          StyleDataGridView(dgvkhuyenmai);
  dgvkhuyenmai.Columns["MaKm"].HeaderText = "Mã KM";
dgvkhuyenmai.Columns["TenKm"].HeaderText = "Tên khuyến mãi";
      dgvkhuyenmai.Columns["MoTa"].HeaderText = "Mô tả";
     dgvkhuyenmai.Columns["LoaiKm"].HeaderText = "Loại KM";
      dgvkhuyenmai.Columns["GiaTri"].HeaderText = "Giá trị (%)";
          dgvkhuyenmai.Columns["NgayBatDau"].HeaderText = "Ngày bắt đầu";
   dgvkhuyenmai.Columns["NgayKetThuc"].HeaderText = "Ngày kết thúc";
 dgvkhuyenmai.Columns["TrangThai"].HeaderText = "Trạng thái";

 // Định dạng ngày tháng
     dgvkhuyenmai.Columns["NgayBatDau"].DefaultCellStyle.Format = "dd/MM/yyyy";
  dgvkhuyenmai.Columns["NgayKetThuc"].DefaultCellStyle.Format = "dd/MM/yyyy";

   db.Dispose();
   }
       catch (Exception ex) {
        MessageBox.Show(
   "Lỗi khi tải dữ liệu khuyến mãi: " + ex.Message,
     "Lỗi",
           MessageBoxButtons.OK,
        MessageBoxIcon.Error);
}
      }

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

        private void ShowKhuyenMaiDetailInPanel(int rowIndex) {
            try {
         DataGridViewRow row = dgvkhuyenmai.Rows[rowIndex];

    if (row.Cells["MaKm"].Value != null) {
       int maKm = Convert.ToInt32(row.Cells["MaKm"].Value);

              DataSqlContext db = new DataSqlContext();
          var khuyenMai = db.KhuyenMais.Find(maKm);

           if (khuyenMai != null) {
            txttenkm.Text = khuyenMai.TenKm;
      txtmota.Text = khuyenMai.MoTa ?? "";
     cmbloaikm.SelectedItem = khuyenMai.LoaiKm ?? "HoaDon";
   txtgiatri.Text = khuyenMai.GiaTri.ToString();
        txtgtkm.Text = "0"; // Không có GiaTriApDung trong model

 dtpngaybatdau.Value = khuyenMai.NgayBatDau.ToDateTime(TimeOnly.MinValue);
       dtpngayketthuc.Value = khuyenMai.NgayKetThuc.ToDateTime(TimeOnly.MinValue);

       cmbtrangthaikm.SelectedItem = khuyenMai.TrangThai ?? "Đang áp dụng";

              // Hiện thị GroupBox
             gbkhuyenmai.Visible = true;
 gbkhuyenmai.BringToFront();
        txttenkm.Focus();
          }

  db.Dispose();
     }
            }
       catch (Exception ex) {
     MessageBox.Show(
           "Lỗi khi hiển thị chi tiết khuyến mãi: " + ex.Message,
     "Lỗi",
          MessageBoxButtons.OK,
      MessageBoxIcon.Error);
        }
     }

        private void btnCreateAccount_Click(object sender, EventArgs e) {
     try {
       if (dtgvquanlynhanvien.SelectedRows.Count == 0) {
           MessageBox.Show("Vui lòng chọn nhân viên cần tạo tài khoản!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
     return;
    }

     int maNv = Convert.ToInt32(dtgvquanlynhanvien.SelectedRows[0].Cells["MaNv"].Value);
    string tenNv = dtgvquanlynhanvien.SelectedRows[0].Cells["TenNv"].Value.ToString();
                string taiKhoan = dtgvquanlynhanvien.SelectedRows[0].Cells["TaiKhoan"].Value.ToString();

                if (taiKhoan != "Chưa có") {
        MessageBox.Show("Nhân viên này đã có tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
     return;
                }

         // Tạo form nhập thông tin tài khoản
     using (Form promptForm = new Form()) {
        promptForm.Width = 450;
        promptForm.Height = 280;
     promptForm.Text = "Tạo tài khoản mới";
   promptForm.StartPosition = FormStartPosition.CenterParent;
    promptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
    promptForm.MaximizeBox = false;
           promptForm.MinimizeBox = false;

            Label lblInfo = new Label() {
            Text = $"Tạo tài khoản cho: {tenNv}",
        Left = 20,
        Top = 20,
        Width = 400,
 Font = new Font("Segoe UI", 10F, FontStyle.Bold)
 };

        Label lblUsername = new Label() {
                Text = "Tên đăng nhập:",
              Left = 20,
   Top = 60,
   Width = 150
       };

               TextBox txtUsername = new TextBox() {
 Left = 180,
            Top = 57,
        Width = 230
         };

        Label lblPassword = new Label() {
   Text = "Mật khẩu:",
      Left = 20,
      Top = 95,
           Width = 150
           };

        TextBox txtPassword = new TextBox() {
            Left = 180,
 Top = 92,
      Width = 230,
      UseSystemPasswordChar = true
   };

 Label lblRole = new Label() {
 Text = "Vai trò:",
              Left = 20,
           Top = 130,
       Width = 150
      };

ComboBox cmbRole = new ComboBox() {
           Left = 180,
          Top = 127,
   Width = 230,
    DropDownStyle = ComboBoxStyle.DropDownList
};

        // Load vai trò từ database
  DataSqlContext tempDb = new DataSqlContext();
    cmbRole.DataSource = tempDb.VaiTros.ToList();
          cmbRole.DisplayMember = "TenVaiTro";
                cmbRole.ValueMember = "MaVaiTro";
            tempDb.Dispose();

                  Button btnOK = new Button() {
        Text = "Tạo",
          Left = 180,
         Top = 175,
                  Width = 110,
    Height = 35,
              BackColor = Color.FromArgb(46, 125, 50),
    ForeColor = Color.White,
      DialogResult = DialogResult.OK
    };

           Button btnCancel = new Button() {
               Text = "Hủy",
          Left = 300,
    Top = 175,
            Width = 110,
      Height = 35,
              BackColor = Color.FromArgb(211, 47, 47),
   ForeColor = Color.White,
    DialogResult = DialogResult.Cancel
        };

        promptForm.Controls.Add(lblInfo);
promptForm.Controls.Add(lblUsername);
                promptForm.Controls.Add(txtUsername);
        promptForm.Controls.Add(lblPassword);
              promptForm.Controls.Add(txtPassword);
    promptForm.Controls.Add(lblRole);
            promptForm.Controls.Add(cmbRole);
    promptForm.Controls.Add(btnOK);
    promptForm.Controls.Add(btnCancel);

       if (promptForm.ShowDialog() == DialogResult.OK) {
           if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text)) {
             MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
   return;
     }

     DataSqlContext db = new DataSqlContext();

          // Kiểm tra tên đăng nhập đã tồn tại chưa
             if (db.TaiKhoans.Any(tk => tk.TenDangNhap == txtUsername.Text)) {
   MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
              return;
 }

    var newAccount = new TaiKhoan {
       MaNv = maNv,
     TenDangNhap = txtUsername.Text.Trim(),
    MatKhau = txtPassword.Text.Trim(),
     MaVaiTro = (int)cmbRole.SelectedValue,
         TrangThai = true
      };

               db.TaiKhoans.Add(newAccount);
             db.SaveChanges();

  MessageBox.Show("Tạo tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

      LoadEmployeeData();
  db.Dispose();
            }
   }
   }
    catch (Exception ex) {
       MessageBox.Show("Lỗi khi tạo tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e) {
        try {
             if (dtgvquanlynhanvien.SelectedRows.Count == 0) {
              MessageBox.Show("Vui lòng chọn nhân viên cần xóa tài khoản!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
     return;
    }

    int maNv = Convert.ToInt32(dtgvquanlynhanvien.SelectedRows[0].Cells["MaNv"].Value);
       string tenNv = dtgvquanlynhanvien.SelectedRows[0].Cells["TenNv"].Value.ToString();
    string taiKhoan = dtgvquanlynhanvien.SelectedRows[0].Cells["TaiKhoan"].Value.ToString();

     if (taiKhoan == "Chưa có") {
    MessageBox.Show("Nhân viên này chưa có tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
       return;
}

                DialogResult confirm = MessageBox.Show(
              $"Xác nhận xóa tài khoản của nhân viên: {tenNv}?",
      "Xác nhận xóa",
         MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

      if (confirm != DialogResult.Yes) {
    return;
   }

    DataSqlContext db = new DataSqlContext();
                var account = db.TaiKhoans.FirstOrDefault(tk => tk.MaNv == maNv);

                if (account != null) {
           db.TaiKhoans.Remove(account);
   db.SaveChanges();
MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
       LoadEmployeeData();
         }

   db.Dispose();
            }
      catch (Exception ex) {
  MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
 }
        }

        private void btnLogout_Click(object sender, EventArgs e) {
         DialogResult result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) {
       this.Close();
      }
      }

        private void btnExit_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
     if (result == DialogResult.Yes) {
                Application.Exit();
        }
        }

        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void lblthongtinnhanvien_Click(object sender, EventArgs e) { }

        private void txtgtkm_TextChanged(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

 private void btnthemkm_Click_1(object sender, EventArgs e) {
     try {
             // Validate input
       if (string.IsNullOrWhiteSpace(txttenkm.Text)) {
    MessageBox.Show("Vui lòng nhập tên khuyến mãi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txttenkm.Focus();
    return;
}

                if (!decimal.TryParse(txtgiatri.Text, out decimal giaTri) || giaTri < 0 || giaTri > 100) {
        MessageBox.Show("Vui lòng nhập giá trị khuyến mãi hợp lệ (0-100)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          txtgiatri.Focus();
  return;
        }

  // Xác nhận thêm mới
        DialogResult confirm = MessageBox.Show(
         "Xác nhận thêm khuyến mãi mới?\n\n" +
      "Tên: " + txttenkm.Text + "\n" +
  "Giá trị: " + giaTri.ToString() + "%",
     "Xác nhận thêm mới",
         MessageBoxButtons.YesNo,
    MessageBoxIcon.Question);

     if (confirm != DialogResult.Yes) {
               return;
    }

                DataSqlContext db = new DataSqlContext();

  var newKhuyenMai = new KhuyenMai {
       TenKm = txttenkm.Text.Trim(),
         MoTa = txtmota.Text.Trim(),
 LoaiKm = cmbloaikm.SelectedItem?.ToString() ?? "HoaDon",
              GiaTri = giaTri,
 NgayBatDau = DateOnly.FromDateTime(dtpngaybatdau.Value),
    NgayKetThuc = DateOnly.FromDateTime(dtpngayketthuc.Value),
      TrangThai = cmbtrangthaikm.SelectedItem?.ToString() ?? "Đang áp dụng"
       };

        db.KhuyenMais.Add(newKhuyenMai);
       db.SaveChanges();

       MessageBox.Show("Thêm khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

    LoadKhuyenMaiData();
     gbkhuyenmai.Visible = false;
       db.Dispose();
       }
            catch (Exception ex) {
      MessageBox.Show("Lỗi khi thêm khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
  }

        private void btnsuakm_Click_1(object sender, EventArgs e) {
  try {
   if (dgvkhuyenmai.CurrentRow == null) {
          MessageBox.Show("Vui lòng chọn khuyến mãi cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
   return;
  }

      int maKm = Convert.ToInt32(dgvkhuyenmai.CurrentRow.Cells["MaKm"].Value);

  if (string.IsNullOrWhiteSpace(txttenkm.Text)) {
MessageBox.Show("Vui lòng nhập tên khuyến mãi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
return;
          }

            if (!decimal.TryParse(txtgiatri.Text, out decimal giaTri) || giaTri < 0 || giaTri > 100) {
        MessageBox.Show("Vui lòng nhập giá trị khuyến mãi hợp lệ (0-100)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
     }

     // Xác nhận cập nhật
         DialogResult confirm = MessageBox.Show(
         "Xác nhận cập nhật khuyến mãi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) {
       return;
    }

     DataSqlContext db = new DataSqlContext();
       var khuyenMai = db.KhuyenMais.Find(maKm);

           if (khuyenMai != null) {
    khuyenMai.TenKm = txttenkm.Text.Trim();
             khuyenMai.MoTa = txtmota.Text.Trim();
            khuyenMai.LoaiKm = cmbloaikm.SelectedItem?.ToString() ?? "HoaDon";
        khuyenMai.GiaTri = giaTri;
  khuyenMai.NgayBatDau = DateOnly.FromDateTime(dtpngaybatdau.Value);
          khuyenMai.NgayKetThuc = DateOnly.FromDateTime(dtpngayketthuc.Value);
           khuyenMai.TrangThai = cmbtrangthaikm.SelectedItem?.ToString() ?? "Đang áp dụng";

         db.SaveChanges();
        MessageBox.Show("Cập nhật khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

          LoadKhuyenMaiData();
     gbkhuyenmai.Visible = false;
          }

         db.Dispose();
            }
     catch (Exception ex) {
                MessageBox.Show("Lỗi khi cập nhật khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }

        private void btnxoakm_Click_1(object sender, EventArgs e) {
            try {
        if (dgvkhuyenmai.SelectedRows.Count == 0) {
      MessageBox.Show("Vui lòng chọn khuyến mãi cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
     return;
                }

    DialogResult confirm = MessageBox.Show(
            $"Xác nhận xóa {dgvkhuyenmai.SelectedRows.Count} khuyến mãi đã chọn?",
        "Xác nhận xóa",
      MessageBoxButtons.YesNo,
     MessageBoxIcon.Warning);

      if (confirm != DialogResult.Yes) {
           return;
    }

          DataSqlContext db = new DataSqlContext();

          foreach (DataGridViewRow row in dgvkhuyenmai.SelectedRows) {
         int maKm = Convert.ToInt32(row.Cells["MaKm"].Value);
            var khuyenMai = db.KhuyenMais.Find(maKm);

             if (khuyenMai != null) {
      db.KhuyenMais.Remove(khuyenMai);
       }
       }

    db.SaveChanges();
        MessageBox.Show("Xóa khuyến mãi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

    LoadKhuyenMaiData();
                gbkhuyenmai.Visible = false;
           db.Dispose();
      }
  catch (Exception ex) {
         MessageBox.Show("Lỗi khi xóa khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}

   private void button3_Click_1(object sender, EventArgs e) {
       try {
             if (string.IsNullOrEmpty(txtmanv.Text)) {
       MessageBox.Show("Vui lòng chọn nhân viên từ danh sách trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
           return;
      }

          int maNv = Convert.ToInt32(txtmanv.Text);
      string tenDangNhap = txttentaikhoan.Text;

       if (tenDangNhap == "Chưa có") {
   MessageBox.Show("Nhân viên này chưa có tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
         return;
          }

           var confirmResult = MessageBox.Show(
       "Xác nhận cập nhật thông tin nhân viên?\n\n" +
      "Nhân viên: " + txthoten.Text + "\n" +
      "Chức vụ mới: " + txtchucvu.Text + "\n" +
  "Vai trò mới: " + txtvaitro.Text,
         "Xác nhận cập nhật",
     MessageBoxButtons.YesNo,
    MessageBoxIcon.Question);

       if (confirmResult != DialogResult.Yes) {
    return;
             }

         DataSqlContext db = new DataSqlContext();

     var nhanVien = db.NhanViens
         .Include(nv => nv.TaiKhoan)
     .Where(nv => nv.MaNv == maNv)
           .FirstOrDefault();

              if (nhanVien == null) {
  MessageBox.Show("Không tìm thấy nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
 return;
     }

        string chucVuMoi = txtchucvu.Text.Trim();
 nhanVien.ChucVu = chucVuMoi;

              if (nhanVien.TaiKhoan != null) {
      string vaiTroMoi = txtvaitro.Text.Trim();
  var vaiTro = db.VaiTros.Where(vt => vt.TenVaiTro == vaiTroMoi).FirstOrDefault();

      if (vaiTro != null) {
                    nhanVien.TaiKhoan.MaVaiTro = vaiTro.MaVaiTro;
    }
       }

        db.SaveChanges();

         MessageBox.Show(
     "Cập nhật thành công!\n\n" +
                  "Nhân viên: " + txthoten.Text + "\n" +
          "Chức vụ: " + chucVuMoi + "\n" +
   "Vai trò: " + txtvaitro.Text,
             "Thành công",
     MessageBoxButtons.OK,
 MessageBoxIcon.Information);

        LoadEmployeeData();
                db.Dispose();
            }
      catch (Exception ex) {
                MessageBox.Show("Lỗi khi cập nhật nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
        }

        // Button3: Xóa nguyên liệu
        private void button3_Click(object sender, EventArgs e) {
            try {
  // Kiểm tra xem người dùng đã chọn hàng nào trong DataGridView 'dgvkho' chưa
      if (dgvkho.SelectedRows.Count == 0) {
       MessageBox.Show(
      "Vui lòng chọn ít nhất một nguyên liệu để xóa.",
     "Chưa chọn nguyên liệu",
         MessageBoxButtons.OK,
 MessageBoxIcon.Information);
          return;
         }

 // Hỏi xác nhận trước khi xóa
        string message = $"Bạn có chắc chắn muốn xóa {dgvkho.SelectedRows.Count} nguyên liệu đã chọn không?";
                DialogResult result = MessageBox.Show(
       message,
   "Xác nhận xóa",
          MessageBoxButtons.YesNo,
    MessageBoxIcon.Warning);

// Nếu người dùng không đồng ý thì dừng lại
            if (result == DialogResult.No) {
           return;
     }

      // Bắt đầu quá trình xóa
       List<string> deletedItems = new List<string>();
        List<string> failedItems = new List<string>();

       using (var db = new DataSqlContext()) {
    // Lặp qua từng hàng được chọn
        foreach (DataGridViewRow row in dgvkho.SelectedRows) {
    int maNl = Convert.ToInt32(row.Cells["MaNl"].Value);
              string tenNl = row.Cells["TenNl"].Value.ToString();

    try {
  // Kiểm tra ràng buộc - nguyên liệu có đang được dùng trong DinhLuong không
 bool isInUse = db.DinhLuongs.Any(d => d.MaNl == maNl);

   if (isInUse) {
 failedItems.Add($"{tenNl} (Lỗi: Đang được sử dụng trong công thức định lượng)");
     }
 else {
      // Nếu không bị ràng buộc, tìm và xóa
           NguyenLieu ingredient = db.NguyenLieus.Find(maNl);
 if (ingredient != null) {
           db.NguyenLieus.Remove(ingredient);
      deletedItems.Add(tenNl);
   }
      }
       }
        catch (Exception ex) {
        failedItems.Add($"{tenNl} (Lỗi: {ex.Message})");
            }
   }

          // Lưu tất cả các thay đổi vào database
    db.SaveChanges();
                }

          // Xây dựng thông báo kết quả
    System.Text.StringBuilder summary = new System.Text.StringBuilder();

    if (deletedItems.Count > 0) {
      summary.AppendLine($"Đã xóa thành công {deletedItems.Count} nguyên liệu.");
                }

           if (failedItems.Count > 0) {
              summary.AppendLine($"\nXóa thất bại {failedItems.Count} nguyên liệu (do có ràng buộc hoặc lỗi):");
               summary.AppendLine(string.Join("\n", failedItems));
       }

     MessageBox.Show(summary.ToString(), "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Tải lại dữ liệu và ẩn panel chi tiết
         LoadInventoryData();
         gbthongtinnguyenlieu.Visible = false;
            }
            catch (Exception ex) {
              MessageBox.Show(
    "Đã xảy ra lỗi nghiêm trọng trong quá trình xóa: " + ex.Message,
               "Lỗi",
   MessageBoxButtons.OK,
           MessageBoxIcon.Error);
}
        }
    }
}
