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
        private int selectedEmployeeId = 0;

        public Admin()
        {
            InitializeComponent();

            // Set form centered
            this.StartPosition = FormStartPosition.CenterScreen;

            // Subscribe to DataGridView click events
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.KeyDown += dataGridView1_KeyDown;

            // Add Save button for updating ChucVu
            CreateSaveButton();

            // Initially hide the panel
            panel1.Visible = false;
        }

        private void CreateSaveButton()
        {
            // Check if button1 already exists for password change
            // Add another button for saving ChucVu changes
            Button btnSaveChucVu = new Button
            {
                Text = "Lưu thay đổi",
                Location = new Point(520, 285),
                Size = new Size(120, 41),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Name = "btnSaveChucVu"
            };
            btnSaveChucVu.Click += BtnSaveChucVu_Click;

            // Add to panel1
            if (!panel1.Controls.Contains(btnSaveChucVu))
            {
                panel1.Controls.Add(btnSaveChucVu);
            }
        }

        private void BtnSaveChucVu_Click(object? sender, EventArgs e)
        {
            if (selectedEmployeeId == 0)
            {
                MessageBox.Show(
  "Vui lòng chọn nhân viên để cập nhật!",
           "Thông báo",
    MessageBoxButtons.OK,
              MessageBoxIcon.Warning);
   return;
            }

 try
  {
         using var db = new DataSqlContext();
                var nhanVien = db.NhanViens.FirstOrDefault(nv => nv.MaNv == selectedEmployeeId);

 if (nhanVien == null)
        {
          MessageBox.Show(
             "Không tìm thấy nhân viên trong cơ sở dữ liệu!",
        "Lỗi",
    MessageBoxButtons.OK,
          MessageBoxIcon.Error);
         return;
    }

         // Get new value from ComboBox instead of TextBox
    ComboBox? cboChucVuEdit = panel1.Controls.OfType<ComboBox>()
    .FirstOrDefault(c => c.Name == "cboChucVuEdit");
     
        if (cboChucVuEdit == null || cboChucVuEdit.SelectedItem == null)
            {
   MessageBox.Show(
          "Vui lòng chọn chức vụ!",
    "Lỗi",
      MessageBoxButtons.OK,
    MessageBoxIcon.Warning);
      return;
    }

         string newChucVu = cboChucVuEdit.SelectedItem.ToString() ?? "";

         // Confirm update
    var confirmResult = MessageBox.Show(
          $"Bạn có chắc muốn cập nhật chức vụ của nhân viên '{nhanVien.TenNv}'?\n\n" +
  $"Chức vụ cũ: {nhanVien.ChucVu}\n" +
       $"Chức vụ mới: {newChucVu}",
         "Xác nhận cập nhật",
          MessageBoxButtons.YesNo,
     MessageBoxIcon.Question);

  if (confirmResult != DialogResult.Yes)
{
               return;
           }

 // Update ChucVu
     nhanVien.ChucVu = newChucVu;
      db.SaveChanges();

MessageBox.Show(
       $"Đã cập nhật chức vụ thành công!\n" +
      $"Nhân viên: {nhanVien.TenNv}\n" +
  $"Chức vụ mới: {newChucVu}",
"Thành công",
    MessageBoxButtons.OK,
 MessageBoxIcon.Information);

  // Refresh data
 LoadEmployeeData();
       panel1.Visible = false;
  selectedEmployeeId = 0;
     }
   catch (Exception ex)
{
      MessageBox.Show(
        $"Lỗi khi cập nhật chức vụ:\n{ex.Message}",
 "Lỗi",
     MessageBoxButtons.OK,
      MessageBoxIcon.Error);
     }
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
        panel1.Visible = false;
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

       dataGridView1.DataSource = employees;

   // Format columns
  StyleDataGridView(dataGridView1);
          dataGridView1.Columns["MaNv"].HeaderText = "Mã NV";
       dataGridView1.Columns["TenNv"].HeaderText = "Họ tên";
           dataGridView1.Columns["ChucVu"].HeaderText = "Chức vụ";
   dataGridView1.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
     dataGridView1.Columns["TaiKhoan"].HeaderText = "Tài khoản";
   dataGridView1.Columns["VaiTro"].HeaderText = "Vai trò";
        dataGridView1.Columns["TrangThai"].HeaderText = "Trạng thái";
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
                             TinhTrang = "Dồi dào" // Luôn hiển thị "Dồi dào"
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

                // Highlight all rows with green color (Dồi dào)
                foreach (DataGridViewRow row in dgvInventory.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(223, 240, 216);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(60, 118, 61);
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
   .ToList()
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
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
          dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dgv.EnableHeadersVisualStyles = false;
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
      dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 181, 246);
       dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
   }

        private void dataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e)
  {
          if (e.RowIndex >= 0)
          {
  ShowEmployeeDetailInPanel(e.RowIndex);
    }
  }

    private void dataGridView1_KeyDown(object? sender, KeyEventArgs e)
    {
  if (e.KeyCode == Keys.Enter && dataGridView1.CurrentRow != null)
 {
e.Handled = true;
         ShowEmployeeDetailInPanel(dataGridView1.CurrentRow.Index);
            }
        }

 private void ShowEmployeeDetailInPanel(int rowIndex)
   {
    try
{
  var row = dataGridView1.Rows[rowIndex];

      if (row.Cells["MaNv"].Value != null)
       {
   int maNv = Convert.ToInt32(row.Cells["MaNv"].Value);
         selectedEmployeeId = maNv;

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
     MatKhau = nv.TaiKhoan != null ? nv.TaiKhoan.MatKhau : "",
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

                        //      // Hiển thị mật khẩu (readonly ban đầu)
                        textBoxPassword.Text = employee.MatKhau;
                        textBoxPassword.ReadOnly = true;
                        textBoxPassword.UseSystemPasswordChar = true;

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

        // Button1: Đổi mật khẩu
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có nhân viên nào được chọn không
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show(
                          "Vui lòng chọn nhân viên từ danh sách trước!",
                         "Thông báo",
                       MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                    return;
                }

                int maNv = Convert.ToInt32(textBox1.Text);
                string tenDangNhap = textBox6.Text;

                // Kiểm tra xem nhân viên có tài khoản không
                if (tenDangNhap == "Chưa có")
                {
                    MessageBox.Show(
                       "Nhân viên này chưa có tài khoản!",
                       "Thông báo",
                     MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                    return;
                }

                // Cho phép chỉnh sửa mật khẩu
                textBoxPassword.ReadOnly = false;
                textBoxPassword.UseSystemPasswordChar = false;
                textBoxPassword.Focus();
                textBoxPassword.SelectAll();

                // Hiển thị hộp thoại nhập mật khẩu mới
                using (Form promptForm = new Form())
                {
                    promptForm.Width = 450;
                    promptForm.Height = 250;
                    promptForm.Text = "Đổi mật khẩu";
                    promptForm.StartPosition = FormStartPosition.CenterParent;
                    promptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    promptForm.MaximizeBox = false;
                    promptForm.MinimizeBox = false;

                    Label lblInfo = new Label()
                    {
                        Text = $"Đổi mật khẩu cho: {textBox2.Text} ({tenDangNhap})",
                        Left = 20,
                        Top = 20,
                        Width = 400,
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                    };

                    Label lblOldPass = new Label()
                    {
                        Text = "Mật khẩu hiện tại:",
                        Left = 20,
                        Top = 60,
                        Width = 150
                    };

                    TextBox txtOldPass = new TextBox()
                    {
                        Left = 180,
                        Top = 57,
                        Width = 230,
                        UseSystemPasswordChar = true
                    };

                    Label lblNewPass = new Label()
                    {
                        Text = "Mật khẩu mới:",
                        Left = 20,
                        Top = 95,
                        Width = 150
                    };

                    TextBox txtNewPass = new TextBox()
                    {
                        Left = 180,
                        Top = 92,
                        Width = 230,
                        UseSystemPasswordChar = true
                    };

                    Label lblConfirmPass = new Label()
                    {
                        Text = "Xác nhận mật khẩu:",
                        Left = 20,
                        Top = 130,
                        Width = 150
                    };

                    TextBox txtConfirmPass = new TextBox()
                    {
                        Left = 180,
                        Top = 127,
                        Width = 230,
                        UseSystemPasswordChar = true
                    };

                    Button btnOK = new Button()
                    {
                        Text = "Lưu",
                        Left = 180,
                        Top = 165,
                        Width = 110,
                        Height = 35,
                        BackColor = Color.FromArgb(46, 125, 50),
                        ForeColor = Color.White,
                        DialogResult = DialogResult.OK
                    };

                    Button btnCancel = new Button()
                    {
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

                    if (promptForm.ShowDialog() == DialogResult.OK)
                    {
                        // Validate input
                        if (string.IsNullOrWhiteSpace(txtOldPass.Text))
                        {
                            MessageBox.Show(
                         "Vui lòng nhập mật khẩu hiện tại!",
                        "Cảnh báo",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Warning);
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(txtNewPass.Text))
                        {
                            MessageBox.Show(
                                "Vui lòng nhập mật khẩu mới!",
                                  "Cảnh báo",
                              MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            return;
                        }

                        if (txtNewPass.Text.Length < 3)
                        {
                            MessageBox.Show(
                   "Mật khẩu mới phải có ít nhất 3 ký tự!",
                   "Cảnh báo",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Warning);
                            return;
                        }

                        if (txtNewPass.Text != txtConfirmPass.Text)
                        {
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

                        if (taiKhoan == null)
                        {
                            MessageBox.Show(
                          "Không tìm thấy tài khoản!",
                            "Lỗi",
                         MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                            return;
                        }

                        // Verify old password
                        if (taiKhoan.MatKhau != txtOldPass.Text)
                        {
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
                   $"Nhân viên: {textBox2.Text}\n\n" +
                       "Mật khẩu mới sẽ được áp dụng ngay lập tức!",
                                "Xác nhận đổi mật khẩu",
                                MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                        if (confirmResult != DialogResult.Yes)
                        {
                            return;
                        }

                        // Update password
                        string oldPassword = taiKhoan.MatKhau;
                        taiKhoan.MatKhau = txtNewPass.Text;
                        db.SaveChanges();

                        MessageBox.Show(
                $"Đổi mật khẩu thành công!\n\n" +
                                $"Tài khoản: {tenDangNhap}\n" +
                   $"Nhân viên: {textBox2.Text}\n" +
                  $"Mật khẩu cũ: {oldPassword}\n" +
                     $"Mật khẩu mới: {txtNewPass.Text}",
                          "Thành công",
                   MessageBoxButtons.OK,
                      MessageBoxIcon.Information);

                        // Update textbox to show new password
                        textBoxPassword.Text = txtNewPass.Text;
                        textBoxPassword.ReadOnly = true;
                        textBoxPassword.UseSystemPasswordChar = true;

                        // Refresh employee data
                        LoadEmployeeData();
                    }
                    else
                    {
                        // User cancelled - restore readonly state
                        textBoxPassword.ReadOnly = true;
                        textBoxPassword.UseSystemPasswordChar = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                                 $"Lỗi khi đổi mật khẩu:\n{ex.Message}\n\n" +
                     $"Chi tiết: {ex.InnerException?.Message}",
                      "Lỗi",
                  MessageBoxButtons.OK,
                              MessageBoxIcon.Error);

                // Restore readonly state on error
                if (textBoxPassword != null)
                {
                    textBoxPassword.ReadOnly = true;
                    textBoxPassword.UseSystemPasswordChar = true;
                }
            }
        }

        // Show inventory detail in panel2
        private void ShowInventoryDetailInPanel(int rowIndex)
        {
            try
            {
                var row = dgvInventory.Rows[rowIndex];

                // Get ingredient ID from the row
                if (row.Cells["MaNl"].Value != null)
                {
                    int maNl = Convert.ToInt32(row.Cells["MaNl"].Value);

                    // Load ingredient details from database
                    using var db = new DataSqlContext();

                    var ingredient = db.NguyenLieus
                         .Where(nl => nl.MaNl == maNl)
                .FirstOrDefault();

                    if (ingredient != null)
                    {
                        // Update TextBoxes in panel2
                        textBox9.Text = ingredient.MaNl.ToString();
                        textBox8.Text = ingredient.TenNl;
                        textBox10.Text = ingredient.SoLuongTon.ToString();
                        textBox7.Text = ingredient.DonViTinh;

                        // Make Mã, Tên, Đơn vị readonly (không cho sửa)
                        textBox9.ReadOnly = true;
                        textBox8.ReadOnly = true;
                        textBox7.ReadOnly = true;

                        // Chỉ cho phép sửa Số Lượng
                        textBox10.ReadOnly = false;
                        textBox10.Focus();
                        textBox10.SelectAll();

                        // Store ingredient ID for update
                        button2.Tag = maNl;

                        // Show panel2
                        panel2.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
          $"Lỗi khi hiển thị chi tiết nguyên liệu:\n{ex.Message}",
             "Lỗi",
                 MessageBoxButtons.OK,
      MessageBoxIcon.Error);
            }
        }

        // Button: Cập nhật số lượng nguyên liệu HOẶC Thêm mới
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate số lượng input
                if (string.IsNullOrWhiteSpace(textBox10.Text) ||
                    !decimal.TryParse(textBox10.Text, out decimal soLuongMoi))
                {
                    MessageBox.Show(
              "Vui lòng nhập số lượng hợp lệ!",
              "Cảnh báo",
           MessageBoxButtons.OK,
           MessageBoxIcon.Warning);
                    textBox10.Focus();
                    return;
                }

                if (soLuongMoi < 0)
                {
                    MessageBox.Show(
                      "Số lượng phải lớn hơn hoặc bằng 0!",
                   "Cảnh báo",
                 MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);
                    textBox10.Focus();
                    return;
                }

                // **KIỂM TRA CHẾ ĐỘ: Update hay Add New**
                if (button2.Tag != null)
                {
                    // ============ CHẾ ĐỘ UPDATE (CÓ TAG) ============
                    int maNl = (int)button2.Tag;
                    string tenNl = textBox8.Text;

                    // Confirm update
                    var confirmResult = MessageBox.Show(
                   $"Xác nhận cập nhật số lượng?\n\n" +
                 $"Nguyên liệu: {tenNl}\n" +
               $"Số lượng mới: {soLuongMoi} {textBox7.Text}",
                "Xác nhận cập nhật",
                      MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);

                    if (confirmResult != DialogResult.Yes) return;

                    using var db = new DataSqlContext();
                    var ingredient = db.NguyenLieus.Find(maNl);

                    if (ingredient == null)
                    {
                        MessageBox.Show(
                             "Không tìm thấy nguyên liệu!",
                               "Lỗi",
                        MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                        return;
                    }

                    // Update số lượng tồn
                    decimal soLuongCu = ingredient.SoLuongTon ?? 0;
                    ingredient.SoLuongTon = soLuongMoi;

                    db.SaveChanges();

                    MessageBox.Show(
               $"Cập nhật số lượng thành công!\n\n" +
                      $"Nguyên liệu: {tenNl}\n" +
                $"Số lượng cũ: {soLuongCu} {textBox7.Text}\n" +
                     $"Số lượng mới: {soLuongMoi} {textBox7.Text}",
                     "Thành công",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
                }
                else
                {
                    // ============ CHẾ ĐỘ ADD NEW (KHÔNG CÓ TAG) ============

                    // Validate tên nguyên liệu
                    if (string.IsNullOrWhiteSpace(textBox8.Text))
                    {
                        MessageBox.Show(
                        "Vui lòng nhập tên nguyên liệu!",
                   "Cảnh báo",
                   MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                        textBox8.Focus();
                        return;
                    }

                    // Validate đơn vị
                    if (string.IsNullOrWhiteSpace(textBox7.Text))
                    {
                        MessageBox.Show(
                         "Vui lòng nhập đơn vị (ví dụ: kg, lít, chai)!",
                   "Cảnh báo",
                  MessageBoxButtons.OK,
                 MessageBoxIcon.Warning);
                        textBox7.Focus();
                        return;
                    }

                    // Confirm add new
                    var confirmResult = MessageBox.Show(
                     $"Xác nhận thêm nguyên liệu mới?\n\n" +
                        $"Tên: {textBox8.Text}\n" +
                     $"Số lượng: {soLuongMoi} {textBox7.Text}\n" +
                      $"Đơn vị: {textBox7.Text}",
                      "Xác nhận thêm mới",
                            MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question);

                    if (confirmResult != DialogResult.Yes) return;

                    using var db = new DataSqlContext();

                    // Tính ngưỡng cảnh báo = 1/4 số lượng nhập
                    decimal nguongCanhBao = Math.Round(soLuongMoi / 4, 2);

                    // Create new ingredient
                    var newIngredient = new NguyenLieu
                    {
                        TenNl = textBox8.Text.Trim(),
                        DonViTinh = textBox7.Text.Trim(),
                        SoLuongTon = soLuongMoi,
                        NguongCanhBao = nguongCanhBao // Ngưỡng = 1/4 số lượng
                    };

                    db.NguyenLieus.Add(newIngredient);
                    db.SaveChanges();

                    MessageBox.Show(
              $"Thêm nguyên liệu mới thành công!\n\n" +
                 $"Tên: {newIngredient.TenNl}\n" +
                    $"Mã nguyên liệu: {newIngredient.MaNl}\n" +
             $"Số lượng: {soLuongMoi} {textBox7.Text}\n" +
               $"Ngưỡng cảnh báo: {nguongCanhBao} {textBox7.Text}",
                "Thành công",
                 MessageBoxButtons.OK,
             MessageBoxIcon.Information);
                }

                // Refresh data and hide panel
                LoadInventoryData();
                ClearInventoryForm();
                panel2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                $"Lỗi khi lưu nguyên liệu:\n{ex.Message}",
            "Lỗi",
              MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Check if an ingredient is selected in DataGridView
            if (dgvInventory.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                 "Vui lòng chọn nguyên liệu cần xóa từ danh sách!",
                      "Thông báo",
                MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var row = dgvInventory.SelectedRows[0];
                if (row.Cells["MaNl"].Value == null) return;

                int maNl = Convert.ToInt32(row.Cells["MaNl"].Value);
                string tenNl = row.Cells["TenNl"].Value?.ToString() ?? "N/A";

                // Confirm deletion
                var confirmResult = MessageBox.Show(
                          $"Bạn có chắc muốn xóa nguyên liệu này?\n\n" +
                       $"Mã: {maNl}\n" +
                $"Tên: {tenNl}\n\n" +
                   "⚠️ CẢNH BÁO: Hành động này sẽ:\n" +
                     "- Xóa thông tin nguyên liệu\n" +
                        "- Xóa định lượng liên quan\n" +
                        "- Có thể ảnh hưởng đến công thức món\n\n" +
                   "Dữ liệu sau khi xóa KHÔNG THỂ KHÔI PHỤC!",
                      "⚠️ Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes) return;

                using var db = new DataSqlContext();

                // Find ingredient with related data
                var ingredient = db.NguyenLieus
          .Include(nl => nl.DinhLuongs) // Include recipes
              .Include(nl => nl.ChiTietPhieuKhos) // Include warehouse receipts
              .FirstOrDefault(nl => nl.MaNl == maNl);

                if (ingredient == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy nguyên liệu!",
                   "Lỗi",
                           MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                    return;
                }

                // Check if ingredient is used in recipes
                if (ingredient.DinhLuongs.Any())
                {
                    var warningResult = MessageBox.Show(
                     $"⚠️ Nguyên liệu này đang được dùng trong {ingredient.DinhLuongs.Count} công thức!\n\n" +
                     "Xóa nguyên liệu sẽ ảnh hưởng đến:\n" +
             "- Công thức món ăn\n" +
              "- Tính toán chi phí\n\n" +
                "Bạn vẫn muốn tiếp tục?",
                      "⚠️⚠️ Cảnh báo nghiêm trọng",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Stop);

                    if (warningResult != DialogResult.Yes) return;

                    // Delete related recipes first
                    db.DinhLuongs.RemoveRange(ingredient.DinhLuongs);
                }

                // Delete related warehouse receipt details
                if (ingredient.ChiTietPhieuKhos.Any())
                {
                    db.ChiTietPhieuKhos.RemoveRange(ingredient.ChiTietPhieuKhos);
                }

                // Delete ingredient
                db.NguyenLieus.Remove(ingredient);
                db.SaveChanges();

                MessageBox.Show(
                       $"Đã xóa nguyên liệu '{tenNl}' thành công!",
                "Thành công",
                         MessageBoxButtons.OK,
              MessageBoxIcon.Information);

                // Refresh data and hide panel
                LoadInventoryData();
                ClearInventoryForm();
                panel2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                     $"Lỗi khi xóa nguyên liệu:\n{ex.Message}\n\n" +
                        $"Chi tiết: {ex.InnerException?.Message}",
                 "Lỗi",
                       MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        // Clear inventory form
        private void ClearInventoryForm()
        {
            textBox9.Clear();
            textBox8.Clear();
            textBox10.Clear();
            textBox7.Clear();

            button2.Tag = null;
            button2.Text = "Cập nhật"; // Reset về text mặc định
        }

        // Button4: Hiển thị form trống để thêm nguyên liệu mới
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Xóa form trước
                ClearInventoryForm();

                // Set panel sang chế độ "Add New"
                textBox9.ReadOnly = true; // Mã tự động (không cho nhập)
                textBox9.Text = "(Tự động)";
                textBox8.ReadOnly = false; // Tên cho nhập
                textBox10.ReadOnly = false; // Số lượng cho nhập
                textBox7.ReadOnly = false; // Đơn vị cho nhập

                // Clear button2 tag để báo hiệu đây là "Add New" mode
                button2.Tag = null;

                // Đổi text button2 thành "Lưu Mới"
                button2.Text = "Lưu Mới";

                // Hiện panel và focus vào tên
                panel2.Visible = true;
                textBox8.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
            $"Lỗi:\n{ex.Message}",
           "Lỗi",
             MessageBoxButtons.OK,
           MessageBoxIcon.Error);
            }
        }

        private void BtnLockUnlock_Click(object? sender, EventArgs e)
        {
            // Placeholder for lock/unlock functionality
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

        if (confirmResult != DialogResult.Yes) return;

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

        // Get max employee ID before deletion
  int? maxMaNvBeforeDelete = db.NhanViens.Max(nv => (int?)nv.MaNv);
        bool isMaxId = (maNv == maxMaNvBeforeDelete);

      // Delete TaiKhoan first (if exists)
        if (nhanVien.TaiKhoan != null)
      {
 db.TaiKhoans.Remove(nhanVien.TaiKhoan);
        }

        // Delete NhanVien
        db.NhanViens.Remove(nhanVien);
        db.SaveChanges();

        // Reset IDENTITY if we deleted the employee with max ID
   if (isMaxId)
        {
 int? newMaxMaNv = db.NhanViens.Max(nv => (int?)nv.MaNv);
  
            if (newMaxMaNv.HasValue)
            {
                // Reset IDENTITY to the current max ID
            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('NhanVien', RESEED, {newMaxMaNv.Value})");
}
        else
       {
         // No employees left, reset to 0
              db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('NhanVien', RESEED, 0)");
   }
        }

        MessageBox.Show(
            $"Đã xóa nhân viên '{tenNv}' thành công!\n" +
            (isMaxId ? "Mã nhân viên đã được reset để tái sử dụng." : ""),
            "Thành công",
            MessageBoxButtons.OK,
       MessageBoxIcon.Information);

        // Refresh the employee list
LoadEmployeeData();

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

            if (confirmResult != DialogResult.Yes) return;

         try
            {
    // Check if there's an existing Loginform instance
  var existingLogin = Application.OpenForms.OfType<Loginform>().FirstOrDefault();

     if (existingLogin != null)
      {
        existingLogin.Show();
   existingLogin.BringToFront();
             existingLogin.Controls["txtUser"]?.ResetText();
             existingLogin.Controls["txtPass"]?.ResetText();
   }
       else
                {
         Loginform loginForm = new Loginform();
        loginForm.StartPosition = FormStartPosition.CenterScreen;
  loginForm.Show();
   }

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
    // Event handler for textBox5
      }

 private void textBox7_TextChanged(object sender, EventArgs e)
  {
            // Event handler for textBox7
        }
 }
}
