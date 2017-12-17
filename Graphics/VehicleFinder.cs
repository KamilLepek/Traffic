using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Traffic.Utilities;
using Traffic.Vehicles;
using Traffic.World.Edges;
using Traffic.World.Vertices;

namespace Traffic.Graphics
{
    public class VehicleFinder
    {
        public bool isVehicleClicked;
        public List<Vehicle> vehicleList;
        public Vector2 selectedVehicleCoordinates;
        public uint selectedVehicleId;

        public VehicleFinder(List<Vehicle> listOfVehicles)
        {
            this.isVehicleClicked = false;
            this.vehicleList = listOfVehicles;
        }
        /// <summary>
        /// Checks if click was on a vehicle
        /// </summary>
        /// <param name="CursorPosition"></param>
        public void CheckIfClickedOnVehicle(Vector2 CursorPosition)
        {
            this.isVehicleClicked = false;
            foreach(var vehicle in this.vehicleList)
            {
                bool isVehicleStreetVertical = false;
                Street street = new Street();
                var placeCoordinates = new Vector2();

                if (vehicle.Place is Street)
                {
                    street = (Street)vehicle.Place;
                }
                

                if (vehicle.Place is Street)
                    placeCoordinates = this.GetStreetCoordinates((Street)vehicle.Place);
                else if (vehicle.Place is Intersection)
                    placeCoordinates = this.GetIntersectionCoordinates((Intersection)vehicle.Place);
                else
                    return;

                if (street.IsVertical == true)
                {

                    if (CursorPosition.X > placeCoordinates.X + vehicle.Position.X - Constants.CarWidth / 2 &&
                        CursorPosition.X < placeCoordinates.X + vehicle.Position.X + Constants.CarWidth / 2
                        && CursorPosition.Y > placeCoordinates.Y + vehicle.Position.Y - Constants.CarLength / 2 &&
                        CursorPosition.Y < placeCoordinates.Y + vehicle.Position.Y + Constants.CarLength / 2)
                    {
                        this.isVehicleClicked = true;
                        this.selectedVehicleId = vehicle.VehicleID;
                    }
                }
                else
                {
                    if (CursorPosition.X > placeCoordinates.X + vehicle.Position.X - Constants.CarLength / 2 &&
                        CursorPosition.X < placeCoordinates.X + vehicle.Position.X + Constants.CarLength / 2
                        && CursorPosition.Y > placeCoordinates.Y + vehicle.Position.Y - Constants.CarWidth / 2 &&
                        CursorPosition.Y < placeCoordinates.Y + vehicle.Position.Y + Constants.CarWidth / 2)
                    {
                        this.isVehicleClicked = true;
                        this.selectedVehicleId = vehicle.VehicleID;
                    }
                }

            }
        }
        /// <summary>
        /// Gets the world coordinates of given vehicle
        /// </summary>
        /// <param name="selectedVehicleIndex"></param>
        /// <returns></returns>
        public void GetVehicleCoordinates(uint selectedVehicleId)
        {
            int numberOfMatchedVehicles = 0;
            var placeCoordinates = new Vector2();

            foreach (var vehicle in this.vehicleList)
            {
                if(vehicle.VehicleID == this.selectedVehicleId)
                {
                    if (vehicle.Place is Street)
                        placeCoordinates = this.GetStreetCoordinates((Street)vehicle.Place);
                    else if (vehicle.Place is Intersection)
                        placeCoordinates = this.GetIntersectionCoordinates((Intersection)vehicle.Place);

                    this.selectedVehicleCoordinates = new Vector2((float)(placeCoordinates.X + vehicle.Position.X), (float)(placeCoordinates.Y + vehicle.Position.Y));
                    numberOfMatchedVehicles += 1;
                }
            }

            if (numberOfMatchedVehicles == 0)
            {
                this.isVehicleClicked = false;
            }

        }

        private Vector2 GetStreetCoordinates(Street street)
        {
            var coordinates = new Vector2();
            if (street.IsVertical)
            {
                coordinates.X = (float)(Math.Ceiling((street.ColumnNumber - 1) / 2f) * Constants.StreetLength
                                        + (Math.Ceiling((street.ColumnNumber - 1) / 2f) - 0.5) * Constants.IntersectionSize);
                coordinates.Y = (float)(Math.Ceiling((street.RowNumber - 1) / 2f) * Constants.StreetLength
                                        + Math.Ceiling((street.RowNumber - 1) / 2f) * Constants.IntersectionSize);
            }
            else
            {
                coordinates.X = (float)(Math.Ceiling((street.ColumnNumber - 1) / 2f) * Constants.StreetLength
                                        + Math.Ceiling((street.ColumnNumber - 1) / 2f) * Constants.IntersectionSize);
                coordinates.Y = (float)(Math.Ceiling((street.RowNumber - 1) / 2f) * Constants.StreetLength
                                        + (Math.Ceiling((street.RowNumber - 1) / 2f) - 0.5) * Constants.IntersectionSize);
            }
            return coordinates;
        }

        private Vector2 GetIntersectionCoordinates(Intersection intersection)
        {
            var coordinates = new Vector2();

            coordinates.X = (float)((intersection.ColumnNumber / 2) * Constants.StreetLength
                                    + ((intersection.ColumnNumber / 2) - 1) * Constants.IntersectionSize
                                    + Constants.IntersectionSize / 2);

            coordinates.Y = (float)((intersection.RowNumber / 2) * Constants.StreetLength
                                    + ((intersection.RowNumber / 2) - 1) * Constants.IntersectionSize
                                    + Constants.IntersectionSize / 2);

            return coordinates;
        }
    }
}
