using System.Security.Cryptography;
using System.Text;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Helpers
{
    /// <summary>
    /// Helper class for password hashing and verification using SHA256
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Hash a plain text password using SHA256
        /// </summary>
        /// <param name="plainPassword">Plain text password to hash</param>
        /// <returns>Hashed password as Base64 string</returns>
        public static string HashPassword(string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword))
                return string.Empty;

            // T?o salt c? ð?nh ho?c có th? random (? ðây dùng c? ð?nh cho ðõn gi?n)
            string salt = "CoffeeShop2025"; // Salt c? ð?nh
            string saltedPassword = plainPassword + salt;

            // S? d?ng SHA256 ð? bãm m?t kh?u
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Chuy?n ð?i sang Base64 string ð? lýu vào database
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Verify if a plain text password matches a hashed password
        /// </summary>
        /// <param name="plainPassword">Plain text password to verify</param>
        /// <param name="hashedPassword">Hashed password to compare against</param>
        /// <returns>True if password matches, false otherwise</returns>
        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(hashedPassword))
                    return false;

                // Hash m?t kh?u nh?p vào và so sánh v?i hash ð? lýu
                string hashOfInput = HashPassword(plainPassword);
                return hashOfInput == hashedPassword;
            }
            catch
            {
                // N?u có l?i, tr? v? false
                return false;
            }
        }

        /// <summary>
        /// Check if a password is already hashed (Base64 SHA256 format)
        /// SHA256 hashes when converted to Base64 are always 44 characters long
        /// </summary>
        /// <param name="password">Password to check</param>
        /// <returns>True if already hashed, false if plain text</returns>
        public static bool IsPasswordHashed(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // SHA256 Base64 hash luôn có ð? dài 44 k? t?
            // và ch? ch?a các k? t? Base64 h?p l? (A-Z, a-z, 0-9, +, /, =)
            if (password.Length != 44)
                return false;

            try
            {
                // Th? decode Base64 ð? ki?m tra format h?p l?
                Convert.FromBase64String(password);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
