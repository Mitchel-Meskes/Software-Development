using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace App.Models
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public class NonogramPuzzle
    {
        public List<List<int>> Grid { get; set; }       // Player's grid (0 = empty, 1 = filled)
        public List<List<int>> Solution { get; set; }   // Correct solution (0/1)
        public int Size { get; set; }
        public Difficulty Level { get; set; }

        private static Random rng = new Random();

        public NonogramPuzzle(int size, Difficulty level)
        {
            Size = size;
            Level = level;

            Grid = new List<List<int>>(size);
            Solution = new List<List<int>>(size);

            for (int i = 0; i < size; i++)
            {
                // initialize with zeros
                Grid.Add(new List<int>(new int[size]));
                Solution.Add(new List<int>(new int[size]));
            }

            GenerateSolution();
        }

        private void GenerateSolution()
        {
            switch (Level)
            {
                case Difficulty.Easy: GenerateEasy(); break;
                case Difficulty.Medium: GenerateMedium(); break;
                case Difficulty.Hard: GenerateHard(); break;
            }

            // Player grid starts empty (ensure zeros)
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    Grid[r][c] = 0;
        }

        private void GenerateEasy()
        {
            for (int r = 0; r < Size; r++)
            {
                int filled = rng.Next(1, Math.Max(2, Size)); // at least 1
                var indices = new HashSet<int>();
                while (indices.Count < filled) indices.Add(rng.Next(Size));
                for (int c = 0; c < Size; c++)
                    Solution[r][c] = indices.Contains(c) ? 1 : 0;
            }
        }

        private void GenerateMedium()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    Solution[r][c] = rng.NextDouble() < 0.5 ? 1 : 0;
        }

        private void GenerateHard()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    Solution[r][c] = rng.NextDouble() < 0.6 ? 1 : 0;
        }

        // -----------------------
        // Clue generation methods
        // -----------------------

        // returns list of clues for every row (each inner list are runs in that row)
        public List<List<int>> GetRowClues()
        {
            var result = new List<List<int>>(Size);
            for (int r = 0; r < Size; r++)
                result.Add(GetRowClues(r));
            return result;
        }

        // returns clues for a single row (no "0" sentinel — returns empty list if no runs)
        public List<int> GetRowClues(int rowIndex)
        {
            var clue = new List<int>();
            int count = 0;
            for (int c = 0; c < Size; c++)
            {
                if (Solution[rowIndex][c] == 1)
                {
                    count++;
                }
                else if (count > 0)
                {
                    clue.Add(count);
                    count = 0;
                }
            }
            if (count > 0) clue.Add(count);
            return clue; // may be empty
        }

        // returns list of clues for every column (each inner list are runs in that column)
        public List<List<int>> GetColumnClues()
        {
            var result = new List<List<int>>(Size);
            for (int c = 0; c < Size; c++)
                result.Add(GetColClues(c));
            return result;
        }

        // returns clues for a single column
        public List<int> GetColClues(int colIndex)
        {
            var clue = new List<int>();
            int count = 0;
            for (int r = 0; r < Size; r++)
            {
                if (Solution[r][colIndex] == 1)
                {
                    count++;
                }
                else if (count > 0)
                {
                    clue.Add(count);
                    count = 0;
                }
            }
            if (count > 0) clue.Add(count);
            return clue; // may be empty
        }

        // -----------------------
        // Display helpers (strings)
        // -----------------------

        // returns display-friendly strings ("0" if empty) — used by UI if you want a visible 0
        public List<string> GetRowCluesForDisplay()
        {
            var rowClues = new List<string>(Size);
            for (int r = 0; r < Size; r++)
            {
                var list = GetRowClues(r);
                rowClues.Add(list.Count > 0 ? string.Join(" ", list) : "0");
            }
            return rowClues;
        }

        public List<string> GetColumnCluesForDisplay()
        {
            var colClues = new List<string>(Size);
            for (int c = 0; c < Size; c++)
            {
                var list = GetColClues(c);
                colClues.Add(list.Count > 0 ? string.Join(" ", list) : "0");
            }
            return colClues;
        }

        // -----------------------
        // Other helpers
        // -----------------------

        public bool IsSolved()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (Grid[r][c] != Solution[r][c]) return false;
            return true;
        }

        public void ShowSolution()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    Grid[r][c] = Solution[r][c];
        }

        public string ToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        public static NonogramPuzzle FromJson(string json) => JsonSerializer.Deserialize<NonogramPuzzle>(json);
    }
}
