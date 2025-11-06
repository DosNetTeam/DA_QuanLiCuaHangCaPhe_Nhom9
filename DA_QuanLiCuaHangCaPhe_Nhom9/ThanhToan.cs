using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using System.Data;
using System.Globalization;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public partial class ThanhToan : Form {
        // Biến (fields) để lưu dữ liệu được truyền từ MainForm
        private decimal _tongTien;
        private int _maNhanVien;
        private ListView.ListViewItemCollection _danhSachMonAn;


        public ThanhToan(ListView.ListViewItemCollection danhSachMonAn, decimal tongTien, int maNhanVien) {
            InitializeComponent();
            // Lưu dữ liệu vào các biến
            _tongTien = tongTien;
            _danhSachMonAn = danhSachMonAn;

            // ----- THÊM KIỂM TRA LỖI NÀY -----
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
            // ----- KẾT THÚC KIỂM TRA -----

            // Lưu dữ liệu vào các biến
            _tongTien = tongTien;
            _danhSachMonAn = danhSachMonAn;
            _maNhanVien = maNhanVien;
        }

        private void ThanhToan_Load(object sender, EventArgs e) {
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
            foreach (ListViewItem item in _danhSachMonAn) {
                // Phải 'Clone' (tạo bản sao) vì item không thể ở 2 form cùng lúc
                lvChiTietBill.Items.Add((ListViewItem)item.Clone());
            }

            // --- 3. Cấu hình phần thanh toán (Bên phải) ---
            rbTienMat.Checked = true;


            //pbQR_InBill.Visible = false; // Ẩn QR code bên trong bill

            // (Nếu bạn dùng bản xem trước hóa đơn)
            if (this.Controls.Find("panelBillPreview", true).FirstOrDefault() is Panel panelBillPreview) {
                pbQR_InBill.Visible = false;
                HienThiBillPreview(panelBillPreview); // Vẽ hóa đơn
            }
            // (Nếu bạn dùng thiết kế cũ)
            else if (this.Controls.Find("pbQR", true).FirstOrDefault() is PictureBox pbQR) {
                pbQR.Visible = false;
            }



            // --- 4. Vẽ hóa đơn xem trước (Bên phải) ---
            //HienThiBillPreview();
        }

        #region
        private Label AddLabelToBill(string text, int verticalPosition,
                                   float fontSize, FontStyle fontStyle = FontStyle.Regular,
                                   int horizontalPosition = -1) {
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
            foreach (ListViewItem item in _danhSachMonAn) {
                string tenMon = item.SubItems[0].Text;
                string soLuong = item.SubItems[1].Text;
                string donGia = item.SubItems[2].Text;
                string thanhTien = item.SubItems[3].Text;


                // (Giả sử panelBillPreview rộng khoảng 380px)
                AddLabelToBill(tenMon, currentY, 9, FontStyle.Regular, 40);     // Cột Tên
                AddLabelToBill(soLuong, currentY, 9, FontStyle.Regular, 370);   // Cột SL 
                AddLabelToBill(donGia, currentY, 9, FontStyle.Regular, 450);    // Cột Giá
                AddLabelToBill(thanhTien, currentY, 9, FontStyle.Regular, 600); // Cột Tổng 

                currentY += 30;
            }

            // --- 5. VẼ PHẦN TỔNG TIỀN (FOOTER) ---
            currentY += 15;
            AddLabelToBill("-----------------------------------", currentY, 9);
            currentY += 25;

            // SỬA: Căn chỉnh lại vị trí X
            AddLabelToBill("Tổng cộng:", currentY, 10, FontStyle.Bold, 40);
            AddLabelToBill(_tongTien.ToString("N0") + " đ", currentY, 12, FontStyle.Bold, 290);
            currentY += 55;

            AddLabelToBill("Xin cảm ơn quý khách!", currentY, 9, FontStyle.Italic);
            currentY += 35;
            AddLabelToBill("Hẹn gặp lại quý khách!", currentY, 9, FontStyle.Italic);
            currentY += 35;

            // --- 6. ĐẶT VỊ TRÍ MÃ QR (ĐANG BỊ ẨN) ---
            // Thêm lại QR vào panel
            panelBillPreview.Controls.Add(pbQR_InBill);
            pbQR_InBill.Top = currentY + 13;
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
            //if (rbQR.Checked == true) {
            //    try {
            //        // Lệnh "gọi" database
            //        using (DataSqlContext db = new DataSqlContext()) {
            //            // Bước 1: Tạo đối tượng DonHang
            //            var donHangMoi = new DonHang {
            //                NgayLap = DateTime.Now,
            //                MaNv = _maNhanVien, // Dùng MaNv được truyền từ MainForm
            //                TrangThai = "Da hoan thanh", // Đã thanh toán
            //                TongTien = _tongTien
            //            };

            //            // Bước 2: Tạo danh sách các ChiTietDonHang
            //            // (Chúng ta *bắt buộc* phải tạo một List<> mới cho EF Core)
            //            var listChiTiet = new List<ChiTietDonHang>();

            //            // Lặp qua từng dòng trong giỏ hàng (ListView)
            //            foreach (ListViewItem item in _danhSachMonAn) {
            //                int maSP = (int)item.Tag;
            //                int soLuong = int.Parse(item.SubItems[1].Text);
            //                decimal donGia = decimal.Parse(item.SubItems[2].Text.Replace(".", ""), CultureInfo.InvariantCulture);

            //                var chiTiet = new ChiTietDonHang {
            //                    MaDhNavigation = donHangMoi, // Gán vào đơn hàng mẹ
            //                    MaSp = maSP,
            //                    SoLuong = soLuong,
            //                    DonGia = donGia
            //                };
            //                listChiTiet.Add(chiTiet);
            //            }

            //            // Bước 3: Báo cho EF Core biết chúng ta muốn...
            //            db.DonHangs.Add(donHangMoi); // ...thêm 1 DonHang mới
            //            db.ChiTietDonHangs.AddRange(listChiTiet); // ...thêm NHIỀU ChiTietDonHang mới

            //            // Lặp qua từng món trong hóa đơn (listChiTiet)
            //            foreach (var monAn in listChiTiet) {
            //                int maSP = monAn.MaSp;
            //                int soLuongBan = monAn.SoLuong;

            //                // Tìm công thức cho món ăn này (Truy vấn CSDL)
            //                // Thêm .ToList() để "đóng" DataReader 1
            //                var congThuc = db.DinhLuongs
            //                                 .Where(dl => dl.MaSp == maSP)
            //                                 .ToList();

            //                // Nếu món này có công thức
            //                if (congThuc.Count > 0) {
            //                    // Lặp qua từng nguyên liệu trong công thức
            //                    foreach (var nguyenLieuCan in congThuc) {
            //                        // Tìm nguyên liệu đó trong kho (Mở DataReader 2)
            //                        var nguyenLieuTrongKho = db.NguyenLieus
            //                                                   .FirstOrDefault(nl => nl.MaNl == nguyenLieuCan.MaNl);

            //                        // Nếu tìm thấy nguyên liệu trong kho
            //                        if (nguyenLieuTrongKho != null) {
            //                            // Tính toán lượng trừ
            //                            decimal luongCanTru = nguyenLieuCan.SoLuongCan * soLuongBan;

            //                            // Trừ kho
            //                            nguyenLieuTrongKho.SoLuongTon -= luongCanTru;
            //                        }
            //                    }
            //                }
            //            }


            //            // Bước 4: Thực thi lệnh, lưu vào CSDL
            //            db.SaveChanges();

            //            MessageBox.Show($"Đã thanh toán thanh công {_tongTien}", "Thông báo");

            //            // Bước 5: Gửi tín hiệu "OK" (thành công) về cho MainForm
            //            this.DialogResult = DialogResult.OK;
            //            this.Close();
            //        }
            //    }
            //    catch (Exception ex) {
            //        MessageBox.Show("Lỗi khi lưu đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
            //        // Nếu lỗi, không đóng form
            //    }
            //}

            //// Kiểm tra tiền khách đưa (nếu là tiền mặt)
            //if (rbTienMat.Checked) {
            //    decimal khachDua = 0;
            //    decimal.TryParse(txtKhachDua.Text.Replace(".", ""), out khachDua);

            //    if (khachDua < _tongTien) {
            //        MessageBox.Show("Số tiền khách đưa không đủ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return; // Không cho đóng form
            //    }

            //    try {
            //        // Lệnh "gọi" database
            //        using (DataSqlContext db = new DataSqlContext()) {
            //            // Bước 1: Tạo đối tượng DonHang
            //            var donHangMoi = new DonHang {
            //                NgayLap = DateTime.Now,
            //                MaNv = _maNhanVien, // Dùng MaNv được truyền từ MainForm
            //                TrangThai = "Da hoan thanh", // Đã thanh toán
            //                TongTien = _tongTien
            //            };

            //            // Bước 2: Tạo danh sách các ChiTietDonHang
            //            // (Chúng ta *bắt buộc* phải tạo một List<> mới cho EF Core)
            //            var listChiTiet = new List<ChiTietDonHang>();

            //            // Lặp qua từng dòng trong giỏ hàng (ListView)
            //            foreach (ListViewItem item in _danhSachMonAn) {
            //                int maSP = (int)item.Tag;
            //                int soLuong = int.Parse(item.SubItems[1].Text);
            //                decimal donGia = decimal.Parse(item.SubItems[2].Text.Replace(".", ""), CultureInfo.InvariantCulture);

            //                var chiTiet = new ChiTietDonHang {
            //                    MaDhNavigation = donHangMoi, // Gán vào đơn hàng mẹ
            //                    MaSp = maSP,
            //                    SoLuong = soLuong,
            //                    DonGia = donGia
            //                };
            //                listChiTiet.Add(chiTiet);
            //            }

            //            // Bước 3: Báo cho EF Core biết chúng ta muốn...
            //            db.DonHangs.Add(donHangMoi); // ...thêm 1 DonHang mới
            //            db.ChiTietDonHangs.AddRange(listChiTiet); // ...thêm NHIỀU ChiTietDonHang mới

            //            // Bước 4: Thực thi lệnh, lưu vào CSDL
            //            db.SaveChanges();

            //            MessageBox.Show($"Đã thanh toán thanh công {_tongTien}", "Thông báo");

            //            // Bước 5: Gửi tín hiệu "OK" (thành công) về cho MainForm
            //            this.DialogResult = DialogResult.OK;
            //            this.Close();
            //        }
            //    }
            //    catch (Exception ex) {
            //        MessageBox.Show("Lỗi khi lưu đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
            //        // Nếu lỗi, không đóng form
            //    }

            //}

            //// (Code logic in hóa đơn thật của bạn sẽ ở đây)

            //// Gửi tín hiệu "OK" về cho MainForm
            //this.DialogResult = DialogResult.OK;
            //this.Close();



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
            // Khối code này bây giờ nằm BÊN NGOÀI, nó sẽ chạy
            // sau khi 'if (rbTienMat.Checked)' đã qua (nếu cần)
            try {
                // Lệnh "gọi" database
                using (DataSqlContext db = new DataSqlContext()) {
                    // Bước 2.1: Tạo đối tượng DonHang
                    var donHangMoi = new DonHang {
                        NgayLap = DateTime.Now,
                        MaNv = _maNhanVien, // Dùng MaNv được truyền từ MainForm
                        TrangThai = "Da hoan thanh", // Đã thanh toán
                        TongTien = _tongTien,
                        // (Thêm MaKH nếu bạn có truyền MaKH qua)
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
