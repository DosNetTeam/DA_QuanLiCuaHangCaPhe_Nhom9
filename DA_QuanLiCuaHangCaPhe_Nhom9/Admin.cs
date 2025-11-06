using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using Microsoft.EntityFrameworkCore;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();

            // Set form centered
            this.StartPosition = FormStartPosition.CenterScreen;

            // Subscribe to DataGridView click events
            dgvEmployees.CellClick += dgvEmployees_CellClick;
            dgvEmployees.KeyDown += dgvEmployees_KeyDown;
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            // Load data for the first tab (Overview)
            LoadOverviewData();
        }

        private void tabControl1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Load data based on selected tab
            switch (tabControl1.SelectedIndex)
            {
                case 0: // Tổng quát
                    LoadOverviewData();
                    break;
                case 1: // Quản Lý Nhân Viên
                    LoadEmployeeData();
                    break;
                case 2: // Quản Lý Kho
                    LoadInventoryData();
                    break;
                case 3: // Doanh thu
                    LoadRevenueData();
                    break;
            }
        }

        // Load overview statistics
        private void LoadOverviewData()
        {
            try
            {
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

                dgvOverview.DataSource = overviewData;

                // Format columns
                StyleDataGridView(dgvOverview);
                dgvOverview.Columns["Category"].HeaderText = "Danh mục";
                dgvOverview.Columns["Count"].HeaderText = "Số lượng";
                dgvOverview.Columns["Details"].HeaderText = "Chi tiết";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                  $"Lỗi khi tải dữ liệu tổng quan:\n{ex.Message}",
                 "Lỗi",
                MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
            }
        }

        // Load employee data from database
        private void LoadEmployeeData()
        {
            try
            {
                using var db = new DataSqlContext();

                var employees = db.NhanViens
                  .Select(nv => new
                  {
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

                dgvEmployees.DataSource = employees;

                // Format columns
                StyleDataGridView(dgvEmployees);
                dgvEmployees.Columns["MaNv"].HeaderText = "Mã NV";
                dgvEmployees.Columns["TenNv"].HeaderText = "Họ tên";
                dgvEmployees.Columns["ChucVu"].HeaderText = "Chức vụ";
                dgvEmployees.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dgvEmployees.Columns["TaiKhoan"].HeaderText = "Tài khoản";
                dgvEmployees.Columns["VaiTro"].HeaderText = "Vai trò";
                dgvEmployees.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                 $"Lỗi khi tải dữ liệu nhân viên:\n{ex.Message}",
               "Lỗi",
               MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        // Load inventory data from database
        private void LoadInventoryData()
        {
            try
            {
                using var db = new DataSqlContext();

                var inventory = db.NguyenLieus
                 .Select(nl => new
                 {
                     nl.MaNl,
                     nl.TenNl,
                     nl.DonViTinh,
                     SoLuongTon = Math.Round(nl.SoLuongTon ?? 0, 2),
                     NguongCanhBao = Math.Round(nl.NguongCanhBao ?? 0, 2),
                     TinhTrang = (nl.SoLuongTon ?? 0) <= (nl.NguongCanhBao ?? 0) ?
             "⚠️ Cần nhập" : "✓ Đủ"
                 })
               .OrderBy(nl => nl.SoLuongTon)
                .ToList();

                dgvInventory.DataSource = inventory;

                // Format columns
                StyleDataGridView(dgvInventory);
                dgvInventory.Columns["MaNl"].HeaderText = "Mã NL";
                dgvInventory.Columns["TenNl"].HeaderText = "Tên nguyên liệu";
                dgvInventory.Columns["DonViTinh"].HeaderText = "Đơn vị";
                dgvInventory.Columns["SoLuongTon"].HeaderText = "Số lượng tồn";
                dgvInventory.Columns["NguongCanhBao"].HeaderText = "Ngưỡng cảnh báo";
                dgvInventory.Columns["TinhTrang"].HeaderText = "Tình trạng";

                // Highlight low stock items
                foreach (DataGridViewRow row in dgvInventory.Rows)
                {
                    if (row.Cells["TinhTrang"].Value?.ToString()?.Contains("Cần nhập") == true)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(211, 47, 47);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi tải dữ liệu kho:\n{ex.Message}",
                 "Lỗi",
                   MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        // Load revenue data from database
        private void LoadRevenueData()
        {
            try
            {
                using var db = new DataSqlContext();

                var revenueData = db.DonHangs
               .Where(dh => dh.TongTien.HasValue && dh.NgayLap.HasValue)
               .ToList() // Load to memory first
               .GroupBy(dh => new
               {
                   Year = dh.NgayLap!.Value.Year,
                   Month = dh.NgayLap!.Value.Month
               })
                  .Select(g => new
                  {
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

                dgvRevenue.DataSource = revenueData;

                // Format columns
                StyleDataGridView(dgvRevenue);
                dgvRevenue.Columns["Thang"].HeaderText = "Tháng";
                dgvRevenue.Columns["SoDonHang"].HeaderText = "Số đơn hàng";
                dgvRevenue.Columns["TongDoanhThu"].HeaderText = "Tổng doanh thu";
                dgvRevenue.Columns["DoanhThuTrungBinh"].HeaderText = "TB/Đơn";
                dgvRevenue.Columns["DonHangLonNhat"].HeaderText = "Cao nhất";
                dgvRevenue.Columns["DonHangNhoNhat"].HeaderText = "Thấp nhất";

                // Format currency columns
                dgvRevenue.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
                dgvRevenue.Columns["DoanhThuTrungBinh"].DefaultCellStyle.Format = "N0";
                dgvRevenue.Columns["DonHangLonNhat"].DefaultCellStyle.Format = "N0";
                dgvRevenue.Columns["DonHangNhoNhat"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
             $"Lỗi khi tải dữ liệu doanh thu:\n{ex.Message}",
              "Lỗi",
              MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        // Apply consistent styling to DataGridViews
        private void StyleDataGridView(DataGridView dgv)
        {
            // Header style
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;

            // Alternate row colors
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 181, 246);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            // Auto size
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Event handler when user clicks on employee row
        private void dgvEmployees_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Valid row clicked
            {
                ShowEmployeeDetailInPanel(e.RowIndex);
            }
        }

        // Event handler for keyboard navigation (Enter key)
        private void dgvEmployees_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvEmployees.CurrentRow != null)
            {
                e.Handled = true; // Prevent default behavior
                ShowEmployeeDetailInPanel(dgvEmployees.CurrentRow.Index);
            }
        }

        private void ShowEmployeeDetailInPanel(int rowIndex)
        {
            try
            {
                var row = dgvEmployees.Rows[rowIndex];

                // Get employee ID from the row
                if (row.Cells["MaNv"].Value != null)
                {
                    int maNv = Convert.ToInt32(row.Cells["MaNv"].Value);

                    // Load employee details from database
                    using var db = new DataSqlContext();

                    var employee = db.NhanViens
               .Where(nv => nv.MaNv == maNv)
                     .Select(nv => new
                     {
                         nv.MaNv,
                         nv.TenNv,
                         nv.ChucVu,
                         nv.SoDienThoai,
                         TenDangNhap = nv.TaiKhoan != null ? nv.TaiKhoan.TenDangNhap : "Chưa có",
                         VaiTro = nv.TaiKhoan != null ? nv.TaiKhoan.MaVaiTroNavigation.TenVaiTro : "N/A",
                         TrangThai = nv.TaiKhoan != null ?
                   (nv.TaiKhoan.TrangThai == true ? "Hoạt động" : "Bị khóa") : "N/A",
                         SoDonHang = nv.DonHangs.Count()
                     })
                     .FirstOrDefault();

                    if (employee != null)
                    {
                        // Update TextBoxes in panel1
                        textBox1.Text = employee.MaNv.ToString();
                        textBox2.Text = employee.TenNv;
                        textBox3.Text = string.IsNullOrEmpty(employee.SoDienThoai) ? "Chưa cập nhật" : employee.SoDienThoai;
                        textBox4.Text = employee.ChucVu;
                        textBox6.Text = employee.TenDangNhap;
                        textBox5.Text = employee.VaiTro;

                        // Show panel1
                        panel1.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
             $"Lỗi khi hiển thị chi tiết:\n{ex.Message}",
              "Lỗi",
              MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnLockUnlock_Click(object? sender, EventArgs e)
        {
            // Tính năng này sẽ được implement sau
   MessageBox.Show("Tính năng đang được phát triển!", "Thông báo");
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                // Open create employee dialog
                CreateEmployeeForm createForm = new CreateEmployeeForm();

                if (createForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh employee data if on employee tab
                    if (tabControl1.SelectedIndex == 1)
                    {
                        LoadEmployeeData();
                    }
                    else
                    {
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
            catch (Exception ex)
            {
                MessageBox.Show(
                     $"Lỗi khi tạo tài khoản:\n{ex.Message}",
                "Lỗi",
                  MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            // Check if an employee is selected
            if (dgvEmployees.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                   "Vui lòng chọn nhân viên cần xóa!",
             "Thông báo",
                 MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var row = dgvEmployees.SelectedRows[0];

                if (row.Cells["MaNv"].Value == null)
                {
                    MessageBox.Show(
                "Không thể xác định nhân viên được chọn!",
                  "Lỗi",
                    MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                    return;
                }

                int maNv = Convert.ToInt32(row.Cells["MaNv"].Value);
                string tenNv = row.Cells["TenNv"].Value?.ToString() ?? "N/A";

                // Confirm deletion
                var confirmResult = MessageBox.Show(
              $"Bạn có chắc muốn xóa nhân viên này?\n\n" +
           $"Mã NV: {maNv}\n" +
            $"Họ tên: {tenNv}\n\n" +
                 "⚠️ CẢNH BÁO: Hành động này sẽ xóa:\n" +
            "- Thông tin nhân viên\n" +
              "- Tài khoản đăng nhập\n" +
                  "- Tất cả dữ liệu liên quan\n\n" +
               "Dữ liệu sau khi xóa KHÔNG THỂ KHÔI PHỤC!",
             "⚠️ Xác nhận xóa",
           MessageBoxButtons.YesNo,
           MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes)
                {
                    return;
                }

                using var db = new DataSqlContext();

                // Find employee
                var nhanVien = db.NhanViens
                     .Include(nv => nv.TaiKhoan)
                .Include(nv => nv.DonHangs)
                   .Include(nv => nv.PhieuKhos)
                .FirstOrDefault(nv => nv.MaNv == maNv);

                if (nhanVien == null)
                {
                    MessageBox.Show(
                   "Không tìm thấy nhân viên trong cơ sở dữ liệu!",
                    "Lỗi",
                      MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
                    return;
                }

                // Check if employee has orders or warehouse receipts
                bool hasOrders = nhanVien.DonHangs.Any();
                bool hasReceipts = nhanVien.PhieuKhos.Any();

                if (hasOrders || hasReceipts)
                {
                    var warningResult = MessageBox.Show(
                       $"⚠️ CẢNH BÁO NGHIÊM TRỌNG:\n\n" +
                   $"Nhân viên này có:\n" +
                         $"- {nhanVien.DonHangs.Count} đơn hàng\n" +
                    $"- {nhanVien.PhieuKhos.Count} phiếu kho\n\n" +
                    "Xóa nhân viên này có thể ảnh hưởng đến:\n" +
                     "- Báo cáo doanh thu\n" +
                      "- Lịch sử đơn hàng\n" +
                    "- Quản lý kho\n\n" +
   "Khuyến nghị: NÊN KHÓA TÀI KHOẢN thay vì xóa.\n\n" +
     "Bạn vẫn muốn tiếp tục xóa?",
                       "⚠️⚠️ Cảnh báo nghiêm trọng",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop);

                    if (warningResult != DialogResult.Yes)
                    {
                        return;
                    }
                }

                // Delete TaiKhoan first (if exists)
                if (nhanVien.TaiKhoan != null)
                {
                    db.TaiKhoans.Remove(nhanVien.TaiKhoan);
                }

                // Delete NhanVien
                db.NhanViens.Remove(nhanVien);

                // Save changes
                db.SaveChanges();

                MessageBox.Show(
             $"Đã xóa nhân viên '{tenNv}' thành công!",
                    "Thành công",
                 MessageBoxButtons.OK,
                   MessageBoxIcon.Information);

                // Refresh the employee list
                LoadEmployeeData();

    // Refresh overview if needed
              if (tabControl1.SelectedIndex == 0)
        {
      LoadOverviewData();
 }

      // Hide detail panel if visible
   panel1.Visible = false;
  }
      catch (Exception ex)
            {
                MessageBox.Show(
        $"Lỗi khi xóa nhân viên:\n{ex.Message}\n\n" +
               $"Chi tiết: {ex.InnerException?.Message}",
           "Lỗi",
    MessageBoxButtons.OK,
          MessageBoxIcon.Error);
       }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Confirm logout
            var confirmResult = MessageBox.Show(
       "Bạn có chắc muốn đăng xuất?\n\n" +
    "Hệ thống sẽ quay về trang đăng nhập.",
     "Xác nhận đăng xuất",
 MessageBoxButtons.YesNo,
  MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            try
            {
                // Check if there's an existing Loginform instance
                var existingLogin = Application.OpenForms.OfType<Loginform>().FirstOrDefault();

                if (existingLogin != null)
                {
                    // If exists, show it
                    existingLogin.Show();
                    existingLogin.BringToFront();
                    // Clear textboxes for security
                    existingLogin.Controls["txtUser"]?.ResetText();
                    existingLogin.Controls["txtPass"]?.ResetText();
                }
                else
                {
                    // If not exists, create new login form
                    Loginform loginForm = new Loginform();
                    loginForm.StartPosition = FormStartPosition.CenterScreen;
                    loginForm.Show();
                }

                // Close this admin form
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
          $"Lỗi khi đăng xuất:\n{ex.Message}",
         "Lỗi",
          MessageBoxButtons.OK,
             MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
          "Bạn có chắc muốn thoát?",
             "Xác nhận",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
