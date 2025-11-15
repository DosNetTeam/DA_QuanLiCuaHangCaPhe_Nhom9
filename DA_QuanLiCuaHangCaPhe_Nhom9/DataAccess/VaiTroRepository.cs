//csharp DataAccess/VaiTroRepository.cs
using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess {
    public static class VaiTroRepository {
        public static List<VaiTro> GetAll() {
            string sql = "SELECT MaVaiTro, TenVaiTro FROM VaiTro ORDER BY TenVaiTro";
            return AdoNetHelper.QueryList(sql, reader => {
                var vt = new VaiTro();
                vt.MaVaiTro = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                vt.TenVaiTro = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                return vt;
            });
        }
    }
}