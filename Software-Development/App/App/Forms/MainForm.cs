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
            // Clear previous controls
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

        private void TimerControl_Tick(object sender, EventArgs e)
        {
            if (timer.IsRunning)
            {
                TimeSpan elapsed = timer.Elapsed;
                timerLabel.Text = $"Time: {elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
            }
        }
    }
}
