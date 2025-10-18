using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models;

public partial class DonHang
{
    public int MaDh { get; set; }

    public DateTime? NgayLap { get; set; }

    public int? MaKh { get; set; }

    public int MaNv { get; set; }

    public decimal? TongTien { get; set; }

    public string? TrangThai { get; set; }

    public int? MaKm { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual KhuyenMai? MaKmNavigation { get; set; }

    public virtual NhanVien MaNvNavigation { get; set; } = null!;

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
}
