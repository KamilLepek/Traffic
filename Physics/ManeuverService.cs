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
                veh.Maneuver != Maneuver.ForwardOnIntersect && veh.Maneuver != Maneuver.TurnLeft &&
                 veh.Maneuver != Maneuver.TurnRight)
            {
                var decision = veh.Route.FirstOrDefault();
                veh.Maneuver = UnitConverter.DecisionToManeuver(decision);
                if (decision == Decision.Left || decision == Decision.Right)
                {
                    veh.InitialTurningDirection = new Point(veh.FrontVector.X, veh.FrontVector.Y);
                }
                veh.Route.Remove(decision);
                return true;
            }
            return false;
        }

        public bool CheckIfVehicleLeftIntersection(Vehicle veh)
        {
            if (!(veh.Place is Intersection) &&
                (veh.Maneuver == Maneuver.ForwardOnIntersect || veh.Maneuver == Maneuver.TurnLeft ||
                 veh.Maneuver == Maneuver.TurnRight))
            {
                veh.Maneuver = Maneuver.None;
                return true;
            }
            return false;
        }

        public bool CheckIfVehicleLeftMiddleOfIntersection(Vehicle veh)
        {
            if (veh.Place is Intersection &&
                veh.Position.DistanceFrom(new Point(0, 0)) >=
                Constants.TurnStartingPoint * Constants.IntersectionSize &&
                (veh.Maneuver == Maneuver.TurnLeft ||
                 veh.Maneuver == Maneuver.TurnRight))
            {
                veh.Maneuver = Maneuver.CorrectAfterTurning;
                veh.TurningArcRadius = 0;
                veh.InitialTurningDirection = null;
                return true;
            }
            return false;
        }
    }
}
