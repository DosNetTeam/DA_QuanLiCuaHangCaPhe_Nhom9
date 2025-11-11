# README - Phân tích Chức năng Form (POS)

Tài liệu này phân tích chi tiết luồng nghiệp vụ và logic code của các form chính trong hệ thống POS (Point of sale - Bán hàng) của dự án `DA_QuanLiCuaHangCaPhe_Nhom9`.

## 📌 Tổng quan Luồng Bán hàng (Workflow)

Luồng bán hàng cốt lõi được xử lý bởi 3 form chính, phối hợp với nhau để quản lý đơn hàng từ khi tạo đến khi hoàn tất:

1.  **`MainForm`**: Nhân viên tạo giỏ hàng.
2.  **`ThucHienLuuTam()` (Hàm nội bộ của MainForm)**: Khi bấm "Lưu tạm" hoặc "Thanh toán", hệ thống sẽ lưu đơn hàng vào CSDL, **trừ kho nguyên liệu**, và tạo một phiếu thanh toán "Chưa thanh toán".
3.  **`ChonDonHangCho`**: (Tùy chọn) Nếu nhân viên muốn thanh toán một đơn cũ, form này sẽ hiện ra.
4.  **`ThanhToan`**: Form này nhận `MaDH` (Mã đơn hàng) đã được lưu, hiển thị chi tiết, và cập nhật trạng thái đơn hàng thành "Đã thanh toán" sau khi thu tiền.

---

## ☕ Form Bán Hàng: `MainForm.cs`

Đây là màn hình POS chính, nơi nhân viên thực hiện hầu hết các thao tác.

### Mục đích

Cung cấp giao diện trực quan để nhân viên tạo đơn hàng mới, quản lý giỏ hàng, áp dụng khuyến mãi, và gửi đơn hàng đi thanh toán (hoặc lưu tạm).

### Logic nghiệp vụ chính

* **Tải sản phẩm động:** Khi form được tải, `MainForm_Load` gọi `TaiLoaiSanPham` và `TaiSanPham`. Các nút sản phẩm (`Button`) được tạo tự động và gán đối tượng `SanPham` vào `Tag` của nút.
* **Kiểm tra tồn kho (Pre-check):** Hàm `TaiSanPham` gọi `KiemTraDuNguyenLieu` để đổi màu nút sản phẩm (Cam/Xám) nếu nguyên liệu sắp hết hoặc đã hết.
* **Kiểm tra tồn kho (Khi thêm):** Khi thêm món (`ThemSanPhamVaoDonHang`), code gọi `KiemTraSoLuongTonThucTe` để đảm bảo kho đủ nguyên liệu cho số lượng yêu cầu.
* **Logic Khuyến mãi:** Hàm `CapNhatTongTien` là trái tim của việc tính tiền.
    1.  Nó lặp qua giỏ hàng (`lvDonHang`).
    2.  Tính `tongTien` (tổng giá gốc).
    3.  Gọi `GetGiaBan` (truy vấn CSDL) để tính giảm giá trên từng sản phẩm (`tongTienGiamGia`).
    4.  Truy vấn CSDL để tìm KM 'HoaDon' và tính `tongGiamGiaHoaDon`.
    5.  Cập nhật 3 `Label` hiển thị: `lblTienTruocGiam` (giá gốc), `lblGiamGia` (tổng giảm), `lblTongCong` (giá cuối).
* **Hàm Lưu trữ (`ThucHienLuuTam`)**: Đây là hàm quan trọng nhất. Khi được gọi (bởi nút "Lưu Tạm" hoặc "Thanh Toán"), nó sẽ:
    1.  Tạo `DonHang` (trạng thái "Đang xử lý") và lưu `TongTien` (là **giá cuối cùng** đã giảm).
    2.  Tạo `ChiTietDonHang` và lưu **giá gốc** vào (`DonGia = donGia`).
    3.  Tạo `ThanhToan` (trạng thái "Chưa thanh toán").
    4.  **Trừ kho nguyên liệu** (`nguyenLieuTrongKho.SoLuongTon -= luongCanTru`).
    5.  Gọi `db.SaveChanges()` và trả về `MaDH` mới.

---

## 🔑 Form Đăng nhập: `Loginform.cs`

Đây là cửa ngõ đầu tiên của ứng dụng.

### Mục đích

Xác thực người dùng dựa trên CSDL và điều hướng họ đến form chính xác dựa trên vai trò (`VaiTro`).

### Logic nghiệp vụ chính

* **Xác thực:** `btnDangnhap_Click` truy vấn bảng `TaiKhoan`.
* **Kiểm tra Trạng thái:** Kiểm tra `account.TrangThai == false` (tài khoản bị khóa).
* **Điều hướng (Routing):** Dựa trên `account.VaiTro`, code sẽ mở form tương ứng:
    * **"Chủ cửa hàng"** -> mở `Admin`
    * **"Quản lý"** -> mở `QuanLi` (và truyền `account.MaNv`)
    * **"Nhân viên"** -> mở `MainForm` (và truyền `account.MaNv`)
* **Quản lý phiên (Session):** Form `Loginform` sẽ `Hide()` (ẩn đi) và đăng ký sự kiện `FormClosed` của form mới. Khi form `Admin`/`QuanLi`/`MainForm` đóng lại, form `Loginform` sẽ `Show()` (hiện lại).
* **Nhận thông báo:** Form này cũng đăng ký `NotificationCenter.NotificationRaised` để nhận thông báo từ admin.

---

## 🧾 Form Chức năng: `ThanhToan.cs`

Đây là form xác nhận thanh toán cuối cùng.

### Mục đích

Hoàn tất một đơn hàng **đã được lưu** trong CSDL (bởi `MainForm`). Form này *không* tạo đơn hàng mới, không trừ kho.

### Logic nghiệp vụ chính

* **Constructor (Hàm khởi tạo):** Bắt buộc phải nhận 3 tham số: `maDonHangChon` (Mã ĐH), `tongGoc`, và `soTienGiam`.
* **`ThanhToan_Load`:** Tải `_donHangCanThanhToan` và `_thanhToanCanCapNhat` (với trạng thái "Chưa thanh toán") từ CSDL.
* **Hiển thị:**
    * `groupBox1` (khu vực tính tiền) hiển thị **giá cuối cùng** (`_tongTien`) để thu ngân nhập tiền.
    * `HienThiBillPreview` (hóa đơn xem trước) sử dụng `_tongTienGoc_passed` và `_soTienGiam_passed` để hiển thị chi tiết giảm giá.
* **`btn_inhoadon_Click` (Hoàn tất):** Đây là logic "chốt sổ". Nó chỉ cập nhật 2 dòng trong CSDL: `_donHangCanThanhToan.TrangThai = "Đã thanh toán"` và `_thanhToanCanCapNhat.TrangThai = "Đã thanh toán"`. Sau đó, nó trả về `DialogResult.OK`.

---

## 🕒 Form Chức năng: `ChonDonHangCho.cs`

Form này cho phép nhân viên quản lý các đơn hàng đã được "Lưu Tạm".

### Mục đích

Hiển thị danh sách các đơn hàng "Đang xử lý" để nhân viên có thể chọn (1) Hủy đơn hoặc (2) Thanh toán.

### Logic nghiệp vụ chính

* **Tải danh sách:** Hàm `TaiDanhSachDonHangCho` truy vấn CSDL, lấy tất cả `DonHang` có `TrangThai == "Đang xử lý"` và hiển thị lên `lvDonHangCho`.
* **`btnChonThanhToan_Click` (Chọn thanh toán):**
    1.  Lấy `maDHCanThanhToan` từ `Tag` của dòng được chọn.
    2.  **Tự tính toán lại:** Nó truy vấn `ChiTietDonHang` (đang lưu giá gốc) để lấy `tongGoc`.
    3.  Lấy `thanhTienCuoi` (giá cuối) từ `DonHang.TongTien`.
    4.  Tính `soTienGiam = tongGoc - thanhTienCuoi`.
    5.  Mở form `ThanhToan` và truyền 3 giá trị này đi.
* **`btnHuyDonCho_Click` (Hủy đơn):** Đây là logic "Hoàn kho" (Stock Rollback).
    1.  Hàm này bắt đầu một `transaction` (giao dịch an toàn).
    2.  Nó lặp qua `chiTiet` (chi tiết đơn hàng) và `congThuc` (công thức).
    3.  Thực hiện phép tính **`nguyenLieuTrongKho.SoLuongTon += luongCanCong;`** để CỘNG TRẢ lại nguyên liệu về kho.
    4.  Cập nhật trạng thái `DonHang` và `ThanhToan` thành "Đã huỷ".
    5.  Gọi `db.SaveChanges()` và `transaction.Commit()`.

---

## 🧑‍💼 Form Chức năng: `ThemKhachHangMoi.cs`

Một form phụ đơn giản để nhập liệu.

### Mục đích

Cung cấp giao diện cho phép nhân viên thêm nhanh một khách hàng mới vào CSDL khi khách hàng đó không tìm thấy qua SĐT.

### Logic nghiệp vụ chính

* Form này được gọi bởi nút "Thêm" trên `MainForm`.
* Nó nhận `sdt` (số điện thoại) từ `MainForm` và tự động điền vào `txtSDT` (đồng thời vô hiệu hóa nó).
* `btnSave_Click` kiểm tra `txtTenKH` không được rỗng, sau đó tạo đối tượng `KhachHang` mới và gọi `db.SaveChanges()`.
* Trả về `DialogResult.OK` để `MainForm` biết và tự động tìm kiếm lại khách hàng mới đó.

---

## 🔔 Lớp Tĩnh: `NotificationCenter.cs`

Đây là một lớp hệ thống (không phải Form) hoạt động ngầm.

### Mục đích

Tạo ra một hệ thống "Phát thanh" (Observer Pattern) cho phép các phần khác nhau của ứng dụng giao tiếp với nhau mà không cần biết về sự tồn tại của nhau.

### Logic nghiệp vụ chính

* **Đăng ký (Subscribe):** Các Form (như `Loginform`, `MainForm`, `QuanLi`) đăng ký vào sự kiện `NotificationCenter.NotificationRaised`.
* **Phát (Raise):** Một form khác (như `QuanLi.cs`) có thể gửi thông báo bằng cách gọi `NotificationCenter.Raise(n)`.
* **Nhận (Receive):** Tất cả các form đã đăng ký sẽ ngay lập tức nhận được thông báo và tự động chạy hàm `ShowToast` (hiển thị thông báo).
* **Tự động quét (Polling):** Lớp này cũng có các hàm (`PollAndPush`, `GetAllNotifications`) để tự quét CSDL tìm các cảnh báo nghiệp vụ (như tồn kho thấp, hóa đơn quá hạn).
