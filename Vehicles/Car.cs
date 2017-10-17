using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic.Vehicles
{
    public class Car : Vehicle
    {

        public string registrationNumber {get; private set;}

        public Car(float v, float t, float dist, string n) : base(v, t, dist)
        {
            this.registrationNumber = n;
        }

        public override void PrintStatistics()
        {
            base.PrintStatistics();
            Console.WriteLine("Numer Rejestracyjny: {0}", registrationNumber);
            Console.WriteLine("------------------------");
        }
    }
}
