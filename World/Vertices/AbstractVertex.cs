﻿namespace Traffic.World.Vertices
{
    /// <summary>
    /// Represents vertex on the Map (intersection or spawn/end point)
    /// </summary>
    public abstract class AbstractVertex : WorldObject
    {

        public AbstractVertex()
        {

        }

        public AbstractVertex(int row, int column) : base (row, column)
        {

        }
    }
}
