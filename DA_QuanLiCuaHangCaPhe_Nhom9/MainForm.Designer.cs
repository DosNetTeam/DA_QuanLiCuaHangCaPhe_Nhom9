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
            panelTongTien = new Panel();
            lblTongCong = new Label();
            label2 = new Label();
            lvDonHang = new ListView();
            TenSP = new ColumnHeader();
            SL = new ColumnHeader();
            DonGia = new ColumnHeader();
            ThanhTien = new ColumnHeader();
            label1 = new Label();
            panelCol2 = new Panel();
            flpSanPham = new FlowLayoutPanel();
            label3 = new Label();
            panelCol3 = new Panel();
            flpLoaiSP = new FlowLayoutPanel();
            label4 = new Label();
            panelChucNang = new Panel();
            btnHuyDon = new Button();
            btnThanhToan = new Button();
            tlpMain.SuspendLayout();
            panelCol1.SuspendLayout();
            panelTongTien.SuspendLayout();
            panelCol2.SuspendLayout();
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
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 1;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpMain.Size = new Size(1772, 1047);
            tlpMain.TabIndex = 0;
            // 
            // panelCol1
            // 
            panelCol1.Controls.Add(panelTongTien);
            panelCol1.Controls.Add(lvDonHang);
            panelCol1.Controls.Add(label1);
            panelCol1.Dock = DockStyle.Fill;
            panelCol1.Location = new Point(3, 3);
            panelCol1.Name = "panelCol1";
            panelCol1.Size = new Size(614, 1041);
            panelCol1.TabIndex = 2;
            // 
            // panelTongTien
            // 
            panelTongTien.Controls.Add(lblTongCong);
            panelTongTien.Controls.Add(label2);
            panelTongTien.Dock = DockStyle.Bottom;
            panelTongTien.Location = new Point(0, 791);
            panelTongTien.Name = "panelTongTien";
            panelTongTien.Size = new Size(614, 250);
            panelTongTien.TabIndex = 2;
            // 
            // lblTongCong
            // 
            lblTongCong.AutoSize = true;
            lblTongCong.Dock = DockStyle.Fill;
            lblTongCong.Location = new Point(169, 0);
            lblTongCong.Name = "lblTongCong";
            lblTongCong.Size = new Size(52, 41);
            lblTongCong.TabIndex = 1;
            lblTongCong.Text = "0đ";
            lblTongCong.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Left;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(169, 41);
            label2.TabIndex = 0;
            label2.Text = "Tổng cộng:";
            // 
            // lvDonHang
            // 
            lvDonHang.Columns.AddRange(new ColumnHeader[] { TenSP, SL, DonGia, ThanhTien });
            lvDonHang.Dock = DockStyle.Fill;
            lvDonHang.Location = new Point(0, 41);
            lvDonHang.Name = "lvDonHang";
            lvDonHang.Size = new Size(614, 1000);
            lvDonHang.TabIndex = 1;
            lvDonHang.UseCompatibleStateImageBehavior = false;
            lvDonHang.View = View.Details;
            // 
            // TenSP
            // 
            TenSP.Text = "Tên SP";
            TenSP.Width = 100;
            // 
            // SL
            // 
            SL.Text = "SL";
            // 
            // DonGia
            // 
            DonGia.Text = "Đơn Giá";
            DonGia.Width = 120;
            // 
            // ThanhTien
            // 
            ThanhTien.Text = "Thành Tiền";
            ThanhTien.Width = 120;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(248, 41);
            label1.TabIndex = 0;
            label1.Text = "Chi tiết đơn hàng";
            // 
            // panelCol2
            // 
            panelCol2.Controls.Add(flpSanPham);
            panelCol2.Controls.Add(label3);
            panelCol2.Dock = DockStyle.Fill;
            panelCol2.Location = new Point(623, 3);
            panelCol2.Name = "panelCol2";
            panelCol2.Size = new Size(702, 1041);
            panelCol2.TabIndex = 3;
            // 
            // flpSanPham
            // 
            flpSanPham.AutoScroll = true;
            flpSanPham.Dock = DockStyle.Fill;
            flpSanPham.Location = new Point(0, 41);
            flpSanPham.Name = "flpSanPham";
            flpSanPham.Size = new Size(702, 1000);
            flpSanPham.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(295, 41);
            label3.TabIndex = 0;
            label3.Text = "Danh sách Sản phẩm";
            // 
            // panelCol3
            // 
            panelCol3.Controls.Add(flpLoaiSP);
            panelCol3.Controls.Add(label4);
            panelCol3.Controls.Add(panelChucNang);
            panelCol3.Dock = DockStyle.Fill;
            panelCol3.Location = new Point(1331, 3);
            panelCol3.Name = "panelCol3";
            panelCol3.Size = new Size(438, 1041);
            panelCol3.TabIndex = 4;
            // 
            // flpLoaiSP
            // 
            flpLoaiSP.AutoScroll = true;
            flpLoaiSP.Dock = DockStyle.Fill;
            flpLoaiSP.FlowDirection = FlowDirection.TopDown;
            flpLoaiSP.Location = new Point(0, 41);
            flpLoaiSP.Name = "flpLoaiSP";
            flpLoaiSP.Size = new Size(438, 750);
            flpLoaiSP.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(212, 41);
            label4.TabIndex = 1;
            label4.Text = "Loại Sản phẩm";
            // 
            // panelChucNang
            // 
            panelChucNang.Controls.Add(btnHuyDon);
            panelChucNang.Controls.Add(btnThanhToan);
            panelChucNang.Dock = DockStyle.Bottom;
            panelChucNang.Location = new Point(0, 791);
            panelChucNang.Name = "panelChucNang";
            panelChucNang.Size = new Size(438, 250);
            panelChucNang.TabIndex = 0;
            // 
            // btnHuyDon
            // 
            btnHuyDon.Dock = DockStyle.Top;
            btnHuyDon.Location = new Point(0, 58);
            btnHuyDon.Name = "btnHuyDon";
            btnHuyDon.Size = new Size(438, 58);
            btnHuyDon.TabIndex = 1;
            btnHuyDon.Text = "Huỷ đơn";
            btnHuyDon.UseVisualStyleBackColor = true;
            btnHuyDon.Click += btnHuyDon_Click_1;
            // 
            // btnThanhToan
            // 
            btnThanhToan.Dock = DockStyle.Top;
            btnThanhToan.Location = new Point(0, 0);
            btnThanhToan.Name = "btnThanhToan";
            btnThanhToan.Size = new Size(438, 58);
            btnThanhToan.TabIndex = 0;
            btnThanhToan.Text = "Thanh Toán";
            btnThanhToan.UseVisualStyleBackColor = true;
            btnThanhToan.Click += btnThanhToan_Click_1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1772, 1047);
            Controls.Add(tlpMain);
            Name = "MainForm";
            Text = "Order";
            Load += MainForm_Load;
            tlpMain.ResumeLayout(false);
            panelCol1.ResumeLayout(false);
            panelCol1.PerformLayout();
            panelTongTien.ResumeLayout(false);
            panelTongTien.PerformLayout();
            panelCol2.ResumeLayout(false);
            panelCol2.PerformLayout();
            panelCol3.ResumeLayout(false);
            panelCol3.PerformLayout();
            panelChucNang.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpMain;
        private Label label1;
        private ListView lvDonHang;
        private Panel panelCol1;
        private Panel panelTongTien;
        private Label label2;
        private Label lblTongCong;
        private ColumnHeader TenSP;
        private ColumnHeader SL;
        private ColumnHeader DonGia;
        private ColumnHeader ThanhTien;
        private Panel panelCol2;
        private FlowLayoutPanel flpSanPham;
        private Label label3;
        private Panel panelCol3;
        private Panel panelChucNang;
        private Button btnThanhToan;
        private Button btnHuyDon;
        private FlowLayoutPanel flpLoaiSP;
        private Label label4;
    }
}