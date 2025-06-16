using System;
using System.Diagnostics;
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
        private int cellSize;

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
            cellSize = Math.Max(10, 250 / size); // Dynamische celgrootte

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
            if (puzzle.IsSolved())
            {
                MessageBox.Show("Congratulations, you solved the puzzle!");
            }
            else
            {
                MessageBox.Show("The solution is incorrect, try again.");
            }
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
            // Placeholder: je zou een PuzzleService.SavePuzzle(puzzle, username) kunnen maken
            MessageBox.Show("Puzzle saved! (feature to be implemented)");
        }

        private void TimerControl_Tick(object sender, EventArgs e)
        {
            if (timer.IsRunning)
            {
                TimeSpan elapsed = timer.Elapsed;
                timerLabel.Text = $"Time: {elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            string selectedDifficulty = difficultyComboBox.SelectedItem.ToString();

            int gridSize = selectedDifficulty switch
            {
                "Easy" => 5,
                "Medium" => 7,
                "Hard" => 10,
                _ => 5
            };

            settings.Preferences.GridSize = gridSize;
            puzzle = new NonogramPuzzle(gridSize);
            InitPuzzle();
            timer.Restart();
        }
    }
}