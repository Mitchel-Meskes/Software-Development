using App.Models; 
using System;

namespace App.Services
{
    public class PuzzleGenerator
    {
        private Random _random = new Random();

        
        public NonogramPuzzle Generate(int size, App.Models.Difficulty difficulty)
        {
            
            var puzzle = new NonogramPuzzle(size, difficulty);

            int density = difficulty switch
            {
                App.Models.Difficulty.Easy => 20,
                App.Models.Difficulty.Medium => 35,
                App.Models.Difficulty.Hard => 50,
                _ => 25
            };

            
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    puzzle.Solution[r][c] = _random.Next(100) < density ? 1 : 0;
                    puzzle.Grid[r][c] = 0; 
                }
            }

            return puzzle;
        }
    }
}
