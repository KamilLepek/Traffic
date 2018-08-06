using System;

namespace Traffic.Exceptions
{
    /// <summary>
    /// Exception thrown if there are no free spawns to spawn vehicles
    /// </summary>
    public class NoUnoccupiedSpawnException : Exception
    {
        public NoUnoccupiedSpawnException(string message) : base(message)
        {

        }
    }
}
