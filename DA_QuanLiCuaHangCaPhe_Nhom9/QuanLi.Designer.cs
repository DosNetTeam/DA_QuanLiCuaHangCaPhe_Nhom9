﻿namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    partial class QuanLi
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
            panelMenu = new Panel();
            btnDangXuat = new Button();
            label1 = new Label();
            panelContent = new Panel();
            tabControlMain = new TabControl();
            tabPageQuanLyNV = new TabPage();
            panel1 = new Panel();
            dgvPerformance = new DataGridView();
            btnLoc = new Button();
            cbThang = new ComboBox();
            lblTieuDeHieuSuat = new Label();
            label2 = new Label();
            tabPageHoaDon = new TabPage();
            panelHoaDon = new Panel();
            dgvHoaDon = new DataGridView();
            cbTrangThaiHD = new ComboBox();
            textBox1 = new TextBox();
            label4 = new Label();
            label3 = new Label();
            tabPageTonKho = new TabPage();
            panel2 = new Panel();
            dgvTonKho = new DataGridView();
            btnThemMoiKho = new Button();
            txtTimKiemNL = new TextBox();
            label6 = new Label();
            label5 = new Label();
            panelMenu.SuspendLayout();
            panelContent.SuspendLayout();
            tabControlMain.SuspendLayout();
            tabPageQuanLyNV.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPerformance).BeginInit();
            tabPageHoaDon.SuspendLayout();
            panelHoaDon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).BeginInit();
            tabPageTonKho.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTonKho).BeginInit();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.Controls.Add(btnDangXuat);
            panelMenu.Controls.Add(label1);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(250, 450);
            panelMenu.TabIndex = 1;
            // 
            // btnDangXuat
            // 
            btnDangXuat.Dock = DockStyle.Bottom;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Location = new Point(0, 403);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(250, 47);
            btnDangXuat.TabIndex = 6;
            btnDangXuat.Text = "Dang Xuat";
            btnDangXuat.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(192, 64, 0);
            label1.Location = new Point(39, 93);
            label1.Name = "label1";
            label1.Size = new Size(176, 60);
            label1.TabIndex = 1;
            label1.Text = "COFFEE";
            // 
            // panelContent
            // 
            panelContent.Controls.Add(tabControlMain);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(250, 0);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(550, 450);
            panelContent.TabIndex = 2;
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabPageQuanLyNV);
            tabControlMain.Controls.Add(tabPageHoaDon);
            tabControlMain.Controls.Add(tabPageTonKho);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Location = new Point(0, 0);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(550, 450);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageQuanLyNV
            // 
            tabPageQuanLyNV.Controls.Add(panel1);
            tabPageQuanLyNV.Controls.Add(label2);
            tabPageQuanLyNV.Location = new Point(4, 29);
            tabPageQuanLyNV.Name = "tabPageQuanLyNV";
            tabPageQuanLyNV.Padding = new Padding(3);
            tabPageQuanLyNV.Size = new Size(542, 417);
            tabPageQuanLyNV.TabIndex = 0;
            tabPageQuanLyNV.Text = "Nhan vien";
            tabPageQuanLyNV.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dgvPerformance);
            panel1.Controls.Add(btnLoc);
            panel1.Controls.Add(cbThang);
            panel1.Controls.Add(lblTieuDeHieuSuat);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 64);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(15);
            panel1.Size = new Size(536, 350);
            panel1.TabIndex = 1;
            // 
            // dgvPerformance
            // 
            dgvPerformance.AllowUserToAddRows = false;
            dgvPerformance.BackgroundColor = SystemColors.Control;
            dgvPerformance.BorderStyle = BorderStyle.None;
            dgvPerformance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPerformance.Location = new Point(15, 90);
            dgvPerformance.Name = "dgvPerformance";
            dgvPerformance.ReadOnly = true;
            dgvPerformance.RowHeadersVisible = false;
            dgvPerformance.RowHeadersWidth = 51;
            dgvPerformance.Size = new Size(516, 255);
            dgvPerformance.TabIndex = 3;
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.FromArgb(255, 192, 128);
            btnLoc.FlatStyle = FlatStyle.Flat;
            btnLoc.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoc.ForeColor = Color.White;
            btnLoc.Location = new Point(172, 50);
            btnLoc.Name = "btnLoc";
            btnLoc.Size = new Size(94, 29);
            btnLoc.TabIndex = 2;
            btnLoc.Text = "Loc";
            btnLoc.UseVisualStyleBackColor = false;
            // 
            // cbThang
            // 
            cbThang.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbThang.FormattingEnabled = true;
            cbThang.Items.AddRange(new object[] { "10/2025" });
            cbThang.Location = new Point(15, 50);
            cbThang.Name = "cbThang";
            cbThang.Size = new Size(151, 31);
            cbThang.TabIndex = 1;
            // 
            // lblTieuDeHieuSuat
            // 
            lblTieuDeHieuSuat.AutoSize = true;
            lblTieuDeHieuSuat.Dock = DockStyle.Top;
            lblTieuDeHieuSuat.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTieuDeHieuSuat.Location = new Point(15, 15);
            lblTieuDeHieuSuat.Name = "lblTieuDeHieuSuat";
            lblTieuDeHieuSuat.Size = new Size(334, 28);
            lblTieuDeHieuSuat.TabIndex = 0;
            lblTieuDeHieuSuat.Text = "Hieu suat ban hang cua nhan vien";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(3, 3);
            label2.Name = "label2";
            label2.Padding = new Padding(10);
            label2.Size = new Size(424, 61);
            label2.TabIndex = 0;
            label2.Text = "Quan li nhan vien ban hang";
            // 
            // tabPageHoaDon
            // 
            tabPageHoaDon.Controls.Add(panelHoaDon);
            tabPageHoaDon.Controls.Add(label3);
            tabPageHoaDon.Location = new Point(4, 29);
            tabPageHoaDon.Name = "tabPageHoaDon";
            tabPageHoaDon.Padding = new Padding(3);
            tabPageHoaDon.Size = new Size(542, 417);
            tabPageHoaDon.TabIndex = 1;
            tabPageHoaDon.Text = "Hoa don";
            tabPageHoaDon.UseVisualStyleBackColor = true;
            // 
            // panelHoaDon
            // 
            panelHoaDon.BackColor = Color.White;
            panelHoaDon.Controls.Add(dgvHoaDon);
            panelHoaDon.Controls.Add(cbTrangThaiHD);
            panelHoaDon.Controls.Add(textBox1);
            panelHoaDon.Controls.Add(label4);
            panelHoaDon.Dock = DockStyle.Fill;
            panelHoaDon.Location = new Point(3, 64);
            panelHoaDon.Name = "panelHoaDon";
            panelHoaDon.Padding = new Padding(15);
            panelHoaDon.Size = new Size(536, 350);
            panelHoaDon.TabIndex = 1;
            // 
            // dgvHoaDon
            // 
            dgvHoaDon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHoaDon.BackgroundColor = SystemColors.Control;
            dgvHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHoaDon.Location = new Point(15, 90);
            dgvHoaDon.Name = "dgvHoaDon";
            dgvHoaDon.RowHeadersWidth = 51;
            dgvHoaDon.Size = new Size(503, 242);
            dgvHoaDon.TabIndex = 3;
            // 
            // cbTrangThaiHD
            // 
            cbTrangThaiHD.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbTrangThaiHD.FormattingEnabled = true;
            cbTrangThaiHD.Location = new Point(385, 50);
            cbTrangThaiHD.Name = "cbTrangThaiHD";
            cbTrangThaiHD.Size = new Size(133, 28);
            cbTrangThaiHD.TabIndex = 2;
            cbTrangThaiHD.Text = "Tat ca trang thai";
            // 
            // textBox1
            // 
            textBox1.ForeColor = Color.Gray;
            textBox1.Location = new Point(15, 50);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 1;
            textBox1.Text = "Tim kiem ma hd";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(15, 15);
            label4.Name = "label4";
            label4.Size = new Size(283, 28);
            label4.TabIndex = 0;
            label4.Text = "Danh sach hoa don gan nhat";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(3, 3);
            label3.Name = "label3";
            label3.Padding = new Padding(10);
            label3.Size = new Size(336, 61);
            label3.TabIndex = 0;
            label3.Text = "Hoa don va giao dich";
            // 
            // tabPageTonKho
            // 
            tabPageTonKho.Controls.Add(panel2);
            tabPageTonKho.Controls.Add(label5);
            tabPageTonKho.Location = new Point(4, 29);
            tabPageTonKho.Name = "tabPageTonKho";
            tabPageTonKho.Size = new Size(542, 417);
            tabPageTonKho.TabIndex = 2;
            tabPageTonKho.Text = "Kho";
            tabPageTonKho.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(dgvTonKho);
            panel2.Controls.Add(btnThemMoiKho);
            panel2.Controls.Add(txtTimKiemNL);
            panel2.Controls.Add(label6);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 61);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(15);
            panel2.Size = new Size(542, 356);
            panel2.TabIndex = 1;
            // 
            // dgvTonKho
            // 
            dgvTonKho.AllowDrop = true;
            dgvTonKho.AllowUserToAddRows = false;
            dgvTonKho.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTonKho.BackgroundColor = SystemColors.Control;
            dgvTonKho.BorderStyle = BorderStyle.None;
            dgvTonKho.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTonKho.Location = new Point(15, 90);
            dgvTonKho.Name = "dgvTonKho";
            dgvTonKho.RowHeadersVisible = false;
            dgvTonKho.RowHeadersWidth = 51;
            dgvTonKho.Size = new Size(477, 248);
            dgvTonKho.TabIndex = 3;
            // 
            // btnThemMoiKho
            // 
            btnThemMoiKho.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnThemMoiKho.BackColor = Color.FromArgb(224, 224, 224);
            btnThemMoiKho.FlatStyle = FlatStyle.Flat;
            btnThemMoiKho.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThemMoiKho.ForeColor = Color.White;
            btnThemMoiKho.Location = new Point(372, 50);
            btnThemMoiKho.Name = "btnThemMoiKho";
            btnThemMoiKho.Size = new Size(120, 29);
            btnThemMoiKho.TabIndex = 2;
            btnThemMoiKho.Text = "+ Thêm Mới";
            btnThemMoiKho.UseVisualStyleBackColor = false;
            // 
            // txtTimKiemNL
            // 
            txtTimKiemNL.ForeColor = Color.Gray;
            txtTimKiemNL.Location = new Point(15, 50);
            txtTimKiemNL.Name = "txtTimKiemNL";
            txtTimKiemNL.Size = new Size(151, 27);
            txtTimKiemNL.TabIndex = 1;
            txtTimKiemNL.Text = "Tim kiem nguyen lieu";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Top;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(15, 15);
            label6.Name = "label6";
            label6.Size = new Size(242, 28);
            label6.TabIndex = 0;
            label6.Text = "Danh sach hang ton kho";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Top;
            label5.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Padding = new Padding(10);
            label5.Size = new Size(264, 61);
            label5.TabIndex = 0;
            label5.Text = "Quan li ton kho ";
            // 
            // QuanLi
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(800, 450);
            Controls.Add(panelContent);
            Controls.Add(panelMenu);
            Name = "QuanLi";
            Text = "QuanLi";
            panelMenu.ResumeLayout(false);
            panelMenu.PerformLayout();
            panelContent.ResumeLayout(false);
            tabControlMain.ResumeLayout(false);
            tabPageQuanLyNV.ResumeLayout(false);
            tabPageQuanLyNV.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPerformance).EndInit();
            tabPageHoaDon.ResumeLayout(false);
            tabPageHoaDon.PerformLayout();
            panelHoaDon.ResumeLayout(false);
            panelHoaDon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).EndInit();
            tabPageTonKho.ResumeLayout(false);
            tabPageTonKho.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTonKho).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMenu;
        private Label label1;
        private Panel panelContent;
        private Button btnDangXuat;
        private TabControl tabControlMain;
        private TabPage tabPageQuanLyNV;
        private TabPage tabPageHoaDon;
        private TabPage tabPageTonKho;
        private Label label2;
        private Panel panel1;
        private Label lblTieuDeHieuSuat;
        private Button btnLoc;
        private ComboBox cbThang;
        private DataGridView dgvPerformance;
        private Panel panelHoaDon;
        private TextBox textBox1;
        private Label label4;
        private Label label3;
        private DataGridView dgvHoaDon;
        private ComboBox cbTrangThaiHD;
        private Panel panel2;
        private Label label6;
        private Label label5;
        private Button btnThemMoiKho;
        private TextBox txtTimKiemNL;
        private DataGridView dgvTonKho;
    }
}