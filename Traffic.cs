using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.Vehicles;
using Traffic.Utilities;
using Traffic.World;
using Traffic.Graphics;

namespace Traffic
{
    class Traffic
    {

        static void Main(string[] args)
        {
            var world = new Map(9, 12);

            var hehe = new ConsolePreview(world);

            var mariuszPudzianPudzianowski = new VehicleGenerator(world);
            mariuszPudzianPudzianowski.GenerateRandomCars(1);

            Console.ReadKey();
        }
    }
}
