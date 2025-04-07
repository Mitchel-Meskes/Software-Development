using System;
using System.Windows.Forms;
using App.Services;
using App.Models;
using App.Utils;

namespace App.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Retrieve the username and password from the textboxes
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Check if both username and password are entered
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            // Load the list of users
            var users = UserService.LoadUsers();
            string hash = UserService.HashPassword(password);

            // Check if the user exists with the provided credentials
            var user = users.Find(u => u.Username == username && u.PasswordHash == hash);
            if (user == null)
            {
                MessageBox.Show("Invalid username or password.");
                Logger.Warning("Failed login attempt.");
                return;
            }

            Logger.Info($"Login successful for {username}");

            // Load settings and open the MainForm
            Settings settings = SettingsManager.LoadSettings();
            MainForm mainForm = new MainForm(username, settings);
            mainForm.Show();
            this.Hide();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            var users = UserService.LoadUsers();
            if (users.Exists(u => u.Username == username))
            {
                MessageBox.Show("Username already exists.");
                return;
            }

            var newUser = new User
            {
                Username = username,
                PasswordHash = UserService.HashPassword(password)
            };

            users.Add(newUser);
            UserService.SaveUsers(users);

            Logger.Info($"New registration: {username}");

            MessageBox.Show("Registration successful. Please log in.");
        }
    }
}
