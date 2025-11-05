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
using DA_QuanLiCuaHangCaPhe_Nhom9.Models; // Đảm bảo bạn đã có using này
using Microsoft.EntityFrameworkCore; // Đây là using đúng cho EF Core
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    public partial class QuanLi : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CoffeeDB"]?.ConnectionString;

        public QuanLi()
        {
            InitializeComponent();

            // Gắn các sự kiện bổ sung
            this.Load += QuanLi_Load;
            txtTimKiemHD.TextChanged += txtTimKiemHD_TextChanged;
            cbTrangThaiHD.SelectedIndexChanged += cbTrangThaiHD_SelectedIndexChanged;
            txtTimKiemKho.TextChanged += txtTimKiemKho_TextChanged;
            btnThemMoiKho.Click += btnThemMoiKho_Click;

            // Gắn CellFormatting để tô màu các cột trạng thái / hiệu suất
            dgvPerformance.CellFormatting += dgvPerformance_CellFormatting;
            dgvHoaDon.CellFormatting += dgvHoaDon_CellFormatting;
            dgvTonKho.CellFormatting += dgvTonKho_CellFormatting;
        }

        private void QuanLi_Load(object sender, EventArgs e)
        {
            // Cài đặt các ComboBox lọc
            SetupFilters();

            // Tải dữ liệu ban đầu
            LoadData_NhanVien();
            LoadData_HoaDon();
            LoadData_TonKho();
        }

        // Cài đặt giá trị ban đầu cho các ComboBox lọc
        private void SetupFilters()
        {
            // Cài đặt cho ComboBox Lọc Tháng
            cbThang.Items.Clear();
            cbThang.Items.Add("Tất cả");
            for (int i = 1; i <= 12; i++)
            {
                cbThang.Items.Add($"Tháng {i}");
            }
            cbThang.SelectedIndex = 0;

            // Cài đặt cho ComboBox Trạng thái Hóa Đơn
            cbTrangThaiHD.Items.Clear();
            cbTrangThaiHD.Items.Add("Tất cả");
            cbTrangThaiHD.Items.Add("Đang xu ly");
            cbTrangThaiHD.Items.Add("Đã thanh toán");
            cbTrangThaiHD.Items.Add("Đã hủy");
            cbTrangThaiHD.SelectedIndex = 0;

            // Placeholder text handling (nếu cần)
            if (string.IsNullOrWhiteSpace(txtTimKiemHD.Text))
                txtTimKiemHD.ForeColor = Color.Gray;
            if (string.IsNullOrWhiteSpace(txtTimKiemKho.Text))
                txtTimKiemKho.ForeColor = Color.Gray;
        }

        #region Các hàm tải dữ liệu (Load Data - Dùng EF Core)

        /// <summary>
        /// Tải và tính toán hiệu suất nhân viên.
        /// </summary>
        private void LoadData_NhanVien()
        {
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    int selectedMonth = cbThang.SelectedIndex; // 0 = Tất cả

                    var query = from nv in db.NhanViens
                                join dh in db.DonHangs
                                    .Where(d => selectedMonth == 0 || (d.NgayLap.HasValue && d.NgayLap.Value.Month == selectedMonth))
                                on nv.MaNv equals dh.MaNv into groupDonHang
                                from donHang in groupDonHang.DefaultIfEmpty()
                                group donHang by new { nv.MaNv, nv.TenNv } into g
                                select new
                                {
                                    TenNV = g.Key.TenNv,
                                    SoDon = g.Count(dh => dh != null),
                                    TongDoanhThu = g.Sum(dh => (decimal?)dh.TongTien) ?? 0
                                };

                    var finalData = query
                        .OrderByDescending(x => x.TongDoanhThu)
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.TenNV,
                            x.SoDon,
                            TongDoanhThu = x.TongDoanhThu.ToString("N0", CultureInfo.InvariantCulture) + " đ",
                            HieuSuat = TinhHieuSuat(x.TongDoanhThu)
                        })
                        .ToList();

                    dgvPerformance.DataSource = finalData;

                    if (dgvPerformance.Columns["TenNV"] != null)
                        dgvPerformance.Columns["TenNV"].HeaderText = "Tên Nhân Viên";
                    if (dgvPerformance.Columns["SoDon"] != null)
                        dgvPerformance.Columns["SoDon"].HeaderText = "Số Đơn";
                    if (dgvPerformance.Columns["TongDoanhThu"] != null)
                        dgvPerformance.Columns["TongDoanhThu"].HeaderText = "Tổng Doanh Thu";
                    if (dgvPerformance.Columns["HieuSuat"] != null)
                        dgvPerformance.Columns["HieuSuat"].HeaderText = "Hiệu Suất";

                    dgvPerformance.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        /// <summary>
        /// Tải danh sách hóa đơn, có lọc theo MaHD và Trạng Thái.
        /// </summary>
        private void LoadData_HoaDon()
        {
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    var query = from dh in db.DonHangs
                                join nv in db.NhanViens on dh.MaNv equals nv.MaNv
                                select new
                                {
                                    MaHD = dh.MaDh,
                                    NgayLap = dh.NgayLap,
                                    NhanVien = nv.TenNv,
                                    TongTienRaw = dh.TongTien ?? 0,
                                    TrangThai = dh.TrangThai
                                };

                    string timKiem = txtTimKiemHD.Text?.Trim().ToLower() ?? "";
                    string trangThai = cbTrangThaiHD.SelectedItem?.ToString() ?? "Tất cả";

                    if (!string.IsNullOrEmpty(timKiem) && timKiem != "tim kiem ma hd")
                    {
                        query = query.Where(x => x.MaHD.ToString().ToLower().Contains(timKiem));
                    }

                    if (trangThai != "Tất cả")
                    {
                        query = query.Where(x => x.TrangThai == trangThai);
                    }

                    var finalData = query
                        .OrderByDescending(x => x.NgayLap)
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.MaHD,
                            x.NgayLap,
                            x.NhanVien,
                            TongTien = x.TongTienRaw.ToString("N0", CultureInfo.InvariantCulture) + " đ",
                            x.TrangThai
                        })
                        .ToList();

                    dgvHoaDon.DataSource = finalData;
                    dgvHoaDon.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu hóa đơn: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        /// <summary>
        /// Tải danh sách tồn kho, có lọc theo Tên Nguyên Liệu.
        /// </summary>
        private void LoadData_TonKho()
        {
            try
            {
                using (DataSqlContext db = new DataSqlContext())
                {
                    var query = from nl in db.NguyenLieus
                                select new
                                {
                                    TenHang = nl.TenNl,
                                    Loai = "Nguyên Liệu",
                                    SoLuongTon = nl.SoLuongTon,
                                    DonVi = nl.DonViTinh,
                                    NguongCanhBao = nl.NguongCanhBao ?? 0
                                };

                    string timKiem = txtTimKiemKho.Text?.Trim().ToLower() ?? "";
                    if (!string.IsNullOrEmpty(timKiem) && timKiem != "tim kiem nguyen lieu")
                    {
                        query = query.Where(x => x.TenHang.ToLower().Contains(timKiem));
                    }

                    var finalData = query
                        .OrderBy(x => x.TenHang)
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.TenHang,
                            x.Loai,
                            SoLuong = $"{x.SoLuongTon} {x.DonVi}",
                            TrangThai = TinhTrangThaiKho(x.SoLuongTon, x.NguongCanhBao)
                        })
                        .ToList();

                    dgvTonKho.DataSource = finalData;
                    dgvTonKho.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu tồn kho: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        #endregion

        #region Các hàm xử lý sự kiện (Event Handlers)

        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadData_NhanVien();
        }

        private void txtTimKiemHD_TextChanged(object sender, EventArgs e)
        {
            LoadData_HoaDon();
        }

        private void cbTrangThaiHD_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData_HoaDon();
        }

        private void txtTimKiemKho_TextChanged(object sender, EventArgs e)
        {
            LoadData_TonKho();
        }

        private void btnThemMoiKho_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng 'Thêm Mới Kho' đang được phát triển!", "Thông báo");
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc muốn đăng xuất Không ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            // If a login Form1 instance was hidden earlier, reuse it; otherwise create a new one.
            var existingLogin = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (existingLogin != null)
            {
                existingLogin.Show();
                existingLogin.BringToFront();
            }
            else
            {
                var login = new Form1();
                login.StartPosition = FormStartPosition.CenterScreen;
                login.Show();
            }

            this.Close();
        }

        // (Các hàm rỗng xuất hiện trong Designer - giữ để tránh lỗi)
        private void cbThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Không làm gì ở đây - dùng nút Lọc
        }

        private void dgvPerformance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Giữ chỗ nếu muốn xử lý click hàng
        }

        #endregion

        #region Các hàm tô màu DataGridView (Cell Formatting)

        private void dgvPerformance_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvPerformance.Columns[e.ColumnIndex].Name == "HieuSuat" && e.Value != null)
            {
                string hieuSuat = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (hieuSuat)
                {
                    case "Xuất Sắc":
                        e.CellStyle.BackColor = Color.FromArgb(223, 240, 216);
                        e.CellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        break;
                    case "Tốt":
                        e.CellStyle.BackColor = Color.FromArgb(252, 248, 227);
                        e.CellStyle.ForeColor = Color.FromArgb(138, 109, 59);
                        break;
                    case "Cần Cải Thiện":
                        e.CellStyle.BackColor = Color.FromArgb(242, 222, 222);
                        e.CellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                        break;
                }
            }
        }

        private void dgvHoaDon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvHoaDon.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string trangThai = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (trangThai)
                {
                    case "Đã thanh toán":
                        e.CellStyle.BackColor = Color.FromArgb(223, 240, 216);
                        e.CellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        break;
                    case "Đang xu ly":
                        e.CellStyle.BackColor = Color.FromArgb(252, 248, 227);
                        e.CellStyle.ForeColor = Color.FromArgb(138, 109, 59);
                        break;
                    case "Đã hủy":
                        e.CellStyle.BackColor = Color.FromArgb(242, 222, 222);
                        e.CellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                        break;
                }
            }
        }

        private void dgvTonKho_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTonKho.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string trangThai = e.Value.ToString();
                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;

                switch (trangThai)
                {
                    case "Dồi dào":
                        e.CellStyle.BackColor = Color.FromArgb(223, 240, 216);
                        e.CellStyle.ForeColor = Color.FromArgb(60, 118, 61);
                        break;
                    case "Cảnh báo":
                        e.CellStyle.BackColor = Color.FromArgb(252, 248, 227);
                        e.CellStyle.ForeColor = Color.FromArgb(138, 109, 59);
                        break;
                    case "Hết hàng":
                        e.CellStyle.BackColor = Color.FromArgb(242, 222, 222);
                        e.CellStyle.ForeColor = Color.FromArgb(169, 68, 66);
                        break;
                }
            }
        }

        #endregion

        #region Các hàm phụ (Helper Functions)

        // Hàm logic để quyết định Hiệu Suất
        private string TinhHieuSuat(decimal tongDoanhThu)
        {
            if (tongDoanhThu > 500000)
                return "Xuất Sắc";
            if (tongDoanhThu > 100000)
                return "Tốt";
            return "Cần Cải Thiện";
        }

        // Hàm logic để quyết định Trạng Thái Kho
        private string TinhTrangThaiKho(decimal? soLuong, decimal nguong)
        {
            if (soLuong == null || soLuong == 0)
                return "Hết hàng";
            if (soLuong < nguong)
                return "Cảnh báo";
            return "Dồi dào";
        }

        #endregion

        private void QuanLi_Load_1(object sender, EventArgs e)
        {

        }
    }
}

