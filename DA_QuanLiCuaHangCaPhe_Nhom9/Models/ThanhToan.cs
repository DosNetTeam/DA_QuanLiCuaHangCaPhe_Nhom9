using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models;

public partial class ThanhToan
{
    public int MaTt { get; set; }

    public int MaDh { get; set; }

    public string? HinhThuc { get; set; }

    public decimal? SoTien { get; set; }

    public DateTime? NgayTt { get; set; }

    public string? TrangThai { get; set; }

    public virtual DonHang MaDhNavigation { get; set; } = null!;
}
