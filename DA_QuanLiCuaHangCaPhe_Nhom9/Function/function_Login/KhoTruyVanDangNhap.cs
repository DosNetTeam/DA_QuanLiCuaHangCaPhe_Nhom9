using DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

//csharp Function/function_Login/KhoTruyVanDangNhap.cs

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Login {

    public class ThongTinTaiKhoan {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public bool? TrangThai { get; set; }
        public int MaNv { get; set; }
        public string TenNhanVien { get; set; }
        public string TenVaiTro { get; set; }
    }

    public class KhoTruyVanDangNhap {

        /// <summary>
        /// Lấy thông tin tài khoản, nhân viên, và vai trò dựa trên Tên đăng nhập.
        /// Thử EF trước, nếu lỗi thì fallback ADO.
        /// </summary>
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
                // Fallback ADO: đọc trực tiếp bằng AdoNetHelper
                try {
                    string sql = @"
                        SELECT t.TenDangNhap, t.MatKhau, t.TrangThai, t.MaNV,
                               nv.TenNV, vt.TenVaiTro
                        FROM TaiKhoan t
                        LEFT JOIN NhanVien nv ON t.MaNV = nv.MaNV
                        LEFT JOIN VaiTro vt ON t.MaVaiTro = vt.MaVaiTro
                        WHERE t.TenDangNhap = @username";

                    var p = new Dictionary<string, object> { ["@username"] = username };

                    var rows = AdoNetHelper.QueryList(sql, r => {
                        var info = new ThongTinTaiKhoan();
                        info.TenDangNhap = r.IsDBNull(0) ? string.Empty : r.GetString(0);
                        info.MatKhau = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                        // TrangThai có thể là bit/int
                        if (!r.IsDBNull(2)) {
                            try { info.TrangThai = r.GetBoolean(2); } catch { info.TrangThai = r.GetInt32(2) == 1; }
                        }
                        else info.TrangThai = null;
                        info.MaNv = r.IsDBNull(3) ? 0 : r.GetInt32(3);
                        info.TenNhanVien = r.IsDBNull(4) ? "(Không tìm thấy NV)" : r.GetString(4);
                        info.TenVaiTro = r.IsDBNull(5) ? "(Không tìm thấy VT)" : r.GetString(5);
                        return info;
                    }, p);

                    if (rows.Count == 0) return null;
                    return rows[0];
                }
                catch (Exception adoEx) {
                    Console.WriteLine($"Lỗi khi xác thực đăng nhập (EF): {ex.Message} | (ADO): {adoEx.Message}");
                    return null;
                }
            }
        }
    }
}