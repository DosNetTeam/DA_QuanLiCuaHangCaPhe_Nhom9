namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models;

public partial class NguyenLieu {
    public int MaNl { get; set; }

    public string TenNl { get; set; } = null!;

    public string DonViTinh { get; set; } = null!;

    public decimal? SoLuongTon { get; set; }

    public decimal? NguongCanhBao { get; set; }

    // ========== ĐÁNH DẤU [NotMapped] ĐỂ EF CORE BỎ QUA CỘT NÀY ==========
    [NotMapped]
    public string TrangThai { get; set; } = "Đang hoạt động";

    public virtual ICollection<ChiTietPhieuKho> ChiTietPhieuKhos { get; set; } = new List<ChiTietPhieuKho>();

    public virtual ICollection<DinhLuong> DinhLuongs { get; set; } = new List<DinhLuong>();
}
