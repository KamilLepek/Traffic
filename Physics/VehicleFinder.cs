using System.Collections.Generic;
using OpenTK;
using Traffic.Utilities;
using Traffic.Vehicles;
using Traffic.World.Edges;
using Traffic.World.Vertices;

namespace Traffic.Physics
{
    public class VehicleFinder
    {

        private readonly List<Vehicle> vehicleList;

        public VehicleFinder(List<Vehicle> listOfVehicles)
        {
            this.vehicleList = listOfVehicles;
        }

        public Vector2 SelectedVehicleCoordinates { get; private set; }
        public Vehicle VehicleWeClickedOn { get; private set; }

        /// <summary>
        ///     Checks if click was on a vehicle, sets VehicleWeClickedOn to that vehicle, otherwise sets it to null
        /// </summary>
        /// <param name="cursorPosition"> position of the cursor </param>
        public void CheckIfClickedOnVehicle(Vector2 cursorPosition)
        {
            this.VehicleWeClickedOn = null;

            foreach (var vehicle in this.vehicleList)
            {
                Vector2 placeCoordinates;
                if (vehicle.Place is Street)
                {
                    placeCoordinates = ((Street)vehicle.Place).GetCoordinates();
                    if (((Street) vehicle.Place).IsVertical)
                    {
                        if (cursorPosition.X > placeCoordinates.X + vehicle.Position.X - Constants.CarWidth / 2 &&
                            cursorPosition.X < placeCoordinates.X + vehicle.Position.X + Constants.CarWidth / 2
                            && cursorPosition.Y > placeCoordinates.Y + vehicle.Position.Y - Constants.CarLength / 2 &&
                            cursorPosition.Y < placeCoordinates.Y + vehicle.Position.Y + Constants.CarLength / 2)
                        {
                            this.VehicleWeClickedOn = vehicle;
                            return;
                        }
                    }
                    else
                    {
                        if (cursorPosition.X > placeCoordinates.X + vehicle.Position.X - Constants.CarLength / 2 &&
                            cursorPosition.X < placeCoordinates.X + vehicle.Position.X + Constants.CarLength / 2
                            && cursorPosition.Y > placeCoordinates.Y + vehicle.Position.Y - Constants.CarWidth / 2 &&
                            cursorPosition.Y < placeCoordinates.Y + vehicle.Position.Y + Constants.CarWidth / 2)
                        {
                            this.VehicleWeClickedOn = vehicle;
                            return;
                        }
                    }
                }
                else if (vehicle.Place is Intersection)
                {
                    placeCoordinates = ((Intersection) vehicle.Place).GetCoordinates();
                    if (vehicle.FrontVector.IsMoreOfVerticalThanHorizontal())
                    {
                        if (cursorPosition.X > placeCoordinates.X + vehicle.Position.X - Constants.CarWidth / 2 &&
                            cursorPosition.X < placeCoordinates.X + vehicle.Position.X + Constants.CarWidth / 2
                            && cursorPosition.Y > placeCoordinates.Y + vehicle.Position.Y - Constants.CarLength / 2 &&
                            cursorPosition.Y < placeCoordinates.Y + vehicle.Position.Y + Constants.CarLength / 2)
                        {
                            this.VehicleWeClickedOn = vehicle;
                            return;
                        }
                    }
                    else
                    {
                        if (cursorPosition.X > placeCoordinates.X + vehicle.Position.X - Constants.CarLength / 2 &&
                            cursorPosition.X < placeCoordinates.X + vehicle.Position.X + Constants.CarLength / 2
                            && cursorPosition.Y > placeCoordinates.Y + vehicle.Position.Y - Constants.CarWidth / 2 &&
                            cursorPosition.Y < placeCoordinates.Y + vehicle.Position.Y + Constants.CarWidth / 2)
                        {
                            this.VehicleWeClickedOn = vehicle;
                            return;
                        }
                    }
                }
            }
        }
    }
}