using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using App.Models;
using App.Services;
using System.Linq;

namespace App.Forms
{
    public partial class MainForm : Form
    {
        private readonly string username;
        private readonly Settings settings;
        private NonogramPuzzle puzzle;
        private Stopwatch timer;
        private TableLayoutPanel gridPanel;
        private bool gameActive = false;



        public MainForm(string username, Settings settings)
        {
            this.username = username;
            this.settings = settings;
            this.timer = new Stopwatch();
            this.timerControl = new System.Windows.Forms.Timer();

            InitializeComponent();
            InitializeGridPanel();

            // Initialize default puzzle
            int gridSize = settings.Preferences.GridSize;
            puzzle = new NonogramPuzzle(gridSize, Difficulty.Easy);
            BuildGrid();

            // Timer setup
            timerControl.Interval = 1000;
            timerControl.Tick += TimerControl_Tick;
        }

        private void InitializeGridPanel()
        {
            gridPanel = new TableLayoutPanel
            {
                Dock = DockStyle.None,
                Location = new Point(20, 20),
                Size = new Size(400, 400),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            this.Controls.Add(gridPanel);
        }

        private void BuildGrid()
        {
            gridPanel.Controls.Clear();
            gridPanel.ColumnStyles.Clear();
            gridPanel.RowStyles.Clear();

            int size = puzzle.Size;

            // +1 for clue row/column
            gridPanel.ColumnCount = size + 1;
            gridPanel.RowCount = size + 1;

            for (int i = 0; i <= size; i++)
            {
                gridPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / (size + 1)));
                gridPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / (size + 1)));
            }

            var rowHints = puzzle.GetRowClues(); // int[]
            var colHints = puzzle.GetColumnClues(); // int[]

            // Top-left empty
            gridPanel.Controls.Add(new Label { Text = "", Dock = DockStyle.Fill }, 0, 0);

            // Column hints
            for (int c = 0; c < size; c++)
            {
                var lbl = new Label
                {
                    Text = colHints[c].ToString(),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(FontFamily.GenericSansSerif, 10)
                };
                gridPanel.Controls.Add(lbl, c + 1, 0);
            }

            // Row hints
            for (int r = 0; r < size; r++)
            {
                var lbl = new Label
                {
                    Text = rowHints[r].ToString(),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(FontFamily.GenericSansSerif, 10)
                };
                gridPanel.Controls.Add(lbl, 0, r + 1);
            }

            // Puzzle buttons
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    var btn = new Button
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        BackColor = Color.White,
                        Tag = (r, c),
                        Enabled = gameActive
                    };
                    btn.Click += Cell_Click;
                    gridPanel.Controls.Add(btn, c + 1, r + 1);
                }
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            if (!gameActive) return;

            var btn = sender as Button;
            var (r, c) = ((int, int))btn.Tag;

            puzzle.Grid[r][c] = puzzle.Grid[r][c] == 0 ? 1 : 0;
            btn.BackColor = puzzle.Grid[r][c] == 0 ? Color.White : Color.Black;

            if (puzzle.IsSolved())
            {
                MessageBox.Show("Congratulations! You solved the puzzle!");
                timer.Stop();
                timerControl.Stop();
                gameActive = false;
                DisableGrid();
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            gameActive = true;
            timer.Start();
            timerControl.Start();
            EnableGrid();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (timer.IsRunning)
            {
                timer.Stop();
                timerControl.Stop();
                gameActive = false;
                pauseButton.Text = "Resume";
                DisableGrid();
            }
            else
            {
                timer.Start();
                timerControl.Start();
                gameActive = true;
                pauseButton.Text = "Pause";
                EnableGrid();
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            puzzle = new NonogramPuzzle(settings.Preferences.GridSize, GetSelectedDifficulty());
            timer.Reset();
            timerControl.Stop();
            gameActive = false;
            BuildGrid();
            timerLabel.Text = "Time: 00:00";
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            puzzle = new NonogramPuzzle(GetGridSize(), GetSelectedDifficulty());
            timer.Reset();
            timerControl.Stop();
            gameActive = false;
            timerLabel.Text = "Time: 00:00";
            BuildGrid();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (puzzle.IsSolved())
                MessageBox.Show("Congratulations! Puzzle solved!");
            else
                MessageBox.Show("The solution is incorrect, try again.");
        }

        private void ShowSolutionButton_Click(object sender, EventArgs e)
        {
            // Fill the grid with solution
            for (int r = 0; r < puzzle.Size; r++)
            {
                for (int c = 0; c < puzzle.Size; c++)
                {
                    puzzle.Grid[r][c] = puzzle.Solution[r][c];
                }
            }
            BuildGrid();
            gameActive = false;
            timerControl.Stop();
            timer.Stop();
        }

        private void TimerControl_Tick(object sender, EventArgs e)
        {
            if (timer.IsRunning)
            {
                TimeSpan elapsed = timer.Elapsed;
                timerLabel.Text = $"Time: {elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
            }
        }

        private void EnableGrid()
        {
            foreach (Control ctrl in gridPanel.Controls)
                if (ctrl is Button btn && btn.Tag != null)
                    btn.Enabled = true;
        }

        private void DisableGrid()
        {
            foreach (Control ctrl in gridPanel.Controls)
                if (ctrl is Button btn && btn.Tag != null)
                    btn.Enabled = false;
        }

        private Difficulty GetSelectedDifficulty()
        {
            return difficultyComboBox.SelectedItem?.ToString() switch
            {
                "Easy" => Difficulty.Easy,
                "Medium" => Difficulty.Medium,
                "Hard" => Difficulty.Hard,
                _ => Difficulty.Easy
            };
        }

        private int GetGridSize()
        {
            return difficultyComboBox.SelectedItem?.ToString() switch
            {
                "Easy" => 5,
                "Medium" => 7,
                "Hard" => 10,
                _ => settings.Preferences.GridSize
            };
        }
    }
}
