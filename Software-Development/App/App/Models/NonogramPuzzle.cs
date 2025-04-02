using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class NonogramPuzzle
    {
        public int[,] Grid { get; set; }
        public int Size { get; set; }

        public NonogramPuzzle(int size)
        {
            Size = size;
            Grid = new int[size, size];
        }

        public NonogramPuzzle GenerateRotated(int rotation)
        {
            var newGrid = new int[Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    newGrid[i, j] = rotation switch
                    {
                        90 => Grid[Size - j - 1, i],
                        180 => Grid[Size - i - 1, Size - j - 1],
                        270 => Grid[j, Size - i - 1],
                        _ => Grid[i, j]
                    };

            return new NonogramPuzzle(Size) { Grid = newGrid };
        }

        public NonogramPuzzle GenerateMirrored(bool horizontal)
        {
            var newGrid = new int[Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    newGrid[i, j] = horizontal ? Grid[i, Size - j - 1] : Grid[Size - i - 1, j];

            return new NonogramPuzzle(Size) { Grid = newGrid };
        }
    }
}
