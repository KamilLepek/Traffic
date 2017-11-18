using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Traffic.World.Edges;
using Traffic.World.Vertices;
using Traffic.Vehicles;
using System;
using Traffic.Utilities;

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

            coordinates.X = (float)((intersection.ColumnNumber / 2) * Constants.StreetLength
                                  + ((intersection.ColumnNumber / 2) - 1) * Constants.IntersectionSize
                                  + Constants.IntersectionSize / 2);

            coordinates.Y = (float)((intersection.RowNumber / 2) * Constants.StreetLength
                                  + ((intersection.RowNumber / 2) - 1) * Constants.IntersectionSize
                                  + Constants.IntersectionSize / 2);

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
            GL.Vertex3(intersectionCoords.X - Constants.StreetWidth / 2, 0.0f, intersectionCoords.Y - Constants.StreetWidth / 2);
            GL.Vertex3(intersectionCoords.X - Constants.IntersectionSize / 2, 0.0f, intersectionCoords.Y - Constants.StreetWidth / 2);
            GL.Vertex3(intersectionCoords.X - Constants.IntersectionSize / 2, 0.0f, intersectionCoords.Y + Constants.StreetWidth / 2);
            GL.Vertex3(intersectionCoords.X - Constants.StreetWidth / 2, 0.0f, intersectionCoords.Y + Constants.StreetWidth / 2);
            GL.Vertex3(intersectionCoords.X - Constants.StreetWidth / 2, 0.0f, intersectionCoords.Y + Constants.IntersectionSize / 2);
            GL.Vertex3(intersectionCoords.X + Constants.StreetWidth / 2, 0.0f, intersectionCoords.Y + Constants.IntersectionSize / 2);
            GL.Vertex3(intersectionCoords.X + Constants.StreetWidth / 2, 0.0f, intersectionCoords.Y + Constants.StreetWidth / 2);
            GL.Vertex3(intersectionCoords.X + Constants.IntersectionSize / 2, 0.0f, intersectionCoords.Y + Constants.StreetWidth / 2);
            GL.Vertex3(intersectionCoords.X + Constants.IntersectionSize / 2, 0.0f, intersectionCoords.Y - Constants.StreetWidth / 2);
            GL.Vertex3(intersectionCoords.X + Constants.StreetWidth / 2, 0.0f, intersectionCoords.Y - Constants.StreetWidth / 2);
            GL.Vertex3(intersectionCoords.X + Constants.StreetWidth / 2, 0.0f, intersectionCoords.Y - Constants.IntersectionSize / 2);
            GL.Vertex3(intersectionCoords.X - Constants.StreetWidth / 2, 0.0f, intersectionCoords.Y - Constants.IntersectionSize / 2);
            GL.End();

            this.DrawTrafficLights(intersectionCoords, intersection.VerticalTrafficLight, intersection.HorizontalTrafficLight);
        }

        private void DrawTrafficLights(Vector2 intersectionCoords, Light verticalLight, Light horizontalLight)
        {
            this.GlDrawTrafficLight(new Vector2((float)(intersectionCoords.X + Constants.StreetWidth + Constants.TrafficLightWidth / 2),
                (float)(intersectionCoords.Y + Constants.StreetWidth + Constants.TrafficLightHeight / 2)), 0, verticalLight);
            this.GlDrawTrafficLight(new Vector2((float)(intersectionCoords.X + Constants.StreetWidth + Constants.TrafficLightHeight / 2),
                (float)(intersectionCoords.Y - Constants.StreetWidth - Constants.TrafficLightWidth / 2)), 90, horizontalLight);
            this.GlDrawTrafficLight(new Vector2((float)(intersectionCoords.X - Constants.StreetWidth - Constants.TrafficLightWidth / 2),
                (float)(intersectionCoords.Y - Constants.StreetWidth - Constants.TrafficLightHeight / 2)), 180, verticalLight);
            this.GlDrawTrafficLight(new Vector2((float)(intersectionCoords.X - Constants.StreetWidth - Constants.TrafficLightHeight / 2),
                (float)(intersectionCoords.Y + Constants.StreetWidth + Constants.TrafficLightWidth / 2)), 270, horizontalLight);
        }

        private void GlDrawTrafficLight(Vector2 center, double rotationAngle, Light light)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.Translate(center.X, 0, center.Y);
            GL.Rotate(rotationAngle, Vector3d.UnitY);

            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(Color.Gray);
            GL.Vertex3(-Constants.TrafficLightWidth / 2, 0.0f, -Constants.TrafficLightHeight / 2);
            GL.Vertex3(Constants.TrafficLightWidth / 2, 0.0f, -Constants.TrafficLightHeight / 2);
            GL.Vertex3(Constants.TrafficLightWidth / 2, 0.0f, Constants.TrafficLightHeight / 2);
            GL.Vertex3(-Constants.TrafficLightWidth / 2, 0.0f, Constants.TrafficLightHeight / 2);
            GL.End();

            var topLightColor = light == Light.Green ? Color.Black : Color.Red;
            var bottomLightColor = light == Light.Green ? Color.Green : Color.Black;

            GL.PushMatrix();
            GL.Translate(0, 0, -Constants.TrafficLightHeight / 4);
            this.GlDrawCircle(Constants.TrafficLightHeight / 6, topLightColor);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 0, Constants.TrafficLightHeight / 4);
            this.GlDrawCircle(Constants.TrafficLightHeight / 6, bottomLightColor);
            GL.PopMatrix();

            GL.PopMatrix();
        }

        private void GlDrawCircle(double radius, Color color)
        {
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(color);
            for (int i = 0; i < Constants.AmountOfLinesInCircle; i++)
            {
                var angle = 2 * Math.PI * i / Constants.AmountOfLinesInCircle;
                GL.Vertex3(radius * Math.Cos(angle), 0, radius * Math.Sin(angle));
            }
            GL.End();
        }

        public void GlDrawVehicle(Vehicle vehicle)
        {
            var placeCoordinates = new Vector2();

            if (vehicle.Place is Street)
                placeCoordinates = this.GetStreetCoordinates((Street)vehicle.Place);
            else if (vehicle.Place is Intersection)
                placeCoordinates = this.GetIntersectionCoordinates((Intersection)vehicle.Place);
            else
                return;

            double rotationAngle = vehicle.FrontVector.GetRotationAngle();
            GL.Color4(Color.Transparent);//todo:Handle Alpha Transparency
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.Translate(placeCoordinates.X + vehicle.Position.X, 0, placeCoordinates.Y + vehicle.Position.Y);
            GL.Rotate(rotationAngle, Vector3d.UnitY);

            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, TexturesLoader.TexturesList[vehicle.TextureAssigned]);

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(1, 0);
            GL.Vertex3(-Constants.CarWidth / 2, 0.0f, -Constants.CarLength / 2);

            GL.TexCoord2(1, 1);
            GL.Vertex3(Constants.CarWidth / 2, 0.0f, -Constants.CarLength / 2);

            GL.TexCoord2(0, 1);
            GL.Vertex3(Constants.CarWidth / 2, 0.0f, Constants.CarLength / 2);

            GL.TexCoord2(0, 0);
            GL.Vertex3(-Constants.CarWidth / 2, 0.0f, Constants.CarLength / 2);

            GL.End();
            GL.Disable(EnableCap.Texture2D);
           

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
