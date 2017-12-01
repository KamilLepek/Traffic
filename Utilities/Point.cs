using System;
using System.Collections.Generic;
using System.Linq;

namespace Traffic.Utilities
{
    public class Point
    {

        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Returns the angle in degrees by which the direction vector is rotated over vertical axis (Z axis)
        /// </summary>
        /// <returns>Angle in degrees</returns>
        public double GetRotationAngle()
        {
            if (this.Y == 0) // we can't compute atan, it's either 90 or 270 degrees
            {
                if (this.X > 0)
                    return 90.0;
                else
                    return 270.0;
            }
            double angle = Math.Atan(this.X / this.Y);

            if (this.Y < 0) // computing tan loses information about signs of X and Y
                angle += Math.PI;
            if (angle < 0) // Math.Atan returns angle from -90 to 90, need to get rid of negatives
                angle += 2 * Math.PI;

            return angle * 180 / Math.PI;
        }

        /// <summary>
        /// Returns distance between this point and point p
        /// </summary>
        public double DistanceFrom(Point p)
        {
            return Math.Sqrt((p.X - this.X) * (p.X - this.X) + (p.Y - this.Y) * (p.Y - this.Y));
        }

        /// <summary>
        /// Returns length of this vector
        /// </summary>
        /// <returns></returns>
        public double Length()
        {
            return Math.Sqrt(this.X * this.X + this.Y * this.Y);
        }

        /// <summary>
        /// Returns angle in degrees between this vector and vector p
        /// </summary>
        public double AngleFrom(Point p)
        {
            var scalarProduct = this.X * p.X + this.Y * p.Y;
            var cos = scalarProduct / (this.Length() * p.Length());
            cos = cos > 1 ? 1 : cos;
            cos = cos < -1 ? -1 : cos;
            return Math.Acos(cos) * 180 / Math.PI;
        }

        /// <summary>
        /// Returns vector with length adjusted to length param
        /// </summary>
        /// <returns></returns>
        public void ChangeLengthOfVector(double length)
        {
            this.X *= (length / this.Length());
            this.Y *= (length / this.Length());
        }

        /// <summary>
        /// Returns the closest in terms of angle horizontal or vertical direction
        /// </summary>
        public Point GetDesiredDirection()
        {
            var acceptedDirections = new List<Point>()
            {
                new Point(1,0),
                new Point(-1,0),
                new Point(0,1),
                new Point(0,-1)
            };
            return acceptedDirections.First(d =>
                d.AngleFrom(this) == acceptedDirections.Min(p => p.AngleFrom(this)));
        }

        /// <summary>
        /// Returns sum of this vector and vector p
        /// </summary>
        public Point Add(Point p)
        {
            return new Point(this.X + p.X, this.Y + p.Y);
        }

        /// <summary>
        /// Returns difference of this vector and vector p
        /// </summary>
        public Point Subtract(Point p)
        {
            return new Point(this.X - p.X, this.Y - p.Y);
        }
    }
}
