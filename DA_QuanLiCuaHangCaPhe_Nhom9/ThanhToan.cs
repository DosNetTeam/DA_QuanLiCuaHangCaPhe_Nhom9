using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    public partial class ThanhToan : Form
    {
        // Biến (fields) để lưu dữ liệu được truyền từ MainForm
        private decimal _tongTien;

        private ListView.ListViewItemCollection _danhSachMonAn;


        public ThanhToan(ListView.ListViewItemCollection danhSachMonAn, decimal tongTien)
        {
            InitializeComponent();
            // Lưu dữ liệu vào các biến
            _tongTien = tongTien;
            _danhSachMonAn = danhSachMonAn;
        }

        public ThanhToan()
        {
        }

        private void pbQR_Click(object sender, EventArgs e)
        {

        }

        private void ThanhToan_Load(object sender, EventArgs e)
        {
            // --- 1. Cấu hình phần tính tiền (Bên trái) ---
            lblTongCongBill.Text = _tongTien.ToString("N0") + " đ";
            txtKhachDua.Text = _tongTien.ToString("N0");
            lblTienDu.Text = "0 đ";

            // --- 2. Đổ danh sách món ăn vào ListView 'lvChiTietBill' (Bên trái) ---
            lvChiTietBill.Items.Clear();
            // Vòng lặp foreach vẫn hoạt động bình thường với ListViewItemCollection
            foreach (ListViewItem item in _danhSachMonAn)
            {
                // Phải 'Clone' (tạo bản sao) vì item không thể ở 2 form cùng lúc
                lvChiTietBill.Items.Add((ListViewItem)item.Clone());
            }

            // --- 3. Cấu hình phần thanh toán (Bên phải) ---
            rbTienMat.Checked = true;
            pbQR_InBill.Visible = false; // Ẩn QR code bên trong bill

            // --- 4. Vẽ hóa đơn xem trước (Bên phải) ---
            HienThiBillPreview();
        }

        private Label AddLabelToBill(string text, int verticalPosition,
                                   float fontSize, FontStyle fontStyle = FontStyle.Regular,
                                   int horizontalPosition = -1)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", fontSize, fontStyle);
            lbl.AutoSize = true;
            lbl.BackColor = Color.Transparent;

            int xPosition = horizontalPosition;
            // Nếu không set vị trí X (-1), thì căn giữa
            if (xPosition == -1)
            {
                // Căn giữa label bên trong panelBillPreview
                xPosition = (panelBillPreview.Width - TextRenderer.MeasureText(text, lbl.Font).Width) / 2;
            }
            lbl.Location = new Point(xPosition, verticalPosition);

            panelBillPreview.Controls.Add(lbl);
            return lbl;
        }

        private void HienThiBillPreview()
        {
            // --- 1. XÓA SẠCH BILL CŨ ---
            // Xóa tất cả control TRỪ pbQR_InBill
            while (panelBillPreview.Controls.Count > 0)
            {
                Control c = panelBillPreview.Controls[0];
                if (c != pbQR_InBill) // Giữ lại PictureBox QR
                {
                    panelBillPreview.Controls.Remove(c);
                    c.Dispose(); // Giải phóng bộ nhớ
                }
                else
                {
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
            AddLabelToBill("SL", currentY, 9, FontStyle.Regular, 360);   // Cột SL 
            AddLabelToBill("Dơn giá", currentY, 9, FontStyle.Regular, 440);    // Cột Giá
            AddLabelToBill("Thành tiền", currentY, 9, FontStyle.Regular, 590); // Cột Tổng 

            // --- 4. VẼ CÁC MÓN ĂN (DÙNG VÒNG LẶP) ---
            foreach (ListViewItem item in _danhSachMonAn)
            {
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
            currentY += 15; // 
            AddLabelToBill("-----------------------------------", currentY, 9);
            currentY += 25; //

            // SỬA: Căn chỉnh lại vị trí X
            AddLabelToBill("Tổng cộng:", currentY, 10, FontStyle.Bold, 40);
            AddLabelToBill(_tongTien.ToString("N0") + " đ", currentY, 12, FontStyle.Bold, 290);
            currentY += 55; // 

            AddLabelToBill("Xin cảm ơn quý khách!", currentY, 9, FontStyle.Italic);
            currentY += 25; // 
            AddLabelToBill("Hẹn gặp lại quý khách!", currentY, 9, FontStyle.Italic);
            currentY += 25;

            // --- 6. ĐẶT VỊ TRÍ MÃ QR (ĐANG BỊ ẨN) ---
            // Thêm lại QR vào panel
            panelBillPreview.Controls.Add(pbQR_InBill);
            pbQR_InBill.Top = currentY + 15;
            pbQR_InBill.Left = (panelBillPreview.Width - pbQR_InBill.Width) / 2;
        }

        private void txtKhachDua_TextChanged(object sender, EventArgs e)
        {
            decimal khachDua = 0;
            // Xóa dấu chấm (ngăn cách hàng nghìn) trước khi parse
            decimal.TryParse(txtKhachDua.Text.Replace(".", ""), out khachDua);

            decimal tienDu = khachDua - _tongTien;
            lblTienDu.Text = tienDu.ToString("N0") + " đ";
        }

        private void rbQR_CheckedChanged(object sender, EventArgs e)
        {
            if (rbQR.Checked)
            {
                pbQR_InBill.Visible = true;
                txtKhachDua.Enabled = false;
                lblTienDu.Text = "0 đ";

                try
                {
                    // Tải 1 ảnh QR mẫu từ VietQR (cần internet)
                    pbQR_InBill.ImageLocation = $"https://api.vietqr.io/image/970436-0909090909-snt03N5.jpg?accountName=TEST&amount={_tongTien}";
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi khi tải mã QR. Vui lòng kiểm tra kết nối internet.");
                }
            }
            else if (rbTienMat.Checked)
            {
                pbQR_InBill.Visible = false;
                txtKhachDua.Enabled = true;
                // Gọi lại hàm tính tiền dư
                txtKhachDua_TextChanged(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra tiền khách đưa (nếu là tiền mặt)
            if (rbTienMat.Checked)
            {
                decimal khachDua = 0;
                decimal.TryParse(txtKhachDua.Text.Replace(".", ""), out khachDua);

                if (khachDua < _tongTien)
                {
                    MessageBox.Show("Số tiền khách đưa không đủ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Không cho đóng form
                }
            }

            // (Code logic in hóa đơn thật của bạn sẽ ở đây)

            // Gửi tín hiệu "OK" về cho MainForm
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void pbQR_InBill_Click(object sender, EventArgs e)
        {

        }
    }
}
