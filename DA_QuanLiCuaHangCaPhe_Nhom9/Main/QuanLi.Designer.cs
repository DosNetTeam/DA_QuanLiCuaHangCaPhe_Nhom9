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
            panel4 = new Panel();
            txtTimKiemHD = new TextBox();
            label4 = new Label();
            dgvHoaDon = new DataGridView();
            groupBox1 = new GroupBox();
            textBox1 = new TextBox();
            button1 = new Button();
            cbTrangThaiHD = new ComboBox();
            label3 = new Label();
            tabPageTonKho = new TabPage();
            panel2 = new Panel();
            dgvTonKho = new DataGridView();
            groupBox2 = new GroupBox();
            textBox2 = new TextBox();
            button2 = new Button();
            panel5 = new Panel();
            txtTimKiemKho = new TextBox();
            label6 = new Label();
            btnThemMoiKho = new Button();
            label5 = new Label();
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
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).BeginInit();
            groupBox1.SuspendLayout();
            tabPageTonKho.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTonKho).BeginInit();
            groupBox2.SuspendLayout();
            panel5.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.AutoSize = true;
            panelMenu.BackColor = Color.FromArgb(45, 45, 48);
            panelMenu.Controls.Add(btnDangXuat);
            panelMenu.Controls.Add(lblCoffee);
            panelMenu.Dock = DockStyle.Fill;
            panelMenu.Location = new Point(13, 12);
            panelMenu.Margin = new Padding(13, 12, 13, 12);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(507, 898);
            panelMenu.TabIndex = 1;
            // 
            // btnDangXuat
            // 
            btnDangXuat.BackColor = Color.FromArgb(220, 80, 0);
            btnDangXuat.Dock = DockStyle.Bottom;
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Location = new Point(0, 802);
            btnDangXuat.Margin = new Padding(6);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(507, 96);
            btnDangXuat.TabIndex = 6;
            btnDangXuat.Text = "Trang Order";
            btnDangXuat.UseVisualStyleBackColor = false;
            btnDangXuat.Click += btnDangXuat_Click;
            // 
            // lblCoffee
            // 
            lblCoffee.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblCoffee.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCoffee.ForeColor = Color.FromArgb(192, 64, 0);
            lblCoffee.Location = new Point(74, 385);
            lblCoffee.Margin = new Padding(13, 0, 13, 0);
            lblCoffee.Name = "lblCoffee";
            lblCoffee.Size = new Size(373, 123);
            lblCoffee.TabIndex = 8;
            lblCoffee.Text = "COFFEE";
            lblCoffee.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtNotifyMessage
            // 
            txtNotifyMessage.Dock = DockStyle.Fill;
            txtNotifyMessage.Location = new Point(2, 42);
            txtNotifyMessage.Margin = new Padding(2);
            txtNotifyMessage.Multiline = true;
            txtNotifyMessage.Name = "txtNotifyMessage";
            txtNotifyMessage.PlaceholderText = "Nội dung thông báo";
            txtNotifyMessage.Size = new Size(950, 81);
            txtNotifyMessage.TabIndex = 0;
            // 
            // grpNotify
            // 
            grpNotify.Controls.Add(txtNotifyMessage);
            grpNotify.Controls.Add(btnSendNotify);
            grpNotify.Dock = DockStyle.Bottom;
            grpNotify.Location = new Point(15, 598);
            grpNotify.Margin = new Padding(2);
            grpNotify.Name = "grpNotify";
            grpNotify.Padding = new Padding(2);
            grpNotify.Size = new Size(1101, 125);
            grpNotify.TabIndex = 5;
            grpNotify.TabStop = false;
            grpNotify.Text = "Gửi thông báo";
            grpNotify.Enter += grpNotify_Enter;
            // 
            // btnSendNotify
            // 
            btnSendNotify.Dock = DockStyle.Right;
            btnSendNotify.Location = new Point(952, 42);
            btnSendNotify.Margin = new Padding(2);
            btnSendNotify.Name = "btnSendNotify";
            btnSendNotify.Size = new Size(147, 81);
            btnSendNotify.TabIndex = 1;
            btnSendNotify.Text = "Gửi";
            btnSendNotify.UseVisualStyleBackColor = true;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(tabControlMain);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(539, 6);
            panelContent.Margin = new Padding(6);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(1155, 910);
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
            tabControlMain.Size = new Size(1155, 910);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageQuanLyNV
            // 
            tabPageQuanLyNV.Controls.Add(panel1);
            tabPageQuanLyNV.Controls.Add(label2);
            tabPageQuanLyNV.Location = new Point(10, 58);
            tabPageQuanLyNV.Margin = new Padding(6);
            tabPageQuanLyNV.Name = "tabPageQuanLyNV";
            tabPageQuanLyNV.Padding = new Padding(2);
            tabPageQuanLyNV.Size = new Size(1135, 842);
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
            panel1.Location = new Point(2, 103);
            panel1.Margin = new Padding(6);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(15, 14, 15, 14);
            panel1.Size = new Size(1131, 737);
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
            dgvPerformance.Location = new Point(15, 151);
            dgvPerformance.Margin = new Padding(6);
            dgvPerformance.Name = "dgvPerformance";
            dgvPerformance.ReadOnly = true;
            dgvPerformance.RowHeadersVisible = false;
            dgvPerformance.RowHeadersWidth = 51;
            dgvPerformance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPerformance.Size = new Size(1101, 447);
            dgvPerformance.TabIndex = 3;
            dgvPerformance.CellContentClick += dgvPerformance_CellContentClick;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnLoc);
            panel3.Controls.Add(cbThang);
            panel3.Controls.Add(lblTieuDeHieuSuat);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(15, 14);
            panel3.Margin = new Padding(6);
            panel3.Name = "panel3";
            panel3.Size = new Size(1101, 137);
            panel3.TabIndex = 7;
            // 
            // btnLoc
            // 
            btnLoc.BackColor = Color.FromArgb(255, 153, 77);
            btnLoc.FlatAppearance.BorderSize = 0;
            btnLoc.FlatStyle = FlatStyle.Flat;
            btnLoc.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLoc.ForeColor = Color.White;
            btnLoc.Location = new Point(334, 64);
            btnLoc.Margin = new Padding(6);
            btnLoc.Name = "btnLoc";
            btnLoc.Size = new Size(234, 64);
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
            cbThang.Location = new Point(6, 64);
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
            lblTieuDeHieuSuat.Location = new Point(0, 0);
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
            label2.Location = new Point(2, 2);
            label2.Margin = new Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Padding = new Padding(11, 10, 11, 10);
            label2.Size = new Size(828, 101);
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
            tabPageHoaDon.Padding = new Padding(2);
            tabPageHoaDon.Size = new Size(1135, 842);
            tabPageHoaDon.TabIndex = 1;
            tabPageHoaDon.Text = "Hóa đơn";
            tabPageHoaDon.UseVisualStyleBackColor = true;
            // 
            // panelHoaDon
            // 
            panelHoaDon.BackColor = Color.White;
            panelHoaDon.Controls.Add(panel4);
            panelHoaDon.Controls.Add(dgvHoaDon);
            panelHoaDon.Controls.Add(groupBox1);
            panelHoaDon.Controls.Add(cbTrangThaiHD);
            panelHoaDon.Dock = DockStyle.Fill;
            panelHoaDon.Location = new Point(2, 103);
            panelHoaDon.Margin = new Padding(6);
            panelHoaDon.Name = "panelHoaDon";
            panelHoaDon.Padding = new Padding(15, 14, 15, 14);
            panelHoaDon.Size = new Size(1131, 737);
            panelHoaDon.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(txtTimKiemHD);
            panel4.Controls.Add(label4);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(15, 14);
            panel4.Margin = new Padding(6);
            panel4.Name = "panel4";
            panel4.Size = new Size(1101, 133);
            panel4.TabIndex = 3;
            // 
            // txtTimKiemHD
            // 
            txtTimKiemHD.ForeColor = Color.Gray;
            txtTimKiemHD.Location = new Point(2, 64);
            txtTimKiemHD.Margin = new Padding(6);
            txtTimKiemHD.Name = "txtTimKiemHD";
            txtTimKiemHD.PlaceholderText = "Tim kiem ma hd";
            txtTimKiemHD.Size = new Size(601, 47);
            txtTimKiemHD.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(0, 0);
            label4.Margin = new Padding(6, 0, 6, 0);
            label4.Name = "label4";
            label4.Size = new Size(562, 54);
            label4.TabIndex = 0;
            label4.Text = "Danh sách hóa đơn gần nhất";
            // 
            // dgvHoaDon
            // 
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.BackgroundColor = SystemColors.Control;
            dgvHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHoaDon.Dock = DockStyle.Fill;
            dgvHoaDon.Location = new Point(15, 14);
            dgvHoaDon.Margin = new Padding(6);
            dgvHoaDon.Name = "dgvHoaDon";
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.RowHeadersWidth = 51;
            dgvHoaDon.Size = new Size(1101, 584);
            dgvHoaDon.TabIndex = 3;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(button1);
            groupBox1.Dock = DockStyle.Bottom;
            groupBox1.Location = new Point(15, 598);
            groupBox1.Margin = new Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(2);
            groupBox1.Size = new Size(1101, 125);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Gửi thông báo";
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(2, 42);
            textBox1.Margin = new Padding(2);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Nội dung thông báo";
            textBox1.Size = new Size(950, 81);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.Location = new Point(952, 42);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(147, 81);
            button1.TabIndex = 1;
            button1.Text = "Gửi";
            button1.UseVisualStyleBackColor = true;
            // 
            // cbTrangThaiHD
            // 
            cbTrangThaiHD.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbTrangThaiHD.FormattingEnabled = true;
            cbTrangThaiHD.Location = new Point(1411, 102);
            cbTrangThaiHD.Margin = new Padding(6);
            cbTrangThaiHD.Name = "cbTrangThaiHD";
            cbTrangThaiHD.Size = new Size(278, 49);
            cbTrangThaiHD.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(2, 2);
            label3.Margin = new Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Padding = new Padding(11, 10, 11, 10);
            label3.Size = new Size(657, 101);
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
            tabPageTonKho.Size = new Size(1135, 842);
            tabPageTonKho.TabIndex = 2;
            tabPageTonKho.Text = "Kho";
            tabPageTonKho.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(dgvTonKho);
            panel2.Controls.Add(groupBox2);
            panel2.Controls.Add(panel5);
            panel2.Controls.Add(btnThemMoiKho);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 109);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(15, 14, 15, 14);
            panel2.Size = new Size(1135, 733);
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
            dgvTonKho.Location = new Point(15, 149);
            dgvTonKho.Margin = new Padding(6);
            dgvTonKho.Name = "dgvTonKho";
            dgvTonKho.ReadOnly = true;
            dgvTonKho.RowHeadersVisible = false;
            dgvTonKho.RowHeadersWidth = 51;
            dgvTonKho.Size = new Size(1105, 445);
            dgvTonKho.TabIndex = 3;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(textBox2);
            groupBox2.Controls.Add(button2);
            groupBox2.Dock = DockStyle.Bottom;
            groupBox2.Location = new Point(15, 594);
            groupBox2.Margin = new Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(2);
            groupBox2.Size = new Size(1105, 125);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Gửi thông báo";
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Location = new Point(2, 42);
            textBox2.Margin = new Padding(2);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Nội dung thông báo";
            textBox2.Size = new Size(954, 81);
            textBox2.TabIndex = 0;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Right;
            button2.Location = new Point(956, 42);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(147, 81);
            button2.TabIndex = 1;
            button2.Text = "Gửi";
            button2.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            panel5.Controls.Add(txtTimKiemKho);
            panel5.Controls.Add(label6);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(15, 14);
            panel5.Margin = new Padding(6);
            panel5.Name = "panel5";
            panel5.Size = new Size(1105, 135);
            panel5.TabIndex = 3;
            // 
            // txtTimKiemKho
            // 
            txtTimKiemKho.ForeColor = Color.Gray;
            txtTimKiemKho.Location = new Point(6, 64);
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
            label6.Location = new Point(0, 0);
            label6.Margin = new Padding(6, 0, 6, 0);
            label6.Name = "label6";
            label6.Size = new Size(479, 54);
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
            btnThemMoiKho.Location = new Point(1377, 102);
            btnThemMoiKho.Margin = new Padding(6);
            btnThemMoiKho.Name = "btnThemMoiKho";
            btnThemMoiKho.Size = new Size(255, 59);
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
            label5.Margin = new Padding(6, 0, 6, 0);
            label5.Name = "label5";
            label5.Padding = new Padding(11, 10, 11, 10);
            label5.Size = new Size(547, 109);
            label5.TabIndex = 0;
            label5.Text = "Quản lí tồn kho";
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
            tableLayoutPanel1.Margin = new Padding(6);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1700, 922);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // QuanLi
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1700, 922);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(6);
            Name = "QuanLi";
            Text = "Quản lí cửa hàng";
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
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHoaDon).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPageTonKho.ResumeLayout(false);
            tabPageTonKho.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTonKho).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
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
        private GroupBox groupBox1;
        private TextBox textBox1;
        private Button button1;
        private GroupBox groupBox2;
        private TextBox textBox2;
        private Button button2;
    }
}