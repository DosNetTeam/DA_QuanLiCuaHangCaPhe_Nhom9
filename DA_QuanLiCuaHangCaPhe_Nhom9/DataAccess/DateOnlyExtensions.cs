//csharp Helpers/DateOnlyExtensions.cs
using System;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Helpers
{
    public static class DateOnlyExtensions
    {
        public static DateTime ToDateTime(this DateOnly d, TimeOnly t) => d.ToDateTime(t);
        public static DateTime ToDateTime(this DateOnly d) => d.ToDateTime(TimeOnly.MinValue);
        public static DateOnly FromDateTime(DateTime dt) => DateOnly.FromDateTime(dt);
    }
}