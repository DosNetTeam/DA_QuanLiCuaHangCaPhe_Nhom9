using DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

//csharp Function/function_Admin/SanPham_function.cs

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin {
    public class DuLieuSanPham {
        public int MaSp { get; set; }
        public string TenSp { get; set; }
        public string LoaiSp { get; set; }
        public decimal DonGia { get; set; }
        public string DonVi { get; set; }
        public string TrangThai { get; set; }
    }

    public class SanPham_function {
        public List<DuLieuSanPham> TaiDuLieuSanPham() {
            var ketQua = new List<DuLieuSanPham>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var products = db.SanPhams.ToList();
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
                    ketQua.Sort((a, b) => string.Compare(a.TenSp, b.TenSp));
                }
            }
            catch (Exception) {
                // Fallback ADO
                var list = SanPhamRepository.GetAllActive();
                foreach (var sp in list) {
                    ketQua.Add(new DuLieuSanPham {
                        MaSp = sp.MaSp,
                        TenSp = sp.TenSp,
                        LoaiSp = sp.LoaiSp,
                        DonGia = sp.DonGia,
                        DonVi = sp.DonVi,
                        TrangThai = sp.TrangThai
                    });
                }
                ketQua.Sort((a, b) => string.Compare(a.TenSp, b.TenSp));
            }
            return ketQua;
        }

        public SanPham LayChiTietSanPham(int maSp) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    foreach (var sp in db.SanPhams.ToList()) {
                        if (sp.MaSp == maSp) return sp;
                    }
                    return null;
                }
            }
            catch (Exception) {
                return SanPhamRepository.GetById(maSp);
            }
        }

        public SanPham ThemSanPham(string tenSp, string loaiSp, decimal donGia, string donVi, string trangThai) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    if (string.IsNullOrEmpty(trangThai)) trangThai = "Còn bán";
                    SanPham newProduct = new SanPham { TenSp = tenSp, LoaiSp = loaiSp, DonGia = donGia, DonVi = donVi, TrangThai = trangThai };
                    db.SanPhams.Add(newProduct);
                    db.SaveChanges();
                    return newProduct;
                }
            }
            catch (Exception) {
                // ADO fallback: simple insert and return null (caller handles)
                string sql = @"INSERT INTO SanPham (TenSP, LoaiSP, DonGia, DonVi, TrangThai)
                               VALUES (@TenSP, @LoaiSP, @DonGia, @DonVi, @TrangThai)";
                var p = new Dictionary<string, object> {
                    ["@TenSP"] = tenSp,
                    ["@LoaiSP"] = loaiSp,
                    ["@DonGia"] = donGia,
                    ["@DonVi"] = donVi,
                    ["@TrangThai"] = string.IsNullOrEmpty(trangThai) ? "Còn bán" : trangThai
                };
                try {
                    AdoNetHelper.ExecuteNonQuery(sql, p);
                    // return repository read (best-effort)
                    var inserted = SanPhamRepository.GetAllActive();
                    // try find by name
                    foreach (var s in inserted) if (s.TenSp == tenSp) return s;
                }
                catch { }
                return null;
            }
        }

        public SanPham CapNhatSanPham(int maSp, string tenSp, string loaiSp, decimal donGia, string donVi, string trangThai) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    SanPham product = null;
                    foreach (var sp in db.SanPhams) { if (sp.MaSp == maSp) { product = sp; break; } }
                    if (product == null) return null;
                    product.TenSp = tenSp;
                    product.LoaiSp = loaiSp;
                    product.DonGia = donGia;
                    product.DonVi = donVi;
                    if (!string.IsNullOrEmpty(trangThai)) product.TrangThai = trangThai;
                    db.SaveChanges();
                    return product;
                }
            }
            catch (Exception) {
                try {
                    string sql = @"UPDATE SanPham SET TenSP=@TenSP, LoaiSP=@LoaiSP, DonGia=@DonGia, DonVi=@DonVi, TrangThai=@TrangThai WHERE MaSP=@MaSP";
                    var p = new Dictionary<string, object> {
                        ["@TenSP"] = tenSp, ["@LoaiSP"] = loaiSp, ["@DonGia"] = donGia, ["@DonVi"] = donVi, ["@TrangThai"] = trangThai, ["@MaSP"] = maSp
                    };
                    AdoNetHelper.ExecuteNonQuery(sql, p);
                    return SanPhamRepository.GetById(maSp);
                }
                catch {
                    return null;
                }
            }
        }

        public bool XoaSanPham(int maSp) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    SanPham product = null;
                    foreach (var sp in db.SanPhams) { if (sp.MaSp == maSp) { product = sp; break; } }
                    if (product == null) return false;
                    db.SanPhams.Remove(product);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception) {
                try {
                    AdoNetHelper.ExecuteNonQuery("DELETE FROM SanPham WHERE MaSP = @MaSP", new Dictionary<string, object> { ["@MaSP"] = maSp });
                    return true;
                }
                catch {
                    return false;
                }
            }
        }
    }
}