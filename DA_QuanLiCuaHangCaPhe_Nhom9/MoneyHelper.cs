using System.Globalization;

namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    public static class MoneyHelper {
        private static readonly CultureInfo ViCulture = CultureInfo.GetCultureInfo("vi-VN");

        public static decimal ParseCurrency(string s) {
            if (string.IsNullOrWhiteSpace(s)) return 0m;
            string cleaned = s.Replace(" đ", "", StringComparison.OrdinalIgnoreCase)
                              .Replace("₫", "")
                              .Trim();

            // Try vi-VN first (thousands '.' , decimal ',')
            if (decimal.TryParse(cleaned, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, ViCulture, out var result))
                return result;

            // Try invariant fallback
            if (decimal.TryParse(cleaned, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result))
                return result;

            // Remove non-digit except separators and try again
            var filtered = new string(cleaned.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());
            filtered = filtered.Replace(",", "");
            decimal.TryParse(filtered, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static string FormatCurrency(decimal amount) {
            return amount.ToString("N0", ViCulture) + " đ";
        }
    }
}