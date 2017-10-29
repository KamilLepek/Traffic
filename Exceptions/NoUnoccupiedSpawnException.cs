using System;

namespace Traffic.Exceptions
{
    public class NoUnoccupiedSpawnException : Exception
    {
        public NoUnoccupiedSpawnException(string message) : base(message)
        {

        }
    }
}
