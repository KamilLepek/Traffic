using System;
using System.Collections.Generic;
using System.Linq;
using Traffic.Utilities;
using Traffic.Vehicles;
using Traffic.World;
using Traffic.World.Vertices;

namespace Traffic.Physics
{
    public class DecisionController
    {
        public Map World;

        public DecisionController(Map world)
        {
            this.World = world;
        }

        public void HandleDecisions()
        {
            foreach (var vehicle in this.World.Vehicles)
            {
                this.CheckIfManeuverFinished(vehicle);
                this.CheckIfManeuverStarts(vehicle);
                this.ComputeAcceleration(vehicle);
            }
        }

        /// <summary>
        /// Determines if the driver of veh has finished performing his Maneuver, and if so sets the vehicle's Maneuver field to None
        /// </summary>
        private void CheckIfManeuverFinished(Vehicle veh)
        {
            // Vehicle got out of intersection
            if (!(veh.Place is Intersection) &&
                (veh.Maneuver == Maneuvers.ForwardOnIntersect || veh.Maneuver == Maneuvers.TurnLeft ||
                 veh.Maneuver == Maneuvers.TurnRight))
            {
                veh.Maneuver = Maneuvers.None;
                veh.TurningArcRadius = 0;
                veh.InitialTurningDirection = null;
            }
        }

        /// <summary>
        /// Determines if the driver of veh should start new maneuver, and if so sets the vehicle's Maneuver field.
        /// </summary>
        private void CheckIfManeuverStarts(Vehicle veh)
        {
            // Vehicle entered intersection
            if ((veh.Place is Intersection) &&
                (veh.Maneuver != Maneuvers.ForwardOnIntersect && veh.Maneuver != Maneuvers.TurnLeft &&
                 veh.Maneuver != Maneuvers.TurnRight))
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
            }
        }

        /// <summary>
        /// Computes and sets the part of acceleration which is tangent to velocity, and is responsible for the velocity's direction change.
        /// Bases mainly on vehicle's Maneuver
        /// </summary>
        private void ComputeAcceleration(Vehicle veh)
        {
            switch (veh.Maneuver)
            {
                case Maneuvers.None:
                    veh.AccelerationVector.X = 0;
                    veh.AccelerationVector.Y = 0;
                    break;
                case Maneuvers.ForwardOnIntersect:
                    veh.AccelerationVector.X = 0;
                    veh.AccelerationVector.Y = 0;
                    break;
                case Maneuvers.TurnLeft:
                    this.ComputeTurnAcceleration(veh, true);
                    break;
                case Maneuvers.TurnRight:
                    this.ComputeTurnAcceleration(veh, false);
                    break;
            }
        }

        /// <summary>
        /// Computes and sets centripetal acceleration of a vehicle which is turning left
        /// </summary>
        private void ComputeTurnAcceleration(Vehicle veh, bool left)
        {
            ConsoleLogger.Log(String.Format("Angle:{0}", veh.FrontVector.AngleFrom(veh.InitialTurningDirection)));
            var isInTheMiddleOfIntersection =
                veh.Position.DistanceFrom(new Point(0, 0)) < Constants.TurnStartingPoint * Constants.IntersectionSize;
            var turnedAngle = veh.FrontVector.AngleFrom(veh.InitialTurningDirection);
            var hasntAlreadyTurned = turnedAngle < 90;

            if (isInTheMiddleOfIntersection && hasntAlreadyTurned)
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

        private double ComputeTurningArcRadius(Vehicle veh, bool left)
        {
            if (left)
                return Math.Abs(veh.Position.X) + Math.Abs(veh.Position.Y); 
            else
                return Math.Abs(Math.Abs(veh.Position.X) - Math.Abs(veh.Position.Y));
        }
    }
}
