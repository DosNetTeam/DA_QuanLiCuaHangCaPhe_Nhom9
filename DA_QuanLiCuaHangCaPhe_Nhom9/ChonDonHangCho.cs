using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class ChonDonHangCho : Form {

        public int MaDonHangDaChon { get; private set; }
        public ChonDonHangCho() {
            InitializeComponent();
        }

        private void ChonDonHangCho_Load(object sender, EventArgs e) {
            TaiDanhSachDonHangCho();
        }

        private void btnTaiLai_Click(object sender, EventArgs e) {
            TaiDanhSachDonHangCho();
        }

        private void btnChonThanhToan_Click(object sender, EventArgs e) {
            // 1. Kiểm tra xem người dùng đã chọn đơn nào chưa
            if (lvDonHangCho.SelectedItems.Count == 0) {
                MessageBox.Show("Vui lòng chọn một đơn hàng để thanh toán.");
                return;
            }

            // 2. Lấy MaDH từ 'Tag' của dòng đã chọn
            ListViewItem itemDaChon = lvDonHangCho.SelectedItems[0];
            int maDHCanThanhToan = (int)itemDaChon.Tag;

            // --- BƯỚC 2: GÁN GIÁ TRỊ CHO BIẾN PUBLIC ---           
            this.MaDonHangDaChon = maDHCanThanhToan;

            // --- BƯỚC 3: SỬA LỖI CS7036 (Constructor) ---
            // (Mở form ThanhToan mới và truyền *chỉ* MaDH vào,
            // vì form ThanhToan (Bước 4) sẽ tự tải dữ liệu)
            ThanhToan frmThanhToan = new ThanhToan(maDHCanThanhToan);
            var result = frmThanhToan.ShowDialog();

            // 4. KIỂM TRA KẾT QUẢ
            // Nếu thanh toán thành công (bấm "In Hóa Đơn")
            if (result == DialogResult.OK) {
                // Tự động tải lại danh sách
                // (Đơn hàng vừa thanh toán sẽ biến mất khỏi list)
                TaiDanhSachDonHangCho();
            }

            // (Nếu bấm "Cancel" trên form ThanhToan thì không làm gì)

        }

        private void TaiDanhSachDonHangCho() {
            lvDonHangCho.Items.Clear();

            try {
                // Mở kết nối CSDL
                using (DataSqlContext db = new DataSqlContext()) {
                    // Bước 1: Lấy TẤT CẢ đơn hàng "Đang xử lý"
                    // (Chúng ta không dùng .Include() nữa)
                    var donHangCho = db.DonHangs
                                       .Where(dh => dh.TrangThai == "Đang xử lý")
                                       .OrderBy(dh => dh.NgayLap) // Xếp cái cũ lên trước
                                       .ToList();

                    // Bước 2: Lấy TẤT CẢ khách hàng ra một danh sách tạm
                    // (Đây là cách cơ bản, chúng ta sẽ "join" bằng tay)
                    var allKhachHang = db.KhachHangs.ToList();

                    // Bước 3: Lặp qua từng đơn hàng (trong danh sách donHangCho)
                    foreach (var dh in donHangCho) {
                        string tenKH = "Khách vãng lai";

                        // Bước 3.1: Kiểm tra xem đơn hàng này có MaKH không
                        // (dh.MaKh != null nghĩa là "không phải khách vãng lai")
                        if (dh.MaKh != null) {
                            // Nếu có, lặp qua danh sách allKhachHang để tìm tên
                            foreach (var kh in allKhachHang) {
                                if (kh.MaKh == dh.MaKh) {
                                    tenKH = kh.TenKh;
                                    break; // Dừng tìm khi thấy
                                }
                            }
                        }

                        // Bước 4: Tạo dòng ListView (giống code cũ)
                        ListViewItem lvi = new ListViewItem(dh.MaDh.ToString());

                        // Gán MaDH vào Tag để lát nữa lấy
                        lvi.Tag = dh.MaDh;

                        // Thêm các cột phụ
                        lvi.SubItems.Add(tenKH);

                        // Vì NgayLap có thể null (DateTime?),
                        // chúng ta phải kiểm tra .HasValue
                        if (dh.NgayLap.HasValue) {
                            lvi.SubItems.Add(dh.NgayLap.Value.ToString("HH:mm dd/MM/yy"));
                        }
                        else {
                            lvi.SubItems.Add("N/A"); // Hoặc "" (chuỗi rỗng)
                        }

                        lvi.SubItems.Add(dh.TongTien?.ToString("N0") + " đ");

                        // Thêm dòng này vào ListView
                        lvDonHangCho.Items.Add(lvi);
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải danh sách đơn chờ: " + ex.Message);
            }
        }

        private void btnHuyDonCho_Click(object sender, EventArgs e) {


            // 1. Kiểm tra xem người dùng đã chọn đơn nào chưa
            if (lvDonHangCho.SelectedItems.Count == 0) {
                MessageBox.Show("Vui lòng chọn một đơn hàng để HỦY.");
                return;
            }

            // 2. Lấy MaDH từ 'Tag' của dòng đã chọn
            ListViewItem itemDaChon = lvDonHangCho.SelectedItems[0];
            int maDHCanHuy = (int)itemDaChon.Tag;

            // 3. Hỏi xác nhận
            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn HỦY đơn hàng [MaDH: {maDHCanHuy}] không?\n\nHÀNH ĐỘNG NÀY SẼ HOÀN TRẢ NGUYÊN LIỆU VỀ KHO.",
                "Xác nhận Hủy Đơn",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.No) {
                return; // Người dùng không muốn hủy
            }

            // 4. Bắt đầu quá trình HỦY và HOÀN KHO
            try {
                // Mở kết nối CSDL
                using (DataSqlContext db = new DataSqlContext()) {
                    // --- BẮT ĐẦU GIAO DỊCH AN TOÀN ---
                    // (Transaction đảm bảo tất cả cùng thành công,
                    // hoặc tất cả cùng thất bại)
                    using (var transaction = db.Database.BeginTransaction()) {
                        // 5. Tải các đối tượng cần thiết
                        var donHang = db.DonHangs.FirstOrDefault(dh => dh.MaDh == maDHCanHuy);
                        var thanhToan = db.ThanhToans.FirstOrDefault(tt => tt.MaDh == maDHCanHuy && tt.TrangThai == "Chưa thanh toán");
                        var chiTiet = db.ChiTietDonHangs
                                        .Where(ct => ct.MaDh == maDHCanHuy)
                                        .ToList(); // (Dùng .ToList() để tránh lỗi DataReader)

                        if (donHang == null || thanhToan == null) {
                            MessageBox.Show("Lỗi: Không tìm thấy đơn hàng hoặc thanh toán để hủy.");
                            transaction.Rollback(); // Hủy bỏ giao dịch
                            return;
                        }

                        // --- 6. HOÀN KHO (LOGIC QUAN TRỌNG) ---
                        // Lặp qua từng món ăn đã bị trừ kho
                        foreach (var monAn in chiTiet) {
                            int maSP = monAn.MaSp;
                            int soLuongBiTru = monAn.SoLuong;

                            // Lấy công thức của món đó
                            var congThuc = db.DinhLuongs
                                             .Where(dl => dl.MaSp == maSP)
                                             .ToList();

                            if (congThuc.Count > 0) {
                                // Lặp qua từng nguyên liệu trong công thức
                                foreach (var nguyenLieuCan in congThuc) {
                                    // Tìm nguyên liệu đó trong kho
                                    var nguyenLieuTrongKho = db.NguyenLieus
                                                               .FirstOrDefault(nl => nl.MaNl == nguyenLieuCan.MaNl);

                                    if (nguyenLieuTrongKho != null) {
                                        // Tính toán lượng CỘNG TRẢ LẠI
                                        decimal luongCanCong = nguyenLieuCan.SoLuongCan * soLuongBiTru;

                                        // CỘNG TRẢ
                                        nguyenLieuTrongKho.SoLuongTon += luongCanCong;

                                        // Báo cho CSDL biết là ta đã sửa nó
                                        db.Update(nguyenLieuTrongKho);
                                    }
                                }
                            }
                        }

                        // --- 7. CẬP NHẬT TRẠNG THÁI ---
                        donHang.TrangThai = "Đã huỷ";
                        thanhToan.TrangThai = "Đã huỷ";
                        db.Update(donHang);
                        db.Update(thanhToan);

                        // --- 8. LƯU TẤT CẢ THAY ĐỔI ---
                        db.SaveChanges(); // Lưu (Cộng kho + Đổi trạng thái)
                        transaction.Commit(); // Hoàn tất giao dịch an toàn

                        MessageBox.Show($"Đã hủy thành công đơn hàng {maDHCanHuy}. \nĐã hoàn trả nguyên liệu về kho.", "Thông báo");

                        // 9. Tải lại danh sách (đơn hàng 101 sẽ biến mất)
                        TaiDanhSachDonHangCho();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi nghiêm trọng khi hủy đơn hàng: " + ex.Message);
            }

        }

        // Bấm đúp vào 1 dòng = Bấm nút "Thanh Toán"
        private void lvDonHangCho_MouseDoubleClick(object sender, MouseEventArgs e) {


            // Giả lập việc bấm nút "Thanh Toán"
            btnChonThanhToan_Click(sender, e);

        }
    }
}
