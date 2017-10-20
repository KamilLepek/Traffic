using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.World;

namespace Traffic.Graphics
{
    /// <summary>
    /// Klasa poglądowa, póki nie będzie lepszej grafiki. Zatem nie srać o brak komentarzy :) 
    /// </summary>
    public class ConsolePreview
    {
        public ConsolePreview(Map m)
        {
            this.PrintMap(m);
        }

        public void PrintMap(Map m)
        {
            this.PrintStreets(m);
            this.PrintSpawns(m);
            this.PrintIntersections(m);
        }

        private void Print(int x, int y, char z)
        {
            Console.SetCursorPosition(y, x);
            Console.Write(z);
        }

        private void PrintStreets(Map m)
        {
            foreach (var street in m.Streets)
            {
                char sign = street.IsVertical ? '|' : '-';
                this.Print(street.RowNumber, street.ColumnNumber, sign);
            }
        }

        private void PrintSpawns(Map m)
        {
            foreach (var spawn in m.SpawnPoints)
            {
                if (spawn.Street.IsVertical)
                {
                    if (spawn.Street.RowNumber == 1)
                        this.Print(spawn.Street.RowNumber - 1, spawn.Street.ColumnNumber, 'S');
                    else
                        this.Print(spawn.Street.RowNumber + 1, spawn.Street.ColumnNumber, 'S');
                }
                else
                {
                    if (spawn.Street.ColumnNumber == 1)
                        this.Print(spawn.Street.RowNumber, spawn.Street.ColumnNumber - 1, 'S');
                    else
                        this.Print(spawn.Street.RowNumber, spawn.Street.ColumnNumber + 1, 'S');
                }
            }
        }

        private void PrintIntersections(Map m)
        {
            foreach (var intersection in m.Intersections)
            {
                this.Print(intersection.RowNumber, intersection.ColumnNumber, '+');
            }
        }
    }
}
