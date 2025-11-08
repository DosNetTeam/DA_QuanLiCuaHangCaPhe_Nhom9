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

            // Bắt đầu form ở giữa
            StartPosition = FormStartPosition.CenterScreen;
            CenterToScreen();

            // cài đặt tiêu đề form

            this.Text = "Coffee Shop Login";

            // chấp nhận mật khẩu
            txtPass.UseSystemPasswordChar = true;

            // cài đặt tab order
            txtUser.TabIndex = 0;
            txtPass.TabIndex = 1;
            btnOK.TabIndex = 2;
            btnThoat.TabIndex = 3;

            // Được Enter để đăng nhập
            this.AcceptButton = btnOK;

            // Giữ ở giữa nếu cài đặt hiển thị thay đổi (độ phân giải/tỷ lệ)
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

            // Nút đăng nhập bị vô hiệu hóa ban đầu
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
            // Huủy đăng ký sự kiện khi form đóng
            SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
        }

        // --- trình xử lý sự kiện được designer tham chiếu ---
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
            // nhận giá trị đầu vào từ các trường
            string username = txtUser.Text.Trim();
            string password = txtPass.Text;

            // đầu vào hợp lệ
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

            // xác minh đăng nhập với cơ sở dữ liệu
            try
            {
                using var db = new DataSqlContext();

                // Tìm tài khoản theo tên đăng nhập
                var account = db.TaiKhoans
          .Where(t => t.TenDangNhap == username)
          .Select(t => new
          {
              t.TenDangNhap,
              t.MatKhau,
              t.TrangThai,
              t.MaNv,
              NhanVien = t.MaNvNavigation.TenNv,
              ChucVu = t.MaNvNavigation.ChucVu,
              VaiTro = t.MaVaiTroNavigation.TenVaiTro
          })
                .SingleOrDefault();

                if (account.TenDangNhap == null)
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

                // Verify password (NOTE: Cập nhật điều này nếu mật khẩu được băm)
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

                // Authentication successful - Hide login form
                this.Hide();

                // Route to appropriate form based on role and position
                // Priority: Admin > Chủ hàng > Quản lý > Nhân viên
                if (account.VaiTro == "Admin")
                {
                    // Admin role OR Chủ hàng position - Open Admin form
                    Admin adminForm = new Admin();
                    adminForm.FormClosed += (s, args) => this.Close();
                    adminForm.Show();
                }
               
                else if (account.VaiTro == "Quản lý")
                {
                    // Manager role - Open QuanLi form
                    QuanLi ql = new QuanLi();
                    ql.FormClosed += (s, args) => this.Close();
                    ql.Show();
                }
                else if (account.VaiTro == "Nhân viên")
                {
                    // Employee role - Open MainForm with MaNv parameter
                    MainForm mainForm = new MainForm(account.MaNv);
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    // Unknown role - show error and return to login
                    MessageBox.Show(
              $"Vai trò '{account.VaiTro}' không được hỗ trợ!\n" +
                       "Vui lòng liên hệ quản trị viên.",
                 "Lỗi vai trò",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Error
              );
                    this.Show(); // Show login form again
                }
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

    }
}
