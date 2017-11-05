using System.Linq;
using Traffic.Utilities;
using Traffic.Vehicles;
using Traffic.World.Vertices;

namespace Traffic.Physics
{
    internal class ManeuverService
    {
        public bool CheckIfVehicleEnteredIntersection(Vehicle veh)
        {
            // TODO Jakieś hamowanie pomiędzy wjazdem na skrzyżowanie a rozpoczęciem skręcania?
            return false;
        }

        public bool CheckIfVehicleEnteredMiddleOfIntersection(Vehicle veh)
        {
            if (veh.Place is Intersection &&
                veh.Position.DistanceFrom(new Point(0, 0)) < Constants.TurnStartingPoint * Constants.IntersectionSize &&
                veh.Maneuver != Maneuvers.ForwardOnIntersect && veh.Maneuver != Maneuvers.TurnLeft &&
                 veh.Maneuver != Maneuvers.TurnRight)
            {
                var decision = veh.Route.FirstOrDefault();
                switch (decision)
                {
                    case Decision.Forward:
                        veh.Maneuver = Maneuvers.ForwardOnIntersect;
                        break;
                    case Decision.Left:
                        veh.Maneuver = Maneuvers.TurnLeft;
                        veh.InitialTurningDirection = new Point(veh.FrontVector.X, veh.FrontVector.Y);
                        break;
                    case Decision.Right:
                        veh.Maneuver = Maneuvers.TurnRight;
                        veh.InitialTurningDirection = new Point(veh.FrontVector.X, veh.FrontVector.Y);
                        break;
                }
                veh.Route.Remove(decision);
                return true;
            }
            return false;
        }

        public bool CheckIfVehicleLeftIntersection(Vehicle veh)
        {
            if (!(veh.Place is Intersection) &&
                (veh.Maneuver == Maneuvers.ForwardOnIntersect || veh.Maneuver == Maneuvers.TurnLeft ||
                 veh.Maneuver == Maneuvers.TurnRight))
            {
                veh.Maneuver = Maneuvers.None;
                return true;
            }
            return false;
        }

        public bool CheckIfVehicleLeftMiddleOfIntersection(Vehicle veh)
        {
            if (veh.Place is Intersection &&
                veh.Position.DistanceFrom(new Point(0, 0)) >=
                Constants.TurnStartingPoint * Constants.IntersectionSize &&
                (veh.Maneuver == Maneuvers.TurnLeft ||
                 veh.Maneuver == Maneuvers.TurnRight))
            {
                veh.Maneuver = Maneuvers.CorrectAfterTurning;
                veh.TurningArcRadius = 0;
                veh.InitialTurningDirection = null;
                return true;
            }
            return false;
        }
    }
}
