using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

//csharp DataAccess/DonHangRepository.cs
using Microsoft.Data.SqlClient;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess {
    public static class DonHangRepository {
        // Insert DonHang + ChiTietDonHang + ThanhToan and update NguyenLieu quantities according to DinhLuong.
        // Returns newly created MaDH or -1 if failed.
        public static int InsertOrder(List<(int MaSP, int SoLuong, decimal DonGia)> items, decimal tongTien, int maNv, int? maKh) {
            var pair = AdoNetHelper.BeginTransaction();
            SqlConnection conn = pair.conn;
            SqlTransaction tx = pair.tx;

            try {
                // 1. Insert DonHang
                string sqlInsertDonHang = @"INSERT INTO DonHang (NgayLap, MaKH, MaNV, TongTien, TrangThai) 
                                           VALUES (@NgayLap, @MaKH, @MaNV, @TongTien, @TrangThai);
                                           SELECT CAST(SCOPE_IDENTITY() AS int);";
                var pDh = new Dictionary<string, object> {
                    ["@NgayLap"] = DateTime.Now,
                    ["@MaKH"] = maKh.HasValue ? (object)maKh.Value : DBNull.Value,
                    ["@MaNV"] = maNv,
                    ["@TongTien"] = tongTien,
                    ["@TrangThai"] = "Đang xử lý"
                };

                int newMaDh;
                using (var cmd = conn.CreateCommand()) {
                    cmd.Transaction = tx;
                    cmd.CommandText = sqlInsertDonHang;
                    foreach (var kv in pDh) cmd.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
                    var idObj = cmd.ExecuteScalar();
                    newMaDh = Convert.ToInt32(idObj);
                }

                // 2. Insert ChiTietDonHang rows
                string sqlInsertCT = @"INSERT INTO ChiTietDonHang (MaDH, MaSP, SoLuong, DonGia) VALUES (@MaDH, @MaSP, @SoLuong, @DonGia)";
                foreach (var it in items) {
                    var p = new Dictionary<string, object> {
                        ["@MaDH"] = newMaDh,
                        ["@MaSP"] = it.MaSP,
                        ["@SoLuong"] = it.SoLuong,
                        ["@DonGia"] = it.DonGia
                    };
                    AdoNetHelper.ExecuteNonQuery(tx, sqlInsertCT, p);
                }

                // 3. Insert ThanhToan (Chưa thanh toán)
                string sqlInsertTT = @"INSERT INTO ThanhToan (MaDH, HinhThuc, SoTien, NgayTT, TrangThai) 
                                       VALUES (@MaDH, NULL, @SoTien, @NgayTT, @TrangThai)";
                var pTt = new Dictionary<string, object> {
                    ["@MaDH"] = newMaDh,
                    ["@SoTien"] = tongTien,
                    ["@NgayTT"] = DateTime.Now,
                    ["@TrangThai"] = "Chưa thanh toán"
                };
                AdoNetHelper.ExecuteNonQuery(tx, sqlInsertTT, pTt);

                // 4. Trừ kho: cho mỗi item -> lấy DinhLuong và trừ NguyenLieu
                string sqlDinhLuong = "SELECT MaNL, SoLuongCan FROM DinhLuong WHERE MaSP = @MaSP";
                string sqlUpdateNL = "UPDATE NguyenLieu SET SoLuongTon = SoLuongTon - @SoLuongTru WHERE MaNL = @MaNL";
                foreach (var it in items) {
                    var pdl = new Dictionary<string, object> { ["@MaSP"] = it.MaSP };
                    var dlList = AdoNetHelper.QueryList(sqlDinhLuong, r => {
                        return (MaNL: r.IsDBNull(0) ? 0 : r.GetInt32(0), SoLuongCan: r.IsDBNull(1) ? 0m : r.GetDecimal(1));
                    }, pdl);

                    foreach (var dl in dlList) {
                        var pUpd = new Dictionary<string, object> { ["@SoLuongTru"] = dl.SoLuongCan * it.SoLuong, ["@MaNL"] = dl.MaNL };
                        AdoNetHelper.ExecuteNonQuery(tx, sqlUpdateNL, pUpd);
                    }
                }

                tx.Commit();
                conn.Close();
                return newMaDh;
            }
            catch {
                try { tx.Rollback(); } catch { }
                try { conn.Close(); } catch { }
                return -1;
            }
            finally {
                try { tx.Dispose(); } catch { }
                try { conn.Dispose(); } catch { }
            }
        }

        // Load DonHang by id (minimal fields) using ADO
        public static DonHang? GetById(int maDh) {
            string sql = "SELECT MaDH, NgayLap, MaKH, MaNV, TongTien, TrangThai, MaKM FROM DonHang WHERE MaDH = @MaDH";
            var p = new Dictionary<string, object> { ["@MaDH"] = maDh };
            var list = AdoNetHelper.QueryList(sql, r => {
                var dh = new DonHang();
                dh.MaDh = r.IsDBNull(0) ? 0 : r.GetInt32(0);
                dh.NgayLap = r.IsDBNull(1) ? null : (DateTime?)r.GetDateTime(1);
                dh.MaKh = r.IsDBNull(2) ? null : (int?)r.GetInt32(2);
                dh.MaNv = r.IsDBNull(3) ? 0 : r.GetInt32(3);
                dh.TongTien = r.IsDBNull(4) ? null : (decimal?)r.GetDecimal(4);
                dh.TrangThai = r.IsDBNull(5) ? null : r.GetString(5);
                dh.MaKm = r.IsDBNull(6) ? null : (int?)r.GetInt32(6);
                return dh;
            }, p);
            return list.Count > 0 ? list[0] : null;
        }
    }
}