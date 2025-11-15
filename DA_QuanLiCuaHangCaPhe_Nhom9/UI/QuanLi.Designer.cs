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
        private void InitializeComponent()
        {
            panelMenu = new Panel();
            btnChuyenFormOder = new Button();
            lblCoffee = new Label();
            txtNotifyMessage = new TextBox();
            grpNotify = new GroupBox();
            btnSendNotify = new Button();
            panelContent = new Panel();
            tabControlMain = new TabControl();
            tabPageQuanLyNV = new TabPage();
            panel1 = new Panel();
            dgvPerformance = new DataGridView();
            panel3 = new Panel();
            btnLoc = new Button();
            cbThang = new ComboBox();
            lblTieuDeHieuSuat = new Label();
            label2 = new Label();
            tabPageHoaDon = new TabPage();
            panelHoaDon = new Panel();
            dgvHoaDon = new DataGridView();
            panel4 = new Panel();
            txtTimKiemHD = new TextBox();
            label4 = new Label();
            cbTrangThaiHD = new ComboBox();
            label3 = new Label();
            tabPageTonKho = new TabPage();
            panel2 = new Panel();
            dgvTonKho = new DataGridView();
            panel5 = new Panel();
            txtTimKiemKho = new TextBox();
            label6 = new Label();
            btnThemMoiKho = new Button();
            label5 = new Label();
            tabPageSanPham = new TabPage();
            panelSanPham = new Panel();
            dgvSanPham = new DataGridView();
            panelSPTop = new Panel();
            txtTimKiemSP = new TextBox();
            lblDanhSachSP = new Label();
            lblTitleSP = new Label();
            tabPageKhuyenMai = new TabPage();
            panelKhuyenMai = new Panel();
            dgvKhuyenMai = new DataGridView();
            panelKMTop = new Panel();
            lblLocTrangThaiKM = new Label();
            cbLocTrangThaiKM = new ComboBox();
            txtTimKiemKM = new TextBox();
            lblDanhSachKM = new Label();
            lblTitleKM = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            panelMenu.SuspendLayout();
            grpNotify.SuspendLayout();
            panelContent.SuspendLayout();
            tabControlMain.SuspendLayout();
            tabPageQuanLyNV.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPerformance).BeginInit();
            panel3.SuspendLayout();
            tabPageHoaDon.SuspendLayout();
            panelHoaDon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).BeginInit();
            panel4.SuspendLayout();
            tabPageTonKho.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTonKho).BeginInit();
            panel5.SuspendLayout();
            tabPageSanPham.SuspendLayout();
            panelSanPham.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSanPham).BeginInit();
            panelSPTop.SuspendLayout();
            tabPageKhuyenMai.SuspendLayout();
            panelKhuyenMai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKhuyenMai).BeginInit();
            panelKMTop.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.AutoSize = true;
            panelMenu.BackColor = Color.FromArgb(45, 45, 48);
            panelMenu.Controls.Add(btnChuyenFormOder);
            panelMenu.Controls.Add(lblCoffee);
            panelMenu.Dock = DockStyle.Fill;
            panelMenu.Location = new Point(6, 6);
            panelMenu.Margin = new Padding(6);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(239, 438);
            panelMenu.TabIndex = 1;
            // 
            // btnChuyenFormOder
            // 
            btnChuyenFormOder.BackColor = Color.FromArgb(220, 80, 0);
            btnChuyenFormOder.Dock = DockStyle.Bottom;
            btnChuyenFormOder.FlatAppearance.BorderSize = 0;
            btnChuyenFormOder.FlatStyle = FlatStyle.Flat;
            btnChuyenFormOder.Location = new Point(0, 391);
            btnChuyenFormOder.Name = "btnChuyenFormOder";
            btnChuyenFormOder.Size = new Size(239, 47);
            btnChuyenFormOder.TabIndex = 6;
            btnChuyenFormOder.Text = "Trang Order";
            btnChuyenFormOder.UseVisualStyleBackColor = false;
            btnChuyenFormOder.Click += btnChuyenFormOder_Click;
            // 
            // lblCoffee
            // 
            lblCoffee.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblCoffee.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCoffee.ForeColor = Color.FromArgb(192, 64, 0);
            lblCoffee.Location = new Point(35, 188);
            lblCoffee.Margin = new Padding(6, 0, 6, 0);
            lblCoffee.Name = "lblCoffee";
            lblCoffee.Size = new Size(176, 60);
            lblCoffee.TabIndex = 8;
            lblCoffee.Text = "COFFEE";
            lblCoffee.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtNotifyMessage
            // 
            txtNotifyMessage.Dock = DockStyle.Fill;
            txtNotifyMessage.Location = new Point(1, 21);
            txtNotifyMessage.Margin = new Padding(1);
            txtNotifyMessage.Multiline = true;
            txtNotifyMessage.Name = "txtNotifyMessage";
            txtNotifyMessage.PlaceholderText = "Nội dung thông báo";
            txtNotifyMessage.Size = new Size(448, 39);
            txtNotifyMessage.TabIndex = 0;
            // 
            // grpNotify
            // 
            grpNotify.Controls.Add(txtNotifyMessage);
            grpNotify.Controls.Add(btnSendNotify);
            grpNotify.Dock = DockStyle.Bottom;
            grpNotify.Location = new Point(7, 290);
            grpNotify.Margin = new Padding(1);
            grpNotify.Name = "grpNotify";
            grpNotify.Padding = new Padding(1);
            grpNotify.Size = new Size(519, 61);
            grpNotify.TabIndex = 5;
            grpNotify.TabStop = false;
            grpNotify.Text = "Gửi thông báo";
            grpNotify.Enter += grpNotify_Enter;
            // 
            // btnSendNotify
            // 
            btnSendNotify.Dock = DockStyle.Right;
            btnSendNotify.Location = new Point(449, 21);
            btnSendNotify.Margin = new Padding(1);
            btnSendNotify.Name = "btnSendNotify";
            btnSendNotify.Size = new Size(69, 39);
            btnSendNotify.TabIndex = 1;
            btnSendNotify.Text = "Gửi";
            btnSendNotify.UseVisualStyleBackColor = true;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(tabControlMain);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(254, 3);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(543, 444);
            panelContent.TabIndex = 2;
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabPageQuanLyNV);
            tabControlMain.Controls.Add(tabPageHoaDon);
            tabControlMain.Controls.Add(tabPageTonKho);
            tabControlMain.Controls.Add(tabPageSanPham);
            tabControlMain.Controls.Add(tabPageKhuyenMai);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Location = new Point(0, 0);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(543, 444);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageQuanLyNV
            // 
            tabPageQuanLyNV.Controls.Add(panel1);
            tabPageQuanLyNV.Controls.Add(label2);
            tabPageQuanLyNV.Location = new Point(4, 29);
            tabPageQuanLyNV.Name = "tabPageQuanLyNV";
            tabPageQuanLyNV.Padding = new Padding(1);
            tabPageQuanLyNV.Size = new Size(535, 411);
            tabPageQuanLyNV.TabIndex = 0;
            tabPageQuanLyNV.Text = "Nhân viên";
            tabPageQuanLyNV.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dgvPerformance);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(grpNotify);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(1, 52);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(7);
            panel1.Size = new Size(533, 358);
            panel1.TabIndex = 1;
            // 
            // dgvPerformance
            // 
            dgvPerformance.AllowUserToAddRows = false;
            dgvPerformance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPerformance.BackgroundColor = SystemColors.Control;
            dgvPerformance.BorderStyle = BorderStyle.None;
            dgvPerformance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPerformance.Dock = DockStyle.Fill;
            dgvPerformance.Location = new Point(7, 74);
            dgvPerformance.Name = "dgvPerformance";
            dgvPerformance.ReadOnly = true;
            dgvPerformance.RowHeadersVisible = false;
            dgvPerformance.RowHeadersWidth = 51;
            dgvPerformance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPerformance.Size = new Size(519, 216);
            dgvPerformance.TabIndex = 3;
            dgvPerformance.CellContentClick += dgvPerformance_CellContentClick;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnLoc);
            panel3.Controls.Add(cbThang);
            panel3.Controls.Add(lblTieuDeHieuSuat);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(7, 7);
            panel3.Name = "panel3";
            panel3.Size = new Size(519, 67);
            panel3.TabIndex = 7;
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.FromArgb(255, 153, 77);
            btnLoc.FlatAppearance.BorderSize = 0;
            btnLoc.FlatStyle = FlatStyle.Flat;
            btnLoc.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoc.ForeColor = Color.White;
            btnLoc.Location = new Point(157, 31);
            btnLoc.Name = "btnLoc";
            btnLoc.Size = new Size(110, 31);
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
            cbThang.Location = new Point(3, 31);
            cbThang.Name = "cbThang";
            cbThang.Size = new Size(151, 31);
            cbThang.TabIndex = 1;
            cbThang.SelectedIndexChanged += cbThang_SelectedIndexChanged;
            // 
            // lblTieuDeHieuSuat
            // 
            lblTieuDeHieuSuat.AutoSize = true;
            lblTieuDeHieuSuat.Dock = DockStyle.Top;
            lblTieuDeHieuSuat.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTieuDeHieuSuat.Location = new Point(0, 0);
            lblTieuDeHieuSuat.Name = "lblTieuDeHieuSuat";
            lblTieuDeHieuSuat.Size = new Size(334, 28);
            lblTieuDeHieuSuat.TabIndex = 0;
            lblTieuDeHieuSuat.Text = "Hiệu suất bán hàng của nhân viên";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(1, 1);
            label2.Name = "label2";
            label2.Padding = new Padding(5);
            label2.Size = new Size(414, 51);
            label2.TabIndex = 0;
            label2.Text = "Quản lí nhân viên bán hàng";
            // 
            // tabPageHoaDon
            // 
            tabPageHoaDon.Controls.Add(panelHoaDon);
            tabPageHoaDon.Controls.Add(label3);
            tabPageHoaDon.Location = new Point(4, 29);
            tabPageHoaDon.Name = "tabPageHoaDon";
            tabPageHoaDon.Padding = new Padding(1);
            tabPageHoaDon.Size = new Size(535, 411);
            tabPageHoaDon.TabIndex = 1;
            tabPageHoaDon.Text = "Hóa đơn";
            tabPageHoaDon.UseVisualStyleBackColor = true;
            // 
            // panelHoaDon
            // 
            panelHoaDon.BackColor = Color.White;
            panelHoaDon.Controls.Add(dgvHoaDon);
            panelHoaDon.Controls.Add(panel4);
            panelHoaDon.Controls.Add(cbTrangThaiHD);
            panelHoaDon.Dock = DockStyle.Fill;
            panelHoaDon.Location = new Point(1, 52);
            panelHoaDon.Name = "panelHoaDon";
            panelHoaDon.Padding = new Padding(7);
            panelHoaDon.Size = new Size(533, 358);
            panelHoaDon.TabIndex = 1;
            // 
            // dgvHoaDon
            // 
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.BackgroundColor = SystemColors.Control;
            dgvHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHoaDon.Dock = DockStyle.Fill;
            dgvHoaDon.Location = new Point(7, 72);
            dgvHoaDon.Name = "dgvHoaDon";
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.RowHeadersWidth = 51;
            dgvHoaDon.Size = new Size(519, 279);
            dgvHoaDon.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.Controls.Add(txtTimKiemHD);
            panel4.Controls.Add(label4);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(7, 7);
            panel4.Name = "panel4";
            panel4.Size = new Size(519, 65);
            panel4.TabIndex = 3;
            // 
            // txtTimKiemHD
            // 
            txtTimKiemHD.ForeColor = Color.Gray;
            txtTimKiemHD.Location = new Point(1, 31);
            txtTimKiemHD.Name = "txtTimKiemHD";
            txtTimKiemHD.PlaceholderText = "Tim kiem ma hd";
            txtTimKiemHD.Size = new Size(285, 27);
            txtTimKiemHD.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(285, 28);
            label4.TabIndex = 0;
            label4.Text = "Danh sách hóa đơn gần nhất";
            // 
            // cbTrangThaiHD
            // 
            cbTrangThaiHD.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbTrangThaiHD.FormattingEnabled = true;
            cbTrangThaiHD.Location = new Point(665, 50);
            cbTrangThaiHD.Name = "cbTrangThaiHD";
            cbTrangThaiHD.Size = new Size(133, 28);
            cbTrangThaiHD.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(1, 1);
            label3.Name = "label3";
            label3.Padding = new Padding(5);
            label3.Size = new Size(327, 51);
            label3.TabIndex = 0;
            label3.Text = "Hoá đơn và giao dịch";
            // 
            // tabPageTonKho
            // 
            tabPageTonKho.Controls.Add(panel2);
            tabPageTonKho.Controls.Add(label5);
            tabPageTonKho.Location = new Point(4, 29);
            tabPageTonKho.Name = "tabPageTonKho";
            tabPageTonKho.Size = new Size(535, 411);
            tabPageTonKho.TabIndex = 2;
            tabPageTonKho.Text = "Kho";
            tabPageTonKho.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(dgvTonKho);
            panel2.Controls.Add(panel5);
            panel2.Controls.Add(btnThemMoiKho);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 56);
            panel2.Margin = new Padding(1);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(7);
            panel2.Size = new Size(535, 355);
            panel2.TabIndex = 1;
            // 
            // dgvTonKho
            // 
            dgvTonKho.AllowDrop = true;
            dgvTonKho.AllowUserToAddRows = false;
            dgvTonKho.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTonKho.BackgroundColor = SystemColors.Control;
            dgvTonKho.BorderStyle = BorderStyle.None;
            dgvTonKho.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTonKho.Dock = DockStyle.Fill;
            dgvTonKho.Location = new Point(7, 73);
            dgvTonKho.Name = "dgvTonKho";
            dgvTonKho.ReadOnly = true;
            dgvTonKho.RowHeadersVisible = false;
            dgvTonKho.RowHeadersWidth = 51;
            dgvTonKho.Size = new Size(521, 275);
            dgvTonKho.TabIndex = 3;
            // 
            // panel5
            // 
            panel5.Controls.Add(txtTimKiemKho);
            panel5.Controls.Add(label6);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(7, 7);
            panel5.Name = "panel5";
            panel5.Size = new Size(521, 66);
            panel5.TabIndex = 3;
            // 
            // txtTimKiemKho
            // 
            txtTimKiemKho.ForeColor = Color.Gray;
            txtTimKiemKho.Location = new Point(3, 31);
            txtTimKiemKho.Name = "txtTimKiemKho";
            txtTimKiemKho.Size = new Size(151, 27);
            txtTimKiemKho.TabIndex = 1;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Top;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(242, 28);
            label6.TabIndex = 0;
            label6.Text = "Danh sách hàng tồn kho";
            // 
            // btnThemMoiKho
            // 
            btnThemMoiKho.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnThemMoiKho.BackColor = Color.FromArgb(255, 153, 77);
            btnThemMoiKho.FlatAppearance.BorderSize = 0;
            btnThemMoiKho.FlatStyle = FlatStyle.Flat;
            btnThemMoiKho.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThemMoiKho.ForeColor = Color.White;
            btnThemMoiKho.Location = new Point(649, 50);
            btnThemMoiKho.Name = "btnThemMoiKho";
            btnThemMoiKho.Size = new Size(120, 29);
            btnThemMoiKho.TabIndex = 2;
            btnThemMoiKho.Text = "+ Thêm mới";
            btnThemMoiKho.UseVisualStyleBackColor = false;
            btnThemMoiKho.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Top;
            label5.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Padding = new Padding(5);
            label5.Size = new Size(278, 56);
            label5.TabIndex = 0;
            label5.Text = "Quản lí tồn kho";
            // 
            // tabPageSanPham
            // 
            tabPageSanPham.Controls.Add(panelSanPham);
            tabPageSanPham.Controls.Add(lblTitleSP);
            tabPageSanPham.Location = new Point(4, 29);
            tabPageSanPham.Name = "tabPageSanPham";
            tabPageSanPham.Size = new Size(535, 411);
            tabPageSanPham.TabIndex = 3;
            tabPageSanPham.Text = "Sản Phẩm";
            tabPageSanPham.UseVisualStyleBackColor = true;
            // 
            // panelSanPham
            // 
            panelSanPham.BackColor = Color.White;
            panelSanPham.Controls.Add(dgvSanPham);
            panelSanPham.Controls.Add(panelSPTop);
            panelSanPham.Dock = DockStyle.Fill;
            panelSanPham.Location = new Point(0, 56);
            panelSanPham.Name = "panelSanPham";
            panelSanPham.Padding = new Padding(7);
            panelSanPham.Size = new Size(535, 355);
            panelSanPham.TabIndex = 1;
            // 
            // dgvSanPham
            // 
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSanPham.BackgroundColor = SystemColors.Control;
            dgvSanPham.BorderStyle = BorderStyle.None;
            dgvSanPham.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSanPham.Dock = DockStyle.Fill;
            dgvSanPham.Location = new Point(7, 74);
            dgvSanPham.Name = "dgvSanPham";
            dgvSanPham.ReadOnly = true;
            dgvSanPham.RowHeadersVisible = false;
            dgvSanPham.RowHeadersWidth = 51;
            dgvSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSanPham.Size = new Size(521, 274);
            dgvSanPham.TabIndex = 1;
            // 
            // panelSPTop
            // 
            panelSPTop.Controls.Add(txtTimKiemSP);
            panelSPTop.Controls.Add(lblDanhSachSP);
            panelSPTop.Dock = DockStyle.Top;
            panelSPTop.Location = new Point(7, 7);
            panelSPTop.Name = "panelSPTop";
            panelSPTop.Size = new Size(521, 67);
            panelSPTop.TabIndex = 0;
            // 
            // txtTimKiemSP
            // 
            txtTimKiemSP.ForeColor = Color.Gray;
            txtTimKiemSP.Location = new Point(3, 33);
            txtTimKiemSP.Name = "txtTimKiemSP";
            txtTimKiemSP.PlaceholderText = "Tìm theo tên sản phẩm";
            txtTimKiemSP.Size = new Size(188, 27);
            txtTimKiemSP.TabIndex = 1;
            // 
            // lblDanhSachSP
            // 
            lblDanhSachSP.AutoSize = true;
            lblDanhSachSP.Dock = DockStyle.Top;
            lblDanhSachSP.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDanhSachSP.Location = new Point(0, 0);
            lblDanhSachSP.Name = "lblDanhSachSP";
            lblDanhSachSP.Size = new Size(207, 28);
            lblDanhSachSP.TabIndex = 0;
            lblDanhSachSP.Text = "Danh sách sản phẩm";
            // 
            // lblTitleSP
            // 
            lblTitleSP.AutoSize = true;
            lblTitleSP.Dock = DockStyle.Top;
            lblTitleSP.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitleSP.Location = new Point(0, 0);
            lblTitleSP.Name = "lblTitleSP";
            lblTitleSP.Padding = new Padding(5);
            lblTitleSP.Size = new Size(307, 56);
            lblTitleSP.TabIndex = 0;
            lblTitleSP.Text = "Quản lí sản phẩm";
            // 
            // tabPageKhuyenMai
            // 
            tabPageKhuyenMai.Controls.Add(panelKhuyenMai);
            tabPageKhuyenMai.Controls.Add(lblTitleKM);
            tabPageKhuyenMai.Location = new Point(4, 29);
            tabPageKhuyenMai.Name = "tabPageKhuyenMai";
            tabPageKhuyenMai.Padding = new Padding(3);
            tabPageKhuyenMai.Size = new Size(535, 411);
            tabPageKhuyenMai.TabIndex = 4;
            tabPageKhuyenMai.Text = "Khuyến mãi";
            tabPageKhuyenMai.UseVisualStyleBackColor = true;
            // 
            // panelKhuyenMai
            // 
            panelKhuyenMai.BackColor = Color.White;
            panelKhuyenMai.Controls.Add(dgvKhuyenMai);
            panelKhuyenMai.Controls.Add(panelKMTop);
            panelKhuyenMai.Dock = DockStyle.Fill;
            panelKhuyenMai.Location = new Point(3, 59);
            panelKhuyenMai.Name = "panelKhuyenMai";
            panelKhuyenMai.Padding = new Padding(7);
            panelKhuyenMai.Size = new Size(529, 349);
            panelKhuyenMai.TabIndex = 1;
            // 
            // dgvKhuyenMai
            // 
            dgvKhuyenMai.AllowUserToAddRows = false;
            dgvKhuyenMai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKhuyenMai.BackgroundColor = SystemColors.Control;
            dgvKhuyenMai.BorderStyle = BorderStyle.None;
            dgvKhuyenMai.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKhuyenMai.Dock = DockStyle.Fill;
            dgvKhuyenMai.Location = new Point(7, 74);
            dgvKhuyenMai.Name = "dgvKhuyenMai";
            dgvKhuyenMai.ReadOnly = true;
            dgvKhuyenMai.RowHeadersVisible = false;
            dgvKhuyenMai.RowHeadersWidth = 51;
            dgvKhuyenMai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhuyenMai.Size = new Size(515, 268);
            dgvKhuyenMai.TabIndex = 1;
            // 
            // panelKMTop
            // 
            panelKMTop.Controls.Add(lblLocTrangThaiKM);
            panelKMTop.Controls.Add(cbLocTrangThaiKM);
            panelKMTop.Controls.Add(txtTimKiemKM);
            panelKMTop.Controls.Add(lblDanhSachKM);
            panelKMTop.Dock = DockStyle.Top;
            panelKMTop.Location = new Point(7, 7);
            panelKMTop.Name = "panelKMTop";
            panelKMTop.Size = new Size(515, 67);
            panelKMTop.TabIndex = 0;
            // 
            // lblLocTrangThaiKM
            // 
            lblLocTrangThaiKM.AutoSize = true;
            lblLocTrangThaiKM.Location = new Point(232, 7);
            lblLocTrangThaiKM.Name = "lblLocTrangThaiKM";
            lblLocTrangThaiKM.Size = new Size(78, 20);
            lblLocTrangThaiKM.TabIndex = 5;
            lblLocTrangThaiKM.Text = "Trạng thái:";
            // 
            // cbLocTrangThaiKM
            // 
            cbLocTrangThaiKM.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLocTrangThaiKM.FormattingEnabled = true;
            cbLocTrangThaiKM.Location = new Point(197, 33);
            cbLocTrangThaiKM.Name = "cbLocTrangThaiKM";
            cbLocTrangThaiKM.Size = new Size(121, 28);
            cbLocTrangThaiKM.TabIndex = 4;
            // 
            // txtTimKiemKM
            // 
            txtTimKiemKM.ForeColor = Color.Gray;
            txtTimKiemKM.Location = new Point(3, 33);
            txtTimKiemKM.Name = "txtTimKiemKM";
            txtTimKiemKM.PlaceholderText = "Tìm theo tên KM";
            txtTimKiemKM.Size = new Size(188, 27);
            txtTimKiemKM.TabIndex = 1;
            // 
            // lblDanhSachKM
            // 
            lblDanhSachKM.AutoSize = true;
            lblDanhSachKM.Dock = DockStyle.Top;
            lblDanhSachKM.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDanhSachKM.Location = new Point(0, 0);
            lblDanhSachKM.Name = "lblDanhSachKM";
            lblDanhSachKM.Size = new Size(226, 28);
            lblDanhSachKM.TabIndex = 0;
            lblDanhSachKM.Text = "Danh sách khuyến mãi";
            // 
            // lblTitleKM
            // 
            lblTitleKM.AutoSize = true;
            lblTitleKM.Dock = DockStyle.Top;
            lblTitleKM.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitleKM.Location = new Point(3, 3);
            lblTitleKM.Name = "lblTitleKM";
            lblTitleKM.Padding = new Padding(5);
            lblTitleKM.Size = new Size(339, 56);
            lblTitleKM.TabIndex = 0;
            lblTitleKM.Text = "Quản lí khuyến mãi";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31.375F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68.625F));
            tableLayoutPanel1.Controls.Add(panelContent, 1, 0);
            tableLayoutPanel1.Controls.Add(panelMenu, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // QuanLi
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "QuanLi";
            Text = "Quản lí cửa hàng";
            WindowState = FormWindowState.Maximized;
            Load += QuanLi_Load_1;
            panelMenu.ResumeLayout(false);
            grpNotify.ResumeLayout(false);
            grpNotify.PerformLayout();
            panelContent.ResumeLayout(false);
            tabControlMain.ResumeLayout(false);
            tabPageQuanLyNV.ResumeLayout(false);
            tabPageQuanLyNV.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPerformance).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tabPageHoaDon.ResumeLayout(false);
            tabPageHoaDon.PerformLayout();
            panelHoaDon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            tabPageTonKho.ResumeLayout(false);
            tabPageTonKho.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTonKho).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            tabPageSanPham.ResumeLayout(false);
            tabPageSanPham.PerformLayout();
            panelSanPham.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSanPham).EndInit();
            panelSPTop.ResumeLayout(false);
            panelSPTop.PerformLayout();
            tabPageKhuyenMai.ResumeLayout(false);
            tabPageKhuyenMai.PerformLayout();
            panelKhuyenMai.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvKhuyenMai).EndInit();
            panelKMTop.ResumeLayout(false);
            panelKMTop.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMenu;
        private Label lblCoffee;
        private Panel panelContent;
        private Button btnChuyenFormOder;
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
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dgvPerformance;
        private Panel panel3;
        private Button btnLoc;
        private Panel panel4;
        private Panel panel5;
        private TabPage tabPageSanPham;
        private Panel panelSanPham;
        private DataGridView dgvSanPham;
        private Panel panelSPTop;
        private TextBox txtTimKiemSP;
        private Label lblDanhSachSP;
        private Label lblTitleSP;

        // CÁC CONTROL MỚI CHO TAB KHUYẾN MÃI
        private TabPage tabPageKhuyenMai;
        private Panel panelKhuyenMai;
        private DataGridView dgvKhuyenMai;
        private Panel panelKMTop;
        private TextBox txtTimKiemKM;
        private Label lblDanhSachKM;
        private Label lblTitleKM;
        private ComboBox cbLocTrangThaiKM;
        private Label lblLocTrangThaiKM;
    }
}