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
            panel1 = new Panel();
            lvDonHang = new ListView();
            TenSP = new ColumnHeader();
            SL = new ColumnHeader();
            DonGia = new ColumnHeader();
            ThanhTien = new ColumnHeader();
            panel2 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnTimKH = new Button();
            txtTimKiemKH = new TextBox();
            label5 = new Label();
            lblTenKH = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnXoaMon = new Button();
            btnGIamSoLuong = new Button();
            panelTongTien = new Panel();
            lblTongCong = new Label();
            label2 = new Label();
            label1 = new Label();
            panelCol2 = new Panel();
            flpSanPham = new FlowLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            txtTimKiemSP = new TextBox();
            label6 = new Label();
            label3 = new Label();
            panelCol3 = new Panel();
            flpLoaiSP = new FlowLayoutPanel();
            label4 = new Label();
            panelChucNang = new Panel();
            btnHuyDon = new Button();
            btnThanhToan = new Button();
            tlpMain.SuspendLayout();
            panelCol1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panelTongTien.SuspendLayout();
            panelCol2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
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
            tlpMain.Size = new Size(1772, 1048);
            tlpMain.TabIndex = 0;
            // 
            // panelCol1
            // 
            panelCol1.BackColor = Color.FromArgb(0, 0, 0, 100);
            panelCol1.Controls.Add(panel1);
            panelCol1.Controls.Add(panelTongTien);
            panelCol1.Controls.Add(label1);
            panelCol1.Dock = DockStyle.Fill;
            panelCol1.ForeColor = Color.Black;
            panelCol1.Location = new Point(2, 2);
            panelCol1.Margin = new Padding(2);
            panelCol1.Name = "panelCol1";
            panelCol1.Size = new Size(616, 1044);
            panelCol1.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(lvDonHang);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 38);
            panel1.Name = "panel1";
            panel1.Size = new Size(616, 883);
            panel1.TabIndex = 6;
            // 
            // lvDonHang
            // 
            lvDonHang.BackColor = SystemColors.ButtonFace;
            lvDonHang.Columns.AddRange(new ColumnHeader[] { TenSP, SL, DonGia, ThanhTien });
            lvDonHang.Dock = DockStyle.Fill;
            lvDonHang.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lvDonHang.ForeColor = SystemColors.MenuText;
            lvDonHang.Location = new Point(0, 111);
            lvDonHang.Margin = new Padding(2);
            lvDonHang.Name = "lvDonHang";
            lvDonHang.Size = new Size(616, 712);
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
            // panel2
            // 
            panel2.Controls.Add(tableLayoutPanel2);
            panel2.Controls.Add(lblTenKH);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(616, 111);
            panel2.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.1314163F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.86858F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 128F));
            tableLayoutPanel2.Controls.Add(btnTimKH, 2, 0);
            tableLayoutPanel2.Controls.Add(txtTimKiemKH, 1, 0);
            tableLayoutPanel2.Controls.Add(label5, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Top;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(616, 58);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // btnTimKH
            // 
            btnTimKH.Dock = DockStyle.Fill;
            btnTimKH.Location = new Point(490, 3);
            btnTimKH.Name = "btnTimKH";
            btnTimKH.Size = new Size(123, 52);
            btnTimKH.TabIndex = 1;
            btnTimKH.Text = "Tìm";
            btnTimKH.UseVisualStyleBackColor = true;
            btnTimKH.Click += btnTimKH_Click;
            // 
            // txtTimKiemKH
            // 
            txtTimKiemKH.Dock = DockStyle.Fill;
            txtTimKiemKH.Location = new Point(140, 3);
            txtTimKiemKH.Multiline = true;
            txtTimKiemKH.Name = "txtTimKiemKH";
            txtTimKiemKH.Size = new Size(344, 52);
            txtTimKiemKH.TabIndex = 2;
            txtTimKiemKH.TextChanged += txtTimKiemKH_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BorderStyle = BorderStyle.Fixed3D;
            label5.Dock = DockStyle.Fill;
            label5.FlatStyle = FlatStyle.Flat;
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(131, 58);
            label5.TabIndex = 0;
            label5.Text = "Tìm SĐT KH\r\n:";
            // 
            // lblTenKH
            // 
            lblTenKH.AutoSize = true;
            lblTenKH.Location = new Point(183, 58);
            lblTenKH.Name = "lblTenKH";
            lblTenKH.Size = new Size(207, 41);
            lblTenKH.TabIndex = 4;
            lblTenKH.Text = "Khách vãng lai";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(btnXoaMon, 0, 0);
            tableLayoutPanel1.Controls.Add(btnGIamSoLuong, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Bottom;
            tableLayoutPanel1.Location = new Point(0, 823);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(616, 60);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // btnXoaMon
            // 
            btnXoaMon.Dock = DockStyle.Fill;
            btnXoaMon.FlatStyle = FlatStyle.Flat;
            btnXoaMon.Location = new Point(3, 3);
            btnXoaMon.Name = "btnXoaMon";
            btnXoaMon.Size = new Size(302, 54);
            btnXoaMon.TabIndex = 3;
            btnXoaMon.Text = "Xoá Món";
            btnXoaMon.UseVisualStyleBackColor = true;
            btnXoaMon.Click += btnXoaMon_Click;
            // 
            // btnGIamSoLuong
            // 
            btnGIamSoLuong.Dock = DockStyle.Fill;
            btnGIamSoLuong.FlatStyle = FlatStyle.Flat;
            btnGIamSoLuong.Location = new Point(311, 3);
            btnGIamSoLuong.Name = "btnGIamSoLuong";
            btnGIamSoLuong.Size = new Size(302, 54);
            btnGIamSoLuong.TabIndex = 4;
            btnGIamSoLuong.Text = "GIảm SL";
            btnGIamSoLuong.UseVisualStyleBackColor = true;
            btnGIamSoLuong.Click += btnGIamSoLuong_Click;
            // 
            // panelTongTien
            // 
            panelTongTien.Controls.Add(lblTongCong);
            panelTongTien.Controls.Add(label2);
            panelTongTien.Dock = DockStyle.Bottom;
            panelTongTien.Font = new Font("Times New Roman", 15.9000006F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panelTongTien.Location = new Point(0, 921);
            panelTongTien.Margin = new Padding(2);
            panelTongTien.Name = "panelTongTien";
            panelTongTien.Size = new Size(616, 123);
            panelTongTien.TabIndex = 2;
            // 
            // lblTongCong
            // 
            lblTongCong.AutoSize = true;
            lblTongCong.Dock = DockStyle.Fill;
            lblTongCong.Location = new Point(283, 0);
            lblTongCong.Margin = new Padding(2, 0, 2, 0);
            lblTongCong.Name = "lblTongCong";
            lblTongCong.Size = new Size(84, 62);
            lblTongCong.TabIndex = 1;
            lblTongCong.Text = "0đ";
            lblTongCong.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Left;
            label2.Location = new Point(0, 0);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(283, 62);
            label2.TabIndex = 0;
            label2.Text = "Tổng cộng:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Times New Roman", 9.900001F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(136, 19);
            label1.TabIndex = 0;
            label1.Text = "Chi tiết đơn hàng";
            // 
            // panelCol2
            // 
            panelCol2.Controls.Add(flpSanPham);
            panelCol2.Controls.Add(tableLayoutPanel3);
            panelCol2.Controls.Add(label3);
            panelCol2.Dock = DockStyle.Fill;
            panelCol2.Location = new Point(622, 2);
            panelCol2.Margin = new Padding(2);
            panelCol2.Name = "panelCol2";
            panelCol2.Size = new Size(704, 1044);
            panelCol2.TabIndex = 3;
            // 
            // flpSanPham
            // 
            flpSanPham.AutoScroll = true;
            flpSanPham.BackColor = Color.WhiteSmoke;
            flpSanPham.Dock = DockStyle.Fill;
            flpSanPham.Location = new Point(0, 96);
            flpSanPham.Margin = new Padding(2);
            flpSanPham.Name = "flpSanPham";
            flpSanPham.Size = new Size(704, 948);
            flpSanPham.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.1875F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 82.8125F));
            tableLayoutPanel3.Controls.Add(txtTimKiemSP, 1, 0);
            tableLayoutPanel3.Controls.Add(label6, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Top;
            tableLayoutPanel3.Location = new Point(0, 38);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(704, 58);
            tableLayoutPanel3.TabIndex = 3;
            tableLayoutPanel3.Paint += tableLayoutPanel3_Paint;
            // 
            // txtTimKiemSP
            // 
            txtTimKiemSP.Dock = DockStyle.Fill;
            txtTimKiemSP.Location = new Point(124, 3);
            txtTimKiemSP.Name = "txtTimKiemSP";
            txtTimKiemSP.Size = new Size(577, 47);
            txtTimKiemSP.TabIndex = 1;
            txtTimKiemSP.TextChanged += txtTimKiemSP_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(3, 0);
            label6.Name = "label6";
            label6.Size = new Size(115, 58);
            label6.TabIndex = 0;
            label6.Text = "Tìm SP:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("Times New Roman", 10F, FontStyle.Bold);
            label3.Location = new Point(0, 0);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(157, 19);
            label3.TabIndex = 0;
            label3.Text = "Danh sách Sản phẩm";
            // 
            // panelCol3
            // 
            panelCol3.BackColor = Color.FromArgb(0, 0, 0, 100);
            panelCol3.Controls.Add(flpLoaiSP);
            panelCol3.Controls.Add(label4);
            panelCol3.Controls.Add(panelChucNang);
            panelCol3.Dock = DockStyle.Fill;
            panelCol3.ForeColor = SystemColors.ControlText;
            panelCol3.Location = new Point(1330, 2);
            panelCol3.Margin = new Padding(2);
            panelCol3.Name = "panelCol3";
            panelCol3.Size = new Size(440, 1044);
            panelCol3.TabIndex = 4;
            // 
            // flpLoaiSP
            // 
            flpLoaiSP.AutoScroll = true;
            flpLoaiSP.BackColor = Color.FromArgb(0, 0, 0, 100);
            flpLoaiSP.Dock = DockStyle.Fill;
            flpLoaiSP.FlowDirection = FlowDirection.TopDown;
            flpLoaiSP.Location = new Point(0, 38);
            flpLoaiSP.Margin = new Padding(2);
            flpLoaiSP.Name = "flpLoaiSP";
            flpLoaiSP.Size = new Size(440, 883);
            flpLoaiSP.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Font = new Font("Times New Roman", 9.900001F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(0, 0);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(117, 19);
            label4.TabIndex = 1;
            label4.Text = "Loại Sản phẩm";
            // 
            // panelChucNang
            // 
            panelChucNang.Controls.Add(btnHuyDon);
            panelChucNang.Controls.Add(btnThanhToan);
            panelChucNang.Dock = DockStyle.Bottom;
            panelChucNang.Location = new Point(0, 921);
            panelChucNang.Margin = new Padding(2);
            panelChucNang.Name = "panelChucNang";
            panelChucNang.Size = new Size(440, 123);
            panelChucNang.TabIndex = 0;
            // 
            // btnHuyDon
            // 
            btnHuyDon.BackColor = Color.FromArgb(255, 128, 128);
            btnHuyDon.Dock = DockStyle.Bottom;
            btnHuyDon.FlatAppearance.BorderSize = 0;
            btnHuyDon.FlatStyle = FlatStyle.Flat;
            btnHuyDon.Font = new Font("Times New Roman", 12F);
            btnHuyDon.Location = new Point(0, 66);
            btnHuyDon.Margin = new Padding(2);
            btnHuyDon.Name = "btnHuyDon";
            btnHuyDon.Size = new Size(440, 57);
            btnHuyDon.TabIndex = 1;
            btnHuyDon.Text = "Huỷ đơn";
            btnHuyDon.UseVisualStyleBackColor = false;
            btnHuyDon.Click += btnHuyDon_Click;
            // 
            // btnThanhToan
            // 
            btnThanhToan.BackColor = Color.YellowGreen;
            btnThanhToan.FlatStyle = FlatStyle.Flat;
            btnThanhToan.Font = new Font("Times New Roman", 12F);
            btnThanhToan.Location = new Point(0, 0);
            btnThanhToan.Margin = new Padding(2);
            btnThanhToan.Name = "btnThanhToan";
            btnThanhToan.Size = new Size(440, 57);
            btnThanhToan.TabIndex = 0;
            btnThanhToan.Text = "Thanh Toán";
            btnThanhToan.UseVisualStyleBackColor = false;
            btnThanhToan.Click += btnThanhToan_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1772, 1048);
            Controls.Add(tlpMain);
            Margin = new Padding(2);
            Name = "MainForm";
            Text = "Order";
            Load += MainForm_Load;
            tlpMain.ResumeLayout(false);
            panelCol1.ResumeLayout(false);
            panelCol1.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            panelTongTien.ResumeLayout(false);
            panelTongTien.PerformLayout();
            panelCol2.ResumeLayout(false);
            panelCol2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            panelCol3.ResumeLayout(false);
            panelCol3.PerformLayout();
            panelChucNang.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label label1;
        private Panel panelCol1;
        private Panel panelTongTien;
        private Label label2;
        private Label lblTongCong;
        private Panel panelCol2;
        private FlowLayoutPanel flpSanPham;
        private Label label3;
        private Panel panelCol3;
        private Panel panelChucNang;
        private Button btnThanhToan;
        private Button btnHuyDon;
        private FlowLayoutPanel flpLoaiSP;
        private Label label4;
        private ListView lvDonHang;
        private ColumnHeader TenSP;
        private ColumnHeader SL;
        private ColumnHeader DonGia;
        private ColumnHeader ThanhTien;
        private Button btnXoaMon;
        private Button btnGIamSoLuong;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox txtTimKiemKH;
        private Button btnTimKH;
        private Label label5;
        private Label lblTenKH;
        private Panel panel2;
        private Label label6;
        private TextBox txtTimKiemSP;
        private TableLayoutPanel tableLayoutPanel3;
    }
}