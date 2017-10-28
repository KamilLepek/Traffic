using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.Utilities;
using Traffic.World.Vertices;

namespace Traffic.Vehicles
{
    public class Car : Vehicle
    {

        public string RegistrationNumber {get; private set;}

        public Car(float v, float t, float dist, string n, EndPoint spawn, List<Decision> initialRoute) : 
            base(v, t, dist, spawn, initialRoute)
        {
            this.VehicleLenght = Constants.CarLength;
            this.VehicleWidth = Constants.CarWidth;
            this.RegistrationNumber = n;
            this.SetInitialPositionAndVelocityVector(spawn, Constants.CarLength, Constants.CarWidth);
        }

        public override void PrintStatistics()
        {
            base.PrintStatistics();
            Console.WriteLine("Numer Rejestracyjny: {0}", RegistrationNumber);
            Console.WriteLine("------------------------");
        }
    }
}
