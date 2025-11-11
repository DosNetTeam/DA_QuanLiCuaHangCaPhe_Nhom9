using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models;

public partial class NguyenLieu
{
    public int MaNl { get; set; }

    public string TenNl { get; set; } = null!;

    public string DonViTinh { get; set; } = null!;

    public decimal? SoLuongTon { get; set; }

    public decimal? NguongCanhBao { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual ICollection<ChiTietPhieuKho> ChiTietPhieuKhos { get; set; } = new List<ChiTietPhieuKho>();

    public virtual ICollection<DinhLuong> DinhLuongs { get; set; } = new List<DinhLuong>();
}
