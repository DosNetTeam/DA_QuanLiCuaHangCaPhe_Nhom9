using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class Admin : Form {
        private int selectedEmployeeId = 0;

        public Admin() {
            InitializeComponent();

            // Set form centered
            this.StartPosition = FormStartPosition.CenterScreen;

            // Subscribe to DataGridView click events
            dtgvquanlynhanvien.CellClick += dataGridView1_CellClick;
            dtgvquanlynhanvien.KeyDown += dataGridView1_KeyDown;

            // Subscribe to DataGridView click events for Inventory
            dgvkho.CellClick += dgvInventory_CellClick;
            dgvkho.KeyDown += dgvInventory_KeyDown;

            // Add Save button for updating ChucVu
            CreateSaveButton();

            // Initially hide the panel
            panel1.Visible = false;

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
                using var db = new DataSqlContext();

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

                // Format columns
                StyleDataGridView(dgvSanPham);
                dgvSanPham.Columns["MaSp"].HeaderText = "Mã SP";
                dgvSanPham.Columns["TenSp"].HeaderText = "Tên sản phẩm";
                dgvSanPham.Columns["LoaiSp"].HeaderText = "Loại";
                dgvSanPham.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvSanPham.Columns["DonVi"].HeaderText = "Đơn vị";
                dgvSanPham.Columns["TrangThai"].HeaderText = "Trạng thái";

                // Format currency column
                dgvSanPham.Columns["DonGia"].DefaultCellStyle.Format = "N0";

                // Apply conditional formatting based on status
                foreach (DataGridViewRow row in dgvSanPham.Rows) {
                    if (row.Cells["TrangThai"].Value != null) {
                        string trangThai = row.Cells["TrangThai"].Value.ToString() ?? "";

                        switch (trangThai) {
                            case "Còn bán":
                           
                                row.DefaultCellStyle.BackColor = Color.FromArgb(223, 240, 216);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                                break;
                           
                            case "Ngừng bán":
                                row.DefaultCellStyle.BackColor = Color.FromArgb(242, 222, 222);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi tải dữ liệu sản phẩm:\n{ex.Message}",
                       "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Admin_Load(object sender, EventArgs e) {
            // Load data for the first tab (Overview)
            LoadOverviewData();
        }

        private void tabControl1_SelectedIndexChanged(object? sender, EventArgs e) {
            // Load data based on selected tab
            switch (bandieukhien.SelectedIndex) {
                case 0: // Tổng quát
                    LoadOverviewData();
                    break;
                case 1: // Quản Lý Nhân Viên
                    LoadEmployeeData();
                    panel1.Visible = false;
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

        // Load overview statistics
        private void LoadOverviewData() {
            try {
                using var db = new DataSqlContext();

                // Create overview statistics
                var overviewData = new[]
  {
         new {
    Category = "Tổng số nhân viên",
         Count = db.NhanViens.Count(),
        Details = "Nhân viên đang làm việc"
   },
               new {
       Category = "Tổng số sản phẩm",
              Count = db.SanPhams.Count(s => s.TrangThai == "Con ban"),
   Details = "Sản phẩm đang bán"
       },
             new {
        Category = "Tổng số đơn hàng",
    Count = db.DonHangs.Count(),
    Details = "Tất cả đơn hàng"
          },
   new {
  Category = "Đơn hàng hôm nay",
     Count = db.DonHangs
       .AsEnumerable()
        .Count(d => d.NgayLap.HasValue && d.NgayLap.Value.Date == DateTime.Today),
      Details = DateTime.Today.ToString("dd/MM/yyyy")
      },
         new {
         Category = "Nguyên liệu trong kho",
 Count = db.NguyenLieus.Count(),
           Details = "Tổng số loại nguyên liệu"
            },
               new {
    Category = "Khách hàng",
       Count = db.KhachHangs.Count(),
   Details = "Tổng số khách hàng"
   }
        };

                tongquandulieu.DataSource = overviewData;

                // Format columns
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

        // Load employee data from database
        private void LoadEmployeeData() {
            try {
                using var db = new DataSqlContext();

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

                // Format columns
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
                MessageBox.Show(
          $"Lỗi khi tải dữ liệu nhân viên:\n{ex.Message}",
        "Lỗi",
        MessageBoxButtons.OK,
     MessageBoxIcon.Error);
            }
        }

        // Load inventory data from database
        private void LoadInventoryData() {
            try {
                using var db = new DataSqlContext();

                // ========== BỎ ĐIỀU KIỆN LỌC TrangThai ==========
                // Lấy TẤT CẢ nguyên liệu (không lọc TrangThai)
                var inventory = db.NguyenLieus
                .Select(nl => new {
                    nl.MaNl,
                    nl.TenNl,
                    nl.DonViTinh,
                    SoLuongTon = Math.Round(nl.SoLuongTon ?? 0, 2),
                    NguongCanhBao = Math.Round(nl.NguongCanhBao ?? 0, 2)
                })
                    .ToList();

                // ========== TÍNH TOÁN TÌNH TRẠNG DỰA TRÊN TỶ LỆ % ==========
                var inventoryWithStatus = inventory.Select(nl => {
                    string tinhTrang;

                    // Tính tỷ lệ % so với ngưỡng cảnh báo
                    decimal nguongCanhBao = nl.NguongCanhBao > 0 ? nl.NguongCanhBao : 100;

                    // Tính số lượng tương ứng với 1/3 và 2/3 của ngưỡng
                    decimal motPhanBa = nguongCanhBao / 3; // 1/3
                    decimal haiPhanBa = (nguongCanhBao * 2) / 3; // 2/3

                    // Xác định tình trạng
                    if (nl.SoLuongTon <= 0) {
                        tinhTrang = "Hết hàng"; // Đỏ
                    }
                    else if (nl.SoLuongTon <= motPhanBa) {
                        tinhTrang = "Hết hàng"; // Đỏ - Còn ≤ 1/3
                    }
                    else if (nl.SoLuongTon <= haiPhanBa) {
                        tinhTrang = "Thiếu thốn"; // Vàng - Còn ≤ 2/3
                    }
                    else {
                        tinhTrang = "Dồi dào"; // Xanh - Còn > 2/3
                    }

                    return new {
                        nl.MaNl,
                        nl.TenNl,
                        nl.DonViTinh,
                        nl.SoLuongTon,
                        nl.NguongCanhBao,
                        TinhTrang = tinhTrang
                    };
                })
                      .OrderBy(nl => nl.SoLuongTon)
                    .ToList();

                dgvkho.DataSource = inventoryWithStatus;

                // Format columns
                StyleDataGridView(dgvkho);
                dgvkho.Columns["MaNl"].HeaderText = "Mã NL";
                dgvkho.Columns["TenNl"].HeaderText = "Tên nguyên liệu";
                dgvkho.Columns["DonViTinh"].HeaderText = "Đơn vị";
                dgvkho.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
                dgvkho.Columns["NguongCanhBao"].HeaderText = "Ngưỡng cảnh báo";
                dgvkho.Columns["TinhTrang"].HeaderText = "Tình trạng";

                // ========== ÁP DỤNG MÀU SẮC ==========
                foreach (DataGridViewRow row in dgvkho.Rows) {
                    if (row.Cells["TinhTrang"].Value != null) {
                        string tinhTrang = row.Cells["TinhTrang"].Value.ToString() ?? "";

                        switch (tinhTrang) {
                            case "Dồi dào":
                                // Xanh lá - Còn > 2/3
                                row.DefaultCellStyle.BackColor = Color.FromArgb(223, 240, 216);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                                break;
                            case "Thiếu thốn":
                                // Vàng cam - Còn ≤ 2/3 nhưng > 1/3
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 205);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(156, 87, 0);
                                break;
                            case "Hết hàng":
                                // Đỏ - Còn ≤ 1/3 hoặc hết
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(183, 28, 28);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(
                 $"Lỗi khi tải dữ liệu kho:\n{ex.Message}",
                     "Lỗi",
                             MessageBoxButtons.OK,
                      MessageBoxIcon.Error);
            }
        }

        // Load revenue data from database
        private void LoadRevenueData() {
            try {
                using var db = new DataSqlContext();

                var revenueData = db.DonHangs
                          .Where(dh => dh.TongTien.HasValue && dh.NgayLap.HasValue)
                  .ToList()
                     .GroupBy(dh => new {
                         Year = dh.NgayLap!.Value.Year,
                         Month = dh.NgayLap!.Value.Month
                     })
                       .Select(g => new {
                           Thang = $"{g.Key.Month:00}/{g.Key.Year}",
                           SoDonHang = g.Count(),
                           TongDoanhThu = Math.Round(g.Sum(dh => dh.TongTien ?? 0), 0),
                           DoanhThuTrungBinh = Math.Round(g.Average(dh => dh.TongTien ?? 0), 0),
                           DonHangLonNhat = Math.Round(g.Max(dh => dh.TongTien ?? 0), 0),
                           DonHangNhoNhat = Math.Round(g.Min(dh => dh.TongTien ?? 0), 0)
                       })
                                 .OrderByDescending(x => x.Thang)
                        .Take(12)
                .ToList();

                dgvdoanhthu.DataSource = revenueData;

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
            }
            catch (Exception ex) {
                MessageBox.Show(
          $"Lỗi khi tải dữ liệu doanh thu:\n{ex.Message}",
                    "Lỗi",
               MessageBoxButtons.OK,
        MessageBoxIcon.Error);
            }
        }

        // Apply consistent styling to DataGridViews
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

        // Event handler when user clicks on inventory row (Nguyên Liệu)
        private void dgvInventory_CellClick(object? sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                ShowInventoryDetailInPanel(e.RowIndex);
            }
        }

        // Event handler for keyboard navigation in inventory grid
        private void dgvInventory_KeyDown(object? sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && dgvkho.CurrentRow != null) {
                e.Handled = true;
                ShowInventoryDetailInPanel(dgvkho.CurrentRow.Index);
            }
        }

        // ========== HÀM HIỂN THỊ CHI TIẾT SẢN PHẨM ==========
        private void ShowProductDetailInPanel(int rowIndex) {
            try {
                var row = dgvSanPham.Rows[rowIndex];

                if (row.Cells["MaSp"].Value != null) {
                    int maSp = Convert.ToInt32(row.Cells["MaSp"].Value);

                    using var db = new DataSqlContext();

                    var product = db.SanPhams
                 .Where(sp => sp.MaSp == maSp)
                 .FirstOrDefault();

                    if (product != null) {
                        // Cập nhật các control trong GroupBox sản phẩm
                        txtmasp.Text = product.MaSp.ToString();
                        txttensp.Text = product.TenSp;
                        txtgia.Text = product.DonGia.ToString("N0");
                        txtloai.Text = product.LoaiSp ?? "";
                        txtdongia.Text = product.DonGia.ToString("N0");
                        txtdon_vi.Text = product.DonVi ?? "";
                        lbltrangthai.Text = $"Trạng thái: {product.TrangThai ?? "N/A"}";

                        // Mã SP readonly (không cho sửa)
                        txtmasp.ReadOnly = true;
    
          // CÁC TRƯỜNG KHÁC CHO PHÉP SỬA (để dùng nút Sửa)
          txttensp.ReadOnly = false;
     txtloai.ReadOnly = false;
        txtdongia.ReadOnly = false;
  txtdon_vi.ReadOnly = false;
    
      // txtgia giống txtdongia nên cũng cho sửa
        txtgia.ReadOnly = false;

      // Hiển thị GroupBox sản phẩm
  gbsanpham.Visible = true;
   gbsanpham.BringToFront();
 
              // Focus vào tên sản phẩm để sẵn sàng chỉnh sửa
   txttensp.Focus();
             txttensp.SelectAll();
                    }
   }
  }
            catch (Exception ex) {
                MessageBox.Show(
      $"Lỗi khi hiển thị chi tiết sản phẩm:\n{ex.Message}",
           "Lỗi",
       MessageBoxButtons.OK,
       MessageBoxIcon.Error);
        }
        }

        private void ShowEmployeeDetailInPanel(int rowIndex) {
            try {
                var row = dtgvquanlynhanvien.Rows[rowIndex];

                if (row.Cells["MaNv"].Value != null) {
                    int maNv = Convert.ToInt32(row.Cells["MaNv"].Value);
                    selectedEmployeeId = maNv;

                    using var db = new DataSqlContext();

                    var employee = db.NhanViens
                            .Where(nv => nv.MaNv == maNv)
                .Select(nv => new {
                    nv.MaNv,
                    nv.TenNv,
                    nv.ChucVu,
                    nv.SoDienThoai,
                    TenDangNhap = nv.TaiKhoan != null ? nv.TaiKhoan.TenDangNhap : "Chưa có",
                    MatKhau = nv.TaiKhoan != null ? nv.TaiKhoan.MatKhau : "",
                    VaiTro = nv.TaiKhoan != null ? nv.TaiKhoan.MaVaiTroNavigation.TenVaiTro : "N/A",
                    TrangThai = nv.TaiKhoan != null ?
                            (nv.TaiKhoan.TrangThai == true ? "Hoạt động" : "Bị khóa") : "N/A",
                    SoDonHang = nv.DonHangs.Count()
                })
                          .FirstOrDefault();

                    if (employee != null) {
                        // Update TextBoxes in panel1
                        txtmanv.Text = employee.MaNv.ToString();
                        txthoten.Text = employee.TenNv;
                        txtsdt.Text = string.IsNullOrEmpty(employee.SoDienThoai) ? "Chưa cập nhật" : employee.SoDienThoai;
                        txtchucvu.Text = employee.ChucVu;
                        txttentaikhoan.Text = employee.TenDangNhap;
                        txtvaitro.Text = employee.VaiTro;

                        //      // Hiển thị mật khẩu (readonly ban đầu)
                        txtmatkhau.Text = employee.MatKhau;
                        txtmatkhau.ReadOnly = true;
                        txtmatkhau.UseSystemPasswordChar = true;

                        // Show panel1
                        panel1.Visible = true;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(
         $"Lỗi khi hiển thị chi tiết:\n{ex.Message}",
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
                        // Validate input
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

                        // Update password in database
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

                        // Verify old password
                        if (taiKhoan.MatKhau != txtOldPass.Text) {
                            MessageBox.Show(
                      "Mật khẩu hiện tại không đúng!",
                           "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                            return;
                        }

                        // Confirm change
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

                        // Update password
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

                        // Update textbox to show new password
                        txtmatkhau.Text = txtNewPass.Text;
                        txtmatkhau.ReadOnly = true;
                        txtmatkhau.UseSystemPasswordChar = true;

                        // Refresh employee data
                        LoadEmployeeData();
                    }
                    else {
                        // User cancelled - restore readonly state
                        txtmatkhau.ReadOnly = true;
                        txtmatkhau.UseSystemPasswordChar = true;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(
                                 $"Lỗi khi đổi mật khẩu:\n{ex.Message}\n\n" +
                     $"Chi tiết: {ex.InnerException?.Message}",
                      "Lỗi",
                  MessageBoxButtons.OK,
                              MessageBoxIcon.Error);

                // Restore readonly state on error
                if (txtmatkhau != null) {
                    txtmatkhau.ReadOnly = true;
                    txtmatkhau.UseSystemPasswordChar = true;
                }
            }
        }

        // Show inventory detail in panel2
        private void ShowInventoryDetailInPanel(int rowIndex) {
            try {
                var row = dgvkho.Rows[rowIndex];

                // Get ingredient ID from the row
                if (row.Cells["MaNl"].Value != null) {
                    int maNl = Convert.ToInt32(row.Cells["MaNl"].Value);

                    // Load ingredient details from database
                    using var db = new DataSqlContext();

                    var ingredient = db.NguyenLieus
             .Where(nl => nl.MaNl == maNl)
               .FirstOrDefault();

                    if (ingredient != null) {
                        // Update TextBoxes in groupBox1
                        txtma.Text = ingredient.MaNl.ToString();
                        txtten.Text = ingredient.TenNl;
                        txtsoluong.Text = ingredient.SoLuongTon.ToString();
                        txtdonvi.Text = ingredient.DonViTinh;

                        // Make Mã, Tên, Đơn vị readonly (không cho sửa)
                        txtma.ReadOnly = true;
                        txtten.ReadOnly = true;
                        txtdonvi.ReadOnly = true;

                        // Chỉ cho phép sửa Số Lượng
                        txtsoluong.ReadOnly = false;
                        txtsoluong.Focus();
                        txtsoluong.SelectAll();

                        // Store ingredient ID for update
                        lblcapnhat.Tag = maNl;

                        // Hiển thị GroupBox1 nguyên liệu
                        gbthongtinnguyenlieu.Visible = true;
                        gbthongtinnguyenlieu.BringToFront();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(
                  $"Lỗi khi hiển thị chi tiết nguyên liệu:\n{ex.Message}",
              "Lỗi",
                MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            try {
                // Validate số lượng input
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

                // **KIỂM TRA CHẾ ĐỘ: Update hay Add New**
                if (lblcapnhat.Tag != null) {
                    // ============ CHẾ ĐỘ UPDATE (CÓ TAG) - THAY ĐỔI SỐ LƯỢNG TỒN ============
                    int maNl = (int)lblcapnhat.Tag;
                    string tenNl = txtten.Text;

                    using var db = new DataSqlContext();
                    var ingredient = db.NguyenLieus.Find(maNl);

                    if (ingredient == null) {
                        MessageBox.Show(
              "Không tìm thấy nguyên liệu!",
              "Lỗi",
               MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                        return;
                    }

                    // Lấy số lượng cũ để hiển thị
                    decimal soLuongCu = ingredient.SoLuongTon ?? 0;

                    // Hỏi xác nhận TRƯỚC KHI thay đổi
                    var confirmResult = MessageBox.Show(
                  $@"Xác nhận THAY ĐỔI số lượng tồn kho?

Nguyên liệu: {tenNl}
Số lượng cũ: {soLuongCu} {txtdonvi.Text}
Số lượng mới: {soLuongMoi} {txtdonvi.Text}

Lưu ý: Số lượng tồn sẽ được ĐẶT LẠI thành {soLuongMoi} {txtdonvi.Text}",
                       "Xác nhận thay đổi",
                 MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question);

                    if (confirmResult != DialogResult.Yes) return;

                    // ========== THAY ĐỔI SỐ LƯỢNG TỒN (KHÔNG PHẢI CỘNG THÊM) ==========
                    ingredient.SoLuongTon = soLuongMoi; // ← GÁN GIÁ TRỊ MỚI thay vì +=

                    db.SaveChanges();

                    MessageBox.Show(
              $"Cập nhật số lượng tồn thành công!\n\n" +
                  $"Nguyên liệu: {tenNl}\n" +
                     $"Số lượng cũ: {soLuongCu} {txtdonvi.Text}\n" +
                      $"Số lượng mới: {ingredient.SoLuongTon} {txtdonvi.Text}",
                       "Thành công",
               MessageBoxButtons.OK,
             MessageBoxIcon.Information);
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

                    var confirmResult = MessageBox.Show(
                           $"Xác nhận thêm nguyên liệu mới?\n\n" +
                        $"Tên: {txtten.Text}\n" +
             $"Số lượng: {soLuongMoi} {txtdonvi.Text}\n" +
                          $"Đơn vị: {txtdonvi.Text}",
              "Xác nhận thêm mới",
                         MessageBoxButtons.YesNo,
                          MessageBoxIcon.Question);

                    if (confirmResult != DialogResult.Yes) return;

                    using var db = new DataSqlContext();
                    decimal nguongCanhBao = Math.Round(soLuongMoi / 4, 2);

                    var newIngredient = new NguyenLieu {
                        TenNl = txtten.Text.Trim(),
                        DonViTinh = txtdonvi.Text.Trim(),
                        SoLuongTon = soLuongMoi,
                        NguongCanhBao = nguongCanhBao
                    };

                    db.NguyenLieus.Add(newIngredient);
                    db.SaveChanges();

                    MessageBox.Show(
                    $"Thêm nguyên liệu mới thành công!\n\n" +
                          $"Tên: {newIngredient.TenNl}\n" +
                      $"Mã nguyên liệu: {newIngredient.MaNl}\n" +
                 $"Số lượng: {soLuongMoi} {txtdonvi.Text}\n" +
                       $"Ngưỡng cảnh báo: {nguongCanhBao} {txtdonvi.Text}",
                "Thành công",
                         MessageBoxButtons.OK,
                       MessageBoxIcon.Information);
                }

                // Refresh data and hide panel
                LoadInventoryData();
                ClearInventoryForm();
                gbthongtinnguyenlieu.Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show(
           $"Lỗi khi lưu nguyên liệu:\n{ex.Message}",
                        "Lỗi",
              MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
        }

        // Clear inventory form
        private void ClearInventoryForm() {
            txtma.Clear();
            txtten.Clear();
            txtsoluong.Clear();
            txtdonvi.Clear();

            lblcapnhat.Tag = null;
            lblcapnhat.Text = "Cập nhật";
        }

        // Button4: Hiển thị form trống để thêm nguyên liệu mới
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
          $"Lỗi:\n{ex.Message}",
        "Lỗi",
              MessageBoxButtons.OK,
             MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            // Check if an ingredient is selected
            if (dgvkho.SelectedRows.Count == 0) {
                MessageBox.Show(
           "Vui lòng chọn nguyên liệu cần ngừng kinh doanh!",
        "Thông báo",
         MessageBoxButtons.OK,
       MessageBoxIcon.Warning);
                return;
            }

            try {
                var row = dgvkho.SelectedRows[0];
                if (row.Cells["MaNl"].Value == null) return;

                int maNl = Convert.ToInt32(row.Cells["MaNl"].Value);
                string tenNl = row.Cells["TenNl"].Value?.ToString() ?? "N/A";

                // Confirm "soft delete"
                var confirmResult = MessageBox.Show(
             $@"Bạn có chắc muốn NGỪNG KINH DOANH nguyên liệu này?Mã: {maNl}Tên: {tenNl}Hành động này sẽ:- Ẩn nguyên liệu khỏi kho và các form bán hàng.- KHÔNG xóa lịch sử nhập kho.- KHÔNG xóa các công thức cũ.", "Xác nhận Ngừng kinh doanh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes) return;

                using var db = new DataSqlContext();

                var ingredient = db.NguyenLieus.Find(maNl);

                if (ingredient == null) {
                    MessageBox.Show(
                         "Không tìm thấy nguyên liệu!",
                       "Lỗi",
                                MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                    return;
                }

                // Cập nhật trạng thái (thay vì xóa)
                ingredient.TrangThai = "Ngừng kinh doanh";
                db.SaveChanges();

                MessageBox.Show(
                  $"Đã ngừng kinh doanh nguyên liệu '{tenNl}'!",
            "Thành công",
                MessageBoxButtons.OK,
                  MessageBoxIcon.Information);

                // Refresh data
                LoadInventoryData();
                ClearInventoryForm();

                // Ẩn GroupBox1 sau khi xóa
                gbthongtinnguyenlieu.Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show(
                      $"Lỗi khi xóa nguyên liệu:\n{ex.Message}\n\n" +
                $"Chi tiết: {ex.InnerException?.Message}",
                     "Lỗi",
            MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
        }

        private void btnCreateAccount_Click(object sender, EventArgs e) {
            try {
                // Open create employee dialog
                CreateEmployeeForm createForm = new CreateEmployeeForm();

                if (createForm.ShowDialog() == DialogResult.OK) {
                    // Refresh employee data if on employee tab
                    if (bandieukhien.SelectedIndex == 1) {
                        LoadEmployeeData();
                    }
                    else {
                        // If not on employee tab, show message to navigate there
                        MessageBox.Show(
                                "Tài khoản đã được tạo thành công!\n" +
                               "Chuyển sang tab 'Quản Lý Nhân Viên' để xem danh sách.",
                       "Thành công",
                                 MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(
   $"Lỗi khi tạo tài khoản:\n{ex.Message}",
         "Lỗi",
   MessageBoxButtons.OK,
   MessageBoxIcon.Error);
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e) {
            // Check if an employee is selected
            if (dtgvquanlynhanvien.SelectedRows.Count == 0) {
                MessageBox.Show(
                      "Vui lòng chọn nhân viên cần vô hiệu hóa!",
                "Thông báo",
                     MessageBoxButtons.OK,
                         MessageBoxIcon.Warning);
                return;
            }

            try {
                var row = dtgvquanlynhanvien.SelectedRows[0];
                if (row.Cells["MaNv"].Value == null) return;

                int maNv = Convert.ToInt32(row.Cells["MaNv"].Value);
                string tenNv = row.Cells["TenNv"].Value?.ToString() ?? "N/A";

                // Confirm "soft delete"
                var confirmResult = MessageBox.Show(
                     $@"Bạn có chắc muốn VÔ HIỆU HÓA nhân viên này?

Mã NV: {maNv}
Họ tên: {tenNv}

Hành động này sẽ:
- Cập nhật trạng thái nhân viên thành 'Đã nghỉ việc'.
- Khóa tài khoản đăng nhập (nếu có).
- KHÔNG xóa đơn hàng cũ của họ.",
                "Xác nhận Vô hiệu hóa",
                        MessageBoxButtons.YesNo,
                   MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes) return;

                using var db = new DataSqlContext();

                // Find employee and their account
                var nhanVien = db.NhanViens
          .Include(nv => nv.TaiKhoan)
                    .FirstOrDefault(nv => nv.MaNv == maNv);

                if (nhanVien == null) {
                    MessageBox.Show(
                             "Không tìm thấy nhân viên trong cơ sở dữ liệu!",
                      "Lỗi",
                           MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }

                // Cập nhật trạng thái nhân viên
                nhanVien.TrangThai = "Đã nghỉ việc";

                // Khóa tài khoản (thay vì xóa)
                if (nhanVien.TaiKhoan != null) {
                    nhanVien.TaiKhoan.TrangThai = false; // false = Bị khóa
                }

                db.SaveChanges();

                MessageBox.Show(
             $"Đã vô hiệu hóa nhân viên '{tenNv}' thành công!",
       "Thành công",
          MessageBoxButtons.OK,
            MessageBoxIcon.Information);

                // Refresh the employee list
                LoadEmployeeData();

                // Hide detail panel if visible
                panel1.Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show(
                $"Lỗi khi vô hiệu hóa nhân viên:\n{ex.Message}\n\n" +
     $"Chi tiết: {ex.InnerException?.Message}",
       "Lỗi",
      MessageBoxButtons.OK,
              MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e) {
            // Xác nhận đăng xuất
            var confirmResult = MessageBox.Show(
           "Bạn có chắc muốn đăng xuất?\n\n" + "Hệ thống sẽ quay về trang đăng nhập.",
               "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;
            else {
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show(
      "Bạn có chắc muốn thoát?",
        "Xác nhận",
  MessageBoxButtons.YesNo,
         MessageBoxIcon.Question);

            if (result == DialogResult.Yes) {
                Application.Exit();
            }
        }
        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }

        // Xử lý sự kiện chuyển tab trong tabControlKho
        private void TabControlKho_SelectedIndexChanged(object? sender, EventArgs e) {
            try {
                switch (tabControlKho.SelectedIndex) {
                    case 0: // Tab Nguyên liệu
                        LoadInventoryData();
                        gbthongtinnguyenlieu.Visible = false; // Ẩn form nhập liệu ban đầu
                        break;
                    case 1: // Tab Sản phẩm
                        LoadProductData();
                        gbsanpham.Visible = false; // Ẩn form nhập liệu ban đầu
                        break;
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi khi chuyển tab: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== XỬ LÝ TAB KHUYẾN MÃI ==========

        // Hàm tải dữ liệu khuyến mãi từ database
        private void LoadKhuyenMaiData() {
            try {
                using var db = new DataSqlContext();

                // Lấy toàn bộ dữ liệu khuyến mãi từ database
                var khuyenMais = db.KhuyenMais
       .Select(km => new {
           km.MaKm,   // Mã khuyến mãi
           km.TenKm,       // Tên khuyến mãi  
           km.MoTa,    // Mô tả
           km.LoaiKm,      // Loại: HoaDon hoặc SanPham
           km.GiaTri,   // Giá trị giảm (%)
           km.NgayBatDau,  // Ngày bắt đầu
           km.NgayKetThuc, // Ngày kết thúc
           km.TrangThai    // Trạng thái
       })
  .OrderByDescending(km => km.NgayBatDau) // Sắp xếp theo ngày mới nhất
        .ToList();

                // Gán dữ liệu vào DataGridView
                dgvkhuyenmai.DataSource = khuyenMais;

                // Format các cột hiển thị
                StyleDataGridView(dgvkhuyenmai);

                // Ẩn cột MaKm (không cần hiển thị)
                if (dgvkhuyenmai.Columns["MaKm"] != null)
                    dgvkhuyenmai.Columns["MaKm"].Visible = false;

                // Đặt tên tiêu đề cho các cột
                if (dgvkhuyenmai.Columns["TenKm"] != null)
                    dgvkhuyenmai.Columns["TenKm"].HeaderText = "Tên Khuyến Mãi";
                if (dgvkhuyenmai.Columns["MoTa"] != null)
                    dgvkhuyenmai.Columns["MoTa"].HeaderText = "Mô Tả";
                if (dgvkhuyenmai.Columns["LoaiKm"] != null)
                    dgvkhuyenmai.Columns["LoaiKm"].HeaderText = "Loại KM";
                if (dgvkhuyenmai.Columns["GiaTri"] != null)
                    dgvkhuyenmai.Columns["GiaTri"].HeaderText = "Giá Trị (%)";
                if (dgvkhuyenmai.Columns["NgayBatDau"] != null)
                    dgvkhuyenmai.Columns["NgayBatDau"].HeaderText = "Ngày Bắt Đầu";
                if (dgvkhuyenmai.Columns["NgayKetThuc"] != null)
                    dgvkhuyenmai.Columns["NgayKetThuc"].HeaderText = "Ngày Kết Thúc";
                if (dgvkhuyenmai.Columns["TrangThai"] != null)
                    dgvkhuyenmai.Columns["TrangThai"].HeaderText = "Trạng Thái";
            }
            catch (Exception ex) {
                MessageBox.Show(
          $"Lỗi khi tải dữ liệu khuyến mãi:\n{ex.Message}",
           "Lỗi",
    MessageBoxButtons.OK,
       MessageBoxIcon.Error);
            }
        }

        // Sự kiện khi click vào một dòng trong DataGridView khuyến mãi
        private void dgvkhuyenmai_CellClick(object? sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                ShowKhuyenMaiDetail(e.RowIndex);
            }
        }

        // Sự kiện khi nhấn Enter trên DataGridView khuyến mãi
        private void dgvkhuyenmai_KeyDown(object? sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && dgvkhuyenmai.CurrentRow != null) {
                e.Handled = true;
                ShowKhuyenMaiDetail(dgvkhuyenmai.CurrentRow.Index);
            }
        }

        // Hiển thị chi tiết khuyến mãi vào form nhập liệu
        private void ShowKhuyenMaiDetail(int rowIndex) {
            try {
                var row = dgvkhuyenmai.Rows[rowIndex];

                // Kiểm tra có dữ liệu không
                if (row.Cells["MaKm"].Value == null) return;

                int maKm = Convert.ToInt32(row.Cells["MaKm"].Value);

                using var db = new DataSqlContext();

                // Lấy thông tin chi tiết khuyến mãi từ database
                var khuyenMai = db.KhuyenMais
           .Where(km => km.MaKm == maKm)
             .FirstOrDefault();

                if (khuyenMai != null) {
                    // Điền dữ liệu vào các control
                    txttenkm.Text = khuyenMai.TenKm;
                    txtmota.Text = khuyenMai.MoTa ?? "";
                    cmbloaikm.SelectedItem = khuyenMai.LoaiKm;
                    txtgiatri.Text = khuyenMai.GiaTri.ToString();

                    // Convert DateOnly sang DateTime cho DateTimePicker
                    dtpngaybatdau.Value = khuyenMai.NgayBatDau.ToDateTime(TimeOnly.MinValue);
                    dtpngayketthuc.Value = khuyenMai.NgayKetThuc.ToDateTime(TimeOnly.MinValue);

                    cmbtrangthaikm.SelectedItem = khuyenMai.TrangThai;

                    // Lưu MaKm vào Tag để biết đang ở chế độ sửa
                    gbkhuyenmai.Tag = maKm;

                    // Hiển thị GroupBox
                    gbkhuyenmai.Visible = true;
                    gbkhuyenmai.BringToFront();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(
              $"Lỗi khi hiển thị chi tiết khuyến mãi:\n{ex.Message}",
                   "Lỗi",
                        MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
            }
        }

        // Nút THÊM khuyến mãi mới
        private void btnthemkm_Click(object sender, EventArgs e) {
            try {
                // ========== VALIDATE DỮ LIỆU NHẬP VÀO ==========

                // Kiểm tra tên khuyến mãi
                if (string.IsNullOrWhiteSpace(txttenkm.Text)) {
                    MessageBox.Show(
                      "Vui lòng nhập tên khuyến mãi!",
                     "Cảnh báo",
                 MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);
                    txttenkm.Focus();
                    return;
                }

                // Kiểm tra loại khuyến mãi
                if (cmbloaikm.SelectedItem == null) {
                    MessageBox.Show(
                      "Vui lòng chọn loại khuyến mãi!",
                   "Cảnh báo",
             MessageBoxButtons.OK,
                      MessageBoxIcon.Warning);
                    cmbloaikm.Focus();
                    return;
                }

                // Kiểm tra giá trị giảm
                if (!decimal.TryParse(txtgiatri.Text, out decimal giaTri) || giaTri <= 0 || giaTri > 100) {
                    MessageBox.Show(
            "Giá trị giảm phải là số từ 0 đến 100!",
                   "Cảnh báo",
             MessageBoxButtons.OK,
         MessageBoxIcon.Warning);
                    txtgiatri.Focus();
                    return;
                }

                // Kiểm tra ngày kết thúc phải sau ngày bắt đầu
                if (dtpngayketthuc.Value <= dtpngaybatdau.Value) {
                    MessageBox.Show(
                    "Ngày kết thúc phải sau ngày bắt đầu!",
                       "Cảnh báo",
                           MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                    dtpngayketthuc.Focus();
                    return;
                }

                // Kiểm tra trạng thái
                if (cmbtrangthaikm.SelectedItem == null) {
                    MessageBox.Show(
                             "Vui lòng chọn trạng thái!",
                     "Cảnh báo",
                         MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                    cmbtrangthaikm.Focus();
                    return;
                }

                // ========== XÁC NHẬN THÊM MỚI ==========
                var confirmResult = MessageBox.Show(
                       $@"Xác nhận thêm khuyến mãi mới?

Tên: {txttenkm.Text}
Loại: {cmbloaikm.SelectedItem}
Giá trị: {giaTri}%
Thời gian: {dtpngaybatdau.Value:dd/MM/yyyy} - {dtpngayketthuc.Value:dd/MM/yyyy}
Trạng thái: {cmbtrangthaikm.SelectedItem}",
                     "Xác nhận thêm mới",
                MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes) return;

                // ========== THÊM VÀO DATABASE ==========
                using var db = new DataSqlContext();

                var newKhuyenMai = new KhuyenMai {
                    TenKm = txttenkm.Text.Trim(),
                    MoTa = txtmota.Text.Trim(),
                    LoaiKm = cmbloaikm.SelectedItem.ToString(),
                    GiaTri = giaTri,
                    NgayBatDau = DateOnly.FromDateTime(dtpngaybatdau.Value),
                    NgayKetThuc = DateOnly.FromDateTime(dtpngayketthuc.Value),
                    TrangThai = cmbtrangthaikm.SelectedItem.ToString()
                };

                db.KhuyenMais.Add(newKhuyenMai);
                db.SaveChanges();

                MessageBox.Show(
                 $"Thêm khuyến mãi thành công!\n\nMã KM: {newKhuyenMai.MaKm}\nTên: {newKhuyenMai.TenKm}",
                   "Thành công",
               MessageBoxButtons.OK,
          MessageBoxIcon.Information);

                // Tải lại dữ liệu và ẩn form
                LoadKhuyenMaiData();
                ClearKhuyenMaiForm();
                gbkhuyenmai.Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show(
       $"Lỗi khi thêm khuyến mãi:\n{ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
                 "Lỗi",
     MessageBoxButtons.OK,
             MessageBoxIcon.Error);
            }
        }

        // Nút SỬA khuyến mãi
        private void btnsuakm_Click(object sender, EventArgs e) {
            try {
                // Kiểm tra xem có đang ở chế độ sửa không (có MaKm trong Tag)
                if (gbkhuyenmai.Tag == null) {
                    MessageBox.Show(
                 "Vui lòng chọn khuyến mãi cần sửa từ danh sách!",
                           "Thông báo",
                 MessageBoxButtons.OK,
               MessageBoxIcon.Warning);
                    return;
                }

                int maKm = (int)gbkhuyenmai.Tag;

                // ========== VALIDATE DỮ LIỆU ==========
                if (string.IsNullOrWhiteSpace(txttenkm.Text)) {
                    MessageBox.Show("Vui lòng nhập tên khuyến mãi!", "Cảnh báo",
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
 txttenkm.Focus();
   return;
    }

    if (cmbloaikm.SelectedItem == null) {
           MessageBox.Show("Vui lòng chọn loại khuyến mãi!", "Cảnh báo",
      MessageBoxButtons.OK, MessageBoxIcon.Warning);
      cmbloaikm.Focus();
 return;
   }

 if (!decimal.TryParse(txtgiatri.Text, out decimal giaTri) || giaTri <= 0 || giaTri > 100) {
       MessageBox.Show("Giá trị giảm phải từ 0 đến 100!", "Cảnh báo",
MessageBoxButtons.OK, MessageBoxIcon.Warning);
txtgiatri.Focus();
    return;
        }

       if (dtpngayketthuc.Value <= dtpngaybatdau.Value) {
           MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!", "Cảnh báo",
                         MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ========== XÁC NHẬN SỬA ==========
                var confirmResult = MessageBox.Show(
                 $@"Xác nhận cập nhật khuyến mãi?

Mã KM: {maKm}
Tên: {txttenkm.Text}
Loại: {cmbloaikm.SelectedItem}
Giá trị: {giaTri}%",
                   "Xác nhận cập nhật",
                    MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes) return;

                // ========== CẬP NHẬT DATABASE ==========
                using var db = new DataSqlContext();

                var khuyenMai = db.KhuyenMais.Find(maKm);

                if (khuyenMai == null) {
                    MessageBox.Show("Không tìm thấy khuyến mãi!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cập nhật thông tin
                khuyenMai.TenKm = txttenkm.Text.Trim();
                khuyenMai.MoTa = txtmota.Text.Trim();
                khuyenMai.LoaiKm = cmbloaikm.SelectedItem.ToString();
                khuyenMai.GiaTri = giaTri;
                khuyenMai.NgayBatDau = DateOnly.FromDateTime(dtpngaybatdau.Value);
                khuyenMai.NgayKetThuc = DateOnly.FromDateTime(dtpngayketthuc.Value);
                khuyenMai.TrangThai = cmbtrangthaikm.SelectedItem?.ToString();

                db.SaveChanges();

                MessageBox.Show(
                 $"Cập nhật khuyến mãi thành công!\n\nMã KM: {maKm}\nTên: {khuyenMai.TenKm}",
               "Thành công",
                MessageBoxButtons.OK,
                  MessageBoxIcon.Information);

                // Tải lại dữ liệu
                LoadKhuyenMaiData();
                ClearKhuyenMaiForm();
                gbkhuyenmai.Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show(
                 $"Lỗi khi cập nhật khuyến mãi:\n{ex.Message}",
                "Lỗi",
             MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
            }
        }

        // Nút XÓA khuyến mãi (soft delete - chỉ cập nhật trạng thái)
        private void btnxoakm_Click(object sender, EventArgs e) {
            try {
                // Kiểm tra có chọn khuyến mãi không
                if (dgvkhuyenmai.SelectedRows.Count == 0) {
                    MessageBox.Show(
                  "Vui lòng chọn khuyến mãi cần xóa!",
            "Thông báo",
           MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                    return;
                }

                var row = dgvkhuyenmai.SelectedRows[0];
                if (row.Cells["MaKm"].Value == null) return;

                int maKm = Convert.ToInt32(row.Cells["MaKm"].Value);
                string tenKm = row.Cells["TenKm"].Value?.ToString() ?? "N/A";

                // ========== XÁC NHẬN XÓA ==========
                var confirmResult = MessageBox.Show(
                        $@"Bạn có chắc muốn XÓA khuyến mãi này?

Mã KM: {maKm}
Tên: {tenKm}

Lưu ý: Khuyến mãi sẽ được đặt trạng thái 'Đã kết thúc'.",
                      "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes) return;

                // ========== CẬP NHẬT TRẠNG THÁI ==========
                using var db = new DataSqlContext();

                var khuyenMai = db.KhuyenMais.Find(maKm);

                if (khuyenMai == null) {
                    MessageBox.Show("Không tìm thấy khuyến mãi!", "Lỗi",
         MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Soft delete - chỉ thay đổi trạng thái thay vì xóa khỏi database
                khuyenMai.TrangThai = "Đã kết thúc";
                db.SaveChanges();

                MessageBox.Show(
              $"Đã xóa khuyến mãi '{tenKm}' thành công!",
                "Thành công",
           MessageBoxButtons.OK,
      MessageBoxIcon.Information);

                // Tải lại dữ liệu
                LoadKhuyenMaiData();
                ClearKhuyenMaiForm();
                gbkhuyenmai.Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show(
                  $"Lỗi khi xóa khuyến mãi:\n{ex.Message}",
                 "Lỗi",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
            }
        }

        // Xóa trắng form nhập liệu khuyến mãi
        private void ClearKhuyenMaiForm() {
            txttenkm.Clear();
            txtmota.Clear();
            txtgiatri.Clear();
            cmbloaikm.SelectedIndex = -1;
            cmbtrangthaikm.SelectedIndex = -1;
            dtpngaybatdau.Value = DateTime.Now;
            dtpngayketthuc.Value = DateTime.Now.AddDays(30);
            gbkhuyenmai.Tag = null; // Xóa MaKm đang lưu
        }

        // ========== NÚT THÊM SẢN PHẨM MỚI ==========
        private void btnthem_Click(object sender, EventArgs e) {
   try {
  // ========== VALIDATE DỮ LIỆU NHẬP VÀO ==========
                if (string.IsNullOrWhiteSpace(txttensp.Text)) {
MessageBox.Show(
       "Vui lòng nhập tên sản phẩm!",
         "Cảnh báo",
     MessageBoxButtons.OK,
     MessageBoxIcon.Warning);
                    txttensp.Focus();
             return;
    }

          if (string.IsNullOrWhiteSpace(txtloai.Text)) {
 MessageBox.Show(
    "Vui lòng nhập loại sản phẩm (VD: Cà phê, Trà, Nước ép)!",
  "Cảnh báo",
            MessageBoxButtons.OK,
          MessageBoxIcon.Warning);
                txtloai.Focus();
          return;
 }

       if (!decimal.TryParse(txtdongia.Text, out decimal donGia) || donGia <= 0) {
         MessageBox.Show(
         "Đơn giá phải là số dương!",
       "Cảnh báo",
           MessageBoxButtons.OK,
         MessageBoxIcon.Warning);
  txtdongia.Focus();
     return;
     }

     if (string.IsNullOrWhiteSpace(txtdon_vi.Text)) {
          MessageBox.Show(
  "Vui lòng nhập đơn vị (VD: Ly, Chai, Cốc)!",
       "Cảnh báo",
         MessageBoxButtons.OK,
   MessageBoxIcon.Warning);
       txtdon_vi.Focus();
         return;
       }

  // ========== XÁC NHẬN THÊM MỚI ==========
  var confirmResult = MessageBox.Show(
          $@"Xác nhận thêm sản phẩm mới?

Tên: {txttensp.Text}
Loại: {txtloai.Text}
Đơn giá: {donGia:N0} VNĐ
Đơn vị: {txtdon_vi.Text}
Trạng thái: Còn bán",
                    "Xác nhận thêm mới",
           MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

           if (confirmResult != DialogResult.Yes) return;

              // ========== THÊM VÀO DATABASE ==========
            using var db = new DataSqlContext();

    var newProduct = new SanPham {
      TenSp = txttensp.Text.Trim(),
    LoaiSp = txtloai.Text.Trim(),
   DonGia = donGia,
          DonVi = txtdon_vi.Text.Trim(),
   TrangThai = "Còn bán" // Mặc định khi thêm mới
       };

            db.SanPhams.Add(newProduct);
db.SaveChanges();

          MessageBox.Show(
         $"Thêm sản phẩm thành công!\n\nMã SP: {newProduct.MaSp}\nTên: {newProduct.TenSp}",
          "Thành công",
        MessageBoxButtons.OK,
    MessageBoxIcon.Information);

        // Tải lại dữ liệu và ẩn form
  LoadProductData();
                ClearProductForm();
        gbsanpham.Visible = false;
 }
            catch (Exception ex) {
              MessageBox.Show(
      $"Lỗi khi thêm sản phẩm:\n{ex.Message}\n\nChi tiết: {ex.InnerException?.Message}",
            "Lỗi",
          MessageBoxButtons.OK,
   MessageBoxIcon.Error);
  }
     }

        // ========== NÚT SỬA SẢN PHẨM ==========
   private void btnsua_Click(object sender, EventArgs e) {
  try {
      // Kiểm tra xem có sản phẩm nào được chọn không
         if (string.IsNullOrWhiteSpace(txtmasp.Text)) {
        MessageBox.Show(
        "Vui lòng chọn sản phẩm cần sửa từ danh sách!",
     "Thông báo",
     MessageBoxButtons.OK,
     MessageBoxIcon.Warning);
     return;
        }

             int maSp = Convert.ToInt32(txtmasp.Text);

                // ========== VALIDATE DỮ LIỆU ==========
       if (string.IsNullOrWhiteSpace(txttensp.Text)) {
           MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Cảnh báo",
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
 txttensp.Focus();
   return;
    }

    if (string.IsNullOrWhiteSpace(txtloai.Text)) {
           MessageBox.Show("Vui lòng nhập loại sản phẩm!", "Cảnh báo",
      MessageBoxButtons.OK, MessageBoxIcon.Warning);
      txtloai.Focus();
 return;
   }

 if (!decimal.TryParse(txtdongia.Text, out decimal donGia) || donGia <= 0) {
       MessageBox.Show("Đơn giá phải là số dương!", "Cảnh báo",
MessageBoxButtons.OK, MessageBoxIcon.Warning);
txtdongia.Focus();
    return;
        }

       if (string.IsNullOrWhiteSpace(txtdon_vi.Text)) {
           MessageBox.Show("Vui lòng nhập đơn vị!", "Cảnh báo",
              MessageBoxButtons.OK, MessageBoxIcon.Warning);
     txtdon_vi.Focus();
        return;
                }

   // ========== XÁC NHẬN SỬA ==========
         var confirmResult = MessageBox.Show(
           $@"Xác nhận cập nhật sản phẩm?

Mã SP: {maSp}
Tên: {txttensp.Text}
Loại: {txtloai.Text}
Đơn giá: {donGia:N0} VNĐ",
       "Xác nhận cập nhật",
              MessageBoxButtons.YesNo,
       MessageBoxIcon.Question);

              if (confirmResult != DialogResult.Yes) return;

 // ========== CẬP NHẬT DATABASE ==========
     using var db = new DataSqlContext();

     var product = db.SanPhams.Find(maSp);

          if (product == null) {
            MessageBox.Show("Không tìm thấy sản phẩm!", "Lỗi",
  MessageBoxButtons.OK, MessageBoxIcon.Error);
           return;
           }

    // Cập nhật thông tin
     product.TenSp = txttensp.Text.Trim();
      product.LoaiSp = txtloai.Text.Trim();
    product.DonGia = donGia;
     product.DonVi = txtdon_vi.Text.Trim();

       db.SaveChanges();

   MessageBox.Show(
     $"Cập nhật sản phẩm thành công!\n\nMã SP: {maSp}\nTên: {product.TenSp}",
     "Thành công",
      MessageBoxButtons.OK,
 MessageBoxIcon.Information);

   // Tải lại dữ liệu
            LoadProductData();
     ClearProductForm();
     gbsanpham.Visible = false;
            }
          catch (Exception ex) {
    MessageBox.Show(
            $"Lỗi khi cập nhật sản phẩm:\n{ex.Message}",
      "Lỗi",
     MessageBoxButtons.OK,
          MessageBoxIcon.Error);
        }
     }

        // ========== NÚT XÓA SẢN PHẨM (SOFT DELETE) ==========
        private void btnxoa_Click(object sender, EventArgs e) {
  try {
          // Kiểm tra có chọn sản phẩm không
           if (dgvSanPham.SelectedRows.Count == 0) {
        MessageBox.Show(
     "Vui lòng chọn sản phẩm cần ngừng bán!",
 "Thông báo",
       MessageBoxButtons.OK,
              MessageBoxIcon.Warning);
    return;
     }

       var row = dgvSanPham.SelectedRows[0];
        if (row.Cells["MaSp"].Value == null) return;

          int maSp = Convert.ToInt32(row.Cells["MaSp"].Value);
       string tenSp = row.Cells["TenSp"].Value?.ToString() ?? "N/A";

   // ========== XÁC NHẬN XÓA ==========
            var confirmResult = MessageBox.Show(
         $@"Bạn có chắc muốn NGỪNG BÁN sản phẩm này?

Mã SP: {maSp}
Tên: {tenSp}

Lưu ý: Sản phẩm sẽ được đặt trạng thái 'Ngừng bán' thay vì xóa hoàn toàn.",
           "Xác nhận ngừng bán",
 MessageBoxButtons.YesNo,
 MessageBoxIcon.Warning);

    if (confirmResult != DialogResult.Yes) return;

    // ========== CẬP NHẬT TRẠNG THÁI ==========
     using var db = new DataSqlContext();

          var product = db.SanPhams.Find(maSp);

  if (product == null) {
       MessageBox.Show("Không tìm thấy sản phẩm!", "Lỗi",
   MessageBoxButtons.OK, MessageBoxIcon.Error);
         return;
              }

                // Soft delete - chỉ thay đổi trạng thái
     product.TrangThai = "Ngừng bán";
    db.SaveChanges();

     MessageBox.Show(
         $"Đã ngừng bán sản phẩm '{tenSp}' thành công!",
       "Thành công",
    MessageBoxButtons.OK,
         MessageBoxIcon.Information);

     // Tải lại dữ liệu
LoadProductData();
    ClearProductForm();
       gbsanpham.Visible = false;
     }
            catch (Exception ex) {
         MessageBox.Show(
    $"Lỗi khi xóa sản phẩm:\n{ex.Message}",
 "Lỗi",
MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }
        }

   // ========== HÀM XÓA TRẮNG FORM SẢN PHẨM ==========
        private void ClearProductForm() {
    txtmasp.Clear();
            txttensp.Clear();
            txtloai.Clear();
  txtdongia.Clear();
        txtgia.Clear();
    txtdon_vi.Clear();
        lbltrangthai.Text = "Trạng thái:";
        }
    }
}
