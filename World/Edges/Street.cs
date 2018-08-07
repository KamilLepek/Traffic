using System;
using System.Collections.Generic;
using OpenTK;
using Traffic.Utilities;
using Traffic.World.Vertices;

namespace Traffic.World.Edges
{
    /// <summary>
    /// Represents edges in our Graph
    /// </summary>
    public class Street : WorldObject
    {
        /// <summary>
        /// Vertices that are bordering to this street
        /// </summary>
        public List<AbstractVertex> Edges { get; }

        public bool IsVertical { get; }

        public Street(int row, int column, bool type) : base (row, column)
        {
            this.IsVertical = type;
            this.Edges = new List<AbstractVertex>();
        }

        public override Vector2 GetCoordinates()
        {
            var coordinates = new Vector2();
            if (this.IsVertical)
            {
                coordinates.X = (float)(Math.Ceiling((this.ColumnNumber - 1) / 2f) * Constants.StreetLength
                                        + (Math.Ceiling((this.ColumnNumber - 1) / 2f) - 0.5) * Constants.IntersectionSize);
                coordinates.Y = (float)(Math.Ceiling((this.RowNumber - 1) / 2f) * Constants.StreetLength
                                        + Math.Ceiling((this.RowNumber - 1) / 2f) * Constants.IntersectionSize);
            }
            else
            {
                coordinates.X = (float)(Math.Ceiling((this.ColumnNumber - 1) / 2f) * Constants.StreetLength
                                        + Math.Ceiling((this.ColumnNumber - 1) / 2f) * Constants.IntersectionSize);
                coordinates.Y = (float)(Math.Ceiling((this.RowNumber - 1) / 2f) * Constants.StreetLength
                                        + (Math.Ceiling((this.RowNumber - 1) / 2f) - 0.5) * Constants.IntersectionSize);
            }
            return coordinates;
        }
    }
}
