using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic.Exceptions
{
    public class NoUnoccupiedSpawnException : Exception
    {
        public NoUnoccupiedSpawnException(string message) : base(message)
        {

        }
    }
}
