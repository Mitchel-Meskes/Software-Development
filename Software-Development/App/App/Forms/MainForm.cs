using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using App.Models;
using App.Services;
using App.Utils;

namespace App.Forms
{
    public class MainForm : Form
    {
        private readonly string username;
        private readonly UserSettings settings;
        private readonly NonogramPuzzle puzzle;
        private readonly Panel gridPanel;
        private readonly Stopwatch timer;
        private int cellSize = 25;

        public MainForm(string username, UserSettings settings)
        {
            this.username = username;
            this.settings = settings;
            this.puzzle = new NonogramPuzzle(settings.GridSize);
            this.gridPanel = new Panel();
            this.timer = new Stopwatch();

            InitPuzzle();
            InitializeComponent();
            timer.Start();
        }

        private void InitializeComponent()
        {
            this.Text = "Nonogram";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = puzzle.Size * cellSize + 60;
            this.Height = puzzle.Size * cellSize + 120;

            gridPanel.Location = new Point(20, 20);
            gridPanel.Width = puzzle.Size * cellSize;
            gridPanel.Height = puzzle.Size * cellSize;
            gridPanel.Paint += DrawPuzzleGrid;
            gridPanel.MouseClick += GridPanel_MouseClick;

            var btnSolve = new Button { Text = "Oplossen", Top = gridPanel.Bottom + 10, Left = 20 };
            btnSolve.Click += async (s, e) =>
            {
                bool solved = await PuzzleSolver.SolveAsync(puzzle);
                MessageBox.Show(solved ? "Puzzel opgelost!" : "Geen oplossing gevonden.");
                Logger.Info($"Puzzel opgelost door {username}");
            };

            var btnOpslaan = new Button { Text = "Opslaan", Top = gridPanel.Bottom + 10, Left = 120 };
            btnOpslaan.Click += (s, e) =>
            {
                GameStateService.Save(puzzle);
                MessageBox.Show("Voortgang opgeslagen.");
                Logger.Info($"Voortgang opgeslagen voor {username}");
            };

            var btnAfsluiten = new Button { Text = "Voltooid", Top = gridPanel.Bottom + 10, Left = 220 };
            btnAfsluiten.Click += (s, e) =>
            {
                timer.Stop();
                double minuten = timer.Elapsed.TotalMinutes;
                StatisticsService.UpdateStats(username, minuten);
                MessageBox.Show($"Tijd: {minuten:F1} min\nStatistieken bijgewerkt.");
                this.Close();
            };

            this.Controls.Add(gridPanel);
            this.Controls.Add(btnSolve);
            this.Controls.Add(btnOpslaan);
            this.Controls.Add(btnAfsluiten);
        }

        private void InitPuzzle()
        {
            Random rnd = new();
            for (int i = 0; i < puzzle.Size; i++)
                for (int j = 0; j < puzzle.Size; j++)
                    puzzle.Grid[i, j] = rnd.Next(2);
        }

        private void DrawPuzzleGrid(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            for (int i = 0; i < puzzle.Size; i++)
            {
                for (int j = 0; j < puzzle.Size; j++)
                {
                    Rectangle rect = new Rectangle(j * cellSize, i * cellSize, cellSize, cellSize);
                    g.FillRectangle(puzzle.Grid[i, j] == 1 ? Brushes.Black : Brushes.White, rect);
                    g.DrawRectangle(Pens.Gray, rect);
                }
            }
        }

        private void GridPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int row = e.Y / cellSize;
            int col = e.X / cellSize;
            if (row < puzzle.Size && col < puzzle.Size)
            {
                puzzle.Grid[row, col] = 1 - puzzle.Grid[row, col];
                gridPanel.Invalidate();
            }
        }
    }
}
