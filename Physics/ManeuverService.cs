using System.Linq;
using Traffic.Utilities;
using Traffic.Vehicles;
using Traffic.World.Edges;
using Traffic.World.Vertices;

namespace Traffic.Physics
{
    internal class ManeuverService
    {
        public bool CheckIfVehicleHasToAvoidCollisionOnStreet(Vehicle veh)
        {
            if (veh.Place is Street)
            {
                foreach (var opponentVehicle in veh.Place.Vehicles)
                {
                    //Since we measure position of vehicle from its center this is the sum of half vehicle lengths
                    double vehiclesLength = veh.VehicleLength / 2 + opponentVehicle.VehicleLength / 2; 

                    //Diffrence of velocities or 1 if its negative
                    double velocityDeceleratingFactor =
                        veh.VelocityVector.Length() > opponentVehicle.VelocityVector.Length()
                            ? veh.VelocityVector.Length() - opponentVehicle.VelocityVector.Length()
                            : 1;

                    //Lenght of rectangle we look for the opponent vehicle, it scales with the velocityDeceleratingFactor and our velocity
                    double searchingRectangleLength =
                        vehiclesLength + veh.DistanceHeld + veh.VelocityVector.Length() * Constants.VelocityDependentCaution / Constants.TicksPerSecond +
                        velocityDeceleratingFactor * Constants.VelocityDifferenceDependentCaution / Constants.TicksPerSecond;

                    double actualDistanceBetweenVehicles = new Point(veh.Position.X - opponentVehicle.Position.X,
                        veh.Position.Y - opponentVehicle.Position.Y).Length() - vehiclesLength;

                    if (((Street) veh.Place).IsVertical)
                    {
                        if (opponentVehicle.Position.X < veh.Position.X + veh.VehicleWidth / 2 && opponentVehicle.Position.X > veh.Position.X - veh.VehicleWidth / 2)
                        {
                            if (veh.FrontVector.Y > 0)
                            {
                                if (opponentVehicle.Position.Y < veh.Position.Y + searchingRectangleLength && opponentVehicle.Position.Y > veh.Position.Y)
                                {
                                    if (veh.VelocityVector.Length() > opponentVehicle.VelocityVector.Length() || actualDistanceBetweenVehicles < veh.DistanceHeld)
                                    {
                                        veh.Maneuver = Maneuver.AvoidCollision;
                                        veh.VehicleInFrontOfUs = opponentVehicle;
                                        return true;
                                    }
                                }
                            }
                            else if (veh.FrontVector.Y < 0)
                            {
                                if (opponentVehicle.Position.Y > veh.Position.Y - searchingRectangleLength && opponentVehicle.Position.Y < veh.Position.Y)
                                {
                                    if (veh.VelocityVector.Length() > opponentVehicle.VelocityVector.Length() || actualDistanceBetweenVehicles < veh.DistanceHeld)
                                    {
                                        veh.Maneuver = Maneuver.AvoidCollision;
                                        veh.VehicleInFrontOfUs = opponentVehicle;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (opponentVehicle.Position.Y < veh.Position.Y + veh.VehicleWidth / 2 && opponentVehicle.Position.Y > veh.Position.Y - veh.VehicleWidth / 2)
                        {
                            if (veh.FrontVector.X > 0)
                            {
                                if (opponentVehicle.Position.X < veh.Position.X + searchingRectangleLength && opponentVehicle.Position.X > veh.Position.X)
                                {
                                    if (veh.VelocityVector.Length() > opponentVehicle.VelocityVector.Length() || actualDistanceBetweenVehicles < veh.DistanceHeld)
                                    {
                                        veh.Maneuver = Maneuver.AvoidCollision;
                                        veh.VehicleInFrontOfUs = opponentVehicle;
                                        return true;
                                    }
                                }
                            }
                            else if (veh.FrontVector.X < 0)
                            {
                                if (opponentVehicle.Position.X > veh.Position.X - searchingRectangleLength && opponentVehicle.Position.X < veh.Position.X)
                                {
                                    if (veh.VelocityVector.Length() > opponentVehicle.VelocityVector.Length() || actualDistanceBetweenVehicles < veh.DistanceHeld)
                                    {
                                        veh.Maneuver = Maneuver.AvoidCollision;
                                        veh.VehicleInFrontOfUs = opponentVehicle;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                veh.Maneuver = Maneuver.Accelerate;
                veh.VehicleInFrontOfUs = null;
            }
            return false;
        }

        public bool CheckIfVehicleIsApproachingEndOfStreet(Vehicle veh)
        {
            if (veh.Place is Street &&
                veh.Maneuver == Maneuver.Accelerate)
            {
                if (((Street) veh.Place).IsVertical)
                {
                    if ((veh.FrontVector.Y > 0 &&
                         veh.Position.Y > Constants.StreetLength * Constants.BreakStartingPoint) ||
                        (veh.FrontVector.Y < 0 &&
                        veh.Position.Y < Constants.StreetLength * (1 - Constants.BreakStartingPoint)))
                    {
                        veh.Maneuver = Maneuver.DecelerateOnStreet;
                        return true;
                    }
                }
                else
                {
                    if ((veh.FrontVector.X > 0 &&
                         veh.Position.X > Constants.StreetLength * Constants.BreakStartingPoint) ||
                        (veh.FrontVector.X < 0 &&
                         veh.Position.X < Constants.StreetLength * (1 - Constants.BreakStartingPoint)))
                    {
                        veh.Maneuver = Maneuver.DecelerateOnStreet;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckIfVehicleEnteredIntersection(Vehicle veh)
        {
            if (veh.Place is Intersection &&
                veh.Position.DistanceFrom(new Point(0, 0)) >=
                Constants.TurnStartingPoint * Constants.IntersectionSize &&
                (veh.Maneuver == Maneuver.DecelerateOnStreet || veh.Maneuver == Maneuver.AvoidCollision))
            {
                veh.Maneuver = Maneuver.DecelerateOnIntersection;
                return true;
            }
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
                (veh.Maneuver == Maneuver.ForwardOnIntersect || veh.Maneuver == Maneuver.CorrectAfterTurning))
            {
                veh.Maneuver = Maneuver.Accelerate;
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
