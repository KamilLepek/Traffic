using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Traffic.World.Edges;
using Traffic.World.Vertices;
using Traffic.Vehicles;
using Traffic.Utilities;
using System;

namespace Traffic.Graphics
{
    internal class DrawingService
    {
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

            coordinates.X = (intersection.ColumnNumber / 2) * Constants.StreetLength
                                  + ((intersection.ColumnNumber / 2) - 1) * Constants.IntersectionSize
                                  + Constants.IntersectionSize / 2;

            coordinates.Y = (intersection.RowNumber / 2) * Constants.StreetLength
                                  + ((intersection.RowNumber / 2) - 1) * Constants.IntersectionSize
                                  + Constants.IntersectionSize / 2;

            return coordinates;
        }

        public void GlDrawStreet(Street street)
        {
            Vector2 streetCoordinates = this.GetStreetCoordinates(street);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.LightGray);
            if (street.IsVertical)
            {
                GL.Vertex3(streetCoordinates.X + Constants.StreetWidth / 2, 0.0f, streetCoordinates.Y + Constants.StreetLength);
                GL.Vertex3(streetCoordinates.X + Constants.StreetWidth / 2, 0.0f, streetCoordinates.Y);
                GL.Vertex3(streetCoordinates.X - Constants.StreetWidth / 2, 0.0f, streetCoordinates.Y);
                GL.Vertex3(streetCoordinates.X - Constants.StreetWidth / 2, 0.0f, streetCoordinates.Y + Constants.StreetLength);
            }
            else
            {
                GL.Vertex3(streetCoordinates.X + Constants.StreetLength, 0.0f, streetCoordinates.Y + Constants.StreetWidth / 2);
                GL.Vertex3(streetCoordinates.X + Constants.StreetLength, 0.0f, streetCoordinates.Y - Constants.StreetWidth / 2);
                GL.Vertex3(streetCoordinates.X, 0.0f, streetCoordinates.Y - Constants.StreetWidth / 2);
                GL.Vertex3(streetCoordinates.X, 0.0f, streetCoordinates.Y + Constants.StreetWidth / 2);
            }
            GL.End();
        }

        public void GlDrawIntersection(Intersection intersection)
        {
            var intersectionCoords = this.GetIntersectionCoordinates(intersection);

            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(Color.Gray);
            GL.Vertex3(intersectionCoords.X - Constants.IntersectionSize / 6, 0.0f, intersectionCoords.Y - Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionCoords.X - Constants.IntersectionSize / 2, 0.0f, intersectionCoords.Y - Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionCoords.X - Constants.IntersectionSize / 2, 0.0f, intersectionCoords.Y + Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionCoords.X - Constants.IntersectionSize / 6, 0.0f, intersectionCoords.Y + Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionCoords.X - Constants.IntersectionSize / 6, 0.0f, intersectionCoords.Y + Constants.IntersectionSize / 2);
            GL.Vertex3(intersectionCoords.X + Constants.IntersectionSize / 6, 0.0f, intersectionCoords.Y + Constants.IntersectionSize / 2);
            GL.Vertex3(intersectionCoords.X + Constants.IntersectionSize / 6, 0.0f, intersectionCoords.Y + Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionCoords.X + Constants.IntersectionSize / 2, 0.0f, intersectionCoords.Y + Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionCoords.X + Constants.IntersectionSize / 2, 0.0f, intersectionCoords.Y - Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionCoords.X + Constants.IntersectionSize / 6, 0.0f, intersectionCoords.Y - Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionCoords.X + Constants.IntersectionSize / 6, 0.0f, intersectionCoords.Y - Constants.IntersectionSize / 2);
            GL.Vertex3(intersectionCoords.X - Constants.IntersectionSize / 6, 0.0f, intersectionCoords.Y - Constants.IntersectionSize / 2);
            GL.End();
        }

        public void GlDrawVehicle(Vehicle vehicle)
        {
            var placeCoordinates = new Vector2();

            if (vehicle.Place is Street)
                placeCoordinates = this.GetStreetCoordinates((Street) vehicle.Place);
            else if (vehicle.Place is Intersection)
                placeCoordinates = this.GetIntersectionCoordinates((Intersection)vehicle.Place);
            else 
                return;

            float rotationAngle = vehicle.FrontVector.GetRotationAngle();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.Translate(placeCoordinates.X + vehicle.Position.X, 0, placeCoordinates.Y + vehicle.Position.Y);
            GL.Rotate(rotationAngle, Vector3.UnitY);
                
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Red);
            GL.Vertex3(-Constants.CarWidth / 2, 0.0f, -Constants.CarLength / 2);
            GL.Vertex3(Constants.CarWidth / 2, 0.0f, -Constants.CarLength / 2);
            GL.Color3(Color.Gold);
            GL.Vertex3(Constants.CarWidth / 2, 0.0f, Constants.CarLength / 2);
            GL.Vertex3(-Constants.CarWidth / 2, 0.0f, Constants.CarLength / 2);
            GL.End();

            GL.PopMatrix();
        }

        /// <summary>
        /// Draw X and Z axes (Z in openGL correspondes with Y in TrafficSimulation models)
        /// </summary>
        public void GlDrawAxes()
        {

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);
            GL.End();
        }
    }
}
