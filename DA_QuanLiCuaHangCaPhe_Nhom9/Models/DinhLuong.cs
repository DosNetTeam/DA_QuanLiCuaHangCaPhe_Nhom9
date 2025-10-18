using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models;

public partial class DinhLuong
{
    public int MaSp { get; set; }

    public int MaNl { get; set; }

    public decimal SoLuongCan { get; set; }

    public virtual NguyenLieu MaNlNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
