using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using System.Data;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {


    public partial class ThanhToan : Form {
        // Biến (fields) để lưu dữ liệu được truyền từ MainForm
        private decimal _tongTien;
        private int _maNhanVien;
        private int? _maKhachHang;
        //private ListView.ListViewItemCollection _danhSachMonAn;

        private int _maDonHangChon; // Sẽ nhận từ ChonDonHangCho
        private DonHang _donHangCanThanhToan; // Đơn hàng đang xử lý
        private Models.ThanhToan _thanhToanCanCapNhat; // Thanh toán đang chờ
        private decimal _tienThua; // Tiền thừa (nếu có)

        // Dùng để lưu lại tổng tiền gốc và số tiền giảm (nếu có)
        private decimal _tongTienGoc_passed;
        private decimal _soTienGiam_passed;


        #region khởi tạo form 
        #region tu sửa lại code
        /*
          public ThanhToan(ListView.ListViewItemCollection danhSachMonAn, decimal tongTien, int maNhanVien, int? maKhachHang) {
              InitializeComponent();
              // Lưu dữ liệu vào các biến
              _tongTien = tongTien;
              _danhSachMonAn = danhSachMonAn;
              _maNhanVien = maNhanVien;
              _maKhachHang = maKhachHang;


              if (maNhanVien <= 0) {
                  MessageBox.Show(
                      "Lỗi: Không nhận được Mã Nhân Viên (MaNV) từ MainForm.\n\nVui lòng kiểm tra lại file MainForm.cs và đảm bảo biến '_currentMaNV' đã được gán giá trị hợp lệ (ví dụ: 1, 2, hoặc 3).",
                      "Lỗi Truyền Dữ Liệu",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);

                  this.DialogResult = DialogResult.Cancel; // Hủy và đóng form

                  // Lên lịch để đóng form ngay sau khi nó vừa kịp tải
                  // (Vì chúng ta không thể Close() ngay trong constructor)
                  this.Load += (s, e) => this.Close();
                  return;
              }

              // Lưu dữ liệu vào các biến
              _tongTien = tongTien;
              _danhSachMonAn = danhSachMonAn;
              _maNhanVien = maNhanVien;
              _maKhachHang = maKhachHang;
          } 
        */
        #endregion

        public ThanhToan(int maDonHangChon, decimal tongGoc, decimal soTienGiam) {
            InitializeComponent();
            _maDonHangChon = maDonHangChon;

            _tongTienGoc_passed = tongGoc;
            _soTienGiam_passed = soTienGiam;
        }


        private void ThanhToan_Load(object sender, EventArgs e) {
            #region code cũ
            /*
             if (this.DialogResult == DialogResult.Cancel) {
                 return;
             }

             // --- 1. Cấu hình phần tính tiền (Bên trái) ---
             lblTongCongBill.Text = _tongTien.ToString("N0") + " đ";
             txtKhachDua.Text = _tongTien.ToString("N0");
             lblTienDu.Text = "0 đ";

             // --- 2. Đổ danh sách món ăn vào ListView 'lvChiTietBill' (Bên trái) ---
             lvChiTietBill.Items.Clear();

             // Vòng lặp foreach vẫn hoạt động bình thường với ListViewItemCollection
             foreach (ListViewItem item in _danhSachMonAn)
                 // Phải 'Clone' (tạo bản sao) vì item không thể ở 2 form cùng lúc
                 lvChiTietBill.Items.Add((ListViewItem)item.Clone());

             // --- 3. Cấu hình phần thanh toán (Bên phải) ---
             rbTienMat.Checked = true;

             // (dùng bản xem trước hóa đơn)
             if (this.Controls.Find("panelBillPreview", true).FirstOrDefault() is Panel panelBillPreview) {
                 pbQR_InBill.Visible = false;
                 HienThiBillPreview(panelBillPreview); // Vẽ hóa đơn
             }
             //// (dùng thiết kế cũ)
             //else if (this.Controls.Find("pbQR", true).FirstOrDefault() is PictureBox pbQR) {
             //    pbQR.Visible = false;
             //}
            */
            #endregion

            try {
                // Mở CSDL để tải thông tin đơn hàng chờ
                using (DataSqlContext db = new DataSqlContext()) {
                    // 1. Tải Đơn Hàng
                    // Chúng ta dùng .FirstOrDefault()
                    _donHangCanThanhToan = db.DonHangs
                                             .FirstOrDefault(dh => dh.MaDh == _maDonHangChon);

                    // 2. Tải Chi Tiết Đơn Hàng
                    // Chúng ta dùng .Where() và .ToList()
                    var chiTietDonHang = db.ChiTietDonHangs
                                           .Where(ct => ct.MaDh == _maDonHangChon)
                                           .ToList();

                    // 3. Tải Thanh Toán (đang "Chưa thanh toán")
                    _thanhToanCanCapNhat = db.ThanhToans
                                             .FirstOrDefault(tt => tt.MaDh == _maDonHangChon && tt.TrangThai == "Chưa thanh toán");

                    // 4. Tải SanPham (để lấy Tên SP)
                    // (Tải 1 lần để dùng cho vòng lặp, tránh lỗi DataReader)
                    var allSanPham = db.SanPhams.ToList();

                    // 5. Kiểm tra lỗi (Nếu không tìm thấy)
                    if (_donHangCanThanhToan == null || _thanhToanCanCapNhat == null) {
                        MessageBox.Show("Lỗi: Không tìm thấy đơn hàng hoặc thanh toán chờ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                        return;
                    }

                    // 6. Lưu lại tổng tiền
                    // (Lấy từ CSDL, không cần truyền qua nữa)
                    _tongTien = _donHangCanThanhToan.TongTien ?? 0;

                    // --- 7. Cấu hình giao diện (Giống code cũ) ---
                    lblTongCongBill.Text = _tongTien.ToString("N0") + " đ";
                    txtKhachDua.Text = _tongTien.ToString("N0");
                    lblTienDu.Text = "0 đ";

                    // --- 8. Đổ danh sách món ăn vào ListView 'lvChiTietBill' ---
                    lvChiTietBill.Items.Clear();
                    foreach (var ct in chiTietDonHang) {
                        // Lấy tên sản phẩm (dùng vòng lặp cơ bản)
                        string tenSP = "Không tìm thấy SP";
                        foreach (var sp in allSanPham) {
                            if (sp.MaSp == ct.MaSp) {
                                tenSP = sp.TenSp;
                                break;
                            }
                        }

                        // Tạo dòng ListViewItem
                        ListViewItem lvi = new ListViewItem(tenSP);
                        lvi.SubItems.Add(ct.SoLuong.ToString());
                        lvi.SubItems.Add(ct.DonGia.ToString("N0"));
                        // Tính thành tiền (SL * Đơn giá)
                        lvi.SubItems.Add((ct.SoLuong * ct.DonGia).ToString("N0"));

                        // (Chúng ta không cần Tag ở đây nữa)
                        lvChiTietBill.Items.Add(lvi);
                    }

                    // --- 9. Cấu hình thanh toán ---
                    rbTienMat.Checked = true;

                    if (this.Controls.Find("panelBillPreview", true).FirstOrDefault() is Panel panelBillPreview) {
                        pbQR_InBill.Visible = false;
                        HienThiBillPreview(panelBillPreview); // Vẽ hóa đơn
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi tải thông tin thanh toán: " + ex.Message);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        #endregion

        #region HÓA ĐƠN XEM TRƯỚC
        private Label AddLabelToBill(string text, int verticalPosition, float fontSize, FontStyle fontStyle = FontStyle.Regular, int horizontalPosition = -1) {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", fontSize, fontStyle);
            lbl.AutoSize = true;
            lbl.BackColor = Color.Transparent;

            int xPosition = horizontalPosition;
            // Nếu không set vị trí X (-1), thì căn giữa
            if (xPosition == -1) {
                // Căn giữa label bên trong panelBillPreview
                xPosition = (panelBillPreview.Width - TextRenderer.MeasureText(text, lbl.Font).Width) / 2;
            }
            lbl.Location = new Point(xPosition, verticalPosition);

            panelBillPreview.Controls.Add(lbl);
            return lbl;
        }

        private void HienThiBillPreview(Panel panelBillPreview) {
            // --- 1. XÓA SẠCH BILL CŨ ---
            // Xóa tất cả control TRỪ pbQR_InBill
            while (panelBillPreview.Controls.Count > 0) {
                Control c = panelBillPreview.Controls[0];
                if (c != pbQR_InBill) // Giữ lại PictureBox QR
                {
                    panelBillPreview.Controls.Remove(c);
                    c.Dispose(); // Giải phóng bộ nhớ
                }
                else {
                    // Tạm thời bỏ nó ra để AddLabelToBill hoạt động đúng
                    panelBillPreview.Controls.Remove(c);
                }
            }

            // --- 2. BIẾN THEO DÕI VỊ TRÍ Y (CHIỀU DỌC) ---
            int currentY = 10;

            // --- 3. VẼ PHẦN TIÊU ĐỀ (HEADER) ---
            Label lblTenQuan = AddLabelToBill("COFFEE", currentY, 14, FontStyle.Bold);
            currentY += lblTenQuan.Height + 2;

            Label lblDiaChi = AddLabelToBill("Ung Van Khiem, Long Xuyen", currentY, 9);
            currentY += lblDiaChi.Height;

            Label lblSDT = AddLabelToBill("0814 585 526", currentY, 9);
            currentY += lblSDT.Height + 5;

            currentY += 20;
            Label lblHoaDon = AddLabelToBill("HÓA ĐƠN", currentY, 12, FontStyle.Bold);
            currentY += lblHoaDon.Height + 15;

            AddLabelToBill("Tên món", currentY, 9, FontStyle.Regular, 40);     // Cột Tên
            AddLabelToBill("SL", currentY, 9, FontStyle.Regular, 250);   // Cột SL 
            AddLabelToBill("Dơn giá", currentY, 9, FontStyle.Regular, 320);    // Cột Giá
            AddLabelToBill("Thành tiền", currentY, 9, FontStyle.Regular, 450); // Cột Tổng 

            currentY += 30 // dãng cách tên cột với nd
                ;
            // --- 4. VẼ CÁC MÓN ĂN (DÙNG VÒNG LẶP) ---
            foreach (ListViewItem item in lvChiTietBill.Items) {
                string tenMon = item.SubItems[0].Text;
                string soLuong = item.SubItems[1].Text;
                string donGia = item.SubItems[2].Text;
                string thanhTien = item.SubItems[3].Text;


                // (Giả sử panelBillPreview rộng khoảng 380px)
                AddLabelToBill(tenMon, currentY, 9, FontStyle.Regular, 40);     // Cột Tên
                AddLabelToBill(soLuong, currentY, 9, FontStyle.Regular, 250);   // Cột SL 
                AddLabelToBill(donGia, currentY, 9, FontStyle.Regular, 320);    // Cột Giá
                AddLabelToBill(thanhTien, currentY, 9, FontStyle.Regular, 450); // Cột Tổng 

                currentY += 30;
            }

            // --- 5. VẼ PHẦN TỔNG TIỀN (FOOTER) ---
            currentY += 15;
            AddLabelToBill("-----------------------------------", currentY, 9);
            currentY += 55;

            AddLabelToBill("Tiền trước giảm:", currentY, 10, FontStyle.Regular, 40);
            // Dùng biến _tongTienGoc_passed mà chúng ta đã truyền qua
            AddLabelToBill(_tongTienGoc_passed.ToString("N0") + " đ", currentY, 14, FontStyle.Regular, 290);
            currentY += 35;

            AddLabelToBill("Giảm giá:", currentY, 10, FontStyle.Regular, 40);
            // Dùng biến _soTienGiam_passed mà chúng ta đã truyền qua
            AddLabelToBill("(-" + _soTienGiam_passed.ToString("N0") + " đ)", currentY, 14, FontStyle.Regular, 290);
            currentY += 35;

            AddLabelToBill("Thành tiền:", currentY, 12, FontStyle.Bold, 40);
            // Dùng biến _tongTien (là giá cuối) đã được tải từ CSDL
            AddLabelToBill(_tongTien.ToString("N0") + " đ", currentY, 14, FontStyle.Bold, 290);
            currentY += 55;

            AddLabelToBill("Xin cảm ơn quý khách!", currentY, 9, FontStyle.Italic);
            currentY += 35;
            AddLabelToBill("Hẹn gặp lại quý khách!", currentY, 9, FontStyle.Italic);
            currentY += 25;

            // --- 6. ĐẶT VỊ TRÍ MÃ QR (ĐANG BỊ ẨN) ---
            // Thêm lại QR vào panel
            panelBillPreview.Controls.Add(pbQR_InBill);
            pbQR_InBill.Top = currentY + 15;
            pbQR_InBill.Left = (panelBillPreview.Width - pbQR_InBill.Width) / 2;
        }
        #endregion

        private void txtKhachDua_TextChanged(object sender, EventArgs e) {
            decimal khachDua = 0;
            // Xóa dấu chấm (ngăn cách hàng nghìn) trước khi parse
            decimal.TryParse(txtKhachDua.Text.Replace(".", ""), out khachDua);

            decimal tienDu = khachDua - _tongTien;
            lblTienDu.Text = tienDu.ToString("N0") + " đ";
        }

        private void rbQR_CheckedChanged(object sender, EventArgs e) {
            if (rbQR.Checked) {
                pbQR_InBill.Visible = true;
                txtKhachDua.Enabled = false;
                lblTienDu.Text = "0 đ";

                try {
                    // Tải 1 ảnh QR mẫu từ VietQR (cần internet)
                    pbQR_InBill.ImageLocation = $"https://api.vietqr.io/image/970436-0909090909-snt03N5.jpg?accountName=TEST&amount={_tongTien}";
                    //pbQR_InBill.Image = Properties.Resources.QR_Code_Sample;
                }
                catch (Exception) {
                    MessageBox.Show("Lỗi khi tải mã QR. Vui lòng kiểm tra kết nối internet.");
                }
            }
            else if (rbTienMat.Checked) {
                pbQR_InBill.Visible = false;
                txtKhachDua.Enabled = true;
                // Gọi lại hàm tính tiền dư
                txtKhachDua_TextChanged(sender, e);
            }
        }

        private void btn_inhoadon_Click(object sender, EventArgs e) {

            // Bước 1: CHỈ KIỂM TRA tiền mặt
            if (rbTienMat.Checked) {
                decimal khachDua = 0;
                decimal.TryParse(txtKhachDua.Text.Replace(".", ""), out khachDua);

                if (khachDua < _tongTien) {
                    MessageBox.Show("Số tiền khách đưa không đủ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Không cho đóng form
                }
            }

            // Bước 2: LƯU CSDL (cho cả Tiền Mặt và QR)
            // Khối code này bây giờ nằm BÊN NGOÀI, nó sẽ chạy sau khi 'if (rbTienMat.Checked)' 
            try {
                // Lệnh "gọi" database
                using (DataSqlContext db = new DataSqlContext()) {
                    #region code cũ
                    /* 
                     // Bước 2.1: Tạo đối tượng DonHang
                     var donHangMoi = new DonHang {
                         NgayLap = DateTime.Now,
                         MaNv = _maNhanVien, // Dùng MaNv được truyền từ MainForm
                         TrangThai = "Da hoan thanh", // Đã thanh toán
                         TongTien = _tongTien,
                         MaKh = _maKhachHang
                     };

                     // Bước 2.2: Tạo danh sách các ChiTietDonHang
                     var listChiTiet = new List<ChiTietDonHang>();
                     foreach (ListViewItem item in _danhSachMonAn) {
                         int maSP = (int)item.Tag;
                         int soLuong = int.Parse(item.SubItems[1].Text);
                         decimal donGia = decimal.Parse(item.SubItems[2].Text.Replace(".", ""), CultureInfo.InvariantCulture);

                         var chiTiet = new ChiTietDonHang {
                             MaDhNavigation = donHangMoi, // Gán vào đơn hàng mẹ
                             MaSp = maSP,
                             SoLuong = soLuong,
                             DonGia = donGia
                         };
                         listChiTiet.Add(chiTiet);
                     }

                     // TẠO MỤC THANH TOÁN 
                     string hinhThucThanhToan = "Tiền mặt";
                     if (rbQR.Checked) {
                         hinhThucThanhToan = "Chuyển khoản QR";
                     }

                     var thanhToanMoi = new Models.ThanhToan // (Chỉ định rõ Models.ThanhToan)
                     {
                         MaDhNavigation = donHangMoi, // Gán vào đơn hàng mẹ
                         HinhThuc = hinhThucThanhToan,
                         SoTien = _tongTien,
                         NgayTt = DateTime.Now,
                         TrangThai = "Đã thanh toán"
                     };
                     db.ThanhToans.Add(thanhToanMoi);

                     // Bước 2.3: Báo cho EF Core biết chúng ta muốn...
                     db.DonHangs.Add(donHangMoi); // ...thêm 1 DonHang mới
                     db.ChiTietDonHangs.AddRange(listChiTiet); // ...thêm NHIỀU ChiTietDonHang mới

                     // Bước 2.4: Trừ kho
                     foreach (var monAn in listChiTiet) {
                         int maSP = monAn.MaSp;
                         int soLuongBan = monAn.SoLuong;

                         // Sửa lỗi "Open DataReader" bằng cách thêm .ToList()
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
                     */
                    #endregion

                    // Bước 2.1: Gán DonHang và ThanhToan vào DbContext
                    // (Báo cho EF Core biết chúng ta sắp sửa 2 đối tượng này)
                    // (Chúng ta đã lấy 2 đối tượng này ở hàm Load)
                    db.DonHangs.Attach(_donHangCanThanhToan);
                    db.ThanhToans.Attach(_thanhToanCanCapNhat);

                    // Bước 2.2: Cập nhật trạng thái
                    _donHangCanThanhToan.TrangThai = "Đã thanh toán";
                    _thanhToanCanCapNhat.TrangThai = "Đã thanh toán";

                    // Cập nhật hình thức thanh toán
                    if (rbTienMat.Checked) {
                        _thanhToanCanCapNhat.HinhThuc = "Tiền mặt";
                    }
                    else {
                        _thanhToanCanCapNhat.HinhThuc = "Chuyển khoản QR";
                    }


                    // Bước 2.5: Thực thi lệnh, lưu vào CSDL
                    db.SaveChanges(); // <-- LƯU TẤT CẢ THAY ĐỔI

                    MessageBox.Show($"Đã thanh toán thành công {_tongTien.ToString("N0")} đ", "Thông báo");

                    // Bước 2.6: Gửi tín hiệu "OK" (thành công) về cho MainForm
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi khi lưu đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
                // Nếu lỗi, không đóng form
            }
        }

        private void pbQR_InBill_Click(object sender, EventArgs e) {

        }
    }
}
