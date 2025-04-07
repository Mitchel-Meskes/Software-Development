using System;
using System.Diagnostics;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using App.Models;
using App.Services;

namespace App.Forms
{
    public partial class MainForm : Form
    {
        private readonly string username;
        private readonly Settings settings;
        private NonogramPuzzle puzzle;
        private Stopwatch timer;
        private int cellSize = 25;

        public MainForm(string username, Settings settings)
        {
            this.username = username;
            this.settings = settings;
            this.puzzle = new NonogramPuzzle(settings.Preferences.GridSize);
            this.timer = new Stopwatch();

            InitializeComponent();
            InitPuzzle();
            timer.Start();
            timerControl.Start();
        }

        private void InitPuzzle()
        {
            gridPanel.Controls.Clear();

            int size = settings.Preferences.GridSize;
            gridPanel.Size = new Size(size * cellSize, size * cellSize);
            gridPanel.BackColor = Color.White;
            gridPanel.BorderStyle = BorderStyle.FixedSingle;

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Label cell = new Label
                    {
                        Size = new Size(cellSize, cellSize),
                        Location = new Point(col * cellSize, row * cellSize),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.LightGray
                    };
                    gridPanel.Controls.Add(cell);
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            puzzle = new NonogramPuzzle(settings.Preferences.GridSize);
            InitPuzzle();
            timer.Restart();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            puzzle = new NonogramPuzzle(settings.Preferences.GridSize);
            InitPuzzle();
            timer.Restart();
            timerControl.Start();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Puzzle submitted!");
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (timer.IsRunning)
            {
                timer.Stop();
                timerControl.Stop();
                pauseButton.Text = "Resume";
            }
            else
            {
                timer.Start();
                timerControl.Start();
                pauseButton.Text = "Pause";
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Hier kun je je eigen logica toevoegen voor het opslaan
            MessageBox.Show("Puzzle saved!");
        }

        private void TimerControl_Tick(object sender, EventArgs e)
        {
            if (timer.IsRunning)
            {
                TimeSpan elapsed = timer.Elapsed;
                timerLabel.Text = $"Time: {elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
            }
        }

        // Deze methode wordt aangeroepen wanneer op de "Genereer puzzel" knop wordt geklikt
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // Verkrijg de geselecteerde moeilijkheidsgraad
            string selectedDifficulty = difficultyComboBox.SelectedItem.ToString();

            // Verander de grootte van de grid afhankelijk van de moeilijkheidsgraad
            int gridSize = selectedDifficulty switch
            {
                "Easy" => 5,     // Makkelijk: 5x5
                "Medium" => 7,   // Medium: 7x7
                "Hard" => 10,    // Moeilijk: 10x10
                _ => 5          // Standaard naar Easy
            };

            // Stel de nieuwe gridgrootte in op basis van de gekozen moeilijkheidsgraad
            settings.Preferences.GridSize = gridSize;
            puzzle = new NonogramPuzzle(gridSize);
            InitPuzzle();
            timer.Restart();  // Reset de timer voor de nieuwe puzzel
        }
    }
}
