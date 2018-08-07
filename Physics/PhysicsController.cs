using System;
using System.Linq;
using Traffic.Utilities;
using Traffic.Vehicles;
using Traffic.World;
using Traffic.World.Edges;
using Traffic.World.Vertices;

namespace Traffic.Physics
{
    /// <summary>
    ///     Handles velocity and position updates based on acceleration
    /// </summary>
    public class PhysicsController
    {
        private readonly Map world;

        public PhysicsController(Map world)
        {
            this.world = world;
        }

        /// <summary>
        ///     Changes place from street to intersection OR from intersection to street
        /// </summary>
        private void ChangePlace(Vehicle veh, Orientation or)
        {
            int horizontal = 0, vertical = 0; //horizontal and vertical diffrence from actual place
            UnitConverter.OrientationToRowColumnDiffrence(or, ref horizontal, ref vertical);
            if (veh.Place is Street)
            {
                veh.Place.Vehicles.Remove(veh);
                veh.Place = ((Street)veh.Place).Edges.Find(item => item.RowNumber == veh.Place.RowNumber + vertical
                                                                   && item.ColumnNumber ==
                                                                   veh.Place.ColumnNumber + horizontal);
                if (veh.Place is EndPoint)
                {
                    this.KillVehicle(veh);
                    return;
                }
                veh.Place.Vehicles.Add(veh);

                this.SetPositionAfterEnteringIntersection(veh, or);
            }
            else if (veh.Place is Intersection)
            {
                veh.Place.Vehicles.Remove(veh);
                veh.Place = this.world.Streets.First(item => item.RowNumber == veh.Place.RowNumber + vertical
                                                             && item.ColumnNumber ==
                                                             veh.Place.ColumnNumber + horizontal);
                veh.Place.Vehicles.Add(veh);
                this.SetPositionAfterEnteringStreet(veh, or);
            }
        }


        /// <summary>
        ///     Checks if we should change place and invokes method if necessary
        /// </summary>
        /// <param name="veh"> vehicle that we deal with </param>
        /// <param name="or"> Orientation candidate for the change (Bottom is the world object below obj, etc.) </param>
        private void ChangePlaceIfNecessary(Vehicle veh, Orientation or)
        {
            if (veh.Place is Street)
            {
                if (veh.Position.X < 0 && or == Orientation.Left ||
                    veh.Position.Y > Constants.StreetLength && or == Orientation.Bottom ||
                    veh.Position.Y < 0 && or == Orientation.Top ||
                    veh.Position.X > Constants.StreetLength && or == Orientation.Right)
                    this.ChangePlace(veh, or);
            }
            else if (veh.Place is Intersection)
                if (veh.Position.X < -Constants.IntersectionSize / 2 && or == Orientation.Left ||
                    veh.Position.Y > Constants.IntersectionSize / 2 && or == Orientation.Bottom ||
                    veh.Position.Y < -Constants.IntersectionSize / 2 && or == Orientation.Top ||
                    veh.Position.X > Constants.IntersectionSize / 2 && or == Orientation.Right)
                    this.ChangePlace(veh, or);
        }

        /// <summary>
        ///     Handles vehicles movement
        /// </summary>
        public void HandlePhysics()
        {
            // by using for like this we can safely remove vehicles from the list while iterating
            for (int i = this.world.Vehicles.Count - 1; i >= 0; i--)
            {
                this.UpdateVelocity(this.world.Vehicles[i]);
                this.MoveVehicle(this.world.Vehicles[i]);
            }

            foreach (var intersection in this.world.Intersections)
                intersection.PerformTimerTick();
        }

        /// <summary>
        ///     Sad but necessary method that ends the journey of our driver
        /// </summary>
        /// <param name="veh"> vehicle to get rid of </param>
        private void KillVehicle(Vehicle veh)
        {
            this.world.Vehicles.Remove(veh);
        }

        /// <summary>
        ///     Changes vehicles position with their velocity vector
        /// </summary>
        /// <param name="veh"> vehicle to change the position </param>
        private void MoveVehicle(Vehicle veh)
        {
            veh.Position.X += veh.VelocityVector.X * (1f / Constants.TicksPerSecond);
            veh.Position.Y += veh.VelocityVector.Y * (1f / Constants.TicksPerSecond);
            this.UpdatePlace(veh);
        }

        /// <summary>
        ///     Sets position on the intersection
        /// </summary>
        /// <param name="veh"> vehicle we deal with </param>
        /// <param name="or">
        ///     orientation that we have been going to in order to enter the intersection
        ///     For example:
        ///     bottom means that we were going bottom in order to enter the intersection and it means
        ///     that after entering it we are at the top :)
        /// </param>
        private void SetPositionAfterEnteringIntersection(Vehicle veh, Orientation or)
        {
            switch (or)
            {
                case Orientation.Bottom:
                    veh.Position.Y = -Constants.IntersectionSize / 2;
                    break;
                case Orientation.Left:
                    veh.Position.X = Constants.IntersectionSize / 2;
                    break;
                case Orientation.Right:
                    veh.Position.X = -Constants.IntersectionSize / 2;
                    break;
                case Orientation.Top:
                    veh.Position.Y = Constants.IntersectionSize / 2;
                    break;
            }
        }

        /// <summary>
        ///     Sets position on the street
        /// </summary>
        /// <param name="veh"> vehicle we deal with </param>
        /// <param name="or"> side of the intersection we came from </param>
        private void SetPositionAfterEnteringStreet(Vehicle veh, Orientation or)
        {
            switch (or)
            {
                case Orientation.Bottom:
                    veh.Position.Y = 0;
                    break;
                case Orientation.Left:
                    veh.Position.X = Constants.StreetLength;
                    break;
                case Orientation.Right:
                    veh.Position.X = 0;
                    break;
                case Orientation.Top:
                    veh.Position.Y = Constants.StreetLength;
                    break;
            }
        }

        /// <summary>
        ///     Determines the side that the vehicle might change place (Street->Intersection or Intersection->Street)
        ///     and invokes update function depending on the side
        /// </summary>
        private void UpdatePlace(Vehicle veh)
        {
            double angle = veh.FrontVector.GetRotationAngle();
            if (angle < Constants.MaximumVehicleAngle || angle > 360 - Constants.MaximumVehicleAngle)
                this.ChangePlaceIfNecessary(veh, Orientation.Bottom);
            else if (angle > 90 - Constants.MaximumVehicleAngle && angle < 90 + Constants.MaximumVehicleAngle)
                this.ChangePlaceIfNecessary(veh, Orientation.Right);
            else if (angle > 180 - Constants.MaximumVehicleAngle && angle < 180 + Constants.MaximumVehicleAngle)
                this.ChangePlaceIfNecessary(veh, Orientation.Top);
            else if (angle > 270 - Constants.MaximumVehicleAngle && angle < 270 + Constants.MaximumVehicleAngle)
                this.ChangePlaceIfNecessary(veh, Orientation.Left);
        }

        /// <summary>
        ///     Updates velocity and front vector of the given vehicle
        /// </summary>
        /// <param name="veh"> Given vehicle </param>
        private void UpdateVelocity(Vehicle veh)
        {
            veh.VelocityVector.X += veh.AccelerationVector.X * (1f / Constants.TicksPerSecond);
            veh.VelocityVector.Y += veh.AccelerationVector.Y * (1f / Constants.TicksPerSecond);

            // Handle situation when we have decelerated so much that we changed our direction to opposite (moving backwards)
            if (veh.VelocityVector.AngleFrom(veh.FrontVector) > Constants.RadiusMarginForGoingBackward)
            {
                veh.VelocityVector.X = 0;
                veh.VelocityVector.Y = 0;
            }

            // Don't update frontVector if velocity is zero - vehicle has stopped
            if (Math.Abs(veh.VelocityVector.X) > Constants.DoubleErrorTolerance ||
                Math.Abs(veh.VelocityVector.Y) > Constants.DoubleErrorTolerance)
            {
                veh.FrontVector.X = veh.VelocityVector.X / veh.VelocityVector.Length();
                veh.FrontVector.Y = veh.VelocityVector.Y / veh.VelocityVector.Length();
            }
        }
    }
}