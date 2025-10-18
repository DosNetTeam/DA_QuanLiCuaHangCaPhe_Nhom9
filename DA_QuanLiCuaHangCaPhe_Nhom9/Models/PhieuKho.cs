using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models;

public partial class PhieuKho
{
    public int MaPhieu { get; set; }

    public DateTime? NgayLap { get; set; }

    public string? LoaiPhieu { get; set; }

    public int MaNv { get; set; }

    public int? MaNcc { get; set; }

    public virtual ICollection<ChiTietPhieuKho> ChiTietPhieuKhos { get; set; } = new List<ChiTietPhieuKho>();

    public virtual NhaCungCap? MaNccNavigation { get; set; }

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
