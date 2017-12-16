using System.Collections.Generic;
using Traffic.World.Vertices;

namespace Traffic.World.Edges
{
    /// <summary>
    /// Represents edges in our Graph
    /// </summary>
    public class Street : WorldObject
    {

        public List<AbstractVertex> Edges { get; private set; }

        public bool IsVertical { get; private set; }

        public Street(int row, int column, bool type) : base (row, column)
        {
            this.IsVertical = type;
            this.Edges = new List<AbstractVertex>();
        }

        public Street()
        {
            this.IsVertical = true;
        }
    }
}
