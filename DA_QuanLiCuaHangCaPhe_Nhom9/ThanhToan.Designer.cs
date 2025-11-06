namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    partial class ThanhToan
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
            btn_inhoadon = new Button();
            panelLeft = new Panel();
            lvChiTietBill = new ListView();
            tenmon = new ColumnHeader();
            Soluong = new ColumnHeader();
            ThanhTien = new ColumnHeader();
            label5 = new Label();
            groupBox1 = new GroupBox();
            label3 = new Label();
            txtKhachDua = new TextBox();
            label2 = new Label();
            lblTienDu = new Label();
            lblTongCongBill = new Label();
            label1 = new Label();
            panelRight = new Panel();
            panelBillPreview = new Panel();
            pbQR_InBill = new PictureBox();
            groupBox2 = new GroupBox();
            rbQR = new RadioButton();
            rbTienMat = new RadioButton();
            panelLeft.SuspendLayout();
            groupBox1.SuspendLayout();
            panelRight.SuspendLayout();
            panelBillPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbQR_InBill).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btn_inhoadon
            // 
            btn_inhoadon.Font = new Font("Segoe UI Black", 9.900001F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            btn_inhoadon.Location = new Point(144, 215);
            btn_inhoadon.Margin = new Padding(2, 2, 2, 2);
            btn_inhoadon.Name = "btn_inhoadon";
            btn_inhoadon.Size = new Size(230, 57);
            btn_inhoadon.TabIndex = 0;
            btn_inhoadon.Text = "In hoá đơn";
            btn_inhoadon.UseVisualStyleBackColor = true;
            btn_inhoadon.Click += btn_inhoadon_Click;
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(lvChiTietBill);
            panelLeft.Controls.Add(label5);
            panelLeft.Controls.Add(groupBox1);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Margin = new Padding(2, 2, 2, 2);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(518, 515);
            panelLeft.TabIndex = 1;
            // 
            // lvChiTietBill
            // 
            lvChiTietBill.Columns.AddRange(new ColumnHeader[] { tenmon, Soluong, ThanhTien });
            lvChiTietBill.Dock = DockStyle.Fill;
            lvChiTietBill.GridLines = true;
            lvChiTietBill.Location = new Point(0, 30);
            lvChiTietBill.Margin = new Padding(2, 2, 2, 2);
            lvChiTietBill.Name = "lvChiTietBill";
            lvChiTietBill.Size = new Size(518, 200);
            lvChiTietBill.TabIndex = 1;
            lvChiTietBill.UseCompatibleStateImageBehavior = false;
            lvChiTietBill.View = View.Details;
            // 
            // tenmon
            // 
            tenmon.Text = "Tên món";
            tenmon.Width = 250;
            // 
            // Soluong
            // 
            Soluong.Text = "SL";
            // 
            // ThanhTien
            // 
            ThanhTien.Text = "Thành tiền";
            ThanhTien.Width = 200;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Top;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(0, 0);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Padding = new Padding(0, 5, 0, 5);
            label5.Size = new Size(149, 30);
            label5.TabIndex = 0;
            label5.Text = " CHI TIẾT HÓA ĐƠN";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btn_inhoadon);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtKhachDua);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(lblTienDu);
            groupBox1.Controls.Add(lblTongCongBill);
            groupBox1.Controls.Add(label1);
            groupBox1.Dock = DockStyle.Bottom;
            groupBox1.Location = new Point(0, 230);
            groupBox1.Margin = new Padding(2, 2, 2, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(2, 2, 2, 2);
            groupBox1.Size = new Size(518, 285);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tính tiền";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 158);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(66, 20);
            label3.TabIndex = 4;
            label3.Text = "Tiền dư: ";
            // 
            // txtKhachDua
            // 
            txtKhachDua.Location = new Point(196, 96);
            txtKhachDua.Margin = new Padding(2, 2, 2, 2);
            txtKhachDua.Name = "txtKhachDua";
            txtKhachDua.Size = new Size(120, 27);
            txtKhachDua.TabIndex = 3;
            txtKhachDua.TextChanged += txtKhachDua_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 100);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(79, 20);
            label2.TabIndex = 2;
            label2.Text = "Khách đưa";
            // 
            // lblTienDu
            // 
            lblTienDu.AutoSize = true;
            lblTienDu.Location = new Point(196, 158);
            lblTienDu.Margin = new Padding(2, 0, 2, 0);
            lblTienDu.Name = "lblTienDu";
            lblTienDu.Size = new Size(30, 20);
            lblTienDu.TabIndex = 1;
            lblTienDu.Text = "0 đ";
            // 
            // lblTongCongBill
            // 
            lblTongCongBill.AutoSize = true;
            lblTongCongBill.Location = new Point(196, 43);
            lblTongCongBill.Margin = new Padding(2, 0, 2, 0);
            lblTongCongBill.Name = "lblTongCongBill";
            lblTongCongBill.Size = new Size(30, 20);
            lblTongCongBill.TabIndex = 1;
            lblTongCongBill.Text = "0 đ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 43);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(87, 20);
            label1.TabIndex = 0;
            label1.Text = "Tổng cộng: ";
            // 
            // panelRight
            // 
            panelRight.Controls.Add(panelBillPreview);
            panelRight.Controls.Add(groupBox2);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(518, 0);
            panelRight.Margin = new Padding(2, 2, 2, 2);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(4, 0, 0, 0);
            panelRight.Size = new Size(387, 515);
            panelRight.TabIndex = 1;
            // 
            // panelBillPreview
            // 
            panelBillPreview.AutoScroll = true;
            panelBillPreview.Controls.Add(pbQR_InBill);
            panelBillPreview.Dock = DockStyle.Fill;
            panelBillPreview.Location = new Point(4, 250);
            panelBillPreview.Margin = new Padding(2, 2, 2, 2);
            panelBillPreview.Name = "panelBillPreview";
            panelBillPreview.Size = new Size(383, 265);
            panelBillPreview.TabIndex = 1;
            // 
            // pbQR_InBill
            // 
            pbQR_InBill.Dock = DockStyle.Bottom;
            pbQR_InBill.Location = new Point(0, -24);
            pbQR_InBill.Margin = new Padding(2, 2, 2, 2);
            pbQR_InBill.Name = "pbQR_InBill";
            pbQR_InBill.Size = new Size(383, 289);
            pbQR_InBill.SizeMode = PictureBoxSizeMode.Zoom;
            pbQR_InBill.TabIndex = 0;
            pbQR_InBill.TabStop = false;
            pbQR_InBill.Click += pbQR_InBill_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rbQR);
            groupBox2.Controls.Add(rbTienMat);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(4, 0);
            groupBox2.Margin = new Padding(2, 2, 2, 2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(2, 2, 2, 2);
            groupBox2.Size = new Size(383, 250);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Hình thức thanh toán";
            // 
            // rbQR
            // 
            rbQR.AutoSize = true;
            rbQR.Location = new Point(60, 154);
            rbQR.Margin = new Padding(2, 2, 2, 2);
            rbQR.Name = "rbQR";
            rbQR.Size = new Size(146, 24);
            rbQR.TabIndex = 0;
            rbQR.Text = "Chuyển khoản QR";
            rbQR.UseVisualStyleBackColor = true;
            rbQR.CheckedChanged += rbQR_CheckedChanged;
            // 
            // rbTienMat
            // 
            rbTienMat.AutoSize = true;
            rbTienMat.Checked = true;
            rbTienMat.Location = new Point(60, 62);
            rbTienMat.Margin = new Padding(2, 2, 2, 2);
            rbTienMat.Name = "rbTienMat";
            rbTienMat.Size = new Size(88, 24);
            rbTienMat.TabIndex = 0;
            rbTienMat.TabStop = true;
            rbTienMat.Text = "Tiền mặt";
            rbTienMat.UseVisualStyleBackColor = true;
            rbTienMat.CheckedChanged += rbQR_CheckedChanged;
            // 
            // ThanhToan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(905, 515);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(2, 2, 2, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ThanhToan";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Xác Nhận Thanh Toán";
            Load += ThanhToan_Load;
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panelRight.ResumeLayout(false);
            panelBillPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbQR_InBill).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panelLeft;
        private Panel panelRight;
        private Label label5;
        private GroupBox groupBox1;
        private Label label3;
        private TextBox txtKhachDua;
        private Label label2;
        private Label lblTienDu;
        private Label lblTongCongBill;
        private Label label1;
        private ListView lvChiTietBill;
        private ColumnHeader tenmon;
        private ColumnHeader Soluong;
        private ColumnHeader ThanhTien;
        private GroupBox groupBox2;
        private RadioButton rbTienMat;
        private RadioButton rbQR;
        private Button btn_inhoadon;
        private Panel panelBillPreview;
        private PictureBox pbQR_InBill;
    }
}