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
            // Verkrijg de gebruikersnaam en wachtwoord uit de tekstvakken
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Controleer of zowel gebruikersnaam als wachtwoord zijn ingevoerd
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            // Laad de lijst met gebruikers
            var users = UserService.LoadUsers();
            string hash = UserService.HashPassword(password);

            // Zoek naar de gebruiker met de opgegeven gegevens
            var user = users.Find(u => u.Username == username && u.PasswordHash == hash);
            if (user == null)
            {
                MessageBox.Show("Invalid username or password.");
                Logger.Warning("Failed login attempt.");
                return;
            }

            Logger.Info($"Login successful for {username}");

            // Laad de game-statistieken voor de ingelogde gebruiker
            GameStatsService gameStatsService = new GameStatsService();
            GameStats userStats = gameStatsService.GetGameStatsByUsername(username);

            if (userStats != null)
            {
                MessageBox.Show($"Game Stats for {username}:\nCompleted Puzzles: {userStats.CompletedPuzzles}\nTotal Play Time: {userStats.TotalPlayTimeMinutes} minutes");
            }
            else
            {
                MessageBox.Show("No game stats found for this user.");
            }

            // Laad instellingen en open het MainForm
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

            // ✅ Gebruik nu de constructor van User correct
            var newUser = new User(username, UserService.HashPassword(password));

            users.Add(newUser);
            UserService.SaveUsers(users);

            Logger.Info($"New registration: {username}");

            MessageBox.Show("Registration successful. Please log in.");
        }
    }
}
