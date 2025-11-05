/*
--------------------------------------------------------------------
-- KỊCH BẢN (SCRIPT) TẠO DỮ LIỆU MẪU (BẢN SỬA LỖI)
-- ĐÃ SẮP XẾP LẠI THỨ TỰ XÓA
--------------------------------------------------------------------
*/

-- PHẦN 1: XÓA SẠCH DỮ LIỆU CŨ (SẮP XẾP LẠI THỨ TỰ CHO ĐÚNG)
-- Xóa từ các bảng "con" (chi tiết) trước
DELETE FROM ThanhToan;
DELETE FROM ChiTietDonHang;
DELETE FROM KhuyenMai_SanPham;
DELETE FROM DinhLuong;
DELETE FROM ChiTietPhieuKho;
GO

-- Xóa các bảng "cha" (chính) có tham chiếu
DELETE FROM DonHang;
DELETE FROM PhieuKho;
DELETE FROM TaiKhoan;
GO

-- Xóa các bảng "gốc" (không còn tham chiếu nào)
DELETE FROM KhuyenMai;
DELETE FROM SanPham;
DELETE FROM NguyenLieu;
DELETE FROM NhaCungCap;
DELETE FROM KhachHang;
DELETE FROM NhanVien;
DELETE FROM VaiTro;
GO

-- PHẦN 2: RESET BỘ ĐẾM IDENTITY (LÀM MỚI ID TỰ TĂNG)
DBCC CHECKIDENT ('ThanhToan', RESEED, 0);
DBCC CHECKIDENT ('DonHang', RESEED, 0);
DBCC CHECKIDENT ('KhuyenMai', RESEED, 0);
DBCC CHECKIDENT ('PhieuKho', RESEED, 0);
DBCC CHECKIDENT ('VaiTro', RESEED, 0);
DBCC CHECKIDENT ('NhanVien', RESEED, 0);
DBCC CHECKIDENT ('KhachHang', RESEED, 0);
DBCC CHECKIDENT ('NhaCungCap', RESEED, 0);
DBCC CHECKIDENT ('SanPham', RESEED, 0);
DBCC CHECKIDENT ('NguyenLieu', RESEED, 0);
GO

-- PHẦN 3: THÊM DỮ LIỆU MẪU MỚI (Thứ tự INSERT đã đúng từ trước)

-- Bảng VAI TRÒ
INSERT INTO VaiTro (TenVaiTro) VALUES
(N'Quản lý'),        -- MaVaiTro = 1
(N'Pha chế'),        -- MaVaiTro = 2
(N'Thu ngân');       -- MaVaiTro = 3
GO

-- Bảng NHÂN VIÊN
INSERT INTO NhanVien (TenNV, ChucVu, SoDienThoai, NgayVaoLam) VALUES
(N'Nguyễn Văn An', N'Quản lý', '0901234567', '2023-01-15'), -- MaNV = 1
(N'Trần Thị Bích', N'Pha chế', '0912345678', '2023-03-20'), -- MaNV = 2
(N'Lê Minh Cường', N'Thu ngân', '0987654321', '2023-05-10'); -- MaNV = 3
GO

-- Bảng TÀI KHOẢN

INSERT INTO TaiKhoan (TenDangNhap, MatKhau, MaNV, MaVaiTro, TrangThai) VALUES
('admin', '123', 1, 1, 1),
('dinh.tt', '123', 2, 2, 1),
('hau.lm', '123', 3, 3, 1);
GO

-- Bảng KHÁCH HÀNG
INSERT INTO KhachHang (TenKH, SoDienThoai, DiaChi, LoaiKH) VALUES
(N'Trần Anh Hùng', '0911112222', N'123 Lê Lợi, Q1, TP.HCM', N'VIP'),          -- MaKH = 1
(N'Nguyễn Thị Lan', '0922223333', N'456 Nguyễn Trãi, Q5, TP.HCM', N'Thuong'), -- MaKH = 2
(N'Khách vãng lai', NULL, NULL, N'Thuong');                                  -- MaKH = 3
GO

-- Bảng NHÀ CUNG CẤP
INSERT INTO NhaCungCap (TenNCC, DiaChi, SoDienThoai) VALUES
(N'Cty Cà Phê Trung Nguyên', N'10 Đặng Lê Nguyên Vũ, Buôn Ma Thuột', '02623123456'), -- MaNCC = 1
(N'Cty Sữa Vinamilk', N'Số 10, Tân Trào, Q7, TP.HCM', '02854155555'),               -- MaNCC = 2
(N'Cty Nguyên Liệu Pha Chế ABC', N'22 Hàng Buồm, Hà Nội', '0243111222');            -- MaNCC = 3
GO

-- Bảng NGUYÊN LIỆU (Bao gồm SoLuongTon)
INSERT INTO NguyenLieu (TenNL, DonViTinh, SoLuongTon, NguongCanhBao) VALUES
(N'Hạt Cà Phê Robusta', N'kg', 50.0, 10.0),   -- MaNL = 1
(N'Sữa Tươi Không Đường', N'lít', 100.0, 20.0), -- MaNL = 2
(N'Đường Cát Trắng', N'kg', 20.0, 5.0),    -- MaNL = 3
(N'Bột Matcha Nhật Bản', N'kg', 5.0, 1.0),     -- MaNL = 4
(N'Siro Vanilla', N'chai', 10.0, 2.0);        -- MaNL = 5
GO

-- Bảng SẢN PHẨM
INSERT INTO SanPham (TenSP, LoaiSP, DonGia, DonVi, TrangThai) VALUES
(N'Cà Phê Đen', N'Cà phê', 20000.00, N'Ly', N'Con ban'),  -- MaSP = 1
(N'Cà Phê Sữa', N'Cà phê', 25000.00, N'Ly', N'Con ban'),  -- MaSP = 2
(N'Matcha Latte', N'Trà', 45000.00, N'Ly', N'Con ban'),  -- MaSP = 3
(N'Bánh Croissant', N'Bánh', 30000.00, N'Cái', N'Con ban'), -- MaSP = 4
(N'Espresso', N'Cà phê', 35000.00, N'Ly', N'Het hang'); -- MaSP = 5
GO

-- Bảng ĐỊNH LƯỢNG (Công thức)
INSERT INTO DinhLuong (MaSP, MaNL, SoLuongCan) VALUES
(1, 1, 0.02), -- Cà Phê Đen: 0.02kg Cà Phê
(1, 3, 0.01), -- Cà Phê Đen: 0.01kg Đường
(2, 1, 0.02), -- Cà Phê Sữa: 0.02kg Cà Phê
(2, 2, 0.04), -- Cà Phê Sữa: 0.04lít Sữa
(3, 4, 0.015),-- Matcha Latte: 0.015kg Bột Matcha
(3, 2, 0.05); -- Matcha Latte: 0.05lít Sữa
GO

-- Bảng KHUYẾN MÃI
INSERT INTO KhuyenMai (TenKM, MoTa, LoaiKM, GiaTri, NgayBatDau, NgayKetThuc, TrangThai) VALUES
(N'Giảm 10% tổng hóa đơn', N'Áp dụng cho mọi hóa đơn trên 50k', 'HoaDon', 10.00, '2025-10-01', '2025-10-31', N'Dang ap dung'), -- MaKM = 1
(N'Giảm 20% Matcha', N'Giảm giá 20% cho dòng sản phẩm Matcha', 'SanPham', 20.00, '2025-10-15', '2025-11-15', N'Dang ap dung'), -- MaKM = 2
(N'Khai trương (Hết hạn)', N'Giảm giá 15%', 'HoaDon', 15.00, '2023-01-01', '2023-01-31', N'Da ket thuc'); -- MaKM = 3
GO

-- Bảng LIÊN KẾT KHUYẾN MÃI VÀ SẢN PHẨM
INSERT INTO KhuyenMai_SanPham (MaKM, MaSP) VALUES
(2, 3); -- Khuyến mãi 2 (Giảm 20% Matcha) áp dụng cho Sản phẩm 3 (Matcha Latte)
GO

-- Bảng PHIẾU KHO (Nhập hàng)
INSERT INTO PhieuKho (NgayLap, LoaiPhieu, MaNV, MaNCC) VALUES
('2025-10-01 10:00:00', N'Nhap', 1, 1), -- Phiếu 1: NV An nhập Cà phê, Đường (MaPhieu = 1)
('2025-10-01 11:00:00', N'Nhap', 1, 2), -- Phiếu 2: NV An nhập Sữa (MaPhieu = 2)
('2025-10-02 14:00:00', N'Nhap', 1, 3); -- Phiếu 3: NV An nhập Matcha, Siro (MaPhieu = 3)
GO

-- Bảng CHI TIẾT PHIẾU KHO
INSERT INTO ChiTietPhieuKho (MaPhieu, MaNL, SoLuong, GiaNhap) VALUES
(1, 1, 50.0, 150000.00), -- Phiếu 1: Nhập 50kg Cà Phê
(1, 3, 20.0, 25000.00),  -- Phiếu 1: Nhập 20kg Đường
(2, 2, 100.0, 30000.00), -- Phiếu 2: Nhập 100lít Sữa
(3, 4, 5.0, 1000000.00), -- Phiếu 3: Nhập 5kg Matcha
(3, 5, 10.0, 80000.00);  -- Phiếu 3: Nhập 10 chai Siro
GO

-- Bảng ĐƠN HÀNG
INSERT INTO DonHang (NgayLap, MaKH, MaNV, TongTien, TrangThai, MaKM) VALUES
('2025-10-25 08:30:00', 1, 3, 72000.00, N'Da hoan thanh', 1), -- ĐH 1: KH Hùng, NV Cường, KM 10% (MaDH = 1)
('2025-10-25 09:15:00', 2, 3, 20000.00, N'Da hoan thanh', NULL),-- ĐH 2: KH Lan, NV Cường, không KM (MaDH = 2)
('2025-10-26 10:00:00', 3, 3, 36000.00, N'Dang xu ly', NULL); -- ĐH 3: Khách vãng lai, NV Cường, (MaDH = 3)
GO

-- Bảng CHI TIẾT ĐƠN HÀNG
INSERT INTO ChiTietDonHang (MaDH, MaSP, SoLuong, DonGia, GhiChu) VALUES
(1, 2, 2, 25000.00, N'Ít đá'), -- ĐH 1
(1, 4, 1, 30000.00, N'Hâm nóng'), -- ĐH 1
(2, 1, 1, 20000.00, NULL), -- ĐH 2
(3, 3, 1, 36000.00, N'KM 20%'); -- ĐH 3
GO

-- Bảng THANH TOÁN
INSERT INTO ThanhToan (MaDH, HinhThuc, SoTien, NgayTT, TrangThai) VALUES
(1, N'Tiền mặt', 72000.00, '2025-10-25 08:31:00', N'Đã thanh toán'),
(2, N'Chuyển khoản QR', 20000.00, '2025-10-25 09:16:00', N'Đã thanh toán'),
(3, N'Thẻ', 36000.00, '2025-10-26 10:01:00', N'Chưa thanh toán');
GO

PRINT N'HOÀN TẤT VIỆC XÓA VÀ TẠO DỮ LIỆU MẪU MỚI (ĐÃ SỬA LỖI).';
GO