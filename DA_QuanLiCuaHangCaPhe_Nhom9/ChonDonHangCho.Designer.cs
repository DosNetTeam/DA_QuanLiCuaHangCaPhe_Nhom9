namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    partial class ChonDonHangCho {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            panel1 = new Panel();
            btnTaiLai = new Button();
            btnChonThanhToan = new Button();
            lvDonHangCho = new ListView();
            MaDH = new ColumnHeader();
            TenKH = new ColumnHeader();
            NgayLap = new ColumnHeader();
            TongTien = new ColumnHeader();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnTaiLai);
            panel1.Controls.Add(btnChonThanhToan);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 544);
            panel1.Name = "panel1";
            panel1.Size = new Size(803, 102);
            panel1.TabIndex = 0;
            // 
            // btnTaiLai
            // 
            btnTaiLai.Dock = DockStyle.Left;
            btnTaiLai.Location = new Point(0, 0);
            btnTaiLai.Name = "btnTaiLai";
            btnTaiLai.Size = new Size(260, 102);
            btnTaiLai.TabIndex = 0;
            btnTaiLai.Text = "Tải Lại Danh Sách";
            btnTaiLai.UseVisualStyleBackColor = true;
            btnTaiLai.Click += btnTaiLai_Click;
            // 
            // btnChonThanhToan
            // 
            btnChonThanhToan.Dock = DockStyle.Right;
            btnChonThanhToan.Location = new Point(475, 0);
            btnChonThanhToan.Name = "btnChonThanhToan";
            btnChonThanhToan.Size = new Size(328, 102);
            btnChonThanhToan.TabIndex = 0;
            btnChonThanhToan.Text = "Thanh Toán Đơn Này";
            btnChonThanhToan.UseVisualStyleBackColor = true;
            btnChonThanhToan.Click += btnChonThanhToan_Click;
            // 
            // lvDonHangCho
            // 
            lvDonHangCho.Columns.AddRange(new ColumnHeader[] { MaDH, TenKH, NgayLap, TongTien });
            lvDonHangCho.Dock = DockStyle.Fill;
            lvDonHangCho.FullRowSelect = true;
            lvDonHangCho.GridLines = true;
            lvDonHangCho.Location = new Point(0, 0);
            lvDonHangCho.Name = "lvDonHangCho";
            lvDonHangCho.Size = new Size(803, 544);
            lvDonHangCho.TabIndex = 1;
            lvDonHangCho.UseCompatibleStateImageBehavior = false;
            lvDonHangCho.View = View.Details;
            // 
            // MaDH
            // 
            MaDH.Text = "Mã ĐH";
            MaDH.Width = 110;
            // 
            // TenKH
            // 
            TenKH.Text = "Tên Khách Hàng";
            TenKH.Width = 250;
            // 
            // NgayLap
            // 
            NgayLap.Text = "Ngày Lập";
            NgayLap.Width = 250;
            // 
            // TongTien
            // 
            TongTien.Text = "Tổng Tiền";
            TongTien.Width = 190;
            // 
            // ChonDonHangCho
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(803, 646);
            Controls.Add(lvDonHangCho);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "ChonDonHangCho";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ChonDonHangCho";
            Load += ChonDonHangCho_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnTaiLai;
        private Button btnChonThanhToan;
        private ListView lvDonHangCho;
        private ColumnHeader MaDH;
        private ColumnHeader TenKH;
        private ColumnHeader NgayLap;
        private ColumnHeader TongTien;
    }
}