# DA_QuanLiCuaHangCaPhe_Nhom9

Detailed README — Point-of-Sale (POS) WinForms demo for a coffee shop.  
Language: C# 13 | Target framework: .NET 9

---

## Project summary
This repository implements a simple coffee-shop POS with:
- Product catalog and categories
- Inventory tracked by raw ingredients (Nguyên liệu)
- Order creation, "save temporary" (Đang xử lý) and checkout flow
- Payment records with support for cash and QR (visual QR preview)
- EF Core DbContext: `DataSqlContext`
- Sample data script: `DulieuMau_data_dosnet.sql`

The UI is WinForms (Windows) with forms:
- `MainForm` — product browsing, cart (ListView), save-temporary, quick checkout.
- `ThanhToan` — finalize payment for an existing order (constructor requires `MaDh`).
- `ChonDonHangCho` — select a saved (pending) order to pay or cancel.
- `ThemKhachHangMoi` — add new customer.

---

## Prerequisites
- Windows with Visual Studio 2026 (or later) with .NET 9 workload installed.
- SQL Server instance (LocalDB or full SQL Server) to host the sample database.
- (Optional) SQL Server Management Studio (SSMS) to run the sample SQL script.

---

## Quick setup (development)
1. Clone repo and open solution in Visual Studio 2026.
2. Restore NuGet packages: __Tools > NuGet Package Manager > Package Manager Console__ (or Visual Studio will restore automatically).
3. Configure database:
   - Option A (recommended): Run `DulieuMau_data_dosnet.sql` on your SQL Server to create sample data.
   - Option B: If you use EF Core migrations, ensure migrations are applied (`Update-Database` or `dotnet ef database update`).
4. Set connection string:
   - If connection is defined inside `DataSqlContext.OnConfiguring`, edit `DataSqlContext.cs`.
   - If the solution uses `appsettings.json`, update the connection there.
5. In Solution Explorer, right-click the WinForms project → __Set as StartUp Project__.
6. Build and run: __Build > Build Solution__, then __Debug > Start Debugging__.

---

## Database / Sample data
- The file `DulieuMau_data_dosnet.sql` contains drop/truncate + seed data for roles, employees, raw materials, products, formulas (`DinhLuong`), orders and payments.
- Run it against your SQL Server database to get sample records (helps exercise "Đơn chờ", low-stock, promotions, etc).

---

## Key workflows

### Add product to order (MainForm)
- Click a product button to add it to the cart (`lvDonHang`).
- The code checks inventory by reading `DinhLuongs` and `NguyenLieus`.
- If item already exists in cart, quantity increments (after checking stock).

### Save temporary order
- When user saves temporarily, `MainForm.ThucHienLuuTam()`:
  - Creates `DonHang` with status `"Đang xử lý"`.
  - Creates related `ChiTietDonHang` entries.
  - Creates a `ThanhToan` row with `TrangThai = "Chưa thanh toán"`.
  - Deducts ingredient quantities from inventory and saves changes.

### Checkout / Payment
- Open `ChonDonHangCho` to pick a pending order or proceed directly if cart had items.
- `ThanhToan` receives an order id (`MaDh`) in its constructor — it loads:
  - `DonHang` by `MaDh`
  - The pending `ThanhToan` (`TrangThai == "Chưa thanh toán"`)
  - `ChiTietDonHang` items and product names
- On confirm:
  - Updates `DonHang.TrangThai = "Đã thanh toán"` and `ThanhToan.TrangThai = "Đã thanh toán"`.
  - Sets `ThanhToan.HinhThuc` (cash or QR) and calls `db.SaveChanges()`.

Important: Do NOT attempt to construct `ThanhToan` with the old ListView overload. Use the integer order id.

---

## Project structure (important files)
- `MainForm.cs` — main UI, product loading, cart logic, save-temporary and checkout triggers.
- `ThanhToan.cs` — loads order for payment, renders bill preview, updates payment status.
- `ChonDonHangCho.cs` — list & selection of pending orders; supports cancel with stock rollback.
- `ThemKhachHangMoi.cs` — add customer flow.
- `DataSqlContext.cs` — EF Core DbContext (contains connection details / OnConfiguring).
- `DulieuMau_data_dosnet.sql` — sample DB seed script.

---

## Common issues & troubleshooting
- Connection errors — verify the connection string and that SQL Server accepts connections. If using LocalDB, ensure LocalDB instance is installed.
- EF "Open DataReader" errors — code has been hardened by using `.ToList()` before iterating on related queries. Ensure you do not enumerate multiple EF queries with an open reader in the same context without materializing them.
- `ThanhToan` not finding pending payment:
  - Ensure `ThanhToan` row for the chosen order exists with `TrangThai = "Chưa thanh toán"`.
  - If the order was saved incorrectly, check `DonHang.TongTien` and related `ChiTietDonHang`.
- If customer search/button states do not update: `MainForm` uses a helper `SearchKhachHangBySDT`. Ensure text input is digits-only and length checks (10 digits) are respected.

---

## Testing tips
- Use `DulieuMau_data_dosnet.sql` to seed predictable test data.
- Test full lifecycle:
  1. Add items to cart → Save Temporary → Check `DonHang` & `ThanhToan` rows.
  2. Open `ChonDonHangCho`, select pending order → Open `ThanhToan` → Complete payment → Confirm statuses changed.
  3. Cancel an order via `ChonDonHangCho` and verify inventory is restored.

---

## Development & maintenance
- Use EF Core migrations for schema changes. In Visual Studio use __Tools > NuGet Package Manager > Package Manager Console__ or CLI `dotnet ef`.
- Keep UI logic separated from data access where practical; `DataSqlContext` is used directly from forms in this demo (acceptable for small app).
- Add more defensive checks for nulls and invalid DB state when moving to production.

---

## Contributing
- Fork, update code, test locally, open a PR.
- Provide a clear description of behavior change and test steps.

---

## License & contacts
- No license file included — add `LICENSE` if you intend to publish.
- For questions about architecture or data models, inspect `DataSqlContext.cs` and EF entity classes under `Models`.

---

If you want, I can:
- Generate a ready-to-add `README.md` file (I can write it to the repo if you permit).
- Add a sample `appsettings.json` template and a short guide to update `DataSqlContext`.
- Produce a developer checklist for testing common scenarios (payment, cancel, low-stock notifications).
