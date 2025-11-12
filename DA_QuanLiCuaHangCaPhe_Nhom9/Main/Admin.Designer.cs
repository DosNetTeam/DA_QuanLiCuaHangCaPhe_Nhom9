namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    partial class Admin
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
            btnCreateAccount = new Button();
            btnExit = new Button();
            btnDeleteAccount = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            dgvOverview = new DataGridView();
            tabPage2 = new TabPage();
            panel1 = new Panel();
            button1 = new Button();
            textBoxPassword = new TextBox();
            textBox6 = new TextBox();
            textBox5 = new TextBox();
            textBox4 = new TextBox();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label7 = new Label();
            label6 = new Label();
            label14 = new Label();
            label5 = new Label();
            label8 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            tabPage3 = new TabPage();
            dgvInventory = new DataGridView();
            groupBox2 = new GroupBox();
            button7 = new Button();
            textBox16 = new TextBox();
            textBox13 = new TextBox();
            textBox15 = new TextBox();
            textBox12 = new TextBox();
            textBox14 = new TextBox();
            textBox11 = new TextBox();
            label17 = new Label();
            label20 = new Label();
            label16 = new Label();
            label19 = new Label();
            label15 = new Label();
            label18 = new Label();
            label10 = new Label();
            button6 = new Button();
            button5 = new Button();
            groupBox1 = new GroupBox();
            textBox8 = new TextBox();
            textBox7 = new TextBox();
            textBox10 = new TextBox();
            textBox9 = new TextBox();
            label13 = new Label();
            label9 = new Label();
            label11 = new Label();
            label12 = new Label();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            menuStrip1 = new MenuStrip();
            nguyênLiệuToolStripMenuItem = new ToolStripMenuItem();
            sảnPhẩmToolStripMenuItem = new ToolStripMenuItem();
            tabPage4 = new TabPage();
            dgvRevenue = new DataGridView();
            btnLognout = new Button();
            fontDialog1 = new FontDialog();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOverview).BeginInit();
            tabPage2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventory).BeginInit();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            menuStrip1.SuspendLayout();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRevenue).BeginInit();
            SuspendLayout();
            // 
            // btnCreateAccount
            // 
            btnCreateAccount.BackColor = Color.FromArgb(46, 125, 50);
            btnCreateAccount.ForeColor = Color.White;
            btnCreateAccount.Location = new Point(340, 385);
            btnCreateAccount.Name = "btnCreateAccount";
            btnCreateAccount.Size = new Size(150, 40);
            btnCreateAccount.TabIndex = 2;
            btnCreateAccount.Text = "Tạo tài khoản";
            btnCreateAccount.UseVisualStyleBackColor = false;
            btnCreateAccount.Click += btnCreateAccount_Click;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(211, 47, 47);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(660, 385);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(120, 40);
            btnExit.TabIndex = 3;
            btnExit.Text = "Thoát";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // btnDeleteAccount
            // 
            btnDeleteAccount.BackColor = Color.FromArgb(244, 67, 54);
            btnDeleteAccount.ForeColor = Color.White;
            btnDeleteAccount.Location = new Point(496, 385);
            btnDeleteAccount.Name = "btnDeleteAccount";
            btnDeleteAccount.Size = new Size(150, 40);
            btnDeleteAccount.TabIndex = 4;
            btnDeleteAccount.Text = "Xóa tài khoản";
            btnDeleteAccount.UseVisualStyleBackColor = false;
            btnDeleteAccount.Click += btnDeleteAccount_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Dock = DockStyle.Top;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 370);
            tabControl1.TabIndex = 4;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvOverview);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(792, 337);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Tổng quát";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvOverview
            // 
            dgvOverview.AllowUserToAddRows = false;
            dgvOverview.AllowUserToDeleteRows = false;
            dgvOverview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOverview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOverview.Dock = DockStyle.Fill;
            dgvOverview.Location = new Point(3, 3);
            dgvOverview.Name = "dgvOverview";
            dgvOverview.ReadOnly = true;
            dgvOverview.RowHeadersWidth = 51;
            dgvOverview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOverview.Size = new Size(786, 331);
            dgvOverview.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(panel1);
            tabPage2.Controls.Add(dataGridView1);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 337);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Quản Lý Nhân Viên";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(textBoxPassword);
            panel1.Controls.Add(textBox6);
            panel1.Controls.Add(textBox5);
            panel1.Controls.Add(textBox4);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label14);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 173);
            panel1.Name = "panel1";
            panel1.Size = new Size(786, 161);
            panel1.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackColor = Color.Cyan;
            button1.ForeColor = Color.Black;
            button1.Location = new Point(673, 118);
            button1.Name = "button1";
            button1.Size = new Size(108, 41);
            button1.TabIndex = 1;
            button1.Text = "Đổi mật khẩu";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(411, 131);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.ReadOnly = true;
            textBoxPassword.Size = new Size(137, 27);
            textBoxPassword.TabIndex = 7;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(413, 89);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(137, 27);
            textBox6.TabIndex = 7;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(633, 47);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(148, 27);
            textBox5.TabIndex = 7;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(411, 47);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(137, 27);
            textBox4.TabIndex = 7;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(114, 132);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(178, 27);
            textBox3.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(114, 92);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(178, 27);
            textBox2.TabIndex = 7;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(114, 50);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(178, 27);
            textBox1.TabIndex = 7;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label7.Location = new Point(333, 50);
            label7.Name = "label7";
            label7.Size = new Size(72, 21);
            label7.TabIndex = 4;
            label7.Text = "Chức vụ:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label6.Location = new Point(566, 50);
            label6.Name = "label6";
            label6.Size = new Size(61, 21);
            label6.TabIndex = 5;
            label6.Text = "Vai trò:";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label14.Location = new Point(323, 135);
            label14.Name = "label14";
            label14.Size = new Size(82, 21);
            label14.TabIndex = 5;
            label14.Text = "Mật khẩu:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label5.Location = new Point(298, 92);
            label5.Name = "label5";
            label5.Size = new Size(109, 21);
            label5.TabIndex = 5;
            label5.Text = "Tên tài khoản:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label8.Location = new Point(44, 95);
            label8.Name = "label8";
            label8.Size = new Size(64, 21);
            label8.TabIndex = 3;
            label8.Text = "Họ Tên:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label4.Location = new Point(5, 50);
            label4.Name = "label4";
            label4.Size = new Size(111, 21);
            label4.TabIndex = 3;
            label4.Text = "Mã nhân viên:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label3.Location = new Point(3, 137);
            label3.Name = "label3";
            label3.Size = new Size(108, 21);
            label3.TabIndex = 2;
            label3.Text = "Số điện thoại:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label2.Location = new Point(40, 50);
            label2.Name = "label2";
            label2.Size = new Size(63, 21);
            label2.TabIndex = 1;
            label2.Text = "Họ tên:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(250, 10);
            label1.Name = "label1";
            label1.Size = new Size(203, 28);
            label1.TabIndex = 0;
            label1.Text = "Thông tin nhân viên";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(786, 167);
            dataGridView1.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(dgvInventory);
            tabPage3.Controls.Add(groupBox2);
            tabPage3.Controls.Add(groupBox1);
            tabPage3.Controls.Add(menuStrip1);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(792, 337);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Quản Lý Kho";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvInventory
            // 
            dgvInventory.AllowUserToAddRows = false;
            dgvInventory.AllowUserToDeleteRows = false;
            dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInventory.Dock = DockStyle.Top;
            dgvInventory.Location = new Point(3, 31);
            dgvInventory.Name = "dgvInventory";
            dgvInventory.ReadOnly = true;
            dgvInventory.RowHeadersWidth = 51;
            dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventory.Size = new Size(786, 157);
            dgvInventory.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button7);
            groupBox2.Controls.Add(textBox16);
            groupBox2.Controls.Add(textBox13);
            groupBox2.Controls.Add(textBox15);
            groupBox2.Controls.Add(textBox12);
            groupBox2.Controls.Add(textBox14);
            groupBox2.Controls.Add(textBox11);
            groupBox2.Controls.Add(label17);
            groupBox2.Controls.Add(label20);
            groupBox2.Controls.Add(label16);
            groupBox2.Controls.Add(label19);
            groupBox2.Controls.Add(label15);
            groupBox2.Controls.Add(label18);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(button5);
            groupBox2.Dock = DockStyle.Bottom;
            groupBox2.Location = new Point(3, 194);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(786, 140);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Thông Tin Sản Phẩm";
            groupBox2.Visible = false;
            groupBox2.Enter += groupBox2_Enter;
            // 
            // button7
            // 
            button7.BackColor = Color.Cyan;
            button7.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            button7.Location = new Point(565, 113);
            button7.Name = "button7";
            button7.Size = new Size(66, 29);
            button7.TabIndex = 3;
            button7.Text = "Thêm";
            button7.UseVisualStyleBackColor = false;
            // 
            // textBox16
            // 
            textBox16.Location = new Point(229, 88);
            textBox16.Name = "textBox16";
            textBox16.ReadOnly = true;
            textBox16.Size = new Size(100, 27);
            textBox16.TabIndex = 2;
            // 
            // textBox13
            // 
            textBox13.Location = new Point(77, 88);
            textBox13.Name = "textBox13";
            textBox13.ReadOnly = true;
            textBox13.Size = new Size(80, 27);
            textBox13.TabIndex = 2;
            // 
            // textBox15
            // 
            textBox15.Location = new Point(677, 22);
            textBox15.Name = "textBox15";
            textBox15.ReadOnly = true;
            textBox15.Size = new Size(100, 27);
            textBox15.TabIndex = 2;
            // 
            // textBox12
            // 
            textBox12.Location = new Point(437, 22);
            textBox12.Name = "textBox12";
            textBox12.ReadOnly = true;
            textBox12.Size = new Size(122, 27);
            textBox12.TabIndex = 2;
            // 
            // textBox14
            // 
            textBox14.Location = new Point(196, 22);
            textBox14.Name = "textBox14";
            textBox14.ReadOnly = true;
            textBox14.Size = new Size(165, 27);
            textBox14.TabIndex = 2;
            // 
            // textBox11
            // 
            textBox11.Location = new Point(54, 22);
            textBox11.Name = "textBox11";
            textBox11.ReadOnly = true;
            textBox11.Size = new Size(77, 27);
            textBox11.TabIndex = 2;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label17.Location = new Point(401, 91);
            label17.Name = "label17";
            label17.Size = new Size(82, 20);
            label17.TabIndex = 1;
            label17.Text = "Trạng thái:";
            label17.Click += label17_Click;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label20.Location = new Point(169, 91);
            label20.Name = "label20";
            label20.Size = new Size(58, 20);
            label20.TabIndex = 1;
            label20.Text = "Đơn vị:";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label16.Location = new Point(6, 91);
            label16.Name = "label16";
            label16.Size = new Size(41, 20);
            label16.TabIndex = 1;
            label16.Text = "Loại:";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label19.Location = new Point(583, 25);
            label19.Name = "label19";
            label19.Size = new Size(67, 20);
            label19.TabIndex = 1;
            label19.Text = "Đơn giá:";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label15.Location = new Point(396, 25);
            label15.Name = "label15";
            label15.Size = new Size(35, 20);
            label15.TabIndex = 1;
            label15.Text = "Giá:";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label18.Location = new Point(155, 25);
            label18.Name = "label18";
            label18.Size = new Size(37, 20);
            label18.TabIndex = 1;
            label18.Text = "Tên:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label10.Location = new Point(6, 25);
            label10.Name = "label10";
            label10.Size = new Size(35, 20);
            label10.TabIndex = 1;
            label10.Text = "Mã:";
            // 
            // button6
            // 
            button6.BackColor = Color.Cyan;
            button6.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            button6.Location = new Point(637, 113);
            button6.Name = "button6";
            button6.Size = new Size(66, 29);
            button6.TabIndex = 0;
            button6.Text = "Sửa";
            button6.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            button5.BackColor = Color.Red;
            button5.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            button5.Location = new Point(709, 113);
            button5.Name = "button5";
            button5.Size = new Size(68, 29);
            button5.TabIndex = 0;
            button5.Text = "Xóa";
            button5.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox8);
            groupBox1.Controls.Add(textBox7);
            groupBox1.Controls.Add(textBox10);
            groupBox1.Controls.Add(textBox9);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button2);
            groupBox1.Location = new Point(0, 194);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(786, 140);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông Tin Nguyên Liệu";
            groupBox1.Visible = false;
            // 
            // textBox8
            // 
            textBox8.Location = new Point(327, 23);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(276, 27);
            textBox8.TabIndex = 1;
            // 
            // textBox7
            // 
            textBox7.Location = new Point(327, 74);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(182, 27);
            textBox7.TabIndex = 1;
            // 
            // textBox10
            // 
            textBox10.Location = new Point(93, 73);
            textBox10.Name = "textBox10";
            textBox10.Size = new Size(125, 27);
            textBox10.TabIndex = 1;
            // 
            // textBox9
            // 
            textBox9.Location = new Point(93, 26);
            textBox9.Name = "textBox9";
            textBox9.Size = new Size(125, 27);
            textBox9.TabIndex = 1;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label13.Location = new Point(0, 74);
            label13.Name = "label13";
            label13.Size = new Size(87, 23);
            label13.TabIndex = 0;
            label13.Text = "Số Lượng:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label9.Location = new Point(277, 27);
            label9.Name = "label9";
            label9.Size = new Size(40, 23);
            label9.TabIndex = 0;
            label9.Text = "Tên:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label11.Location = new Point(5, 27);
            label11.Name = "label11";
            label11.Size = new Size(39, 23);
            label11.TabIndex = 0;
            label11.Text = "Mã:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label12.Location = new Point(256, 77);
            label12.Name = "label12";
            label12.Size = new Size(64, 23);
            label12.TabIndex = 0;
            label12.Text = "Đơn vị:";
            // 
            // button4
            // 
            button4.BackColor = Color.Cyan;
            button4.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 163);
            button4.Location = new Point(617, 108);
            button4.Name = "button4";
            button4.Size = new Size(71, 29);
            button4.TabIndex = 2;
            button4.Text = "Thêm";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Red;
            button3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 163);
            button3.Location = new Point(701, 108);
            button3.Name = "button3";
            button3.Size = new Size(79, 33);
            button3.TabIndex = 2;
            button3.Text = "Xóa";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Cyan;
            button2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 163);
            button2.Location = new Point(504, 108);
            button2.Name = "button2";
            button2.Size = new Size(89, 29);
            button2.TabIndex = 2;
            button2.Text = "Cập nhật";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { nguyênLiệuToolStripMenuItem, sảnPhẩmToolStripMenuItem });
            menuStrip1.Location = new Point(3, 3);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(786, 28);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // nguyênLiệuToolStripMenuItem
            // 
            nguyênLiệuToolStripMenuItem.Name = "nguyênLiệuToolStripMenuItem";
            nguyênLiệuToolStripMenuItem.Size = new Size(102, 24);
            nguyênLiệuToolStripMenuItem.Text = "Nguyên liệu";
            // 
            // sảnPhẩmToolStripMenuItem
            // 
            sảnPhẩmToolStripMenuItem.Name = "sảnPhẩmToolStripMenuItem";
            sảnPhẩmToolStripMenuItem.Size = new Size(89, 24);
            sảnPhẩmToolStripMenuItem.Text = "Sản phẩm";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(dgvRevenue);
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(792, 337);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Doanh thu";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvRevenue
            // 
            dgvRevenue.AllowUserToAddRows = false;
            dgvRevenue.AllowUserToDeleteRows = false;
            dgvRevenue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRevenue.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRevenue.Dock = DockStyle.Fill;
            dgvRevenue.Location = new Point(3, 3);
            dgvRevenue.Name = "dgvRevenue";
            dgvRevenue.ReadOnly = true;
            dgvRevenue.RowHeadersWidth = 51;
            dgvRevenue.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRevenue.Size = new Size(786, 331);
            dgvRevenue.TabIndex = 0;
            // 
            // btnLognout
            // 
            btnLognout.BackColor = Color.Gray;
            btnLognout.Location = new Point(23, 391);
            btnLognout.Name = "btnLognout";
            btnLognout.Size = new Size(130, 34);
            btnLognout.TabIndex = 5;
            btnLognout.Text = "Đăng xuất";
            btnLognout.UseVisualStyleBackColor = false;
            btnLognout.Click += btnLogout_Click;
            // 
            // Admin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnLognout);
            Controls.Add(btnDeleteAccount);
            Controls.Add(btnExit);
            Controls.Add(btnCreateAccount);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "Admin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ADMIN";
            Load += Admin_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOverview).EndInit();
            tabPage2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventory).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRevenue).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button btnCreateAccount;
        private Button btnDeleteAccount;
        private Button btnExit;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private DataGridView dgvOverview;
        private DataGridView dataGridView1;
        private DataGridView dgvRevenue;
        private Panel panel1;
        private Label label1;
    private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox textBox1;
  private TextBox textBox2;
    private TextBox textBox3;
        private TextBox textBox4;
   private TextBox textBox5;
        private TextBox textBox6;
  private Button button1;
        private Label label14;
        private TextBox textBoxPassword;
        private Button btnLognout;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem nguyênLiệuToolStripMenuItem;
        private ToolStripMenuItem sảnPhẩmToolStripMenuItem;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private Button button3;
        private Button button4;
        private Button button2;
        private TextBox textBox8;
        private Label label13;
        private TextBox textBox10;
        private Label label9;
        private TextBox textBox7;
        private Label label11;
        private TextBox textBox9;
        private Label label12;
        private FontDialog fontDialog1;
        private DataGridView dgvInventory;
        private Button button5;
        private TextBox textBox16;
        private TextBox textBox13;
        private TextBox textBox15;
        private TextBox textBox12;
        private TextBox textBox14;
        private TextBox textBox11;
        private Label label17;
        private Label label20;
        private Label label16;
        private Label label19;
        private Label label15;
        private Label label18;
        private Label label10;
        private Button button6;
        private Button button7;
    }
}