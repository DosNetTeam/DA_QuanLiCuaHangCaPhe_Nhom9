using System.Collections.Generic;
using DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_QuanLi {
    public class AdoKhoTruyVan : IKhoTruyVan {
        private readonly KhoTruyVanQuanLi _inner = new KhoTruyVanQuanLi();

        public List<string> TaiThongBao() => _inner.TaiThongBao();
        public List<object> TaiDuLieuNhanVien(int thang) => _inner.TaiDuLieuNhanVien(thang);
        public List<object> TaiDuLieuHoaDon(string timKiem, string trangThai) => _inner.TaiDuLieuHoaDon(timKiem, trangThai);
        public List<object> TaiDuLieuTonKho(string timKiem) => _inner.TaiDuLieuTonKho(timKiem);
        public List<object> TaiDuLieuSanPham(string timKiem) => _inner.TaiDuLieuSanPham(timKiem);
        public List<object> TaiDuLieuKhuyenMai(string timKiem, string trangThai) => _inner.TaiDuLieuKhuyenMai(timKiem, trangThai);
    }
}
