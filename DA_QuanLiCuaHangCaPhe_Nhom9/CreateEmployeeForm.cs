using System;
using System.Windows.Forms;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    public partial class CreateEmployeeForm : Form
    {
   public string FullName { get; private set; } = string.Empty;
        public string Username { get; private set; } = string.Empty;
  public string Password { get; private set; } = string.Empty;
  public string Position { get; private set; } = string.Empty;
        public bool IsEmployee { get; private set; }
        public bool IsManager { get; private set; }

        public CreateEmployeeForm()
        {
      InitializeComponent();
  this.StartPosition = FormStartPosition.CenterParent;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
  // Validate
  if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
   MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", 
      MessageBoxButtons.OK, MessageBoxIcon.Warning);
       txtFullName.Focus();
       return;
            }

         if (string.IsNullOrWhiteSpace(txtUsername.Text))
       {
          MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", 
       MessageBoxButtons.OK, MessageBoxIcon.Warning);
txtUsername.Focus();
         return;
   }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
       MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", 
      MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPassword.Focus();
return;
         }

 if (txtPassword.Text.Length < 6)
     {
          MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo", 
       MessageBoxButtons.OK, MessageBoxIcon.Warning);
txtPassword.Focus();
      return;
 }

            if (txtPassword.Text != txtConfirmPassword.Text)
     {
   MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", 
     MessageBoxButtons.OK, MessageBoxIcon.Warning);
txtConfirmPassword.Focus();
       return;
            }

 if (string.IsNullOrWhiteSpace(txtPosition.Text))
  {
    MessageBox.Show("Vui lòng nhập chức vụ!", "Thông báo", 
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
       txtPosition.Focus();
        return;
   }

            // Check if at least one role is selected
          if (!chkIsEmployee.Checked && !chkIsManager.Checked)
   {
       MessageBox.Show("Vui lòng chọn ít nhất một vai trò (Nhân viên hoặc Quản lý)!", "Thông báo", 
     MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
            }

            // Save data
     FullName = txtFullName.Text.Trim();
      Username = txtUsername.Text.Trim();
      Password = txtPassword.Text;
   Position = txtPosition.Text.Trim();
IsEmployee = chkIsEmployee.Checked;
            IsManager = chkIsManager.Checked;

            this.DialogResult = DialogResult.OK;
  this.Close();
    }

        private void btnCancel_Click(object sender, EventArgs e)
  {
  this.DialogResult = DialogResult.Cancel;
this.Close();
        }

        private void chkIsEmployee_CheckedChanged(object? sender, EventArgs e)
        {
     // If Employee is checked and Manager is also checked, uncheck Manager
   // (Optional: You can allow both to be checked if needed)
            if (chkIsEmployee.Checked && chkIsManager.Checked)
     {
           // Option 1: Allow both (do nothing)
// Option 2: Only one at a time (uncomment below)
 // chkIsManager.Checked = false;
   }
     }

  private void chkIsManager_CheckedChanged(object? sender, EventArgs e)
        {
     // If Manager is checked, automatically check Employee
// (Manager has all employee permissions plus more)
 if (chkIsManager.Checked)
            {
     chkIsEmployee.Checked = true;
     chkIsEmployee.Enabled = false; // Disable because manager is always employee
   }
            else
     {
        chkIsEmployee.Enabled = true;
       }
        }
    }
}
