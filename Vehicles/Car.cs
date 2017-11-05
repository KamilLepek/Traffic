using System;
using System.Collections.Generic;
using Traffic.Utilities;
using Traffic.World.Vertices;

namespace Traffic.Vehicles
{
    public class Car : Vehicle
    {

        public string RegistrationNumber {get; private set;}

        public Car(double v, double t, double dist, string n, EndPoint spawn, List<Decision> initialRoute, EndPoint finishPoint) : 
            base(v, t, dist, spawn, initialRoute, finishPoint)
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
