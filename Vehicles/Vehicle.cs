using System;
using System.Collections.Generic;
using Traffic.Utilities;
using Traffic.World;
using Traffic.World.Edges;
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
        public double MaximumVelocity { get; private set; }

        /// <summary>
        /// reaction time in ms
        /// </summary>
        public double ReactionTime { get; private set; }

        /// <summary>
        /// minimum distance held in m
        /// </summary>
        public double DistanceHeld { get; private set; }

        public double VehicleLength { get; protected set; }

        public double VehicleWidth { get; protected set; }
 
        public Point VelocityVector { get; set; }

        public Point AccelerationVector { get; set; }

        /// <summary>
        /// List of decisions to make to finish the race
        /// </summary>
        public List<Decision> Route { get; protected set; }

        public Maneuver Maneuver { get; set; }

        public Vehicle VehicleInFrontOfUs { get; set; }

        /// <summary>
        /// If the vehicle is turning, it's the middle point of the arc. Else it's null
        /// </summary>
        public double TurningArcRadius { get; set; }

        public Point InitialTurningDirection { get; set; }

        public int TextureAssigned { get; private set; }

        protected Vehicle(double v, double t, double dist, EndPoint spawnPlace, List<Decision> initialRoute, EndPoint finishPoint, int textureAssigned)
        {
            this.Place = spawnPlace;
            spawnPlace.IsOccupied = true;
            this.MaximumVelocity = v;
            this.ReactionTime = t;
            this.DistanceHeld = dist;
            this.FinishPoint = finishPoint;
            this.Route = initialRoute;
            this.AccelerationVector = new Point(0,0);
            this.Maneuver = Maneuver.Accelerate;
            this.TextureAssigned = textureAssigned;
            this.VehicleInFrontOfUs = null;
        }

        public virtual void PrintStatistics()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Predkosc maksymalna: {0} km/h", this.MaximumVelocity);
            Console.WriteLine("Czas reakcji: {0} ms", this.ReactionTime);
            Console.WriteLine("Zachowywana odleglosc: {0} m", this.DistanceHeld);
        }

        protected void SetInitialPositionAndVelocityVector(EndPoint spawnPlace, double vehicleLength, double vehicleWidth, double value = Constants.InitialVelocity)
        {
            switch (spawnPlace.Orient)
            {
                case Orientation.Top:
                    this.Position = new Point(-Constants.StreetWidth / 4, vehicleLength / 2);
                    this.VelocityVector = new Point(0, Constants.InitialVelocity);
                    this.FrontVector = new Point(0, 1);
                    break;
                case Orientation.Right:
                    this.Position = new Point(Constants.StreetLength - vehicleLength / 2, - Constants.StreetWidth / 4);
                    this.VelocityVector = new Point(-Constants.InitialVelocity, 0);
                    this.FrontVector = new Point(-1, 0);
                    break;
                case Orientation.Bottom:
                    this.Position = new Point(Constants.StreetWidth / 4, Constants.StreetLength - vehicleLength / 2);
                    this.VelocityVector = new Point(0, -Constants.InitialVelocity);
                    this.FrontVector = new Point(0, -1);
                    break;
                case Orientation.Left:
                    this.Position = new Point(vehicleLength / 2, Constants.StreetWidth / 4);
                    this.VelocityVector = new Point(Constants.InitialVelocity, 0);
                    this.FrontVector = new Point(1, 0);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// If vehicle is on intersection, method returns current intersection.
        /// If the vehicle is on the street it returns reference to the next intersection the vehicle will enter. If such intersection doesn't exist, return value is null
        /// If the vehicle is on endpoint, return value is null
        /// </summary>
        /// <returns></returns>
        public Intersection GetNextIntersection()
        {
            if (this.Place is Intersection)
                return (Intersection) this.Place;

            if (this.Place is EndPoint)
                return null;

            var idealFrontVector = this.FrontVector.GetDesiredDirection();
            var nextVertex = ((Street)this.Place).Edges.Find(item => (item.RowNumber == this.Place.RowNumber + (int)idealFrontVector.Y)
                                                                     && (item.ColumnNumber == this.Place.ColumnNumber + (int)idealFrontVector.X));
            return nextVertex as Intersection;
        }

        /// <summary>
        /// Returns distance in meters which car has to drive to get to the end of the street
        /// </summary>
        /// <returns></returns>
        public double GetDistanceToEndOfStreet()
        {
            var orientation = UnitConverter.IdealFrontVectorToOrentation(this.FrontVector.GetDesiredDirection());

            switch (orientation)
            {
                case Orientation.Bottom:
                    return Constants.StreetLength - this.Position.Y - Constants.CarLength / 2;
                case Orientation.Top:
                    return this.Position.Y - Constants.CarLength / 2;
                case Orientation.Right:
                    return Constants.StreetLength - this.Position.X - Constants.CarLength / 2;
                case Orientation.Left:
                    return this.Position.X - Constants.CarLength / 2;
            }
            throw new InvalidOperationException();
        }
    }
}
