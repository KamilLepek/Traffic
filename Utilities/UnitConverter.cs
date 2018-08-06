using System;

namespace Traffic.Utilities
{
    /// <summary>
    /// Class determining unit conversions in simulation
    /// </summary>
    public static class UnitConverter
    {
        /// <summary>
        /// Converts orientation we're facing to diffrence in rows and columns to the next game object (the one that we're facing)
        /// </summary>
        /// <param name="or">Orientation that the driver is facing</param>
        /// <param name="horizontal">Horizontal diffrence with the game object that the dirver is facing</param>
        /// <param name="vertical">Vertical diffrence with the game object that the dirver is facing</param>
        public static void OrientationToRowColumnDiffrence(Orientation or, ref int horizontal, ref int vertical)
        {
            switch (or)
            {
                case Orientation.Bottom:
                    horizontal = 0;
                    vertical = 1;
                    break;
                case Orientation.Left:
                    horizontal = -1;
                    vertical = 0;
                    break;
                case Orientation.Right:
                    horizontal = 1;
                    vertical = 0;
                    break;
                case Orientation.Top:
                    horizontal = 0;
                    vertical = -1;
                    break;
                default:
                    horizontal = 0;
                    vertical = 0;
                    break;
            }
        }

        /// <summary>
        /// Converts front vector to orientation that it faces
        /// </summary>
        /// <param name="frontVector">Front vector to approximate</param>
        /// <returns>Orientation the front vector is facing</returns>
        public static Orientation IdealFrontVectorToOrentation(Point frontVector)
        {
            if (frontVector.X == 1 && frontVector.Y == 0)
                return Orientation.Right;
            if (frontVector.X == 0 && frontVector.Y == 1)
                return Orientation.Bottom;
            if (frontVector.X == -1 && frontVector.Y == 0)
                return Orientation.Left;
            if (frontVector.X == 0 && frontVector.Y == -1)
                return Orientation.Top;

            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Converts decision on the next intersection to proper maneuver
        /// </summary>
        /// <param name="decision">Decision on next intersection</param>
        /// <returns>Maneuver to execute</returns>
        public static Maneuver DecisionToManeuver(Decision decision)
        {
            switch (decision)
            {
                case Decision.Forward:
                    return Maneuver.ForwardOnIntersect;
                case Decision.Left:
                    return Maneuver.TurnLeft;
                case Decision.Right:
                    return Maneuver.TurnRight;
            }
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Returns opposite orientation for given orientation
        /// </summary>
        /// <param name="orientation">Given orientation</param>
        /// <returns>Opposite orientation</returns>
        public static Orientation OppositeOrientation(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Bottom:
                    return Orientation.Top;
                case Orientation.Top:
                    return Orientation.Bottom;
                case Orientation.Left:
                    return Orientation.Right;
                case Orientation.Right:
                    return Orientation.Left;
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
