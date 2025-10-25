namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
  partial class CreateEmployeeForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
   if (disposing && (components != null))
     {
components.Dispose();
        }
        base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblFullName = new Label();
            txtFullName = new TextBox();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            lblPosition = new Label();
            txtPosition = new TextBox();
            chkIsEmployee = new CheckBox();
         chkIsManager = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(33, 150, 243);
            lblTitle.Location = new Point(120, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(260, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "T?O TÀI KHO?N M?I";
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(30, 80);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(56, 20);
            lblFullName.TabIndex = 0;
            lblFullName.Text = "H? tên:";
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(180, 77);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(280, 27);
            txtFullName.TabIndex = 0;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(30, 125);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(109, 20);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "Tên ðãng nh?p:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(180, 122);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(280, 27);
            txtUsername.TabIndex = 1;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(30, 170);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(73, 20);
            lblPassword.TabIndex = 0;
            lblPassword.Text = "M?t kh?u:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(180, 167);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(280, 27);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Location = new Point(30, 215);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(137, 20);
            lblConfirmPassword.TabIndex = 0;
            lblConfirmPassword.Text = "Xác nh?n m?t kh?u:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(180, 212);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(280, 27);
            txtConfirmPassword.TabIndex = 3;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // lblPosition
            // 
            lblPosition.AutoSize = true;
            lblPosition.Location = new Point(30, 260);
            lblPosition.Name = "lblPosition";
            lblPosition.Size = new Size(63, 20);
            lblPosition.TabIndex = 0;
            lblPosition.Text = "Ch?c v?:";
            // 
            // txtPosition
            // 
            txtPosition.Location = new Point(180, 257);
            txtPosition.Name = "txtPosition";
            txtPosition.Size = new Size(280, 27);
            txtPosition.TabIndex = 4;
            // 
            // chkIsEmployee
            // 
            chkIsEmployee.AutoSize = true;
            chkIsEmployee.Location = new Point(180, 305);
            chkIsEmployee.Name = "chkIsEmployee";
            chkIsEmployee.Size = new Size(103, 24);
            chkIsEmployee.TabIndex = 5;
            chkIsEmployee.Text = "Nhân viên";
            chkIsEmployee.UseVisualStyleBackColor = true;
            chkIsEmployee.CheckedChanged += chkIsEmployee_CheckedChanged;
            // 
            // chkIsManager
            // 
            chkIsManager.AutoSize = true;
            chkIsManager.Location = new Point(300, 305);
            chkIsManager.Name = "chkIsManager";
            chkIsManager.Size = new Size(84, 24);
            chkIsManager.TabIndex = 6;
            chkIsManager.Text = "Qu?n l?";
            chkIsManager.UseVisualStyleBackColor = true;
            chkIsManager.CheckedChanged += chkIsManager_CheckedChanged;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(46, 125, 50);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(180, 350);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 40);
            btnSave.TabIndex = 7;
            btnSave.Text = "Lýu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(158, 158, 158);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(330, 350);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(130, 40);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "H?y";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // CreateEmployeeForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 420);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(chkIsManager);
            Controls.Add(chkIsEmployee);
            Controls.Add(txtPosition);
            Controls.Add(lblPosition);
            Controls.Add(txtConfirmPassword);
            Controls.Add(lblConfirmPassword);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblUsername);
            Controls.Add(txtFullName);
            Controls.Add(lblFullName);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CreateEmployeeForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "T?o tài kho?n";
            ResumeLayout(false);
        PerformLayout();
        }

  private Label lblTitle;
        private Label lblFullName;
        private TextBox txtFullName;
     private Label lblUsername;
 private TextBox txtUsername;
     private Label lblPassword;
        private TextBox txtPassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private Label lblPosition;
        private TextBox txtPosition;
        private CheckBox chkIsEmployee;
        private CheckBox chkIsManager;
  private Button btnSave;
        private Button btnCancel;
    }
}
