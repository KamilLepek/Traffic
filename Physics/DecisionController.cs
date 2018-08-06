using System;
using Traffic.Utilities;
using Traffic.Vehicles;
using Traffic.World;

namespace Traffic.Physics
{
    /// <summary>
    /// Handles decisions of vehicles
    /// </summary>
    public class DecisionController
    {
        private readonly Map world;
        private readonly ManeuverService maneuverService;

        public DecisionController(Map world)
        {
            this.world = world;
            this.maneuverService = new ManeuverService();
        }

        /// <summary>
        /// Updates maneuvers and then computes acceleration for every vehicle
        /// </summary>
        public void HandleDecisions()
        {
            foreach (var vehicle in this.world.Vehicles)
            {
                this.UpdateManeuverIfNeccessary(vehicle);
                this.ComputeAcceleration(vehicle);
            }
        }

        /// <summary>
        /// Updates maneuvers property of given vehicle
        /// </summary>
        /// <param name="veh">given vehicle</param>
        private void UpdateManeuverIfNeccessary(Vehicle veh)
        {
            if (this.maneuverService.CheckIfVehicleHasToAvoidCollisionOnStreet(veh))
                return;
            if (this.maneuverService.CheckIfVehicleHasToStopOnLights(veh))
                return;
            if (this.maneuverService.CheckIfVehicleHasToWaitOnIntersectionEntrance(veh))
                return;
            if (this.maneuverService.CheckIfVehicleIsApproachingEndOfStreet(veh))
                return;
            if (this.maneuverService.CheckIfVehicleEnteredIntersection(veh))
                return;
            if (this.maneuverService.CheckIfVehicleEnteredMiddleOfIntersection(veh))
                return;
            if (this.maneuverService.CheckIfVehicleLeftMiddleOfIntersection(veh))
                return;
            veh.Maneuver = Maneuver.Accelerate;
        }

        /// <summary>
        /// Computes and sets the part of acceleration which is tangent to velocity, and is responsible for the velocity's direction change.
        /// Bases mainly on vehicle's Maneuver
        /// </summary>
        private void ComputeAcceleration(Vehicle veh)
        {
            switch (veh.Maneuver)
            {
                case Maneuver.AvoidCollision:
                    this.DecelerateToAvoidCollision(veh);
                    break;
                case Maneuver.DecelerateOnStreet:
                    this.ComputeDeceleration(veh, true);
                    break;
                case Maneuver.Accelerate:
                    this.ComputeAccelerationOnStraightRoad(veh);
                    break;
                case Maneuver.DecelerateOnIntersection:
                    this.ComputeDeceleration(veh, false);
                    break;
                case Maneuver.ForwardOnIntersect:
                    veh.AccelerationVector.X = 0;
                    veh.AccelerationVector.Y = 0;
                    break;
                case Maneuver.TurnLeft:
                    this.ComputeTurnAcceleration(veh, true);
                    break;
                case Maneuver.TurnRight:
                    this.ComputeTurnAcceleration(veh, false);
                    break;
                case Maneuver.CorrectAfterTurning:
                    this.ComputeCorrectionAcceleration(veh);
                    break;
                case Maneuver.StopOnLights:
                case Maneuver.WaitToEnterIntersection:
                    this.DecelerateToStopBeforeEnteringIntersection(veh);
                    break;
            }
        }

        /// <summary>
        /// Computes deceleration to stop before entering intersection for given vehicle
        /// </summary>
        /// <param name="veh">given vehicle</param>
        private void DecelerateToStopBeforeEnteringIntersection(Vehicle veh)
        {
            if (veh.VelocityVector.Length() > Constants.DoubleErrorTolerance)
            {
                double velocity = veh.VelocityVector.Length();
                double distanceToLine = veh.GetDistanceToEndOfStreet();
                if (distanceToLine < Constants.DoubleErrorTolerance)
                    return;
                double deceleration = 1.5 * velocity * velocity / distanceToLine;
                veh.AccelerationVector.X = -veh.FrontVector.X * deceleration;
                veh.AccelerationVector.Y = -veh.FrontVector.Y * deceleration;
            }
        }

        /// <summary>
        /// Computes deceleration to avoid collision for given vehicle
        /// </summary>
        /// <param name="veh">given vehicle</param>
        private void DecelerateToAvoidCollision(Vehicle veh)
        {
            if (veh.VelocityVector.Length() > Constants.DoubleErrorTolerance)
            {
                double velocityDifference = veh.VelocityVector.Length() - veh.VehicleInFrontOfUs.VelocityVector.Length();
                if (velocityDifference < 0)
                    velocityDifference = 1;
                double distanceBetween =
                    new Point(veh.Position.X - veh.VehicleInFrontOfUs.Position.X,
                        veh.Position.Y - veh.VehicleInFrontOfUs.Position.Y).Length() - veh.VehicleLength / 2 -
                    veh.VehicleInFrontOfUs.VehicleLength / 2;

                //We should decelerate more if we are closer to collision thats why we "invert"the distance
                double invertedDistance =
                    Constants.DistanceToOmmitDistanceDifferenceDeceleratingFactor - distanceBetween;
                invertedDistance = invertedDistance > 0 ? invertedDistance : 0;

                double multiplyingFactor = velocityDifference * Constants.VelocityDifferenceDeceleratingFactor +
                                            invertedDistance * Constants.DistanceDifferenceDeceleratingFactor;
                veh.AccelerationVector.X = -veh.FrontVector.X * multiplyingFactor;
                veh.AccelerationVector.Y = -veh.FrontVector.Y * multiplyingFactor;
            }
        }

        /// <summary>
        /// Vehicles tries to decelerate in order to achieve desired velocity
        /// </summary>
        /// <param name="isStreet">true if we are decelerating on street, false if on intersection</param>
        private void ComputeDeceleration(Vehicle veh, bool isStreet)
        {
            double desiredVelocity, driverDecelerationMultiplier;
            if (isStreet)
            {
                desiredVelocity = Constants.BeforeEnteringIntersectionDesiredVelocity;
                driverDecelerationMultiplier = veh.VelocityVector.Length() * Constants.VelocityDeceleratingFactorOnStreet;
            }
            else
            {
                desiredVelocity = Constants.IntersectionDesiredVelocity;
                driverDecelerationMultiplier = veh.VelocityVector.Length() * Constants.VelocityDeceleratingFactorOnIntersection;
            }

            if (veh.VelocityVector.Length() <= desiredVelocity && veh.VelocityVector.Length() >= desiredVelocity - Constants.DesiredVelocityMargin)
            {
                //do not change your velocity
                veh.AccelerationVector.X = 0;
                veh.AccelerationVector.Y = 0;
            }
            else if(veh.VelocityVector.Length() > desiredVelocity)
            {
                //decelerate
                veh.AccelerationVector.X = -veh.FrontVector.X * driverDecelerationMultiplier;
                veh.AccelerationVector.Y = -veh.FrontVector.Y * driverDecelerationMultiplier;
            }
            else
            {
                //acelerate if you are by whatever reasons driving too slow!
                veh.AccelerationVector.X = veh.FrontVector.X * desiredVelocity;
                veh.AccelerationVector.Y = veh.FrontVector.Y * desiredVelocity;  
            }
        }

        /// <summary>
        /// Vehicle tries to accelerate in order to achieve maximum velocity while on a straight road
        /// </summary>
        private void ComputeAccelerationOnStraightRoad(Vehicle veh)
        {
            if (veh.VelocityVector.Length() >= veh.MaximumVelocity)
            {
                veh.AccelerationVector.X = 0;
                veh.AccelerationVector.Y = 0;
            }
            else if (veh.VelocityVector.Length() < veh.MaximumVelocity)
            {
                veh.AccelerationVector.X = veh.FrontVector.X * Constants.DriverAcceleratingOnStraightRoadMultiplier;
                veh.AccelerationVector.Y = veh.FrontVector.Y * Constants.DriverAcceleratingOnStraightRoadMultiplier;
            }
        }

        /// <summary>
        /// Computes and sets centripetal acceleration of a vehicle which is turning left
        /// </summary>
        /// <param name="left">True if car is turning left, false if right</param>
        private void ComputeTurnAcceleration(Vehicle veh, bool left)
        {
            var turnedAngle = veh.FrontVector.AngleFrom(veh.InitialTurningDirection);
            var hasntAlreadyTurned = turnedAngle < 90;
            if (hasntAlreadyTurned)
            {
                if (veh.TurningArcRadius == 0)
                    veh.TurningArcRadius = this.ComputeTurningArcRadius(veh, left);
                if (left)
                {
                    veh.AccelerationVector.X = veh.VelocityVector.Y;
                    veh.AccelerationVector.Y = -veh.VelocityVector.X;
                }
                else
                {
                    veh.AccelerationVector.X = -veh.VelocityVector.Y;
                    veh.AccelerationVector.Y = veh.VelocityVector.X;
                }
                veh.AccelerationVector.X *= veh.VelocityVector.Length() / veh.TurningArcRadius;
                veh.AccelerationVector.Y *= veh.VelocityVector.Length() / veh.TurningArcRadius;
            }
            else
            {
                veh.AccelerationVector.X = 0;
                veh.AccelerationVector.Y = 0;
            }
        }

        /// <summary>
        /// Returns radius of an arc the car is supposed to ride on.
        /// </summary>
        /// <param name="left">True if car is turning left, false if right</param>
        private double ComputeTurningArcRadius(Vehicle veh, bool left)
        {
            if (left)
                return Math.Abs(veh.Position.X) + Math.Abs(veh.Position.Y); 
            else
                return Math.Abs(Math.Abs(veh.Position.X) - Math.Abs(veh.Position.Y));
        }

        /// <summary>
        /// Computes and sets acceleration of a vehicle which is correcting its angle after turning
        /// Actually cheats by setting velocity to desired velocity
        /// </summary>
        private void ComputeCorrectionAcceleration(Vehicle veh)
        {
            var desiredDirection = veh.FrontVector.GetDesiredDirection();
            var speed = veh.VelocityVector.Length();
            veh.VelocityVector.X = desiredDirection.X * speed;
            veh.VelocityVector.Y = desiredDirection.Y * speed;
            veh.FrontVector.X = veh.VelocityVector.X / veh.VelocityVector.Length();
            veh.FrontVector.Y = veh.VelocityVector.Y / veh.VelocityVector.Length();
            veh.AccelerationVector.X = 0;
            veh.AccelerationVector.Y = 0;
        }
    }
}
