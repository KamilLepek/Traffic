using System;

namespace Traffic.Utilities
{
    public static class UnitConverter
    {
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
