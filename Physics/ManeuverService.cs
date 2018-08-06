using System;
using System.Linq;
using Traffic.Utilities;
using Traffic.Vehicles;
using Traffic.World.Edges;
using Traffic.World.Vertices;

namespace Traffic.Physics
{
    /// <summary>
    /// Handles maneuvers updates
    /// </summary>
    internal class ManeuverService
    {
        /// <summary>
        /// Checks if given vehicle has to avoid collision on street
        /// </summary>
        /// <param name="veh">given vehicle</param>
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

                    //Length of rectangle we look for the opponent vehicle, it scales with the velocityDeceleratingFactor and our velocity
                    double searchingRectangleLength =
                        vehiclesLength + veh.DistanceHeld + veh.VelocityVector.Length() * Constants.VelocityDependentCaution / Constants.TicksPerSecond +
                        velocityDeceleratingFactor * Constants.VelocityDifferenceDependentCaution / Constants.TicksPerSecond;

                    double actualDistanceBetweenVehicles = new Point(veh.Position.X - opponentVehicle.Position.X,
                        veh.Position.Y - opponentVehicle.Position.Y).Length() - vehiclesLength;

                    if (((Street)veh.Place).IsVertical)
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
                veh.VehicleInFrontOfUs = null;
            }
            return false;
        }

        /// <summary>
        /// Checks if given vehicle is approaching the end of street
        /// </summary>
        /// <param name="veh">given vehicle</param>
        public bool CheckIfVehicleIsApproachingEndOfStreet(Vehicle veh)
        {
            if (veh.Place is Street)
            {
                if (((Street)veh.Place).IsVertical)
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

        /// <summary>
        /// Checks if given vehicle has entered intersection
        /// </summary>
        /// <param name="veh">given vehicle</param>
        public bool CheckIfVehicleEnteredIntersection(Vehicle veh)
        {
            if (veh.Place is Intersection)
            {
                bool facesTowardsTheMiddle = (new Point(0, 0)).DistanceFrom(veh.Position.Add(veh.FrontVector))
                                             < (new Point(0, 0)).DistanceFrom(veh.Position.Subtract(veh.FrontVector));
                bool isNotInTheMiddleOfIntersection = veh.Position.DistanceFrom(new Point(0, 0)) >=
                                                      Constants.TurnStartingPoint * Constants.IntersectionSize;
                if (isNotInTheMiddleOfIntersection && facesTowardsTheMiddle)
                {
                    veh.Maneuver = Maneuver.DecelerateOnIntersection;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if given vehicle should execute decision on intersection
        /// </summary>
        /// <param name="veh">given vehicle</param>
        public bool CheckIfVehicleEnteredMiddleOfIntersection(Vehicle veh)
        {
            if (veh.Place is Intersection)
            {
                if (veh.Position.DistanceFrom(new Point(0, 0)) <
                    Constants.TurnStartingPoint * Constants.IntersectionSize)
                {
                    if (veh.Maneuver == Maneuver.ForwardOnIntersect || veh.Maneuver ==
                        Maneuver.TurnLeft || veh.Maneuver == Maneuver.TurnRight)
                        return true;
                    var decision = veh.Route.FirstOrDefault();
                    veh.Maneuver = UnitConverter.DecisionToManeuver(decision);
                    if (decision == Decision.Left || decision == Decision.Right)
                    {
                        veh.InitialTurningDirection = new Point(veh.FrontVector.X, veh.FrontVector.Y);
                    }
                    veh.Route.Remove(decision);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if given vehicle is leaving middle of intersection
        /// </summary>
        /// <param name="veh">given vehicle</param>
        public bool CheckIfVehicleLeftMiddleOfIntersection(Vehicle veh)
        {
            if (veh.Place is Intersection)
            {
                bool doesntFaceTowardsTheMiddle = (new Point(0, 0)).DistanceFrom(veh.Position.Add(veh.FrontVector))
                                             > (new Point(0, 0)).DistanceFrom(veh.Position.Subtract(veh.FrontVector));
                bool isNotInTheMiddleOfIntersection = veh.Position.DistanceFrom(new Point(0, 0)) >=
                                                      Constants.TurnStartingPoint * Constants.IntersectionSize;
                if (doesntFaceTowardsTheMiddle && isNotInTheMiddleOfIntersection)
                {
                    if (veh.Maneuver == Maneuver.CorrectAfterTurning)
                    {
                        veh.Maneuver = Maneuver.Accelerate;
                        return true;
                    }

                    veh.Maneuver = Maneuver.CorrectAfterTurning;
                    veh.TurningArcRadius = 0;
                    veh.InitialTurningDirection = null;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if given vehicle has to stop on lights
        /// </summary>
        /// <param name="veh">given vehicle</param>
        public bool CheckIfVehicleHasToStopOnLights(Vehicle veh)
        {
            if (veh.Place is Intersection)
                return false;
                

            var nextIntersection = veh.GetNextIntersection();
            if (nextIntersection == null)
                return false;

            if (nextIntersection.GetTrafficLight(
                    UnitConverter.IdealFrontVectorToOrentation(veh.FrontVector.GetDesiredDirection())) == Light.Red)
            {
                if (veh.Maneuver == Maneuver.StopOnLights)
                    return true;

                double velocity = veh.VelocityVector.Length();
                if (veh.GetDistanceToEndOfStreet() <= 1.5 * velocity * velocity / Constants.MinTrafficLightsDeceleration &&
                    veh.GetDistanceToEndOfStreet() >= 1.5 * velocity * velocity / Constants.MaxTrafficLightsDeceleration)
                {
                    veh.Maneuver = Maneuver.StopOnLights;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if given vehicle has to wait on intersection entrance
        /// </summary>
        /// <param name="veh">given vehicle</param>
        public bool CheckIfVehicleHasToWaitOnIntersectionEntrance(Vehicle veh)
        {
            if (veh.Place is Street && veh.Route.Any())
            {
                if (veh.Route.First() == Decision.Left)
                {
                    var nextIntersection = veh.GetNextIntersection();
                    if (nextIntersection == null)
                        return false;

                    if (nextIntersection.GetTrafficLight(
                            UnitConverter.IdealFrontVectorToOrentation(veh.FrontVector.GetDesiredDirection())) == Light.Green)
                    {
                        double rectangleLength = veh.VehicleLength * Constants.VehicleLengthSearchingDependantFactor + Constants.IntersectionSize;

                        if (rectangleLength > veh.GetDistanceToEndOfStreet() + Constants.IntersectionSize) // Does the rectangle catches next street at all?
                        {
                            var idealFrontVector = veh.FrontVector.GetDesiredDirection();

                            var oppositeStreet = nextIntersection.IntersectingStreets.Find(item =>
                                (item.RowNumber == (nextIntersection.RowNumber + idealFrontVector.Y)) &&
                                (item.ColumnNumber == (nextIntersection.ColumnNumber + idealFrontVector.X)));

                            var oppositeOrientationThanVeh = UnitConverter.OppositeOrientation(
                                UnitConverter.IdealFrontVectorToOrentation(veh.FrontVector.GetDesiredDirection()));

                            if (!oppositeStreet.Vehicles.Any())
                                return false;

                            var oppositeVehicles = oppositeStreet.Vehicles.FindAll(item =>
                                UnitConverter.IdealFrontVectorToOrentation(item.FrontVector.GetDesiredDirection()) ==
                                oppositeOrientationThanVeh);

                            if (!oppositeVehicles.Any())
                                return false;

                            //Handle situation when the opposite vehicle is already waiting to enter intersection
                            //but the vehicle behind him is going right so that we are forced to wait anyway
                            //In order to avoid eternal freeze:
                            var oppositeVehiclesSorted = oppositeVehicles.OrderBy(item => item.GetDistanceToEndOfStreet()).ToList();
                            if (oppositeVehiclesSorted.First().Maneuver == Maneuver.WaitToEnterIntersection)
                                return false;

                            //check first 3 vehicles on opposite street
                            int amountOfVehicles = Math.Min(3, oppositeVehiclesSorted.Count);
                            for (int i = 0; i < amountOfVehicles; i++)
                            {
                                if (oppositeVehiclesSorted[i].Route.First() != Decision.Left
                                ) // Does he drive forward or right at all?
                                {
                                    if (oppositeVehiclesSorted[i].GetDistanceToEndOfStreet() < rectangleLength -
                                        Constants.IntersectionSize -
                                        veh.GetDistanceToEndOfStreet()) // Is he inside the rectangle?
                                    {
                                        veh.Maneuver = Maneuver.WaitToEnterIntersection;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;

        }
    }
}
