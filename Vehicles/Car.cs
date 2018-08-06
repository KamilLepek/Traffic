using System;
using System.Collections.Generic;
using Traffic.Utilities;
using Traffic.World.Vertices;

namespace Traffic.Vehicles
{
    /// <summary>
    /// Implementation of specified Vehicle: Car
    /// </summary>
    public class Car : Vehicle
    {

        public string RegistrationNumber {get; private set;}

        public Car(double v, double t, double dist, string n, EndPoint spawn, List<Decision> initialRoute, EndPoint finishPoint, int textureAssigned) :
            base(v, t, dist, spawn, initialRoute, finishPoint, textureAssigned)
        {
            this.VehicleLength = Constants.CarLength;
            this.VehicleWidth = Constants.CarWidth;
            this.RegistrationNumber = n;
            this.SetInitialPositionAndVelocityVector(spawn, Constants.CarLength, Constants.CarWidth);
        }

        /// <summary>
        /// Prints Car Statistics
        /// </summary>
        public override void PrintStatistics()
        {
            base.PrintStatistics();
            Console.WriteLine("Numer Rejestracyjny: {0}", this.RegistrationNumber);
            Console.WriteLine("------------------------");
        }
    }
}
