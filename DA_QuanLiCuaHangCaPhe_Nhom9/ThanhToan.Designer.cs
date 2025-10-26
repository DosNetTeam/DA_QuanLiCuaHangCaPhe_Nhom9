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
            panelBottom = new Panel();
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
            label4 = new Label();
            lblTongCongBill = new Label();
            label1 = new Label();
            panelRight = new Panel();
            pbQR = new PictureBox();
            groupBox2 = new GroupBox();
            rbQR = new RadioButton();
            rbTienMat = new RadioButton();
            button1 = new Button();
            panelBottom.SuspendLayout();
            panelLeft.SuspendLayout();
            groupBox1.SuspendLayout();
            panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbQR).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(button1);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 805);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1337, 250);
            panelBottom.TabIndex = 0;
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(lvChiTietBill);
            panelLeft.Controls.Add(label5);
            panelLeft.Controls.Add(groupBox1);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(500, 805);
            panelLeft.TabIndex = 1;
            // 
            // lvChiTietBill
            // 
            lvChiTietBill.Columns.AddRange(new ColumnHeader[] { tenmon, Soluong, ThanhTien });
            lvChiTietBill.Dock = DockStyle.Fill;
            lvChiTietBill.GridLines = true;
            lvChiTietBill.Location = new Point(0, 61);
            lvChiTietBill.Name = "lvChiTietBill";
            lvChiTietBill.Size = new Size(500, 494);
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
            label5.Name = "label5";
            label5.Padding = new Padding(0, 10, 0, 10);
            label5.Size = new Size(300, 61);
            label5.TabIndex = 0;
            label5.Text = " CHI TIẾT HÓA ĐƠN";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtKhachDua);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(lblTongCongBill);
            groupBox1.Controls.Add(label1);
            groupBox1.Dock = DockStyle.Bottom;
            groupBox1.Location = new Point(0, 555);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(500, 250);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tính tiền";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 157);
            label3.Name = "label3";
            label3.Size = new Size(133, 41);
            label3.TabIndex = 4;
            label3.Text = "Tiền dư: ";
            // 
            // txtKhachDua
            // 
            txtKhachDua.Location = new Point(195, 97);
            txtKhachDua.Name = "txtKhachDua";
            txtKhachDua.Size = new Size(250, 47);
            txtKhachDua.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 100);
            label2.Name = "label2";
            label2.Size = new Size(157, 41);
            label2.TabIndex = 2;
            label2.Text = "Khách đưa";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(195, 157);
            label4.Name = "label4";
            label4.Size = new Size(60, 41);
            label4.TabIndex = 1;
            label4.Text = "0 đ";
            // 
            // lblTongCongBill
            // 
            lblTongCongBill.AutoSize = true;
            lblTongCongBill.Location = new Point(195, 43);
            lblTongCongBill.Name = "lblTongCongBill";
            lblTongCongBill.Size = new Size(60, 41);
            lblTongCongBill.TabIndex = 1;
            lblTongCongBill.Text = "0 đ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 43);
            label1.Name = "label1";
            label1.Size = new Size(177, 41);
            label1.TabIndex = 0;
            label1.Text = "Tổng cộng: ";
            // 
            // panelRight
            // 
            panelRight.Controls.Add(pbQR);
            panelRight.Controls.Add(groupBox2);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(500, 0);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(5, 0, 0, 0);
            panelRight.Size = new Size(837, 805);
            panelRight.TabIndex = 1;
            // 
            // pbQR
            // 
            pbQR.BorderStyle = BorderStyle.FixedSingle;
            pbQR.Dock = DockStyle.Fill;
            pbQR.Location = new Point(5, 250);
            pbQR.Name = "pbQR";
            pbQR.Size = new Size(832, 555);
            pbQR.SizeMode = PictureBoxSizeMode.Zoom;
            pbQR.TabIndex = 1;
            pbQR.TabStop = false;
            pbQR.Visible = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rbQR);
            groupBox2.Controls.Add(rbTienMat);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(5, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(832, 250);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Hình thức thanh toán";
            // 
            // rbQR
            // 
            rbQR.AutoSize = true;
            rbQR.Location = new Point(60, 154);
            rbQR.Name = "rbQR";
            rbQR.Size = new Size(295, 45);
            rbQR.TabIndex = 0;
            rbQR.Text = "Chuyển khoản QR";
            rbQR.UseVisualStyleBackColor = true;
            // 
            // rbTienMat
            // 
            rbTienMat.AutoSize = true;
            rbTienMat.Checked = true;
            rbTienMat.Location = new Point(60, 61);
            rbTienMat.Name = "rbTienMat";
            rbTienMat.Size = new Size(170, 45);
            rbTienMat.TabIndex = 0;
            rbTienMat.TabStop = true;
            rbTienMat.Text = "Tiền mặt";
            rbTienMat.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI Black", 9.900001F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            button1.Location = new Point(128, 31);
            button1.Name = "button1";
            button1.Size = new Size(229, 58);
            button1.TabIndex = 0;
            button1.Text = "In hoá đơn";
            button1.TextAlign = ContentAlignment.BottomCenter;
            button1.UseVisualStyleBackColor = true;
            // 
            // ThanhToan
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1337, 1055);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            Controls.Add(panelBottom);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ThanhToan";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Xác Nhận Thanh Toán";
            panelBottom.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbQR).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelBottom;
        private Panel panelLeft;
        private Panel panelRight;
        private Label label5;
        private GroupBox groupBox1;
        private Label label3;
        private TextBox txtKhachDua;
        private Label label2;
        private Label label4;
        private Label lblTongCongBill;
        private Label label1;
        private ListView lvChiTietBill;
        private ColumnHeader tenmon;
        private ColumnHeader Soluong;
        private ColumnHeader ThanhTien;
        private GroupBox groupBox2;
        private RadioButton rbTienMat;
        private PictureBox pbQR;
        private RadioButton rbQR;
        private Button button1;
    }
}