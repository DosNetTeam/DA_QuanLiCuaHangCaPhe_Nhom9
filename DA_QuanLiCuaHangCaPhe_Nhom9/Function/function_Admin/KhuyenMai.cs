using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin {
    // --- LỚP CHỨA KẾT QUẢ TRUY VẤN (DTO) ---
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


    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho chức năng Quản Lý Khuyến Mãi.


    public class KhuyenMai_function // Đổi tên class cho khớp tên file
    {
        public List<DuLieuKhuyenMai> TaiDuLieuKhuyenMai() {
            var ketQua = new List<DuLieuKhuyenMai>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var khuyenMais = db.KhuyenMais.ToList();

                    // Sắp xếp
                    khuyenMais.Sort((a, b) => b.NgayBatDau.CompareTo(a.NgayBatDau));

                    // Chuyển đổi
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

        public Models.KhuyenMai LayChiTietKhuyenMai(int maKm) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
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

        public bool ThemKhuyenMai(Models.KhuyenMai km) {
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
                    Models.KhuyenMai khuyenMai = null;
                    foreach (var km in db.KhuyenMais) // Tối ưu
                    {
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
                                // Kiểm tra ràng buộc (nếu cần, nhưng code gốc không có)
                                db.KhuyenMais.Remove(km);
                                break;
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