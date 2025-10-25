-- Bảng KHÁCH HÀNG
CREATE TABLE KhachHang (
    MaKH INT PRIMARY KEY IDENTITY(1,1),
    TenKH NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(200),
    LoaiKH NVARCHAR(20) DEFAULT 'Thuong'
);

-- Bảng NHÂN VIÊN
CREATE TABLE NhanVien (
    MaNV INT PRIMARY KEY IDENTITY(1,1),
    TenNV NVARCHAR(100) NOT NULL,
    ChucVu NVARCHAR(50),
    SoDienThoai VARCHAR(15),
    NgayVaoLam DATE NOT NULL
);

-- Bảng SẢN PHẨM
CREATE TABLE SanPham (
    MaSP INT PRIMARY KEY IDENTITY(1,1),
    TenSP NVARCHAR(100) NOT NULL,
    LoaiSP NVARCHAR(50),
    DonGia DECIMAL(10,2) NOT NULL,
    DonVi NVARCHAR(20),
    TrangThai NVARCHAR(20) DEFAULT 'Con ban'
);

-- Bảng ĐƠN HÀNG
CREATE TABLE DonHang (
    MaDH INT PRIMARY KEY IDENTITY(1,1),
    NgayLap DATETIME DEFAULT GETDATE(),
    MaKH INT NULL,
    MaNV INT NOT NULL,
    TongTien DECIMAL(12,2),
    TrangThai NVARCHAR(30) DEFAULT 'Dang xu ly',
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

-- Bảng CHI TIẾT ĐƠN HÀNG
CREATE TABLE ChiTietDonHang (
    MaDH INT,
    MaSP INT,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(10,2) NOT NULL,
    GhiChu NVARCHAR(200),
    PRIMARY KEY (MaDH, MaSP),
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- Bảng NGUYÊN LIỆU
CREATE TABLE NguyenLieu (
    MaNL INT PRIMARY KEY IDENTITY(1,1),
    TenNL NVARCHAR(100) NOT NULL,
    DonViTinh NVARCHAR(20) NOT NULL,
    SoLuongTon DECIMAL(10,2) DEFAULT 0,
    NguongCanhBao DECIMAL(10,2) DEFAULT 0
);

-- Bảng ĐỊNH LƯỢNG (công thức pha chế)
CREATE TABLE DinhLuong (
    MaSP INT,
    MaNL INT,
    SoLuongCan DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (MaSP, MaNL),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP),
    FOREIGN KEY (MaNL) REFERENCES NguyenLieu(MaNL)
);

-- Bảng NHÀ CUNG CẤP
CREATE TABLE NhaCungCap (
    MaNCC INT PRIMARY KEY IDENTITY(1,1),
    TenNCC NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200),
    SoDienThoai VARCHAR(15)
);

-- Bảng PHIẾU KHO
CREATE TABLE PhieuKho (
    MaPhieu INT PRIMARY KEY IDENTITY(1,1),
    NgayLap DATETIME DEFAULT GETDATE(),
    LoaiPhieu NVARCHAR(10) CHECK (LoaiPhieu IN ('Nhap', 'Xuat')),
    MaNV INT NOT NULL,
    MaNCC INT NULL,
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC)
);

-- Bảng CHI TIẾT PHIẾU KHO
CREATE TABLE ChiTietPhieuKho (
    MaPhieu INT,
    MaNL INT,
    SoLuong DECIMAL(10,2) NOT NULL,
    GiaNhap DECIMAL(12,2) NULL,
    PRIMARY KEY (MaPhieu, MaNL),
    FOREIGN KEY (MaPhieu) REFERENCES PhieuKho(MaPhieu),
    FOREIGN KEY (MaNL) REFERENCES NguyenLieu(MaNL)
);
-- Bảng KHUYẾN MÃI
CREATE TABLE KhuyenMai (
    MaKM INT PRIMARY KEY IDENTITY(1,1),
    TenKM NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(200),
    LoaiKM NVARCHAR(20) CHECK (LoaiKM IN ('HoaDon', 'SanPham')),
    GiaTri DECIMAL(5,2) NOT NULL,   -- % giảm hoặc số tiền
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    TrangThai NVARCHAR(20) DEFAULT 'Dang ap dung'
);

-- Bảng LIÊN KẾT KHUYẾN MÃI VÀ SẢN PHẨM
CREATE TABLE KhuyenMai_SanPham (
    MaKM INT,
    MaSP INT,
    PRIMARY KEY (MaKM, MaSP),
    FOREIGN KEY (MaKM) REFERENCES KhuyenMai(MaKM),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- Thêm cột MaKM vào bảng ĐƠN HÀNG (nếu áp dụng khuyến mãi cho toàn bộ hóa đơn)
ALTER TABLE DonHang
ADD MaKM INT NULL,
    FOREIGN KEY (MaKM) REFERENCES KhuyenMai(MaKM);

CREATE TABLE ThanhToan (
    MaTT INT PRIMARY KEY IDENTITY(1,1),
    MaDH INT NOT NULL, -- Giống kiểu trong DonHang
    HinhThuc NVARCHAR(50), -- QR, TienMat, The, ...
    SoTien DECIMAL(12,2),
    NgayTT DATETIME DEFAULT GETDATE(),
    TrangThai NVARCHAR(20) DEFAULT N'Chưa thanh toán',
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH)
);

CREATE TABLE VaiTro (
    MaVaiTro INT PRIMARY KEY IDENTITY(1,1),
    TenVaiTro NVARCHAR(50) NOT NULL UNIQUE  -- Ví dụ: 'Admin', 'NhanVienPhaChe', 'NhanVienThuNgan'
);

CREATE TABLE TaiKhoan (
    TenDangNhap VARCHAR(50) PRIMARY KEY,      -- Tên dùng để login, không nên là Tiếng Việt có dấu
    MatKhau NVARCHAR(256) NOT NULL,           -- Sẽ lưu chuỗi HASH, không bao giờ lưu mật khẩu gốc!
    MaNV INT NOT NULL UNIQUE,                 -- Đảm bảo 1 nhân viên chỉ có 1 tài khoản
    MaVaiTro INT NOT NULL,
    TrangThai BIT DEFAULT 1,                  -- 1 = Hoạt động, 0 = Bị khóa
    
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);
