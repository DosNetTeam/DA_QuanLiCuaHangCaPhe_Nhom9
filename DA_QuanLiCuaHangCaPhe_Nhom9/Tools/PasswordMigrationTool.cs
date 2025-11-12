using DA_QuanLiCuaHangCaPhe_Nhom9.Models;
using DA_QuanLiCuaHangCaPhe_Nhom9.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Tools
{
    /// <summary>
    /// Tool ð? migrate m?t kh?u plain text sang BCrypt hashed format
    /// Ch?y tool này M?T L?N sau khi deploy password hashing feature
    /// </summary>
    public static class PasswordMigrationTool
    {
     public static void MigrateAllPasswords()
        {
     Console.WriteLine("=== PASSWORD MIGRATION TOOL ===");
            Console.WriteLine();
            Console.WriteLine("Tool này s? bãm t?t c? m?t kh?u plain text trong database.");
       Console.WriteLine("B?n s? KHÔNG TH? xem l?i m?t kh?u g?c sau khi migrate!");
            Console.WriteLine();
          Console.Write("B?n có ch?c mu?n ti?p t?c? (yes/no): ");
            
            string? confirmation = Console.ReadLine();
            if (confirmation?.ToLower() != "yes")
  {
                Console.WriteLine("Migration ð? b? h?y.");
       return;
     }

         try
   {
          using var db = new DataSqlContext();

    // Load all accounts
 var accounts = db.TaiKhoans.ToList();
      int totalAccounts = accounts.Count;
     int migratedCount = 0;
    int skippedCount = 0;

 Console.WriteLine();
 Console.WriteLine($"T?m th?y {totalAccounts} tài kho?n...");
      Console.WriteLine();

     foreach (var account in accounts)
       {
         // Check if password is already hashed
               if (PasswordHelper.IsPasswordHashed(account.MatKhau))
          {
    Console.WriteLine($"[SKIP] {account.TenDangNhap} - Ð? ðý?c bãm r?i");
  skippedCount++;
            continue;
    }

    // Store original password for logging
      string originalPassword = account.MatKhau;

        // Hash the password
    account.MatKhau = PasswordHelper.HashPassword(originalPassword);

       Console.WriteLine($"[MIGRATE] {account.TenDangNhap}");
     Console.WriteLine($"  M?t kh?u c?: {originalPassword}");
      Console.WriteLine($"  M?t kh?u m?i (hash): {account.MatKhau.Substring(0, 20)}...");
   Console.WriteLine();

               migratedCount++;
    }

      // Save changes
       db.SaveChanges();

       Console.WriteLine();
           Console.WriteLine("=== K?T QU? MIGRATION ===");
     Console.WriteLine($"T?ng s? tài kho?n: {totalAccounts}");
           Console.WriteLine($"Ð? migrate: {migratedCount}");
     Console.WriteLine($"B? qua (ð? hash): {skippedCount}");
       Console.WriteLine();
         Console.WriteLine("Migration hoàn t?t thành công!");
      }
      catch (Exception ex)
    {
          Console.WriteLine();
        Console.WriteLine($"L?I: {ex.Message}");
          Console.WriteLine($"Chi ti?t: {ex.InnerException?.Message}");
  }
        }

    /// <summary>
        /// Reset t?t c? password v? m?t giá tr? m?c ð?nh (ð? hash)
   /// Dùng khi mu?n force t?t c? users ð?i password
     /// </summary>
        public static void ResetAllPasswordsToDefault(string defaultPassword = "ChangeMe123")
        {
            Console.WriteLine("=== RESET ALL PASSWORDS TOOL ===");
      Console.WriteLine();
    Console.WriteLine($"Tool này s? reset t?t c? password v?: {defaultPassword}");
 Console.WriteLine("Users s? ph?i ð?i password khi ðãng nh?p l?n ð?u.");
   Console.WriteLine();
            Console.Write("B?n có ch?c mu?n ti?p t?c? (yes/no): ");

      string? confirmation = Console.ReadLine();
     if (confirmation?.ToLower() != "yes")
  {
     Console.WriteLine("Reset ð? b? h?y.");
return;
    }

 try
            {
       using var db = new DataSqlContext();

       // Hash the default password
    string hashedPassword = PasswordHelper.HashPassword(defaultPassword);

       // Update all accounts
       var accounts = db.TaiKhoans.ToList();
   foreach (var account in accounts)
     {
 account.MatKhau = hashedPassword;
      Console.WriteLine($"[RESET] {account.TenDangNhap} -> {defaultPassword}");
     }

         db.SaveChanges();

     Console.WriteLine();
    Console.WriteLine($"Ð? reset {accounts.Count} tài kho?n!");
       Console.WriteLine($"Password m?i: {defaultPassword}");
        Console.WriteLine($"Hash: {hashedPassword}");
}
         catch (Exception ex)
        {
      Console.WriteLine();
Console.WriteLine($"L?I: {ex.Message}");
          Console.WriteLine($"Chi ti?t: {ex.InnerException?.Message}");
         }
     }

        /// <summary>
   /// Test password hashing
        /// </summary>
        public static void TestPasswordHashing()
        {
  Console.WriteLine("=== PASSWORD HASHING TEST ===");
  Console.WriteLine();
            Console.Write("Nh?p password ð? test: ");
            string? password = Console.ReadLine();

      if (string.IsNullOrEmpty(password))
          {
 Console.WriteLine("Password không ðý?c r?ng!");
  return;
          }

     // Hash password
    string hashed = PasswordHelper.HashPassword(password);
            Console.WriteLine();
         Console.WriteLine($"Password g?c: {password}");
            Console.WriteLine($"Password hash: {hashed}");
            Console.WriteLine($"Ð? dài hash: {hashed.Length} k? t?");
       Console.WriteLine();

            // Verify password
  bool isValid = PasswordHelper.VerifyPassword(password, hashed);
  Console.WriteLine($"Verify (ðúng): {isValid}");

         bool isWrong = PasswordHelper.VerifyPassword("wrong_password", hashed);
 Console.WriteLine($"Verify (sai): {isWrong}");
     }
    }
}
