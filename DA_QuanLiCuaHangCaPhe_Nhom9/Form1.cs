using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class Form1 : Form
    {
        public Form1()
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
            
            // Allow Enter key to submit form
            this.AcceptButton = btnOK;
            
            // Keep centered if display settings change (resolution/scale)
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
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
            // Optional: Enable/disable login button based on input
            UpdateLoginButtonState();
        }

        private void textBox2_TextChanged(object? sender, EventArgs e)
        {
            // Optional: Enable/disable login button based on input
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

            // TODO: Replace with actual authentication logic (database check, API call, etc.)
            // Example hardcoded credentials for demo:
            if (username == "admin" && password == "123456")
            {
                MessageBox.Show(
                    $"Đăng nhập thành công!\nXin chào {username}",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            
                // TODO: Open main application form
                // Example:
                // MainForm mainForm = new MainForm();
                // mainForm.Show();
                // this.Hide();
            }
            else
            {
                MessageBox.Show(
                    "Tên đăng nhập hoặc mật khẩu không đúng!",
                    "Lỗi đăng nhập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                
                // Clear password field for security
                txtPass.Clear();
                txtUser.Focus();
            }

            // TODO: Kết nối với database để check tài khoản
            // using (var context = new YourDbContext())
            // {
            //     var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            //     if (user != null) { /* success */ }
            // }
        }

        private void label1_Click(object? sender, EventArgs e)
        {
            // Not needed for labels, keep empty
        }

        private void label2_Click(object? sender, EventArgs e)
        {
            // Not needed for labels, keep empty
        }

        private void label3_Click(object? sender, EventArgs e)
        {
            // Not needed for labels, keep empty
        }

        // Helper method to enable/disable login button
        private void UpdateLoginButtonState()
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(txtUser.Text) && 
                            !string.IsNullOrWhiteSpace(txtPass.Text);
        }
    }
}
