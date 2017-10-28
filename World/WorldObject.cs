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

        public WorldObject()
        {

        }

        public WorldObject(int row, int column)
        {
            this.RowNumber = row;
            this.ColumnNumber = column;
        }
    }
}
