using DA_QuanLiCuaHangCaPhe_Nhom9.Function;
// *** THAY ĐỔI: Trỏ đến function_Login ***
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Login;
using Microsoft.Win32;
// Thêm global:: cho các thư viện System

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class Loginform : Form {
        // Khai báo Kho Truy Vấn
        private readonly KhoTruyVanDangNhap _khoDangNhap;

        public Loginform() {
            InitializeComponent();

            // Khởi tạo Kho Truy Vấn
            _khoDangNhap = new KhoTruyVanDangNhap();
        }

        // (Các hàm thông báo Toast giữ nguyên)
        private void OnNotificationRaised(NotificationCenter.Notification note) {
            try { if (note.Type == NotificationCenter.NotificationType.NhanVienInactive) { ShowToast(note.Message); } } catch { }
        }
        private void ShowToast(string message) {
            if (this.InvokeRequired) { this.BeginInvoke(new Action(() => ShowToast(message))); return; }
            Form toast = new Form(); toast.FormBorderStyle = FormBorderStyle.None; toast.StartPosition = FormStartPosition.Manual; toast.BackColor = Color.FromArgb(45, 45, 48); toast.Size = new Size(300, 80); var screen = Screen.PrimaryScreen.WorkingArea; toast.Location = new Point(screen.Right - toast.Width - 10, screen.Bottom - toast.Height - 10); Label lbl = new Label(); lbl.Text = message; lbl.ForeColor = Color.White; lbl.Dock = DockStyle.Fill; lbl.Padding = new Padding(8); toast.Controls.Add(lbl); System.Windows.Forms.Timer t = new System.Windows.Forms.Timer(); t.Interval = 6000; t.Tick += (s, e) => { t.Stop(); toast.Close(); }; t.Start(); toast.Show();
        }
        private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e) {
            if (!IsDisposed && !Disposing) { BeginInvoke(new Action(() => CenterToScreen())); }
        }
        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
            SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
            NotificationCenter.NotificationRaised -= OnNotificationRaised;
        }

        // (Các hàm sự kiện TextChanged giữ nguyên)
        private void textBox1_TextChanged(object? sender, EventArgs e) {
            UpdateLoginButtonState();
        }
        private void textBox2_TextChanged(object? sender, EventArgs e) {
            UpdateLoginButtonState();
        }

        // *** ĐÃ THAY ĐỔI: Gọi KhoTruyVan ***
        private void btnDangnhap_Click(object? sender, EventArgs e) {
            string username = txtUser.Text.Trim();
            string password = txtPass.Text;

            if (string.IsNullOrWhiteSpace(username)) {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(password)) {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPass.Focus(); return;
            }

            try {
                // *** THAY ĐỔI: Gọi KhoTruyVan ***
                var account = _khoDangNhap.XacThuc(username);

                if (account == null) {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPass.Clear(); txtUser.Focus(); return;
                }

                if (account.MatKhau != password) {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPass.Clear(); txtUser.Focus(); return;
                }

                if (account.TrangThai.HasValue && account.TrangThai.Value == false) {
                    MessageBox.Show("Tài khoản đã bị vô hiệu hóa.", "Tài khoản bị khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                txtUser.Clear();
                txtPass.Clear();
                this.txtUser.Focus();

                // Điều hướng (Giữ nguyên)
                if (account.TenVaiTro == "Chủ cửa hàng") {
                    Admin adminForm = new Admin();
                    adminForm.FormClosed += (s, args) => { this.Show(); txtUser.Focus(); UpdateLoginButtonState(); };
                    adminForm.Show();
                }
                else if (account.TenVaiTro == "Quản lý") {
                    QuanLi ql = new QuanLi(account.MaNv);
                    ql.FormClosed += (s, args) => { this.Show(); txtUser.Focus(); UpdateLoginButtonState(); };
                    ql.Show();
                }
                else if (account.TenVaiTro == "Nhân viên") {
                    MainForm mainForm = new MainForm(account.MaNv);
                    mainForm.FormClosed += (s, args) => { this.Show(); txtUser.Focus(); UpdateLoginButtonState(); };
                    mainForm.Show();
                }
                else {
                    MessageBox.Show($"Vai trò '{account.TenVaiTro}' không được hỗ trợ!", "Lỗi vai trò", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Show();
                    txtUser.Focus();
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // (Các hàm còn lại giữ nguyên)
        private void btnHuy_Click(object? sender, EventArgs e) {
            var result = MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) {
                Application.Exit();
            }
        }

        private void UpdateLoginButtonState() {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(txtUser.Text) && !string.IsNullOrWhiteSpace(txtPass.Text);
        }

        private void Loginform_Load(object sender, EventArgs e) {
            StartPosition = FormStartPosition.CenterScreen;
            CenterToScreen();
            this.Text = "Coffee Shop Login";
            txtPass.UseSystemPasswordChar = true;
            txtUser.TabIndex = 0;
            txtPass.TabIndex = 1;
            btnOK.TabIndex = 2;
            btnThoat.TabIndex = 3;
            this.AcceptButton = btnOK;
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            UpdateLoginButtonState();
            NotificationCenter.NotificationRaised += OnNotificationRaised;
        }
    }
}