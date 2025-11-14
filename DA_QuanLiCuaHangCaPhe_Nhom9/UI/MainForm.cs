using DA_QuanLiCuaHangCaPhe_Nhom9.Function;
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using global::System.Globalization;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {

    public partial class MainForm : Form {
        // === KHAI BÁO CÁC BIẾN ===
        private int _currentMaNV = 3;
        private int? _currentMaKH = null;
        private string _currentMaLoai = "TatCa";
        private decimal tongGoc = 0;
        private decimal soTienGiam = 0;

        // Khai báo các lớp dịch vụ, kho, và giỏ hàng
        private readonly DichVuDonHang _dichVuDonHang;
        private readonly KhoTruyVanMainForm _khoTruyVan;
        private readonly GioHang _gioHang; // <-- BIẾN MỚI


        #region thông báo toast
        // (Giữ nguyên code)
        private void NotificationCenter_NotificationRaised(NotificationCenter.Notification n) {
            try { if (n.Type == NotificationCenter.NotificationType.UnpaidInvoice || n.Type == NotificationCenter.NotificationType.LowStock) { ShowToast(n.Message); } } catch { }
        }
        private void ShowToast(string message) {
            if (this.InvokeRequired) { this.BeginInvoke(new Action(() => ShowToast(message))); return; }
            Form toast = new Form(); toast.FormBorderStyle = FormBorderStyle.None; toast.StartPosition = FormStartPosition.Manual; toast.BackColor = Color.FromArgb(45, 45, 48); toast.Size = new Size(350, 90); var ownerRect = this.Bounds; var screen = Screen.FromControl(this).WorkingArea; var x = Math.Min(ownerRect.Right + 10, screen.Right - toast.Width - 10); var y = Math.Min(ownerRect.Bottom - toast.Height - 10, screen.Bottom - toast.Height - 10); toast.Location = new Point(x - toast.Width, y); Label lbl = new Label(); lbl.Text = message; lbl.ForeColor = Color.White; lbl.Dock = DockStyle.Fill; lbl.Padding = new Padding(8); toast.Controls.Add(lbl); System.Windows.Forms.Timer t = new System.Windows.Forms.Timer(); t.Interval = 6000; t.Tick += (s, e) => { t.Stop(); toast.Close(); }; t.Start(); toast.Show(this);
        }
        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e); NotificationCenter.NotificationRaised -= NotificationCenter_NotificationRaised;
        }
        #endregion

        #region Hàm khởi tạo và tải form
        public MainForm(int MaNV) {
            InitializeComponent();
            _currentMaNV = MaNV;

            // *** THAY ĐỔI: Khởi tạo các lớp ***
            _dichVuDonHang = new DichVuDonHang();
            _khoTruyVan = new KhoTruyVanMainForm();
            // Truyền _dichVuDonHang vào cho _gioHang sử dụng
            _gioHang = new GioHang(_dichVuDonHang);

            NotificationCenter.NotificationRaised += NotificationCenter_NotificationRaised;
        }

        private void MainForm_Load(object sender, EventArgs e) {
            lvDonHang.View = View.Details;
            lvDonHang.Columns.Clear();
            lvDonHang.Columns.Add("Tên SP", 200);
            lvDonHang.Columns.Add("SL", 40);
            lvDonHang.Columns.Add("Đơn Giá", 80);
            lvDonHang.Columns.Add("Thành Tiền", 100);

            TaiLoaiSanPham();
            TaiSanPham("TatCa");

            this.btnThem.Enabled = false;
            this.btnThem.Visible = true;
        }
        #endregion

        #region Các hàm tải dữ liệu (Đã tách CSDL)

        // (Giữ nguyên)
        private void TaiLoaiSanPham() {
            flpLoaiSP.Controls.Clear();
            flpLoaiSP.FlowDirection = FlowDirection.TopDown;
            try {
                var cacLoaiSP = _khoTruyVan.TaiLoaiSanPham();
                Button btnTatCa = new Button { Text = "Tất Cả", Tag = "TatCa", Width = flpLoaiSP.Width, Height = 45, Margin = new Padding(5), Font = new Font("Segoe UI", 9F, FontStyle.Bold), BackColor = Color.LightGray, };
                btnTatCa.Click += BtnLoai_Click;
                flpLoaiSP.Controls.Add(btnTatCa);
                foreach (var tenLoai in cacLoaiSP) {
                    Button btn = new Button { Text = tenLoai, Tag = tenLoai, Width = flpLoaiSP.Width, Height = 50, Margin = new Padding(5) };
                    btn.Click += BtnLoai_Click;
                    flpLoaiSP.Controls.Add(btn);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi tải loại sản phẩm: " + ex.InnerException?.Message ?? ex.Message); }
        }

        // (Giữ nguyên)
        private void TaiSanPham(string maLoai) {
            flpSanPham.Controls.Clear();
            string searchText = txtTimKiemSP.Text.Trim().ToLower();
            try {
                var duLieu = _khoTruyVan.LayDuLieuSanPham();
                var tatCaSanPham = duLieu.TatCaSanPham;
                var allDinhLuong = duLieu.AllDinhLuong;
                var allNguyenLieu = duLieu.AllNguyenLieu;

                foreach (var sp in tatCaSanPham) {
                    if (maLoai != "TatCa" && sp.LoaiSp != maLoai) continue;
                    if (!string.IsNullOrEmpty(searchText) && !sp.TenSp.ToLower().Contains(searchText)) continue;

                    Button btn = new Button { Text = $"{sp.TenSp}\n{sp.DonGia:N0} đ", Tag = sp, Width = 140, Height = 100, Margin = new Padding(5), BackColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.Black };
                    btn.FlatAppearance.BorderSize = 1; btn.FlatAppearance.BorderColor = Color.Gainsboro;

                    var trangThaiKho = _dichVuDonHang.KiemTraDuNguyenLieu(sp.MaSp, allDinhLuong, allNguyenLieu);
                    switch (trangThaiKho) {
                        case DichVuDonHang.TrangThaiKho.DuHang: btn.Enabled = true; btn.BackColor = Color.White; btn.ForeColor = Color.Black; break;
                        case DichVuDonHang.TrangThaiKho.SapHet: btn.Enabled = true; btn.BackColor = Color.Orange; btn.ForeColor = Color.White; btn.Text += "\n(Sắp hết)"; break;
                        case DichVuDonHang.TrangThaiKho.HetHang: default: btn.Enabled = false; btn.BackColor = Color.LightGray; btn.ForeColor = Color.Gray; btn.Text += "\n(HẾT HÀNG)"; break;
                    }
                    btn.Click += BtnSanPham_Click;
                    flpSanPham.Controls.Add(btn);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi khi tải sản phẩm: " + ex.Message); }
        }

        #endregion

        #region Các hàm xử lý sự kiện (Event Handlers)

        // (Giữ nguyên)
        private void BtnLoai_Click(object sender, EventArgs e) {
            Button nutDuocBam = (Button)sender;
            string maLoai = nutDuocBam.Tag.ToString();
            _currentMaLoai = maLoai;
            TaiSanPham(maLoai);
        }

        // *** THAY ĐỔI: Gọi _gioHang.ThemMon ***
        private void BtnSanPham_Click(object sender, EventArgs e) {
            Button nutDuocBam = (Button)sender;
            SanPham spDuocChon = (SanPham)nutDuocBam.Tag;

            // 1. Gọi logic giỏ hàng
            var ketQua = _gioHang.ThemMon(spDuocChon);

            // 2. Kiểm tra kết quả
            if (ketQua.Success) {
                // 3. Cập nhật UI
                CapNhatGiaoDienGioHang();
                CapNhatTongTien();
            }
            else {
                // Báo lỗi nếu thêm thất bại (ví dụ: hết hàng)
                MessageBox.Show(ketQua.Message, "Hết Hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // (Giữ nguyên)
        private void btnThanhToan_Click(object sender, EventArgs e) {
            // *** THAY ĐỔI: Kiểm tra giỏ hàng bằng _gioHang ***
            if (_gioHang.LaySoLuongMon() > 0) {
                // CÓ MÓN (THANH TOÁN NHANH)
                int maDonHangVuaTao = ThucHienLuuTam();
                if (maDonHangVuaTao > 0) {
                    ThanhToan frmThanhToan = new ThanhToan(maDonHangVuaTao, tongGoc, soTienGiam);
                    var result = frmThanhToan.ShowDialog();
                    if ((result == DialogResult.OK) || (result == DialogResult.Cancel)) {
                        ResetMainForm();
                    }
                }
            }
            else {
                // GIỎ HÀNG RỖNG (THANH TOÁN ĐƠN CŨ)
                ChonDonHangCho cdhc = new ChonDonHangCho();
                var resultChon = cdhc.ShowDialog();
                if (resultChon == DialogResult.OK) {
                    int maDonHangChon = cdhc.MaDonHangDaChon;
                    ThanhToan thanhtoan = new ThanhToan(maDonHangChon, tongGoc, soTienGiam); // tongGoc, soTienGiam sẽ là 0
                    var resultThanhToan = thanhtoan.ShowDialog();
                    if ((resultThanhToan == DialogResult.OK) || (resultThanhToan == DialogResult.Cancel)) {
                        TaiSanPham(_currentMaLoai);
                    }
                }
                else if (resultChon == DialogResult.Cancel) TaiSanPham(_currentMaLoai);
            }
        }

        // *** THAY ĐỔI: Gọi _gioHang.XoaTatCa ***
        private void btnHuyDon_Click(object sender, EventArgs e) {

            if (lvDonHang.SelectedItems.Count > 0) {
                var confirm = MessageBox.Show("Bạn có chắc muốn hủy đơn hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes) {
                    ResetMainForm();
                }
            }
            else {
                MessageBox.Show("Vui lòng thêm sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        // *** THAY ĐỔI: Gọi _gioHang.XoaMon ***
        private void btnXoaMon_Click(object sender, EventArgs e) {
            if (lvDonHang.SelectedItems.Count > 0) {
                // 1. Lấy MaSp từ Tag của ListViewItem
                ListViewItem itemDaChon = lvDonHang.SelectedItems[0];
                int maSp = (int)itemDaChon.Tag;

                // 2. Gọi logic giỏ hàng
                _gioHang.XoaMon(maSp);

                // 3. Cập nhật UI
                CapNhatGiaoDienGioHang();
                CapNhatTongTien();
            }
            else {
                MessageBox.Show("Vui lòng chọn một món ăn trong giỏ hàng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // *** THAY ĐỔI: Gọi _gioHang.GiamSoLuong ***
        private void btnGIamSoLuong_Click(object sender, EventArgs e) {
            if (lvDonHang.SelectedItems.Count > 0) {
                // 1. Lấy MaSp từ Tag
                ListViewItem itemDaChon = lvDonHang.SelectedItems[0];
                int maSp = (int)itemDaChon.Tag;
                int soLuongHienTai = int.Parse(itemDaChon.SubItems[1].Text);

                bool daXoaMon = false;

                if (soLuongHienTai > 1) {
                    // Chỉ giảm
                    _gioHang.GiamSoLuong(maSp);
                }
                else {
                    // Giảm về 0 -> Xóa
                    var confirm = MessageBox.Show(
                        "Số lượng món này là 1. Giảm nữa sẽ xóa món này khỏi giỏ hàng. Bạn có chắc không?",
                        "Xác nhận xóa món",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes) {
                        _gioHang.GiamSoLuong(maSp);
                        daXoaMon = true;
                    }
                }

                // 3. Cập nhật UI (nếu có thay đổi)
                if (daXoaMon || soLuongHienTai > 1) {
                    CapNhatGiaoDienGioHang();
                    CapNhatTongTien();
                }
            }
            else {
                MessageBox.Show("Vui lòng chọn một món ăn trong giỏ hàng để giảm số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // (Giữ nguyên)
        private void txtTimKiemSP_TextChanged(object sender, EventArgs e) {
            TaiSanPham(_currentMaLoai);
        }

        // (Giữ nguyên)
        private void btnThem_Click(object sender, EventArgs e) {
            ThemKhachHangMoi tmk = new ThemKhachHangMoi(txtTimKiemKH.Text.Trim());
            var result = tmk.ShowDialog();
            if (result == DialogResult.OK) {
                SearchKhachHangBySDT(txtTimKiemKH.Text.Trim());
            }
        }

        // (Giữ nguyên)
        private void btnLuuTam_Click(object sender, EventArgs e) {
            int maDonHangMoi = ThucHienLuuTam();
            if (maDonHangMoi > 0) {
                MessageBox.Show($"Đã lưu tạm đơn hàng {maDonHangMoi}", "Lưu tạm thành công");
                ResetMainForm();
            }
        }

        // (Giữ nguyên)
        private void txtTimKiemKH_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) {
                e.Handled = true;
            }
        }

        // (Giữ nguyên)
        private void txtTimKiemKH_TextChanged(object sender, EventArgs e) {
            string sdt = txtTimKiemKH.Text.Trim();
            if (string.IsNullOrEmpty(sdt)) { lblTenKH.Text = "Khách vãng lai"; _currentMaKH = null; btnThem.Enabled = false; return; }
            if (sdt.Length != 10 || !sdt.All(char.IsDigit)) { lblTenKH.Text = "Nhập đủ 10 số"; _currentMaKH = null; btnThem.Enabled = false; return; }
            SearchKhachHangBySDT(sdt);
        }

        #endregion

        #region Các hàm logic nghiệp vụ (Business Logic)

        // *** HÀM MỚI: Cập nhật ListView từ GioHang ***
        /// <summary>
        /// Xóa sạch ListView và vẽ lại từ dữ liệu trong _gioHang.
        /// </summary>
        private void CapNhatGiaoDienGioHang() {
            lvDonHang.Items.Clear();

            var dsMonAn = _gioHang.LayTatCaMon();

            foreach (var item in dsMonAn) {
                ListViewItem lvi = new ListViewItem(item.TenSp);
                lvi.Tag = item.MaSp;
                lvi.SubItems.Add(item.SoLuong.ToString());
                lvi.SubItems.Add(item.DonGiaGoc.ToString("N0"));
                lvi.SubItems.Add(item.ThanhTienGoc.ToString("N0"));

                lvDonHang.Items.Add(lvi);
            }
        }


        /*
        HÀM ThemSanPhamVaoDonHang() CŨ ĐÃ BỊ XÓA
        (Logic đã chuyển vào BtnSanPham_Click và GioHang.cs)
        */


        // *** THAY ĐỔI: Tính tổng từ _gioHang ***
        private void CapNhatTongTien() {
            // 1. Lấy tổng tiền gốc từ giỏ hàng
            decimal tongTien = _gioHang.LayTongTienGoc();
            decimal tongTienGiamGia = 0;

            var dsMonAn = _gioHang.LayTatCaMon();

            // Lặp qua giỏ hàng để tính giảm giá 'SanPham'
            foreach (var item in dsMonAn) {
                // Gọi DichVuDonHang để lấy giá đã KM
                decimal donGiaMoi = _dichVuDonHang.GetGiaBan(item.MaSp, item.DonGiaGoc);
                decimal discountPerItem = item.DonGiaGoc - donGiaMoi;
                tongTienGiamGia += (discountPerItem * item.SoLuong);
            }

            // 2. Tìm khuyến mãi 'HoaDon' (Gọi KhoTruyVan)
            KhuyenMai kmHoaDon = null;
            try {
                // Logic này giữ nguyên (vẫn gọi KhoTruyVan)
                kmHoaDon = _khoTruyVan.LayKhuyenMaiHoaDon();
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lấy KM hóa đơn: " + ex.Message);
            }

            // 3. Tính toán giảm giá 'HoaDon'
            decimal baseForHoaDon = tongTien - tongTienGiamGia;
            decimal tongGiamGiaHoaDon = 0;
            if (kmHoaDon != null) {
                decimal phanTramGiam = kmHoaDon.GiaTri / 100;
                tongGiamGiaHoaDon = baseForHoaDon * phanTramGiam;
            }

            // 4. Tính toán tổng cuối cùng
            decimal tongTienGiaHD = tongTienGiamGia + tongGiamGiaHoaDon;
            decimal finalTotal = tongTien - tongTienGiaHD;

            // 5. Cập nhật UI
            lblTienTruocGiam.Text = tongTien.ToString("N0") + " đ";
            lblGiamGia.Text = (-tongTienGiaHD).ToString("N0") + " đ";
            lblTongCong.Text = finalTotal.ToString("N0") + " đ";

            soTienGiam = tongTienGiaHD;
            tongGoc = tongTien;
        }


        // *** THAY ĐỔI: Lấy dữ liệu từ _gioHang ***
        private int ThucHienLuuTam() {
            if (_gioHang.LaySoLuongMon() == 0) {
                MessageBox.Show("Vui lòng thêm sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            string tongTienStr = lblTongCong.Text.Replace(" đ", "").Replace(".", "");
            decimal tongTien = decimal.Parse(tongTienStr, CultureInfo.InvariantCulture);

            try {
                // Bước 1: Chuẩn bị dữ liệu (Lấy từ _gioHang)
                var gioHangChoDb = new List<ChiTietGioHang>();
                foreach (var item in _gioHang.LayTatCaMon()) {
                    gioHangChoDb.Add(new ChiTietGioHang {
                        MaSP = item.MaSp,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGiaGoc // Lưu giá gốc vào CSDL
                    });
                }

                // Bước 2: Gọi KhoTruyVan
                int maDonHangMoi = _khoTruyVan.LuuDonHangTam(gioHangChoDb, tongTien, _currentMaNV, _currentMaKH);

                if (maDonHangMoi == -1) {
                    MessageBox.Show("Lỗi khi lưu tạm đơn hàng. Vui lòng kiểm tra log.");
                }
                return maDonHangMoi;
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lưu tạm đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
                return -1;
            }
        }

        // *** THAY ĐỔI: Gọi _gioHang.XoaTatCa() ***
        private void ResetMainForm() {
            // 1. Xóa giỏ hàng (logic)
            _gioHang.XoaTatCa();
            // 2. Xóa giỏ hàng (UI)
            lvDonHang.Items.Clear();
            // 3. Cập nhật tổng tiền (về 0)
            CapNhatTongTien();
            // 4. Reset thông tin khách hàng
            lblTenKH.Text = "Khách vãng lai";
            _currentMaKH = null;
            txtTimKiemKH.Text = "";
            txtTimKiemSP.Text = "";
            // 5. Tải lại danh sách sản phẩm (Vì kho có thể đã bị trừ)
            TaiSanPham(_currentMaLoai);
            lvDonHang.SelectedItems.Clear();
            btnThem.Enabled = false;
        }

        // *** THAY ĐỔI: Gọi _khoTruyVan ***
        private void SearchKhachHangBySDT(string sdt) {
            try {
                // 1. Gọi KhoTruyVan
                var khachHang = _khoTruyVan.SearchKhachHangBySDT(sdt);

                // 2. Cập nhật UI
                if (khachHang != null) {
                    lblTenKH.Text = khachHang.TenKh;
                    _currentMaKH = khachHang.MaKh;
                    btnThem.Enabled = false;
                }
                else {
                    lblTenKH.Text = "Không tìm thấy KH";
                    _currentMaKH = null;
                    btnThem.Enabled = true;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tìm khách hàng: " + ex.Message);
                lblTenKH.Text = "Lỗi khi tìm";
                _currentMaKH = null;
                btnThem.Enabled = false;
            }
        }

        #endregion

        private void flpLoaiSP_Paint(object sender, PaintEventArgs e) {

        }
    }
}