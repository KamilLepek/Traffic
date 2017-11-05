using System;
using System.Collections.Generic;
using Traffic.Utilities;
using Traffic.World;
using Traffic.World.Vertices;

namespace Traffic.Vehicles
{
    public abstract class Vehicle
    {
        /// <summary>
        /// Object the vehicle is on (Spawn/Street/Intersection)
        /// </summary>
        public WorldObject Place { get; set; }

        /// <summary>
        /// Finish point of the vehicle, will be needed to determine in-flight route optimalization
        /// </summary>
        private readonly EndPoint FinishPoint;

        /// <summary>
        /// coordinates of vehicle on World Object
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// vector that determines where is the front of the vehicle
        /// </summary>
        public Point FrontVector { get; set; }

        /// <summary>
        /// maximum velocity in km/h
        /// </summary>
        protected double MaximumVelocity { get; private set; }

        /// <summary>
        /// reaction time in ms
        /// </summary>
        protected double ReactionTime { get; private set; }

        /// <summary>
        /// minimum distance held in m
        /// </summary>
        protected double DistanceHeld { get; private set; }

        public double VehicleLenght { get; protected set; }

        public double VehicleWidth { get; protected set; }
 
        public Point VelocityVector { get; set; }

        public Point AccelerationVector { get; set; }

        /// <summary>
        /// List of decisions to make to finish the race
        /// </summary>
        public List<Decision> Route { get; protected set; }

        public Maneuver Maneuver { get; set; }

        /// <summary>
        /// If the vehicle is turning, it's the middle point of the arc. Else it's null
        /// </summary>
        public double TurningArcRadius { get; set; }

        public Point InitialTurningDirection { get; set; }

        protected Vehicle(double v, double t, double dist, EndPoint spawnPlace, List<Decision> initialRoute, EndPoint finishPoint)
        {
            this.Place = spawnPlace;
            spawnPlace.IsOccupied = true;
            this.MaximumVelocity = v;
            this.ReactionTime = t;
            this.DistanceHeld = dist;
            this.FinishPoint = finishPoint;
            this.Route = initialRoute;
            this.AccelerationVector = new Point(0,0);
            this.Maneuver = Maneuver.None;
        }

        public virtual void PrintStatistics()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Predkosc maksymalna: {0} km/h", MaximumVelocity);
            Console.WriteLine("Czas reakcji: {0} ms", ReactionTime);
            Console.WriteLine("Zachowywana odleglosc: {0} m", DistanceHeld);
        }

        protected void SetInitialPositionAndVelocityVector(EndPoint spawnPlace, double vehicleLenght, double vehicleWidth, double value = Constants.InitialVelocity)
        {
            switch (spawnPlace.Orient)
            {
                case Orientation.Top:
                    this.Position = new Point(-Constants.StreetWidth / 4, vehicleLenght / 2);
                    this.VelocityVector = new Point(0, Constants.InitialVelocity);
                    this.FrontVector = new Point(0, 1);
                    break;
                case Orientation.Right:
                    this.Position = new Point(Constants.StreetLength - vehicleLenght / 2, - Constants.StreetWidth / 4);
                    this.VelocityVector = new Point(-Constants.InitialVelocity, 0);
                    this.FrontVector = new Point(-1, 0);
                    break;
                case Orientation.Bottom:
                    this.Position = new Point(Constants.StreetWidth / 4, Constants.StreetLength - vehicleLenght / 2);
                    this.VelocityVector = new Point(0, -Constants.InitialVelocity);
                    this.FrontVector = new Point(0, -1);
                    break;
                case Orientation.Left:
                    this.Position = new Point(vehicleLenght / 2, Constants.StreetWidth / 4);
                    this.VelocityVector = new Point(Constants.InitialVelocity, 0);
                    this.FrontVector = new Point(1, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
