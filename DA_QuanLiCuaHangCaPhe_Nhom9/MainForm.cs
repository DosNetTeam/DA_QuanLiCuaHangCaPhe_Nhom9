using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization; // Cần thêm namespace này
using System.Linq; // Cần thêm namespace này
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// QUAN TRỌNG: Thêm namespace Models của dự án
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    public partial class MainForm : Form
    {

        // Tên class Context này PHẢI KHỚP với tên file Context
        // trong thư mục /Models của bạn.
        // Dựa theo ảnh chụp, tên file là "DataSet1Context.cs"
        private string _tenDbContext = "DataSqlContext"; // <-- KIỂM TRA LẠI TÊN NÀY
        private Type _contextType;

        // Giả định ID nhân viên đang đăng nhập.
        // Dữ liệu mẫu của bạn có NV 'cuong.lm' (MaNV = 3)
        private int _currentMaNV = 3;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Tìm Type của DbContext dựa trên tên
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            _contextType = assembly.GetTypes().FirstOrDefault(t =>
                t.Name == _tenDbContext &&
                t.IsSubclassOf(typeof(Microsoft.EntityFrameworkCore.DbContext))
            );

            if (_contextType == null)
            {
                MessageBox.Show($"Không thể tìm thấy DbContext với tên: '{_tenDbContext}'.\nHãy kiểm tra lại tên trong thư mục /Models và trong code.", "Lỗi EF Core", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            // Cấu hình ListView (Giả định tên là 'lvDonHang')
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

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (lvDonHang.Items.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm sản phẩm vào đơn hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = (dynamic)CreateDbContext())
                {

                    var donHangMoi = new DonHang
                    {
                        NgayLap = DateTime.Now,
                        MaNv = _currentMaNV, // Dùng MaNV = 3
                        TrangThai = "Dang xu ly"
                    };

                    decimal tongTien = 0;
                    var listChiTiet = new List<ChiTietDonHang>();
                    foreach (ListViewItem item in lvDonHang.Items)
                    {
                        int maSP = (int)item.Tag;
                        int soLuong = int.Parse(item.SubItems[1].Text);
                        decimal donGia = decimal.Parse(item.SubItems[2].Text.Replace(".", ""), CultureInfo.InvariantCulture);

                        var chiTiet = new ChiTietDonHang
                        {
                            MaDhNavigation = donHangMoi,
                            MaSp = maSP,
                            SoLuong = soLuong,
                            DonGia = donGia
                        };
                        listChiTiet.Add(chiTiet);
                        tongTien += (soLuong * donGia);
                    }

                    donHangMoi.TongTien = tongTien;

                    db.DonHangs.Add(donHangMoi);
                    db.ChiTietDonHangs.AddRange(listChiTiet);

                    db.SaveChanges(); // GHI VÀO CSDL

                    MessageBox.Show($"Đã lưu đơn hàng {donHangMoi.MaDh} thành công!", "Thông báo");

                    lvDonHang.Items.Clear();
                    CapNhatTongTien();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu đơn hàng: " + ex.InnerException?.Message ?? ex.Message);
            }
        }

        private void btnHuyDon_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc muốn hủy đơn hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                lvDonHang.Items.Clear();
                CapNhatTongTien();
            }
        }

        #region Các hàm trợ giúp (Helpers)

        // Hàm trợ giúp để tạo một instance mới của DbContext
        private Microsoft.EntityFrameworkCore.DbContext CreateDbContext()
        {
            if (_contextType == null)
                throw new InvalidOperationException("DbContext type is not found.");

            var dbContext = (Microsoft.EntityFrameworkCore.DbContext)Activator.CreateInstance(_contextType);

            if (dbContext == null)
                throw new InvalidOperationException($"Không thể tạo instance của '{_tenDbContext}'. Hãy chắc chắn bạn có constructor không tham số.");

            return dbContext;
        }

        #endregion

        #region Các hàm tải dữ liệu (Load Data - Dùng EF Core)

        // Tải các nút Loại Sản Phẩm (từ CSDL)
        private void TaiLoaiSanPham()
        {
            // Giả định bạn có FlowLayoutPanel tên là 'flpLoaiSP'
            flpLoaiSP.Controls.Clear();
            flpLoaiSP.FlowDirection = FlowDirection.TopDown;

            try
            {
                using (var db = (dynamic)CreateDbContext())
                {
                    var cacLoaiSP = ((IQueryable<SanPham>)db.SanPhams)
                                         .Select(sp => sp.LoaiSp)
                                         .Where(loai => loai != null && loai != "")
                                         .Distinct()
                                         .ToList();

                    // 1. Tạo nút "Tất Cả"
                    Button btnTatCa = new Button
                    {
                        Text = "Tất Cả",
                        Tag = "TatCa",
                        Width = flpLoaiSP.Width - 30,
                        Height = 40,
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
                            Width = flpLoaiSP.Width - 30,
                            Height = 40,
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
            // Giả định bạn có FlowLayoutPanel tên là 'flpSanPham'
            flpSanPham.Controls.Clear();

            try
            {
                using (var db = (dynamic)CreateDbContext())
                {
                    IQueryable<SanPham> query = db.SanPhams;

                    if (maLoai != "TatCa")
                    {
                        query = query.Where(sp => sp.LoaiSp == maLoai);
                    }

                    var spCanHienThi = query.Where(sp => sp.TrangThai == "Con ban").ToList();

                    foreach (var sp in spCanHienThi)
                    {
                        Button btn = new Button
                        {
                            Text = $"{sp.TenSp}\n{sp.DonGia:N0} đ",
                            Tag = sp,
                            Width = 100,
                            Height = 80,
                            Margin = new Padding(5),
                            BackColor = Color.FromArgb(255, 240, 200)
                        };
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

        private void BtnLoai_Click(object sender, EventArgs e)
        {
            string maLoai = (sender as Button).Tag.ToString();
            TaiSanPham(maLoai);
        }

        private void BtnSanPham_Click(object sender, EventArgs e)
        {
            SanPham spDuocChon = (sender as Button).Tag as SanPham;
            ThemSanPhamVaoDonHang(spDuocChon);
        }

        #endregion

        #region Các hàm logic nghiệp vụ (Business Logic)

        private void ThemSanPhamVaoDonHang(SanPham sp)
        {
            // Giả định tên ListView là 'lvDonHang'
            foreach (ListViewItem item in lvDonHang.Items)
            {
                if ((int)item.Tag == sp.MaSp)
                {
                    int soLuong = int.Parse(item.SubItems[1].Text);
                    soLuong++;
                    decimal thanhTien = soLuong * sp.DonGia;

                    item.SubItems[1].Text = soLuong.ToString();
                    item.SubItems[3].Text = thanhTien.ToString("N0");
                    CapNhatTongTien();
                    return;
                }
            }

            ListViewItem lvi = new ListViewItem(sp.TenSp);
            lvi.Tag = sp.MaSp;
            lvi.SubItems.Add("1");
            lvi.SubItems.Add(sp.DonGia.ToString("N0"));
            lvi.SubItems.Add(sp.DonGia.ToString("N0"));

            lvDonHang.Items.Add(lvi);
            CapNhatTongTien();
        }

        private void CapNhatTongTien()
        {
            decimal tongTien = 0;
            foreach (ListViewItem item in lvDonHang.Items)
            {
                tongTien += decimal.Parse(item.SubItems[3].Text.Replace(".", ""), CultureInfo.CurrentCulture);
            }
            // Giả định Label tổng tiền tên là 'lblTongCong'
            lblTongCong.Text = tongTien.ToString("N0") + " đ";
        }

        #endregion
    }
}
