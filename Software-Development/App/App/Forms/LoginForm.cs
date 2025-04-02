using System;
using System.Windows.Forms;
using App.Models;
using App.Services;
using App.Utils;

namespace App.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtUsername = new TextBox { PlaceholderText = "Gebruikersnaam", Top = 20, Width = 200, Left = 10 };
            this.txtPassword = new TextBox { PlaceholderText = "Wachtwoord", Top = 50, Width = 200, Left = 10, PasswordChar = '*' };
            this.btnLogin = new Button { Text = "Inloggen", Top = 85, Width = 95, Left = 10 };
            this.btnRegister = new Button { Text = "Registreren", Top = 85, Width = 95, Left = 115 };

            this.btnLogin.Click += BtnLogin_Click;
            this.btnRegister.Click += BtnRegister_Click;

            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
            this.Text = "Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new System.Drawing.Size(240, 140);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var users = UserService.LoadUsers();
            string hash = UserService.HashPassword(txtPassword.Text);

            if (users.Exists(u => u.Username == txtUsername.Text && u.PasswordHash == hash))
            {
                Logger.Info($"Inloggen geslaagd voor {txtUsername.Text}");
                this.Hide();
                var settings = SettingsManager.LoadSettings();
                var mainForm = new MainForm(txtUsername.Text, settings);
                mainForm.ShowDialog();
                this.Close();
            }
            else
            {
                Logger.Warning("Mislukte inlogpoging");
                MessageBox.Show("Onjuiste gebruikersnaam of wachtwoord.");
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var users = UserService.LoadUsers();
            if (users.Exists(u => u.Username == txtUsername.Text))
            {
                MessageBox.Show("Gebruiker bestaat al.");
                return;
            }

            users.Add(new User
            {
                Username = txtUsername.Text,
                PasswordHash = UserService.HashPassword(txtPassword.Text)
            });

            UserService.SaveUsers(users);
            Logger.Info($"Nieuwe registratie: {txtUsername.Text}");
            MessageBox.Show("Registratie geslaagd.");
        }
    }
}
