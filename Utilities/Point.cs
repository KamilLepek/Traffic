using System;

namespace Traffic.Utilities
{
    public class Point
    {

        public float X { get; set; }
        public float Y { get; set; }

        public Point(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Returns the angle in degrees by which the direction vector is rotated over vertical axis (Z axis)
        /// </summary>
        /// <returns>Angle in degrees</returns>
        public float GetRotationAngle()
        {
            if (this.Y == 0) // we can't compute atan, it's either 90 or 270 degrees
            {
                if (this.X > 0)
                    return 90.0f;
                else
                    return 270.0f;
            }
            double angle = Math.Atan(this.X / this.Y);

            if (this.Y < 0) // computing tan loses information about signs of X and Y
                angle += Math.PI;
            if (angle < 0) // Math.Atan returns angle from -90 to 90, need to get rid of negatives
                angle += 2 * Math.PI;

            return (float)(angle * 180 / Math.PI);
        }
    }
}
