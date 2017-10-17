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
            //usuniety brzydki komentarz
            var rysiu = new VehicleGenerator();
            var cars = rysiu.generateRandomCars(10);
            foreach(var car in cars)
            {
                car.PrintStatistics();
            }
            Console.ReadKey();
        }
    }
}
