using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models;

public partial class NhanVien
{
    public int MaNv { get; set; }

    public string TenNv { get; set; } = null!;

    public string? ChucVu { get; set; }

    public string? SoDienThoai { get; set; }

    public DateOnly NgayVaoLam { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<PhieuKho> PhieuKhos { get; set; } = new List<PhieuKho>();

    public virtual TaiKhoan? TaiKhoan { get; set; }
}
