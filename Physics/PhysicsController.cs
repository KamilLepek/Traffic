﻿using System.Linq;
using Traffic.Utilities;
using Traffic.Vehicles;
using Traffic.World;
using Traffic.World.Edges;
using Traffic.World.Vertices;

namespace Traffic.Physics
{
    public class PhysicsController
    {
        public Map World;

        public PhysicsController(Map world)
        {
            this.World = world;
        }

        /// <summary>
        /// Handles vehicles movement
        /// </summary>
        public void HandlePhysics()
        {
            //by using for like this we can safely remove vehicles from the list while iterating
            for (int i = this.World.Vehicles.Count - 1; i >= 0; i--)
            {
                this.MoveVehicle(this.World.Vehicles[i]);
                this.UpdatePlace(this.World.Vehicles[i]);
            }
        }

        /// <summary>
        /// Changes vehicles position with their velocity vector
        /// </summary>
        /// <param name="veh">vehicle to change the position</param>
        private void MoveVehicle(Vehicle veh)
        {
            veh.Position.X += veh.VelocityVector.X * (1f / Constants.TicksPerSecond);
            veh.Position.Y += veh.VelocityVector.Y * (1f / Constants.TicksPerSecond);
        }

        /// <summary>
        /// Determines the side that the vehicle might change place (Street->Intersection or Intersection->Street)
        /// and invokes update function depending on the side
        /// </summary>
        private void UpdatePlace(Vehicle veh)
        {
            float angle = veh.FrontVector.GetRotationAngle();
            if (angle < Constants.MaximumVehicleAngle || angle > 360 - Constants.MaximumVehicleAngle)
                this.ChangePlaceIfNecessary(veh, Orientation.Bottom);//kontekst: hipotetycznie zmienimy place na ten o 1 nizej w naszym ukladzie, tj porusza się w dół
            else if (angle > 90 - Constants.MaximumVehicleAngle && angle < 90 + Constants.MaximumVehicleAngle)
                this.ChangePlaceIfNecessary(veh, Orientation.Right);
            else if (angle > 180 - Constants.MaximumVehicleAngle && angle < 180 + Constants.MaximumVehicleAngle)
                this.ChangePlaceIfNecessary(veh, Orientation.Top);
            else if (angle > 270 - Constants.MaximumVehicleAngle && angle < 270 + Constants.MaximumVehicleAngle)
                this.ChangePlaceIfNecessary(veh, Orientation.Left);
        }

        
        /// <summary>
        /// Checks if we should change place and invokes method if necessary
        /// </summary>
        /// <param name="veh">vehicle that we deal with</param>
        /// <param name="or">Orientation candidate for the change (Bottom is the world object below obj, etc.)</param>
        private void ChangePlaceIfNecessary(Vehicle veh, Orientation or)
        {
            if (veh.Place is Street)
            {
                if (veh.Position.X < 0 && or == Orientation.Left ||
                    veh.Position.Y > Constants.StreetLength && or == Orientation.Bottom ||
                    veh.Position.Y < 0 && or == Orientation.Top ||
                    veh.Position.X > Constants.StreetLength && or == Orientation.Right)
                    this.StreetIntersectionSwapper(veh, or);
            }
            else if (veh.Place is Intersection)
            {
                if (veh.Position.X < -Constants.IntersectionSize / 2 && or == Orientation.Left ||
                    veh.Position.Y > Constants.IntersectionSize / 2 && or == Orientation.Bottom ||
                    veh.Position.Y < -Constants.IntersectionSize / 2 && or == Orientation.Top ||
                    veh.Position.X > Constants.IntersectionSize / 2 && or == Orientation.Right)
                    this.StreetIntersectionSwapper(veh, or);
            }
        }

        /// <summary>
        /// Changes place from street to intersection OR from intersection to street
        /// </summary>
        private void StreetIntersectionSwapper(Vehicle veh, Orientation or)
        {
            int horizontal, vertical; //horizontal and vertical diffrence from actual place
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
            ConsoleLogger.Log("R:" + veh.Place.RowNumber + " C:" + veh.Place.ColumnNumber);
            if (veh.Place is Street)
            {
                veh.Place = this.World.Intersections.Find(item => (item.RowNumber == veh.Place.RowNumber + vertical)
                                                              && (item.ColumnNumber == veh.Place.ColumnNumber + horizontal));
                if (veh.Place == null)
                {
                    this.KillVehicle(veh);
                    return;
                }

                ConsoleLogger.Log("Vehicle STREET -> INTERSECTION");
                this.SetPositionAfterEnteringIntersection(veh, or);
            }
            else if (veh.Place is Intersection)
            {
                veh.Place = this.World.Streets.First(item => (item.RowNumber == veh.Place.RowNumber + vertical)
                                                              && (item.ColumnNumber == veh.Place.ColumnNumber + horizontal));
                ConsoleLogger.Log("Vehicle INTERSECTION -> STREET");
                this.SetPositionAfterEnteringStreet(veh, or);
            }
            ConsoleLogger.Log("R:" + veh.Place.RowNumber + " C:" + veh.Place.ColumnNumber);
        }

        /// <summary>
        /// Sets position on the intersection
        /// </summary>
        /// <param name="veh">vehicle we deal with</param>
        /// <param name="or">orientation that we have been going to in order to enter the intersection
        /// For example:
        /// bottom means that we were going bottom in order to enter the intersection and it means
        /// that after entering it we are at the top :)
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
        /// Sets position on the street
        /// </summary>
        /// <param name="veh">vehicle we deal with</param>
        /// <param name="or">side of the intersection we came from</param>
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
        /// Sad but necessary method that ends the journey of our driver
        /// </summary>
        /// <param name="veh">vehicle to get rid of</param>
        private void KillVehicle(Vehicle veh)
        {
            this.World.Vehicles.Remove(veh);
        }
    }
}
