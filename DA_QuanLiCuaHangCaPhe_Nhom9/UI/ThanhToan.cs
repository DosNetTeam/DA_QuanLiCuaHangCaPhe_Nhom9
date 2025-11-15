
using DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Main;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class ThanhToan : Form {
        // Biến (fields) để lưu dữ liệu
        private decimal _tongTien;
        private int _maDonHangChon;
        private DonHang _donHangCanThanhToan;
        private Models.ThanhToan _thanhToanCanCapNhat;

        private decimal _tongTienGoc_passed;
        private decimal _soTienGiam_passed;

        // *** THAY ĐỔI: Khai báo Dịch Vụ ***
        private readonly DichVuThanhToan _dichVuThanhToan;
        #region KHỞI TẠO VÀ TẢI DỮ LIỆU
        public ThanhToan(int maDonHangChon, decimal tongGoc, decimal soTienGiam) {
            InitializeComponent();
            _maDonHangChon = maDonHangChon;
            _tongTienGoc_passed = tongGoc;
            _soTienGiam_passed = soTienGiam;

            // *** THAY ĐỔI: Khởi tạo Dịch Vụ ***
            _dichVuThanhToan = new DichVuThanhToan();
        }

        
        private void ThanhToan_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Gọi Dịch Vụ để tải thông tin
                var ketQua = _dichVuThanhToan.TaiThongTinThanhToan(_maDonHangChon);

                // 2. Kiểm tra lỗi
                if (ketQua == null)
                {
                    MessageBox.Show("Lỗi: Không tìm thấy đơn hàng hoặc thanh toán chờ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                // 3. Gán dữ liệu vào các biến
                _donHangCanThanhToan = ketQua.DonHang;
                _thanhToanCanCapNhat = ketQua.ThanhToan;
                _tongTien = _donHangCanThanhToan.TongTien ?? 0; // <-- Lấy 36.000 (ĐÚNG)

                var chiTietDonHang = ketQua.ChiTiet;
                var allSanPham = ketQua.SanPhams;

                // 4. Cấu hình giao diện
                lblTongCongBill.Text = _tongTien.ToString("N0") + " đ";
                txtKhachDua.Text = _tongTien.ToString("N0");
                lblTienDu.Text = "0 đ";

                
                // Chúng ta sẽ tính lại tổng gốc (Tiền trước giảm)
                // ngay tại đây để đảm bảo nó luôn đúng,
                // bất kể MainForm truyền gì sang.

                decimal tongGocMoi = 0;

                // 5. Đổ danh sách món ăn vào ListView VÀ TÍNH LẠI TỔNG GỐC
                lvChiTietBill.Items.Clear();
                foreach (var ct in chiTietDonHang)
                {
                    string tenSP = "Không tìm thấy SP";
                    foreach (var sp in allSanPham) // Join thủ công
                    {
                        if (sp.MaSp == ct.MaSp)
                        {
                            tenSP = sp.TenSp;
                            break;
                        }
                    }

                    // Tính thành tiền của món này (ct.DonGia là giá gốc)
                    decimal thanhTienGoc = ct.SoLuong * ct.DonGia;

                    // Cộng dồn vào tổng gốc mới
                    tongGocMoi += thanhTienGoc; // <-- Sẽ tính ra 45.000

                    ListViewItem lvi = new ListViewItem(tenSP);
                    lvi.SubItems.Add(ct.SoLuong.ToString());
                    lvi.SubItems.Add(ct.DonGia.ToString("N0"));
                    lvi.SubItems.Add(thanhTienGoc.ToString("N0"));
                    lvChiTietBill.Items.Add(lvi);
                }

                // 6. GHI ĐÈ CÁC BIẾN _passed BẰNG GIÁ TRỊ ĐÃ TÍNH LẠI
                _tongTienGoc_passed = tongGocMoi; // <-- Ghi đè thành 45.000
                _soTienGiam_passed = tongGocMoi - _tongTien; // <-- Ghi đè thành (45.000 - 36.000) = 9.000

                // --- KẾT THÚC SỬA LỖI ---


                // 7. Cấu hình thanh toán (trước đây là bước 6)
                rbTienMat.Checked = true;
                if (this.Controls.Find("panelBillPreview", true).FirstOrDefault() is Panel panelBillPreview)
                {
                    pbQR_InBill.Visible = false;
                    // Giờ hàm này sẽ dùng _tongTienGoc_passed = 45.000 và _soTienGiam_passed = 9.000
                    HienThiBillPreview(panelBillPreview); // Vẽ hóa đơn
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin thanh toán: " + ex.Message);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        #endregion

        #region HÓA ĐƠN XEM TRƯỚC (Giữ nguyên logic)

        private Label AddLabelToBill(string text, int verticalPosition, float fontSize, FontStyle fontStyle = FontStyle.Regular, int horizontalPosition = -1) {
            Label lbl = new Label(); lbl.Text = text; lbl.Font = new Font("Segoe UI", fontSize, fontStyle); lbl.AutoSize = true; lbl.BackColor = Color.Transparent; int xPosition = horizontalPosition; if (xPosition == -1) { xPosition = (panelBillPreview.Width - TextRenderer.MeasureText(text, lbl.Font).Width) / 2; }
            lbl.Location = new Point(xPosition, verticalPosition); panelBillPreview.Controls.Add(lbl); return lbl;
        }
        private void HienThiBillPreview(Panel panelBillPreview) {
            while (panelBillPreview.Controls.Count > 0) { Control c = panelBillPreview.Controls[0]; if (c != pbQR_InBill) { panelBillPreview.Controls.Remove(c); c.Dispose(); } else { panelBillPreview.Controls.Remove(c); } }
            int currentY = 10;
            Label lblTenQuan = AddLabelToBill("COFFEE", currentY, 14, FontStyle.Bold); 
            currentY += lblTenQuan.Height + 2;
            Label lblDiaChi = AddLabelToBill("Ung Van Khiem, Long Xuyen", currentY, 9); 
            currentY += lblDiaChi.Height;
            Label lblSDT = AddLabelToBill("0814 585 526", currentY, 9); 
            currentY += lblSDT.Height + 5; 
            currentY += 20;
            Label lblHoaDon = AddLabelToBill("HÓA ĐƠN", currentY, 12, FontStyle.Bold); 
            currentY += lblHoaDon.Height + 15;
            AddLabelToBill("Tên món", currentY, 9, FontStyle.Regular, 40); 
            AddLabelToBill("SL", currentY, 9, FontStyle.Regular, 250); 
            AddLabelToBill("Dơn giá", currentY, 9, FontStyle.Regular, 320); 
            AddLabelToBill("Thành tiền", currentY, 9, FontStyle.Regular, 450); currentY += 30;
            foreach (ListViewItem item in lvChiTietBill.Items) { string tenMon = item.SubItems[0].Text; string soLuong = item.SubItems[1].Text; string donGia = item.SubItems[2].Text; string thanhTien = item.SubItems[3].Text; AddLabelToBill(tenMon, currentY, 9, FontStyle.Regular, 40); AddLabelToBill(soLuong, currentY, 9, FontStyle.Regular, 250); AddLabelToBill(donGia, currentY, 9, FontStyle.Regular, 320); AddLabelToBill(thanhTien, currentY, 9, FontStyle.Regular, 450); currentY += 30; }
            currentY += 15; AddLabelToBill("-----------------------------------", currentY, 9); currentY += 55;
            AddLabelToBill("Tiền trước giảm:", currentY, 10, FontStyle.Regular, 40); AddLabelToBill(_tongTienGoc_passed.ToString("N0") + " đ", currentY, 14, FontStyle.Regular, 290); currentY += 55;
            AddLabelToBill("Giảm giá:", currentY, 10, FontStyle.Regular, 40); AddLabelToBill("(-" + _soTienGiam_passed.ToString("N0") + " đ)", currentY, 14, FontStyle.Regular, 290); currentY += 55;
            AddLabelToBill("Thành tiền:", currentY, 12, FontStyle.Bold, 40); AddLabelToBill(_tongTien.ToString("N0") + " đ", currentY, 14, FontStyle.Bold, 290); currentY += 55;
            AddLabelToBill("Xin cảm ơn quý khách!", currentY, 9, FontStyle.Italic); currentY += 35; AddLabelToBill("Hẹn gặp lại quý khách!", currentY, 9, FontStyle.Italic); currentY += 25;
            panelBillPreview.Controls.Add(pbQR_InBill); pbQR_InBill.Top = currentY + 15; pbQR_InBill.Left = (panelBillPreview.Width - pbQR_InBill.Width) / 2;
        }
        #endregion

        #region XỬ LÝ SỰ KIỆN (Thay đổi btn_inhoadon_Click)


        private void txtKhachDua_TextChanged(object sender, EventArgs e) {
            decimal khachDua = 0;
            decimal.TryParse(txtKhachDua.Text.Replace(".", ""), out khachDua);
            decimal tienDu = khachDua - _tongTien;
            lblTienDu.Text = tienDu.ToString("N0") + " đ";
        }


        private void rbQR_CheckedChanged(object sender, EventArgs e) {
            if (rbQR.Checked) {
                pbQR_InBill.Visible = true;
                txtKhachDua.Enabled = false;
                lblTienDu.Text = "0 đ";
                try { pbQR_InBill.ImageLocation = $"https://api.vietqr.io/image/970436-0909090909-snt03N5.jpg?accountName=TEST&amount={_tongTien}"; }
                catch (Exception) { MessageBox.Show("Lỗi khi tải mã QR. Vui lòng kiểm tra kết nối internet."); }
            }
            else if (rbTienMat.Checked) {
                pbQR_InBill.Visible = false;
                txtKhachDua.Enabled = true;
                txtKhachDua_TextChanged(sender, e);
            }
        }

        // *** ĐÃ THAY ĐỔI: Gọi DichVuThanhToan ***
        private void btn_inhoadon_Click(object sender, EventArgs e) {
            // 1. Kiểm tra tiền mặt (Giữ nguyên)
            if (rbTienMat.Checked) {
                decimal khachDua = 0;
                decimal.TryParse(txtKhachDua.Text.Replace(".", ""), out khachDua);
                if (khachDua < _tongTien) {
                    MessageBox.Show("Số tiền khách đưa không đủ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 2. LƯU CSDL (*** THAY ĐỔI ***)
            try {
                string hinhThucThanhToan = rbTienMat.Checked ? "Tiền mặt" : "Chuyển khoản QR";

                // Gọi Dịch Vụ
                bool success = _dichVuThanhToan.XacNhanThanhToan(_maDonHangChon, hinhThucThanhToan);

                if (success) {
                    MessageBox.Show($"Đã thanh toán thành công {_tongTien.ToString("N0")} đ", "Thông báo");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else {
                    MessageBox.Show("Lỗi khi lưu đơn hàng: Không tìm thấy đơn hàng hoặc thanh toán.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lưu đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        #endregion
        private void pbQR_InBill_Click(object sender, EventArgs e) {

        }
    }
}