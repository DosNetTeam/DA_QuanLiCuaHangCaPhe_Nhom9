using DA_QuanLiCuaHangCaPhe_Nhom9.Function;
// Thêm namespace của lớp service mới
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using System.Data;
using System.Globalization;
// Thư viện Linq mà code gốc của bạn đã dùng (ví dụ: .Select, .FirstOrDefault)

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {

    public partial class MainForm : Form {
        // === KHAI BÁO CÁC BIẾN ===

        private int _currentMaNV = 3;
        private int? _currentMaKH = null;
        private string _currentMaLoai = "TatCa";
        private decimal tongGoc = 0;
        private decimal soTienGiam = 0;

        // Khởi tạo lớp dịch vụ
        private readonly DichVuDonHang _dichVuDonHang;


        #region thông báo toast
        // (Giữ nguyên code)
        private void NotificationCenter_NotificationRaised(NotificationCenter.Notification n) {
            try {
                if (n.Type == NotificationCenter.NotificationType.UnpaidInvoice || n.Type == NotificationCenter.NotificationType.LowStock) {
                    ShowToast(n.Message);
                }
            }
            catch { }
        }

        private void ShowToast(string message) {
            if (this.InvokeRequired) {
                this.BeginInvoke(new Action(() => ShowToast(message)));
                return;
            }

            Form toast = new Form();
            toast.FormBorderStyle = FormBorderStyle.None;
            toast.StartPosition = FormStartPosition.Manual;
            toast.BackColor = Color.FromArgb(45, 45, 48);
            toast.Size = new Size(350, 90);

            var ownerRect = this.Bounds;
            var screen = Screen.FromControl(this).WorkingArea;
            var x = Math.Min(ownerRect.Right + 10, screen.Right - toast.Width - 10);
            var y = Math.Min(ownerRect.Bottom - toast.Height - 10, screen.Bottom - toast.Height - 10);
            toast.Location = new Point(x - toast.Width, y);

            Label lbl = new Label();
            lbl.Text = message;
            lbl.ForeColor = Color.White;
            lbl.Dock = DockStyle.Fill;
            lbl.Padding = new Padding(8);

            toast.Controls.Add(lbl);

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 6000;
            t.Tick += (s, e) => { t.Stop(); toast.Close(); };
            t.Start();

            toast.Show(this);
        }

        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
            NotificationCenter.NotificationRaised -= NotificationCenter_NotificationRaised;
        }
        #endregion

        #region Hàm khởi tạo và tải form
        public MainForm(int MaNV) {
            InitializeComponent();
            _currentMaNV = MaNV;

            // Khởi tạo DichVuDonHang khi MainForm được tạo
            _dichVuDonHang = new DichVuDonHang();

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

        #region Các hàm tải dữ liệu (Load Data - Dùng EF Core)

        // Tải các nút Loại Sản Phẩm (Giữ NGUYÊN BẢN logic)
        private void TaiLoaiSanPham() {
            flpLoaiSP.Controls.Clear();
            flpLoaiSP.FlowDirection = FlowDirection.TopDown;

            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Logic .Select, .Where, .Distinct gốc của bạn
                    var cacLoaiSP = db.SanPhams
                                     .Select(sp => sp.LoaiSp)
                                     .Where(loai => loai != null && loai != "")
                                     .Distinct()
                                     .ToList();

                    // 1. Tạo nút "Tất Cả"
                    Button btnTatCa = new Button {
                        Text = "Tất Cả",
                        Tag = "TatCa",
                        Width = flpLoaiSP.Width,
                        Height = 45,
                        Margin = new Padding(5),
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        BackColor = Color.LightGray,
                    };

                    btnTatCa.Click += BtnLoai_Click;
                    flpLoaiSP.Controls.Add(btnTatCa);

                    // 2. Tạo các nút cho từng loại
                    foreach (var tenLoai in cacLoaiSP) {
                        Button btn = new Button {
                            Text = tenLoai,
                            Tag = tenLoai,
                            Width = flpLoaiSP.Width,
                            Height = 50,
                            Margin = new Padding(5)
                        };
                        btn.Click += BtnLoai_Click;
                        flpLoaiSP.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải loại sản phẩm: " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        // Tải các nút Sản Phẩm (Giữ NGUYÊN BẢN logic)
        private void TaiSanPham(string maLoai) {
            flpSanPham.Controls.Clear();
            string searchText = txtTimKiemSP.Text.Trim().ToLower();

            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // --- BƯỚC 1: LẤY HẾT DỮ LIỆU (Logic gốc của bạn) ---
                    var tatCaSanPham = db.SanPhams.Where(sp => sp.TrangThai == "Còn bán").ToList();
                    var allDinhLuong = db.DinhLuongs.ToList();
                    var allNguyenLieu = db.NguyenLieus.Where(nl => nl.TrangThai == "Đang kinh doanh").ToList();

                    // --- BƯỚC 2: LỌC BẰNG FOREACH (Logic gốc của bạn) ---
                    foreach (var sp in tatCaSanPham) {
                        // 1. Lọc theo Loại
                        if (maLoai != "TatCa" && sp.LoaiSp != maLoai) {
                            continue;
                        }

                        // 2. Lọc theo Tên
                        if (!string.IsNullOrEmpty(searchText) && !sp.TenSp.ToLower().Contains(searchText)) {
                            continue;
                        }

                        // Tạo nút
                        Button btn = new Button {
                            Text = $"{sp.TenSp}\n{sp.DonGia:N0} đ",
                            Tag = sp,
                            Width = 140,
                            Height = 100,
                            Margin = new Padding(5),
                            BackColor = Color.White,
                            FlatStyle = FlatStyle.Flat,
                            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                            ForeColor = Color.Black
                        };

                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.Gainsboro;

                        // *** THAY ĐỔI: Gọi hàm từ DichVuDonHang ***
                        var trangThaiKho = _dichVuDonHang.KiemTraDuNguyenLieu(sp.MaSp, allDinhLuong, allNguyenLieu);

                        switch (trangThaiKho) {
                            case DichVuDonHang.TrangThaiKho.DuHang:
                                btn.Enabled = true;
                                btn.BackColor = Color.White;
                                btn.ForeColor = Color.Black;
                                break;

                            case DichVuDonHang.TrangThaiKho.SapHet:
                                btn.Enabled = true;
                                btn.BackColor = Color.Orange;
                                btn.ForeColor = Color.White;
                                btn.Text += "\n(Sắp hết)";
                                break;

                            case DichVuDonHang.TrangThaiKho.HetHang:
                            default:
                                btn.Enabled = false;
                                btn.BackColor = Color.LightGray;
                                btn.ForeColor = Color.Gray;
                                btn.Text += "\n(HẾT HÀNG)";
                                break;
                        }

                        btn.Click += BtnSanPham_Click;
                        flpSanPham.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải sản phẩm: " + ex.Message);
            }
        }

        /*
        HÀM GetGiaBan() ĐÃ ĐƯỢC DI CHUYỂN SANG DichVuDonHang.cs
        */

        #endregion

        #region Các hàm xử lý sự kiện (Event Handlers)
        // (Giữ nguyên code)

        private void BtnLoai_Click(object sender, EventArgs e) {
            Button nutDuocBam = (Button)sender;
            string maLoai = nutDuocBam.Tag.ToString();
            _currentMaLoai = maLoai;
            TaiSanPham(maLoai);
        }

        private void BtnSanPham_Click(object sender, EventArgs e) {
            Button nutDuocBam = (Button)sender;
            SanPham spDuocChon = (SanPham)nutDuocBam.Tag;
            ThemSanPhamVaoDonHang(spDuocChon);
        }

        private void btnThanhToan_Click(object sender, EventArgs e) {
            if (lvDonHang.Items.Count > 0) {
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
                ChonDonHangCho cdhc = new ChonDonHangCho();
                var resultChon = cdhc.ShowDialog();

                if (resultChon == DialogResult.OK) {
                    int maDonHangChon = cdhc.MaDonHangDaChon;
                    ThanhToan thanhtoan = new ThanhToan(maDonHangChon, tongGoc, soTienGiam);
                    var resultThanhToan = thanhtoan.ShowDialog();

                    if (resultThanhToan == DialogResult.OK) {
                        TaiSanPham(_currentMaLoai);
                    }
                }
            }
        }

        private void btnHuyDon_Click(object sender, EventArgs e) {
            var confirm = MessageBox.Show("Bạn có chắc muốn hủy đơn hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes) {
                ResetMainForm();
            }
        }

        private void btnXoaMon_Click(object sender, EventArgs e) {
            if (lvDonHang.SelectedItems.Count > 0) {
                ListViewItem itemDaChon = lvDonHang.SelectedItems[0];
                lvDonHang.Items.Remove(itemDaChon);
                CapNhatTongTien();
            }
            else {
                MessageBox.Show("Vui lòng chọn một món ăn trong giỏ hàng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnGIamSoLuong_Click(object sender, EventArgs e) {
            if (lvDonHang.SelectedItems.Count > 0) {
                ListViewItem itemDaChon = lvDonHang.SelectedItems[0];
                int soLuongHienTai = int.Parse(itemDaChon.SubItems[1].Text);
                bool daThayDoi = false;

                if (soLuongHienTai > 1) {
                    int soLuongMoi = soLuongHienTai - 1;
                    string donGiaStr = itemDaChon.SubItems[2].Text.Replace(".", "");
                    decimal donGia = decimal.Parse(donGiaStr, System.Globalization.CultureInfo.InvariantCulture);
                    decimal thanhTienMoi = soLuongMoi * donGia;

                    itemDaChon.SubItems[1].Text = soLuongMoi.ToString();
                    itemDaChon.SubItems[3].Text = thanhTienMoi.ToString("N0");
                    daThayDoi = true;
                }
                else {
                    var confirm = MessageBox.Show(
                        "Số lượng món này là 1. Giảm nữa sẽ xóa món này khỏi giỏ hàng. Bạn có chắc không?",
                        "Xác nhận xóa món",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes) {
                        lvDonHang.Items.Remove(itemDaChon);
                        daThayDoi = true;
                    }
                }

                if (daThayDoi) {
                    CapNhatTongTien();
                }
            }
            else {
                MessageBox.Show("Vui lòng chọn một món ăn trong giỏ hàng để giảm số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtTimKiemSP_TextChanged(object sender, EventArgs e) {
            TaiSanPham(_currentMaLoai);
        }

        #region nút tìm KH - đã bỏ
        private void btnTimKH_Click(object sender, EventArgs e) {
            // (Logic đã chuyển sang txtTimKiemKH_TextChanged)
        }
        #endregion

        private void btnThem_Click(object sender, EventArgs e) {
            ThemKhachHangMoi tmk = new ThemKhachHangMoi(txtTimKiemKH.Text.Trim());
            var result = tmk.ShowDialog();

            if (result == DialogResult.OK) {
                SearchKhachHangBySDT(txtTimKiemKH.Text.Trim());
            }
        }

        private void btnLuuTam_Click(object sender, EventArgs e) {
            int maDonHangMoi = ThucHienLuuTam();
            if (maDonHangMoi > 0) {
                MessageBox.Show($"Đã lưu tạm đơn hàng {maDonHangMoi}", "Lưu tạm thành công");
                ResetMainForm();
            }
        }

        private void txtTimKiemKH_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void txtTimKiemKH_TextChanged(object sender, EventArgs e) {
            string sdt = txtTimKiemKH.Text.Trim();

            if (string.IsNullOrEmpty(sdt)) {
                lblTenKH.Text = "Khách vãng lai";
                _currentMaKH = null;
                btnThem.Enabled = false;
                return;
            }

            if (sdt.Length != 10 || !sdt.All(char.IsDigit)) {
                lblTenKH.Text = "Nhập đủ 10 số";
                _currentMaKH = null;
                btnThem.Enabled = false;
                return;
            }

            SearchKhachHangBySDT(sdt);
        }

        #endregion

        #region Các hàm logic nghiệp vụ (Business Logic)

        /*
        HÀM KiemTraDuNguyenLieu() ĐÃ ĐƯỢC DI CHUYỂN SANG DichVuDonHang.cs
        */

        /*
        HÀM KiemTraSoLuongTonThucTe() ĐÃ ĐƯỢC DI CHUYỂN SANG DichVuDonHang.cs
        */


        // Hàm này xử lý việc thêm SP vào giỏ hàng (ListView)
        private void ThemSanPhamVaoDonHang(SanPham sp) {
            foreach (ListViewItem item in lvDonHang.Items) {
                int maSpTrongGio = (int)item.Tag;
                if (maSpTrongGio == sp.MaSp) {
                    // ----- ĐÃ CÓ, TĂNG SỐ LƯỢNG -----
                    int soLuongCu = int.Parse(item.SubItems[1].Text);
                    int soLuongMoi = soLuongCu + 1;

                    // *** THAY ĐỔI: Gọi hàm từ DichVuDonHang ***
                    var kiemTra = _dichVuDonHang.KiemTraSoLuongTonThucTe(sp.MaSp, soLuongMoi);

                    if (kiemTra.DuHang == false) {
                        MessageBox.Show(kiemTra.ThongBao, "Hết Hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    decimal thanhTienMoi = soLuongMoi * sp.DonGia;
                    item.SubItems[1].Text = soLuongMoi.ToString();
                    item.SubItems[3].Text = thanhTienMoi.ToString("N0");
                    CapNhatTongTien();
                    return;
                }
            }

            // ----- CHƯA CÓ, THÊM DÒNG MỚI -----

            // *** THAY ĐỔI: Gọi hàm từ DichVuDonHang ***
            var kiemTraMoi = _dichVuDonHang.KiemTraSoLuongTonThucTe(sp.MaSp, 1);

            if (kiemTraMoi.DuHang == false) {
                MessageBox.Show(kiemTraMoi.ThongBao, "Hết Hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ListViewItem lvi = new ListViewItem(sp.TenSp);
            lvi.Tag = sp.MaSp;
            lvi.SubItems.Add("1"); // Số lượng
            lvi.SubItems.Add(sp.DonGia.ToString("N0")); // Đơn giá
            lvi.SubItems.Add(sp.DonGia.ToString("N0")); // Thành tiền

            lvDonHang.Items.Add(lvi);
            CapNhatTongTien();
        }

        // Hàm này tính lại tổng tiền từ đầu
        private void CapNhatTongTien() {
            decimal tongTien = 0;
            decimal tongTienGiamGia = 0;

            foreach (ListViewItem item in lvDonHang.Items) {
                int maSP = (int)item.Tag;
                int soLuong = int.Parse(item.SubItems[1].Text);

                string donGiaGocStr = item.SubItems[2].Text.Replace(".", "");
                decimal donGiaGoc = decimal.Parse(donGiaGocStr, CultureInfo.CurrentCulture);

                string chuoiThanhTien = item.SubItems[3].Text.Replace(".", "");
                decimal thanhTien = decimal.Parse(chuoiThanhTien, CultureInfo.CurrentCulture);

                tongTien = tongTien + thanhTien;

                // *** THAY ĐỔI: Gọi hàm từ DichVuDonHang ***
                decimal donGiaMoi = _dichVuDonHang.GetGiaBan(maSP, donGiaGoc);
                decimal discountPerItem = donGiaGoc - donGiaMoi;
                tongTienGiamGia += (discountPerItem * soLuong);
            }

            // 2. Tìm khuyến mãi 'HoaDon' 
            // *** ĐÃ KHÔI PHỤC LOGIC GỐC CỦA BẠN ***
            KhuyenMai kmHoaDon = null;
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                    var allKM = db.KhuyenMais.ToList(); // Lấy tất cả

                    // Dùng foreach y hệt code gốc của bạn
                    foreach (KhuyenMai km in allKM) {
                        if (km.LoaiKm == "HoaDon" &&
                            km.TrangThai == "Đang áp dụng" &&
                            km.NgayBatDau <= now &&
                            km.NgayKetThuc >= now) {
                            if (kmHoaDon == null || km.GiaTri > kmHoaDon.GiaTri) {
                                kmHoaDon = km;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lấy KM hóa đơn: " + ex.Message);
            }

            // 3. Tính toán giảm giá 'HoaDon' (Giữ nguyên)
            decimal baseForHoaDon = tongTien - tongTienGiamGia;
            decimal tongGiamGiaHoaDon = 0;

            if (kmHoaDon != null) {
                decimal phanTramGiam = kmHoaDon.GiaTri / 100;
                tongGiamGiaHoaDon = baseForHoaDon * phanTramGiam;
            }

            // 4. Tính toán tổng cuối cùng (Giữ nguyên)
            decimal tongTienGiaHD = tongTienGiamGia + tongGiamGiaHoaDon;
            decimal finalTotal = tongTien - tongTienGiaHD;

            // 5. Cập nhật UI (Giữ nguyên)
            lblTienTruocGiam.Text = tongTien.ToString("N0") + " đ";
            lblGiamGia.Text = (-tongTienGiaHD).ToString("N0") + " đ";
            lblTongCong.Text = finalTotal.ToString("N0") + " đ";

            soTienGiam = tongTienGiaHD;
            tongGoc = tongTien;
        }


        // (Hàm này trả về MaDH mới, hoặc -1 nếu lỗi)
        // Giữ NGUYÊN BẢN logic
        private int ThucHienLuuTam() {
            if (lvDonHang.Items.Count == 0) {
                MessageBox.Show("Vui lòng thêm sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            string tongTienStr = lblTongCong.Text.Replace(" đ", "").Replace(".", "");
            decimal tongTien = decimal.Parse(tongTienStr, CultureInfo.InvariantCulture);

            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Bước 1: Tạo DonHang
                    var donHangMoi = new DonHang {
                        NgayLap = DateTime.Now,
                        MaNv = _currentMaNV,
                        TrangThai = "Đang xử lý",
                        TongTien = tongTien,
                        MaKh = _currentMaKH
                    };

                    // Bước 2: Tạo List ChiTietDonHang
                    var listChiTiet = new List<ChiTietDonHang>();
                    foreach (ListViewItem item in lvDonHang.Items) {
                        int maSP = (int)item.Tag;
                        int soLuong = int.Parse(item.SubItems[1].Text);
                        decimal donGia = decimal.Parse(item.SubItems[2].Text.Replace(".", ""), CultureInfo.InvariantCulture);

                        var chiTiet = new ChiTietDonHang {
                            MaDhNavigation = donHangMoi,
                            MaSp = maSP,
                            SoLuong = soLuong,
                            DonGia = donGia
                        };
                        listChiTiet.Add(chiTiet);
                    }

                    // Bước 3: Tạo ThanhToan "Chưa thanh toán"
                    var thanhToanMoi = new Models.ThanhToan {
                        MaDhNavigation = donHangMoi,
                        HinhThuc = null,
                        SoTien = tongTien,
                        TrangThai = "Chưa thanh toán"
                    };

                    // Bước 4: Add vào DbContext
                    db.DonHangs.Add(donHangMoi);
                    db.ChiTietDonHangs.AddRange(listChiTiet);
                    db.ThanhToans.Add(thanhToanMoi);

                    // Bước 5: Trừ kho (Logic gốc của bạn)
                    foreach (var monAn in listChiTiet) {
                        var congThuc = db.DinhLuongs
                                         .Where(dl => dl.MaSp == monAn.MaSp)
                                         .ToList();

                        if (congThuc.Count > 0) {
                            foreach (var nguyenLieuCan in congThuc) {
                                var nguyenLieuTrongKho = db.NguyenLieus
                                                           .FirstOrDefault(nl => nl.MaNl == nguyenLieuCan.MaNl);
                                if (nguyenLieuTrongKho != null) {
                                    decimal luongCanTru = nguyenLieuCan.SoLuongCan * monAn.SoLuong;
                                    nguyenLieuTrongKho.SoLuongTon -= luongCanTru;
                                    db.Update(nguyenLieuTrongKho);
                                }
                            }
                        }
                    }

                    // Bước 6: Lưu CSDL
                    db.SaveChanges();

                    // Bước 7: Trả về MaDH mới
                    return donHangMoi.MaDh;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lưu tạm đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
                return -1;
            }
        }

        // Reset lại form về trạng thái ban đầu
        private void ResetMainForm() {
            lvDonHang.Items.Clear();
            CapNhatTongTien(); // Về 0
            lblTenKH.Text = "Khách vãng lai";
            _currentMaKH = null;
            txtTimKiemKH.Text = "";
            txtTimKiemSP.Text = ""; // Giữ nguyên thay đổi này
            TaiSanPham(_currentMaLoai);
            lvDonHang.SelectedItems.Clear();
            btnThem.Enabled = false; // Giữ nguyên thay đổi này
        }

        // Tìm khách hàng theo SĐT (Giữ NGUYÊN BẢN logic)
        private void SearchKhachHangBySDT(string sdt) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Logic .FirstOrDefault gốc của bạn
                    var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SoDienThoai == sdt);

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