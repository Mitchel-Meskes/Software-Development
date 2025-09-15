using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.Services
{
    public static class PuzzleSolver
    {
        public static Task<bool> SolveAsync(NonogramPuzzle puzzle)
        {
            return Task.Run(() => BacktrackSolve(puzzle));
        }

        private static bool BacktrackSolve(NonogramPuzzle puzzle)
        {
            int size = puzzle.Size;

            // Work on a copy so we can backtrack
            int[][] grid = puzzle.Grid.Select(row => row.ToArray()).ToArray();

            if (SolveCell(puzzle, grid, 0, 0))
            {
                // Copy solution into puzzle.Grid
                for (int r = 0; r < size; r++)
                    for (int c = 0; c < size; c++)
                        puzzle.Grid[r][c] = grid[r][c];

                return true;
            }

            return false;
        }

        private static bool SolveCell(NonogramPuzzle puzzle, int[][] grid, int r, int c)
        {
            int size = puzzle.Size;
            if (r == size) return Validate(puzzle, grid);

            int nextR = c == size - 1 ? r + 1 : r;
            int nextC = c == size - 1 ? 0 : c + 1;

            // Try both filled (1) and empty (0)
            foreach (int val in new[] { 0, 1 })
            {
                grid[r][c] = val;
                if (SolveCell(puzzle, grid, nextR, nextC))
                    return true;
            }

            return false;
        }

        private static bool Validate(NonogramPuzzle puzzle, int[][] grid)
        {
            return ValidateLines(grid, puzzle.GetRowClues()) &&
                   ValidateLines(Transpose(grid), puzzle.GetColumnClues());
        }

        private static bool ValidateLines(int[][] lines, List<List<int>> clues)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (!MatchesClues(lines[i], clues[i]))
                    return false;
            }
            return true;
        }

        private static bool MatchesClues(int[] line, List<int> clues)
        {
            var runs = new List<int>();
            int count = 0;

            foreach (var cell in line)
            {
                if (cell == 1)
                {
                    count++;
                }
                else if (count > 0)
                {
                    runs.Add(count);
                    count = 0;
                }
            }
            if (count > 0) runs.Add(count);

            return runs.SequenceEqual(clues);
        }

        private static int[][] Transpose(int[][] grid)
        {
            int size = grid.Length;
            var result = new int[size][];
            for (int c = 0; c < size; c++)
            {
                result[c] = new int[size];
                for (int r = 0; r < size; r++)
                    result[c][r] = grid[r][c];
            }
            return result;
        }
    }
}
