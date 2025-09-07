using App.Models; // Make sure to use the Models Difficulty
using System;

namespace App.Services
{
    public class PuzzleGenerator
    {
        private Random _random = new Random();

        // Use App.Models.Difficulty here to match NonogramPuzzle
        public NonogramPuzzle Generate(int size, App.Models.Difficulty difficulty)
        {
            // Pass both size and difficulty
            var puzzle = new NonogramPuzzle(size, difficulty);

            int density = difficulty switch
            {
                App.Models.Difficulty.Easy => 20,
                App.Models.Difficulty.Medium => 35,
                App.Models.Difficulty.Hard => 50,
                _ => 25
            };

            // Fill the solution grid randomly according to density
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    puzzle.Solution[r][c] = _random.Next(100) < density ? 1 : 0;
                    puzzle.Grid[r][c] = 0; // player grid starts empty
                }
            }

            return puzzle;
        }
    }
}
