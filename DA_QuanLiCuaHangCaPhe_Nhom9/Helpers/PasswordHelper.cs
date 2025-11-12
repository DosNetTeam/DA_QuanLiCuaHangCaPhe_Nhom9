using BCrypt.Net;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Helpers
{
    /// <summary>
    /// Helper class for password hashing and verification using BCrypt
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Hash a plain text password using BCrypt
        /// </summary>
        /// <param name="plainPassword">Plain text password to hash</param>
        /// <returns>Hashed password</returns>
        public static string HashPassword(string plainPassword) {
            // BCrypt automatically generates a salt and hashes the password
            // WorkFactor 12 provides good security (default is 10, higher = more secure but slower)
            return BCrypt.Net.BCrypt.HashPassword(plainPassword, workFactor: 12);
        }

        /// <summary>
        /// Verify if a plain text password matches a hashed password
        /// </summary>
        /// <param name="plainPassword">Plain text password to verify</param>
        /// <param name="hashedPassword">Hashed password to compare  against</param>
        /// <returns>True if password matches, false otherwise</returns>
        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
            }
            catch
            {
                // If hashed password is invalid format, return false
                return false;
            }
        }

        /// <summary>
        /// Check if a password is already hashed (BCrypt format)
        /// BCrypt hashes start with "$2a$", "$2b$", or "$2y$"
        /// </summary>
        /// <param name="password">Password to check</param>
        /// <returns>True if already hashed, false if plain text</returns>
        public static bool IsPasswordHashed(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // BCrypt hashes are always 60 characters long and start with $2
            return password.Length == 60 &&
                (password.StartsWith("$2a$") ||
                 password.StartsWith("$2b$") ||
                 password.StartsWith("$2y$"));
        }
    }
}
