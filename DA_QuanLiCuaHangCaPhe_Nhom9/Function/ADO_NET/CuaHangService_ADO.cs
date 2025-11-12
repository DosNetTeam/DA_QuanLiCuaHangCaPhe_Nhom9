using DA_QuanLiCuaHangCaPhe_Nhom9.Models; // Dùng các Model "từ vựng"
using Microsoft.Data.SqlClient; // Thư viện ADO.NET chính
using System.Data;


namespace DA_QuanLiCuaHangCaPhe_Nhom9.Function {
    public class CuaHangService_ADO {
        private string _connectionString = DatabaseConnection.GetConnectionString();

        /// <summary>
        /// (ADO.NET) Tải tất cả các Loại Sản Phẩm
        /// </summary>
        public DataTable TaiTatCaLoaiSanPham() {
            // 'using' để tự động đóng kết nối
            using (SqlConnection conn = new SqlConnection(_connectionString)) {
                conn.Open(); // Mở kết nối

                // Câu lệnh SQL để lấy các loại SP (không trùng lặp)
                string sql = @"SELECT DISTINCT LoaiSP 
                               FROM SanPham 
                               WHERE LoaiSP IS NOT NULL AND LoaiSP != ''";

                // SqlDataAdapter là "người vận chuyển"
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                // DataTable là một "cái bảng" tạm để chứa dữ liệu
                DataTable dt = new DataTable();

                // Đổ dữ liệu từ CSDL vào "cái bảng" tạm
                adapter.Fill(dt);

                return dt; // Trả về cái bảng đã chứa đầy dữ liệu
            }
        } // Kết nối tự động đóng ở đây

        /// <summary>
        /// (ADO.NET) Tải 3 bảng dữ liệu quan trọng để hiển thị Sản phẩm
        /// </summary>
        public DataSet TaiDuLieuSanPham() {
            // DataSet là "cái kho" có thể chứa NHIỀU bảng
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(_connectionString)) {
                conn.Open();

                // 1. Tải bảng SanPham (chỉ lấy "Còn bán")
                string sqlSanPham = "SELECT * FROM SanPham WHERE TrangThai = N'Còn bán'";
                SqlDataAdapter daSanPham = new SqlDataAdapter(sqlSanPham, conn);
                daSanPham.Fill(ds, "SanPham"); // Đổ vào kho, đặt tên là "SanPham"

                // 2. Tải bảng DinhLuong (công thức)
                string sqlDinhLuong = "SELECT * FROM DinhLuong";
                SqlDataAdapter daDinhLuong = new SqlDataAdapter(sqlDinhLuong, conn);
                daDinhLuong.Fill(ds, "DinhLuong"); // Đổ vào kho, đặt tên là "DinhLuong"

                // 3. Tải bảng NguyenLieu (chỉ lấy "Đang kinh doanh")
                string sqlNguyenLieu = "SELECT * FROM NguyenLieu WHERE TrangThai = N'Đang kinh doanh'";
                SqlDataAdapter daNguyenLieu = new SqlDataAdapter(sqlNguyenLieu, conn);
                daNguyenLieu.Fill(ds, "NguyenLieu"); // Đổ vào kho, đặt tên là "NguyenLieu"
            }

            return ds; // Trả về "cái kho" chứa cả 3 bảng
        }

        // (Thêm 3 hàm này vào CuaHangService_ADO.cs)

        /// <summary>
        /// (ADO.NET) Kiểm tra kho (Dùng DataTable đã tải về)
        /// </summary>
        public string KiemTraDuNguyenLieu_ADO(int maSP, DataTable dtDinhLuong, DataTable dtNguyenLieu) {
            // 1. Lọc bảng DinhLuong (công thức) bằng tay
            var congThuc = new List<DataRow>();
            foreach (DataRow dr in dtDinhLuong.Rows) {
                if ((int)dr["MaSP"] == maSP) {
                    congThuc.Add(dr);
                }
            }

            if (congThuc.Count == 0) {
                return "DU_HANG"; // Không có công thức = Luôn đủ
            }

            string trangThaiTongQuat = "DU_HANG";

            // 3. Lặp qua TỪNG NGUYÊN LIỆU trong công thức
            foreach (DataRow nguyenLieuCan in congThuc) {
                int maNL = (int)nguyenLieuCan["MaNL"];

                // 4. Lấy nguyên liệu trong kho từ DataTable tạm
                DataRow nguyenLieuTrongKho = null;
                foreach (DataRow nl in dtNguyenLieu.Rows) {
                    if ((int)nl["MaNL"] == maNL) {
                        nguyenLieuTrongKho = nl;
                        break;
                    }
                }

                if (nguyenLieuTrongKho == null) {
                    return "HET_HANG"; // Lỗi, coi như hết
                }

                // 5.1. KIỂM TRA HẾT HẲN (<= 0)
                decimal soLuongTon = (decimal)nguyenLieuTrongKho["SoLuongTon"];
                if (soLuongTon <= 0) {
                    return "HET_HANG"; // Hết hẳn
                }

                // 5.2. KIỂM TRA SẮP HẾT (dưới ngưỡng)
                decimal nguongCanhBao = (decimal)nguyenLieuTrongKho["NguongCanhBao"];
                if (soLuongTon <= nguongCanhBao) {
                    trangThaiTongQuat = "SAP_HET";
                }
            }

            return trangThaiTongQuat;
        }


        /// <summary>
        /// (ADO.NET) Kiểm tra kho (Truy vấn CSDL trực tiếp)
        /// </summary>
        public bool KiemTraSoLuongTonThucTe_ADO(int maSP, int soLuongMuonKiemTra) {
            // Dùng 'using' để tự đóng kết nối
            using (SqlConnection conn = new SqlConnection(_connectionString)) {
                conn.Open();

                // 1. Lấy công thức của sản phẩm
                string sqlCongThuc = "SELECT MaNL, SoLuongCan FROM DinhLuong WHERE MaSP = @MaSP";
                SqlCommand cmdCongThuc = new SqlCommand(sqlCongThuc, conn);
                cmdCongThuc.Parameters.AddWithValue("@MaSP", maSP);

                // Dùng 'using' để tự đóng DataReader
                using (SqlDataReader reader = cmdCongThuc.ExecuteReader()) {
                    // Nếu không có công thức, luôn đủ hàng
                    if (!reader.HasRows) {
                        return true;
                    }

                    // Lặp qua từng nguyên liệu trong công thức
                    while (reader.Read()) {
                        int maNL = (int)reader["MaNL"];
                        decimal soLuongCanMotLy = (decimal)reader["SoLuongCan"];
                        decimal tongNguyenLieuCan = soLuongCanMotLy * soLuongMuonKiemTra;

                        // 2. Lấy số lượng tồn kho của nguyên liệu đó
                        // (Phải đóng reader cũ trước khi chạy lệnh mới)
                        reader.Close(); // <-- Đóng reader tạm thời

                        string sqlKho = "SELECT TenNL, SoLuongTon FROM NguyenLieu WHERE MaNL = @MaNL AND TrangThai = N'Đang kinh doanh'";
                        SqlCommand cmdKho = new SqlCommand(sqlKho, conn);
                        cmdKho.Parameters.AddWithValue("@MaNL", maNL);

                        decimal soLuongTonThucTe = 0;
                        string tenNL = "";

                        using (SqlDataReader readerKho = cmdKho.ExecuteReader()) {
                            if (readerKho.Read()) {
                                tenNL = readerKho["TenNL"].ToString();
                                soLuongTonThucTe = (decimal)readerKho["SoLuongTon"];
                            }
                            else {
                                // Không tìm thấy nguyên liệu (đã bị ngừng kinh doanh)
                                MessageBox.Show($"Lỗi CSDL: Không tìm thấy nguyên liệu (ID: {maNL})");
                                return false;
                            }
                        } // Tự động đóng readerKho

                        // 3. So sánh
                        if (soLuongTonThucTe < tongNguyenLieuCan) {
                            MessageBox.Show(
                                $"Không đủ hàng trong kho cho {soLuongMuonKiemTra} ly!\n\n" +
                                $"Nguyên liệu: {tenNL}\n" +
                                $"Chỉ còn: {soLuongTonThucTe}\n" +
                                $"Cần: {tongNguyenLieuCan}",
                                "Hết Hàng",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false; // KHÔNG ĐỦ
                        }

                        // Mở lại reader chính để tiếp tục vòng lặp 'while'
                        reader.Read(); // (Bỏ qua vì chúng ta đã dùng return, nhưng đây là cách làm nếu không return)
                                       // Thực ra, chúng ta cần tải công thức vào List trước
                                       // Đây là một lỗi logic phức tạp của ADO.NET (nested reader)
                                       // Chúng ta sẽ sửa lại hàm này một chút:
                    }
                } // Tự động đóng reader
            }

            // Nếu lặp hết mà không 'return false'
            return true; // TẤT CẢ ĐỀU ĐỦ
        }

        /// (ADO.NET) Sửa lại hàm KiemTraSoLuongTonThucTe để tránh lỗi nested reader

        public bool KiemTraSoLuongTonThucTe_ADO_FIXED(int maSP, int soLuongMuonKiemTra) {
            // Tạo một "cái hộp" tạm để lưu công thức
            var congThuc = new List<DinhLuong>(); // Dùng lại Model "từ vựng"

            using (SqlConnection conn = new SqlConnection(_connectionString)) {
                conn.Open();

                // 1. Tải CÔNG THỨC về List C# trước
                string sqlCongThuc = "SELECT MaNL, SoLuongCan FROM DinhLuong WHERE MaSP = @MaSP";
                SqlCommand cmdCongThuc = new SqlCommand(sqlCongThuc, conn);
                cmdCongThuc.Parameters.AddWithValue("@MaSP", maSP);

                using (SqlDataReader reader = cmdCongThuc.ExecuteReader()) {
                    while (reader.Read()) {
                        congThuc.Add(new DinhLuong {
                            MaNl = (int)reader["MaNL"],
                            SoLuongCan = (decimal)reader["SoLuongCan"]
                        });
                    }
                } // Reader tự động đóng

                // Nếu không có công thức, luôn đủ
                if (congThuc.Count == 0) {
                    return true;
                }

                // 2. Lặp qua List C# (không còn giữ kết nối)
                foreach (var nguyenLieuCan in congThuc) {
                    decimal tongNguyenLieuCan = nguyenLieuCan.SoLuongCan * soLuongMuonKiemTra;

                    // 3. Bây giờ mới chạy truy vấn thứ 2
                    string sqlKho = "SELECT TenNL, SoLuongTon FROM NguyenLieu WHERE MaNL = @MaNL AND TrangThai = N'Đang kinh doanh'";
                    SqlCommand cmdKho = new SqlCommand(sqlKho, conn);
                    cmdKho.Parameters.AddWithValue("@MaNL", nguyenLieuCan.MaNl);

                    decimal soLuongTonThucTe = 0;
                    string tenNL = "";

                    using (SqlDataReader readerKho = cmdKho.ExecuteReader()) {
                        if (readerKho.Read()) {
                            tenNL = readerKho["TenNL"].ToString();
                            soLuongTonThucTe = (decimal)readerKho["SoLuongTon"];
                        }
                        else {
                            MessageBox.Show($"Lỗi CSDL: Không tìm thấy nguyên liệu (ID: {nguyenLieuCan.MaNl})");
                            return false;
                        }
                    } // readerKho tự đóng

                    // 4. So sánh
                    if (soLuongTonThucTe < tongNguyenLieuCan) {
                        MessageBox.Show(
                            $"Không đủ hàng trong kho cho {soLuongMuonKiemTra} ly!\n\n" +
                            $"Nguyên liệu: {tenNL}\n" +
                            $"Chỉ còn: {soLuongTonThucTe}\n" +
                            $"Cần: {tongNguyenLieuCan}",
                            "Hết Hàng",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false; // KHÔNG ĐỦ
                    }
                }
            } // Kết nối tự động đóng

            return true; // TẤT CẢ ĐỀU ĐỦ
        }
    } // Kết thúc class
}