using System;
using System.Collections.Generic;
using OpenTK;
using Traffic.Vehicles;

namespace Traffic.World
{
    /// <summary>
    /// Represents an object on the map (Edge or Vertex of our Graph)
    /// </summary>
    public abstract class WorldObject
    {
        /// <summary>
        /// Coordinates on the map
        /// </summary>
        public int RowNumber { get; protected set; }
        public int ColumnNumber { get; protected set; }

        public List<Vehicle> Vehicles { get; protected set; }

        protected WorldObject()
        {
        }

        protected WorldObject(int row, int column)
        {
            this.RowNumber = row;
            this.ColumnNumber = column;
            this.Vehicles = new List<Vehicle>();
        }

        public virtual Vector2 GetCoordinates()
        {
            throw new NotImplementedException();
        }
    }
}
