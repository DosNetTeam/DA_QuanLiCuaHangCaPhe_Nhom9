using DA_QuanLiCuaHangCaPhe_Nhom9.Models;

namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function {
    public static class NotificationCenter {
        public enum NotificationType {
            NhanVienInactive,
            UnpaidInvoice,
            LowStock
        }

        public class Notification {
            public NotificationType Type { get; set; }
            public string Message { get; set; } = string.Empty;
            public object? Data { get; set; }
        }

        public static event Action<Notification>? NotificationRaised;

        // Allow external callers to raise notifications safely
        public static void Raise(Notification notification) {
            NotificationRaised?.Invoke(notification);
        }

        // Poll database, create notifications and raise events
        public static void PollAndPush() {
            try {
                using (var db = new DataSqlContext()) {
                    // 1) Employees inactive (no orders in last 30 days)
                    var since = DateTime.Now.AddDays(-30);
                    var inactive = db.NhanViens
                        .Where(nv => !db.DonHangs.Any(dh => dh.MaNv == nv.MaNv && dh.NgayLap >= since))
                        .Select(nv => nv.TenNv)
                        .Take(5)
                        .ToList();

                    foreach (var name in inactive) {
                        var n = new Notification { Type = NotificationType.NhanVienInactive, Message = $"Nhân viên lâu không hoạt động: {name}" };
                        Raise(n);
                    }

                    // 2) Unpaid invoices (older than 1 day and ThanhToan status 'Ch?a thanh toán')
                    var unpaid = db.ThanhToans
                        .Where(tt => tt.TrangThai == "Chưa thanh toán" && tt.MaDhNavigation.NgayLap <= DateTime.Now.AddDays(-1))
                        .Select(tt => new { tt.MaDh, tt.MaDhNavigation.NgayLap })
                        .Take(10)
                        .ToList();

                    foreach (var u in unpaid) {
                        var msg = $"Hóa đơn chưa thanh toán: #{u.MaDh} - {u.NgayLap?.ToString("dd/MM/yy")}";
                        var n = new Notification { Type = NotificationType.UnpaidInvoice, Message = msg, Data = u.MaDh };
                        Raise(n);
                    }

                    // 3) Low stock items (SoLuongTon <= NguongCanhBao)
                    var low = db.NguyenLieus
                        .Where(nl => nl.SoLuongTon <= (nl.NguongCanhBao ?? 0))
                        .Select(nl => new { nl.MaNl, nl.TenNl, nl.SoLuongTon })
                        .Take(10)
                        .ToList();

                    foreach (var nl in low) {
                        var msg = $"Hàng trong kho còn ít: {nl.TenNl} ({nl.SoLuongTon ?? 0})";
                        var n = new Notification { Type = NotificationType.LowStock, Message = msg, Data = nl.MaNl };
                        Raise(n);
                    }
                }
            }
            catch {
                // swallow errors to avoid crashing callers
            }
        }

        // Return all current notifications (for manual display)
        public static List<Notification> GetAllNotifications() {
            var list = new List<Notification>();
            try {
                using (var db = new DataSqlContext()) {
                    var since = DateTime.Now.AddDays(-30);
                    var inactive = db.NhanViens
                        .Where(nv => !db.DonHangs.Any(dh => dh.MaNv == nv.MaNv && dh.NgayLap >= since))
                        .Select(nv => nv.TenNv)
                        .Take(5)
                        .ToList();

                    list.AddRange(inactive.Select(name => new Notification { Type = NotificationType.NhanVienInactive, Message = $"Nhân viên lâu không hoạt động: {name}" }));

                    var unpaid = db.ThanhToans
                        .Where(tt => tt.TrangThai == "Chưa thanh toán" && tt.MaDhNavigation.NgayLap <= DateTime.Now.AddDays(-1))
                        .Select(tt => new { tt.MaDh, tt.MaDhNavigation.NgayLap })
                        .Take(10)
                        .ToList();

                    list.AddRange(unpaid.Select(u => new Notification { Type = NotificationType.UnpaidInvoice, Message = $"Hóa đơn chưa thanh toán: #{u.MaDh} - {u.NgayLap?.ToString("dd/MM/yy")}", Data = u.MaDh }));

                    var low = db.NguyenLieus
                        .Where(nl => nl.SoLuongTon <= (nl.NguongCanhBao ?? 0))
                        .Select(nl => new { nl.MaNl, nl.TenNl, nl.SoLuongTon })
                        .Take(10)
                        .ToList();

                    list.AddRange(low.Select(nl => new Notification { Type = NotificationType.LowStock, Message = $"Hàng trong kho còn ít: {nl.TenNl} ({nl.SoLuongTon ?? 0})", Data = nl.MaNl }));
                }
            }
            catch {
            }

            if (list.Count == 0)
                list.Add(new Notification { Type = NotificationType.LowStock, Message = "Không có thông báo mới." });

            return list;
        }
    }
}
