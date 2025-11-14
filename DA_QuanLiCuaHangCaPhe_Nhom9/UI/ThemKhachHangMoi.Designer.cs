namespace DA_QuanLiCuaHangCaPhe_Nhom9 {
    partial class ThemKhachHangMoi {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTenKH;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.Label lblLoaiKH;
        private System.Windows.Forms.TextBox txtTenKH;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.ComboBox cbLoaiKH;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            lblTitle = new Label();
            lblTenKH = new Label();
            lblSDT = new Label();
            lblDiaChi = new Label();
            lblLoaiKH = new Label();
            txtTenKH = new TextBox();
            txtSDT = new TextBox();
            txtDiaChi = new TextBox();
            cbLoaiKH = new ComboBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.Location = new Point(16, 14);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(408, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Thêm khách hàng mới";
            // 
            // lblTenKH
            // 
            lblTenKH.AutoSize = true;
            lblTenKH.Location = new Point(12, 82);
            lblTenKH.Name = "lblTenKH";
            lblTenKH.Size = new Size(166, 41);
            lblTenKH.TabIndex = 1;
            lblTenKH.Text = "Họ và tên *";
            // 
            // lblSDT
            // 
            lblSDT.AutoSize = true;
            lblSDT.Location = new Point(16, 160);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new Size(193, 41);
            lblSDT.TabIndex = 3;
            lblSDT.Text = "Số điện thoại";
            // 
            // lblDiaChi
            // 
            lblDiaChi.AutoSize = true;
            lblDiaChi.Location = new Point(16, 244);
            lblDiaChi.Name = "lblDiaChi";
            lblDiaChi.Size = new Size(107, 41);
            lblDiaChi.TabIndex = 5;
            lblDiaChi.Text = "Địa chỉ";
            // 
            // lblLoaiKH
            // 
            lblLoaiKH.AutoSize = true;
            lblLoaiKH.Location = new Point(16, 319);
            lblLoaiKH.Name = "lblLoaiKH";
            lblLoaiKH.Size = new Size(118, 41);
            lblLoaiKH.TabIndex = 7;
            lblLoaiKH.Text = "Loại KH";
            // 
            // txtTenKH
            // 
            txtTenKH.Location = new Point(258, 82);
            txtTenKH.Name = "txtTenKH";
            txtTenKH.Size = new Size(166, 47);
            txtTenKH.TabIndex = 2;
            // 
            // txtSDT
            // 
            txtSDT.Location = new Point(258, 160);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(166, 47);
            txtSDT.TabIndex = 4;
            // 
            // txtDiaChi
            // 
            txtDiaChi.Location = new Point(258, 238);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(166, 47);
            txtDiaChi.TabIndex = 6;
            // 
            // cbLoaiKH
            // 
            cbLoaiKH.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLoaiKH.Items.AddRange(new object[] { "Thuong", "VIP" });
            cbLoaiKH.Location = new Point(258, 316);
            cbLoaiKH.Name = "cbLoaiKH";
            cbLoaiKH.Size = new Size(166, 49);
            cbLoaiKH.TabIndex = 8;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.DodgerBlue;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(16, 412);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(166, 49);
            btnSave.TabIndex = 11;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancel.Location = new Point(258, 413);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(166, 47);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Huỷ";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // ThemKhachHangMoi
            // 
            ClientSize = new Size(464, 508);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(lblTitle);
            Controls.Add(lblTenKH);
            Controls.Add(txtTenKH);
            Controls.Add(lblSDT);
            Controls.Add(txtSDT);
            Controls.Add(lblDiaChi);
            Controls.Add(txtDiaChi);
            Controls.Add(lblLoaiKH);
            Controls.Add(cbLoaiKH);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ThemKhachHangMoi";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thêm khách hàng mới";
            Load += ThemKhachHangMoi_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


    }
}