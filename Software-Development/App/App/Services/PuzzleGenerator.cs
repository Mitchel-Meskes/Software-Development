using App.Models;

namespace App.Services
{
	public enum Difficulty
	{
		Easy,
		Medium,
		Hard
	}

	public class PuzzleGenerator
	{
		private Random _random = new Random();

		public NonogramPuzzle Generate(int size, Difficulty difficulty)
		{
			var puzzle = new NonogramPuzzle(size);

			int density = difficulty switch
			{
				Difficulty.Easy => 20,
				Difficulty.Medium => 35,
				Difficulty.Hard => 50,
				_ => 25
			};

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					puzzle.Grid[i, j] = _random.Next(100) < density ? 1 : 0;
				}
			}

			return puzzle;
		}
	}
}
