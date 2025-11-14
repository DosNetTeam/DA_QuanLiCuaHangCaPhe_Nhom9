using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin {
    // --- LỚP CHỨA KẾT QUẢ TRUY VẤN (DTO) ---
    public class DuLieuNhanVien {
        public int MaNv { get; set; }
        public string TenNv { get; set; }
        public string ChucVu { get; set; }
        public string SoDienThoai { get; set; }
        public string TaiKhoan { get; set; }
        public string VaiTro { get; set; }
        public string TrangThai { get; set; }
    }


    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho chức năng Quản Lý Nhân Viên và Tài Khoản.
    /// (ĐÃ VIẾT LẠI BẰNG FOREACH, KHÔNG LINQ)

    public class NhanVien_function // Đổi tên class cho khớp tên file
    {
        // Lấy danh sách nhân viên chuyển thành DTO để hiển thị ở Admin
        public List<DuLieuNhanVien> TaiDuLieuNhanVien() {
            var ketQua = new List<DuLieuNhanVien>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Tải toàn bộ NhanVien, TaiKhoan, VaiTro để join thủ công
                    var allNhanVien = db.NhanViens.ToList();
                    var allTaiKhoan = db.TaiKhoans.ToList();
                    var allVaiTro = db.VaiTros.ToList();

                    // Với từng nhân viên, tìm tài khoản (nếu có) và vai trò tương ứng
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

        // Lấy entity NhanVien kèm TaiKhoan và VaiTro navigation nếu có
        public Models.NhanVien LayChiTietNhanVien(int maNv) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    Models.NhanVien nhanVien = null;
                    foreach (var nv in db.NhanViens.ToList()) {
                        if (nv.MaNv == maNv) {
                            nhanVien = nv;
                            break;
                        }
                    }

                    if (nhanVien != null) {
                        // Tìm tài khoản liên quan và gán navigation property
                        foreach (var tk in db.TaiKhoans.ToList()) {
                            if (tk.MaNv == nhanVien.MaNv) {
                                nhanVien.TaiKhoan = tk;
                                // Nếu tìm thấy tài khoản thì load VaiTro navigation
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

        // Lấy danh sách VaiTro để fill combobox khi tạo tài khoản
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

        // Đổi mật khẩu tài khoản: trả về mã trạng thái (0=OK,2=sai MK,3=không tìm thấy,1=lỗi)
        public int DoiMatKhau(string tenDangNhap, string matKhauCu, string matKhauMoi) {
            try {
                using (var db = new DataSqlContext()) {
                    TaiKhoan taiKhoan = null;
                    foreach (var tk in db.TaiKhoans) // Lặp trên DbSet
                    {
                        if (tk.TenDangNhap == tenDangNhap) {
                            taiKhoan = tk;
                            break;
                        }
                    }

                    if (taiKhoan == null) return 3; // Không tìm thấy
                    if (taiKhoan.MatKhau != matKhauCu) return 2; // Sai mật khẩu hiện tại

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

        // Cập nhật thông tin NV (chucVu) và gán lại vai trò theo tên vai trò
        public bool CapNhatNhanVien(int maNv, string chucVuMoi, string tenVaiTroMoi) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // Tìm vai trò theo tên (string)
                    VaiTro vaiTro = null;
                    foreach (var vt in db.VaiTros.ToList()) {
                        if (vt.TenVaiTro == tenVaiTroMoi) {
                            vaiTro = vt;
                            break;
                        }
                    }
                    if (vaiTro == null) return false;

                    // Tìm nhân viên theo MaNv
                    Models.NhanVien nhanVien = null;
                    foreach (var nv in db.NhanViens) // Lặp trực tiếp
                    {
                        if (nv.MaNv == maNv) {
                            nhanVien = nv;
                            break;
                        }
                    }
                    if (nhanVien == null) return false;

                    // Cập nhật chức vụ
                    nhanVien.ChucVu = chucVuMoi;

                    // Nếu NV có tài khoản -> cập nhật MaVaiTro
                    TaiKhoan taiKhoan = null;
                    foreach (var tk in db.TaiKhoans) // Lặp trực tiếp
                    {
                        if (tk.MaNv == maNv) {
                            taiKhoan = tk;
                            break;
                        }
                    }

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

        // Tạo tài khoản mới cho MaNv: kiểm tra tồn tại username trước khi tạo
        public bool TaoTaiKhoan(int maNv, string tenDangNhap, string matKhau, int maVaiTro) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
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

        // Xóa tài khoản theo MaNv
        public bool XoaTaiKhoan(int maNv) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    TaiKhoan account = null;
                    foreach (var tk in db.TaiKhoans) // Lặp trực tiếp
                    {
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
    }
}