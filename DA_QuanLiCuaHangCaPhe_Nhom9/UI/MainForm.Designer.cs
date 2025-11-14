namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tlpMain = new TableLayoutPanel();
            panelCol1 = new Panel();
            panel_C1 = new Panel();
            lvDonHang = new ListView();
            TenSP = new ColumnHeader();
            SL = new ColumnHeader();
            DonGia = new ColumnHeader();
            ThanhTien = new ColumnHeader();
            panel_KH = new Panel();
            frame_KhachHang = new TableLayoutPanel();
            lblTenKH = new Label();
            frame_TimKH = new TableLayoutPanel();
            btnThem = new Button();
            txtTimKiemKH = new TextBox();
            label5 = new Label();
            frame_btnX_btnG = new TableLayoutPanel();
            btnXoaMon = new Button();
            btnGIamSoLuong = new Button();
            panelTongTien = new Panel();
            lblGiamGia = new Label();
            lblTienTruocGiam = new Label();
            lblTongCong = new Label();
            label1 = new Label();
            label2 = new Label();
            lbl_text_TT = new Label();
            lbl_title = new Label();
            panelCol2 = new Panel();
            flpSanPham = new FlowLayoutPanel();
            frame_TimSP = new TableLayoutPanel();
            txtTimKiemSP = new TextBox();
            lbl_textTimSP = new Label();
            lbl_title_2 = new Label();
            panelCol3 = new Panel();
            flpLoaiSP = new FlowLayoutPanel();
            lbl_title3 = new Label();
            panelChucNang = new Panel();
            btnLuuTam = new Button();
            btnHuyDon = new Button();
            btnThanhToan = new Button();
            tlpMain.SuspendLayout();
            panelCol1.SuspendLayout();
            panel_C1.SuspendLayout();
            panel_KH.SuspendLayout();
            frame_KhachHang.SuspendLayout();
            frame_TimKH.SuspendLayout();
            frame_btnX_btnG.SuspendLayout();
            panelTongTien.SuspendLayout();
            panelCol2.SuspendLayout();
            frame_TimSP.SuspendLayout();
            panelCol3.SuspendLayout();
            panelChucNang.SuspendLayout();
            SuspendLayout();
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 3;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpMain.Controls.Add(panelCol1, 0, 0);
            tlpMain.Controls.Add(panelCol2, 1, 0);
            tlpMain.Controls.Add(panelCol3, 2, 0);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 0);
            tlpMain.Margin = new Padding(2);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 1;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpMain.Size = new Size(905, 515);
            tlpMain.TabIndex = 0;
            // 
            // panelCol1
            // 
            panelCol1.BackColor = Color.FromArgb(0, 0, 0, 100);
            panelCol1.Controls.Add(panel_C1);
            panelCol1.Controls.Add(panelTongTien);
            panelCol1.Controls.Add(lbl_title);
            panelCol1.Dock = DockStyle.Fill;
            panelCol1.ForeColor = Color.Black;
            panelCol1.Location = new Point(2, 2);
            panelCol1.Margin = new Padding(2);
            panelCol1.Name = "panelCol1";
            panelCol1.Size = new Size(312, 511);
            panelCol1.TabIndex = 2;
            // 
            // panel_C1
            // 
            panel_C1.Controls.Add(lvDonHang);
            panel_C1.Controls.Add(panel_KH);
            panel_C1.Controls.Add(frame_btnX_btnG);
            panel_C1.Dock = DockStyle.Fill;
            panel_C1.Location = new Point(0, 19);
            panel_C1.Name = "panel_C1";
            panel_C1.Size = new Size(312, 360);
            panel_C1.TabIndex = 6;
            // 
            // lvDonHang
            // 
            lvDonHang.BackColor = SystemColors.ButtonFace;
            lvDonHang.Columns.AddRange(new ColumnHeader[] { TenSP, SL, DonGia, ThanhTien });
            lvDonHang.Dock = DockStyle.Fill;
            lvDonHang.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lvDonHang.ForeColor = SystemColors.MenuText;
            lvDonHang.Location = new Point(0, 63);
            lvDonHang.Margin = new Padding(2);
            lvDonHang.Name = "lvDonHang";
            lvDonHang.Size = new Size(312, 237);
            lvDonHang.TabIndex = 1;
            lvDonHang.UseCompatibleStateImageBehavior = false;
            lvDonHang.View = View.Details;
            // 
            // TenSP
            // 
            TenSP.Text = "Tên SP";
            TenSP.Width = 200;
            // 
            // SL
            // 
            SL.Text = "SL";
            // 
            // DonGia
            // 
            DonGia.Text = "Đơn Giá";
            DonGia.Width = 190;
            // 
            // ThanhTien
            // 
            ThanhTien.Text = "Thành Tiền";
            ThanhTien.Width = 190;
            // 
            // panel_KH
            // 
            panel_KH.Controls.Add(frame_KhachHang);
            panel_KH.Controls.Add(frame_TimKH);
            panel_KH.Dock = DockStyle.Top;
            panel_KH.Location = new Point(0, 0);
            panel_KH.Name = "panel_KH";
            panel_KH.Size = new Size(312, 63);
            panel_KH.TabIndex = 5;
            // 
            // frame_KhachHang
            // 
            frame_KhachHang.ColumnCount = 1;
            frame_KhachHang.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48.4693871F));
            frame_KhachHang.Controls.Add(lblTenKH, 0, 0);
            frame_KhachHang.Dock = DockStyle.Fill;
            frame_KhachHang.Location = new Point(0, 41);
            frame_KhachHang.Margin = new Padding(6);
            frame_KhachHang.Name = "frame_KhachHang";
            frame_KhachHang.RowCount = 1;
            frame_KhachHang.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            frame_KhachHang.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            frame_KhachHang.Size = new Size(312, 22);
            frame_KhachHang.TabIndex = 6;
            // 
            // lblTenKH
            // 
            lblTenKH.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblTenKH.Location = new Point(6, 0);
            lblTenKH.Margin = new Padding(6, 0, 6, 0);
            lblTenKH.Name = "lblTenKH";
            lblTenKH.Size = new Size(300, 22);
            lblTenKH.TabIndex = 4;
            lblTenKH.Text = "Khách vãng lai";
            lblTenKH.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frame_TimKH
            // 
            frame_TimKH.ColumnCount = 3;
            frame_TimKH.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.1314163F));
            frame_TimKH.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.86858F));
            frame_TimKH.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 99F));
            frame_TimKH.Controls.Add(btnThem, 2, 0);
            frame_TimKH.Controls.Add(txtTimKiemKH, 1, 0);
            frame_TimKH.Controls.Add(label5, 0, 0);
            frame_TimKH.Dock = DockStyle.Top;
            frame_TimKH.Location = new Point(0, 0);
            frame_TimKH.Name = "frame_TimKH";
            frame_TimKH.RowCount = 1;
            frame_TimKH.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            frame_TimKH.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            frame_TimKH.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            frame_TimKH.Size = new Size(312, 41);
            frame_TimKH.TabIndex = 3;
            // 
            // btnThem
            // 
            btnThem.Dock = DockStyle.Fill;
            btnThem.Location = new Point(218, 6);
            btnThem.Margin = new Padding(6);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(88, 29);
            btnThem.TabIndex = 5;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // txtTimKiemKH
            // 
            txtTimKiemKH.Dock = DockStyle.Fill;
            txtTimKiemKH.Location = new Point(65, 6);
            txtTimKiemKH.Margin = new Padding(6);
            txtTimKiemKH.MaxLength = 10;
            txtTimKiemKH.Name = "txtTimKiemKH";
            txtTimKiemKH.Size = new Size(141, 27);
            txtTimKiemKH.TabIndex = 2;
            txtTimKiemKH.TextChanged += txtTimKiemKH_TextChanged;
            txtTimKiemKH.KeyPress += txtTimKiemKH_KeyPress;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label5.BorderStyle = BorderStyle.Fixed3D;
            label5.FlatStyle = FlatStyle.Flat;
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(53, 41);
            label5.TabIndex = 0;
            label5.Text = "Tìm SĐT KH\r\n";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frame_btnX_btnG
            // 
            frame_btnX_btnG.ColumnCount = 2;
            frame_btnX_btnG.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            frame_btnX_btnG.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            frame_btnX_btnG.Controls.Add(btnXoaMon, 0, 0);
            frame_btnX_btnG.Controls.Add(btnGIamSoLuong, 1, 0);
            frame_btnX_btnG.Dock = DockStyle.Bottom;
            frame_btnX_btnG.Location = new Point(0, 300);
            frame_btnX_btnG.Name = "frame_btnX_btnG";
            frame_btnX_btnG.RowCount = 1;
            frame_btnX_btnG.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            frame_btnX_btnG.Size = new Size(312, 60);
            frame_btnX_btnG.TabIndex = 5;
            // 
            // btnXoaMon
            // 
            btnXoaMon.Dock = DockStyle.Fill;
            btnXoaMon.FlatStyle = FlatStyle.Flat;
            btnXoaMon.Location = new Point(3, 3);
            btnXoaMon.Name = "btnXoaMon";
            btnXoaMon.Size = new Size(150, 54);
            btnXoaMon.TabIndex = 3;
            btnXoaMon.Text = "Xoá Món";
            btnXoaMon.UseVisualStyleBackColor = true;
            btnXoaMon.Click += btnXoaMon_Click;
            // 
            // btnGIamSoLuong
            // 
            btnGIamSoLuong.Dock = DockStyle.Fill;
            btnGIamSoLuong.FlatStyle = FlatStyle.Flat;
            btnGIamSoLuong.Location = new Point(159, 3);
            btnGIamSoLuong.Name = "btnGIamSoLuong";
            btnGIamSoLuong.Size = new Size(150, 54);
            btnGIamSoLuong.TabIndex = 4;
            btnGIamSoLuong.Text = "GIảm SL";
            btnGIamSoLuong.UseVisualStyleBackColor = true;
            btnGIamSoLuong.Click += btnGIamSoLuong_Click;
            // 
            // panelTongTien
            // 
            panelTongTien.Controls.Add(lblGiamGia);
            panelTongTien.Controls.Add(lblTienTruocGiam);
            panelTongTien.Controls.Add(lblTongCong);
            panelTongTien.Controls.Add(label1);
            panelTongTien.Controls.Add(label2);
            panelTongTien.Controls.Add(lbl_text_TT);
            panelTongTien.Dock = DockStyle.Bottom;
            panelTongTien.Font = new Font("Times New Roman", 15.9000006F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panelTongTien.Location = new Point(0, 379);
            panelTongTien.Margin = new Padding(2);
            panelTongTien.Name = "panelTongTien";
            panelTongTien.Size = new Size(312, 132);
            panelTongTien.TabIndex = 2;
            // 
            // lblGiamGia
            // 
            lblGiamGia.AutoSize = true;
            lblGiamGia.Location = new Point(173, 42);
            lblGiamGia.Margin = new Padding(1, 0, 1, 0);
            lblGiamGia.Name = "lblGiamGia";
            lblGiamGia.Size = new Size(50, 31);
            lblGiamGia.TabIndex = 2;
            lblGiamGia.Text = "0 đ";
            lblGiamGia.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTienTruocGiam
            // 
            lblTienTruocGiam.AutoSize = true;
            lblTienTruocGiam.Location = new Point(242, 3);
            lblTienTruocGiam.Margin = new Padding(1, 0, 1, 0);
            lblTienTruocGiam.Name = "lblTienTruocGiam";
            lblTienTruocGiam.Size = new Size(50, 31);
            lblTienTruocGiam.TabIndex = 2;
            lblTienTruocGiam.Text = "0 đ";
            lblTienTruocGiam.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTongCong
            // 
            lblTongCong.AutoSize = true;
            lblTongCong.Location = new Point(141, 92);
            lblTongCong.Margin = new Padding(2, 0, 2, 0);
            lblTongCong.Name = "lblTongCong";
            lblTongCong.Size = new Size(50, 31);
            lblTongCong.TabIndex = 1;
            lblTongCong.Text = "0 đ";
            lblTongCong.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(-11, -6);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(260, 31);
            label1.TabIndex = 0;
            label1.Text = "Tiền trước giảm giá: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(-2, 43);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(137, 31);
            label2.TabIndex = 0;
            label2.Text = "Giảm giá: ";
            // 
            // lbl_text_TT
            // 
            lbl_text_TT.AutoSize = true;
            lbl_text_TT.Location = new Point(-2, 92);
            lbl_text_TT.Margin = new Padding(2, 0, 2, 0);
            lbl_text_TT.Name = "lbl_text_TT";
            lbl_text_TT.Size = new Size(151, 31);
            lbl_text_TT.TabIndex = 0;
            lbl_text_TT.Text = "Thành tiền:";
            // 
            // lbl_title
            // 
            lbl_title.AutoSize = true;
            lbl_title.BackColor = Color.Transparent;
            lbl_title.Dock = DockStyle.Top;
            lbl_title.Font = new Font("Times New Roman", 9.900001F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_title.Location = new Point(0, 0);
            lbl_title.Margin = new Padding(2, 0, 2, 0);
            lbl_title.Name = "lbl_title";
            lbl_title.Size = new Size(136, 19);
            lbl_title.TabIndex = 0;
            lbl_title.Text = "Chi tiết đơn hàng";
            // 
            // panelCol2
            // 
            panelCol2.Controls.Add(flpSanPham);
            panelCol2.Controls.Add(frame_TimSP);
            panelCol2.Controls.Add(lbl_title_2);
            panelCol2.Dock = DockStyle.Fill;
            panelCol2.Location = new Point(318, 2);
            panelCol2.Margin = new Padding(2);
            panelCol2.Name = "panelCol2";
            panelCol2.Size = new Size(358, 511);
            panelCol2.TabIndex = 3;
            // 
            // flpSanPham
            // 
            flpSanPham.AutoScroll = true;
            flpSanPham.BackColor = Color.WhiteSmoke;
            flpSanPham.Dock = DockStyle.Fill;
            flpSanPham.Location = new Point(0, 51);
            flpSanPham.Margin = new Padding(2);
            flpSanPham.Name = "flpSanPham";
            flpSanPham.Size = new Size(358, 460);
            flpSanPham.TabIndex = 1;
            // 
            // frame_TimSP
            // 
            frame_TimSP.ColumnCount = 2;
            frame_TimSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.1875F));
            frame_TimSP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 82.8125F));
            frame_TimSP.Controls.Add(txtTimKiemSP, 1, 0);
            frame_TimSP.Controls.Add(lbl_textTimSP, 0, 0);
            frame_TimSP.Dock = DockStyle.Top;
            frame_TimSP.Location = new Point(0, 19);
            frame_TimSP.Name = "frame_TimSP";
            frame_TimSP.RowCount = 1;
            frame_TimSP.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            frame_TimSP.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            frame_TimSP.Size = new Size(358, 32);
            frame_TimSP.TabIndex = 3;
            // 
            // txtTimKiemSP
            // 
            txtTimKiemSP.Dock = DockStyle.Fill;
            txtTimKiemSP.Location = new Point(64, 3);
            txtTimKiemSP.Name = "txtTimKiemSP";
            txtTimKiemSP.Size = new Size(291, 27);
            txtTimKiemSP.TabIndex = 1;
            txtTimKiemSP.TextChanged += txtTimKiemSP_TextChanged;
            // 
            // lbl_textTimSP
            // 
            lbl_textTimSP.AutoSize = true;
            lbl_textTimSP.Dock = DockStyle.Fill;
            lbl_textTimSP.Location = new Point(3, 0);
            lbl_textTimSP.Name = "lbl_textTimSP";
            lbl_textTimSP.Size = new Size(55, 32);
            lbl_textTimSP.TabIndex = 0;
            lbl_textTimSP.Text = "Tìm SP:";
            // 
            // lbl_title_2
            // 
            lbl_title_2.AutoSize = true;
            lbl_title_2.Dock = DockStyle.Top;
            lbl_title_2.Font = new Font("Times New Roman", 10F, FontStyle.Bold);
            lbl_title_2.Location = new Point(0, 0);
            lbl_title_2.Margin = new Padding(2, 0, 2, 0);
            lbl_title_2.Name = "lbl_title_2";
            lbl_title_2.Size = new Size(157, 19);
            lbl_title_2.TabIndex = 0;
            lbl_title_2.Text = "Danh sách Sản phẩm";
            // 
            // panelCol3
            // 
            panelCol3.BackColor = Color.FromArgb(0, 0, 0, 100);
            panelCol3.Controls.Add(flpLoaiSP);
            panelCol3.Controls.Add(lbl_title3);
            panelCol3.Controls.Add(panelChucNang);
            panelCol3.Dock = DockStyle.Fill;
            panelCol3.ForeColor = SystemColors.ControlText;
            panelCol3.Location = new Point(680, 2);
            panelCol3.Margin = new Padding(2);
            panelCol3.Name = "panelCol3";
            panelCol3.Size = new Size(223, 511);
            panelCol3.TabIndex = 4;
            // 
            // flpLoaiSP
            // 
            flpLoaiSP.AutoScroll = true;
            flpLoaiSP.BackColor = Color.FromArgb(0, 0, 0, 100);
            flpLoaiSP.Dock = DockStyle.Fill;
            flpLoaiSP.FlowDirection = FlowDirection.TopDown;
            flpLoaiSP.Location = new Point(0, 19);
            flpLoaiSP.Margin = new Padding(2);
            flpLoaiSP.Name = "flpLoaiSP";
            flpLoaiSP.Size = new Size(223, 319);
            flpLoaiSP.TabIndex = 2;
            flpLoaiSP.Paint += flpLoaiSP_Paint;
            // 
            // lbl_title3
            // 
            lbl_title3.AutoSize = true;
            lbl_title3.Dock = DockStyle.Top;
            lbl_title3.Font = new Font("Times New Roman", 9.900001F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_title3.Location = new Point(0, 0);
            lbl_title3.Margin = new Padding(2, 0, 2, 0);
            lbl_title3.Name = "lbl_title3";
            lbl_title3.Size = new Size(117, 19);
            lbl_title3.TabIndex = 1;
            lbl_title3.Text = "Loại Sản phẩm";
            // 
            // panelChucNang
            // 
            panelChucNang.Controls.Add(btnLuuTam);
            panelChucNang.Controls.Add(btnHuyDon);
            panelChucNang.Controls.Add(btnThanhToan);
            panelChucNang.Dock = DockStyle.Bottom;
            panelChucNang.Location = new Point(0, 338);
            panelChucNang.Margin = new Padding(2);
            panelChucNang.Name = "panelChucNang";
            panelChucNang.Size = new Size(223, 173);
            panelChucNang.TabIndex = 0;
            // 
            // btnLuuTam
            // 
            btnLuuTam.BackColor = Color.LightGoldenrodYellow;
            btnLuuTam.Dock = DockStyle.Top;
            btnLuuTam.FlatStyle = FlatStyle.Flat;
            btnLuuTam.Font = new Font("Times New Roman", 12F);
            btnLuuTam.Location = new Point(0, 57);
            btnLuuTam.Margin = new Padding(2);
            btnLuuTam.Name = "btnLuuTam";
            btnLuuTam.Size = new Size(223, 57);
            btnLuuTam.TabIndex = 2;
            btnLuuTam.Text = "Lưu Tạm";
            btnLuuTam.UseVisualStyleBackColor = false;
            btnLuuTam.Click += btnLuuTam_Click;
            // 
            // btnHuyDon
            // 
            btnHuyDon.BackColor = Color.FromArgb(255, 128, 128);
            btnHuyDon.Dock = DockStyle.Bottom;
            btnHuyDon.FlatAppearance.BorderSize = 0;
            btnHuyDon.FlatStyle = FlatStyle.Flat;
            btnHuyDon.Font = new Font("Times New Roman", 12F);
            btnHuyDon.Location = new Point(0, 116);
            btnHuyDon.Margin = new Padding(2);
            btnHuyDon.Name = "btnHuyDon";
            btnHuyDon.Size = new Size(223, 57);
            btnHuyDon.TabIndex = 1;
            btnHuyDon.Text = "Huỷ đơn";
            btnHuyDon.UseVisualStyleBackColor = false;
            btnHuyDon.Click += btnHuyDon_Click;
            // 
            // btnThanhToan
            // 
            btnThanhToan.BackColor = Color.YellowGreen;
            btnThanhToan.Dock = DockStyle.Top;
            btnThanhToan.FlatStyle = FlatStyle.Flat;
            btnThanhToan.Font = new Font("Times New Roman", 12F);
            btnThanhToan.Location = new Point(0, 0);
            btnThanhToan.Margin = new Padding(2);
            btnThanhToan.Name = "btnThanhToan";
            btnThanhToan.Size = new Size(223, 57);
            btnThanhToan.TabIndex = 0;
            btnThanhToan.Text = "Thanh Toán";
            btnThanhToan.UseVisualStyleBackColor = false;
            btnThanhToan.Click += btnThanhToan_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(905, 515);
            Controls.Add(tlpMain);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Margin = new Padding(2);
            Name = "MainForm";
            Text = "Order";
            WindowState = FormWindowState.Maximized;
            Load += MainForm_Load;
            tlpMain.ResumeLayout(false);
            panelCol1.ResumeLayout(false);
            panelCol1.PerformLayout();
            panel_C1.ResumeLayout(false);
            panel_KH.ResumeLayout(false);
            frame_KhachHang.ResumeLayout(false);
            frame_TimKH.ResumeLayout(false);
            frame_TimKH.PerformLayout();
            frame_btnX_btnG.ResumeLayout(false);
            panelTongTien.ResumeLayout(false);
            panelTongTien.PerformLayout();
            panelCol2.ResumeLayout(false);
            panelCol2.PerformLayout();
            frame_TimSP.ResumeLayout(false);
            frame_TimSP.PerformLayout();
            panelCol3.ResumeLayout(false);
            panelCol3.PerformLayout();
            panelChucNang.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label lbl_title;
        private Panel panelCol1;
        private Panel panelTongTien;
        private Label lbl_text_TT;
        private Label lblTongCong;
        private Panel panelCol2;
        private FlowLayoutPanel flpSanPham;
        private Label lbl_title_2;
        private Panel panelCol3;
        private Panel panelChucNang;
        private Button btnThanhToan;
        private Button btnHuyDon;
        private FlowLayoutPanel flpLoaiSP;
        private Label lbl_title3;
        private ListView lvDonHang;
        private ColumnHeader TenSP;
        private ColumnHeader SL;
        private ColumnHeader DonGia;
        private ColumnHeader ThanhTien;
        private Button btnXoaMon;
        private Button btnGIamSoLuong;
        private TableLayoutPanel frame_btnX_btnG;
        private Panel panel_C1;
        private TableLayoutPanel frame_TimKH;
        private TextBox txtTimKiemKH;
        private Label label5;
        private Panel panel_KH;
        private Label lbl_textTimSP;
        private TextBox txtTimKiemSP;
        private TableLayoutPanel frame_TimSP;
        private Button btnThem;
        private TableLayoutPanel frame_KhachHang;
        private Button btnLuuTam;
        private Label lblTenKH;
        private Label lblTienTruocGiam;
        private Label lblGiamGia;
        private Label label1;
        private Label label2;
    }
}