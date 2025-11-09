namespace DA_QuanLiCuaHangCaPhe_Nhom9
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
        private void InitializeComponent() {
            panelMenu = new Panel();
            btnDangXuat = new Button();
            grpNotify = new GroupBox();
            txtNotifyMessage = new TextBox();
            btnSendNotify = new Button();
            lblCoffee = new Label();
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
            txtTimKiemHD = new TextBox();
            label4 = new Label();
            label3 = new Label();
            tabPageTonKho = new TabPage();
            panel2 = new Panel();
            dgvTonKho = new DataGridView();
            btnThemMoiKho = new Button();
            txtTimKiemKho = new TextBox();
            label6 = new Label();
            label5 = new Label();
            panelMenu.SuspendLayout();
            grpNotify.SuspendLayout();
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
            panelMenu.BackColor = Color.FromArgb(45, 45, 48);
            panelMenu.Controls.Add(btnDangXuat);
            panelMenu.Controls.Add(grpNotify);
            panelMenu.Controls.Add(lblCoffee);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Margin = new Padding(6);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(531, 922);
            panelMenu.TabIndex = 1;
            // 
            // btnDangXuat
            // 
            btnDangXuat.BackColor = Color.FromArgb(220, 80, 0);
            btnDangXuat.Dock = DockStyle.Bottom;
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Location = new Point(0, 826);
            btnDangXuat.Margin = new Padding(6);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(531, 96);
            btnDangXuat.TabIndex = 6;
            btnDangXuat.Text = "Trang Order";
            btnDangXuat.UseVisualStyleBackColor = false;
            btnDangXuat.Click += btnDangXuat_Click;
            // 
            // grpNotify
            // 
            grpNotify.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            grpNotify.Controls.Add(txtNotifyMessage);
            grpNotify.Controls.Add(btnSendNotify);
            grpNotify.Location = new Point(12, 321);
            grpNotify.Name = "grpNotify";
            grpNotify.Size = new Size(220, 64);
            grpNotify.TabIndex = 5;
            grpNotify.TabStop = false;
            grpNotify.Text = "Gửi thông báo";
            // 
            // txtNotifyMessage
            // 
            txtNotifyMessage.Location = new Point(10, 22);
            txtNotifyMessage.Name = "txtNotifyMessage";
            txtNotifyMessage.PlaceholderText = "Nội dung thông báo";
            txtNotifyMessage.Size = new Size(140, 27);
            txtNotifyMessage.TabIndex = 0;
            // 
            // btnSendNotify
            // 
            btnSendNotify.Location = new Point(155, 22);
            btnSendNotify.Name = "btnSendNotify";
            btnSendNotify.Size = new Size(55, 27);
            btnSendNotify.TabIndex = 1;
            btnSendNotify.Text = "Gửi";
            btnSendNotify.UseVisualStyleBackColor = true;
            // 
            // lblCoffee
            // 
            lblCoffee.AutoSize = true;
            lblCoffee.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCoffee.ForeColor = Color.FromArgb(192, 64, 0);
            lblCoffee.Location = new Point(51, 191);
            lblCoffee.Margin = new Padding(6, 0, 6, 0);
            lblCoffee.Name = "lblCoffee";
            lblCoffee.Size = new Size(176, 60);
            lblCoffee.TabIndex = 8;
            lblCoffee.Text = "COFFEE";
            // 
            // panelContent
            // 
            panelContent.Controls.Add(tabControlMain);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(531, 0);
            panelContent.Margin = new Padding(6);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(1169, 922);
            panelContent.TabIndex = 2;
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabPageQuanLyNV);
            tabControlMain.Controls.Add(tabPageHoaDon);
            tabControlMain.Controls.Add(tabPageTonKho);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Location = new Point(0, 0);
            tabControlMain.Margin = new Padding(6);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(1169, 922);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageQuanLyNV
            // 
            tabPageQuanLyNV.Controls.Add(panel1);
            tabPageQuanLyNV.Controls.Add(label2);
            tabPageQuanLyNV.Location = new Point(10, 58);
            tabPageQuanLyNV.Margin = new Padding(6);
            tabPageQuanLyNV.Name = "tabPageQuanLyNV";
            tabPageQuanLyNV.Padding = new Padding(3);
            tabPageQuanLyNV.Size = new Size(542, 417);
            tabPageQuanLyNV.TabIndex = 0;
            tabPageQuanLyNV.Text = "Nhân viên";
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
            panel1.Location = new Point(6, 127);
            panel1.Margin = new Padding(6);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(15);
            panel1.Size = new Size(536, 350);
            panel1.TabIndex = 1;
            // 
            // dgvPerformance
            // 
            dgvPerformance.AllowUserToAddRows = false;
            dgvPerformance.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPerformance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPerformance.BackgroundColor = SystemColors.Control;
            dgvPerformance.BorderStyle = BorderStyle.None;
            dgvPerformance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPerformance.Location = new Point(32, 184);
            dgvPerformance.Margin = new Padding(6);
            dgvPerformance.Name = "dgvPerformance";
            dgvPerformance.ReadOnly = true;
            dgvPerformance.RowHeadersVisible = false;
            dgvPerformance.RowHeadersWidth = 51;
            dgvPerformance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPerformance.Size = new Size(516, 255);
            dgvPerformance.TabIndex = 3;
            dgvPerformance.CellContentClick += dgvPerformance_CellContentClick;
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.FromArgb(255, 153, 77);
            btnLoc.FlatAppearance.BorderSize = 0;
            btnLoc.FlatStyle = FlatStyle.Flat;
            btnLoc.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoc.ForeColor = Color.White;
            btnLoc.Location = new Point(366, 102);
            btnLoc.Margin = new Padding(6);
            btnLoc.Name = "btnLoc";
            btnLoc.Size = new Size(200, 59);
            btnLoc.TabIndex = 2;
            btnLoc.Text = "Lọc";
            btnLoc.UseVisualStyleBackColor = false;
            btnLoc.Click += btnLoc_Click;
            // 
            // cbThang
            // 
            cbThang.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbThang.FormattingEnabled = true;
            cbThang.Items.AddRange(new object[] { "10/2025" });
            cbThang.Location = new Point(32, 102);
            cbThang.Margin = new Padding(6);
            cbThang.Name = "cbThang";
            cbThang.Size = new Size(316, 54);
            cbThang.TabIndex = 1;
            cbThang.SelectedIndexChanged += cbThang_SelectedIndexChanged;
            // 
            // lblTieuDeHieuSuat
            // 
            lblTieuDeHieuSuat.AutoSize = true;
            lblTieuDeHieuSuat.Dock = DockStyle.Top;
            lblTieuDeHieuSuat.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTieuDeHieuSuat.Location = new Point(32, 31);
            lblTieuDeHieuSuat.Margin = new Padding(6, 0, 6, 0);
            lblTieuDeHieuSuat.Name = "lblTieuDeHieuSuat";
            lblTieuDeHieuSuat.Size = new Size(661, 54);
            lblTieuDeHieuSuat.TabIndex = 0;
            lblTieuDeHieuSuat.Text = "Hiệu suất bán hàng của nhân viên";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(6, 6);
            label2.Margin = new Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Padding = new Padding(10);
            label2.Size = new Size(424, 61);
            label2.TabIndex = 0;
            label2.Text = "Quản lí nhân viên bán hàng";
            // 
            // tabPageHoaDon
            // 
            tabPageHoaDon.Controls.Add(panelHoaDon);
            tabPageHoaDon.Controls.Add(label3);
            tabPageHoaDon.Location = new Point(10, 58);
            tabPageHoaDon.Margin = new Padding(6);
            tabPageHoaDon.Name = "tabPageHoaDon";
            tabPageHoaDon.Padding = new Padding(3);
            tabPageHoaDon.Size = new Size(542, 417);
            tabPageHoaDon.TabIndex = 1;
            tabPageHoaDon.Text = "Hóa đơn";
            tabPageHoaDon.UseVisualStyleBackColor = true;
            // 
            // panelHoaDon
            // 
            panelHoaDon.BackColor = Color.White;
            panelHoaDon.Controls.Add(dgvHoaDon);
            panelHoaDon.Controls.Add(cbTrangThaiHD);
            panelHoaDon.Controls.Add(txtTimKiemHD);
            panelHoaDon.Controls.Add(label4);
            panelHoaDon.Dock = DockStyle.Fill;
            panelHoaDon.Location = new Point(6, 127);
            panelHoaDon.Margin = new Padding(6);
            panelHoaDon.Name = "panelHoaDon";
            panelHoaDon.Padding = new Padding(15);
            panelHoaDon.Size = new Size(536, 350);
            panelHoaDon.TabIndex = 1;
            // 
            // dgvHoaDon
            // 
            dgvHoaDon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.BackgroundColor = SystemColors.Control;
            dgvHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHoaDon.Location = new Point(32, 184);
            dgvHoaDon.Margin = new Padding(6);
            dgvHoaDon.Name = "dgvHoaDon";
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.RowHeadersWidth = 51;
            dgvHoaDon.Size = new Size(1067, 499);
            dgvHoaDon.TabIndex = 3;
            // 
            // cbTrangThaiHD
            // 
            cbTrangThaiHD.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbTrangThaiHD.FormattingEnabled = true;
            cbTrangThaiHD.Location = new Point(816, 102);
            cbTrangThaiHD.Margin = new Padding(6);
            cbTrangThaiHD.Name = "cbTrangThaiHD";
            cbTrangThaiHD.Size = new Size(278, 49);
            cbTrangThaiHD.TabIndex = 2;
            // 
            // txtTimKiemHD
            // 
            txtTimKiemHD.ForeColor = Color.Gray;
            txtTimKiemHD.Location = new Point(32, 102);
            txtTimKiemHD.Margin = new Padding(6);
            txtTimKiemHD.Name = "txtTimKiemHD";
            txtTimKiemHD.PlaceholderText = "Tim kiem ma hd";
            txtTimKiemHD.Size = new Size(261, 47);
            txtTimKiemHD.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(32, 31);
            label4.Margin = new Padding(6, 0, 6, 0);
            label4.Name = "label4";
            label4.Size = new Size(285, 28);
            label4.TabIndex = 0;
            label4.Text = "Danh sách hóa đơn gần nhất";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(6, 6);
            label3.Margin = new Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Padding = new Padding(10);
            label3.Size = new Size(337, 61);
            label3.TabIndex = 0;
            label3.Text = "Hoá đơn và giao dịch";
            // 
            // tabPageTonKho
            // 
            tabPageTonKho.Controls.Add(panel2);
            tabPageTonKho.Controls.Add(label5);
            tabPageTonKho.Location = new Point(10, 58);
            tabPageTonKho.Margin = new Padding(6);
            tabPageTonKho.Name = "tabPageTonKho";
            tabPageTonKho.Size = new Size(1149, 854);
            tabPageTonKho.TabIndex = 2;
            tabPageTonKho.Text = "Kho";
            tabPageTonKho.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(dgvTonKho);
            panel2.Controls.Add(btnThemMoiKho);
            panel2.Controls.Add(txtTimKiemKho);
            panel2.Controls.Add(label6);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 66);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(15);
            panel2.Size = new Size(542, 351);
            panel2.TabIndex = 1;
            // 
            // dgvTonKho
            // 
            dgvTonKho.AllowDrop = true;
            dgvTonKho.AllowUserToAddRows = false;
            dgvTonKho.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTonKho.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTonKho.BackgroundColor = SystemColors.Control;
            dgvTonKho.BorderStyle = BorderStyle.None;
            dgvTonKho.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTonKho.Location = new Point(32, 184);
            dgvTonKho.Margin = new Padding(6);
            dgvTonKho.Name = "dgvTonKho";
            dgvTonKho.ReadOnly = true;
            dgvTonKho.RowHeadersVisible = false;
            dgvTonKho.RowHeadersWidth = 51;
            dgvTonKho.Size = new Size(477, 243);
            dgvTonKho.TabIndex = 3;
            // 
            // btnThemMoiKho
            // 
            btnThemMoiKho.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnThemMoiKho.BackColor = Color.FromArgb(255, 153, 77);
            btnThemMoiKho.FlatAppearance.BorderSize = 0;
            btnThemMoiKho.FlatStyle = FlatStyle.Flat;
            btnThemMoiKho.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThemMoiKho.ForeColor = Color.White;
            btnThemMoiKho.Location = new Point(785, 102);
            btnThemMoiKho.Margin = new Padding(6);
            btnThemMoiKho.Name = "btnThemMoiKho";
            btnThemMoiKho.Size = new Size(255, 59);
            btnThemMoiKho.TabIndex = 2;
            btnThemMoiKho.Text = "+ Thêm mới";
            btnThemMoiKho.UseVisualStyleBackColor = false;
            btnThemMoiKho.Visible = false;
            // 
            // txtTimKiemKho
            // 
            txtTimKiemKho.ForeColor = Color.Gray;
            txtTimKiemKho.Location = new Point(32, 102);
            txtTimKiemKho.Margin = new Padding(6);
            txtTimKiemKho.Name = "txtTimKiemKho";
            txtTimKiemKho.Size = new Size(316, 47);
            txtTimKiemKho.TabIndex = 1;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Top;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(32, 31);
            label6.Margin = new Padding(6, 0, 6, 0);
            label6.Name = "label6";
            label6.Size = new Size(479, 54);
            label6.TabIndex = 0;
            label6.Text = "Danh sách hàng tồn kho";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Top;
            label5.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(0, 0);
            label5.Margin = new Padding(6, 0, 6, 0);
            label5.Name = "label5";
            label5.Padding = new Padding(10);
            label5.Size = new Size(288, 66);
            label5.TabIndex = 0;
            label5.Text = "Quản lí tồn kho";
            // 
            // QuanLi
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1700, 922);
            Controls.Add(panelContent);
            Controls.Add(panelMenu);
            Margin = new Padding(6);
            Name = "QuanLi";
            Text = "Quản lí cửa hàng";
            Load += QuanLi_Load_1;
            panelMenu.ResumeLayout(false);
            panelMenu.PerformLayout();
            grpNotify.ResumeLayout(false);
            grpNotify.PerformLayout();
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
        private Label lblCoffee;
        private Panel panelContent;
        private Button btnDangXuat;
        private GroupBox grpNotify;
        private TextBox txtNotifyMessage;
        private Button btnSendNotify;
        private TabControl tabControlMain;
        private TabPage tabPageQuanLyNV;
        private TabPage tabPageHoaDon;
        private TabPage tabPageTonKho;
        private Label label2;
        private Panel panel1;
        private Panel panelHoaDon;
        private TextBox txtTimKiemHD;
        private DataGridView dgvPerformance;
        private Button btnLoc;
        private ComboBox cbThang;
        private Label lblTieuDeHieuSuat;
        private Label label4;
        private Label label3;
        private DataGridView dgvHoaDon;
        private ComboBox cbTrangThaiHD;
        private Panel panel2;
        private Label label6;
        private Label label5;
        private Button btnThemMoiKho;
        private TextBox txtTimKiemKho;
        private DataGridView dgvTonKho;
    }
}