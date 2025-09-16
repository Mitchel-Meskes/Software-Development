namespace App.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.TableLayoutPanel mainPanel;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;

        private void InitializeComponent()
        {
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();

            this.SuspendLayout();

            // 
            // mainPanel
            // 
            this.mainPanel.ColumnCount = 2;
            this.mainPanel.RowCount = 3;
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Padding = new Padding(20);
            this.mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            this.mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            this.mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            // 
            // lblUsername
            // 
            this.lblUsername.Text = "Gebruikersnaam:";
            this.lblUsername.TextAlign = ContentAlignment.MiddleRight;
            this.lblUsername.Dock = DockStyle.Fill;

            // 
            // txtUsername
            // 
            this.txtUsername.Dock = DockStyle.Fill;
            this.txtUsername.TabIndex = 0;

            // 
            // lblPassword
            // 
            this.lblPassword.Text = "Wachtwoord:";
            this.lblPassword.TextAlign = ContentAlignment.MiddleRight;
            this.lblPassword.Dock = DockStyle.Fill;

            // 
            // txtPassword
            // 
            this.txtPassword.Dock = DockStyle.Fill;
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.TabIndex = 1;

            // 
            // buttonPanel
            // 
            this.buttonPanel.Dock = DockStyle.Fill;
            this.buttonPanel.FlowDirection = FlowDirection.LeftToRight;
            this.buttonPanel.Controls.Add(this.btnLogin);
            this.buttonPanel.Controls.Add(this.btnRegister);

            // 
            // btnLogin
            // 
            this.btnLogin.Text = "Login";
            this.btnLogin.AutoSize = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // 
            // btnRegister
            // 
            this.btnRegister.Text = "Register";
            this.btnRegister.AutoSize = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

            // 
            // Add controls to mainPanel
            // 
            this.mainPanel.Controls.Add(this.lblUsername, 0, 0);
            this.mainPanel.Controls.Add(this.txtUsername, 1, 0);
            this.mainPanel.Controls.Add(this.lblPassword, 0, 1);
            this.mainPanel.Controls.Add(this.txtPassword, 1, 1);
            this.mainPanel.Controls.Add(this.buttonPanel, 1, 2);

            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(600, 250);
            this.Controls.Add(this.mainPanel);
            this.Name = "LoginForm";
            this.Text = "Login";

            this.ResumeLayout(false);
        }
    }
}
