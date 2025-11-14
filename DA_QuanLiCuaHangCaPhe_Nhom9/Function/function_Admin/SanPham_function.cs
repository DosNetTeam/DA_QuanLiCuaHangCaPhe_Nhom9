using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin {
    // --- LỚP CHỨA KẾT QUẢ TRUY VẤN (DTO) ---
    public class DuLieuSanPham {
        public int MaSp { get; set; }
        public string TenSp { get; set; }
        public string LoaiSp { get; set; }
        public decimal DonGia { get; set; }
        public string DonVi { get; set; }
        public string TrangThai { get; set; }
    }

    /// <summary>
    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho chức năng Quản Lý Sản Phẩm.
    /// (ĐÃ VIẾT LẠI BẰNG FOREACH, KHÔNG LINQ)
    /// </summary>
    public class SanPham_function {
        public List<DuLieuSanPham> TaiDuLieuSanPham() {
            var ketQua = new List<DuLieuSanPham>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var products = db.SanPhams.ToList();

                    // Chuyển đổi
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

                    // Sắp xếp
                    ketQua.Sort((a, b) => string.Compare(a.TenSp, b.TenSp));
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải dữ liệu sản phẩm: {ex.Message}");
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
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi lấy chi tiết sản phẩm: {ex.Message}");
                return null;
            }
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
                    SanPham product = null;
                    foreach (var sp in db.SanPhams) // Tối ưu: không cần ToList()
                    {
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
                    SanPham product = null;
                    foreach (var sp in db.SanPhams) // Tối ưu
                    {
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
                // Kiểm tra lỗi ràng buộc
                if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE constraint")) {
                    Console.WriteLine($"Lỗi ràng buộc khi xóa SP: {ex.InnerException.Message}");
                    // (Bạn có thể ném lại lỗi hoặc trả về một mã lỗi cụ thể)
                }
                else {
                    Console.WriteLine($"Lỗi khi xóa sản phẩm: {ex.Message}");
                }
                return false;
            }
        }
    }
}