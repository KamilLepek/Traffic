using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.Vehicles;
using Traffic.Utilities;

namespace Traffic
{
    class Traffic
    {

        static void Main(string[] args)
        {
            //ponizej syf, ktory służy tylko do przetestowania gównaa
            var zbysiu = new VehicleGenerator();
            var cars = zbysiu.generateRandomCars(10);
            foreach(var car in cars)
            {
                car.printStatistics();
            }
            Console.ReadKey();
        }
    }
}
