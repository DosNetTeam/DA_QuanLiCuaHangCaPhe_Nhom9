using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_QuanLi {
    public interface IKhoTruyVan {
        List<string> TaiThongBao();
        List<object> TaiDuLieuNhanVien(int thang);
        List<object> TaiDuLieuHoaDon(string timKiem, string trangThai);
        List<object> TaiDuLieuTonKho(string timKiem);
        List<object> TaiDuLieuSanPham(string timKiem);
        List<object> TaiDuLieuKhuyenMai(string timKiem, string trangThai);
    }
}
