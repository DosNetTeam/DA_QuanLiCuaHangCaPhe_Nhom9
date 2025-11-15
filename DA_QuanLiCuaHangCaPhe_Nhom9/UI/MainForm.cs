using DA_QuanLiCuaHangCaPhe_Nhom9.Function;                 // import namespace chứa các dịch vụ nghiệp vụ chung
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main;   // import các lớp trong folder function_Main (GioHang, KhoTruyVanMainForm, DichVuDonHang,...)
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;                   // import các entity model EF Core (SanPham, DonHang, ...)
using global::System.Globalization;                         // import CultureInfo/formatting dùng khi parse/format số/ngày

namespace DA_QuanLiCuaHangCaPhe_Nhom9 { // namespace của project

    // MainForm: màn hình POS chính nơi nhân viên tạo đơn, quản lý giỏ hàng và thanh toán.
    public partial class MainForm : Form {
        // === KHAI BÁO CÁC BIẾN ===
        // _currentMaNV: id nhân viên hiện tại, được truyền từ Loginform.
        private int _currentMaNV = 3;
        // _currentMaKH: id khách hàng (nullable) nếu tìm thấy qua SĐT.
        private int? _currentMaKH = null;
        // _currentMaLoai: bộ lọc loại sản phẩm hiện tại ("TatCa" mặc định).
        private string _currentMaLoai = "TatCa";
        // tongGoc: tổng tiền trước khi áp dụng giảm / dùng để truyền sang ThanhToan.
        private decimal tongGoc = 0;
        // soTienGiam: tổng tiền đã giảm (dùng để hiển thị và truyền).
        private decimal soTienGiam = 0;

        // Các service/ repository / domain logic:
        // _dichVuDonHang: service chứa logic giá bán / kiểm tra kho cho sản phẩm.
        private readonly DichVuDonHang _dichVuDonHang;
        // _khoTruyVan: repository dữ liệu cần cho MainForm (sản phẩm, khách,...).
        private readonly KhoTruyVanMainForm _khoTruyVan;
        // _gioHang: lớp quản lý trạng thái giỏ hàng trên client (thêm/xóa/giam/lay)
        private readonly GioHang _gioHang; // <-- BIẾN MỚI


        #region thông báo toast
        // Bắt sự kiện thông báo toàn cục để hiển thị toast (NotificationCenter pattern).
        private void NotificationCenter_NotificationRaised(NotificationCenter.Notification n) {
            try { if (n.Type == NotificationCenter.NotificationType.UnpaidInvoice || n.Type == NotificationCenter.NotificationType.LowStock) { ShowToast(n.Message); } } catch { }
        }
        // Tạo form nhỏ làm "toast" — vị trí tính dựa trên vị trí form chính.
        private void ShowToast(string message) {
            if (this.InvokeRequired) { this.BeginInvoke(new Action(() => ShowToast(message))); return; }
            Form toast = new Form(); // biên thể toast
            toast.FormBorderStyle = FormBorderStyle.None;
            toast.StartPosition = FormStartPosition.Manual;
            toast.BackColor = Color.FromArgb(45, 45, 48);
            toast.Size = new Size(350, 90);
            var ownerRect = this.Bounds;
            var screen = Screen.FromControl(this).WorkingArea;
            // Tính toạ độ đặt toast bên phải của form (nếu không quá rìa màn hình)
            var x = Math.Min(ownerRect.Right + 10, screen.Right - toast.Width - 10);
            var y = Math.Min(ownerRect.Bottom - toast.Height - 10, screen.Bottom - toast.Height - 10);
            toast.Location = new Point(x - toast.Width, y);
            Label lbl = new Label(); lbl.Text = message; lbl.ForeColor = Color.White; lbl.Dock = DockStyle.Fill; lbl.Padding = new Padding(8);
            toast.Controls.Add(lbl);
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 6000; // thời gian hiển thị 6s
            t.Tick += (s, e) => { t.Stop(); toast.Close(); };
            t.Start();
            toast.Show(this); // hiển thị và đặt owner
        }
        // Khi form đóng, huỷ đăng ký sự kiện thông báo
        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
            NotificationCenter.NotificationRaised -= NotificationCenter_NotificationRaised; // unsubscribe
        }
        #endregion

        #region Hàm khởi tạo và tải form
        // Constructor: nhận MaNV từ Loginform, khởi tạo các service và khởi đăng ký notification
        public MainForm(int MaNV) {
            InitializeComponent();          // phương thức auto-generated khởi tạo control
            _currentMaNV = MaNV;            // gán id nhân viên hiện tại

            // Khởi tạo service: dùng để tách logic khỏi UI
            _dichVuDonHang = new DichVuDonHang();
            _khoTruyVan = new KhoTruyVanMainForm();
            // GioHang nhận DichVuDonHang để kiểm tra giá / tồn kho khi cần
            _gioHang = new GioHang(_dichVuDonHang);

            // Đăng ký lắng nghe notification toàn cục
            NotificationCenter.NotificationRaised += NotificationCenter_NotificationRaised;
        }

        // Load event: cấu hình ListView giỏ hàng và tải loại/sản phẩm
        private void MainForm_Load(object sender, EventArgs e) {
            // Cấu hình hiển thị ListView lvDonHang
            lvDonHang.View = View.Details;
            lvDonHang.Columns.Clear();
            lvDonHang.Columns.Add("Tên SP", 200);
            lvDonHang.Columns.Add("SL", 40);
            lvDonHang.Columns.Add("Đơn Giá", 80);
            lvDonHang.Columns.Add("Thành Tiền", 100);

            // Tải danh sách loại sản phẩm và các nút sản phẩm
            TaiLoaiSanPham();
            TaiSanPham("TatCa");

            // Nút thêm khách (btnThem) mặc định vô hiệu cho đến khi cần
            this.btnThem.Enabled = false;
            this.btnThem.Visible = true;
        }
        #endregion

        #region Các hàm tải dữ liệu (Đã tách CSDL)

        // Tạo danh sách nút cho từng loại sản phẩm (dựa trên dữ liệu từ KhoTruyVan)
        private void TaiLoaiSanPham() {
            flpLoaiSP.Controls.Clear();                         // xóa controls cũ
            flpLoaiSP.FlowDirection = FlowDirection.TopDown;    // sắp xếp top->down
            try {
                var cacLoaiSP = _khoTruyVan.TaiLoaiSanPham(); // Lấy danh sách tên loại
                // Tạo nút "Tất Cả"
                Button btnTatCa = new Button { Text = "Tất Cả", Tag = "TatCa", Width = flpLoaiSP.Width, Height = 45, Margin = new Padding(5), Font = new Font("Segoe UI", 9F, FontStyle.Bold), BackColor = Color.LightGray, };
                btnTatCa.Click += BtnLoai_Click;
                flpLoaiSP.Controls.Add(btnTatCa);
                // Tạo nút cho từng loại trả về
                foreach (var tenLoai in cacLoaiSP) {
                    Button btn = new Button { Text = tenLoai, Tag = tenLoai, Width = flpLoaiSP.Width, Height = 50, Margin = new Padding(5) };
                    btn.Click += BtnLoai_Click;
                    flpLoaiSP.Controls.Add(btn);
                }
            }
            catch (Exception ex) { // nếu lỗi khi gọi repo
                MessageBox.Show("Lỗi khi tải loại sản phẩm: " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        // Tạo các nút sản phẩm trong flowpanel flpSanPham dựa trên bộ lọc và tìm kiếm
        private void TaiSanPham(string maLoai) {
            flpSanPham.Controls.Clear(); // xóa các nút cũ
            string searchText = txtTimKiemSP.Text.Trim().ToLower(); // lấy nội dung tìm kiếm, chuẩn hoá lowercase
            try {
                // Lấy data aggregate (sản phẩm, định lượng, nguyên liệu)
                var duLieu = _khoTruyVan.LayDuLieuSanPham();
                var tatCaSanPham = duLieu.TatCaSanPham;
                var allDinhLuong = duLieu.AllDinhLuong;
                var allNguyenLieu = duLieu.AllNguyenLieu;

                foreach (var sp in tatCaSanPham) {
                    // Áp filter loại và text search
                    if (maLoai != "TatCa" && sp.LoaiSp != maLoai) continue;
                    if (!string.IsNullOrEmpty(searchText) && !sp.TenSp.ToLower().Contains(searchText)) continue;

                    // Tạo nút hiển thị tên và giá
                    Button btn = new Button { Text = $"{sp.TenSp}\n{sp.DonGia:N0} đ", Tag = sp, Width = 140, Height = 100, Margin = new Padding(5), BackColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.Black };
                    btn.FlatAppearance.BorderSize = 1; btn.FlatAppearance.BorderColor = Color.Gainsboro; // style đơn giản

                    // Kiểm tra trạng thái kho cho món (DuHang/SapHet/HetHang) bằng DichVuDonHang
                    var trangThaiKho = _dichVuDonHang.KiemTraDuNguyenLieu(sp.MaSp, allDinhLuong, allNguyenLieu);
                    switch (trangThaiKho) {
                        case DichVuDonHang.TrangThaiKho.DuHang: // đủ nguyên liệu
                            btn.Enabled = true; btn.BackColor = Color.White; btn.ForeColor = Color.Black;
                            break;
                        case DichVuDonHang.TrangThaiKho.SapHet: // sắp hết
                            btn.Enabled = true; btn.BackColor = Color.Orange; btn.ForeColor = Color.White; btn.Text += "\n(Sắp hết)";
                            break;
                        case DichVuDonHang.TrangThaiKho.HetHang: // hết hàng
                        default:
                            btn.Enabled = false; btn.BackColor = Color.LightGray; btn.ForeColor = Color.Gray; btn.Text += "\n(HẾT HÀNG)";
                            break;
                    }
                    // Gán sự kiện click sản phẩm
                    btn.Click += BtnSanPham_Click;
                    flpSanPham.Controls.Add(btn);
                }
            }
            catch (Exception ex) { // nếu lỗi khi lấy dữ liệu
                MessageBox.Show("Lỗi khi tải sản phẩm: " + ex.Message);
            }
        }

        #endregion

        #region Các hàm xử lý sự kiện (Event Handlers)

        // Xử lý khi chọn 1 loại: lấy tag của nút để lọc và tải lại sản phẩm
        private void BtnLoai_Click(object sender, EventArgs e) {
            Button nutDuocBam = (Button)sender;             // cast sender về Button
            string maLoai = nutDuocBam.Tag.ToString();      // lấy tag (mã loại)
            _currentMaLoai = maLoai;                        // gán loại hiện tại
            TaiSanPham(maLoai);                             // tải lại sản phẩm theo loại
        }

        // Thêm sản phẩm vào giỏ hàng: gọi logic trong GioHang và cập nhật UI nếu thành công
        private void BtnSanPham_Click(object sender, EventArgs e) {
            Button nutDuocBam = (Button)sender;             // button được bấm
            SanPham spDuocChon = (SanPham)nutDuocBam.Tag;   // lấy entity SanPham từ Tag

            // 1. Gọi logic giỏ hàng (thêm 1 đơn vị)
            var ketQua = _gioHang.ThemMon(spDuocChon);

            // 2. Kiểm tra kết quả add
            if (ketQua.Success) {
                // 3. Nếu ok -> cập nhật listview và tổng tiền
                CapNhatGiaoDienGioHang();
                CapNhatTongTien();
            }
            else {
                // Nếu thất bại (ví dụ hết nguyên liệu) -> báo cho người dùng
                MessageBox.Show(ketQua.Message, "Hết Hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Xử lý nút Thanh Toán:
        // - Nếu giỏ hàng có món -> lưu tạm, mở ThanhToan cho đơn vừa tạo
        // - Nếu giỏ hàng rỗng -> mở ChonDonHangCho để chọn đơn cũ thanh toán
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_gioHang.LaySoLuongMon() > 0)
            {
                // KỊCH BẢN 1: TẠO ĐƠN MỚI (ĐÃ CHẠY ĐÚNG)
                int maDonHangVuaTao = ThucHienLuuTam();
                if (maDonHangVuaTao > 0)
                {
                    // Truyền tongGoc, soTienGiam hiện tại của giỏ hàng
                    ThanhToan frmThanhToan = new ThanhToan(maDonHangVuaTao, tongGoc, soTienGiam);
                    var result = frmThanhToan.ShowDialog();

                    if ((result == DialogResult.OK) || (result == DialogResult.Cancel))
                    {
                        ResetMainForm();
                    }
                }
            }
            else
            {
                // KỊCH BẢN 2: MỞ ĐƠN CŨ (ĐANG BỊ LỖI)
                ChonDonHangCho cdhc = new ChonDonHangCho();
                var resultChon = cdhc.ShowDialog();

                if (resultChon == DialogResult.OK)
                {
                    int maDonHangChon = cdhc.MaDonHangDaChon;

                    // --- BẮT ĐẦU SỬA LỖI ---
                    // Phải tải lại chi tiết đơn hàng đã chọn để lấy tổng gốc và tổng giảm
                    // vì 'tongGoc' và 'soTienGiam' của MainForm hiện tại là 0

                    // 1. Dùng DichVuThanhToan để tải lại chi tiết đơn hàng
                    DichVuThanhToan dichVuThanhToan = new DichVuThanhToan();
                    // 2. Dùng LayChiTietDonHangGoc (vì nó lấy cả DonHang và ChiTietDonHangs)
                    var donHangGoc = dichVuThanhToan.LayChiTietDonHangGoc(maDonHangChon);

                    if (donHangGoc != null && donHangGoc.ChiTietDonHangs != null)
                    {
                        decimal tongGocMoi = 0;

                        // 3. Tính lại Tiền trước giảm (tổng gốc)
                        // (ThucHienLuuTam đã lưu DonGiaGoc vào ct.DonGia)
                        foreach (var ct in donHangGoc.ChiTietDonHangs)
                        {
                            tongGocMoi += (ct.DonGia * ct.SoLuong);
                        }

                        // 4. Lấy Thành tiền (tổng cuối) đã lưu trong đơn
                        decimal tongCuoi = donHangGoc.TongTien ?? 0;

                        // 5. Tính lại Giảm giá
                        decimal soTienGiamMoi = tongGocMoi - tongCuoi;

                        // 6. Mở ThanhToan với DỮ LIỆU ĐÚNG của đơn hàng cũ
                        ThanhToan thanhtoan = new ThanhToan(maDonHangChon, tongGocMoi, soTienGiamMoi);
                        var resultThanhToan = thanhtoan.ShowDialog();

                        if ((resultThanhToan == DialogResult.OK) || (resultThanhToan == DialogResult.Cancel))
                        {
                            TaiSanPham(_currentMaLoai);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Không thể tải chi tiết đơn hàng đã chọn.");
                        TaiSanPham(_currentMaLoai);
                    }
                    // --- KẾT THÚC SỬA LỖI ---
                }
                else if (resultChon == DialogResult.Cancel)
                {
                    TaiSanPham(_currentMaLoai);
                }
            }
        }

        // Hủy đơn hiện tại (trong UI): hỏi xác nhận rồi reset nếu có món
        private void btnHuyDon_Click(object sender, EventArgs e) {

            if (lvDonHang.SelectedItems.Count > 0) { // nếu có item được chọn trong ListView
                var confirm = MessageBox.Show("Bạn có chắc muốn hủy đơn hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes) {
                    ResetMainForm(); // reset toàn bộ form -> xóa giỏ, reset thông tin
                }
            }
            else {
                MessageBox.Show("Vui lòng thêm sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Xóa 1 món đã chọn khỏi giỏ: lấy MaSp từ Tag của ListViewItem và gọi GioHang.XoaMon
        private void btnXoaMon_Click(object sender, EventArgs e) {
            if (lvDonHang.SelectedItems.Count > 0) {
                // Lấy item được chọn
                ListViewItem itemDaChon = lvDonHang.SelectedItems[0];
                int maSp = (int)itemDaChon.Tag;

                // Gọi logic giỏ hàng để xóa
                _gioHang.XoaMon(maSp);

                // Cập nhật UI sau khi xóa
                CapNhatGiaoDienGioHang();
                CapNhatTongTien();
            }
            else {
                MessageBox.Show("Vui lòng chọn một món ăn trong giỏ hàng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Giảm số lượng 1 đơn vị cho món được chọn:
        // - Nếu >1 thì chỉ giảm
        // - Nếu =1 thì hỏi xác nhận xóa
        private void btnGIamSoLuong_Click(object sender, EventArgs e) {
            if (lvDonHang.SelectedItems.Count > 0) {
                ListViewItem itemDaChon = lvDonHang.SelectedItems[0];
                int maSp = (int)itemDaChon.Tag;
                int soLuongHienTai = int.Parse(itemDaChon.SubItems[1].Text); // cột SL

                bool daXoaMon = false;

                if (soLuongHienTai > 1) {
                    // Giảm đơn giản
                    _gioHang.GiamSoLuong(maSp);
                }
                else {
                    // Xác nhận trước khi xóa nếu giảm sẽ về 0
                    var confirm = MessageBox.Show(
                        "Số lượng món này là 1. Giảm nữa sẽ xóa món này khỏi giỏ hàng. Bạn có chắc không?",
                        "Xác nhận xóa món",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes) {
                        _gioHang.GiamSoLuong(maSp); // gọi giảm (sẽ xóa item)
                        daXoaMon = true;
                    }
                }

                // Nếu có sự thay đổi -> cập nhật UI và tổng tiền
                if (daXoaMon || soLuongHienTai > 1) {
                    CapNhatGiaoDienGioHang();
                    CapNhatTongTien();
                }
            }
            else {
                MessageBox.Show("Vui lòng chọn một món ăn trong giỏ hàng để giảm số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // TextChanged cho tìm sản phẩm -> reload sản phẩm với filter mới
        private void txtTimKiemSP_TextChanged(object sender, EventArgs e) {
            TaiSanPham(_currentMaLoai);
        }

        // Mở form thêm khách hàng mới (nếu SĐT không tìm thấy)
        private void btnThem_Click(object sender, EventArgs e) {
            ThemKhachHangMoi tmk = new ThemKhachHangMoi(txtTimKiemKH.Text.Trim()); // truyền SĐT sẵn có
            var result = tmk.ShowDialog();
            if (result == DialogResult.OK) {
                SearchKhachHangBySDT(txtTimKiemKH.Text.Trim()); // nếu thêm thành công -> tìm lại KH để bind id
            }
        }

        // Lưu tạm đơn (không in, không trừ kho nữa vì LuuDonHangTam trong KhoTruyVan đã trừ)
        private void btnLuuTam_Click(object sender, EventArgs e) {
            int maDonHangMoi = ThucHienLuuTam();
            if (maDonHangMoi > 0) {
                MessageBox.Show($"Đã lưu tạm đơn hàng {maDonHangMoi}", "Lưu tạm thành công");
                ResetMainForm(); // reset giao diện sau khi lưu
            }
        }

        // Chỉ cho phép nhập số trong ô tìm SĐT khách hàng
        private void txtTimKiemKH_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) {
                e.Handled = true; // chặn ký tự không phải số
            }
        }

        // Khi thay đổi text tìm SĐT, validate độ dài 10 chữ số rồi gọi SearchKhachHangBySDT
        private void txtTimKiemKH_TextChanged(object sender, EventArgs e) {
            string sdt = txtTimKiemKH.Text.Trim();
            if (string.IsNullOrEmpty(sdt)) { // nếu rỗng -> mặc định khách vãng lai
                lblTenKH.Text = "Khách vãng lai";
                _currentMaKH = null;
                btnThem.Enabled = false;
                return;
            }
            if (sdt.Length != 10 || !sdt.All(char.IsDigit)) { // nếu chưa đủ 10 chữ số -> yêu cầu nhập đủ
                lblTenKH.Text = "Nhập đủ 10 số";
                _currentMaKH = null;
                btnThem.Enabled = false;
                return;
            }
            SearchKhachHangBySDT(sdt); // nếu hợp lệ -> gọi repo tìm khách
        }

        #endregion

        #region Các hàm logic nghiệp vụ (Business Logic)

        // Cập nhật ListView hiển thị giỏ hàng từ dữ liệu trong _gioHang
        private void CapNhatGiaoDienGioHang() {
            lvDonHang.Items.Clear(); // xóa tất cả item hiện tại

            var dsMonAn = _gioHang.LayTatCaMon(); // Lấy danh sách item từ GioHang

            foreach (var item in dsMonAn) {
                ListViewItem lvi = new ListViewItem(item.TenSp); // cột 0: tên
                lvi.Tag = item.MaSp;                              // Tag lưu MaSp để thao tác
                lvi.SubItems.Add(item.SoLuong.ToString());        // cột 1: số lượng
                lvi.SubItems.Add(item.DonGiaGoc.ToString("N0"));  // cột 2: đơn giá gốc
                lvi.SubItems.Add(item.ThanhTienGoc.ToString("N0"));// cột 3: thành tiền gốc

                lvDonHang.Items.Add(lvi); // thêm row vào ListView
            }
        }


        /*
        HÀM ThemSanPhamVaoDonHang() CŨ ĐÃ BỊ XÓA
        (Logic đã chuyển vào BtnSanPham_Click và GioHang.cs)
        */


        // Tính tổng tiền hiển thị trên UI dựa trên giỏ hàng và khuyến mãi
        private void CapNhatTongTien() {
            // 1. Tổng tiền gốc (chưa áp giảm)
            decimal tongTien = _gioHang.LayTongTienGoc();
            decimal tongTienGiamGia = 0;

            var dsMonAn = _gioHang.LayTatCaMon();               // lấy danh sách món

            // 2. Tính giảm giá theo từng sản phẩm (dùng DichVuDonHang.GetGiaBan)
            foreach (var item in dsMonAn) {
                // GetGiaBan trả về giá sau KM (nếu có), giá gốc là item.DonGiaGoc
                decimal donGiaMoi = _dichVuDonHang.GetGiaBan(item.MaSp, item.DonGiaGoc);
                decimal discountPerItem = item.DonGiaGoc - donGiaMoi;
                tongTienGiamGia += (discountPerItem * item.SoLuong);
            }

            // 3. Tìm khuyến mãi theo hóa đơn (loại HoaDon) từ KhoTruyVan
            KhuyenMai kmHoaDon = null;
            try {
                kmHoaDon = _khoTruyVan.LayKhuyenMaiHoaDon();
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lấy KM hóa đơn: " + ex.Message); // báo lỗi nếu có
            }

            // 4. Tính giảm giá theo hóa đơn (phần trăm trên base sau giảm sản phẩm)
            decimal baseForHoaDon = tongTien - tongTienGiamGia;
            decimal tongGiamGiaHoaDon = 0;
            if (kmHoaDon != null) { // nếu có KM hóa đơn áp dụng
                decimal phanTramGiam = kmHoaDon.GiaTri / 100;       // chuyển % -> hệ số
                tongGiamGiaHoaDon = baseForHoaDon * phanTramGiam;   // tổng giảm do hóa đơn
            }

            // 5. Tổng giảm = giảm theo SP + giảm theo hóa đơn => tính final total
            decimal tongTienGiaHD = tongTienGiamGia + tongGiamGiaHoaDon;
            decimal finalTotal = tongTien - tongTienGiaHD; // tổng cuối cùng khách phải trả

            // 6. Cập nhật labels trên UI (định dạng tiền)
            lblTienTruocGiam.Text = tongTien.ToString("N0") + " đ";
            lblGiamGia.Text = (-tongTienGiaHD).ToString("N0") + " đ";
            lblTongCong.Text = finalTotal.ToString("N0") + " đ";

            // Lưu giá trị để truyền sang form thanh toán nếu cần
            soTienGiam = tongTienGiaHD;
            tongGoc = tongTien;
        }


        // Lưu đơn hàng tạm lên DB: chuẩn hóa dữ liệu từ _gioHang sang DTO ChiTietGioHang và gọi KhoTruyVan.LuuDonHangTam
        private int ThucHienLuuTam() {
            if (_gioHang.LaySoLuongMon() == 0) { // nếu giỏ rỗng -> không lưu
                MessageBox.Show("Vui lòng thêm sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            // Lấy tổng tiền hiển thị từ label (đã format), chuyển về decimal
            string tongTienStr = lblTongCong.Text.Replace(" đ", "").Replace(".", "");
            decimal tongTien = decimal.Parse(tongTienStr, CultureInfo.InvariantCulture); // parse về decimal

            try {
                // Bước 1: Chuẩn bị dữ liệu theo kiểu ChiTietGioHang để repo lưu
                var gioHangChoDb = new List<ChiTietGioHang>();
                foreach (var item in _gioHang.LayTatCaMon()) {
                    gioHangChoDb.Add(new ChiTietGioHang {
                        MaSP = item.MaSp,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGiaGoc // Lưu giá gốc vào CSDL (theo business của bạn)
                    });
                }

                // Bước 2: Gọi KhoTruyVan để lưu (bao gồm logic trừ kho bên trong)
                int maDonHangMoi = _khoTruyVan.LuuDonHangTam(gioHangChoDb, tongTien, _currentMaNV, _currentMaKH);

                if (maDonHangMoi == -1) { // nếu lưu thất bại -> báo lỗi
                    MessageBox.Show("Lỗi khi lưu tạm đơn hàng. Vui lòng kiểm tra log.");
                }
                return maDonHangMoi; // trả về MaDH mới (hoặc -1 nếu lỗi)
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lưu tạm đơn hàng: " + ex.InnerException?.Message ?? ex.Message); // thông báo lỗi
                return -1;
            }
        }

        // Reset form sau khi lưu/thoát đơn: xóa giỏ hàng logic + UI, reset tìm khách, tải lại sản phẩm (vì kho thay đổi)
        private void ResetMainForm() {
            _gioHang.XoaTatCa();                // xóa toàn bộ món trong giỏ logic
            lvDonHang.Items.Clear();            // xóa UI ListView
            CapNhatTongTien();                  // cập nhật lại tổng (về 0)
            lblTenKH.Text = "Khách vãng lai";   // reset thông tin khách
            _currentMaKH = null;
            txtTimKiemKH.Text = "";             // xoá textbox tìm khách
            txtTimKiemSP.Text = "";             // xoá text tìm sản phẩm
            TaiSanPham(_currentMaLoai);         // tải lại sản phẩm (vì tồn kho có thể đã thay đổi)
            lvDonHang.SelectedItems.Clear();    // clear selection
            btnThem.Enabled = false;            // disable nút thêm khách
        }

        // Tìm khách hàng theo SĐT bằng KhoTruyVan và cập nhật UI (lblTenKH) + bật/tắt nút Thêm
        private void SearchKhachHangBySDT(string sdt) {
            try {
                var khachHang = _khoTruyVan.SearchKhachHangBySDT(sdt);

                if (khachHang != null) {
                    lblTenKH.Text = khachHang.TenKh;
                    _currentMaKH = khachHang.MaKh;
                    btnThem.Enabled = false; // không cần thêm mới
                }
                else { // không tìm thấy -> bật nút Thêm để tạo khách mới
                    lblTenKH.Text = "Không tìm thấy KH";
                    _currentMaKH = null;
                    btnThem.Enabled = true; // cho phép thêm khách mới
                }
            }
            catch (Exception ex) {
                // nếu lỗi khi tìm -> show message và reset trạng thái
                MessageBox.Show("Lỗi khi tìm khách hàng: " + ex.Message);
                lblTenKH.Text = "Lỗi khi tìm";
                _currentMaKH = null;
                btnThem.Enabled = false;
            }
        }

        #endregion

        // Paint event trống (placeholder)
        private void flpLoaiSP_Paint(object sender, PaintEventArgs e) {

        }
    }
}