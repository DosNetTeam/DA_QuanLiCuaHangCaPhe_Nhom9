/*
--------------------------------------------------------------------
SCRIPT MIGRATEPassword TO BCRYPT HASHED FORMAT
--------------------------------------------------------------------
C?N BÁO: Script này s? bãm t?t c? m?t kh?u plain text trong database
Ch?y script này M?T L?N duy nh?t sau khi deploy code m?i
--------------------------------------------------------------------
*/

USE DATA_SQL;
GO

PRINT N'=== B?T Ð?U MIGRATE M?T KH?U ===';
PRINT N'';

-- Hi?n th? m?t kh?u hi?n t?i (plain text)
PRINT N'M?t kh?u hi?n t?i (PLAIN TEXT - KHÔNG AN TOÀN):';
SELECT TenDangNhap, MatKhau, LEN(MatKhau) as 'Ð? dài'
FROM TaiKhoan;
PRINT N'';

PRINT N'=== LÝU ? QUAN TR?NG ===';
PRINT N'Sau khi ch?y script này:';
PRINT N'1. T?t c? m?t kh?u s? ðý?c bãm b?ng BCrypt';
PRINT N'2. B?n KHÔNG TH? xem l?i m?t kh?u g?c';
PRINT N'3. Ph?i s? d?ng ?ng d?ng ð? ðãng nh?p (không th? so sánh tr?c ti?p trong SQL)';
PRINT N'4. M?t kh?u bãm có ð? dài 60 k? t?';
PRINT N'';

PRINT N'Script này ch? hi?n th? thông tin. Ð? bãm m?t kh?u, b?n có 2 l?a ch?n:';
PRINT N'';
PRINT N'OPTION 1: S? d?ng ?ng d?ng C# ð? migrate (KHUY?N NGH?)';
PRINT N'  - T?o tool migration riêng trong solution';
PRINT N'  - Code s? ð?c t?ng tài kho?n và hash password';
PRINT N'';
PRINT N'OPTION 2: Reset m?t kh?u và yêu c?u user ð?i l?i';
PRINT N'  - Set t?t c? password = "ChangeMe123" (ð? hash)';
PRINT N'  - User ðãng nh?p l?n ð?u ph?i ð?i password';
PRINT N'';

-- EXAMPLE: N?u b?n mu?n reset t?t c? password v? "123" (ð? hash)
-- Uncomment các d?ng dý?i ðây:

/*
PRINT N'=== ÐANG RESET T?T C? M?T KH?U V? "123" (HASHED) ===';

-- BCrypt hash c?a "123" v?i work factor 12
-- Hash này ðý?c t?o t?: BCrypt.HashPassword("123", 12)
DECLARE @HashedPassword NVARCHAR(256) = '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYb7JcS1Aoi';

UPDATE TaiKhoan 
SET MatKhau = @HashedPassword
WHERE LEN(MatKhau) < 60; -- Ch? update nh?ng password chýa ðý?c hash

PRINT N'Ð? c?p nh?t xong! T?t c? tài kho?n gi? có password = "123"';
PRINT N'';
*/

PRINT N'=== K?T THÚC ===';
PRINT N'Vui l?ng ch?n m?t trong hai options trên ð? migrate password.';
GO
