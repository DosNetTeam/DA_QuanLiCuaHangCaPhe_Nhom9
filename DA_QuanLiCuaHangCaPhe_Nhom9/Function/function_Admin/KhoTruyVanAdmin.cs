using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin {
    // --- CÁC LỚP CHỨA KẾT QUẢ TRUY VẤN (DTOs) ---
    // (Dùng để gửi dữ liệu về cho DataGridView)

    public class DuLieuTongQuan {
        public string Category { get; set; }
        public int Count { get; set; }
        public string Details { get; set; }
    }

    public class DuLieuNhanVien {
        public int MaNv { get; set; }
        public string TenNv { get; set; }
        public string ChucVu { get; set; }
        public string SoDienThoai { get; set; }
        public string TaiKhoan { get; set; }
        public string VaiTro { get; set; }
        public string TrangThai { get; set; }
    }

    public class DuLieuKho {
        public int MaNl { get; set; }
        public string TenNl { get; set; }
        public string DonViTinh { get; set; }
        public decimal SoLuongTon { get; set; }
        public decimal NguongCanhBao { get; set; }
        public string TinhTrang { get; set; }
    }

    public class DuLieuSanPham {
        public int MaSp { get; set; }
        public string TenSp { get; set; }
        public string LoaiSp { get; set; }
        public decimal DonGia { get; set; }
        public string DonVi { get; set; }
        public string TrangThai { get; set; }
    }

    public class DuLieuDoanhThu {
        public string Thang { get; set; }
        public int SoDonHang { get; set; }
        public decimal TongDoanhThu { get; set; }
        public decimal DoanhThuTrungBinh { get; set; }
        public decimal DonHangLonNhat { get; set; }
        public decimal DonHangNhoNhat { get; set; }
    }

    public class DuLieuKhuyenMai {
        public int MaKm { get; set; }
        public string TenKm { get; set; }
        public string MoTa { get; set; }
        public string LoaiKm { get; set; }
        public decimal GiaTri { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string TrangThai { get; set; }
    }


    /// <summary>
    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho Form Admin.
    /// (ĐÃ VIẾT LẠI BẰNG FOREACH, KHÔNG LINQ)
    /// </summary>
    public class KhoTruyVanAdmin {
        // --- 1. CÁC HÀM TẢI DỮ LIỆU (READ) ---

        public List<DuLieuTongQuan> TaiDuLieuTongQuan() {
            var overviewData = new List<DuLieuTongQuan>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Lấy dữ liệu thô
                    var allNhanVien = db.NhanViens.ToList();
                    var allSanPham = db.SanPhams.ToList();
                    var allDonHang = db.DonHangs.ToList();
                    var allNguyenLieu = db.NguyenLieus.ToList();
                    var allKhachHang = db.KhachHangs.ToList();

                    // 1. Tổng NV (thay .Count())
                    int tongNhanVien = allNhanVien.Count;

                    // 2. Tổng SP (thay .Count(s => ...))
                    int tongSanPham = 0;
                    foreach (var sp in allSanPham) {
                        if (sp.TrangThai == "Con ban" || sp.TrangThai == "Còn bán") // Thêm "Còn bán" cho chắc
                        {
                            tongSanPham++;
                        }
                    }

                    // 3. Tổng ĐH (thay .Count())
                    int tongDonHang = allDonHang.Count;

                    // 4. ĐH Hôm nay (logic gốc)
                    int donHangHomNay = 0;
                    foreach (var dh in allDonHang) {
                        if (dh.NgayLap.HasValue && dh.NgayLap.Value.Date == DateTime.Today) {
                            donHangHomNay++;
                        }
                    }

                    // 5. Tổng NL (thay .Count())
                    int tongNguyenLieu = allNguyenLieu.Count;

                    // 6. Tổng KH (thay .Count())
                    int tongKhachHang = allKhachHang.Count;

                    // Thêm vào danh sách
                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số nhân viên", Count = tongNhanVien, Details = "Nhân viên đang làm việc" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số sản phẩm", Count = tongSanPham, Details = "Sản phẩm đang bán" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Tổng số đơn hàng", Count = tongDonHang, Details = "Tất cả đơn hàng" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Đơn hàng hôm nay", Count = donHangHomNay, Details = DateTime.Today.ToString("dd/MM/yyyy") });
                    overviewData.Add(new DuLieuTongQuan { Category = "Nguyên liệu trong kho", Count = tongNguyenLieu, Details = "Tổng số loại nguyên liệu" });
                    overviewData.Add(new DuLieuTongQuan { Category = "Khách hàng", Count = tongKhachHang, Details = "Tổng số khách hàng" });
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải dữ liệu tổng quan: {ex.Message}");
            }
            return overviewData;
        }

        public List<DuLieuNhanVien> TaiDuLieuNhanVien() {
            var ketQua = new List<DuLieuNhanVien>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Lấy dữ liệu thô
                    var allNhanVien = db.NhanViens.ToList();
                    var allTaiKhoan = db.TaiKhoans.ToList();
                    var allVaiTro = db.VaiTros.ToList();

                    // Join thủ công (thay .Select và .Include)
                    foreach (var nv in allNhanVien) {
                        TaiKhoan tkCuaNv = null;
                        foreach (var tk in allTaiKhoan) {
                            if (tk.MaNv == nv.MaNv) {
                                tkCuaNv = tk;
                                break;
                            }
                        }

                        string tenTaiKhoan = "Chưa có";
                        string tenVaiTro = "N/A";
                        string trangThaiTK = "N/A";

                        if (tkCuaNv != null) {
                            tenTaiKhoan = tkCuaNv.TenDangNhap;
                            trangThaiTK = (tkCuaNv.TrangThai == true) ? "Hoạt động" : "Bị khóa";

                            foreach (var vt in allVaiTro) {
                                if (vt.MaVaiTro == tkCuaNv.MaVaiTro) {
                                    tenVaiTro = vt.TenVaiTro;
                                    break;
                                }
                            }
                        }

                        ketQua.Add(new DuLieuNhanVien {
                            MaNv = nv.MaNv,
                            TenNv = nv.TenNv,
                            ChucVu = nv.ChucVu,
                            SoDienThoai = nv.SoDienThoai,
                            TaiKhoan = tenTaiKhoan,
                            VaiTro = tenVaiTro,
                            TrangThai = trangThaiTK
                        });
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải dữ liệu nhân viên: {ex.Message}");
            }
            return ketQua;
        }

        public List<DuLieuKho> TaiDuLieuKho() {
            var inventoryWithStatus = new List<DuLieuKho>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var inventory = db.NguyenLieus.ToList(); // Lấy tất cả

                    // Tính toán tình trạng (logic gốc)
                    foreach (var nl in inventory) {
                        string tinhTrang;
                        decimal soLuongTon = Math.Round(nl.SoLuongTon ?? 0, 2);
                        decimal nguongCanhBao = Math.Round(nl.NguongCanhBao ?? 0, 2);

                        decimal nguong = nguongCanhBao > 0 ? nguongCanhBao : 100;
                        decimal motPhanBa = nguong / 3;
                        decimal haiPhanBa = (nguong * 2) / 3;

                        if (soLuongTon <= 0) tinhTrang = "Hết hàng";
                        else if (soLuongTon <= motPhanBa) tinhTrang = "Hết hàng";
                        else if (soLuongTon <= haiPhanBa) tinhTrang = "Thiếu thốn";
                        else tinhTrang = "Dồi dào";

                        inventoryWithStatus.Add(new DuLieuKho {
                            MaNl = nl.MaNl,
                            TenNl = nl.TenNl,
                            DonViTinh = nl.DonViTinh,
                            SoLuongTon = soLuongTon,
                            NguongCanhBao = nguongCanhBao,
                            TinhTrang = tinhTrang
                        });
                    }

                    // Sắp xếp (thay .OrderBy)
                    inventoryWithStatus.Sort((a, b) => a.SoLuongTon.CompareTo(b.SoLuongTon));
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải dữ liệu kho: {ex.Message}");
            }
            return inventoryWithStatus;
        }

        public List<DuLieuSanPham> TaiDuLieuSanPham() {
            var ketQua = new List<DuLieuSanPham>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var products = db.SanPhams.ToList();

                    // Chuyển đổi (thay .Select)
                    foreach (var sp in products) {
                        ketQua.Add(new DuLieuSanPham {
                            MaSp = sp.MaSp,
                            TenSp = sp.TenSp,
                            LoaiSp = sp.LoaiSp,
                            DonGia = sp.DonGia,
                            DonVi = sp.DonVi,
                            TrangThai = sp.TrangThai
                        });
                    }

                    // Sắp xếp (thay .OrderBy)
                    ketQua.Sort((a, b) => string.Compare(a.TenSp, b.TenSp));
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải dữ liệu sản phẩm: {ex.Message}");
            }
            return ketQua;
        }

        public List<DuLieuDoanhThu> TaiDuLieuDoanhThu() {
            var revenueData = new List<DuLieuDoanhThu>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Lọc DonHang (thay .Where)
                    var donHangsHopLe = new List<DonHang>();
                    foreach (var dh in db.DonHangs.ToList()) {
                        if (dh.TongTien != null && dh.NgayLap != null) {
                            donHangsHopLe.Add(dh);
                        }
                    }

                    // Nhóm thủ công (logic gốc)
                    Dictionary<string, List<DonHang>> groups = new Dictionary<string, List<DonHang>>();
                    foreach (var dh in donHangsHopLe) {
                        if (dh.NgayLap.HasValue) {
                            string key = dh.NgayLap.Value.Month.ToString("00") + "/" + dh.NgayLap.Value.Year.ToString();
                            if (!groups.ContainsKey(key)) {
                                groups[key] = new List<DonHang>();
                            }
                            groups[key].Add(dh);
                        }
                    }

                    // Tính toán (logic gốc)
                    List<DuLieuDoanhThu> tempData = new List<DuLieuDoanhThu>();
                    foreach (var group in groups) {
                        int soDonHang = group.Value.Count;
                        decimal tongDoanhThu = 0;
                        decimal maxDoanhThu = 0;
                        decimal minDoanhThu = decimal.MaxValue;

                        foreach (var dh in group.Value) {
                            decimal tien = dh.TongTien ?? 0;
                            tongDoanhThu += tien;
                            if (tien > maxDoanhThu) maxDoanhThu = tien;
                            if (tien < minDoanhThu) minDoanhThu = tien;
                        }

                        decimal trungBinh = soDonHang > 0 ? tongDoanhThu / soDonHang : 0;

                        tempData.Add(new DuLieuDoanhThu {
                            Thang = group.Key,
                            SoDonHang = soDonHang,
                            TongDoanhThu = Math.Round(tongDoanhThu, 0),
                            DoanhThuTrungBinh = Math.Round(trungBinh, 0),
                            DonHangLonNhat = Math.Round(maxDoanhThu, 0),
                            DonHangNhoNhat = (minDoanhThu == decimal.MaxValue) ? 0 : Math.Round(minDoanhThu, 0)
                        });
                    }

                    // Sắp xếp và lấy 12 (thay .OrderByDescending.Take(12))
                    tempData.Sort((a, b) => string.Compare(b.Thang, a.Thang));

                    int count = 0;
                    foreach (var item in tempData) {
                        revenueData.Add(item);
                        count++;
                        if (count >= 12) break;
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải dữ liệu doanh thu: {ex.Message}");
            }
            return revenueData;
        }

        public List<DuLieuKhuyenMai> TaiDuLieuKhuyenMai() {
            var ketQua = new List<DuLieuKhuyenMai>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var khuyenMais = db.KhuyenMais.ToList();

                    // Sắp xếp (thay .OrderByDescending)
                    khuyenMais.Sort((a, b) => b.NgayBatDau.CompareTo(a.NgayBatDau));

                    // Chuyển đổi (thay .Select)
                    foreach (var km in khuyenMais) {
                        ketQua.Add(new DuLieuKhuyenMai {
                            MaKm = km.MaKm,
                            TenKm = km.TenKm,
                            MoTa = km.MoTa,
                            LoaiKm = km.LoaiKm,
                            GiaTri = km.GiaTri,
                            NgayBatDau = km.NgayBatDau.ToDateTime(TimeOnly.MinValue),
                            NgayKetThuc = km.NgayKetThuc.ToDateTime(TimeOnly.MinValue),
                            TrangThai = km.TrangThai
                        });
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải dữ liệu khuyến mãi: {ex.Message}");
            }
            return ketQua;
        }

        // --- 2. CÁC HÀM LẤY CHI TIẾT (READ) ---

        public SanPham LayChiTietSanPham(int maSp) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Thay .Where().FirstOrDefault()
                    foreach (var sp in db.SanPhams.ToList()) {
                        if (sp.MaSp == maSp) return sp;
                    }
                    return null;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi lấy chi tiết sản phẩm: {ex.Message}");
                return null;
            }
        }

        public NhanVien LayChiTietNhanVien(int maNv) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Thay .Include()...Where().FirstOrDefault()
                    NhanVien nhanVien = null;
                    foreach (var nv in db.NhanViens.ToList()) {
                        if (nv.MaNv == maNv) {
                            nhanVien = nv;
                            break;
                        }
                    }

                    if (nhanVien != null) {
                        foreach (var tk in db.TaiKhoans.ToList()) {
                            if (tk.MaNv == nhanVien.MaNv) {
                                nhanVien.TaiKhoan = tk;
                                foreach (var vt in db.VaiTros.ToList()) {
                                    if (vt.MaVaiTro == tk.MaVaiTro) {
                                        tk.MaVaiTroNavigation = vt;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    return nhanVien;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi lấy chi tiết nhân viên: {ex.Message}");
                return null;
            }
        }

        public NguyenLieu LayChiTietNguyenLieu(int maNl) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Thay .Where().FirstOrDefault()
                    foreach (var nl in db.NguyenLieus.ToList()) {
                        if (nl.MaNl == maNl) return nl;
                    }
                    return null;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi lấy chi tiết nguyên liệu: {ex.Message}");
                return null;
            }
        }

        public KhuyenMai LayChiTietKhuyenMai(int maKm) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Thay .Find()
                    foreach (var km in db.KhuyenMais.ToList()) {
                        if (km.MaKm == maKm) return km;
                    }
                    return null;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi lấy chi tiết khuyến mãi: {ex.Message}");
                return null;
            }
        }

        public List<VaiTro> LayDanhSachVaiTro() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    return db.VaiTros.ToList();
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải vai trò: {ex.Message}");
                return new List<VaiTro>();
            }
        }

        // --- 3. CÁC HÀM GHI DỮ LIỆU (WRITE/UPDATE/DELETE) ---

        // (Hàm này trả về: 0 = OK, 1 = Lỗi, 2 = Sai MK, 3 = Không tìm thấy TK)
        public int DoiMatKhau(string tenDangNhap, string matKhauCu, string matKhauMoi) {
            try {
                using (var db = new DataSqlContext()) {
                    TaiKhoan taiKhoan = null;
                    // Thay .FirstOrDefault
                    foreach (var tk in db.TaiKhoans.ToList()) {
                        if (tk.TenDangNhap == tenDangNhap) {
                            taiKhoan = tk;
                            break;
                        }
                    }

                    if (taiKhoan == null) return 3; // Không tìm thấy
                    if (taiKhoan.MatKhau != matKhauCu) return 2; // Sai MK

                    taiKhoan.MatKhau = matKhauMoi;
                    db.SaveChanges();
                    return 0; // OK
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi đổi mật khẩu: {ex.Message}");
                return 1; // Lỗi
            }
        }

        public bool CapNhatNhanVien(int maNv, string chucVuMoi, string tenVaiTroMoi) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // 1. Tìm VaiTro (thay .Where().FirstOrDefault())
                    VaiTro vaiTro = null;
                    foreach (var vt in db.VaiTros.ToList()) {
                        if (vt.TenVaiTro == tenVaiTroMoi) {
                            vaiTro = vt;
                            break;
                        }
                    }
                    if (vaiTro == null) return false; // Lỗi không tìm thấy vai trò

                    // 2. Tìm NhanVien (thay .Include().Where().FirstOrDefault())
                    NhanVien nhanVien = null;
                    foreach (var nv in db.NhanViens.ToList()) {
                        if (nv.MaNv == maNv) {
                            nhanVien = nv;
                            break;
                        }
                    }
                    if (nhanVien == null) return false; // Lỗi không tìm thấy NV

                    // Cập nhật Chức vụ
                    nhanVien.ChucVu = chucVuMoi;

                    // 3. Tìm TaiKhoan (thay .Include())
                    TaiKhoan taiKhoan = null;
                    foreach (var tk in db.TaiKhoans.ToList()) {
                        if (tk.MaNv == maNv) {
                            taiKhoan = tk;
                            break;
                        }
                    }

                    // Cập nhật Vai trò
                    if (taiKhoan != null) {
                        taiKhoan.MaVaiTro = vaiTro.MaVaiTro;
                    }

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi cập nhật nhân viên: {ex.Message}");
                return false;
            }
        }

        public bool TaoTaiKhoan(int maNv, string tenDangNhap, string matKhau, int maVaiTro) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Kiểm tra tên đăng nhập (thay .Any())
                    bool daTonTai = false;
                    foreach (var tk in db.TaiKhoans.ToList()) {
                        if (tk.TenDangNhap == tenDangNhap) {
                            daTonTai = true;
                            break;
                        }
                    }
                    if (daTonTai) return false; // Đã tồn tại

                    var newAccount = new TaiKhoan {
                        MaNv = maNv,
                        TenDangNhap = tenDangNhap,
                        MatKhau = matKhau,
                        MaVaiTro = maVaiTro,
                        TrangThai = true
                    };

                    db.TaiKhoans.Add(newAccount);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tạo tài khoản: {ex.Message}");
                return false;
            }
        }

        public bool XoaTaiKhoan(int maNv) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Thay .FirstOrDefault()
                    TaiKhoan account = null;
                    foreach (var tk in db.TaiKhoans.ToList()) {
                        if (tk.MaNv == maNv) {
                            account = tk;
                            break;
                        }
                    }

                    if (account != null) {
                        db.TaiKhoans.Remove(account);
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi xóa tài khoản: {ex.Message}");
                return false;
            }
        }

        public bool CapNhatNguyenLieu(int maNl, decimal soLuongMoi, string trangThaiMoi) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Thay .Find()
                    NguyenLieu ingredient = null;
                    foreach (var nl in db.NguyenLieus.ToList()) {
                        if (nl.MaNl == maNl) {
                            ingredient = nl;
                            break;
                        }
                    }

                    if (ingredient == null) return false;

                    ingredient.SoLuongTon = soLuongMoi;
                    if (!string.IsNullOrEmpty(trangThaiMoi)) {
                        ingredient.TrangThai = trangThaiMoi;
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi cập nhật nguyên liệu: {ex.Message}");
                return false;
            }
        }

        public NguyenLieu ThemNguyenLieu(string tenNl, string donVi, decimal soLuongMoi, string trangThaiMoi) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    decimal nguongCanhBao = Math.Round(soLuongMoi / 4, 2);
                    if (string.IsNullOrEmpty(trangThaiMoi)) {
                        trangThaiMoi = "Đang kinh doanh";
                    }

                    NguyenLieu newIngredient = new NguyenLieu {
                        TenNl = tenNl,
                        DonViTinh = donVi,
                        SoLuongTon = soLuongMoi,
                        NguongCanhBao = nguongCanhBao,
                        TrangThai = trangThaiMoi
                    };

                    db.NguyenLieus.Add(newIngredient);
                    db.SaveChanges();
                    return newIngredient; // Trả về để UI hiển thị
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi thêm nguyên liệu: {ex.Message}");
                return null;
            }
        }

        public string XoaNguyenLieu(List<int> maNlList) {
            List<string> deletedItems = new List<string>();
            List<string> failedItems = new List<string>();

            try {
                using (var db = new DataSqlContext()) {
                    var allDinhLuong = db.DinhLuongs.ToList();
                    var allNguyenLieu = db.NguyenLieus.ToList();

                    foreach (int maNl in maNlList) {
                        // 1. Tìm nguyên liệu (thay .Find)
                        NguyenLieu ingredient = null;
                        string tenNl = "(Không rõ)";
                        foreach (var nl in allNguyenLieu) {
                            if (nl.MaNl == maNl) {
                                ingredient = nl;
                                tenNl = nl.TenNl;
                                break;
                            }
                        }
                        if (ingredient == null) continue;

                        // 2. Kiểm tra ràng buộc (thay .Any)
                        bool isInUse = false;
                        foreach (var dl in allDinhLuong) {
                            if (dl.MaNl == maNl) {
                                isInUse = true;
                                break;
                            }
                        }

                        if (isInUse) {
                            failedItems.Add($"{tenNl} (Lỗi: Đang được sử dụng trong công thức)");
                        }
                        else {
                            db.NguyenLieus.Remove(ingredient);
                            deletedItems.Add(tenNl);
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex) {
                return "Lỗi nghiêm trọng khi xóa: " + ex.Message;
            }

            // Xây dựng thông báo kết quả
            System.Text.StringBuilder summary = new System.Text.StringBuilder();
            if (deletedItems.Count > 0) {
                summary.AppendLine($"Đã xóa thành công {deletedItems.Count} nguyên liệu.");
            }
            if (failedItems.Count > 0) {
                summary.AppendLine($"\nXóa thất bại {failedItems.Count} nguyên liệu:");
                summary.AppendLine(string.Join("\n", failedItems));
            }
            return summary.ToString();
        }

        public SanPham ThemSanPham(string tenSp, string loaiSp, decimal donGia, string donVi, string trangThai) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    if (string.IsNullOrEmpty(trangThai)) {
                        trangThai = "Còn bán";
                    }

                    SanPham newProduct = new SanPham {
                        TenSp = tenSp,
                        LoaiSp = loaiSp,
                        DonGia = donGia,
                        DonVi = donVi,
                        TrangThai = trangThai
                    };

                    db.SanPhams.Add(newProduct);
                    db.SaveChanges();
                    return newProduct;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi thêm sản phẩm: {ex.Message}");
                return null;
            }
        }

        public SanPham CapNhatSanPham(int maSp, string tenSp, string loaiSp, decimal donGia, string donVi, string trangThai) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Thay .Find()
                    SanPham product = null;
                    foreach (var sp in db.SanPhams.ToList()) {
                        if (sp.MaSp == maSp) {
                            product = sp;
                            break;
                        }
                    }

                    if (product == null) return null;

                    product.TenSp = tenSp;
                    product.LoaiSp = loaiSp;
                    product.DonGia = donGia;
                    product.DonVi = donVi;
                    if (!string.IsNullOrEmpty(trangThai)) {
                        product.TrangThai = trangThai;
                    }

                    db.SaveChanges();
                    return product;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi cập nhật sản phẩm: {ex.Message}");
                return null;
            }
        }

        public bool XoaSanPham(int maSp) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Thay .Find()
                    SanPham product = null;
                    foreach (var sp in db.SanPhams.ToList()) {
                        if (sp.MaSp == maSp) {
                            product = sp;
                            break;
                        }
                    }

                    if (product == null) return false;

                    db.SanPhams.Remove(product);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi xóa sản phẩm: {ex.Message}");
                return false;
            }
        }

        public bool ThemKhuyenMai(KhuyenMai km) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    db.KhuyenMais.Add(km);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi thêm khuyến mãi: {ex.Message}");
                return false;
            }
        }

        public bool CapNhatKhuyenMai(int maKm, string ten, string moTa, string loai, decimal giaTri, DateOnly ngayBD, DateOnly ngayKT, string trangThai) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Thay .Find()
                    KhuyenMai khuyenMai = null;
                    foreach (var km in db.KhuyenMais.ToList()) {
                        if (km.MaKm == maKm) {
                            khuyenMai = km;
                            break;
                        }
                    }

                    if (khuyenMai == null) return false;

                    khuyenMai.TenKm = ten;
                    khuyenMai.MoTa = moTa;
                    khuyenMai.LoaiKm = loai;
                    khuyenMai.GiaTri = giaTri;
                    khuyenMai.NgayBatDau = ngayBD;
                    khuyenMai.NgayKetThuc = ngayKT;
                    khuyenMai.TrangThai = trangThai;

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi cập nhật khuyến mãi: {ex.Message}");
                return false;
            }
        }

        public bool XoaKhuyenMai(List<int> maKmList) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var allKMs = db.KhuyenMais.ToList();

                    foreach (int maKm in maKmList) {
                        foreach (var km in allKMs) {
                            if (km.MaKm == maKm) {
                                db.KhuyenMais.Remove(km);
                                break; // Xóa và chuyển sang mã tiếp theo
                            }
                        }
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi xóa khuyến mãi: {ex.Message}");
                return false;
            }
        }
    }
}