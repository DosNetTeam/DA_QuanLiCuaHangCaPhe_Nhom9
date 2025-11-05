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
       lblPhoneNumber = new Label();
            txtPhoneNumber = new TextBox();
            lblRole = new Label();
            rbEmployee = new RadioButton();
            rbManager = new RadioButton();
            btnSave = new Button();
      btnCancel = new Button();
       panelRole = new Panel();
     panelRole.SuspendLayout();
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
     lblTitle.Text = "TẠO TÀI KHOẢN MỚI";
            // 
     // lblFullName
      // 
       lblFullName.AutoSize = true;
 lblFullName.Location = new Point(30, 80);
            lblFullName.Name = "lblFullName";
  lblFullName.Size = new Size(57, 20);
            lblFullName.TabIndex = 0;
            lblFullName.Text = "Họ tên:";
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
            lblUsername.Size = new Size(110, 20);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "Tên đăng nhập:";
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
            lblPassword.Text = "Mật khẩu:";
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
          lblConfirmPassword.Text = "Xác nhận mật khẩu:";
            // 
       // txtConfirmPassword
     // 
        txtConfirmPassword.Location = new Point(180, 212);
            txtConfirmPassword.Name = "txtConfirmPassword";
txtConfirmPassword.Size = new Size(280, 27);
 txtConfirmPassword.TabIndex = 3;
     txtConfirmPassword.UseSystemPasswordChar = true;
            // 
          // lblPhoneNumber
 // 
      lblPhoneNumber.AutoSize = true;
    lblPhoneNumber.Location = new Point(30, 260);
            lblPhoneNumber.Name = "lblPhoneNumber";
      lblPhoneNumber.Size = new Size(100, 20);
            lblPhoneNumber.TabIndex = 0;
            lblPhoneNumber.Text = "Số điện thoại:";
       // 
            // txtPhoneNumber
  // 
            txtPhoneNumber.Location = new Point(180, 257);
     txtPhoneNumber.Name = "txtPhoneNumber";
       txtPhoneNumber.Size = new Size(280, 27);
    txtPhoneNumber.TabIndex = 4;
      txtPhoneNumber.PlaceholderText = "VD: 0912345678";
     // 
   // lblRole
  // 
            lblRole.AutoSize = true;
            lblRole.Location = new Point(30, 305);
 lblRole.Name = "lblRole";
       lblRole.Size = new Size(57, 20);
            lblRole.TabIndex = 0;
         lblRole.Text = "Vai trò:";
            // 
      // panelRole
     // 
            panelRole.Controls.Add(rbEmployee);
  panelRole.Controls.Add(rbManager);
     panelRole.Location = new Point(180, 300);
   panelRole.Name = "panelRole";
            panelRole.Size = new Size(280, 35);
      panelRole.TabIndex = 5;
// 
     // rbEmployee
            // 
        rbEmployee.AutoSize = true;
            rbEmployee.Checked = true;
            rbEmployee.Location = new Point(3, 5);
       rbEmployee.Name = "rbEmployee";
     rbEmployee.Size = new Size(97, 24);
      rbEmployee.TabIndex = 0;
   rbEmployee.TabStop = true;
    rbEmployee.Text = "Nhân viên";
            rbEmployee.UseVisualStyleBackColor = true;
    rbEmployee.CheckedChanged += rbEmployee_CheckedChanged;
   // 
    // rbManager
            // 
rbManager.AutoSize = true;
            rbManager.Location = new Point(150, 5);
 rbManager.Name = "rbManager";
       rbManager.Size = new Size(81, 24);
        rbManager.TabIndex = 1;
            rbManager.Text = "Quản lý";
            rbManager.UseVisualStyleBackColor = true;
     rbManager.CheckedChanged += rbManager_CheckedChanged;
            // 
      // btnSave
       // 
            btnSave.BackColor = Color.FromArgb(46, 125, 50);
    btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(163, 360);
            btnSave.Name = "btnSave";
         btnSave.Size = new Size(130, 40);
       btnSave.TabIndex = 6;
       btnSave.Text = "Lưu";
        btnSave.UseVisualStyleBackColor = false;
    btnSave.Click += btnSave_Click;
      // 
            // btnCancel
        // 
            btnCancel.BackColor = Color.FromArgb(158, 158, 158);
      btnCancel.ForeColor = Color.White;
      btnCancel.Location = new Point(319, 360);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(130, 40);
  btnCancel.TabIndex = 7;
            btnCancel.Text = "Hủy";
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
            Controls.Add(panelRole);
    Controls.Add(lblRole);
         Controls.Add(txtPhoneNumber);
      Controls.Add(lblPhoneNumber);
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
         Text = "Tạo tài khoản";
            panelRole.ResumeLayout(false);
     panelRole.PerformLayout();
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
    private Label lblPhoneNumber;
        private TextBox txtPhoneNumber;
        private Label lblRole;
        private Panel panelRole;
   private RadioButton rbEmployee;
        private RadioButton rbManager;
        private Button btnSave;
        private Button btnCancel;
    }
}
