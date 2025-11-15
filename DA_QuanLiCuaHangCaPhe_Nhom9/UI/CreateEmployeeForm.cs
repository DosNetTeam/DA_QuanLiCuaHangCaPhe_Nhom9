using DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

//csharp UI/CreateEmployeeForm.cs

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class CreateEmployeeForm : Form {
        public string FullName { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public string Position { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public bool IsManager { get; private set; }

        public CreateEmployeeForm() {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;

            // Wire up the event for role selection change
            cb_Vaitro.SelectedIndexChanged += cb_Vaitro_SelectedIndexChanged;
        }

        private void CreateEmployeeForm_Load(object sender, EventArgs e) {
            LoadVaiTro();
            // Disable cb_Chucvu initially until a role is selected
            cb_Chucvu.Enabled = false;
        }

        /// <summary>
        /// Nạp vai trò: thử EF Core trước, nếu lỗi thì dùng ADO.NET fallback.
        /// </summary>
        private void LoadVaiTro() {
            try {
                // Thử EF Core (giữ nguyên cách hoạt động hiện tại)
                using var db = new DataSqlContext();
                var vaiTros = db.VaiTros.ToList();
                cb_Vaitro.DataSource = vaiTros;
                cb_Vaitro.DisplayMember = "TenVaiTro";
                cb_Vaitro.ValueMember = "MaVaiTro";
                cb_Vaitro.SelectedIndex = -1;
            }
            catch (Exception exEf) {
                // Nếu EF lỗi, dùng ADO.NET fallback
                try {
                    var vaiTros = VaiTroRepository.GetAll();
                    cb_Vaitro.DataSource = vaiTros;
                    cb_Vaitro.DisplayMember = "TenVaiTro";
                    cb_Vaitro.ValueMember = "MaVaiTro";
                    cb_Vaitro.SelectedIndex = -1;
                }
                catch (Exception exAdo) {
                    MessageBox.Show(
                        $"Lỗi khi tải danh sách vai trò:\nEF error: {exEf.Message}\nADO error: {exAdo.Message}",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void cb_Vaitro_SelectedIndexChanged(object? sender, EventArgs e) {
            if (cb_Vaitro.SelectedIndex == -1) {
                cb_Chucvu.Enabled = false;
                cb_Chucvu.DataSource = null;
                return;
            }

            // Get selected role
            var selectedVaiTro = cb_Vaitro.SelectedItem as VaiTro;
            if (selectedVaiTro == null) return;

            // Enable position combobox
            cb_Chucvu.Enabled = true;

            // Populate cb_Chucvu based on selected role
            if (selectedVaiTro.TenVaiTro.ToLower().Contains("chủ cửa hàng") || selectedVaiTro.TenVaiTro.ToLower().Contains("chu cua hang")) {
                cb_Chucvu.Enabled = false;
                cb_Chucvu.DataSource = new[] { "Chủ cửa hàng" };
                cb_Chucvu.SelectedIndex = 0;
            }
            else if (selectedVaiTro.TenVaiTro.ToLower().Contains("quản lý") || selectedVaiTro.TenVaiTro.ToLower().Contains("quan ly")) {
                cb_Chucvu.Enabled = false;
                cb_Chucvu.DataSource = new[] { "Quản lý" };
                cb_Chucvu.SelectedIndex = 0;
            }
            else if (selectedVaiTro.TenVaiTro.ToLower().Contains("nhân viên") || selectedVaiTro.TenVaiTro.ToLower().Contains("nhan vien")) {
                cb_Chucvu.Enabled = true;
                cb_Chucvu.DataSource = new[] { "Thu ngân", "Oder" };
                cb_Chucvu.SelectedIndex = -1;
            }
            else {
                cb_Chucvu.DataSource = new[] { "Thu ngân", "Oder" };
                cb_Chucvu.SelectedIndex = -1;
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            // Validate
            if (string.IsNullOrWhiteSpace(txtFullName.Text)) {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text)) {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text)) {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text.Length < 3) {
                MessageBox.Show("Mật khẩu phải có ít nhất 3 ký tự!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text) {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return;
            }

            // Validate role selection
            if (cb_Vaitro.SelectedIndex == -1) {
                MessageBox.Show("Vui lòng chọn vai trò!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cb_Vaitro.Focus();
                return;
            }

            // Validate position selection
            if (cb_Chucvu.SelectedIndex == -1 || cb_Chucvu.SelectedItem == null) {
                MessageBox.Show("Vui lòng chọn chức vụ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cb_Chucvu.Focus();
                return;
            }

            // Validate phone number (optional but if entered, must be valid)
            if (!string.IsNullOrWhiteSpace(txtPhoneNumber.Text)) {
                string phone = txtPhoneNumber.Text.Trim();
                if (phone.Length < 10 || phone.Length > 11 || !phone.All(char.IsDigit)) {
                    MessageBox.Show(
                           "Số điện thoại không hợp lệ!\nVui lòng nhập 10-11 chữ số.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                 MessageBoxIcon.Warning);
                    txtPhoneNumber.Focus();
                    return;
                }
            }

            // Save to database — cố gắng dùng EF, nếu lỗi fallback sang ADO
            try {
                using var db = new DataSqlContext();

                // Check if username already exists
                if (db.TaiKhoans.Any(t => t.TenDangNhap == txtUsername.Text.Trim())) {
                    MessageBox.Show(
                     "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác!",
                     "Lỗi",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                    txtUsername.Focus();
                    return;
                }

                // Get selected role and position
                var selectedVaiTro = cb_Vaitro.SelectedItem as VaiTro;
                string chucVu = cb_Chucvu.SelectedItem?.ToString() ?? string.Empty;

                // Create new NhanVien
                var nhanVien = new NhanVien {
                    TenNv = txtFullName.Text.Trim(),
                    ChucVu = chucVu,
                    SoDienThoai = txtPhoneNumber.Text.Trim(),
                    NgayVaoLam = DateOnly.FromDateTime(DateTime.Now)
                };

                db.NhanViens.Add(nhanVien);
                db.SaveChanges(); // Save to get MaNv

                // Get role ID
                int maVaiTro = selectedVaiTro!.MaVaiTro;

                // Create new TaiKhoan
                var taiKhoan = new TaiKhoan {
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
                IsManager = selectedVaiTro.TenVaiTro.ToLower().Contains("quản lý") ||
                            selectedVaiTro.TenVaiTro.ToLower().Contains("quan ly");

                MessageBox.Show(
                 $"Tạo tài khoản thành công!\n\n" +
                 $"Họ tên: {FullName}\n" +
                 $"Tài khoản: {Username}\n" +
                 $"Vai trò: {selectedVaiTro.TenVaiTro}\n" +
                 $"Chức vụ: {Position}\n" +
                 $"Số điện thoại: {(string.IsNullOrEmpty(PhoneNumber) ? "Chưa cập nhật" : PhoneNumber)}\n" +
                 $"Mã nhân viên: {nhanVien.MaNv}",
                 "Thành công",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception exEf) {
                // EF lỗi → fallback ADO.NET
                try {
                    // Kiểm tra username bằng ADO
                    if (TaiKhoanRepository.ExistsUsername(txtUsername.Text.Trim())) {
                        MessageBox.Show(
                         "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác!",
                         "Lỗi",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
                        txtUsername.Focus();
                        return;
                    }

                    // Lưu NhanVien và TaiKhoan bằng ADO.NET (transaction)
                    var pair = AdoNetHelper.BeginTransaction();
                    var conn = pair.conn;
                    var tx = pair.tx;

                    try {
                        string insertNhanVienSql = @"
                            INSERT INTO NhanVien (TenNV, ChucVu, SoDienThoai, NgayVaoLam, TrangThai)
                            VALUES (@TenNV, @ChucVu, @SoDienThoai, @NgayVaoLam, @TrangThai);
                            SELECT SCOPE_IDENTITY();";

                        var p1 = new Dictionary<string, object> {
                            ["@TenNV"] = txtFullName.Text.Trim(),
                            ["@ChucVu"] = cb_Chucvu.SelectedItem?.ToString() ?? string.Empty,
                            ["@SoDienThoai"] = txtPhoneNumber.Text.Trim(),
                            ["@NgayVaoLam"] = DateOnly.FromDateTime(DateTime.Now).ToDateTime(TimeOnly.MinValue),
                            ["@TrangThai"] = "Đang làm việc"
                        };

                        // Insert and get new MaNv
                        int newMaNv;
                        using (var cmd = conn.CreateCommand()) {
                            cmd.Transaction = tx;
                            cmd.CommandText = insertNhanVienSql;
                            foreach (var kv in p1) cmd.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
                            var idObj = cmd.ExecuteScalar();
                            newMaNv = Convert.ToInt32(idObj);
                        }

                        // Insert TaiKhoan
                        int maVaiTro = (cb_Vaitro.SelectedItem as VaiTro)!.MaVaiTro;
                        string insertTaiKhoanSql = @"
                            INSERT INTO TaiKhoan (TenDangNhap, MatKhau, MaNV, MaVaiTro, TrangThai)
                            VALUES (@TenDangNhap, @MatKhau, @MaNV, @MaVaiTro, @TrangThai)";

                        var p2 = new Dictionary<string, object> {
                            ["@TenDangNhap"] = txtUsername.Text.Trim(),
                            ["@MatKhau"] = txtPassword.Text,
                            ["@MaNV"] = newMaNv,
                            ["@MaVaiTro"] = maVaiTro,
                            ["@TrangThai"] = 1
                        };

                        AdoNetHelper.ExecuteNonQuery(tx, insertTaiKhoanSql, p2);

                        tx.Commit();
                        conn.Close();

                        // Set properties for confirmation
                        FullName = txtFullName.Text.Trim();
                        Username = txtUsername.Text.Trim();
                        Password = txtPassword.Text;
                        Position = cb_Chucvu.SelectedItem?.ToString() ?? string.Empty;
                        PhoneNumber = txtPhoneNumber.Text.Trim();
                        IsManager = (cb_Vaitro.SelectedItem as VaiTro)!.TenVaiTro.ToLower().Contains("quản lý")
                                    || (cb_Vaitro.SelectedItem as VaiTro)!.TenVaiTro.ToLower().Contains("quan ly");

                        MessageBox.Show(
                         $"Tạo tài khoản thành công (ADO)! \n\n" +
                         $"Họ tên: {FullName}\n" +
                         $"Tài khoản: {Username}\n" +
                         $"Vai trò: {(cb_Vaitro.SelectedItem as VaiTro)!.TenVaiTro}\n" +
                         $"Chức vụ: {Position}\n" +
                         $"Số điện thoại: {(string.IsNullOrEmpty(PhoneNumber) ? "Chưa cập nhật" : PhoneNumber)}\n" +
                         $"Mã nhân viên: {newMaNv}",
                         "Thành công",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch {
                        try { tx.Rollback(); } catch { }
                        try { conn.Close(); } catch { }
                        throw;
                    }
                    finally {
                        try { tx.Dispose(); } catch { }
                        try { conn.Dispose(); } catch { }
                    }
                }
                catch (Exception exAdo) {
                    MessageBox.Show(
                        $"Lỗi khi lưu vào database:\nEF error: {exEf.Message}\nADO error: {exAdo.Message}",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cb_Chucvu_SelectedIndexChanged(object? sender, EventArgs e) {
        }

        private void cb_Vaitro_SelectedIndexChanged_1(object? sender, EventArgs e) {
        }
    }
}