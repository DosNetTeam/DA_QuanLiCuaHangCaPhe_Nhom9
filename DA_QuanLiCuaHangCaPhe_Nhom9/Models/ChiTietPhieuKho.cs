using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models;

public partial class ChiTietPhieuKho
{
    public int MaPhieu { get; set; }

    public int MaNl { get; set; }

    public decimal SoLuong { get; set; }

    public decimal? GiaNhap { get; set; }

    public virtual NguyenLieu MaNlNavigation { get; set; } = null!;

    public virtual PhieuKho MaPhieuNavigation { get; set; } = null!;
}
