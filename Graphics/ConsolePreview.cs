using System;
using Traffic.World;

namespace Traffic.Graphics
{
    /// <summary>
    /// Klasa poglądowa, póki nie będzie lepszej grafiki. Zatem nie srać o brak komentarzy :) 
    /// </summary>
    public static class ConsolePreview
    {
        public static void PrintMap(Map m)
        {
            ConsolePreview.PrintStreets(m);
            ConsolePreview.PrintSpawns(m);
            ConsolePreview.PrintIntersections(m);
        }

        private static void Print(int x, int y, char z)
        {
            Console.SetCursorPosition(y, x);
            Console.Write(z);
        }

        private static void PrintStreets(Map m)
        {
            foreach (var street in m.Streets)
            {
                char sign = street.IsVertical ? '|' : '-';
                ConsolePreview.Print(street.RowNumber, street.ColumnNumber, sign);
            }
        }

        private static void PrintSpawns(Map m)
        {
            foreach (var spawn in m.SpawnPoints)
            {
                if (spawn.Street.IsVertical)
                {
                    if (spawn.Street.RowNumber == 1)
                        ConsolePreview.Print(spawn.Street.RowNumber - 1, spawn.Street.ColumnNumber, 'S');
                    else
                        ConsolePreview.Print(spawn.Street.RowNumber + 1, spawn.Street.ColumnNumber, 'S');
                }
                else
                {
                    if (spawn.Street.ColumnNumber == 1)
                        ConsolePreview.Print(spawn.Street.RowNumber, spawn.Street.ColumnNumber - 1, 'S');
                    else
                        ConsolePreview.Print(spawn.Street.RowNumber, spawn.Street.ColumnNumber + 1, 'S');
                }
            }
        }

        private static void PrintIntersections(Map m)
        {
            foreach (var intersection in m.Intersections)
            {
                ConsolePreview.Print(intersection.RowNumber, intersection.ColumnNumber, '+');
            }
        }
    }
}
