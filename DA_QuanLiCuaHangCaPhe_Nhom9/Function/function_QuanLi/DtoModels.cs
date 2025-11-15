using System;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function.function_QuanLi {
    public class NhanVienDto { public string TenNV { get; set; } = string.Empty; public int SoDon { get; set; } public decimal TongDoanhThu { get; set; } public string HieuSuat { get; set; } = string.Empty; }
    public class HoaDonDto { public int MaDH { get; set; } public string TenKH { get; set; } = string.Empty; public DateTime? NgayLap { get; set; } public decimal TongTien { get; set; } public string TrangThai { get; set; } = string.Empty; }
    public class TonKhoDto { public int MaNl { get; set; } public string TenNl { get; set; } = string.Empty; public string DonViTinh { get; set; } = string.Empty; public decimal SoLuongTon { get; set; } public decimal NguongCanhBao { get; set; } public string TinhTrang { get; set; } = string.Empty; }
    public class SanPhamDto { public int MaSp { get; set; } public string TenSp { get; set; } = string.Empty; public string Loai { get; set; } = string.Empty; public decimal DonGia { get; set; } public string DonVi { get; set; } = string.Empty; public string TrangThai { get; set; } = string.Empty; }
    public class KhuyenMaiDto { public int MaKm { get; set; } public string TenKM { get; set; } = string.Empty; public string Loai { get; set; } = string.Empty; public decimal GiaTri { get; set; } public DateTime NgayBatDau { get; set; } public DateTime NgayKetThuc { get; set; } public string TrangThai { get; set; } = string.Empty; }
}
