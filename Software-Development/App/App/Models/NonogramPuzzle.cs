using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace App.Models
{
    public class NonogramPuzzle
    {
        public List<List<int>> Grid { get; set; }  // Gebruik een List van List voor betere serialisatie
        public int Size { get; set; }

        public NonogramPuzzle(int size)
        {
            Size = size;
            Grid = new List<List<int>>(size);
            for (int i = 0; i < size; i++)
            {
                Grid.Add(new List<int>(new int[size])); // Vul met 0’en voor correcte lengte
            }
        }

        // Serialisatie naar JSON
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true // Optioneel, voor leesbaarheid
            });
        }

        // Deserialisatie van JSON naar NonogramPuzzle
        public static NonogramPuzzle FromJson(string json)
        {
            return JsonSerializer.Deserialize<NonogramPuzzle>(json);
        }

        // Genereren van geroteerde puzzel
        public NonogramPuzzle GenerateRotated(int rotation)
        {
            var newGrid = new List<List<int>>(Size);
            for (int i = 0; i < Size; i++)
                newGrid.Add(new List<int>(new int[Size]));

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    newGrid[i][j] = rotation switch
                    {
                        90 => Grid[Size - j - 1][i],
                        180 => Grid[Size - i - 1][Size - j - 1],
                        270 => Grid[j][Size - i - 1],
                        _ => Grid[i][j]
                    };

            return new NonogramPuzzle(Size) { Grid = newGrid };
        }

        // Genereren van gespiegeld grid
        public NonogramPuzzle GenerateMirrored(bool horizontal)
        {
            var newGrid = new List<List<int>>(Size);
            for (int i = 0; i < Size; i++)
                newGrid.Add(new List<int>(new int[Size]));

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    newGrid[i][j] = horizontal ? Grid[i][Size - j - 1] : Grid[Size - i - 1][j];

            return new NonogramPuzzle(Size) { Grid = newGrid };
        }

        public bool IsSolved()
        {
            foreach (var row in Grid)
            {
                foreach (var cell in row)
                {
                    if (cell == 0)
                        return false; // Nog lege cel gevonden
                }
            }
            return true;
        }
    }
}
