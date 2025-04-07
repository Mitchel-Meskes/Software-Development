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
        private readonly string _username;
        private readonly UserSettings _settings;
        private readonly NonogramPuzzle _puzzle;
        private readonly Panel _gridPanel;
        private readonly Stopwatch _timer;
        private int _cellSize = 25;

        public MainForm(string username, UserSettings settings)
        {
            _username = username;
            _settings = settings;
            _puzzle = new NonogramPuzzle(settings.GridSize);
            _gridPanel = new Panel();
            _timer = new Stopwatch();

            InitPuzzle();
            InitializeComponent();
            _timer.Start();
        }

        private void InitializeComponent()
        {
            this.Text = "Nonogram";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = _puzzle.Size * _cellSize + 60;
            this.Height = _puzzle.Size * _cellSize + 120;

            _gridPanel.Location = new Point(20, 20);
            _gridPanel.Width = _puzzle.Size * _cellSize;
            _gridPanel.Height = _puzzle.Size * _cellSize;
            _gridPanel.Paint += DrawPuzzleGrid;
            _gridPanel.MouseClick += GridPanel_MouseClick;

            var btnSolve = new Button { Text = "Oplossen", Top = _gridPanel.Bottom + 10, Left = 20 };
            btnSolve.Click += async (s, e) =>
            {
                bool solved = await PuzzleSolver.SolveAsync(_puzzle);
                MessageBox.Show(solved ? "Puzzel opgelost!" : "Geen oplossing gevonden.");
                Logger.Info($"Puzzel opgelost door {_username}");
            };

            var btnOpslaan = new Button { Text = "Opslaan", Top = _gridPanel.Bottom + 10, Left = 120 };
            btnOpslaan.Click += (s, e) =>
            {
                GameStateService.Save(_puzzle);
                MessageBox.Show("Voortgang opgeslagen.");
                Logger.Info($"Voortgang opgeslagen voor {_username}");
            };

            var btnAfsluiten = new Button { Text = "Voltooid", Top = _gridPanel.Bottom + 10, Left = 220 };
            btnAfsluiten.Click += (s, e) =>
            {
                _timer.Stop();
                double minuten = _timer.Elapsed.TotalMinutes;
                StatisticsService.UpdateStats(_username, minuten);
                MessageBox.Show($"Tijd: {minuten:F1} min\nStatistieken bijgewerkt.");
                this.Close();
            };

            this.Controls.Add(_gridPanel);
            this.Controls.Add(btnSolve);
            this.Controls.Add(btnOpslaan);
            this.Controls.Add(btnAfsluiten);
        }

        private void InitPuzzle()
        {
            Random rnd = new();
            for (int i = 0; i < _puzzle.Size; i++)
                for (int j = 0; j < _puzzle.Size; j++)
                    _puzzle.Grid[i, j] = rnd.Next(2);
        }

        private void DrawPuzzleGrid(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            for (int i = 0; i < _puzzle.Size; i++)
            {
                for (int j = 0; j < _puzzle.Size; j++)
                {
                    Rectangle rect = new Rectangle(j * _cellSize, i * _cellSize, _cellSize, _cellSize);
                    g.FillRectangle(_puzzle.Grid[i, j] == 1 ? Brushes.Black : Brushes.White, rect);
                    g.DrawRectangle(Pens.Gray, rect);
                }
            }
        }

        private void GridPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int row = e.Y / _cellSize;
            int col = e.X / _cellSize;
            if (row < _puzzle.Size && col < _puzzle.Size)
            {
                _puzzle.Grid[row, col] = 1 - _puzzle.Grid[row, col];
                _gridPanel.Invalidate();
            }
        }
    }
}
