namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    partial class Loginform
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            txtPass = new TextBox();
            txtUser = new TextBox();
            btnOK = new Button();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            btnThoat = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // txtPass
            // 
            txtPass.Location = new Point(128, 199);
            txtPass.Margin = new Padding(1);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(155, 27);
            txtPass.TabIndex = 13;
            txtPass.TextChanged += textBox2_TextChanged;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(128, 155);
            txtUser.Margin = new Padding(1);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(155, 27);
            txtUser.TabIndex = 14;
            txtUser.TextChanged += textBox1_TextChanged;
            // 
            // btnOK
            // 
            btnOK.BackColor = Color.Lime;
            btnOK.Location = new Point(62, 260);
            btnOK.Margin = new Padding(1);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(106, 28);
            btnOK.TabIndex = 12;
            btnOK.Text = "Đăng nhập";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnDangnhap_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BorderStyle = BorderStyle.Fixed3D;
            label3.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            label3.Location = new Point(25, 200);
            label3.Margin = new Padding(1, 0, 1, 0);
            label3.Name = "label3";
            label3.Size = new Size(87, 25);
            label3.TabIndex = 9;
            label3.Text = "Mật Khẩu";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BorderStyle = BorderStyle.Fixed3D;
            label2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            label2.Location = new Point(26, 157);
            label2.Margin = new Padding(1, 0, 1, 0);
            label2.Name = "label2";
            label2.Size = new Size(86, 25);
            label2.TabIndex = 10;
            label2.Text = "Tài Khoản";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label1.Location = new Point(170, 19);
            label1.Margin = new Padding(1, 0, 1, 0);
            label1.Name = "label1";
            label1.Size = new Size(194, 46);
            label1.TabIndex = 11;
            label1.Text = "Đăng nhập";
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Image = Properties.Resources.download;
            pictureBox1.Location = new Point(298, 114);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(237, 183);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 15;
            pictureBox1.TabStop = false;
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.Red;
            btnThoat.Location = new Point(195, 260);
            btnThoat.Margin = new Padding(1);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(88, 28);
            btnThoat.TabIndex = 13;
            btnThoat.Text = "Hủy";
            btnThoat.UseVisualStyleBackColor = false;
            btnThoat.Click += btnHuy_Click;
            // 
            // Loginform
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.NavajoWhite;
            ClientSize = new Size(547, 309);
            Controls.Add(pictureBox1);
            Controls.Add(txtPass);
            Controls.Add(txtUser);
            Controls.Add(btnThoat);
            Controls.Add(btnOK);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(1);
            Name = "Loginform";
            Text = "Loginform";
            Load += Loginform_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtPass;
        private TextBox txtUser;
        private Button btnOK;
        private Label label3;
        private Label label2;
        private Label label1;
        private PictureBox pictureBox1;
        private Button btnThoat;
    }
}
