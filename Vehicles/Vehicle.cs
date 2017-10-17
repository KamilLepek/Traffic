using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Traffic
{
    public abstract class Vehicle
    {

        /// <summary>
        /// maximum velocity in km/h
        /// </summary>
        public float maximumVelocity { get; private set; }

        /// <summary>
        /// reaction time in ms
        /// </summary>
        protected float reactionTime { get; private set; }

        /// <summary>
        /// distance held in m
        /// </summary>
        protected float distanceHeld { get; private set; }

        //TODO: spawn point? destination point?

        protected Vehicle(float v, float t, float dist)
        {
            this.maximumVelocity = v;
            this.reactionTime = t;
            this.distanceHeld = dist;
        }

        public virtual void printStatistics()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Predkosc maksymalna: {0} km/h", maximumVelocity);
            Console.WriteLine("Czas reakcji: {0} ms", reactionTime);
            Console.WriteLine("Zachowywana odleglosc: {0} m", distanceHeld);
        }
    }
}
