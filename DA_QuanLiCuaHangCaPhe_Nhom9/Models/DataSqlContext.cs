using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Models;

public partial class DataSqlContext : DbContext {
    public DataSqlContext() {
    }

    public DataSqlContext(DbContextOptions<DataSqlContext> options)
        : base(options) {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietPhieuKho> ChiTietPhieuKhos { get; set; }

    public virtual DbSet<DinhLuong> DinhLuongs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<NguyenLieu> NguyenLieus { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhieuKho> PhieuKhos { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       // => optionsBuilder.UseSqlServer("Server=EMBOU;Database=DATA_SQL;Trusted_Connection=True;TrustServerCertificate=True");
       // => optionsBuilder.UseSqlServer("Server=KenG_Kanowaki\\LEMINHDUCSQL;Database=DATA_SQL;Trusted_Connection=True;TrustServerCertificate=True");
       // => optionsBuilder.UseSqlServer("Server=LAPTOP-2Q4VT418\\SQLEXPRESS;Database=DA_QuanLiBanCaPhe;Trusted_Connection=True;TrustServerCertificate=True");
       {
        if (!optionsBuilder.IsConfigured) {
            // 1. Xây dựng đối tượng Configuration

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Lấy thư mục chạy (bin/Debug)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // Đọc file json

            var configuration = configBuilder.Build();

            // 2. Lấy chuỗi kết nối từ file json
            //    ("MyDatabase" là tên bạn đặt trong file json)
            string connString = configuration.GetConnectionString("MyDatabase");

            // 3. Sử dụng chuỗi kết nối đó
            optionsBuilder.UseSqlServer(connString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ChiTietDonHang>(entity => {
            entity.HasKey(e => new { e.MaDh, e.MaSp }).HasName("PK__ChiTietD__F557D6E09790EFA5");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.DonGia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.GhiChu).HasMaxLength(200);

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDon__MaDH__44FF419A");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDon__MaSP__45F365D3");
        });

        modelBuilder.Entity<ChiTietPhieuKho>(entity => {
            entity.HasKey(e => new { e.MaPhieu, e.MaNl }).HasName("PK__ChiTietP__F412E29346EA070F");

            entity.ToTable("ChiTietPhieuKho");

            entity.Property(e => e.MaNl).HasColumnName("MaNL");
            entity.Property(e => e.GiaNhap).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.SoLuong).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MaNlNavigation).WithMany(p => p.ChiTietPhieuKhos)
                .HasForeignKey(d => d.MaNl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPhi__MaNL__59063A47");

            entity.HasOne(d => d.MaPhieuNavigation).WithMany(p => p.ChiTietPhieuKhos)
                .HasForeignKey(d => d.MaPhieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__MaPhi__5812160E");
        });

        modelBuilder.Entity<DinhLuong>(entity => {
            entity.HasKey(e => new { e.MaSp, e.MaNl }).HasName("PK__DinhLuon__F557556F9AEEB440");

            entity.ToTable("DinhLuong");

            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.MaNl).HasColumnName("MaNL");
            entity.Property(e => e.SoLuongCan).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MaNlNavigation).WithMany(p => p.DinhLuongs)
                .HasForeignKey(d => d.MaNl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DinhLuong__MaNL__4D94879B");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.DinhLuongs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DinhLuong__MaSP__4CA06362");
        });

        modelBuilder.Entity<DonHang>(entity => {
            entity.HasKey(e => e.MaDh).HasName("PK__DonHang__27258661F88B9A27");

            entity.ToTable("DonHang");

            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MaKm).HasColumnName("MaKM");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.NgayLap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("Dang xu ly");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__DonHang__MaKH__412EB0B6");

            entity.HasOne(d => d.MaKmNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKm)
                .HasConstraintName("FK__DonHang__MaKM__619B8048");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaNV__4222D4EF");
        });

        modelBuilder.Entity<KhachHang>(entity => {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1EFC73BF21");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.LoaiKh)
                .HasMaxLength(20)
                .HasDefaultValue("Thuong")
                .HasColumnName("LoaiKH");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenKh)
                .HasMaxLength(100)
                .HasColumnName("TenKH");
        });

        modelBuilder.Entity<KhuyenMai>(entity => {
            entity.HasKey(e => e.MaKm).HasName("PK__KhuyenMa__2725CF15520CAB81");

            entity.ToTable("KhuyenMai");

            entity.Property(e => e.MaKm).HasColumnName("MaKM");
            entity.Property(e => e.GiaTri).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.LoaiKm)
                .HasMaxLength(20)
                .HasColumnName("LoaiKM");
            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.TenKm)
                .HasMaxLength(100)
                .HasColumnName("TenKM");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Dang ap dung");

            entity.HasMany(d => d.MaSps).WithMany(p => p.MaKms)
                .UsingEntity<Dictionary<string, object>>(
                    "KhuyenMaiSanPham",
                    r => r.HasOne<SanPham>().WithMany()
                        .HasForeignKey("MaSp")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__KhuyenMai___MaSP__60A75C0F"),
                    l => l.HasOne<KhuyenMai>().WithMany()
                        .HasForeignKey("MaKm")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__KhuyenMai___MaKM__5FB337D6"),
                    j => {
                        j.HasKey("MaKm", "MaSp").HasName("PK__KhuyenMa__F5579F94A99FCA89");
                        j.ToTable("KhuyenMai_SanPham");
                        j.IndexerProperty<int>("MaKm").HasColumnName("MaKM");
                        j.IndexerProperty<int>("MaSp").HasColumnName("MaSP");
                    });
        });

        modelBuilder.Entity<NguyenLieu>(entity => {
            entity.HasKey(e => e.MaNl).HasName("PK__NguyenLi__2725D73CB65E92CE");

            entity.ToTable("NguyenLieu");

            entity.Property(e => e.MaNl).HasColumnName("MaNL");
            entity.Property(e => e.DonViTinh).HasMaxLength(20);
            entity.Property(e => e.NguongCanhBao)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SoLuongTon)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TenNl)
                .HasMaxLength(100)
                .HasColumnName("TenNL");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Ðang kinh doanh");
        });

        modelBuilder.Entity<NhaCungCap>(entity => {
            entity.HasKey(e => e.MaNcc).HasName("PK__NhaCungC__3A185DEB8FE137B0");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenNcc)
                .HasMaxLength(100)
                .HasColumnName("TenNCC");
        });

        modelBuilder.Entity<NhanVien>(entity => {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70A108ED2F1");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenNv)
                .HasMaxLength(100)
                .HasColumnName("TenNV");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Ðang làm vi?c");
        });

        modelBuilder.Entity<PhieuKho>(entity => {
            entity.HasKey(e => e.MaPhieu).HasName("PK__PhieuKho__2660BFE0A7262730");

            entity.ToTable("PhieuKho");

            entity.Property(e => e.LoaiPhieu).HasMaxLength(10);
            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.NgayLap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.PhieuKhos)
                .HasForeignKey(d => d.MaNcc)
                .HasConstraintName("FK__PhieuKho__MaNCC__5535A963");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.PhieuKhos)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhieuKho__MaNV__5441852A");
        });

        modelBuilder.Entity<SanPham>(entity => {
            entity.HasKey(e => e.MaSp).HasName("PK__SanPham__2725081C0D209471");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.DonGia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DonVi).HasMaxLength(20);
            entity.Property(e => e.LoaiSp)
                .HasMaxLength(50)
                .HasColumnName("LoaiSP");
            entity.Property(e => e.TenSp)
                .HasMaxLength(100)
                .HasColumnName("TenSP");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Con ban");
        });

        modelBuilder.Entity<TaiKhoan>(entity => {
            entity.HasKey(e => e.TenDangNhap).HasName("PK__TaiKhoan__55F68FC194C0D713");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.MaNv, "UQ__TaiKhoan__2725D70B5DF2CDC5").IsUnique();

            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.MatKhau).HasMaxLength(256);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaNvNavigation).WithOne(p => p.TaiKhoan)
                .HasForeignKey<TaiKhoan>(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiKhoan__MaNV__787EE5A0");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiKhoan__MaVaiT__797309D9");
        });

        modelBuilder.Entity<ThanhToan>(entity => {
            entity.HasKey(e => e.MaTt).HasName("PK__ThanhToa__2725007984D4E1A4");

            entity.ToTable("ThanhToan");

            entity.Property(e => e.MaTt).HasColumnName("MaTT");
            entity.Property(e => e.HinhThuc).HasMaxLength(50);
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.NgayTt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NgayTT");
            entity.Property(e => e.SoTien).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Chưa thanh toán");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThanhToan__MaDH__66603565");
        });

        modelBuilder.Entity<VaiTro>(entity => {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VaiTro__C24C41CF7C8796F0");

            entity.ToTable("VaiTro");

            entity.HasIndex(e => e.TenVaiTro, "UQ__VaiTro__1DA55814536790C0").IsUnique();

            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
