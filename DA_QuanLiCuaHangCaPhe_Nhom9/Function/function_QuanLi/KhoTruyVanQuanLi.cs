using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using global::System.Globalization;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_QuanLi {
    // --- CÁC LỚP CHỨA KẾT QUẢ TRUY VẤN (DTOs) ---
    // (Dùng để gửi dữ liệu về cho DataGridView)

    public class DuLieuNhanVien {
        public string TenNV { get; set; }
        public int SoDon { get; set; }
        public string TongDoanhThu { get; set; }
        public string HieuSuat { get; set; }
    }

    public class DuLieuHoaDon {
        public int MaHD { get; set; }
        public DateTime? NgayLap { get; set; }
        public string NhanVien { get; set; }
        public string TongTien { get; set; }
        public string TrangThai { get; set; }
    }

    public class DuLieuTonKho {
        public string TenHang { get; set; }
        public string Loai { get; set; }
        public string SoLuong { get; set; }
        public string TrangThai { get; set; }
    }

    public class DuLieuSanPham {
        public int MaSp { get; set; }
        public string TenSP { get; set; }
        public string Loai { get; set; }
        public string DonGia { get; set; }
        public string DonVi { get; set; }
        public string TrangThai { get; set; }
    }

    public class DuLieuKhuyenMai {
        public int MaKm { get; set; }
        public string TenKM { get; set; }
        public string Loai { get; set; }
        public string GiaTri { get; set; }
        public DateOnly? BatDau { get; set; }
        public DateOnly? KetThuc { get; set; }
        public string TrangThai { get; set; }
    }



    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho Form QuanLi.
    /// (ĐÃ VIẾT LẠI BẰNG FOREACH, KHÔNG LINQ)

    public class KhoTruyVanQuanLi {
        // --- CÁC HÀM PHỤ (HELPER) ĐƯỢC CHUYỂN TỪ QUANLI.CS SANG ---
        // (Đây là các hàm logic, không phải truy vấn CSDL)

        public string TinhHieuSuat(decimal tongDoanhThu) {
            if (tongDoanhThu > 500000)
                return "Xuất Sắc";
            if (tongDoanhThu > 100000)
                return "Tốt";
            return "Cần Cải Thiện";
        }

        public string TinhTrangThaiKho(decimal? soLuong, decimal nguong) {
            if (soLuong == null || soLuong == 0)
                return "Hết hàng";
            if (soLuong < nguong)
                return "Cảnh báo";
            return "Dồi dào";
        }

        public string TinhGiaTriKM(decimal? giaTri, string loai) {
            if (giaTri == null) return "N/A";
            if (loai == "HoaDon" || loai == "SanPham") {
                return $"{giaTri.Value.ToString("G29")}%";
            }
            if (loai == "GiamTien") {
                return $"{giaTri.Value.ToString("N0", CultureInfo.InvariantCulture)} đ";
            }
            return giaTri.Value.ToString("G29");
        }

        // --- CÁC HÀM TRUY VẤN CSDL (ĐÃ VIẾT LẠI BẰNG FOREACH) ---

        public List<DuLieuNhanVien> TaiDuLieuNhanVien(int selectedMonth) {
            var ketQua = new List<DuLieuNhanVien>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var allNhanVien = db.NhanViens.ToList();
                    var allDonHang = db.DonHangs.ToList();

                    // 1. Lọc NhanVien (thay .Where(nv => nv.TrangThai == "Đang làm việc"))
                    var nhanVienDangLam = new List<NhanVien>();
                    foreach (var nv in allNhanVien) {
                        if (nv.TrangThai == "Đang làm việc") {
                            nhanVienDangLam.Add(nv);
                        }
                    }

                    // 2. Lọc DonHang theo tháng (thay .Where(d => ...))
                    var donHangTrongThang = new List<DonHang>();
                    if (selectedMonth == 0) // "Tất cả"
                    {
                        donHangTrongThang = allDonHang;
                    }
                    else {
                        foreach (var dh in allDonHang) {
                            if (dh.NgayLap.HasValue && dh.NgayLap.Value.Month == selectedMonth) {
                                donHangTrongThang.Add(dh);
                            }
                        }
                    }

                    // 3. Thực hiện Group By thủ công (thay 'group by')
                    foreach (var nv in nhanVienDangLam) {
                        int soDon = 0;
                        decimal tongDoanhThu = 0;

                        // (thay 'join' và 'group')
                        foreach (var dh in donHangTrongThang) {
                            if (dh.MaNv == nv.MaNv) {
                                soDon++;
                                tongDoanhThu += (dh.TongTien ?? 0);
                            }
                        }

                        // Thêm vào kết quả (kể cả NV không có đơn)
                        ketQua.Add(new DuLieuNhanVien {
                            TenNV = nv.TenNv,
                            SoDon = soDon,
                            TongDoanhThu = tongDoanhThu.ToString("N0", CultureInfo.InvariantCulture) + " đ",
                            HieuSuat = TinhHieuSuat(tongDoanhThu)
                        });
                    }

                    // 4. Sắp xếp (thay .OrderByDescending)
                    // Dùng Sort của List<>
                    ketQua.Sort((a, b) => {
                        // Chuyển đổi TongDoanhThu (string) về decimal để so sánh
                        decimal doanhThuA = decimal.Parse(a.TongDoanhThu.Replace(" đ", "").Replace(".", ""), CultureInfo.InvariantCulture);
                        decimal doanhThuB = decimal.Parse(b.TongDoanhThu.Replace(" đ", "").Replace(".", ""), CultureInfo.InvariantCulture);
                        return doanhThuB.CompareTo(doanhThuA); // Sắp xếp giảm dần
                    });
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải dữ liệu nhân viên: " + (ex.InnerException?.Message ?? ex.Message));
            }
            return ketQua;
        }

        public List<DuLieuHoaDon> TaiDuLieuHoaDon(string timKiem, string trangThai) {
            var ketQua = new List<DuLieuHoaDon>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var allDonHang = db.DonHangs.ToList();
                    var allNhanVien = db.NhanViens.ToList();

                    // 1. Join thủ công DonHang và NhanVien
                    var dsDonHangDayDu = new List<DuLieuHoaDon>();
                    foreach (var dh in allDonHang) {
                        string tenNV = "(Không rõ)";
                        foreach (var nv in allNhanVien) {
                            if (dh.MaNv == nv.MaNv) {
                                tenNV = nv.TenNv;
                                break;
                            }
                        }

                        dsDonHangDayDu.Add(new DuLieuHoaDon {
                            MaHD = dh.MaDh,
                            NgayLap = dh.NgayLap,
                            NhanVien = tenNV,
                            TongTien = (dh.TongTien ?? 0).ToString("N0", CultureInfo.InvariantCulture) + " đ",
                            TrangThai = dh.TrangThai
                        });
                    }

                    // 2. Lọc (thay .Where)
                    if (string.IsNullOrEmpty(timKiem) && trangThai == "Tất cả") {
                        ketQua = dsDonHangDayDu; // Không lọc
                    }
                    else {
                        foreach (var x in dsDonHangDayDu) {
                            bool timKiemOk = true;
                            bool trangThaiOk = true;

                            // Lọc tìm kiếm
                            if (!string.IsNullOrEmpty(timKiem)) {
                                timKiemOk = x.MaHD.ToString().ToLower().Contains(timKiem);
                            }

                            // Lọc trạng thái
                            if (trangThai != "Tất cả") {
                                trangThaiOk = (x.TrangThai == trangThai);
                            }

                            if (timKiemOk && trangThaiOk) {
                                ketQua.Add(x);
                            }
                        }
                    }

                    // 3. Sắp xếp (thay .OrderByDescending)
                    ketQua.Sort((a, b) => b.NgayLap.GetValueOrDefault().CompareTo(a.NgayLap.GetValueOrDefault()));
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải dữ liệu hóa đơn: " + (ex.InnerException?.Message ?? ex.Message));
            }
            return ketQua;
        }

        public List<DuLieuTonKho> TaiDuLieuTonKho(string timKiem) {
            var ketQua = new List<DuLieuTonKho>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var allNguyenLieu = db.NguyenLieus.ToList();
                    var dsNguyenLieuDayDu = new List<DuLieuTonKho>();

                    // 1. Lọc và chuyển đổi (thay .Where và .Select)
                    foreach (var nl in allNguyenLieu) {
                        if (nl.TrangThai == "Đang kinh doanh") {
                            dsNguyenLieuDayDu.Add(new DuLieuTonKho {
                                TenHang = nl.TenNl,
                                Loai = "Nguyên Liệu",
                                SoLuong = $"{nl.SoLuongTon} {nl.DonViTinh}",
                                TrangThai = TinhTrangThaiKho(nl.SoLuongTon, nl.NguongCanhBao ?? 0)
                            });
                        }
                    }

                    // 2. Lọc tìm kiếm (thay .Where)
                    if (string.IsNullOrEmpty(timKiem)) {
                        ketQua = dsNguyenLieuDayDu; // Không lọc
                    }
                    else {
                        foreach (var x in dsNguyenLieuDayDu) {
                            if (x.TenHang.ToLower().Contains(timKiem)) {
                                ketQua.Add(x);
                            }
                        }
                    }

                    // 3. Sắp xếp (thay .OrderBy)
                    ketQua.Sort((a, b) => a.TenHang.CompareTo(b.TenHang));
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải dữ liệu tồn kho: " + (ex.InnerException?.Message ?? ex.Message));
            }
            return ketQua;
        }

        public List<DuLieuSanPham> TaiDuLieuSanPham(string timKiem) {
            var ketQua = new List<DuLieuSanPham>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var allSanPham = db.SanPhams.ToList();
                    var dsSanPhamDayDu = new List<DuLieuSanPham>();

                    // 1. Chuyển đổi (thay .Select)
                    foreach (var sp in allSanPham) {
                        dsSanPhamDayDu.Add(new DuLieuSanPham {
                            MaSp = sp.MaSp,
                            TenSP = sp.TenSp,
                            Loai = sp.LoaiSp,
                            DonGia = sp.DonGia.ToString("N0", CultureInfo.InvariantCulture) + " đ",
                            DonVi = sp.DonVi,
                            TrangThai = sp.TrangThai
                        });
                    }

                    // 2. Lọc tìm kiếm (thay .Where)
                    if (string.IsNullOrEmpty(timKiem)) {
                        ketQua = dsSanPhamDayDu; // Không lọc
                    }
                    else {
                        foreach (var x in dsSanPhamDayDu) {
                            if (x.TenSP.ToLower().Contains(timKiem)) {
                                ketQua.Add(x);
                            }
                        }
                    }

                    // 3. Sắp xếp (thay .OrderBy)
                    ketQua.Sort((a, b) => a.TenSP.CompareTo(b.TenSP));
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải dữ liệu sản phẩm: " + (ex.InnerException?.Message ?? ex.Message));
            }
            return ketQua;
        }

        public List<DuLieuKhuyenMai> TaiDuLieuKhuyenMai(string timKiem, string trangThai) {
            var ketQua = new List<DuLieuKhuyenMai>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var allKhuyenMai = db.KhuyenMais.ToList();
                    var dsKMDaChuyenDoi = new List<DuLieuKhuyenMai>();

                    // 1. Chuyển đổi (thay .Select)
                    foreach (var km in allKhuyenMai) {
                        dsKMDaChuyenDoi.Add(new DuLieuKhuyenMai {
                            MaKm = km.MaKm,
                            TenKM = km.TenKm,
                            Loai = km.LoaiKm,
                            GiaTri = TinhGiaTriKM(km.GiaTri, km.LoaiKm),
                            BatDau = km.NgayBatDau,
                            KetThuc = km.NgayKetThuc,
                            TrangThai = km.TrangThai
                        });
                    }

                    // 2. Lọc (thay .Where)
                    if (string.IsNullOrEmpty(timKiem) && trangThai == "Tất cả") {
                        ketQua = dsKMDaChuyenDoi; // Không lọc
                    }
                    else {
                        foreach (var x in dsKMDaChuyenDoi) {
                            bool timKiemOk = true;
                            bool trangThaiOk = true;

                            if (!string.IsNullOrEmpty(timKiem)) {
                                timKiemOk = x.TenKM.ToLower().Contains(timKiem);
                            }
                            if (trangThai != "Tất cả") {
                                trangThaiOk = (x.TrangThai == trangThai);
                            }

                            if (timKiemOk && trangThaiOk) {
                                ketQua.Add(x);
                            }
                        }
                    }

                    // 3. Sắp xếp (thay .OrderByDescending)
                    ketQua.Sort((a, b) => b.BatDau.GetValueOrDefault().CompareTo(a.BatDau.GetValueOrDefault()));
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Lỗi khi tải dữ liệu khuyến mãi: " + (ex.InnerException?.Message ?? ex.Message));
            }
            return ketQua;
        }

        public List<string> TaiThongBao() {
            var list = new List<string>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    DateTime since = DateTime.Now.AddDays(-30);
                    var allNhanVien = db.NhanViens.ToList();
                    var allDonHang = db.DonHangs.ToList();
                    var allThanhToan = db.ThanhToans.ToList();
                    var allNguyenLieu = db.NguyenLieus.ToList();

                    // 1) Nhân viên không hoạt động
                    int countInactive = 0;
                    foreach (var nv in allNhanVien) {
                        bool coDonHang = false;
                        foreach (var dh in allDonHang) {
                            if (dh.MaNv == nv.MaNv && dh.NgayLap >= since) {
                                coDonHang = true;
                                break;
                            }
                        }

                        if (!coDonHang) {
                            list.Add($"Nhân viên lâu không hoạt động: {nv.TenNv}");
                            countInactive++;
                            if (countInactive >= 5) break; // (thay .Take(5))
                        }
                    }

                    // 2) Hóa đơn chưa thanh toán
                    int countUnpaid = 0;
                    foreach (var tt in allThanhToan) {
                        if (tt.TrangThai == "Chưa thanh toán") {
                            // Cần join thủ công để lấy NgayLap
                            foreach (var dh in allDonHang) {
                                if (tt.MaDh == dh.MaDh && dh.NgayLap <= DateTime.Now.AddDays(-1)) {
                                    list.Add($"Hóa đơn chưa thanh toán: #{dh.MaDh} - {dh.NgayLap?.ToString("dd/MM/yy")}");
                                    countUnpaid++;
                                    break; // Dừng vòng lặp DonHang, đi đến ThanhToan tiếp theo
                                }
                            }
                        }
                        if (countUnpaid >= 5) break; // (thay .Take(5))
                    }

                    // 3) Hàng tồn kho sắp hết
                    int countLow = 0;
                    foreach (var nl in allNguyenLieu) {
                        if (nl.SoLuongTon <= (nl.NguongCanhBao ?? 0)) {
                            list.Add($"Hàng trong kho còn ít: {nl.TenNl} ({nl.SoLuongTon ?? 0})");
                            countLow++;
                            if (countLow >= 10) break; // (thay .Take(10))
                        }
                    }
                }
            }
            catch (Exception ex) {
                list.Add("Lỗi khi tải thông báo: " + ex.Message);
            }

            if (list.Count == 0)
                list.Add("Không có thông báo mới.");

            return list;
        }
    }
}