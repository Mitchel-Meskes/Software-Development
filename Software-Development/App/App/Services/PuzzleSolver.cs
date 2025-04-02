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
            // Placeholder
            return true;
        }
    }
}
