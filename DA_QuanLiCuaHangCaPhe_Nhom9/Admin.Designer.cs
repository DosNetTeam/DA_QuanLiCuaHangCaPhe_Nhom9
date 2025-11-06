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
        private void InitializeComponent()
        {
            btnCreateAccount = new Button();
            btnExit = new Button();
            btnDeleteAccount = new Button();
            btnLogout = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            dgvOverview = new DataGridView();
            tabPage2 = new TabPage();
            dataGridView1 = new DataGridView();
            panel1 = new Panel();
            button1 = new Button();
            textBox3 = new TextBox();
            textBox6 = new TextBox();
            textBox5 = new TextBox();
            textBox4 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            tabPage3 = new TabPage();
            dgvInventory = new DataGridView();
            tabPage4 = new TabPage();
            dgvRevenue = new DataGridView();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOverview).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventory).BeginInit();
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
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(96, 125, 139);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(12, 385);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(150, 40);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "Đăng xuất";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
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
            tabPage2.Controls.Add(dataGridView1);
            tabPage2.Controls.Add(panel1);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 337);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Quản Lý Nhân Viên";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(-4, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(800, 167);
            dataGridView1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(textBox6);
            panel1.Controls.Add(textBox5);
            panel1.Controls.Add(textBox4);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(786, 331);
            panel1.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackColor = Color.Cyan;
            button1.ForeColor = Color.Black;
            button1.Location = new Point(673, 285);
            button1.Name = "button1";
            button1.Size = new Size(108, 41);
            button1.TabIndex = 1;
            button1.Text = "Đổi mật khẩu";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnLogout_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(111, 299);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(178, 27);
            textBox3.TabIndex = 7;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(411, 254);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(137, 27);
            textBox6.TabIndex = 7;
            textBox6.TextChanged += textBox5_TextChanged;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(633, 206);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(148, 27);
            textBox5.TabIndex = 7;
            textBox5.TextChanged += textBox5_TextChanged;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(411, 209);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(137, 27);
            textBox4.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(111, 254);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(178, 27);
            textBox2.TabIndex = 7;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(111, 209);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(178, 27);
            textBox1.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label5.Location = new Point(296, 257);
            label5.Name = "label5";
            label5.Size = new Size(109, 21);
            label5.TabIndex = 5;
            label5.Text = "Tên tài khoản:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label6.Location = new Point(566, 212);
            label6.Name = "label6";
            label6.Size = new Size(61, 21);
            label6.TabIndex = 5;
            label6.Text = "Vai trò:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label7.Location = new Point(333, 212);
            label7.Name = "label7";
            label7.Size = new Size(72, 21);
            label7.TabIndex = 4;
            label7.Text = "Chức vụ:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Calibri", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label4.Location = new Point(5, 212);
            label4.Name = "label4";
            label4.Size = new Size(111, 21);
            label4.TabIndex = 3;
            label4.Text = "Mã nhân viên:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label3.Location = new Point(3, 302);
            label3.Name = "label3";
            label3.Size = new Size(108, 21);
            label3.TabIndex = 2;
            label3.Text = "Số điện thoại:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 10.2F, FontStyle.Bold);
            label2.Location = new Point(40, 257);
            label2.Name = "label2";
            label2.Size = new Size(63, 21);
            label2.TabIndex = 1;
            label2.Text = "Họ tên:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Emoji", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(250, 167);
            label1.Name = "label1";
            label1.Size = new Size(204, 27);
            label1.TabIndex = 0;
            label1.Text = "Thông tin nhân viên";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(dgvInventory);
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
            dgvInventory.Dock = DockStyle.Fill;
            dgvInventory.Location = new Point(3, 3);
            dgvInventory.Name = "dgvInventory";
            dgvInventory.ReadOnly = true;
            dgvInventory.RowHeadersWidth = 51;
            dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventory.Size = new Size(786, 331);
            dgvInventory.TabIndex = 0;
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
            // Admin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnLogout);
            Controls.Add(btnDeleteAccount);
            Controls.Add(btnExit);
            Controls.Add(btnCreateAccount);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Admin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ADMIN";
            Load += Admin_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOverview).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvInventory).EndInit();
            tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRevenue).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button btnCreateAccount;
        private Button btnDeleteAccount;
        private Button btnLogout;
        private Button btnExit;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private DataGridView dgvOverview;
        private DataGridView dgvInventory;
        private DataGridView dgvRevenue;
        private Panel panel1;
        private DataGridView dataGridView1;
        private Button button1;
        private TextBox textBox3;
        private TextBox textBox5;
        private TextBox textBox4;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label label6;
        private Label label7;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox textBox6;
        private Label label5;
    }
}