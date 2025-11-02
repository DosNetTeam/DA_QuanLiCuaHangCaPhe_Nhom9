using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    public partial class MainForm : Form
    {


        // Giả định ID nhân viên đang đăng nhập.
        private int _currentMaNV =  3;


        // Biến này sẽ lưu Mã Khách Hàng sau khi tìm thấy
        // Dấu ? có nghĩa là "nullable" (có thể rỗng,
        // rỗng = Khách vãng lai)
        private int? _currentMaKH = null;

        // Biến này lưu loại sản phẩm đang được chọn (ví dụ: "Cà phê")
        // Ban đầu mặc định là "Tất Cả"
        private string _currentMaLoai = "TatCa";

        public MainForm()
        {
            InitializeComponent();
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
        }




        #region Các hàm tải dữ liệu (Load Data - Dùng EF Core)

        // Tải các nút Loại Sản Phẩm (từ CSDL)
        private void TaiLoaiSanPham()
        {
            flpLoaiSP.Controls.Clear();
            flpLoaiSP.FlowDirection = FlowDirection.TopDown;

            try
            {

                using (DataSqlContext db = new DataSqlContext())
                // -----------------------------------------------------------------
                {
                    var cacLoaiSP = db.SanPhams
                                     .Select(sp => sp.LoaiSp)
                                     .Where(loai => loai != null && loai != "")
                                     .Distinct()
                                     .ToList();
                    // -----------------------------------------------------------------

                    // 1. Tạo nút "Tất Cả"
                    Button btnTatCa = new Button
                    {
                        Text = "Tất Cả",
                        Tag = "TatCa",
                        Width = flpLoaiSP.Width,//- 30
                        Height = 45,
                        Margin = new Padding(5),
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        BackColor = Color.LightGray
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

            try
            {

                using (DataSqlContext db = new DataSqlContext())
                // -----------------------------------------------------------------
                {
                    // Biến 'query' này sẽ lưu câu lệnh truy vấn
                    IQueryable<SanPham> query;

                    // Gán truy vấn ban đầu là lấy tất cả sản phẩm
                    query = db.SanPhams;

                    // Nếu 'maLoai' *không* phải là "TatCa"
                    if (maLoai != "TatCa")
                    {
                        // thì chúng ta thêm một điều kiện 'Where' vào câu truy vấn
                        query = query.Where(sp => sp.LoaiSp == maLoai);
                    }


                    //Lọc theo Tên SP ---
                    // Nếu ô tìm kiếm không rỗng
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        // Thêm điều kiện Where: Tên SP (chuyển chữ thường)
                        // phải chứa (Contains) chữ tìm kiếm
                        query = query.Where(sp =>
                            sp.TenSp.ToLower().Contains(searchText)
                        );
                    }

                    // Thêm một điều kiện 'Where' nữa: chỉ lấy sp "Con ban"
                    var spCanHienThi = query.Where(sp => sp.TrangThai == "Con ban").ToList();

                    // Tạo các nút sản phẩm
                    foreach (var sp in spCanHienThi)
                    {
                        Button btn = new Button
                        {
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

        #endregion

        #region Các hàm logic nghiệp vụ (Business Logic)

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

            // Lặp qua TẤT CẢ các dòng trong giỏ hàng
            foreach (ListViewItem item in lvDonHang.Items)
            {
                // Lấy giá trị của cột Thành Tiền (cột 3)
                // Phải .Replace(".", "") để xóa dấu phẩy hàng nghìn
                // ví dụ: "20.000" -> "20000"
                string chuoiThanhTien = item.SubItems[3].Text.Replace(".", "");

                // Chuyển chữ "20000" thành số 20000
                decimal thanhTien = decimal.Parse(chuoiThanhTien, CultureInfo.CurrentCulture);

                // Cộng dồn vào tổng tiền
                tongTien = tongTien + thanhTien;
            }

            // Hiển thị tổng tiền lên Label (thêm "N0" để nó tự format
            // thành "20.000 đ")
            lblTongCong.Text = tongTien.ToString("N0") + " đ";
        }

        #endregion

        // Hàm này được gọi khi bấm nút "Thanh Toán"
        // (Tên hàm _1 là do bạn double-click vào nút trong designer)
        private void btnThanhToan_Click(object sender, EventArgs e)
        {

            //ThanhToan frmThanhToan = new ThanhToan(lvDonHang.Items, decimal.Parse(lblTongCong.Text.Replace(" đ", "").Replace(".", ""), CultureInfo.InvariantCulture));
            //frmThanhToan.ShowDialog();

            // Kiểm tra xem có hàng trong giỏ chưa
            if (lvDonHang.Items.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm sản phẩm vào đơn hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Thoát hàm
            }


            // 2. Lấy tổng tiền (Phải Parse đúng cách)
            string tongTienStr = lblTongCong.Text.Replace(" đ", "").Replace(".", "");
            decimal tongTien = decimal.Parse(tongTienStr, CultureInfo.InvariantCulture);

            ThanhToan frmThanhToan = new ThanhToan(lvDonHang.Items, decimal.Parse(lblTongCong.Text.Replace(" đ", "").Replace(".", ""), CultureInfo.InvariantCulture), _currentMaNV);
            //frmThanhToan.ShowDialog();




            // 4. HIỂN THỊ và LẮNG NGHE TÍN HIỆU TRẢ VỀ
            var result = frmThanhToan.ShowDialog();

            // 5. KIỂM TRA TÍN HIỆU
            if (result == DialogResult.OK)
            {
                // Nếu form Thanh Toán báo "OK" (đã lưu CSDL thành công)
                // thì chúng ta "Làm mới hóa đơn" (như bạn muốn)
                lvDonHang.Items.Clear(); // Xóa giỏ hàng
                CapNhatTongTien(); // Cập nhật tổng tiền (về 0)
            }
            // Nếu result == DialogResult.Cancel (người dùng bấm "Hủy" hoặc nút X), 
            // thì không làm gì cả, giỏ hàng vẫn còn đó.


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
            }
        }

        private void panelCol3_Paint(object sender, PaintEventArgs e)
        {

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

        private void txtTimKiemKH_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtTimKiemSP_TextChanged(object sender, EventArgs e)
        {
            // Gọi lại hàm TaiSanPham với loại SP ta đang chọn
            TaiSanPham(_currentMaLoai);
        }

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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm khách hàng: " + ex.Message);
            }
        }
    }
}

