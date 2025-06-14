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

            // Automatisch schalen op DPI
            this.AutoScaleMode = AutoScaleMode.Dpi;
            // Form automatisch laten groeien/krimpen indien nodig
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            // Startpositie in het midden van het scherm
            this.StartPosition = FormStartPosition.CenterScreen;
            // Minimale grootte instellen
            this.MinimumSize = new Size(400, 300); // Pas aan naar wens
            // Optioneel: standaardgrootte instellen
            this.Size = new Size(500, 350); // Pas aan naar wens
            // Optioneel: groter lettertype voor leesbaarheid
            this.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vul zowel gebruikersnaam als wachtwoord in.", "Invoer ontbreekt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var users = UserService.LoadUsers();
                string hash = UserService.HashPassword(password);

                var user = users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.PasswordHash == hash);
                if (user == null)
                {
                    MessageBox.Show("Ongeldige gebruikersnaam of wachtwoord.", "Login mislukt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Warning($"Mislukte login poging voor gebruiker: {username}");
                    return;
                }

                Logger.Info($"Login succesvol voor gebruiker: {username}");

                // Optioneel: game stats tonen
                var gameStatsService = new GameStatsService();
                var stats = gameStatsService.GetGameStatsByUsername(username);
                if (stats != null)
                {
                    MessageBox.Show($"Spelstatistieken voor {username}:\nVoltooide puzzels: {stats.CompletedPuzzles}\nTotale speeltijd: {stats.TotalPlayTimeMinutes} minuten", "Spelstatistieken", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                var settings = SettingsManager.LoadSettings();
                var mainForm = new MainForm(username, settings);
                mainForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                Logger.Error($"Fout tijdens login: {ex.Message}");
                MessageBox.Show("Er is een fout opgetreden bij het inloggen. Probeer het later opnieuw.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vul zowel gebruikersnaam als wachtwoord in.", "Invoer ontbreekt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var users = UserService.LoadUsers();

                if (users.Exists(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Deze gebruikersnaam bestaat al.", "Registratie mislukt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newUser = new User(username, UserService.HashPassword(password));
                users.Add(newUser);
                UserService.SaveUsers(users);

                Logger.Info($"Nieuwe gebruiker geregistreerd: {username}");
                MessageBox.Show("Registratie gelukt! Je kunt nu inloggen.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optioneel: maak velden leeg en focus op username
                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error($"Fout tijdens registratie: {ex.Message}");
                MessageBox.Show("Er is een fout opgetreden bij het registreren. Probeer het later opnieuw.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
