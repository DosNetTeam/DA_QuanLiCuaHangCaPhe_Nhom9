using DA_QuanLiCuaHangCaPhe_Nhom9.Function;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {

    public partial class MainForm : Form
    {

        // Giả định ID nhân viên đang đăng nhập.
        private int _currentMaNV = 3;

        // Biến này sẽ lưu Mã Khách Hàng sau khi tìm thấy
        // Dấu ? có nghĩa là "nullable" (có thể rỗng,
        // rỗng = Khách vãng lai)
        private int? _currentMaKH = null;

        // Biến này lưu loại sản phẩm đang được chọn (ví dụ: "Cà phê")
        // Ban đầu mặc định là "Tất Cả"
        private string _currentMaLoai = "TatCa";

        // Biến lưu tổng gốc và số tiền giảm (dùng cho thanh toán)
        private decimal tongGoc = 0;
        private decimal soTienGiam = 0;



        #region thông báo toast
        private void NotificationCenter_NotificationRaised(NotificationCenter.Notification n)
        {
            try
            {
                // Only show unpaid invoice and low stock in main form
                if (n.Type == NotificationCenter.NotificationType.UnpaidInvoice || n.Type == NotificationCenter.NotificationType.LowStock)
                {
                    ShowToast(n.Message);
                }
            }
            catch { }
        }

        private void ShowToast(string message)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ShowToast(message)));
                return;
            }

            Form toast = new Form();
            toast.FormBorderStyle = FormBorderStyle.None;
            toast.StartPosition = FormStartPosition.Manual;
            toast.BackColor = Color.FromArgb(45, 45, 48);
            toast.Size = new Size(350, 90);

            // position bottom-right of owner form
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            NotificationCenter.NotificationRaised -= NotificationCenter_NotificationRaised;
        }
        #endregion

        #region Hàm khởi tạo và tải form
        public MainForm(int MaNV)
        {
            InitializeComponent();
            _currentMaNV = MaNV;

            // subscribe to notifications
            NotificationCenter.NotificationRaised += NotificationCenter_NotificationRaised;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Cấu hình ListView
            lvDonHang.View = View.Details;
            lvDonHang.Columns.Clear();
            lvDonHang.Columns.Add("Tên SP", 200);
            lvDonHang.Columns.Add("SL", 40);
            lvDonHang.Columns.Add("Đơn Giá", 80);
            lvDonHang.Columns.Add("Thành Tiền", 100);

            // Tải dữ liệu từ CSDL
            TaiLoaiSanPham();
            TaiSanPham("TatCa");

            // Ban đầu hiển thị nhưng không cho tương tác (disabled)
            this.btnThem.Enabled = false;
            this.btnThem.Visible = true;
        }
        #endregion

        #region Các hàm tải dữ liệu (Load Data - Dùng EF Core)
        // Tải các nút Loại Sản Phẩm (từ CSDL)
        private void TaiLoaiSanPham()
        {
            flpLoaiSP.Controls.Clear();
            flpLoaiSP.FlowDirection = FlowDirection.TopDown;

            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    var cacLoaiSP = db.SanPhams
                                     .Select(sp => sp.LoaiSp)
                                     .Where(loai => loai != null && loai != "")
                                     .Distinct()
                                     .ToList();

                    // 1. Tạo nút "Tất Cả"
                    Button btnTatCa = new Button
                    {
                        Text = "Tất Cả",
                        Tag = "TatCa",
                        Width = flpLoaiSP.Width,//- 30
                        Height = 45,
                        Margin = new Padding(5),
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        BackColor = Color.LightGray,
                        

                    };

                    btnTatCa.Click += BtnLoai_Click;
                    flpLoaiSP.Controls.Add(btnTatCa);

                    // 2. Tạo các nút cho từng loại
                    foreach (var tenLoai in cacLoaiSP)
                    {
                        Button btn = new Button
                        {
                            Text = tenLoai,
                            Tag = tenLoai,
                            Width = flpLoaiSP.Width,//- 30
                            Height = 50,
                            Margin = new Padding(5)
                        };
                        btn.Click += BtnLoai_Click;
                        flpLoaiSP.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải loại sản phẩm: " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        // Tải các nút Sản Phẩm (từ CSDL)
        private void TaiSanPham(string maLoai)
        {
            flpSanPham.Controls.Clear();

            //Lấy text tìm kiếm ---
            string searchText = txtTimKiemSP.Text.Trim().ToLower();
            #region code cũ
            /*
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Biến 'query' này sẽ lưu câu lệnh truy vấn
                    IQueryable<SanPham> query;

                    // Gán truy vấn ban đầu là lấy tất cả sản phẩm
                    query = db.SanPhams;

                    // Nếu 'maLoai' *không* phải là "TatCa"
                    if (maLoai != "TatCa") {
                        // thì chúng ta thêm một điều kiện 'Where' vào câu truy vấn
                        query = query.Where(sp => sp.LoaiSp == maLoai);
                    }

                    //Lọc theo Tên SP ---
                    // Nếu ô tìm kiếm không rỗng
                    if (!string.IsNullOrEmpty(searchText)) {
                        // Thêm điều kiện Where: Tên SP (chuyển chữ thường)
                        // phải chứa (Contains) chữ tìm kiếm
                        query = query.Where(sp =>
                            sp.TenSp.ToLower().Contains(searchText)
                        );
                    }

                    // Thêm một điều kiện 'Where' nữa: chỉ lấy sp "Còn bán"
                    var spCanHienThi = query.Where(sp => sp.TrangThai == "Còn bán").ToList();

                    // Chúng ta cần lấy công thức và nguyên liệu
                    // ra 2 danh sách TẠM
                    var allDinhLuong = db.DinhLuongs.ToList();

                    var allNguyenLieu = db.NguyenLieus.Where(nl => nl.TrangThai == "Đang kinh doanh").ToList();

                    // Tạo các nút sản phẩm
                    foreach (var sp in spCanHienThi) {
                        Button btn = new Button {
                            Text = $"{sp.TenSp}\n{sp.DonGia:N0} đ",
                            Tag = sp,
                            Width = 140, // từ 120 t cho lên 140
                            Height = 100,
                            Margin = new Padding(5),
                            // BackColor = Color.FromArgb(255, 240, 200)

                            // --- CÁC THUỘC TÍNH STYLE MỚI ---
                            BackColor = Color.White, // Nền nút màu trắng
                            FlatStyle = FlatStyle.Flat, // Kiểu phẳng
                            Font = new Font("Segoe UI", 9F, FontStyle.Bold), // Chữ đậm
                            ForeColor = Color.Black // Chữ màu đen

                        };

                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.Gainsboro; // Màu viền xám nhạt

                        // Hàm này trả về 1 trong 3 chữ:
                        // "DU_HANG", "SAP_HET", "HET_HANG"
                        //string trangThaiKho = KiemTraDuNguyenLieu(sp.MaSp);

                        // (Hàm này giờ dùng 2 List tạm, không gọi CSDL)
                        string trangThaiKho = KiemTraDuNguyenLieu(sp.MaSp, allDinhLuong, allNguyenLieu);

                        // Dùng switch case (hoặc if/else) để xử lý 3 trường hợp
                        switch (trangThaiKho) {
                            case "DU_HANG":
                                // Đủ hàng: Nút trắng, cho bấm
                                btn.Enabled = true;
                                btn.BackColor = Color.White;
                                btn.ForeColor = Color.Black;
                                break;

                            case "SAP_HET":
                                // Sắp hết: Nút CAM, vẫn cho bấm
                                btn.Enabled = true;
                                btn.BackColor = Color.Orange; // Màu cam
                                btn.ForeColor = Color.White; // Chữ trắng
                                btn.Text += "\n(Sắp hết)";
                                break;

                            case "HET_HANG":
                            default:
                                // Hết hàng: Nút XÁM, không cho bấm
                                btn.Enabled = false;
                                btn.BackColor = Color.LightGray;
                                btn.ForeColor = Color.Gray; // Chữ mờ
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
            */
            #endregion


            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {

                    // --- BƯỚC 1: LẤY HẾT DỮ LIỆU CẦN THIẾT TỪ CSDL ---

                    // 1. Lấy TẤT CẢ sản phẩm "Còn bán" về máy (giống cách bạn làm)
                    var tatCaSanPham = db.SanPhams.Where(sp => sp.TrangThai == "Còn bán").ToList();

                    // 2. Lấy TẤT CẢ công thức (DinhLuong)
                    var allDinhLuong = db.DinhLuongs.ToList();

                    // 3. Lấy TẤT CẢ nguyên liệu "Đang kinh doanh"
                    var allNguyenLieu = db.NguyenLieus.Where(nl => nl.TrangThai == "Đang kinh doanh").ToList();


                    // --- BƯỚC 2: LỌC VÀ TẠO NÚT (DÙNG FOREACH VÀ IF) ---

                    // Lặp qua danh sách sản phẩm TẤT CẢ mà chúng ta vừa tải về
                    foreach (var sp in tatCaSanPham)
                    {

                        // --- Bắt đầu kiểm tra điều kiện lọc ---

                        // 1. Lọc theo Loại (maLoai)
                        // Nếu người dùng CÓ CHỌN loại (không phải "TatCa") VÀ
                        // LoaiSp của sản phẩm này KHÔNG KHỚP với loại người dùng chọn
                        // thì BỎ QUA (continue) sản phẩm này.
                        if (maLoai != "TatCa" && sp.LoaiSp != maLoai)
                        {
                            continue; // Đi đến sản phẩm tiếp theo trong vòng lặp
                        }

                        // 2. Lọc theo Tên (searchText)
                        // Nếu người dùng CÓ GÕ tìm kiếm (không rỗng) VÀ
                        // Tên SP (chữ thường) KHÔNG CHỨA chữ người dùng gõ
                        // thì BỎ QUA (continue) sản phẩm này.
                        if (!string.IsNullOrEmpty(searchText) && !sp.TenSp.ToLower().Contains(searchText))
                        {
                            continue; // Đi đến sản phẩm tiếp theo trong vòng lặp
                        }

                        // --- Kết thúc kiểm tra điều kiện ---

                        // Nếu code chạy được đến đây, nghĩa là sản phẩm này THỎA MÃN
                        // cả 2 điều kiện lọc. Giờ chúng ta tạo nút cho nó:

                        Button btn = new Button
                        {
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

                        // Kiểm tra tồn kho (giữ nguyên logic cũ)
                        string trangThaiKho = KiemTraDuNguyenLieu(sp.MaSp, allDinhLuong, allNguyenLieu);

                        switch (trangThaiKho)
                        {
                            case "DU_HANG":
                                btn.Enabled = true;
                                btn.BackColor = Color.White;
                                btn.ForeColor = Color.Black;
                                break;

                            case "SAP_HET":
                                btn.Enabled = true;
                                btn.BackColor = Color.Orange;
                                btn.ForeColor = Color.White;
                                btn.Text += "\n(Sắp hết)";
                                break;

                            case "HET_HANG":
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải sản phẩm: " + ex.Message);
            }
        }

        /*
        private decimal GetGiaBan(int maSP, decimal giaGoc) {
            try {
                // 1. Mở kết nối CSDL
                using (DataSqlContext db = new DataSqlContext()) {
                    // 2. Tìm đúng sản phẩm và YÊU CẦU EF TẢI KÈM
                    //    danh sách Khuyến mãi của nó (dùng .Include)
                    var sanPham = db.SanPhams
                                  .Include(sp => sp.MaKms) // Tải kèm danh sách KM
                                  .FirstOrDefault(sp => sp.MaSp == maSP); // Tìm SP theo ID

                    // Nếu không tìm thấy SP (ví dụ lỗi)
                    if (sanPham == null) {
                        return giaGoc; // Trả về giá gốc
                    }

                    // 3. Lấy ngày hôm nay (vì CSDL dùng DateOnly)
                    DateOnly now = DateOnly.FromDateTime(DateTime.Now);

                    KhuyenMai bestPromotion = null; // Biến lưu KM 'SanPham' tốt nhất

                    // 4. Lặp qua danh sách KM của sản phẩm đó
                    //    (sanPham.MaKms là danh sách KM mà .Include đã tải)
                    foreach (var km in sanPham.MaKms) // 
                    {
                        // 5. Kiểm tra xem KM này có hợp lệ không
                        if (km.LoaiKm == "SanPham" &&
                            km.TrangThai == "Đang áp dụng" &&
                            km.NgayBatDau <= now &&
                            km.NgayKetThuc >= now) {
                            // 6. So sánh tìm cái tốt nhất (giảm nhiều nhất)
                            if (bestPromotion == null || km.GiaTri > bestPromotion.GiaTri) {
                                bestPromotion = km;
                            }
                        }
                    }

                    // 7. Tính giá cuối cùng
                    if (bestPromotion != null) {
                        // Nếu tìm thấy KM, tính giá đã giảm
                        decimal phanTramGiam = bestPromotion.GiaTri / 100;
                        return giaGoc - (giaGoc * phanTramGiam);
                    }

                    // Không có KM nào hợp lệ, trả về giá gốc
                    return giaGoc;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lấy giá khuyến mãi: " + ex.Message);
                return giaGoc; // Nếu lỗi, trả về giá gốc
            }
        }
        */

        /// <summary>
        /// Lấy giá bán cuối cùng của 1 sản phẩm (đã trừ KM 'SanPham' nếu có)
        /// </summary>
        /// <param name="maSanPham">Mã sản phẩm cần kiểm tra</param>
        /// <param name="giaGoc">Giá gốc (chưa giảm) của sản phẩm</param>
        /// <returns>Giá cuối cùng sau khi đã áp dụng KM</returns>
        private decimal GetGiaBan(int maSanPham, decimal giaGoc)
        {
            try
            {
                // Mở kết nối CSDL
                using (DataSqlContext db = new DataSqlContext())
                {
                    // --- BƯỚC 1: LẤY HẾT DỮ LIỆU CẦN THIẾT TỪ CSDL ---

                    // 1. Lấy ngày hôm nay để so sánh
                    DateOnly homNay = DateOnly.FromDateTime(DateTime.Now);

                    // 2. Lấy TẤT CẢ các khuyến mãi 'SanPham' đang hoạt động
                    //    (Chúng ta tạo một danh sách mới để lưu kết quả lọc)
                    var dsKMDangChay_SP = new List<KhuyenMai>();

                    // Tải TẤT CẢ khuyến mãi từ CSDL về máy
                    foreach (var km in db.KhuyenMais.ToList())
                    {

                        // Lọc thủ công: Chỉ lấy KM "SanPham" VÀ "Đang áp dụng" VÀ còn hạn
                        if (km.LoaiKm == "SanPham" &&
                            km.TrangThai == "Đang áp dụng" &&
                            km.NgayBatDau <= homNay &&
                            km.NgayKetThuc >= homNay)
                        {
                            // Nếu thỏa mãn thì thêm vào danh sách "đang chạy"
                            dsKMDangChay_SP.Add(km);
                        }
                    }

                    // Tối ưu: Nếu không có KM sản phẩm nào đang chạy, trả về giá gốc ngay
                    if (dsKMDangChay_SP.Count == 0)
                    {
                        return giaGoc;
                    }

                    // 3. Tải TẤT CẢ sản phẩm và "liên kết" khuyến mãi của chúng
                    // (Hàm .Include() là BẮT BUỘC vì CSDL của bạn "giấu" bảng KhuyenMai_SanPham)
                    // (Nó giúp EF Core tải kèm danh sách "liên kết" KM (sp.MaKms) cho mỗi sản phẩm)
                    var dsSanPham_va_KM = db.SanPhams.Include(sp => sp.MaKms).ToList();

                    // --- BƯỚC 2: LỌC BẰNG VÒNG LẶP (FOREACH VÀ IF) ---

                    // 4. Tìm sản phẩm ta cần (maSanPham) trong danh sách vừa tải về
                    SanPham sanPhamCuaToi = null;

                    // Lặp qua TẤT CẢ sản phẩm
                    foreach (var sp in dsSanPham_va_KM)
                    {
                        if (sp.MaSp == maSanPham)
                        {
                            sanPhamCuaToi = sp; // Tìm thấy
                            break; // Thoát vòng lặp
                        }
                    }

                    // Nếu không tìm thấy (lỗi hiếm gặp), trả về giá gốc
                    if (sanPhamCuaToi == null)
                    {
                        return giaGoc;
                    }

                    // 5. Tìm khuyến mãi tốt nhất (giảm nhiều nhất) cho sản phẩm này
                    KhuyenMai kmTotNhat = null;

                    // Lặp qua các "liên kết" KM CỦA RIÊNG sản phẩm này (sanPhamCuaToi.MaKms)
                    foreach (var kmCuaSP in sanPhamCuaToi.MaKms)
                    {
                        // Lặp qua danh sách KM "đang chạy" mà ta đã lọc ở Bước 1
                        foreach (var kmDangChay in dsKMDangChay_SP)
                        {
                            // Nếu KM của sản phẩm này khớp với một KM đang chạy
                            if (kmCuaSP.MaKm == kmDangChay.MaKm)
                            {
                                // So sánh: 
                                // 1. Nếu chưa có KM nào (kmTotNhat == null), gán nó.
                                // 2. Nếu KM đang chạy giảm nhiều hơn (GiaTri >), gán đè.
                                if (kmTotNhat == null || kmDangChay.GiaTri > kmTotNhat.GiaTri)
                                {
                                    kmTotNhat = kmDangChay;
                                }

                                // Đã tìm thấy, không cần lặp dsKMDangChay_SP nữa
                                break;
                            }
                        }
                    } // Kết thúc vòng lặp tìm KM

                    // --- BƯỚC 3: TÍNH GIÁ CUỐI CÙNG ---

                    // 6. Nếu chúng ta tìm được một KM tốt nhất
                    if (kmTotNhat != null)
                    {
                        // Tính toán % giảm
                        decimal phanTramGiam = kmTotNhat.GiaTri / 100;
                        // Trả về giá đã chiết khấu
                        return giaGoc - (giaGoc * phanTramGiam);
                    }

                    // 7. Không tìm thấy KM nào, trả về giá gốc
                    return giaGoc;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy giá khuyến mãi: " + ex.Message);
                return giaGoc; // Nếu lỗi, trả về giá gốc
            }
        }

        #endregion

        #region Các hàm xử lý sự kiện (Event Handlers)

        // Hàm này được gọi khi bấm vào nút Loại SP (Tất cả, Cà phê, Trà...)
        private void BtnLoai_Click(object sender, EventArgs e)
        {
            // 'sender' là cái nút vừa được bấm
            Button nutDuocBam = (Button)sender;

            // Lấy 'Tag' (chữ "TatCa" hoặc "Cà phê") mà chúng ta đã gán
            string maLoai = nutDuocBam.Tag.ToString();

            _currentMaLoai = maLoai; // Lưu lại loại SP vừa chọn

            // Gọi hàm tải sản phẩm với mã loại vừa lấy
            TaiSanPham(maLoai);
        }

        // Hàm này được gọi khi bấm vào nút Sản Phẩm (Cà phê sữa...)
        private void BtnSanPham_Click(object sender, EventArgs e)
        {
            // 'sender' là cái nút vừa được bấm
            Button nutDuocBam = (Button)sender;

            // Lấy 'Tag' (là đối tượng 'SanPham' đầy đủ) mà chúng ta đã gán
            SanPham spDuocChon = (SanPham)nutDuocBam.Tag;

            // Gọi hàm thêm sản phẩm đó vào giỏ hàng
            ThemSanPhamVaoDonHang(spDuocChon);
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            #region code cũ lưu đơn hàng trực tiếp trong MainForm (bây giờ chuyển sang form ThanhToan) - code thời tiền sử
            //try
            //{

            //    using (DataSqlContext db = new DataSqlContext())
            //    // -----------------------------------------------------------------
            //    {
            //        // Bước 1: Tạo đối tượng DonHang
            //        var donHangMoi = new DonHang
            //        {
            //            NgayLap = DateTime.Now,
            //            MaNv = _currentMaNV, // Dùng MaNv = 3
            //            TrangThai = "Dang xu ly"
            //        };

            //        decimal tongTien = 0;

            //        // Bước 2: Tạo danh sách các ChiTietDonHang
            //        var listChiTiet = new List<ChiTietDonHang>();

            //        // Lặp qua từng dòng trong giỏ hàng (ListView)
            //        foreach (ListViewItem item in lvDonHang.Items)
            //        {
            //            int maSP = (int)item.Tag;
            //            int soLuong = int.Parse(item.SubItems[1].Text);
            //            decimal donGia = decimal.Parse(item.SubItems[2].Text.Replace(".", ""), CultureInfo.InvariantCulture);

            //            // Tạo một đối tượng ChiTietDonHang
            //            var chiTiet = new ChiTietDonHang
            //            {
            //                // Gán chi tiết này vào đơn hàng mẹ
            //                MaDhNavigation = donHangMoi,
            //                MaSp = maSP,
            //                SoLuong = soLuong,
            //                DonGia = donGia
            //            };
            //            // Thêm vào danh sách tạm
            //            listChiTiet.Add(chiTiet);

            //            // Cộng dồn tổng tiền
            //            tongTien += (soLuong * donGia);
            //        }

            //        // Bước 3: Cập nhật tổng tiền cho DonHang
            //        donHangMoi.TongTien = tongTien;

            //        // Bước 4: Báo cho EF Core biết chúng ta muốn...
            //        db.DonHangs.Add(donHangMoi); // ...thêm 1 DonHang mới
            //        db.ChiTietDonHangs.AddRange(listChiTiet); // ...thêm NHIỀU ChiTietDonHang mới

            //        // Bước 5: Thực thi lệnh, lưu vào CSDL
            //        db.SaveChanges();

            //        MessageBox.Show($"Đã lưu đơn hàng {donHangMoi.MaDh} thành công!", "Thông báo");

            //        // Bước 6: Xóa giỏ hàng trên UI
            //        lvDonHang.Items.Clear();
            //        CapNhatTongTien();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Lỗi khi lưu đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
            //}

            #endregion

            #region code thanh toán v2 - chuyển sang thanh toán chờ
            /*
            // Kiểm tra xem có hàng trong giỏ chưa
            if (lvDonHang.Items.Count == 0) {
                MessageBox.Show("Vui lòng thêm sản phẩm vào đơn hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Thoát hàm
            }

            // 2. Lấy tổng tiền (Phải Parse đúng cách)
            string tongTienStr = lblTongCong.Text.Replace(" đ", "").Replace(".", "");
            decimal tongTien = decimal.Parse(tongTienStr, CultureInfo.InvariantCulture);


            ThanhToan frmThanhToan = new ThanhToan(lvDonHang.Items, decimal.Parse(lblTongCong.Text.Replace(" đ", "").Replace(".", ""), CultureInfo.InvariantCulture), _currentMaNV, _currentMaKH);


            // 4. HIỂN THỊ và LẮNG NGHE TÍN HIỆU TRẢ VỀ
            var result = frmThanhToan.ShowDialog();

            // 5. KIỂM TRA TÍN HIỆU
            if (result == DialogResult.OK) {
                // Nếu form Thanh Toán báo "OK" (đã lưu CSDL thành công)
                // thì chúng ta "Làm mới hóa đơn" (như bạn muốn)
                lvDonHang.Items.Clear(); // Xóa giỏ hàng
                CapNhatTongTien(); // Cập nhật tổng tiền (về 0)

                TaiSanPham(_currentMaLoai);

            }
            // Nếu result == DialogResult.Cancel (người dùng bấm "Hủy" hoặc nút X), 
            // thì không làm gì cả, giỏ hàng vẫn còn đó.
            */
            #endregion

            #region code v3 - chọn đơn hàng chờ 
            /*
            ChonDonHangCho cdhc = new ChonDonHangCho();
            cdhc.ShowDialog();

            TaiSanPham(_currentMaLoai); */
            #endregion

            // KIỂM TRA: Giỏ hàng có món không?
            if (lvDonHang.Items.Count > 0)
            {
                // TRƯỜNG HỢP 1: CÓ MÓN (THANH TOÁN NHANH)
                // 1. Tự động "Lưu Tạm" (Lưu CSDL, Trừ Kho, Tạo ThanhToan "Chưa TT")
                int maDonHangVuaTao = ThucHienLuuTam();

                // 2. Kiểm tra xem Lưu Tạm có thành công không
                if (maDonHangVuaTao > 0)
                {
                    // 3. Mở thẳng form Thanh Toán
                    ThanhToan frmThanhToan = new ThanhToan(maDonHangVuaTao, tongGoc, soTienGiam);
                    var result = frmThanhToan.ShowDialog();

                    // 4. Reset MainForm nếu thanh toán OK
                    // (Nếu Cancel thì đơn hàng đó vẫn nằm trong "Đơn chờ")
                    if ((result == DialogResult.OK) || (result == DialogResult.Cancel))
                    {
                        ResetMainForm();
                    }
                }
                // (Nếu Lưu Tạm lỗi (return -1), hàm ThucHienLuuTam đã tự báo lỗi rồi)
            }
            else
            {
                // TRƯỜNG HỢP 2: GIỎ HÀNG RỖNG (THANH TOÁN ĐƠN CŨ)
                ChonDonHangCho cdhc = new ChonDonHangCho();
                var resultChon = cdhc.ShowDialog();

                if (resultChon == DialogResult.OK)
                {
                    // Lấy MaDH từ form trung gian
                    int maDonHangChon = cdhc.MaDonHangDaChon;

                    // Mở form ThanhToan
                    ThanhToan thanhtoan = new ThanhToan(maDonHangChon, tongGoc, soTienGiam);
                    var resultThanhToan = thanhtoan.ShowDialog();

                    // Nếu thanh toán thành công, tải lại DS sản phẩm
                    // (Vì kho của các đơn khác có thể đã thay đổi)
                    if (resultThanhToan == DialogResult.OK)
                    {
                        TaiSanPham(_currentMaLoai);
                    }
                }
            }


        }

        // Hàm này được gọi khi bấm nút "Hủy Đơn"
        private void btnHuyDon_Click(object sender, EventArgs e)
        {
            // Hỏi xác nhận
            var confirm = MessageBox.Show("Bạn có chắc muốn hủy đơn hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng bấm "Yes"
            if (confirm == DialogResult.Yes)
            {
                // Xóa sạch giỏ hàng
                lvDonHang.Items.Clear();
                // Cập nhật tổng tiền về 0
                CapNhatTongTien();

                txtTimKiemKH.Clear();
                txtTimKiemSP.Clear();
                lblTenKH.ResetText();
            }
        }

        private void btnXoaMon_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra xem người dùng đã chọn món nào chưa
            // (ListView cho phép chọn nhiều, nhưng ta chỉ xử lý 1)
            if (lvDonHang.SelectedItems.Count > 0)
            {
                // Bước 2: Lấy món ăn đang được chọn
                // (SelectedItems[0] là món đầu tiên trong danh sách được chọn)
                ListViewItem itemDaChon = lvDonHang.SelectedItems[0];

                // Bước 3: Xóa món ăn đó khỏi giỏ hàng (ListView)
                lvDonHang.Items.Remove(itemDaChon);

                // Bước 4 (Rất quan trọng): Cập nhật lại tổng tiền
                CapNhatTongTien();
            }
            else
            {
                // Nếu không chọn gì mà bấm xóa, thì thông báo
                MessageBox.Show("Vui lòng chọn một món ăn trong giỏ hàng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnGIamSoLuong_Click(object sender, EventArgs e)
        {
            // Bước 1: Kiểm tra xem người dùng đã chọn món nào chưa
            if (lvDonHang.SelectedItems.Count > 0)
            {
                // Bước 2: Lấy món ăn đang được chọn
                ListViewItem itemDaChon = lvDonHang.SelectedItems[0];

                // Bước 3: Lấy số lượng hiện tại (từ cột 1)
                int soLuongHienTai = int.Parse(itemDaChon.SubItems[1].Text);

                // Biến này để theo dõi xem chúng ta có cần cập nhật tổng tiền không
                bool daThayDoi = false;

                // Bước 4: Xử lý logic
                if (soLuongHienTai > 1)
                {

                    // ----- TRƯỜNG HỢP 1: SỐ LƯỢNG > 1 (Ví dụ: 3 -> 2) -----
                    int soLuongMoi = soLuongHienTai - 1;

                    // Lấy đơn giá (từ cột 2) và tính lại
                    string donGiaStr = itemDaChon.SubItems[2].Text.Replace(".", "");
                    decimal donGia = decimal.Parse(donGiaStr, System.Globalization.CultureInfo.InvariantCulture);
                    decimal thanhTienMoi = soLuongMoi * donGia;

                    // Cập nhật lại giỏ hàng
                    itemDaChon.SubItems[1].Text = soLuongMoi.ToString();
                    itemDaChon.SubItems[3].Text = thanhTienMoi.ToString("N0");

                    daThayDoi = true; // Đánh dấu là đã thay đổi
                }
                else
                {
                    // ----- TRƯỜNG HỢP 2: SỐ LƯỢNG = 1 (Giảm nữa là xóa) -----
                    // Hỏi người dùng cho chắc
                    var confirm = MessageBox.Show(
                        "Số lượng món này là 1. Giảm nữa sẽ xóa món này khỏi giỏ hàng. Bạn có chắc không?",
                        "Xác nhận xóa món",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes)
                    {
                        // Nếu người dùng đồng ý, xóa món ăn
                        lvDonHang.Items.Remove(itemDaChon);
                        daThayDoi = true; // Đánh dấu là đã thay đổi
                    }
                }

                // Bước 5 (Rất quan trọng):
                // Nếu đã có thay đổi (hoặc đã giảm, hoặc đã xóa) thì mới cập nhật tổng tiền
                if (daThayDoi)
                {
                    CapNhatTongTien();
                }
            }
            else
            {
                // Nếu không chọn gì mà bấm giảm
                MessageBox.Show("Vui lòng chọn một món ăn trong giỏ hàng để giảm số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtTimKiemSP_TextChanged(object sender, EventArgs e)
        {
            // Gọi lại hàm TaiSanPham với loại SP ta đang chọn
            TaiSanPham(_currentMaLoai);
        }


        #region nút tìm KH - nhưng đã bỏ vì cơ chế quá rờm rà mất thười gian
        private void btnTimKH_Click(object sender, EventArgs e)
        {
            // Lấy SĐT từ TextBox, xóa khoảng trắng
            string sdt = txtTimKiemKH.Text.Trim();

            // Nếu không gõ gì, trả về khách vãng lai
            if (string.IsNullOrEmpty(sdt))
            {
                lblTenKH.Text = "Khách vãng lai";
                _currentMaKH = null;
                return;
            }

            try
            {
                // 1. "Gọi" Database
                using (DataSqlContext db = new DataSqlContext())
                {
                    // 2. Tìm khách hàng có SĐT khớp
                    // .FirstOrDefault() sẽ trả về null nếu không tìm thấy
                    var khachHang = db.KhachHangs
                                      .FirstOrDefault(kh => kh.SoDienThoai == sdt);

                    // 3. Xử lý kết quả
                    if (khachHang != null)
                    {
                        // TÌM THẤY
                        lblTenKH.Text = khachHang.TenKh;
                        _currentMaKH = khachHang.MaKh; // Lưu lại MaKH!
                    }
                    else
                    {
                        // KHÔNG TÌM THẤY
                        lblTenKH.Text = "Không tìm thấy KH";
                        _currentMaKH = null;

                        this.btnThem.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm khách hàng: " + ex.Message);
            }
        }
        #endregion

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Mở form thêm, truyền SĐT hiện tại
            ThemKhachHangMoi tmk = new ThemKhachHangMoi(txtTimKiemKH.Text.Trim());
            var result = tmk.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Sau khi thêm thành công, tự động tìm lại khách hàng (cập nhật tên + disable nút)
                SearchKhachHangBySDT(txtTimKiemKH.Text.Trim());
            }
        }

        private void btnLuuTam_Click(object sender, EventArgs e)
        {
            #region đươn giản hoá =))) chuyển sanng nút tạm lưu tạm trong form thanh toán
            /*
            // 1. Kiểm tra giỏ hàng
            if (lvDonHang.Items.Count == 0) {
                MessageBox.Show("Vui lòng thêm sản phẩm vào đơn hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. LƯU CSDL (TẠM) VÀ TRỪ KHO
            // (Code C# cơ bản, không tối ưu)
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Bước 2.1: Tạo đối tượng DonHang
                    var donHangMoi = new DonHang {
                        NgayLap = DateTime.Now,
                        MaNv = _currentMaNV,
                        TrangThai = "Đang xử Lý", // TRẠNG THÁI QUAN TRỌNG
                        MaKh = _currentMaKH // Lưu khách hàng
                    };

                    // Bước 2.2: Tạo danh sách các ChiTietDonHang
                    // (Bắt buộc phải dùng List<> cho EF Core)
                    var listChiTiet = new List<ChiTietDonHang>();
                    decimal tongTien = 0; // Tính tổng tiền ở đây

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
                        tongTien += (soLuong * donGia); // Cộng tổng tiền
                    }

                    donHangMoi.TongTien = tongTien; // Gán tổng tiền

                    // Bước 2.3: Báo cho EF Core
                    db.DonHangs.Add(donHangMoi);
                    db.ChiTietDonHangs.AddRange(listChiTiet);

                    var thanhToanMoi = new Models.ThanhToan {
                        MaDhNavigation = donHangMoi, // Gán vào đơn hàng mẹ
                        HinhThuc = "Chờ thanh toán", // (Hoặc bạn có thể để null)
                        SoTien = donHangMoi.TongTien,
                        NgayTt = DateTime.Now,
                        TrangThai = "Chưa thanh toán" // Trạng thái mặc định
                    };
                    db.ThanhToans.Add(thanhToanMoi);

                    // Bước 2.4: Trừ kho (Logic quan trọng)
                    foreach (var monAn in listChiTiet) {
                        int maSP = monAn.MaSp;
                        int soLuongBan = monAn.SoLuong;

                        // Sửa lỗi "Open DataReader"
                        var congThuc = db.DinhLuongs
                                         .Where(dl => dl.MaSp == maSP)
                                         .ToList();

                        if (congThuc.Count > 0) {
                            foreach (var nguyenLieuCan in congThuc) {
                                var nguyenLieuTrongKho = db.NguyenLieus
                                                           .FirstOrDefault(nl => nl.MaNl == nguyenLieuCan.MaNl);
                                if (nguyenLieuTrongKho != null) {
                                    decimal luongCanTru = nguyenLieuCan.SoLuongCan * soLuongBan;
                                    nguyenLieuTrongKho.SoLuongTon -= luongCanTru;
                                }
                            }
                        }
                    }

                    // Bước 2.5: Lưu CSDL
                    db.SaveChanges();

                    MessageBox.Show($"Đã LƯU TẠM đơn hàng {donHangMoi.MaDh}.\nKho đã được trừ.", "Thông báo");

                    // Bước 2.6: Làm mới giỏ hàng
                    lvDonHang.Items.Clear();
                    CapNhatTongTien();
                    TaiSanPham(_currentMaLoai); // Tải lại SP (vì kho đã đổi)
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lưu tạm đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
            }
            */
            #endregion

            // 1. Gọi hàm ThucHienLuuTam
            int maDonHangMoi = ThucHienLuuTam();

            // 2. Kiểm tra kết quả
            if (maDonHangMoi > 0) // (Hàm ThucHienLuuTam trả về -1 nếu lỗi)
            {
                MessageBox.Show($"Đã lưu tạm đơn hàng {maDonHangMoi}", "Lưu tạm thành công");
                // 3. Reset Form
                ResetMainForm();
            }
        }

        private void txtTimKiemKH_KeyPress(object sender, KeyPressEventArgs e)
        {

            // 'e.KeyChar' là ký tự mà người dùng vừa gõ (ví dụ: 'a', '1', '%')

            // Bước 1: Kiểm tra xem phím vừa gõ có phải là SỐ (Digit) không
            // (char.IsDigit() là hàm cơ bản của C# để kiểm tra số 0-9)
            bool laSo = char.IsDigit(e.KeyChar);

            // Bước 2: Kiểm tra xem phím vừa gõ có phải là phím "Điều khiển" không
            // (Phím điều khiển là các phím như Backspace (xóa), Enter, Ctrl+C, Ctrl+V)
            // (Chúng ta phải cho phép phím Backspace để người dùng còn sửa lỗi)
            bool laPhimDieuKhien = char.IsControl(e.KeyChar);

            // Bước 3: Ra quyết định
            // Nếu phím gõ KHÔNG PHẢI là số VÀ cũng KHÔNG PHẢI là phím điều khiển
            if (laSo == false && laPhimDieuKhien == false)
            {
                // ...thì chúng ta "hủy" phím gõ đó đi

                // e.Handled = true; có nghĩa là:
                // "Tôi đã xử lý phím này rồi, TextBox không cần làm gì nữa"
                // -> Kết quả: Ký tự (ví dụ: 'a') sẽ không bao giờ xuất hiện.
                e.Handled = true;
            }
        }

        private void txtTimKiemKH_TextChanged(object sender, EventArgs e)
        {
            // Khi người dùng tác động lại vào ô nhập SĐT thì tự động tìm khi đủ 10 số.
            string sdt = txtTimKiemKH.Text.Trim();

            // Nếu trống => Khách vãng lai, tắt nút Thêm
            if (string.IsNullOrEmpty(sdt))
            {
                lblTenKH.Text = "Khách vãng lai";
                _currentMaKH = null;
                btnThem.Enabled = false;
                return;
            }

            // Nếu không phải là chữ số hoặc không đủ 10 chữ số => thông báo và tắt nút Thêm
            if (sdt.Length != 10 || !sdt.All(char.IsDigit))
            {
                lblTenKH.Text = "Nhập đủ 10 số";
                _currentMaKH = null;
                btnThem.Enabled = false;
                return;
            }

            // Nếu đến đây là đúng 10 chữ số, kiểm tra CSDL
            SearchKhachHangBySDT(sdt);
        }

        #endregion

        #region Các hàm logic nghiệp vụ (Business Logic)
        private string KiemTraDuNguyenLieu(int maSP, List<DinhLuong> allDinhLuong, List<NguyenLieu> allNguyenLieu)
        {
            // 1. Lấy công thức từ List tạm
            var congThuc = new List<DinhLuong>();

            foreach (var dl in allDinhLuong)
            {
                if (dl.MaSp == maSP)
                {
                    congThuc.Add(dl);
                }
            }

            if (congThuc.Count == 0)
            {
                return "DU_HANG"; // Không có công thức = Luôn đủ
            }

            string trangThaiTongQuat = "DU_HANG";

            // 3. Lặp qua TỪNG NGUYÊN LIỆU trong công thức
            foreach (var nguyenLieuCan in congThuc)
            {
                // 4. Lấy nguyên liệu trong kho từ List tạm
                NguyenLieu nguyenLieuTrongKho = null;
                foreach (var nl in allNguyenLieu)
                {
                    if (nl.MaNl == nguyenLieuCan.MaNl)
                    {
                        nguyenLieuTrongKho = nl;
                        break;
                    }
                }

                if (nguyenLieuTrongKho == null)
                {
                    return "HET_HANG"; // Lỗi CSDL, coi như hết
                }

                // 5.1. KIỂM TRA HẾT HẲN (<= 0)
                if (nguyenLieuTrongKho.SoLuongTon <= 0)
                {
                    return "HET_HANG"; // Hết hẳn
                }

                // 5.2. KIỂM TRA SẮP HẾT (dưới ngưỡng)
                if (nguyenLieuTrongKho.SoLuongTon <= nguyenLieuTrongKho.NguongCanhBao)
                {
                    trangThaiTongQuat = "SAP_HET";
                }
            }

            return trangThaiTongQuat;
        }

        // Hàm này được gọi KHI THÊM VÀO GIỎ HÀNG
        // Nó sẽ kiểm tra xem kho có đủ cho (X) ly không
        private bool KiemTraSoLuongTonThucTe(int maSP, int soLuongMuonKiemTra)
        {
            // Mở CSDL (chỉ để kiểm tra)
            using (DataSqlContext db = new DataSqlContext())
            {
                // 1. Lấy công thức (Sửa lỗi DataReader)
                var congThuc = db.DinhLuongs
                                 .Where(dl => dl.MaSp == maSP)
                                 .ToList(); // .ToList()

                if (congThuc.Count == 0)
                {
                    return true; // Không có công thức, luôn đủ
                }

                // 2. Lặp qua công thức
                foreach (var nguyenLieuCan in congThuc)
                {
                    // 3. Lấy NL trong kho
                    var nguyenLieuTrongKho = db.NguyenLieus
                                               .FirstOrDefault(nl => nl.MaNl == nguyenLieuCan.MaNl && nl.TrangThai == "Đang kinh doanh");

                    if (nguyenLieuTrongKho == null)
                    {
                        MessageBox.Show($"Lỗi CSDL: Không tìm thấy nguyên liệu '{nguyenLieuCan.MaNl}'");
                        return false; // Lỗi, không cho bán
                    }

                    // 4. Tính toán
                    // (Lượng NL cần cho 1 ly) * (Tổng số ly muốn mua)
                    decimal tongNguyenLieuCan = nguyenLieuCan.SoLuongCan * soLuongMuonKiemTra;

                    // 5. KIỂM TRA QUAN TRỌNG
                    if (nguyenLieuTrongKho.SoLuongTon < tongNguyenLieuCan)
                    {
                        // Nếu số lượng tồn THỰC TẾ
                        // nhỏ hơn số lượng TỔNG CỘNG ta cần
                        MessageBox.Show(
                            $"Không đủ hàng trong kho cho {soLuongMuonKiemTra} ly!\n\n" +
                            $"Nguyên liệu: {nguyenLieuTrongKho.TenNl}\n" +
                            $"Chỉ còn: {nguyenLieuTrongKho.SoLuongTon}\n" +
                            $"Cần: {tongNguyenLieuCan}",
                            "Hết Hàng",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false; // KHÔNG ĐỦ
                    }
                }

                // Nếu lặp hết mà không 'return false'
                return true; // TẤT CẢ ĐỀU ĐỦ
            }
        }

        // Hàm này xử lý việc thêm SP vào giỏ hàng (ListView)
        private void ThemSanPhamVaoDonHang(SanPham sp)
        {

            // Bước 1: Kiểm tra xem SP này đã có trong giỏ hàng chưa
            foreach (ListViewItem item in lvDonHang.Items)
            {

                // 'Tag' của mỗi dòng trong ListView ta lưu MaSp (kiểu int)
                int maSpTrongGio = (int)item.Tag;

                // Nếu MaSp trong giỏ == MaSp của SP vừa bấm
                if (maSpTrongGio == sp.MaSp)
                {

                    // ----- ĐÃ CÓ, TĂNG SỐ LƯỢNG -----
                    // 1. Lấy số lượng cũ (từ cột 1)
                    int soLuongCu = int.Parse(item.SubItems[1].Text);

                    // 2. Tăng số lượng lên
                    int soLuongMoi = soLuongCu + 1;

                    // Kiểm tra kho có đủ không
                    bool duHang = KiemTraSoLuongTonThucTe(sp.MaSp, soLuongMoi);
                    if (duHang == false)
                    {
                        return; // Dừng lại, không tăng SL
                    }

                    // 3. Tính thành tiền mới
                    decimal thanhTienMoi = soLuongMoi * sp.DonGia;

                    // 4. Cập nhật lại ListView
                    item.SubItems[1].Text = soLuongMoi.ToString();
                    item.SubItems[3].Text = thanhTienMoi.ToString("N0");

                    // 5. Cập nhật tổng tiền
                    CapNhatTongTien();

                    // 6. Thoát hàm, không làm gì nữa
                    return;
                }
            }

            // ----- CHƯA CÓ, THÊM DÒNG MỚI -----
            // Nếu vòng lặp 'foreach' chạy hết mà không 'return',
            // nghĩa là đây là sản phẩm mới.

            // KIỂM TRA KHO TRƯỚC KHI THÊM  
            bool duHangMoi = KiemTraSoLuongTonThucTe(sp.MaSp, 1);
            if (duHangMoi == false)
            {
                return; // Dừng lại, không thêm
            }

            // 1. Tạo một dòng (ListViewItem) mới, cột đầu tiên là Tên SP
            ListViewItem lvi = new ListViewItem(sp.TenSp);

            // 2. Gán 'Tag' là MaSp để sau này kiểm tra
            lvi.Tag = sp.MaSp;

            // 3. Thêm các cột phụ (SubItems)
            lvi.SubItems.Add("1"); // Cột [1]: Số lượng
            lvi.SubItems.Add(sp.DonGia.ToString("N0")); // Cột [2]: Đơn giá
            lvi.SubItems.Add(sp.DonGia.ToString("N0")); // Cột [3]: Thành tiền

            // 4. Thêm dòng mới này vào ListView
            lvDonHang.Items.Add(lvi);

            // 5. Cập nhật tổng tiền
            CapNhatTongTien();
        }

        // Hàm này tính lại tổng tiền từ đầu
        private void CapNhatTongTien()
        {
            decimal tongTien = 0;
            decimal tongTienGiamGia = 0;

            // Lặp qua TẤT CẢ các dòng trong giỏ hàng
            foreach (ListViewItem item in lvDonHang.Items)
            {

                // Lấy thông tin từ giỏ hàng (đang là giá gốc)
                int maSP = (int)item.Tag;
                int soLuong = int.Parse(item.SubItems[1].Text);

                string donGiaGocStr = item.SubItems[2].Text.Replace(".", "");
                decimal donGiaGoc = decimal.Parse(donGiaGocStr, CultureInfo.CurrentCulture);


                // Lấy giá trị của cột Thành Tiền (cột 3)
                // Phải .Replace(".", "") để xóa dấu phẩy hàng nghìn
                // ví dụ: "20.000" -> "20000"
                string chuoiThanhTien = item.SubItems[3].Text.Replace(".", "");

                // Chuyển chữ "20000" thành số 20000
                decimal thanhTien = decimal.Parse(chuoiThanhTien, CultureInfo.CurrentCulture);

                // Cộng dồn vào tổng tiền
                tongTien = tongTien + thanhTien;

                // 1b. Tính chiết khấu 'SanPham' cho món này
                //     Hàm GetGiaBan() sẽ trả về giá đã giảm (ví dụ: 36,000)
                decimal donGiaMoi = GetGiaBan(maSP, donGiaGoc);

                decimal discountPerItem = donGiaGoc - donGiaMoi; // (ví dụ: 45,000 - 36,000 = 9,000)

                // Cộng dồn tổng giảm giá SP (ví dụ: 9,000 * 2 ly = 18,000)
                tongTienGiamGia += (discountPerItem * soLuong);
            }

            // 2. Tìm khuyến mãi 'HoaDon' (Code này giữ nguyên)
            KhuyenMai kmHoaDon = null;
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    // Lấy ngày hiện tại
                    DateOnly now = DateOnly.FromDateTime(DateTime.Now);

                    // Lấy tất cả KM từ CSDL
                    var allKM = db.KhuyenMais.ToList();

                    // Tìm KM phù hợp
                    foreach (KhuyenMai km in allKM)
                    {
                        if (km.LoaiKm == "HoaDon" &&
                            km.TrangThai == "Đang áp dụng" &&
                            km.NgayBatDau <= now &&
                            km.NgayKetThuc >= now)
                        {
                            if (kmHoaDon == null || km.GiaTri > kmHoaDon.GiaTri)
                            {
                                kmHoaDon = km;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy KM hóa đơn: " + ex.Message);
            }

            // 3. Tính toán giảm giá 'HoaDon'
            // KM 'HoaDon' được tính trên số tiền SAU KHI đã giảm KM 'SanPham'
            decimal baseForHoaDon = tongTien - tongTienGiamGia;
            decimal tongGiamGiaHoaDon = 0;

            // Áp dụng khuyến mãi 'Hóa Đơn' nếu có
            if (kmHoaDon != null)
            {
                decimal phanTramGiam = kmHoaDon.GiaTri / 100;
                tongGiamGiaHoaDon = baseForHoaDon * phanTramGiam;
            }

            // 4. Tính toán tổng cuối cùng
            // Tổng giảm giá = giảm trên SP + giảm trên Hóa Đơn
            decimal tongTienGiaHD = tongTienGiamGia + tongGiamGiaHoaDon;
            decimal finalTotal = tongTien - tongTienGiaHD; // Thành tiền


            // 5. Cập nhật UI
            lblTienTruocGiam.Text = tongTien.ToString("N0") + " đ";
            lblGiamGia.Text = (-tongTienGiaHD).ToString("N0") + " đ";
            lblTongCong.Text = finalTotal.ToString("N0") + " đ";

            soTienGiam = tongTienGiaHD;
            tongGoc = tongTien;
        }


        // (Hàm này trả về MaDH mới, hoặc -1 nếu lỗi)
        private int ThucHienLuuTam()
        {
            // 1. Kiểm tra giỏ hàng
            if (lvDonHang.Items.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm sản phẩm vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1; // Báo lỗi
            }

            // 2. Lấy tổng tiền
            string tongTienStr = lblTongCong.Text.Replace(" đ", "").Replace(".", "");
            decimal tongTien = decimal.Parse(tongTienStr, CultureInfo.InvariantCulture);

            // 3. LƯU CSDL (Lưu Đơn, Trừ Kho, Tạo ThanhToan "Chưa TT")
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    // Bước 1: Tạo đối tượng DonHang
                    var donHangMoi = new DonHang
                    {
                        NgayLap = DateTime.Now,
                        MaNv = _currentMaNV,
                        TrangThai = "Đang xử lý", // Trạng thái chờ
                        TongTien = tongTien,
                        MaKh = _currentMaKH // Gán MaKH (có thể null)
                    };

                    // Bước 2: Tạo danh sách các ChiTietDonHang
                    var listChiTiet = new List<ChiTietDonHang>();
                    foreach (ListViewItem item in lvDonHang.Items)
                    {
                        int maSP = (int)item.Tag;
                        int soLuong = int.Parse(item.SubItems[1].Text);
                        decimal donGia = decimal.Parse(item.SubItems[2].Text.Replace(".", ""), CultureInfo.InvariantCulture);

                        var chiTiet = new ChiTietDonHang
                        {
                            MaDhNavigation = donHangMoi, // Gán vào đơn hàng mẹ
                            MaSp = maSP,
                            SoLuong = soLuong,
                            DonGia = donGia
                        };
                        listChiTiet.Add(chiTiet);
                    }

                    // Bước 3: Tạo ThanhToan (Trạng thái "Chưa thanh toán")
                    var thanhToanMoi = new Models.ThanhToan
                    {
                        MaDhNavigation = donHangMoi, // Gán vào đơn hàng mẹ
                        HinhThuc = null,
                        SoTien = tongTien,
                        TrangThai = "Chưa thanh toán"
                    };

                    // Bước 4: Báo cho EF Core biết chúng ta muốn...
                    db.DonHangs.Add(donHangMoi);
                    db.ChiTietDonHangs.AddRange(listChiTiet);
                    db.ThanhToans.Add(thanhToanMoi);

                    // Bước 5: Trừ kho (Code y hệt trong ThanhToan.cs)
                    foreach (var monAn in listChiTiet)
                    {
                        int maSP = monAn.MaSp;
                        int soLuongBan = monAn.SoLuong;

                        // Sửa lỗi "Open DataReader" bằng cách thêm .ToList()
                        var congThuc = db.DinhLuongs
                                         .Where(dl => dl.MaSp == maSP)
                                         .ToList();

                        if (congThuc.Count > 0)
                        {
                            foreach (var nguyenLieuCan in congThuc)
                            {
                                var nguyenLieuTrongKho = db.NguyenLieus
                                                           .FirstOrDefault(nl => nl.MaNl == nguyenLieuCan.MaNl);
                                if (nguyenLieuTrongKho != null)
                                {
                                    decimal luongCanTru = nguyenLieuCan.SoLuongCan * soLuongBan;
                                    nguyenLieuTrongKho.SoLuongTon -= luongCanTru;

                                    db.Update(nguyenLieuTrongKho);
                                }
                            }
                        }
                    }

                    // Bước 6: Thực thi lệnh, lưu vào CSDL
                    db.SaveChanges(); // <-- LƯU TẤT CẢ THAY ĐỔI

                    // Bước 7: Trả về MaDH mới
                    return donHangMoi.MaDh;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu tạm đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
                return -1; // Báo lỗi
            }
        }

        private void ResetMainForm()
        {
            // 1. Xóa giỏ hàng
            lvDonHang.Items.Clear();
            // 2. Cập nhật tổng tiền (về 0)
            CapNhatTongTien();
            // 3. Reset thông tin khách hàng
            lblTenKH.Text = "Khách vãng lai";
            _currentMaKH = null;
            txtTimKiemKH.Text = "";
            // 4. Tải lại danh sách sản phẩm (Vì kho đã bị trừ)
            TaiSanPham(_currentMaLoai);
            lvDonHang.SelectedItems.Clear();
        }

        // Helper để tìm khách hàng theo SĐT và cập nhật UI
        private void SearchKhachHangBySDT(string sdt)
        {
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SoDienThoai == sdt);

                    if (khachHang != null)
                    {
                        // TÌM THẤY
                        lblTenKH.Text = khachHang.TenKh;
                        _currentMaKH = khachHang.MaKh; // Lưu lại MaKH!
                        btnThem.Enabled = false; // đã có trong DB -> không cần thêm
                    }
                    else
                    {
                        // KHÔNG TÌM THẤY -> hiển thị thông báo "Không tìm thấy KH"
                        // và cho phép thêm khách mới
                        lblTenKH.Text = "Không tìm thấy KH";
                        _currentMaKH = null;

                        // Cho phép tương tác với nút Thêm (enabled)
                        btnThem.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm khách hàng: " + ex.Message);
                lblTenKH.Text = "Lỗi khi tìm";
                _currentMaKH = null;
                btnThem.Enabled = false;
            }
        }

        #endregion








        private void flpLoaiSP_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
