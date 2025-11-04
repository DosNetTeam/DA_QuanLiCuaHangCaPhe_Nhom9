using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;

namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    public partial class Admin : Form
    {
        // Employee data class
     public class Employee
        {
        public int Id { get; set; }
         public string? FullName { get; set; }
        public string? Username { get; set; }
          public string? Position { get; set; }
      public bool IsEmployee { get; set; }
    public bool IsManager { get; set; }
    }

        private BindingList<Employee> employees;
        private int nextId = 1;
   private readonly string dataFilePath = "employees.json";

        public Admin()
      {
      InitializeComponent();
          
     // Set form centered
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize empty employee list
     employees = new BindingList<Employee>();

       // Initialize DataGridView
      InitializeDataGridView();
  
            // Load data from file
            LoadEmployeesFromFile();
          
  // Bind list
  dgvEmployees.DataSource = employees;
     }

        private void InitializeDataGridView()
{
          // Clear existing columns
    dgvEmployees.Columns.Clear();

            // Add columns
         dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
 {
      Name = "Id",
  HeaderText = "ID",
    DataPropertyName = "Id",
      Width = 50
     });

            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
    Name = "FullName",
    HeaderText = "Họ và tên",
    DataPropertyName = "FullName",
     AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
  });

        dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
            {
  Name = "Username",
          HeaderText = "Tên đăng nhập",
                DataPropertyName = "Username",
        Width = 120
 });

            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
      {
          Name = "Position",
       HeaderText = "Chức vụ",
           DataPropertyName = "Position",
    Width = 100
            });

            dgvEmployees.Columns.Add(new DataGridViewCheckBoxColumn
       {
        Name = "IsEmployee",
    HeaderText = "NV",
        DataPropertyName = "IsEmployee",
   Width = 50
          });

     dgvEmployees.Columns.Add(new DataGridViewCheckBoxColumn
        {
  Name = "IsManager",
      HeaderText = "QL",
   DataPropertyName = "IsManager",
   Width = 50
         });

    // Style the header
    dgvEmployees.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 150, 243);
dgvEmployees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
  dgvEmployees.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
     dgvEmployees.EnableHeadersVisualStyles = false;

   // Alternate row colors
      dgvEmployees.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

   private void LoadEmployeesFromFile()
        {
            try
  {
    if (File.Exists(dataFilePath))
         {
        string jsonString = File.ReadAllText(dataFilePath);
           var loadedEmployees = JsonSerializer.Deserialize<List<Employee>>(jsonString);
           
if (loadedEmployees != null && loadedEmployees.Count > 0)
          {
       foreach (var emp in loadedEmployees)
            {
 employees.Add(emp);
      }
     
   // Update nextId to be higher than the highest existing ID
        nextId = employees.Max(e => e.Id) + 1;
  }
        }
      }
          catch (Exception ex)
            {
     MessageBox.Show(
   $"Lỗi khi tải dữ liệu: {ex.Message}",
                    "Lỗi",
        MessageBoxButtons.OK,
 MessageBoxIcon.Error);
            }
        }

        private void SaveEmployeesToFile()
        {
  try
    {
    var options = new JsonSerializerOptions
{
            WriteIndented = true,
     Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
         
                string jsonString = JsonSerializer.Serialize(employees.ToList(), options);
       File.WriteAllText(dataFilePath, jsonString);
   }
      catch (Exception ex)
    {
    MessageBox.Show(
   $"Lỗi khi lưu dữ liệu: {ex.Message}",
     "Lỗi",
             MessageBoxButtons.OK,
           MessageBoxIcon.Error);
  }
   }

   private void dgvEmployees_SelectionChanged(object? sender, EventArgs e)
    {
       // When user selects an employee, update button states
      UpdatePermissionButtons();
  }

        private void UpdatePermissionButtons()
   {
      // Reset all buttons to default state
            ResetButtonStates();

    if (dgvEmployees.SelectedRows.Count > 0)
            {
            var selectedRow = dgvEmployees.SelectedRows[0];
      if (selectedRow.DataBoundItem is Employee employee)
                {
  // Highlight active permissions based on employee role
   if (employee.IsEmployee)
     {
      btnEmployee.BackColor = Color.FromArgb(96, 125, 139);
}
               
        if (employee.IsManager)
         {
                 // Manager has all permissions
         btnManager.BackColor = Color.FromArgb(156, 39, 176);
        btnManageProducts.BackColor = Color.FromArgb(33, 150, 243);
      btnViewReports.BackColor = Color.FromArgb(255, 152, 0);
              btnEditAccount.BackColor = Color.FromArgb(76, 175, 80);
     btnDeleteAccount.BackColor = Color.FromArgb(244, 67, 54);
         }
      }
     }
 }

        private void ResetButtonStates()
  {
            // Set all buttons to inactive (lighter) colors
            btnEmployee.BackColor = Color.LightGray;
 btnManager.BackColor = Color.LightGray;
            btnManageProducts.BackColor = Color.LightGray;
            btnViewReports.BackColor = Color.LightGray;
   btnEditAccount.BackColor = Color.LightGray;
            btnDeleteAccount.BackColor = Color.LightGray;
        }

private void btnCreateAccount_Click(object sender, EventArgs e)
      {
      // Open create employee dialog
CreateEmployeeForm createForm = new CreateEmployeeForm();
            
     if (createForm.ShowDialog() == DialogResult.OK)
            {
                // Add new employee to list
 var newEmployee = new Employee
 {
         Id = nextId++,
FullName = createForm.FullName,
      Username = createForm.Username,
      Position = createForm.Position,
  IsEmployee = createForm.IsEmployee,
             IsManager = createForm.IsManager
                };

      employees.Add(newEmployee);
            
     // Save to file immediately
  SaveEmployeesToFile();

      string roleInfo = "";
     if (newEmployee.IsManager) roleInfo = "Quản lý";
     else if (newEmployee.IsEmployee) roleInfo = "Nhân viên";

           MessageBox.Show(
     $"Đã tạo tài khoản thành công!\n\nHọ tên: {newEmployee.FullName}\nVai trò: {roleInfo}",
        "Thành công",
             MessageBoxButtons.OK,
          MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Bạn có chắc muốn thoát?",
    "Xác nhận",
   MessageBoxButtons.YesNo,
             MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
     {
  // Save before exiting
                SaveEmployeesToFile();
                Application.Exit();
      }
        }

        // Permission button handlers
        private void btnEmployee_Click(object sender, EventArgs e)
     {
    MessageBox.Show("Quyền: Nhân viên\n- Xem thông tin cơ bản\n- Thực hiện nghiệp vụ", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnManager_Click(object sender, EventArgs e)
   {
            MessageBox.Show("Quyền: Quản lý\n- Toàn quyền trên hệ thống\n- Quản lý tất cả chức năng", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Information);
   }

        private void btnManageProducts_Click(object sender, EventArgs e)
        {
    MessageBox.Show("Quyền: Quản lý sản phẩm\n- Thêm/Sửa/Xóa sản phẩm\n- Quản lý kho", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

    private void btnViewReports_Click(object sender, EventArgs e)
      {
            MessageBox.Show("Quyền: Xem báo cáo\n- Xem thống kê\n- Xem doanh thu", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Information);
 }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Quyền: Chỉnh sửa tài khoản\n- Sửa thông tin nhân viên\n- Đổi mật khẩu", "Phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
   {
       if (dgvEmployees.SelectedRows.Count > 0)
  {
            DialogResult result = MessageBox.Show(
    "Bạn có chắc muốn xóa tài khoản này?",
 "Xác nhận xóa",
             MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

 if (result == DialogResult.Yes)
           {
         var selectedRow = dgvEmployees.SelectedRows[0];
         if (selectedRow.DataBoundItem is Employee employee)
           {
     employees.Remove(employee);
     
      // Save to file immediately
     SaveEmployeesToFile();

       MessageBox.Show("Đã xóa tài khoản!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
          }
       }
else
 {
       MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
