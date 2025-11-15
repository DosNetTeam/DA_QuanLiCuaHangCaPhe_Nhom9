using System.Collections.Generic;
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using System.Linq;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_QuanLi {
    public class EfKhoTruyVan : IKhoTruyVan {
        public List<string> TaiThongBao() {
            using var db = new DataSqlContext();
            var list = new List<string>();
            int pending = db.DonHangs.Count(d => d.TrangThai == "?ang x? lý");
            if (pending > 0) list.Add($"Có {pending} ??n hàng ?ang x? lý.");
            int low = db.NguyenLieus.Count(n => n.SoLuongTon < (n.NguongCanhBao ?? 0));
            if (low > 0) list.Add($"Có {low} nguyên li?u d??i ng??ng c?nh báo.");
            list.Add("H? th?ng ho?t ??ng bình th??ng.");
            return list;
        }

        public List<object> TaiDuLieuNhanVien(int thang) {
            using var db = new DataSqlContext();
            // reuse existing EF logic from ThongKe/DichVu
            var rows = db.NhanViens.Select(nv => new { TenNV = nv.TenNv }).ToList();
            var result = new List<object>();
            foreach (var r in rows) result.Add(r);
            return result;
        }

        public List<object> TaiDuLieuHoaDon(string timKiem, string trangThai) {
            using var db = new DataSqlContext();
            var result = new List<object>();
            var q = db.DonHangs.AsQueryable();
            foreach (var dh in q.Take(100).ToList()) result.Add(dh);
            return result;
        }

        public List<object> TaiDuLieuTonKho(string timKiem) {
            using var db = new DataSqlContext();
            var result = new List<object>();
            foreach (var nl in db.NguyenLieus.ToList()) result.Add(nl);
            return result;
        }

        public List<object> TaiDuLieuSanPham(string timKiem) {
            using var db = new DataSqlContext();
            var result = new List<object>();
            foreach (var sp in db.SanPhams.ToList()) result.Add(sp);
            return result;
        }

        public List<object> TaiDuLieuKhuyenMai(string timKiem, string trangThai) {
            using var db = new DataSqlContext();
            var result = new List<object>();
            foreach (var km in db.KhuyenMais.ToList()) result.Add(km);
            return result;
        }
    }
}
