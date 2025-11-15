//csharp DataAccess/ThanhToanRepository.cs
using System;
using System.Collections.Generic;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.DataAccess
{
    public static class ThanhToanRepository
    {
        public static bool UpdatePaymentStatus(int maDh, string hinhThuc, string trangThai)
        {
            string sql = "UPDATE ThanhToan SET HinhThuc = @HinhThuc, TrangThai = @TrangThai WHERE MaDH = @MaDH AND TrangThai = 'Ch?a thanh toán'";
            var p = new Dictionary<string, object> { ["@HinhThuc"] = hinhThuc, ["@TrangThai"] = trangThai, ["@MaDH"] = maDh };
            int rows = AdoNetHelper.ExecuteNonQuery(sql, p);
            if (rows > 0)
            {
                // update DonHang status
                string sql2 = "UPDATE DonHang SET TrangThai = @TrangThai WHERE MaDH = @MaDH";
                var p2 = new Dictionary<string, object> { ["@TrangThai"] = trangThai, ["@MaDH"] = maDh };
                AdoNetHelper.ExecuteNonQuery(sql2, p2);
                return true;
            }
            return false;
        }
    }
}