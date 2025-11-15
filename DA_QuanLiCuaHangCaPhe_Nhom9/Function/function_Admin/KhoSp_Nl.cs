using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_Admin {
    // --- LỚP CHỨA KẾT QUẢ TRUY VẤN (DTO) ---
    public class DuLieuKho {
        public int MaNl { get; set; }
        public string TenNl { get; set; }
        public string DonViTinh { get; set; }
        public decimal SoLuongTon { get; set; }
        public decimal NguongCanhBao { get; set; }
        public string TinhTrang { get; set; }
    }


    /// Lớp này chịu trách nhiệm truy vấn CSDL
    /// cho chức năng Quản Lý Kho (Nguyên Liệu).
    /// (ĐÃ VIẾT LẠI BẰNG FOREACH, KHÔNG LINQ)

    public class KhoSp_Nl {
        // --- HÀM PHỤ (HELPER) ---
        public string TinhTrangThaiKho(decimal? soLuong, decimal nguong) {
            if (soLuong == null || soLuong == 0)
                return "Hết hàng";
            if (soLuong < nguong)
                return "Cảnh báo";
            return "Dồi dào";
        }

        // --- CÁC HÀM TRUY VẤN CSDL ---

        public List<DuLieuKho> TaiDuLieuKho() {
            var inventoryWithStatus = new List<DuLieuKho>();
            try {
                using (DataSqlContext db = new DataSqlContext()) {
                    var inventory = db.NguyenLieus.ToList(); // Lấy tất cả

                    //// Tính toán tình trạng (logic gốc)
                    //foreach (var nl in inventory) {
                    //    string tinhTrang;
                    //    decimal soLuongTon = Math.Round(nl.SoLuongTon ?? 0, 2);
                    //    decimal nguongCanhBao = Math.Round(nl.NguongCanhBao ?? 0, 2);

                    //    decimal nguong = nguongCanhBao > 0 ? nguongCanhBao : 100;
                    //    decimal motPhanBa = nguong / 3;
                    //    decimal haiPhanBa = (nguong * 2) / 3;

                    //    if (soLuongTon <= 0) tinhTrang = "Hết hàng";
                    //    else if (soLuongTon <= motPhanBa) tinhTrang = "Hết hàng";
                    //    else if (soLuongTon <= haiPhanBa) tinhTrang = "Thiếu thốn";
                    //    else tinhTrang = "Dồi dào";

                    //    inventoryWithStatus.Add(new DuLieuKho {
                    //        MaNl = nl.MaNl,
                    //        TenNl = nl.TenNl,
                    //        DonViTinh = nl.DonViTinh,
                    //        SoLuongTon = soLuongTon,
                    //        NguongCanhBao = nguongCanhBao,
                    //        TinhTrang = tinhTrang
                    //   });
                    //}

                    // Sắp xếp
                    //inventoryWithStatus.Sort((a, b) => a.SoLuongTon.CompareTo(b.SoLuongTon));


                    // Tính toán tình trạng (logic mới)
                    foreach (var nl in inventory) {
                        decimal soLuongTon = Math.Round(nl.SoLuongTon ?? 0, 2);
                        decimal nguongCanhBao = Math.Round(nl.NguongCanhBao ?? 0, 2);
                        string tinhTrang = TinhTrangThaiKho(soLuongTon, nguongCanhBao);
                        inventoryWithStatus.Add(new DuLieuKho {
                            MaNl = nl.MaNl,
                            TenNl = nl.TenNl,
                            DonViTinh = nl.DonViTinh,
                            SoLuongTon = soLuongTon,
                            NguongCanhBao = nguongCanhBao,
                            TinhTrang = tinhTrang
                        });

                    }
                    // Sắp xếp
                    inventoryWithStatus.Sort((a, b) => a.SoLuongTon.CompareTo(b.SoLuongTon));
                    return inventoryWithStatus;

                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Lỗi khi tải dữ liệu kho: {ex.Message}");
            }
            return inventoryWithStatus;
        }

        public NguyenLieu LayChiTietNguyenLieu(int maNl) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
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

        public bool CapNhatNguyenLieu(int maNl, decimal soLuongMoi, string trangThaiMoi) {
            try {
                using (DataSqlContext db = new DataSqlContext()) {
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

        public string XoaNguyenLieu(List<int> maNlList)
        {
            List<string> deletedItems = new List<string>();
            List<string> failedItems = new List<string>();

            try
            {
                using (var db = new DataSqlContext())
                {
                    var allDinhLuong = db.DinhLuongs.ToList();
                    var allNguyenLieu = db.NguyenLieus.ToList();

                    foreach (int maNl in maNlList)
                    {
                        NguyenLieu ingredient = null;
                        string tenNl = "(Không rõ)";
                        foreach (var nl in allNguyenLieu)
                        {
                            if (nl.MaNl == maNl)
                            {
                                ingredient = nl;
                                tenNl = nl.TenNl;
                                break;
                            }
                        }
                        if (ingredient == null) continue;

                        // *** BẮT ĐẦU THAY ĐỔI LOGIC ***

                        // KIỂM TRA 1: Trạng thái kinh doanh
                        if (ingredient.TrangThai == "Đang kinh doanh")
                        {
                            failedItems.Add($"{tenNl} (Lỗi: Đang kinh doanh)");
                            continue; // Bỏ qua, không xóa
                        }

                        // KIỂM TRA 2: Ràng buộc công thức
                        bool isInUse = false;
                        foreach (var dl in allDinhLuong)
                        {
                            if (dl.MaNl == maNl)
                            {
                                isInUse = true;
                                break;
                            }
                        }

                        if (isInUse)
                        {
                            failedItems.Add($"{tenNl} (Lỗi: Đang dùng trong công thức)");
                        }
                        else
                        {
                            db.NguyenLieus.Remove(ingredient);
                            deletedItems.Add(tenNl);
                        }
                        // *** KẾT THÚC THAY ĐỔI LOGIC ***
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return "Lỗi nghiêm trọng khi xóa: " + ex.Message;
            }

            // Xây dựng thông báo kết quả
            System.Text.StringBuilder summary = new System.Text.StringBuilder();
            if (deletedItems.Count > 0)
            {
                summary.AppendLine($"Đã xóa thành công {deletedItems.Count} nguyên liệu.");
            }
            if (failedItems.Count > 0)
            {
                summary.AppendLine($"\nXóa thất bại {failedItems.Count} nguyên liệu:");
                summary.AppendLine(string.Join("\n", failedItems));
            }
            return summary.ToString();
        }
    }
}