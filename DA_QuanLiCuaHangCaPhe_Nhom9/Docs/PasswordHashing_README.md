# Password Hashing Implementation

## ?? T?ng quan

H? th?ng ð? ðý?c nâng c?p ð? s? d?ng **BCrypt** bãm m?t kh?u thay v? lýu plain text. Ðây là m?t best practice quan tr?ng cho b?o m?t.

## ?? Tính nãng ð? implement

### 1. **PasswordHelper Class**
Located: `DA_QuanLiCuaHangCaPhe_Nhom9\Helpers\PasswordHelper.cs`

```csharp
// Hash password
string hashedPassword = PasswordHelper.HashPassword("mypassword123");

// Verify password
bool isValid = PasswordHelper.VerifyPassword("mypassword123", hashedPassword);

// Check if already hashed
bool isHashed = PasswordHelper.IsPasswordHashed(password);
```

### 2. **Login Form** ?
- S? d?ng `PasswordHelper.VerifyPassword()` thay v? so sánh tr?c ti?p
- An toàn v?i timing attacks

### 3. **Create Employee Form** ?
- T? ð?ng bãm password khi t?o tài kho?n m?i
- Password ðý?c hash trý?c khi lýu vào database

### 4. **Admin - Change Password** ?
- Verify password c? b?ng BCrypt
- Hash password m?i trý?c khi update

## ?? Cách s? d?ng

### Option 1: Migrate passwords hi?n có (KHUY?N NGH?)

Ch?y tool migration trong ?ng d?ng (khuy?n ngh? t?o console app riêng):

```csharp
using DA_QuanLiCuaHangCaPhe_Nhom9.Tools;

// Migrate t?t c? passwords plain text -> BCrypt hash
PasswordMigrationTool.MigrateAllPasswords();
```

**Lýu ?:**
- Tool s? gi? nguyên password g?c và hash nó
- Sau khi migrate, user v?n ðãng nh?p b?ng password c?
- Passwords ð? hash s? b? skip (không hash l?i)

### Option 2: Reset t?t c? passwords

N?u mu?n force users ð?i password:

```csharp
// Reset t?t c? v? "ChangeMe123"
PasswordMigrationTool.ResetAllPasswordsToDefault("ChangeMe123");
```

### Option 3: Update manually trong SQL

```sql
-- Reset t?t c? password v? "123" (hashed)
UPDATE TaiKhoan 
SET MatKhau = '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYb7JcS1Aoi'
WHERE LEN(MatKhau) < 60;
```

## ?? Ð?nh d?ng Password Hash

BCrypt hash có format:
```
$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYb7JcS1Aoi
 ?  ?  ?         
 ?  ?  ?? Salt + Hash (50 chars)
 ?  ????? Work Factor (12 = 4096 rounds)
 ??????? Algorithm Version ($2a, $2b, $2y)
```

- **Ð? dài:** 60 k? t?
- **Work Factor:** 12 (an toàn, không quá ch?m)
- **Salt:** T? ð?ng generate, unique cho m?i password

## ?? Lýu ? quan tr?ng

### Database Schema
Ð?m b?o column `MatKhau` có ð? ð? dài:
```sql
ALTER TABLE TaiKhoan 
ALTER COLUMN MatKhau NVARCHAR(256) NOT NULL;
```

### Không th? rollback!
Sau khi hash, **KHÔNG TH?** xem l?i password g?c. Ch? có th?:
- Verify password (check ðúng/sai)
- Reset password m?i

### Test Migration
Luôn test trên database backup trý?c:
```csharp
// Test hashing
PasswordMigrationTool.TestPasswordHashing();
```

## ?? Examples

### Ví d? 1: Hash và Verify
```csharp
string password = "admin123";
string hashed = PasswordHelper.HashPassword(password);
// Output: $2a$12$randomsaltandhashhere...

bool isValid = PasswordHelper.VerifyPassword("admin123", hashed);
// Output: true

bool isWrong = PasswordHelper.VerifyPassword("wrong", hashed);
// Output: false
```

### Ví d? 2: Check if hashed
```csharp
string plainText = "password123";
bool isHashed1 = PasswordHelper.IsPasswordHashed(plainText);
// Output: false (ð? dài < 60, không b?t ð?u b?ng $2)

string bcryptHash = "$2a$12$LQv3c1yqBWVHxkd0LHAkCO...";
bool isHashed2 = PasswordHelper.IsPasswordHashed(bcryptHash);
// Output: true
```

## ?? Troubleshooting

### Problem: "Password không ðúng" sau migration
**Nguyên nhân:** Có th? database chýa ðý?c update
**Gi?i pháp:** 
1. Check trong SQL: `SELECT MatKhau FROM TaiKhoan WHERE TenDangNhap = 'xxx'`
2. Verify password có b?t ð?u b?ng `$2a$` và dài 60 k? t?
3. Ch?y l?i migration tool

### Problem: "Column MatKhau too small"
**Nguyên nhân:** Column MatKhau < 60 chars
**Gi?i pháp:**
```sql
ALTER TABLE TaiKhoan ALTER COLUMN MatKhau NVARCHAR(256);
```

### Problem: Login ch?m
**Nguyên nhân:** BCrypt c? t?nh ch?m (security feature)
**Gi?i pháp:** Normal behavior, work factor 12 là chu?n

## ?? Tài li?u tham kh?o

- [BCrypt.Net-Next GitHub](https://github.com/BcryptNet/bcrypt.net)
- [OWASP Password Storage Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html)
- [How BCrypt Works](https://auth0.com/blog/hashing-in-action-understanding-bcrypt/)

## ? Checklist Deploy

- [ ] Install BCrypt.Net-Next package
- [ ] Implement PasswordHelper class
- [ ] Update Loginform.cs (verify password)
- [ ] Update CreateEmployeeForm.cs (hash new passwords)
- [ ] Update Admin.cs (change password with hash)
- [ ] Expand MatKhau column to NVARCHAR(256)
- [ ] Run migration tool to hash existing passwords
- [ ] Test login with old passwords (should still work)
- [ ] Test create new employee
- [ ] Test change password
- [ ] Backup database trý?c khi deploy!

## ?? Migration Status

| Step | Status | Notes |
|------|--------|-------|
| Install BCrypt | ? | Version 4.0.3 |
| PasswordHelper | ? | Helpers/PasswordHelper.cs |
| Login Form | ? | Uses BCrypt verification |
| Create Employee | ? | Hashes new passwords |
| Change Password | ? | Hashes new passwords |
| Migration Tool | ? | Tools/PasswordMigrationTool.cs |
| Database Schema | ?? | Need to verify MatKhau column size |
| Run Migration | ? | Pending - run after deploy |

---

**Created:** 2025-01-11  
**Version:** 1.0  
**Author:** System Migration
