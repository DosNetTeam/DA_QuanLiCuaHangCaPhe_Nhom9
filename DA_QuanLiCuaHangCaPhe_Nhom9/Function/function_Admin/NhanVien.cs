using DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

//csharp Function/function_Admin/NhanVien.cs

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin {
    public class DuLieuNhanVien {
        public int MaNv { get; set; }
        public string TenNv { get; set; }
        public string ChucVu { get; set; }
        public string SoDienThoai { get; set; }
        public string TaiKhoan { get; set; }
        public string VaiTro { get; set; }
        public string TrangThai { get; set; }
    }

    public class NhanVien_function {
        public List<DuLieuNhanVien> TaiDuLieuNhanVien() {
            var ketQua = new List<DuLieuNhanVien>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var allNhanVien = db.NhanViens.ToList();
                    var allTaiKhoan = db.TaiKhoans.ToList();
                    var allVaiTro = db.VaiTros.ToList();

                    foreach (var nv in allNhanVien) {
                        TaiKhoan tkCuaNv = null;
                        foreach (var tk in allTaiKhoan) {
                            if (tk.MaNv == nv.MaNv) { tkCuaNv = tk; break; }
                        }

                        string tenTaiKhoan = "Chưa có";
                        string tenVaiTro = "N/A";
                        string trangThaiTK = "N/A";

                        if (tkCuaNv != null) {
                            tenTaiKhoan = tkCuaNv.TenDangNhap;
                            trangThaiTK = (tkCuaNv.TrangThai == true) ? "Hoạt động" : "Bị khóa";

                            foreach (var vt in allVaiTro) {
                                if (vt.MaVaiTro == tkCuaNv.MaVaiTro) { tenVaiTro = vt.TenVaiTro; break; }
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
            catch (Exception) {
                // Fallback ADO
                var allNv = AdoNetHelper.QueryList("SELECT MaNV, TenNV, ChucVu, SoDienThoai, TrangThai FROM NhanVien", r => {
                    return new {
                        MaNv = r.IsDBNull(0) ? 0 : r.GetInt32(0),
                        TenNv = r.IsDBNull(1) ? string.Empty : r.GetString(1),
                        ChucVu = r.IsDBNull(2) ? string.Empty : r.GetString(2),
                        SoDienThoai = r.IsDBNull(3) ? string.Empty : r.GetString(3),
                        TrangThai = r.IsDBNull(4) ? string.Empty : r.GetString(4)
                    };
                });

                var allTk = AdoNetHelper.QueryList("SELECT TenDangNhap, MaNV, MaVaiTro, TrangThai FROM TaiKhoan", r => {
                    return new {
                        TenDangNhap = r.IsDBNull(0) ? string.Empty : r.GetString(0),
                        MaNV = r.IsDBNull(1) ? 0 : r.GetInt32(1),
                        MaVaiTro = r.IsDBNull(2) ? 0 : r.GetInt32(2),
                        TrangThai = r.IsDBNull(3) ? (object)DBNull.Value : r.GetValue(3)
                    };
                });

                var allVt = AdoNetHelper.QueryList("SELECT MaVaiTro, TenVaiTro FROM VaiTro", r => {
                    return new { MaVaiTro = r.GetInt32(0), TenVaiTro = r.GetString(1) };
                });

                foreach (var nv in allNv) {
                    string tenTk = "Chưa có";
                    string tenVaiTro = "N/A";
                    string trangThai = "N/A";

                    foreach (var tk in allTk) {
                        if (tk.MaNV == nv.MaNv) {
                            tenTk = tk.TenDangNhap;
                            try { trangThai = (tk.TrangThai is bool b && b) ? "Hoạt động" : "Bị khóa"; } catch { trangThai = "N/A"; }
                            foreach (var vt in allVt) {
                                if (vt.MaVaiTro == tk.MaVaiTro) { tenVaiTro = vt.TenVaiTro; break; }
                            }
                            break;
                        }
                    }

                    ketQua.Add(new DuLieuNhanVien {
                        MaNv = nv.MaNv,
                        TenNv = nv.TenNv,
                        ChucVu = nv.ChucVu,
                        SoDienThoai = nv.SoDienThoai,
                        TaiKhoan = tenTk,
                        VaiTro = tenVaiTro,
                        TrangThai = trangThai
                    });
                }
            }
            return ketQua;
        }

        public Models.NhanVien LayChiTietNhanVien(int maNv) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    Models.NhanVien nhanVien = null;
                    foreach (var nv in db.NhanViens.ToList()) {
                        if (nv.MaNv == maNv) { nhanVien = nv; break; }
                    }

                    if (nhanVien != null) {
                        foreach (var tk in db.TaiKhoans.ToList()) {
                            if (tk.MaNv == nhanVien.MaNv) {
                                nhanVien.TaiKhoan = tk;
                                foreach (var vt in db.VaiTros.ToList()) {
                                    if (vt.MaVaiTro == tk.MaVaiTro) { tk.MaVaiTroNavigation = vt; break; }
                                }
                                break;
                            }
                        }
                    }
                    return nhanVien;
                }
            }
            catch (Exception) {
                // Fallback ADO: load NhanVien
                string sqlNv = "SELECT MaNV, TenNV, ChucVu, SoDienThoai, NgayVaoLam, TrangThai FROM NhanVien WHERE MaNV = @MaNV";
                var p = new Dictionary<string, object> { ["@MaNV"] = maNv };
                var rows = AdoNetHelper.QueryList(sqlNv, r => {
                    var nv = new Models.NhanVien();
                    nv.MaNv = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                    nv.TenNv = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                    nv.ChucVu = r.IsDBNull(2) ? null : r.GetString(2);
                    nv.SoDienThoai = r.IsDBNull(3) ? null : r.GetString(3);
                    if (!r.IsDBNull(4)) nv.NgayVaoLam = DateOnly.FromDateTime(r.GetDateTime(4));
                    nv.TrangThai = r.IsDBNull(5) ? null : r.GetString(5);
                    return nv;
                }, p);

                var nvEntity = rows.Count > 0 ? rows[0] : null;
                if (nvEntity != null) {
                    // load TaiKhoan
                    string sqlTk = "SELECT TenDangNhap, MatKhau, MaNV, MaVaiTro, TrangThai FROM TaiKhoan WHERE MaNV = @MaNV";
                    var tks = AdoNetHelper.QueryList(sqlTk, r => {
                        var tk = new TaiKhoan();
                        tk.TenDangNhap = r.IsDBNull(0) ? string.Empty : r.GetString(0);
                        tk.MatKhau = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                        tk.MaNv = r.IsDBNull(2) ? 0 : r.GetInt32(2);
                        tk.MaVaiTro = r.IsDBNull(3) ? 0 : r.GetInt32(3);
                        if (!r.IsDBNull(4)) tk.TrangThai = r.GetBoolean(4);
                        return tk;
                    }, p);

                    if (tks.Count > 0) {
                        nvEntity.TaiKhoan = tks[0];
                        // load VaiTro name
                        var vts = AdoNetHelper.QueryList("SELECT MaVaiTro, TenVaiTro FROM VaiTro WHERE MaVaiTro = @MaVT",
                            r => new VaiTro { MaVaiTro = r.GetInt32(0), TenVaiTro = r.GetString(1) },
                            new Dictionary<string, object> { ["@MaVT"] = nvEntity.TaiKhoan.MaVaiTro });
                        if (vts.Count > 0) nvEntity.TaiKhoan.MaVaiTroNavigation = vts[0];
                    }
                }
                return nvEntity;
            }
        }

        public List<VaiTro> LayDanhSachVaiTro() {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    return db.VaiTros.ToList();
                }
            }
            catch (Exception) {
                return VaiTroRepository.GetAll();
            }
        }

        public int DoiMatKhau(string tenDangNhap, string matKhauCu, string matKhauMoi) {
            try {
                using (var db = new DataSqlContext()) {
                    TaiKhoan taiKhoan = null;
                    foreach (var tk in db.TaiKhoans) {
                        if (tk.TenDangNhap == tenDangNhap) { taiKhoan = tk; break; }
                    }

                    if (taiKhoan == null) return 3;
                    if (taiKhoan.MatKhau != matKhauCu) return 2;

                    taiKhoan.MatKhau = matKhauMoi;
                    db.SaveChanges();
                    return 0;
                }
            }
            catch (Exception) {
                // ADO fallback
                string sqlCheck = "SELECT MatKhau FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap";
                var pCheck = new Dictionary<string, object> { ["@TenDangNhap"] = tenDangNhap };
                try {
                    var list = AdoNetHelper.QueryList(sqlCheck, r => r.IsDBNull(0) ? string.Empty : r.GetString(0), pCheck);
                    if (list.Count == 0) return 3;
                    if (list[0] != matKhauCu) return 2;

                    string sqlUpd = "UPDATE TaiKhoan SET MatKhau = @MatKhau WHERE TenDangNhap = @TenDangNhap";
                    AdoNetHelper.ExecuteNonQuery(sqlUpd, new Dictionary<string, object> { ["@MatKhau"] = matKhauMoi, ["@TenDangNhap"] = tenDangNhap });
                    return 0;
                }
                catch {
                    return 1;
                }
            }
        }

        public bool CapNhatNhanVien(int maNv, string chucVuMoi, string tenVaiTroMoi) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    VaiTro vaiTro = null;
                    foreach (var vt in db.VaiTros.ToList()) {
                        if (vt.TenVaiTro == tenVaiTroMoi) { vaiTro = vt; break; }
                    }
                    if (vaiTro == null) return false;

                    Models.NhanVien nhanVien = null;
                    foreach (var nv in db.NhanViens) { if (nv.MaNv == maNv) { nhanVien = nv; break; } }
                    if (nhanVien == null) return false;

                    nhanVien.ChucVu = chucVuMoi;

                    TaiKhoan taiKhoan = null;
                    foreach (var tk in db.TaiKhoans) { if (tk.MaNv == maNv) { taiKhoan = tk; break; } }

                    if (taiKhoan != null) taiKhoan.MaVaiTro = vaiTro.MaVaiTro;

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception) {
                try {
                    // find MaVaiTro
                    var vts = AdoNetHelper.QueryList("SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro = @Ten",
                        r => r.GetInt32(0), new Dictionary<string, object> { ["@Ten"] = tenVaiTroMoi });
                    if (vts.Count == 0) return false;
                    int maVT = vts[0];

                    // update NhanVien
                    AdoNetHelper.ExecuteNonQuery("UPDATE NhanVien SET ChucVu = @ChucVu WHERE MaNV = @MaNV",
                        new Dictionary<string, object> { ["@ChucVu"] = chucVuMoi, ["@MaNV"] = maNv });

                    // update TaiKhoan if exists
                    AdoNetHelper.ExecuteNonQuery("UPDATE TaiKhoan SET MaVaiTro = @MaVaiTro WHERE MaNV = @MaNV",
                        new Dictionary<string, object> { ["@MaVaiTro"] = maVT, ["@MaNV"] = maNv });

                    return true;
                }
                catch {
                    return false;
                }
            }
        }

        public bool TaoTaiKhoan(int maNv, string tenDangNhap, string matKhau, int maVaiTro) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    bool daTonTai = false;
                    foreach (var tk in db.TaiKhoans.ToList()) { if (tk.TenDangNhap == tenDangNhap) { daTonTai = true; break; } }
                    if (daTonTai) return false;

                    var newAccount = new TaiKhoan { MaNv = maNv, TenDangNhap = tenDangNhap, MatKhau = matKhau, MaVaiTro = maVaiTro, TrangThai = true };
                    db.TaiKhoans.Add(newAccount);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception) {
                return TaiKhoanRepository.Create(tenDangNhap, matKhau, maNv, maVaiTro, true);
            }
        }

        public bool XoaTaiKhoan(int maNv) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    TaiKhoan account = null;
                    foreach (var tk in db.TaiKhoans) { if (tk.MaNv == maNv) { account = tk; break; } }
                    if (account != null) { db.TaiKhoans.Remove(account); db.SaveChanges(); }
                    return true;
                }
            }
            catch (Exception) {
                try {
                    AdoNetHelper.ExecuteNonQuery("DELETE FROM TaiKhoan WHERE MaNV = @MaNV", new Dictionary<string, object> { ["@MaNV"] = maNv });
                    return true;
                }
                catch {
                    return false;
                }
            }
        }
    }
}