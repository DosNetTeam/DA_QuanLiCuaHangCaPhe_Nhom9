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
   TenDangNhap = nv.TaiKhoan != null ? nv.TaiKhoan.TenDangNhap : "",
  VaiTro = nv.TaiKhoan != null ? nv.TaiKhoan.MaVaiTroNavigation.TenVaiTro : "",
       MaVaiTro = nv.TaiKhoan != null ? nv.TaiKhoan.MaVaiTro : 0
      })
   .FirstOrDefault();

         if (employee != null)
        {
      textBox1.Text = employee.MaNv.ToString();
   textBox2.Text = employee.TenNv ?? "";
    textBox3.Text = employee.SoDienThoai ?? "";

   // Replace textBox4 with ComboBox for ChucVu
    ComboBox cboChucVuEdit = panel1.Controls.OfType<ComboBox>()
    .FirstOrDefault(c => c.Name == "cboChucVuEdit");
      
if (cboChucVuEdit == null)
    {
   // Create ComboBox if it doesn't exist
         cboChucVuEdit = new ComboBox
         {
      Name = "cboChucVuEdit",
       Location = textBox4.Location,
     Size = textBox4.Size,
     DropDownStyle = ComboBoxStyle.DropDownList,
      Font = textBox4.Font,
       BackColor = Color.FromArgb(255, 255, 204) // Light yellow
    };
        cboChucVuEdit.Items.AddRange(new object[]
{
    "Nhân viên thu ngân",
         "Nhân viên pha chế",
   "Quản lý",
       "Chủ hàng"
      });
           
  // Hide textBox4 and add ComboBox
    textBox4.Visible = false;
    panel1.Controls.Add(cboChucVuEdit);
     cboChucVuEdit.BringToFront();
 }
 
    // Set ComboBox value
  string currentChucVu = employee.ChucVu ?? "";
if (cboChucVuEdit.Items.Contains(currentChucVu))
{
  cboChucVuEdit.SelectedItem = currentChucVu;
    }
    else
   {
 if (!string.IsNullOrEmpty(currentChucVu))
     {
     cboChucVuEdit.Items.Add(currentChucVu);
        cboChucVuEdit.SelectedItem = currentChucVu;
    }
        else
    {
  cboChucVuEdit.SelectedIndex = 0;
     }
      }

     // Replace textBox5 with ComboBox for VaiTro
           ComboBox cboVaiTroEdit = panel1.Controls.OfType<ComboBox>()
   .FirstOrDefault(c => c.Name == "cboVaiTroEdit");
           
    if (cboVaiTroEdit == null)
 {
       // Create ComboBox for VaiTro
    cboVaiTroEdit = new ComboBox
     {
          Name = "cboVaiTroEdit",
         Location = textBox5.Location,
      Size = textBox5.Size,
     DropDownStyle = ComboBoxStyle.DropDownList,
           Font = textBox5.Font,
         BackColor = Color.FromArgb(255, 255, 204), // Light yellow
     Tag = employee.MaVaiTro // Store current MaVaiTro
     };
  
     // Load VaiTro from database
    var vaiTros = db.VaiTros.OrderBy(v => v.MaVaiTro).ToList();
       foreach (var vt in vaiTros)
       {
         cboVaiTroEdit.Items.Add(new { Text = vt.TenVaiTro, Value = vt.MaVaiTro });
     }
  cboVaiTroEdit.DisplayMember = "Text";
      cboVaiTroEdit.ValueMember = "Value";
  
       // Hide textBox5 and add ComboBox
 textBox5.Visible = false;
    panel1.Controls.Add(cboVaiTroEdit);
 cboVaiTroEdit.BringToFront();
 }
     else
    {
    // Update Tag with current MaVaiTro
    cboVaiTroEdit.Tag = employee.MaVaiTro;
  }
      
     // Set VaiTro ComboBox value
            for (int i = 0; i < cboVaiTroEdit.Items.Count; i++)
   {
      dynamic item = cboVaiTroEdit.Items[i];
           if (item.Value == employee.MaVaiTro)
     {
   cboVaiTroEdit.SelectedIndex = i;
  break;
        }
      }

         textBox6.Text = employee.TenDangNhap;

  // Make textboxes readonly
 textBox1.ReadOnly = true;
      textBox2.ReadOnly = true;
      textBox3.ReadOnly = true;
     textBox6.ReadOnly = true;

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

     // Get new values from ComboBoxes
    ComboBox? cboChucVuEdit = panel1.Controls.OfType<ComboBox>()
    .FirstOrDefault(c => c.Name == "cboChucVuEdit");
     ComboBox? cboVaiTroEdit = panel1.Controls.OfType<ComboBox>()
      .FirstOrDefault(c => c.Name == "cboVaiTroEdit");
         
        if (cboChucVuEdit == null || cboChucVuEdit.SelectedItem == null)
   {
   MessageBox.Show(
          "Vui lòng chọn chức vụ!",
    "Lỗi",
      MessageBoxButtons.OK,
    MessageBoxIcon.Warning);
      return;
 }

   if (cboVaiTroEdit == null || cboVaiTroEdit.SelectedItem == null)
     {
   MessageBox.Show(
         "Vui lòng chọn vai trò!",
  "Lỗi",
  MessageBoxButtons.OK,
   MessageBoxIcon.Warning);
   return;
   }

         string newChucVu = cboChucVuEdit.SelectedItem.ToString() ?? "";
      dynamic selectedVaiTro = cboVaiTroEdit.SelectedItem;
    int newMaVaiTro = selectedVaiTro.Value;
          string newTenVaiTro = selectedVaiTro.Text;

      int oldMaVaiTro = (int)(cboVaiTroEdit.Tag ?? 0);

     // Build confirmation message
    string message = $"Bạn có chắc muốn cập nhật thông tin của nhân viên '{nhanVien.TenNv}'?\n\n";
    message += $"Chức vụ cũ: {nhanVien.ChucVu}\n";
    message += $"Chức vụ mới: {newChucVu}\n\n";
     
 if (oldMaVaiTro != newMaVaiTro && nhanVien.TaiKhoan != null)
      {
       var oldVaiTro = db.VaiTros.FirstOrDefault(v => v.MaVaiTro == oldMaVaiTro);
   message += $"Vai trò cũ: {oldVaiTro?.TenVaiTro}\n";
     message += $"Vai trò mới: {newTenVaiTro}";
  }

 // Confirm update
    var confirmResult = MessageBox.Show(
   message,
         "Xác nhận cập nhật",
          MessageBoxButtons.YesNo,
  MessageBoxIcon.Question);

  if (confirmResult != DialogResult.Yes)
{
    return;
      }

 // Update ChucVu
     nhanVien.ChucVu = newChucVu;
      
      // Update VaiTro if changed and TaiKhoan exists
    if (oldMaVaiTro != newMaVaiTro && nhanVien.TaiKhoan != null)
  {
   var taiKhoan = db.TaiKhoans.FirstOrDefault(tk => tk.MaNv == selectedEmployeeId);
    if (taiKhoan != null)
      {
      taiKhoan.MaVaiTro = newMaVaiTro;
   }
       }

    db.SaveChanges();

MessageBox.Show(
       $"Đã cập nhật thành công!\n" +
      $"Nhân viên: {nhanVien.TenNv}\n" +
  $"Chức vụ mới: {newChucVu}\n" +
      $"Vai trò mới: {newTenVaiTro}",
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
        $"Lỗi khi cập nhật:\n{ex.Message}",
 "Lỗi",
     MessageBoxButtons.OK,
      MessageBoxIcon.Error);
     }
      }
