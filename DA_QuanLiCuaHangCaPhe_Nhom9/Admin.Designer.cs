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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            dgvOverview = new DataGridView();
            tabPage2 = new TabPage();
            dgvEmployees = new DataGridView();
            tabPage3 = new TabPage();
            dgvInventory = new DataGridView();
            tabPage4 = new TabPage();
            dgvRevenue = new DataGridView();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOverview).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
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
            // btnDeleteAccount
            // 
            btnDeleteAccount.BackColor = Color.FromArgb(244, 67, 54);
            btnDeleteAccount.ForeColor = Color.White;
            btnDeleteAccount.Location = new Point(500, 385);
            btnDeleteAccount.Name = "btnDeleteAccount";
            btnDeleteAccount.Size = new Size(150, 40);
            btnDeleteAccount.TabIndex = 4;
            btnDeleteAccount.Text = "Xóa tài khoản";
            btnDeleteAccount.UseVisualStyleBackColor = false;
            btnDeleteAccount.Click += btnDeleteAccount_Click;
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
            tabPage2.Controls.Add(dgvEmployees);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 337);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Quản Lý Nhân Viên";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvEmployees
            // 
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.AllowUserToDeleteRows = false;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEmployees.Dock = DockStyle.Fill;
            dgvEmployees.Location = new Point(3, 3);
            dgvEmployees.Name = "dgvEmployees";
            dgvEmployees.ReadOnly = true;
            dgvEmployees.RowHeadersWidth = 51;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.Size = new Size(786, 331);
            dgvEmployees.TabIndex = 0;
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
            Controls.Add(btnDeleteAccount);
            Controls.Add(btnExit);
            Controls.Add(btnCreateAccount);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Admin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản Lý - Coffee Shop";
            Load += Admin_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOverview).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).EndInit();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvInventory).EndInit();
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
        private DataGridView dgvEmployees;
        private DataGridView dgvInventory;
        private DataGridView dgvRevenue;
    }
}