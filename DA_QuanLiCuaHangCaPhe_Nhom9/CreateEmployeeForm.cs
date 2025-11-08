using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    public partial class CreateEmployeeForm : Form
    {
        public string FullName { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string Position { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public bool IsEmployee { get; private set; }
        public bool IsManager { get; private set; }

        public CreateEmployeeForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Set default ComboBox value
            cboChucVu.SelectedIndex = 0; // Default to "Nhân viên thu ngân"
                
            // Update ComboBox based on role selection
                UpdateChucVuComboBox();
        }

          private void rbEmployee_CheckedChanged(object? sender, EventArgs e)
        {
          UpdateChucVuComboBox();
        }

        private void rbManager_CheckedChanged(object? sender, EventArgs e)
        {
UpdateChucVuComboBox();
 }

        private void UpdateChucVuComboBox()
        {
     cboChucVu.Items.Clear();
      
 if (rbEmployee.Checked)
     {
         // Employee role - show specific positions
   cboChucVu.Items.Add("Nhân viên thu ngân");
       cboChucVu.Items.Add("Nhân viên pha chế");
          }
   else if (rbManager.Checked)
{
                // Manager role - show management positions
            cboChucVu.Items.Add("Quản lý");
    cboChucVu.Items.Add("Chủ hàng");
     }
  
            // Set default selection
     if (cboChucVu.Items.Count > 0)
      {
 cboChucVu.SelectedIndex = 0;
            }
        }

     private void btnSave_Click(object? sender, EventArgs e)
   {
  // Validate
  if (string.IsNullOrWhiteSpace(txtFullName.Text))
   {
   MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", 
MessageBoxButtons.OK, MessageBoxIcon.Warning);
 txtFullName.Focus();
return;
    }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo",
                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo",
                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return;
            }

          // Validate phone number (optional but if entered, must be valid)
if (!string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
     string phone = txtPhoneNumber.Text.Trim();
       if (phone.Length < 10 || phone.Length > 11 || !phone.All(char.IsDigit))
    {
         MessageBox.Show(
           "Số điện thoại không hợp lệ!\nVui lòng nhập 10-11 chữ số.",
         "Thông báo",
      MessageBoxButtons.OK,
    MessageBoxIcon.Warning);
             txtPhoneNumber.Focus();
      return;
    }
        }

            // Validate ComboBox selection
            if (cboChucVu.SelectedIndex == -1)
            {
         MessageBox.Show("Vui lòng chọn chức vụ!", "Thông báo",
         MessageBoxButtons.OK, MessageBoxIcon.Warning);
      cboChucVu.Focus();
           return;
 }

      // Save to database
try
  {
     using var db = new DataSqlContext();

          // Check if username already exists
if (db.TaiKhoans.Any(t => t.TenDangNhap == txtUsername.Text.Trim()))
          {
   MessageBox.Show(
    "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác!",
 "Lỗi",
          MessageBoxButtons.OK,
  MessageBoxIcon.Error);
   txtUsername.Focus();
      return;
    }

   // Get selected ChucVu from ComboBox
    string chucVu = cboChucVu.SelectedItem?.ToString() ?? "";

     // Create new NhanVien
   var nhanVien = new NhanVien
   {
      TenNv = txtFullName.Text.Trim(),
     ChucVu = chucVu,
    SoDienThoai = txtPhoneNumber.Text.Trim()
   };

                db.NhanViens.Add(nhanVien);
                db.SaveChanges(); // Save to get MaNv

       // Determine VaiTro ID based on RadioButton
   int maVaiTro = rbManager.Checked ? 2 : 1; // 2=Quản lý, 1=Nhân viên

     // Verify VaiTro exists
          if (!db.VaiTros.Any(v => v.MaVaiTro == maVaiTro))
   {
      MessageBox.Show(
 $"Vai trò với mã {maVaiTro} không tồn tại trong database!",
         "Lỗi",
MessageBoxButtons.OK,
         MessageBoxIcon.Error);
  
       // Rollback - remove the employee we just added
 db.NhanViens.Remove(nhanVien);
  db.SaveChanges();
   return;
  }

                // Create new TaiKhoan
                var taiKhoan = new TaiKhoan
                {
                    TenDangNhap = txtUsername.Text.Trim(),
                    MatKhau = txtPassword.Text, // NOTE: Should hash password in production
                    MaNv = nhanVien.MaNv,
                    MaVaiTro = maVaiTro,
                    TrangThai = true
                };

                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();

    // Set properties for confirmation
    FullName = txtFullName.Text.Trim();
    Username = txtUsername.Text.Trim();
          Password = txtPassword.Text;
        Position = chucVu;
        PhoneNumber = txtPhoneNumber.Text.Trim();
  IsEmployee = rbEmployee.Checked;
          IsManager = rbManager.Checked;

    MessageBox.Show(
$"Tạo tài khoản thành công!\n\n" +
$"Họ tên: {FullName}\n" +
 $"Tài khoản: {Username}\n" +
  $"Chức vụ: {Position}\n" +
       $"Vai trò: {(IsManager ? "Quản lý" : "Nhân viên")}\n" +
       $"Số điện thoại: {(string.IsNullOrEmpty(PhoneNumber) ? "Chưa cập nhật" : PhoneNumber)}\n" +
   $"Mã nhân viên: {nhanVien.MaNv}",
    "Thành công",
     MessageBoxButtons.OK,
    MessageBoxIcon.Information);

   this.DialogResult = DialogResult.OK;
  this.Close();
      }
  catch (Exception ex)
 {
       MessageBox.Show(
       $"Lỗi khi lưu vào database:\n{ex.Message}\n\n{ex.InnerException?.Message}",
   "Lỗi",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error);
      }
    }

        private void btnCancel_Click(object? sender, EventArgs e)
  {
  this.DialogResult = DialogResult.Cancel;
this.Close();
        }
    }
}
