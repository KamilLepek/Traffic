using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.World.Edges;

namespace Traffic.World.Vertices
{
    public class Intersection : AbstractVertex
    {

        /// <summary>
        /// Streets that come out of this intersection
        /// </summary>
        public List<Street> IntersectingStreets { get; private set; }

        public Intersection(int row, int column) : base (row, column)
        {
            IntersectingStreets = new List<Street>();
        }
    }
}
