using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using System.Linq;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class Loginform : Form
    {
        public Loginform()
        {
            InitializeComponent();

            // Start centered on screen
            StartPosition = FormStartPosition.CenterScreen;
            CenterToScreen();

            // Set form title
            this.Text = "Coffee Shop Login";

            // Configure password textbox to hide characters
            txtPass.UseSystemPasswordChar = true;

            // Set tab order for better UX
            txtUser.TabIndex = 0;
            txtPass.TabIndex = 1;
            btnOK.TabIndex = 2;
            btnThoat.TabIndex = 3;

            // Allow Enter key to submit form
            this.AcceptButton = btnOK;

            // Keep centered if display settings change (resolution/scale)
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

            // Initialize login button state
            UpdateLoginButtonState();
        }

        private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e)
        {
            if (!IsDisposed && !Disposing)
            {
                BeginInvoke(new Action(() => CenterToScreen()));
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            // Unsubscribe to avoid memory leaks
            SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
        }

        // --- Event handlers referenced by Designer ---
        private void textBox1_TextChanged(object? sender, EventArgs e)
        {
            UpdateLoginButtonState();
        }

        private void textBox2_TextChanged(object? sender, EventArgs e)
        {
            UpdateLoginButtonState();
        }

        private void button1_Click(object? sender, EventArgs e)
        {
            // Get input values
            string username = txtUser.Text.Trim();
            string password = txtPass.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show(
      "Vui lòng nhập tên đăng nhập!",
               "Thông báo",
         MessageBoxButtons.OK,
             MessageBoxIcon.Warning
          );
                txtUser.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
          "Vui lòng nhập mật khẩu!",
            "Thông báo",
           MessageBoxButtons.OK,
       MessageBoxIcon.Warning
      );
                txtPass.Focus();
                return;
            }

            // Authenticate against database
            try
            {
                using var db = new DataSqlContext();

                // Tìm tên tài khoản
                
                var account = db.TaiKhoans
          .Where(t => t.TenDangNhap == username)
          .Select(t => new
          {
              t.TenDangNhap,
              t.MatKhau,
              t.TrangThai,
              t.MaNv,
              NhanVien = t.MaNvNavigation.TenNv,
              VaiTro = t.MaVaiTroNavigation.TenVaiTro
          })
                .SingleOrDefault();

                if (account == null)
                {
                    MessageBox.Show(
                     "Tên đăng nhập hoặc mật khẩu không đúng!",
                 "Lỗi đăng nhập",
                        MessageBoxButtons.OK,
                       MessageBoxIcon.Error
                   );
                    txtPass.Clear();
                    txtUser.Focus();
                    return;
                }

                // Verify password (NOTE: Update this if passwords are hashed)
                if (account.MatKhau != password)
                {
                    MessageBox.Show(
                      "Tên đăng nhập hoặc mật khẩu không đúng!",
                       "Lỗi đăng nhập",
                      MessageBoxButtons.OK,
                  MessageBoxIcon.Error
                 );
                    txtPass.Clear();
                    txtUser.Focus();
                    return;
                }

                // Check account status
                if (account.TrangThai.HasValue && account.TrangThai.Value == false)
                {
                    MessageBox.Show(
                     "Tài khoản đã bị vô hiệu hóa. Vui lòng liên hệ quản trị viên.",
                      "Tài khoản bị khóa",
                                  MessageBoxButtons.OK,
                       MessageBoxIcon.Warning
                              );
                    return;
                }

                // Authentication successful
                MessageBox.Show(
                   $"Đăng nhập thành công!\n" +
               $"Xin chào: {account.NhanVien}\n" +
                     $"Vai trò: {account.VaiTro}",
                     "Thành công",
                MessageBoxButtons.OK,
          MessageBoxIcon.Information
                  );

                // Open Admin form and hide login form
                Admin adminForm = new Admin();
                this.Hide();
                adminForm.FormClosed += (s, args) => this.Close();
                adminForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
             $"Lỗi kết nối cơ sở dữ liệu:\n{ex.Message}",
             "Lỗi",
                    MessageBoxButtons.OK,
                     MessageBoxIcon.Error
                );
            }
        }

        // Handler for Cancel button
        private void btnThoat_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                 "Bạn có chắc muốn thoát?",
              "Xác nhận",
                      MessageBoxButtons.YesNo,
               MessageBoxIcon.Question
           );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label1_Click(object? sender, EventArgs e)
        {
            // Not needed for labels
        }

        private void label2_Click(object? sender, EventArgs e)
        {
            // Not needed for labels
        }

        private void label3_Click(object? sender, EventArgs e)
        {
            // Not needed for labels
        }

        // Helper method to enable/disable login button
        private void UpdateLoginButtonState()
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(txtUser.Text) &&
                !string.IsNullOrWhiteSpace(txtPass.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form load logic if needed
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // PictureBox click logic if needed
        }
    }
}
