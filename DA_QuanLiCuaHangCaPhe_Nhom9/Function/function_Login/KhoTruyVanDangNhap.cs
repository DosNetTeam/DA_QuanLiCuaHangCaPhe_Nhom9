using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

// *** Namespace trỏ đến function_Login ***
namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Login {

    /// Lớp DTO (Đối tượng truyền dữ liệu)
    /// Dùng để trả về thông tin tài khoản cho Form Login

    public class ThongTinTaiKhoan {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public bool? TrangThai { get; set; }
        public int MaNv { get; set; }
        public string TenNhanVien { get; set; }
        public string TenVaiTro { get; set; }
    }


    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho chức năng Đăng nhập.
    /// (ĐÃ VIẾT LẠI BẰNG FOREACH, KHÔNG LINQ)

    public class KhoTruyVanDangNhap {

        /// Lấy thông tin tài khoản, nhân viên, và vai trò
        /// dựa trên Tên đăng nhập.
        /// Trả về null nếu không tìm thấy.

        public ThongTinTaiKhoan XacThuc(string username) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    // 1. Lấy tất cả dữ liệu thô để join thủ công
                    var allTaiKhoan = db.TaiKhoans.ToList();
                    var allNhanVien = db.NhanViens.ToList();
                    var allVaiTro = db.VaiTros.ToList();

                    // 2. Tìm tài khoản theo TenDangNhap
                    TaiKhoan taiKhoanTimThay = null;
                    foreach (var tk in allTaiKhoan) {
                        if (tk.TenDangNhap == username) {
                            taiKhoanTimThay = tk;
                            break;
                        }
                    }

                    // 3. Nếu không tìm thấy -> trả null cho caller (Loginform xử lý)
                    if (taiKhoanTimThay == null) {
                        return null;
                    }

                    // 4. Tìm tên nhân viên tương ứng (join thủ công)
                    string tenNV = "(Không tìm thấy NV)";
                    foreach (var nv in allNhanVien) {
                        if (nv.MaNv == taiKhoanTimThay.MaNv) {
                            tenNV = nv.TenNv;
                            break;
                        }
                    }

                    // 5. Tìm tên vai trò tương ứng (join thủ công)
                    string tenVaiTro = "(Không tìm thấy VT)";
                    foreach (var vt in allVaiTro) {
                        if (vt.MaVaiTro == taiKhoanTimThay.MaVaiTro) {
                            tenVaiTro = vt.TenVaiTro;
                            break;
                        }
                    }

                    // 6. Gói kết quả vào DTO và trả về
                    var thongTin = new ThongTinTaiKhoan {
                        TenDangNhap = taiKhoanTimThay.TenDangNhap,
                        MatKhau = taiKhoanTimThay.MatKhau,
                        TrangThai = taiKhoanTimThay.TrangThai,
                        MaNv = taiKhoanTimThay.MaNv,
                        TenNhanVien = tenNV,
                        TenVaiTro = tenVaiTro
                    };

                    return thongTin;
                }
            }
            catch (Exception ex) {
                // Ghi log lên console để dev trace lỗi kết nối/EF
                Console.WriteLine("Lỗi khi xác thực đăng nhập: " + ex.Message);
                return null; // Trả về null nếu có lỗi CSDL
            }
        }
    }
}