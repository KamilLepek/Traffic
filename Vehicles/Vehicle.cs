using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
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
        /// coordinates of vehicle
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// maximum velocity in km/h
        /// </summary>
        protected float MaximumVelocity { get; private set; }

        /// <summary>
        /// reaction time in ms
        /// </summary>
        protected float ReactionTime { get; private set; }

        /// <summary>
        /// minimum distance held in m
        /// </summary>
        protected float DistanceHeld { get; private set; }

        //TODO: spawn point? destination point?

        protected Vehicle(float v, float t, float dist, EndPoint spawnPlace)
        {
            this.Place = spawnPlace;
            spawnPlace.IsOccupied = true;
            this.MaximumVelocity = v;
            this.ReactionTime = t;
            this.DistanceHeld = dist;
        }

        public virtual void PrintStatistics()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Predkosc maksymalna: {0} km/h", MaximumVelocity);
            Console.WriteLine("Czas reakcji: {0} ms", ReactionTime);
            Console.WriteLine("Zachowywana odleglosc: {0} m", DistanceHeld);
        }
    }
}
