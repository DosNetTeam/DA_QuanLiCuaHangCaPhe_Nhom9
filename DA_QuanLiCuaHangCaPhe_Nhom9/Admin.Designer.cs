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
            groupBoxEmployeeList = new GroupBox();
            dgvEmployees = new DataGridView();
            groupBoxPermissions = new GroupBox();
            btnEmployee = new Button();
            btnManageProducts = new Button();
            btnViewReports = new Button();
            btnEditAccount = new Button();
            btnDeleteAccount = new Button();
            btnManager = new Button();
            btnCreateAccount = new Button();
            btnExit = new Button();
            lblTitle = new Label();
            groupBoxEmployeeList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
            groupBoxPermissions.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxEmployeeList
            // 
            groupBoxEmployeeList.Controls.Add(dgvEmployees);
            groupBoxEmployeeList.Location = new Point(30, 70);
            groupBoxEmployeeList.Name = "groupBoxEmployeeList";
            groupBoxEmployeeList.Size = new Size(450, 300);
            groupBoxEmployeeList.TabIndex = 0;
            groupBoxEmployeeList.TabStop = false;
            groupBoxEmployeeList.Text = "Danh sách nhân viên";
            // 
            // dgvEmployees
            // 
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.AllowUserToDeleteRows = false;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.BackgroundColor = Color.White;
            dgvEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEmployees.Dock = DockStyle.Fill;
            dgvEmployees.Location = new Point(3, 23);
            dgvEmployees.MultiSelect = false;
            dgvEmployees.Name = "dgvEmployees";
            dgvEmployees.ReadOnly = true;
            dgvEmployees.RowHeadersWidth = 51;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.Size = new Size(444, 274);
            dgvEmployees.TabIndex = 0;
            dgvEmployees.SelectionChanged += dgvEmployees_SelectionChanged;
            // 
            // groupBoxPermissions
            // 
            groupBoxPermissions.Controls.Add(btnEmployee);
            groupBoxPermissions.Controls.Add(btnManageProducts);
            groupBoxPermissions.Controls.Add(btnViewReports);
            groupBoxPermissions.Controls.Add(btnEditAccount);
            groupBoxPermissions.Controls.Add(btnDeleteAccount);
            groupBoxPermissions.Controls.Add(btnManager);
            groupBoxPermissions.Location = new Point(500, 70);
            groupBoxPermissions.Name = "groupBoxPermissions";
            groupBoxPermissions.Size = new Size(280, 300);
            groupBoxPermissions.TabIndex = 1;
            groupBoxPermissions.TabStop = false;
            groupBoxPermissions.Text = "Phân quyền";
            // 
            // btnEmployee
            // 
            btnEmployee.BackColor = Color.FromArgb(96, 125, 139);
            btnEmployee.ForeColor = Color.White;
            btnEmployee.Location = new Point(20, 40);
            btnEmployee.Name = "btnEmployee";
            btnEmployee.Size = new Size(240, 35);
            btnEmployee.TabIndex = 0;
            btnEmployee.Text = "Nhân viên";
            btnEmployee.UseVisualStyleBackColor = false;
            btnEmployee.Click += btnEmployee_Click;
            // 
            // btnManager
            // 
            btnManager.BackColor = Color.FromArgb(156, 39, 176);
            btnManager.ForeColor = Color.White;
            btnManager.Location = new Point(20, 85);
            btnManager.Name = "btnManager";
            btnManager.Size = new Size(240, 35);
            btnManager.TabIndex = 1;
            btnManager.Text = "Quản lý";
            btnManager.UseVisualStyleBackColor = false;
            btnManager.Click += btnManager_Click;
            // 
            // btnManageProducts
            // 
            btnManageProducts.BackColor = Color.FromArgb(33, 150, 243);
            btnManageProducts.ForeColor = Color.White;
            btnManageProducts.Location = new Point(20, 130);
            btnManageProducts.Name = "btnManageProducts";
            btnManageProducts.Size = new Size(240, 35);
            btnManageProducts.TabIndex = 2;
            btnManageProducts.Text = "Quản lý sản phẩm";
            btnManageProducts.UseVisualStyleBackColor = false;
            btnManageProducts.Click += btnManageProducts_Click;
            // 
            // btnViewReports
            // 
            btnViewReports.BackColor = Color.FromArgb(255, 152, 0);
            btnViewReports.ForeColor = Color.White;
            btnViewReports.Location = new Point(20, 175);
            btnViewReports.Name = "btnViewReports";
            btnViewReports.Size = new Size(240, 35);
            btnViewReports.TabIndex = 3;
            btnViewReports.Text = "Xem báo cáo";
            btnViewReports.UseVisualStyleBackColor = false;
            btnViewReports.Click += btnViewReports_Click;
            // 
            // btnEditAccount
            // 
            btnEditAccount.BackColor = Color.FromArgb(76, 175, 80);
            btnEditAccount.ForeColor = Color.White;
            btnEditAccount.Location = new Point(20, 220);
            btnEditAccount.Name = "btnEditAccount";
            btnEditAccount.Size = new Size(240, 35);
            btnEditAccount.TabIndex = 4;
            btnEditAccount.Text = "Chỉnh sửa tài khoản";
            btnEditAccount.UseVisualStyleBackColor = false;
            btnEditAccount.Click += btnEditAccount_Click;
            // 
            // btnDeleteAccount
            // 
            btnDeleteAccount.BackColor = Color.FromArgb(244, 67, 54);
            btnDeleteAccount.ForeColor = Color.White;
            btnDeleteAccount.Location = new Point(20, 265);
            btnDeleteAccount.Name = "btnDeleteAccount";
            btnDeleteAccount.Size = new Size(115, 35);
            btnDeleteAccount.TabIndex = 5;
            btnDeleteAccount.Text = "Xóa tài khoản";
            btnDeleteAccount.UseVisualStyleBackColor = false;
            btnDeleteAccount.Click += btnDeleteAccount_Click;
            // 
            // btnCreateAccount
            // 
            btnCreateAccount.BackColor = Color.FromArgb(46, 125, 50);
            btnCreateAccount.ForeColor = Color.White;
            btnCreateAccount.Location = new Point(500, 385);
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
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(33, 150, 243);
            lblTitle.Location = new Point(310, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(140, 37);
            lblTitle.TabIndex = 4;
            lblTitle.Text = "ADMIN";
            // 
            // Admin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblTitle);
            Controls.Add(btnExit);
            Controls.Add(btnCreateAccount);
            Controls.Add(groupBoxPermissions);
            Controls.Add(groupBoxEmployeeList);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Admin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Admin";
            groupBoxEmployeeList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).EndInit();
            groupBoxPermissions.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBoxEmployeeList;
        private DataGridView dgvEmployees;
        private GroupBox groupBoxPermissions;
        private Button btnEmployee;
        private Button btnManager;
        private Button btnManageProducts;
        private Button btnViewReports;
        private Button btnEditAccount;
        private Button btnDeleteAccount;
        private Button btnCreateAccount;
        private Button btnExit;
        private Label lblTitle;
    }
}