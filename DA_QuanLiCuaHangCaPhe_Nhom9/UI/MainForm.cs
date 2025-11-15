using DA_QuanLiCuaHangCaPhe_Nhom9.Function;                 // import namespace chứa các dịch vụ nghiệp vụ chung
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main;   // import các lớp trong folder function_Main (GioHang, KhoTruyVanMainForm, DichVuDonHang,...)
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;                   // import các entity model EF Core (SanPham, DonHang, ...)
using global::System.Globalization;                         // import CultureInfo/formatting dùng khi parse/format số/ngày

namespace DA_QuanLiCuaHangCaPhe_Nhom9 { // namespace của project

    // MainForm: form chính của POS, xử lý UI và điều phối các dịch vụ nghiệp vụ
    public partial class MainForm : Form {
        // === KHAI BÁO CÁC BIẾN ===
        private int _currentMaNV = 3;               // id nhân viên hiện tại (được gán trong constructor)
        private int? _currentMaKH = null;           // id khách hàng hiện tại (nullable)
        private string _currentMaLoai = "TatCa";    // bộ lọc loại sản phẩm hiện tại
        private decimal tongGoc = 0;                // tổng tiền gốc chưa áp khuyến mãi (để truyền sang ThanhToan)
        private decimal soTienGiam = 0;             // tổng tiền đã giảm (dùng hiển thị và truyền)

        // Khai báo các lớp dịch vụ, kho, và giỏ hàng (tách logic khỏi UI)
        private readonly DichVuDonHang _dichVuDonHang;      // chứa logic giá/kiểm tra tồn
        private readonly KhoTruyVanMainForm _khoTruyVan;    // repo trả dữ liệu cho MainForm
        private readonly GioHang _gioHang;                  // quản lý trạng thái giỏ hàng ở client


        #region thông báo toast
        // Sự kiện nhận notification từ NotificationCenter (toàn cục)
        private void NotificationCenter_NotificationRaised(NotificationCenter.Notification n) {
            try {
                // Nếu notification là hóa đơn chưa thanh toán hoặc tồn kho thấp -> hiển thị toast
                if (n.Type == NotificationCenter.NotificationType.UnpaidInvoice || n.Type == NotificationCenter.NotificationType.LowStock) {
                    ShowToast(n.Message);
                }
            }
            catch { } // im lặng nếu lỗi trong xử lý notification để tránh crash UI
        }

        // Tạo và hiển thị toast nhỏ trên màn hình — chạy trên thread UI
        private void ShowToast(string message) {
            if (this.InvokeRequired) { // nếu gọi từ thread khác, marshal về UI thread
                this.BeginInvoke(new Action(() => ShowToast(message)));
                return;
            }
            Form toast = new Form();                        // tạo form tạm để hiển thị message
            toast.FormBorderStyle = FormBorderStyle.None;   // không border
            toast.StartPosition = FormStartPosition.Manual; // vị trí tự tính
            toast.BackColor = Color.FromArgb(45, 45, 48);   // nền tối
            toast.Size = new Size(350, 90);                 // kích thước cố định
            var ownerRect = this.Bounds;                    // lấy vị trí form chính
            var screen = Screen.FromControl(this).WorkingArea; // kích thước màn hình khả dụng
            var x = Math.Min(ownerRect.Right + 10, screen.Right - toast.Width - 10);                    // tính toạ độ X
            var y = Math.Min(ownerRect.Bottom - toast.Height - 10, screen.Bottom - toast.Height - 10);  // tính toạ độ Y
            toast.Location = new Point(x - toast.Width, y); // đặt toast bên phải form (nếu đủ chỗ)
            Label lbl = new Label();                        // label chứa text
            lbl.Text = message;                             // gán nội dung
            lbl.ForeColor = Color.White;                    // chữ màu trắng
            lbl.Dock = DockStyle.Fill;                      // chiếm toàn vùng form
            lbl.Padding = new Padding(8);                   // padding để dễ đọc
            toast.Controls.Add(lbl);                        // thêm label vào form
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer(); // timer để tự đóng toast
            t.Interval = 6000;                                  // thời gian hiển thị 6 giây
            t.Tick += (s, e) => { t.Stop(); toast.Close(); };   // khi hết thời gian -> đóng
            t.Start();          // khởi chạy timer
            toast.Show(this);   // hiển thị toast và đặt owner là MainForm
        }

        // Khi form đóng, hủy đăng ký event để tránh leak
        protected override void OnFormClosed(FormClosedEventArgs e) {
            base.OnFormClosed(e);
            NotificationCenter.NotificationRaised -= NotificationCenter_NotificationRaised; // unsubscribe
        }
        #endregion

        #region Hàm khởi tạo và tải form
        // Constructor nhận MaNV từ Loginform
        public MainForm(int MaNV) {
            InitializeComponent();          // phương thức auto-generated khởi tạo control
            _currentMaNV = MaNV;            // gán id nhân viên hiện tại

            // Khởi tạo các lớp dịch vụ / repository / domain
            _dichVuDonHang = new DichVuDonHang();           // dịch vụ xử lý giá/kiểm tra tồn
            _khoTruyVan = new KhoTruyVanMainForm();         // repo dữ liệu cho MainForm
            _gioHang = new GioHang(_dichVuDonHang);         // giỏ hàng nhận DichVuDonHang để kiểm tra tồn

            // Đăng ký lắng nghe notification toàn cục
            NotificationCenter.NotificationRaised += NotificationCenter_NotificationRaised;
        }

        // Sự kiện Load của form — cấu hình ListView giỏ hàng và tải dữ liệu ban đầu
        private void MainForm_Load(object sender, EventArgs e) {
            lvDonHang.View = View.Details;              // thiết lập ListView hiển thị dạng columns
            lvDonHang.Columns.Clear();                  // xóa cột cũ (nếu có)
            lvDonHang.Columns.Add("Tên SP", 200);       // cột tên sản phẩm
            lvDonHang.Columns.Add("SL", 40);            // cột số lượng
            lvDonHang.Columns.Add("Đơn Giá", 80);       // cột đơn giá
            lvDonHang.Columns.Add("Thành Tiền", 100);   // cột thành tiền

            TaiLoaiSanPham();                           // tải danh sách loại sản phẩm và render buttons
            TaiSanPham("TatCa");                        // tải tất cả sản phẩm (mặc định)

            this.btnThem.Enabled = false;               // nút thêm khách mặc định disable
            this.btnThem.Visible = true;                // đảm bảo nút hiển thị
        }
        #endregion

        #region Các hàm tải dữ liệu (Đã tách CSDL)

        // Tải danh sách loại sản phẩm từ repository và tạo nút cho từng loại
        private void TaiLoaiSanPham() {
            flpLoaiSP.Controls.Clear();                         // xóa controls cũ
            flpLoaiSP.FlowDirection = FlowDirection.TopDown;    // sắp xếp top->down
            try {
                var cacLoaiSP = _khoTruyVan.TaiLoaiSanPham();   // gọi repo lấy danh sách loại (List<string>)
                Button btnTatCa = new Button { Text = "Tất Cả", Tag = "TatCa", Width = flpLoaiSP.Width, Height = 45, Margin = new Padding(5), Font = new Font("Segoe UI", 9F, FontStyle.Bold), BackColor = Color.LightGray, }; // nút Tất cả
                btnTatCa.Click += BtnLoai_Click;                // đăng ký sự kiện click cho nút
                flpLoaiSP.Controls.Add(btnTatCa);               // thêm nút Tất cả vào flowpanel
                foreach (var tenLoai in cacLoaiSP) { // với mỗi loại trả về
                    Button btn = new Button { Text = tenLoai, Tag = tenLoai, Width = flpLoaiSP.Width, Height = 50, Margin = new Padding(5) }; // tạo nút loại
                    btn.Click += BtnLoai_Click;                 // đăng ký sự kiện click
                    flpLoaiSP.Controls.Add(btn);                // thêm nút vào flowpanel
                }
            }
            catch (Exception ex) { // nếu lỗi khi gọi repo
                MessageBox.Show("Lỗi khi tải loại sản phẩm: " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        // Tải sản phẩm và tạo nút cho từng sản phẩm, áp filter loại và search text
        private void TaiSanPham(string maLoai) {
            flpSanPham.Controls.Clear(); // xóa các nút cũ
            string searchText = txtTimKiemSP.Text.Trim().ToLower(); // lấy nội dung tìm kiếm, chuẩn hoá lowercase
            try {
                var duLieu = _khoTruyVan.LayDuLieuSanPham();        // lấy dữ liệu thô (SanPham, DinhLuong, NguyenLieu)
                var tatCaSanPham = duLieu.TatCaSanPham;             // danh sách SP đã filter "Còn bán"
                var allDinhLuong = duLieu.AllDinhLuong;             // danh sách định lượng (công thức)
                var allNguyenLieu = duLieu.AllNguyenLieu;           // danh sách nguyên liệu đang kinh doanh

                foreach (var sp in tatCaSanPham) {// lặp qua từng sản phẩm
                    if (maLoai != "TatCa" && sp.LoaiSp != maLoai) continue; // nếu đang lọc theo loại khác "TatCa", bỏ qua SP không thuộc loại
                    if (!string.IsNullOrEmpty(searchText) && !sp.TenSp.ToLower().Contains(searchText)) continue; // áp filter tìm kiếm theo tên

                    // Tạo nút hiển thị tên + giá, gán Tag = entity SanPham để dùng sau
                    Button btn = new Button { Text = $"{sp.TenSp}\n{sp.DonGia:N0} đ", Tag = sp, Width = 140, Height = 100, Margin = new Padding(5), BackColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.Black };
                    btn.FlatAppearance.BorderSize = 1; btn.FlatAppearance.BorderColor = Color.Gainsboro; // style đơn giản

                    // Kiểm tra trạng thái kho cho sản phẩm hiện tại bằng DichVuDonHang
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
                    btn.Click += BtnSanPham_Click; // đăng ký sự kiện click -> thêm món vào giỏ
                    flpSanPham.Controls.Add(btn); // thêm nút vào flowpanel
                }
            }
            catch (Exception ex) { // nếu lỗi khi lấy dữ liệu
                MessageBox.Show("Lỗi khi tải sản phẩm: " + ex.Message);
            }
        }

        #endregion

        #region Các hàm xử lý sự kiện (Event Handlers)

        // Khi bấm vào nút loại: chuyển bộ lọc và tải lại danh sách sản phẩm
        private void BtnLoai_Click(object sender, EventArgs e) {
            Button nutDuocBam = (Button)sender;             // cast sender về Button
            string maLoai = nutDuocBam.Tag.ToString();      // lấy tag (mã loại)
            _currentMaLoai = maLoai;                        // gán loại hiện tại
            TaiSanPham(maLoai);                             // tải lại sản phẩm theo loại
        }

        // Khi bấm vào nút sản phẩm: thêm 1 đơn vị món đó vào giỏ hàng
        private void BtnSanPham_Click(object sender, EventArgs e) {
            Button nutDuocBam = (Button)sender;             // button được bấm
            SanPham spDuocChon = (SanPham)nutDuocBam.Tag;   // lấy entity SanPham từ Tag

            // 1. Gọi logic giỏ hàng để thêm món (GioHang.ThemMon kiểm tra tồn kho)
            var ketQua = _gioHang.ThemMon(spDuocChon);

            // 2. Nếu thêm thành công -> cập nhật UI (listview, tổng)
            if (ketQua.Success) {
                CapNhatGiaoDienGioHang();                   // vẽ lại lvDonHang theo dữ liệu GioHang
                CapNhatTongTien();                          // tính lại các label tổng tiền
            }
            else {
                // Nếu thất bại (ví dụ nguyên liệu không đủ) -> hiển thị message
                MessageBox.Show(ketQua.Message, "Hết Hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Xử lý khi người dùng bấm Thanh Toán
        private void btnThanhToan_Click(object sender, EventArgs e) {
            if (_gioHang.LaySoLuongMon() > 0) {             // nếu giỏ có món
                int maDonHangVuaTao = ThucHienLuuTam();     // lưu tạm đơn vào DB và trừ kho -> trả về MaDH
                if (maDonHangVuaTao > 0) {                  // nếu lưu thành công
                    ThanhToan frmThanhToan = new ThanhToan(maDonHangVuaTao, tongGoc, soTienGiam); // mở form thanh toán với MaDH + tổng gốc/giảm
                    var result = frmThanhToan.ShowDialog(); // hiển thị modal
                    if ((result == DialogResult.OK) || (result == DialogResult.Cancel)) {
                        ResetMainForm(); // sau khi đóng form thanh toán -> reset main form (xóa giỏ, load lại sản phẩm)
                    }
                }
            }
            else { // giỏ hàng rỗng -> mở danh sách đơn chờ để chọn đơn cũ thanh toán
                ChonDonHangCho cdhc = new ChonDonHangCho();
                var resultChon = cdhc.ShowDialog();
                if (resultChon == DialogResult.OK) {
                    int maDonHangChon = cdhc.MaDonHangDaChon;   // lấy MaDH được chọn
                    ThanhToan thanhtoan = new ThanhToan(maDonHangChon, tongGoc, soTienGiam); // mở form ThanhToan cho đơn chọn
                    var resultThanhToan = thanhtoan.ShowDialog();
                    if ((resultThanhToan == DialogResult.OK) || (resultThanhToan == DialogResult.Cancel)) {
                        TaiSanPham(_currentMaLoai);             // reload sản phẩm (vì kho có thể đã thay đổi)
                    }
                }
                else if (resultChon == DialogResult.Cancel) TaiSanPham(_currentMaLoai); // nếu cancel -> reload sản phẩm
            }
        }

        // Hủy đơn hiện tại (UI) — thực chất xóa giỏ hàng local, không tác động DB
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

        // Xóa một món đã chọn khỏi giỏ: lấy MaSp từ Tag của ListViewItem và gọi GioHang.XoaMon
        private void btnXoaMon_Click(object sender, EventArgs e) {
            if (lvDonHang.SelectedItems.Count > 0) {
                ListViewItem itemDaChon = lvDonHang.SelectedItems[0]; // lấy item chọn
                int maSp = (int)itemDaChon.Tag; // Tag chứa MaSp

                _gioHang.XoaMon(maSp);          // gọi domain logic xóa món

                CapNhatGiaoDienGioHang();       // cập nhật UI
                CapNhatTongTien();              // cập nhật tổng tiền
            }
            else {
                MessageBox.Show("Vui lòng chọn một món ăn trong giỏ hàng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Giảm số lượng 1 đơn vị cho món được chọn; nếu giảm từ 1 xuống 0 -> hỏi xác nhận xóa
        private void btnGIamSoLuong_Click(object sender, EventArgs e) {
            if (lvDonHang.SelectedItems.Count > 0) {
                ListViewItem itemDaChon = lvDonHang.SelectedItems[0];
                int maSp = (int)itemDaChon.Tag;
                int soLuongHienTai = int.Parse(itemDaChon.SubItems[1].Text); // cột SL

                bool daXoaMon = false;

                if (soLuongHienTai > 1) {
                    _gioHang.GiamSoLuong(maSp); // giảm 1 đơn vị
                }
                else {
                    // nếu hiện tại =1, xác nhận xóa
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

                // nếu có thay đổi (giảm/xóa) -> cập nhật UI và tổng
                if (daXoaMon || soLuongHienTai > 1) {
                    CapNhatGiaoDienGioHang();
                    CapNhatTongTien();
                }
            }
            else {
                MessageBox.Show("Vui lòng chọn một món ăn trong giỏ hàng để giảm số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Khi thay đổi text tìm SP -> reload sản phẩm với bộ lọc hiện tại
        private void txtTimKiemSP_TextChanged(object sender, EventArgs e) {
            TaiSanPham(_currentMaLoai);
        }

        // Mở form thêm khách hàng mới (với SĐT mặc định từ textbox)
        private void btnThem_Click(object sender, EventArgs e) {
            ThemKhachHangMoi tmk = new ThemKhachHangMoi(txtTimKiemKH.Text.Trim()); // truyền SĐT sẵn có
            var result = tmk.ShowDialog();
            if (result == DialogResult.OK) {
                SearchKhachHangBySDT(txtTimKiemKH.Text.Trim()); // nếu thêm thành công -> tìm lại KH để bind id
            }
        }

        // Lưu tạm đơn (gọi ThucHienLuuTam) và thông báo MaDH vừa tạo
        private void btnLuuTam_Click(object sender, EventArgs e) {
            int maDonHangMoi = ThucHienLuuTam();
            if (maDonHangMoi > 0) {
                MessageBox.Show($"Đã lưu tạm đơn hàng {maDonHangMoi}", "Lưu tạm thành công");
                ResetMainForm(); // reset giao diện sau khi lưu
            }
        }

        // Chỉ cho nhập số trong textbox SĐT
        private void txtTimKiemKH_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) {
                e.Handled = true; // chặn ký tự không phải số
            }
        }

        // Khi thay đổi SĐT -> validate 10 số và gọi tìm khách
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

        // Cập nhật ListView lvDonHang từ dữ liệu trong _gioHang
        private void CapNhatGiaoDienGioHang() {
            lvDonHang.Items.Clear(); // xóa tất cả item hiện tại

            var dsMonAn = _gioHang.LayTatCaMon(); // lấy danh sách GioHangItem

            foreach (var item in dsMonAn) {
                ListViewItem lvi = new ListViewItem(item.TenSp); // cột 0: tên sản phẩm
                lvi.Tag = item.MaSp; // lưu MaSp vào Tag để thao tác sau
                lvi.SubItems.Add(item.SoLuong.ToString()); // cột 1: số lượng
                lvi.SubItems.Add(item.DonGiaGoc.ToString("N0")); // cột 2: đơn giá gốc (format N0)
                lvi.SubItems.Add(item.ThanhTienGoc.ToString("N0")); // cột 3: thành tiền gốc (format N0)

                lvDonHang.Items.Add(lvi); // thêm row vào ListView
            }
        }


        /*
        HÀM ThemSanPhamVaoDonHang() CŨ ĐÃ BỊ XÓA
        (Logic đã chuyển vào BtnSanPham_Click và GioHang.cs)
        */

        // Tính tổng tiền, áp khuyến mãi sản phẩm và hóa đơn, và cập nhật UI labels
        private void CapNhatTongTien() {
            decimal tongTien = _gioHang.LayTongTienGoc();       // tổng tiền gốc (chưa KM)
            decimal tongTienGiamGia = 0;                        // tổng tiền được giảm do KM sản phẩm

            var dsMonAn = _gioHang.LayTatCaMon();               // lấy danh sách món

            // Tính giảm theo từng sản phẩm bằng cách gọi DichVuDonHang.GetGiaBan
            foreach (var item in dsMonAn) {
                decimal donGiaMoi = _dichVuDonHang.GetGiaBan(item.MaSp, item.DonGiaGoc);        // giá sau KM cho SP
                decimal discountPerItem = item.DonGiaGoc - donGiaMoi;                           // chênh lệch = giảm trên mỗi đơn vị
                tongTienGiamGia += (discountPerItem * item.SoLuong);                            // nhân số lượng để cộng vào tổng giảm
            }

            // Tìm khuyến mãi loại HoaDon áp dụng cho hóa đơn (nếu có)
            KhuyenMai kmHoaDon = null;
            try {
                kmHoaDon = _khoTruyVan.LayKhuyenMaiHoaDon(); // gọi repo lấy KM HoaDon tốt nhất
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lấy KM hóa đơn: " + ex.Message); // báo lỗi nếu có
            }

            // Tính giảm theo hóa đơn: baseForHoaDon = tổng sau khi trừ giảm theo SP
            decimal baseForHoaDon = tongTien - tongTienGiamGia;
            decimal tongGiamGiaHoaDon = 0;
            if (kmHoaDon != null) { // nếu có KM hóa đơn áp dụng
                decimal phanTramGiam = kmHoaDon.GiaTri / 100;       // chuyển % -> hệ số
                tongGiamGiaHoaDon = baseForHoaDon * phanTramGiam;   // tổng giảm do hóa đơn
            }

            // Tổng giảm = giảm theo SP + giảm theo hóa đơn
            decimal tongTienGiaHD = tongTienGiamGia + tongGiamGiaHoaDon;
            decimal finalTotal = tongTien - tongTienGiaHD; // tổng cuối cùng khách phải trả

            // Cập nhật các label hiển thị trên UI (định dạng tiền)
            lblTienTruocGiam.Text = tongTien.ToString("N0") + " đ";     // tiền trước giảm
            lblGiamGia.Text = (-tongTienGiaHD).ToString("N0") + " đ";   // hiển thị dấu trừ chọn hiển thị (-xxx)
            lblTongCong.Text = finalTotal.ToString("N0") + " đ";        // thành tiền cuối cùng

            // Lưu lại các giá trị cho việc truyền xuống form ThanhToan nếu cần
            soTienGiam = tongTienGiaHD;
            tongGoc = tongTien;
        }

        // Lưu đơn hàng tạm lên DB: chuẩn hoá dữ liệu từ _gioHang và gọi KhoTruyVan.LuuDonHangTam
        private int ThucHienLuuTam() {
            if (_gioHang.LaySoLuongMon() == 0) { // nếu giỏ rỗng -> không lưu
                MessageBox.Show("Vui lòng thêm sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            // Lấy tổng tiền final từ label (format "xxx đ") -> remove " đ" và dấu chấm hàng nghìn
            string tongTienStr = lblTongCong.Text.Replace(" đ", "").Replace(".", "");
            decimal tongTien = decimal.Parse(tongTienStr, CultureInfo.InvariantCulture); // parse về decimal

            try {
                // Chuẩn bị danh sách ChiTietGioHang để gửi xuống KhoTruyVan
                var gioHangChoDb = new List<ChiTietGioHang>();
                foreach (var item in _gioHang.LayTatCaMon()) {
                    gioHangChoDb.Add(new ChiTietGioHang {
                        MaSP = item.MaSp,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGiaGoc // lưu giá gốc vào DB theo business
                    });
                }

                // Gọi KhoTruyVan để lưu đơn tạm: phương thức này thực hiện transaction và trừ kho
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

        // Reset lại toàn bộ UI và trạng thái giỏ hàng sau khi lưu/huỷ/thoát
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

        // Tìm khách theo SĐT bằng KhoTruyVan và cập nhật UI (lblTenKH) + bật/tắt nút Thêm
        private void SearchKhachHangBySDT(string sdt) {
            try {
                var khachHang = _khoTruyVan.SearchKhachHangBySDT(sdt); // gọi repo

                if (khachHang != null) { // nếu tìm thấy -> hiển thị tên và lưu MaKh
                    lblTenKH.Text = khachHang.TenKh;
                    _currentMaKH = khachHang.MaKh;
                    btnThem.Enabled = false; // không cần thêm mới
                }
                else { // không tìm thấy -> bật nút Thêm để tạo khách mới
                    lblTenKH.Text = "Không tìm thấy KH";
                    _currentMaKH = null;
                    btnThem.Enabled = true;
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

        // Paint event trống — placeholder nếu cần custom painting cho flpLoaiSP
        private void flpLoaiSP_Paint(object sender, PaintEventArgs e) {

        }
    }
}