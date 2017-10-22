using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.World.Edges;
using Traffic.Utilities;

namespace Traffic.World.Vertices
{
    /// <summary>
    ///     Spawn/End Point
    /// </summary>
    public class EndPoint : AbstractVertex
    {

        public bool IsOccupied { get; set; } 

        public int Orientation {get; private set;} //0 -góra, 1- prawo, 2-dół, 3-lewo ,można zmienić na enuma prawilniej. Potrzebne do respienia auta w odpowiednim miejscu, pewnie można to lepiej ohandlować bez tego

        public Street Street { get; private set; }

        public EndPoint(Street street)
        {
            this.IsOccupied = false;
            this.Street = street;
            this.SetRowAndColumn(street);
        }

        private void SetRowAndColumn (Street street)
        {
            if (street.IsVertical)
            {
                this.ColumnNumber = street.ColumnNumber;
                this.RowNumber = street.RowNumber == 1 ? 0 : street.RowNumber + 1;
                this.Orientation = street.RowNumber == 1 ? 0 : 2;
            }
            else
            {
                this.RowNumber = street.RowNumber;
                this.ColumnNumber = street.ColumnNumber == 1 ? 0 : street.ColumnNumber + 1;
                this.Orientation = street.ColumnNumber == 1 ? 3 : 1;
            }
        }
    }
}
