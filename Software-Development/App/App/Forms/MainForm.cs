using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using App.Models;

namespace App.Forms
{
    public partial class MainForm : Form
    {
        private NonogramPuzzle puzzle;
        private Stopwatch stopwatch;
        private Random random = new Random();
        private bool gameActive = false;

        // Controls
        private TableLayoutPanel gridPanel;
        private TableLayoutPanel rowHintsPanel;
        private TableLayoutPanel colHintsPanel;
        private Panel rightPanel;
        private Button startButton, pauseButton, resetButton, submitButton;
        private Button generateButton, hintButton, solveButton;
        private ComboBox difficultyComboBox;
        private Label timerLabel;
        private System.Windows.Forms.Timer timerControl;

        private readonly string username;
        private readonly Settings settings;

        private int hintsUsed = 0;
        private const int maxHints = 3;
        private bool solveConfirmed = false;

        public MainForm(string username, Settings settings)
        {
            this.username = username;
            this.settings = settings;
            this.stopwatch = new Stopwatch();

            InitializeComponent();
            InitializeControls();

            CreatePuzzle(Difficulty.Easy);

            rowHintsPanel.Visible = false;
            colHintsPanel.Visible = false;

            this.FormClosing += MainForm_FormClosing;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop de timer
            timerControl?.Stop();

            // Stop de stopwatch
            if (stopwatch != null && stopwatch.IsRunning)
                stopwatch.Stop();

            // Disable grid buttons
            EnableGrid(false);

            // Sluit de applicatie volledig af
            Application.Exit();
        }

        private void InitializeControls()
        {
            timerControl = new System.Windows.Forms.Timer { Interval = 1000 };
            timerControl.Tick += TimerControl_Tick;

            // Grid panel
            gridPanel = new TableLayoutPanel
            {
                Location = new Point(200, 100),
                Size = new Size(500, 500),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            // Hints panels
            rowHintsPanel = new TableLayoutPanel
            {
                Location = new Point(140, 100),
                Size = new Size(60, 500)
            };
            colHintsPanel = new TableLayoutPanel
            {
                Location = new Point(200, 40),
                Size = new Size(500, 60)
            };

            // Right panel
            rightPanel = new Panel
            {
                Location = new Point(720, 100),
                Size = new Size(150, 500)
            };

            // Right panel controls
            startButton = new Button { Text = "Start", Dock = DockStyle.Top, Height = 40 };
            pauseButton = new Button { Text = "Pause", Dock = DockStyle.Top, Height = 40, Enabled = false };
            resetButton = new Button { Text = "Reset", Dock = DockStyle.Top, Height = 40, Enabled = false };
            submitButton = new Button { Text = "Submit", Dock = DockStyle.Top, Height = 40, Enabled = false };
            solveButton = new Button { Text = "Solve Puzzle", Dock = DockStyle.Top, Height = 40, BackColor = Color.Red, ForeColor = Color.White, Enabled = false };

            timerLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Text = "Time: 00:00",
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            rightPanel.Controls.Add(timerLabel);
            rightPanel.Controls.Add(solveButton);
            rightPanel.Controls.Add(submitButton);
            rightPanel.Controls.Add(resetButton);
            rightPanel.Controls.Add(pauseButton);
            rightPanel.Controls.Add(startButton);

            // Generate and difficulty controls
            generateButton = new Button { Text = "Generate Puzzle", Location = new Point(20, 620), Size = new Size(160, 50) };
            difficultyComboBox = new ComboBox
            {
                Location = new Point(20, 680),
                Size = new Size(160, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            difficultyComboBox.Items.AddRange(new string[] { "Easy", "Medium", "Hard" });
            difficultyComboBox.SelectedIndex = 0;

            // Hint button
            hintButton = new Button
            {
                Text = "Hint",
                Location = new Point(420, 620),
                Size = new Size(120, 50),
                Enabled = false
            };

            Controls.Add(gridPanel);
            Controls.Add(rowHintsPanel);
            Controls.Add(colHintsPanel);
            Controls.Add(rightPanel);
            Controls.Add(generateButton);
            Controls.Add(difficultyComboBox);
            Controls.Add(hintButton);

            // Event handlers
            startButton.Click += StartButton_Click;
            pauseButton.Click += PauseButton_Click;
            resetButton.Click += ResetButton_Click;
            submitButton.Click += SubmitButton_Click;
            generateButton.Click += GenerateButton_Click;
            hintButton.Click += HintButton_Click;
            solveButton.Click += SolveButton_Click;
        }

        private void CreatePuzzle(Difficulty level)
        {
            int gridSize = level switch
            {
                Difficulty.Easy => 5,
                Difficulty.Medium => 10,
                Difficulty.Hard => 15,
                _ => 5
            };

            puzzle = new NonogramPuzzle(gridSize, level);
            BuildGrid();
        }

        private void BuildGrid()
        {
            // Clear old controls/styles
            gridPanel.Controls.Clear();
            gridPanel.ColumnStyles.Clear();
            gridPanel.RowStyles.Clear();
            rowHintsPanel.Controls.Clear();
            rowHintsPanel.RowStyles.Clear();
            rowHintsPanel.ColumnStyles.Clear();
            colHintsPanel.Controls.Clear();
            colHintsPanel.RowStyles.Clear();
            colHintsPanel.ColumnStyles.Clear();

            int size = puzzle.Size;

            // Grid layout
            gridPanel.ColumnCount = size;
            gridPanel.RowCount = size;
            for (int i = 0; i < size; i++)
            {
                gridPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / size));
                gridPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / size));
            }

            gridPanel.SuspendLayout();
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
                    gridPanel.Controls.Add(btn, c, r);
                }
            }
            gridPanel.ResumeLayout();

            // Make hint panels sized relative to grid (keurt flexibelere layout)
            int rowHintWidth = Math.Max(60, gridPanel.Width / 8);
            int colHintHeight = Math.Max(40, gridPanel.Height / 8);
            rowHintsPanel.Size = new Size(rowHintWidth, gridPanel.Height);
            colHintsPanel.Size = new Size(gridPanel.Width, colHintHeight);

            // Row hints (links)
            rowHintsPanel.RowCount = size;
            rowHintsPanel.ColumnCount = 1;
            rowHintsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            var allRowClues = puzzle.GetRowClues();
            rowHintsPanel.SuspendLayout();
            for (int r = 0; r < size; r++)
            {
                rowHintsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / size));
                var lbl = new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new Font("Arial", 12F, FontStyle.Bold),
                    Text = string.Join(" ", allRowClues[r]),
                    AutoSize = false,
                    Padding = new Padding(0, 0, 6, 0)
                    // **NO Visible = false** here — labels standaard zichtbaar
                };
                rowHintsPanel.Controls.Add(lbl, 0, r);
            }
            rowHintsPanel.ResumeLayout();

            // Column hints (boven)
            colHintsPanel.RowCount = 1;
            colHintsPanel.ColumnCount = size;
            colHintsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            var allColClues = puzzle.GetColumnClues();
            colHintsPanel.SuspendLayout();
            for (int c = 0; c < size; c++)
            {
                colHintsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / size));
                var lbl = new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.BottomCenter,
                    Font = new Font("Arial", 10F, FontStyle.Bold),
                    Text = string.Join(Environment.NewLine, allColClues[c]),
                    AutoSize = false,
                    Margin = new Padding(0)
                    // **NO Visible = false** hier ook
                };
                colHintsPanel.Controls.Add(lbl, c, 0);
            }
            colHintsPanel.ResumeLayout();

            // Position hint panels relative to the grid so they stay in the right place
            rowHintsPanel.Location = new Point(gridPanel.Left - rowHintsPanel.Width - 5, gridPanel.Top);
            colHintsPanel.Location = new Point(gridPanel.Left, gridPanel.Top - colHintsPanel.Height - 5);
        }

        private void ToggleHints(bool visible)
        {
            rowHintsPanel.Visible = visible;
            colHintsPanel.Visible = visible;

            foreach (Label lbl in rowHintsPanel.Controls) lbl.Visible = visible;
            foreach (Label lbl in colHintsPanel.Controls) lbl.Visible = visible;
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
                stopwatch.Stop();
                timerControl.Stop();
                gameActive = false;
                EnableGrid(false);
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            gameActive = true;
            ToggleHints(true);

            stopwatch.Start();
            timerControl.Start();
            EnableGrid(true);

            pauseButton.Enabled = true;
            resetButton.Enabled = true;
            submitButton.Enabled = true;
            hintButton.Enabled = true;
            solveButton.Enabled = true;

            startButton.Enabled = false; // <-- Start-knop direct disabled
        }


        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (gameActive)
            {
                gameActive = false;
                stopwatch.Stop();
                timerControl.Stop();
                pauseButton.Text = "Resume";
                foreach (Button btn in gridPanel.Controls) btn.BackColor = Color.Gray;
                ToggleHints(false);
            }
            else
            {
                gameActive = true;
                stopwatch.Start();
                timerControl.Start();
                pauseButton.Text = "Pause";
                foreach (Button btn in gridPanel.Controls)
                {
                    var (r, c) = ((int, int))btn.Tag;
                    btn.BackColor = puzzle.Grid[r][c] == 1 ? Color.Black : Color.White;
                    btn.Enabled = true;
                }
                ToggleHints(true);
            }
        }

        private void TimerControl_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            timerLabel.Text = $"Time: {elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            CreatePuzzle(puzzle.Level);
            stopwatch.Reset();
            timerControl.Stop();
            gameActive = false;
            hintsUsed = 0;
            EnableGrid(false);
            ToggleHints(false);

            startButton.Enabled = true;   // <-- Start-knop opnieuw beschikbaar
            pauseButton.Enabled = false;
            resetButton.Enabled = false;
            submitButton.Enabled = false;
            hintButton.Enabled = false;
            solveButton.Enabled = false;

            timerLabel.Text = "Time: 00:00";
        }


        private void SubmitButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(puzzle.IsSolved() ? "Correct!" : "Incorrect, try again.");
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            Difficulty selectedLevel = (Difficulty)difficultyComboBox.SelectedIndex;
            CreatePuzzle(selectedLevel);

            stopwatch.Reset();
            timerControl.Stop();
            gameActive = false;       // spel nog niet actief
            hintsUsed = 0;
            EnableGrid(false);        // grid uit
            ToggleHints(false);       // hints uit
            hintButton.Enabled = false;
            pauseButton.Enabled = false;
            resetButton.Enabled = false;
            submitButton.Enabled = false;
            solveButton.Enabled = false;

            startButton.Enabled = true; // <-- Start-knop weer beschikbaar
            timerLabel.Text = "Time: 00:00";
        }


        private void HintButton_Click(object sender, EventArgs e)
        {
            if (!gameActive || hintsUsed >= maxHints) return;

            var available = Enumerable.Range(0, puzzle.Size)
                .SelectMany(r => Enumerable.Range(0, puzzle.Size).Select(c => (r, c)))
                .Where(t => puzzle.Solution[t.r][t.c] == 1 && puzzle.Grid[t.r][t.c] != 1)
                .ToList();

            if (!available.Any()) return;

            var hintCell = available[random.Next(available.Count)];
            puzzle.Grid[hintCell.r][hintCell.c] = 1;

            foreach (Button btn in gridPanel.Controls)
            {
                var (r, c) = ((int, int))btn.Tag;
                if (r == hintCell.r && c == hintCell.c) { btn.BackColor = Color.Black; break; }
            }

            hintsUsed++;
            if (hintsUsed >= maxHints) hintButton.Enabled = false;
        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            if (!solveConfirmed)
            {
                MessageBox.Show("Press Solve Puzzle again to reveal the solution! (Counts as a loss)");
                solveConfirmed = true;
                return;
            }

            for (int r = 0; r < puzzle.Size; r++)
            {
                for (int c = 0; c < puzzle.Size; c++)
                {
                    puzzle.Grid[r][c] = puzzle.Solution[r][c];
                    foreach (Button btn in gridPanel.Controls)
                    {
                        var (br, bc) = ((int, int))btn.Tag;
                        if (br == r && bc == c) btn.BackColor = puzzle.Grid[r][c] == 1 ? Color.Black : Color.White;
                    }
                }
            }

            gameActive = false;
            EnableGrid(false);
            stopwatch.Stop();
            timerControl.Stop();
            solveConfirmed = false;
            MessageBox.Show("You pressed Solve Puzzle — you lost!");
        }

        private void EnableGrid(bool enabled)
        {
            foreach (Button btn in gridPanel.Controls) btn.Enabled = enabled;
        }
    }
}
