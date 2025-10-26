namespace DA_QuanLiCuaHangCaPhe_Nhom9
{
    partial class Form1
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
        private void InitializeComponent()
        {
            txtPass = new TextBox();
            txtUser = new TextBox();
            btnOK = new Button();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // txtPass
            // 
            txtPass.Location = new Point(274, 159);
            txtPass.Margin = new Padding(1);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(120, 27);
            txtPass.TabIndex = 13;
            txtPass.TextChanged += textBox2_TextChanged;
            // 
            // txtUser
            // 
            txtUser.Location = new Point(274, 116);
            txtUser.Margin = new Padding(1);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(120, 27);
            txtUser.TabIndex = 14;
            txtUser.TextChanged += textBox1_TextChanged;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(287, 225);
            btnOK.Margin = new Padding(1);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 28);
            btnOK.TabIndex = 12;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += button1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(153, 162);
            label3.Margin = new Padding(1, 0, 1, 0);
            label3.Name = "label3";
            label3.Size = new Size(103, 20);
            label3.TabIndex = 9;
            label3.Text = "User Password";
            label3.Click += label3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(174, 119);
            label2.Margin = new Padding(1, 0, 1, 0);
            label2.Name = "label2";
            label2.Size = new Size(82, 20);
            label2.TabIndex = 10;
            label2.Text = "User Name";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(274, 56);
            label1.Margin = new Padding(1, 0, 1, 0);
            label1.Name = "label1";
            label1.Size = new Size(116, 20);
            label1.TabIndex = 11;
            label1.Text = "Sign in Account \r\n";
            label1.Click += label1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(547, 309);
            Controls.Add(txtPass);
            Controls.Add(txtUser);
            Controls.Add(btnOK);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
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
    }
}
