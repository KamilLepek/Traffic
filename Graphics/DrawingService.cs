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
            float intersectionX = (intersection.ColumnNumber / 2) * Constants.StreetLength
                                  + ((intersection.ColumnNumber / 2) - 1) * Constants.IntersectionSize
                                  + Constants.IntersectionSize / 2;
            float intersectionZ = (intersection.RowNumber / 2) * Constants.StreetLength
                                  + ((intersection.RowNumber / 2) - 1) * Constants.IntersectionSize
                                  + Constants.IntersectionSize / 2;

            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(Color.Gray);
            GL.Vertex3(intersectionX - Constants.IntersectionSize / 6, 0.0f, intersectionZ - Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionX - Constants.IntersectionSize / 2, 0.0f, intersectionZ - Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionX - Constants.IntersectionSize / 2, 0.0f, intersectionZ + Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionX - Constants.IntersectionSize / 6, 0.0f, intersectionZ + Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionX - Constants.IntersectionSize / 6, 0.0f, intersectionZ + Constants.IntersectionSize / 2);
            GL.Vertex3(intersectionX + Constants.IntersectionSize / 6, 0.0f, intersectionZ + Constants.IntersectionSize / 2);
            GL.Vertex3(intersectionX + Constants.IntersectionSize / 6, 0.0f, intersectionZ + Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionX + Constants.IntersectionSize / 2, 0.0f, intersectionZ + Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionX + Constants.IntersectionSize / 2, 0.0f, intersectionZ - Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionX + Constants.IntersectionSize / 6, 0.0f, intersectionZ - Constants.IntersectionSize / 6);
            GL.Vertex3(intersectionX + Constants.IntersectionSize / 6, 0.0f, intersectionZ - Constants.IntersectionSize / 2);
            GL.Vertex3(intersectionX - Constants.IntersectionSize / 6, 0.0f, intersectionZ - Constants.IntersectionSize / 2);
            GL.End();
        }

        /// <summary>
        /// Returns the angle in degrees by which the direction vector is rotated over vertical axis (Z axis)
        /// </summary>
        /// <param name="direction">Vector to get angle from</param>
        /// <returns>Angle in degrees</returns>
        private float GetRotationAngle(Utilities.Point direction)
        {
            if (direction.Y == 0) // we can't compute atan, it's either 90 or 270 degrees
            {
                if (direction.X > 0)
                    return 90.0f;
                else
                    return 270.0f;
            }
            double angle = Math.Atan(direction.X/direction.Y);

            if (direction.Y < 0) // computing tan loses information about signs of X and Y
                angle += Math.PI;
            if (angle < 0) // Math.Atan returns angle from -90 to 90, need to get rid of negatives
                angle += 2 * Math.PI;

            return (float)(angle*180/Math.PI);
        }

        public void GlDrawVehicle(Vehicle vehicle)
        {
            if (vehicle.Place is Street)
            {
                Vector2 streetCoordinates = this.GetStreetCoordinates((Street)vehicle.Place);
                float rotationAngle = this.GetRotationAngle(vehicle.FrontVector);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                GL.Translate(streetCoordinates.X + vehicle.Position.X, 0, streetCoordinates.Y + vehicle.Position.Y);
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
