using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models;

public partial class SanPham
{
    public int MaSp { get; set; }

    public string TenSp { get; set; } = null!;

    public string? LoaiSp { get; set; }

    public decimal DonGia { get; set; }

    public string? DonVi { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<DinhLuong> DinhLuongs { get; set; } = new List<DinhLuong>();

    public virtual ICollection<KhuyenMai> MaKms { get; set; } = new List<KhuyenMai>();
}
