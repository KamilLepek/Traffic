using System;
using System.Collections.Generic;
using System.Linq;
using Traffic.Utilities;
using Traffic.Vehicles;
using Traffic.World;

namespace Traffic.Physics
{
    public class DecisionController
    {
        private readonly Map world;
        private readonly ManeuverService maneuverService;

        public DecisionController(Map world)
        {
            this.world = world;
            this.maneuverService = new ManeuverService();
        }

        public void HandleDecisions()
        {
            foreach (var vehicle in this.world.Vehicles)
            {
                this.UpdateManeuverIfNeccessary(vehicle);
                this.ComputeAcceleration(vehicle);
            }
        }

        private void UpdateManeuverIfNeccessary(Vehicle veh)
        {
            if (maneuverService.CheckIfVehicleHasToAvoidCollisionOnStreet(veh))
                return;
            if (this.maneuverService.CheckIfVehicleIsApproachingEndOfStreet(veh))
                return;
            if (this.maneuverService.CheckIfVehicleEnteredIntersection(veh))
                return;
            if (this.maneuverService.CheckIfVehicleEnteredMiddleOfIntersection(veh))
                return;
            if (this.maneuverService.CheckIfVehicleLeftIntersection(veh))
                return;
            this.maneuverService.CheckIfVehicleLeftMiddleOfIntersection(veh);
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
            }
        }

        private void DecelerateToAvoidCollision(Vehicle veh)
        {
            if (veh.VelocityVector.Length() > Constants.DoubleErrorTolerance)
            {
                veh.AccelerationVector.X = -veh.FrontVector.X * (Constants.DriverDecelerationForCollisionAvoidanceMultiplier / veh.FrontVector.Length());
                veh.AccelerationVector.Y = -veh.FrontVector.Y * (Constants.DriverDecelerationForCollisionAvoidanceMultiplier / veh.FrontVector.Length());
            }
        }

        /// <summary>
        /// Vehicles tries to decelerate in order to achieve desired velocity
        /// </summary>
        /// <param name="isStreet">true if we are decelerating on street, false if on intersection</param>
        private void ComputeDeceleration(Vehicle veh, bool isStreet)
        {
            double desiredVelocity = isStreet
                ? Constants.BeforeEnteringIntersectionDesiredVelocity
                : Constants.IntersectionDesiredVelocity;
            if (veh.VelocityVector.Length() <= desiredVelocity)
            {
                veh.AccelerationVector.X = 0;
                veh.AccelerationVector.Y = 0;
            }
            else
            {
                veh.AccelerationVector.X = -veh.FrontVector.X * (Constants.DriverDecelerationMultiplier / veh.FrontVector.Length());
                veh.AccelerationVector.Y = -veh.FrontVector.Y * (Constants.DriverDecelerationMultiplier / veh.FrontVector.Length());
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
                veh.AccelerationVector.X = veh.FrontVector.X * (Constants.DriverAcceleratingOnStraightRoadMultiplier / veh.FrontVector.Length());
                veh.AccelerationVector.Y = veh.FrontVector.Y * (Constants.DriverAcceleratingOnStraightRoadMultiplier / veh.FrontVector.Length());
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
            var desiredDirection = this.GetDesiredDirection(veh.FrontVector);
            var speed = veh.VelocityVector.Length();
            veh.VelocityVector.X = desiredDirection.X * speed;
            veh.VelocityVector.Y = desiredDirection.Y * speed;
            veh.FrontVector.X = veh.VelocityVector.X;
            veh.FrontVector.Y = veh.VelocityVector.Y;
            veh.AccelerationVector.X = 0;
            veh.AccelerationVector.Y = 0;
        }

        /// <summary>
        /// Returns the closest in terms of angle horizontal or vertical direction
        /// </summary>
        private Point GetDesiredDirection(Point realDirection)
        {
            var acceptedDirections = new List<Point>()
            {
                new Point(1,0),
                new Point(-1,0),
                new Point(0,1),
                new Point(0,-1)
            };
            return acceptedDirections.First(d =>
                d.AngleFrom(realDirection) == acceptedDirections.Min(p => p.AngleFrom(realDirection)));
        }
    }
}
