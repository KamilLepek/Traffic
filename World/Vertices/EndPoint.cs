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

        /// <summary>
        /// determines side of the map where the spawn point exist
        /// </summary>
        public Orientation Orient {get; private set;} 

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
                this.Orient = street.RowNumber == 1 ? Orientation.Top : Orientation.Bottom;
            }
            else
            {
                this.RowNumber = street.RowNumber;
                this.ColumnNumber = street.ColumnNumber == 1 ? 0 : street.ColumnNumber + 1;
                this.Orient = street.ColumnNumber == 1 ? Orientation.Left : Orientation.Right;
            }
        }
    }
}
