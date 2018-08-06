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
    /// <summary>
    ///     Handles drawing map and vehicles.
    /// </summary>
    internal class DrawingService
    {

        /// <summary>
        ///     Draws street on map with respect to its coordinates
        /// </summary>
        /// <param name="street"> Street to draw </param>
        public void GlDrawStreet(Street street)
        {
            Vector2 streetCoordinates = street.GetCoordinates();
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

        /// <summary>
        ///     Draws intersection on map with respect to its coordinates
        /// </summary>
        /// <param name="intersection"> Intersection to draw </param>
        public void GlDrawIntersection(Intersection intersection)
        {
            var intersectionCoords = intersection.GetCoordinates();

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

        /// <summary>
        ///     Draws traffic lights
        /// </summary>
        /// <param name="intersectionCoords"> Coordinates of the intersection on which the traffic lights are </param>
        /// <param name="verticalLight"> color of the vertical lights </param>
        /// <param name="horizontalLight"> color of the horizontal lights </param>
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

        /// <summary>
        ///     Draws traffic light with specified color, center and angle
        /// </summary>
        /// <param name="center"> Center of the traffic light </param>
        /// <param name="rotationAngle"> Rotation of the traffic light </param>
        /// <param name="light"> Color of the traffic light </param>
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

        /// <summary>
        ///     Draws circle
        /// </summary>
        /// <param name="radius"> Radius of the circle </param>
        /// <param name="color"> Color of the circle </param>
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
        
        public void GlDrawVehicle(Vehicle vehicle, bool isVehicleClicked, Vehicle selectedVehicle)
        {
            if (!(vehicle.Place is Street) && !(vehicle.Place is Intersection))
                return;
            var placeCoordinates = vehicle.Place.GetCoordinates();

            double rotationAngle = vehicle.FrontVector.GetRotationAngle();
            
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.Translate(placeCoordinates.X + vehicle.Position.X, 0, placeCoordinates.Y + vehicle.Position.Y);
            GL.Rotate(rotationAngle, Vector3d.UnitY);

            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, TexturesLoader.TexturesList[vehicle.TextureAssigned]);

            GL.Begin(PrimitiveType.Quads);
            if (isVehicleClicked && vehicle == selectedVehicle)
                GL.Color4(Color.LightSalmon);
            else
                GL.Color3(Color.White);
            
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
            GL.Color3(Color.White);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.White);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);
            GL.End();
        }

        /// <summary>
        ///     Draws cursor on screen
        /// </summary>
        public void GlDrawCursor(double x, double y, double cameraDistance)
        {
            GL.PushMatrix();

            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(Color.White);
            GL.Vertex3(x, 0.0f, y);
            GL.Vertex3(x, 0.0f, y + 2 * Constants.CursorSize * Math.Abs(cameraDistance) / 260);
            GL.Vertex3(x + 2 * Constants.CursorSize * Math.Abs(cameraDistance) / 260, 0,
                y + 2 * Constants.CursorSize * Math.Abs(cameraDistance) / 260);
            GL.End();

            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(Color.White);
            GL.Vertex3(x, 0.0f, y + 2 * Constants.CursorSize * Math.Abs(cameraDistance) / 260);
            GL.Vertex3(x + 0.9 * Constants.CursorSize * Math.Abs(cameraDistance) / 260, 0,
                y + 2 * Constants.CursorSize * Math.Abs(cameraDistance) / 260);
            GL.Vertex3(x, 0.0f, y + 2.8 * Constants.CursorSize * Math.Abs(cameraDistance) / 260);
            GL.End();

            GL.LineWidth((float)1.3);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);

            GL.Vertex3(x, 0.0f, y);
            GL.Vertex3(x, 0.0f, y + 2.8 * Constants.CursorSize * Math.Abs(cameraDistance) / 260);
            GL.Vertex3(x + 0.9 * Constants.CursorSize * Math.Abs(cameraDistance) / 260, 0,
                y + 2 * Constants.CursorSize * Math.Abs(cameraDistance) / 260);
            GL.Vertex3(x + 2 * Constants.CursorSize * Math.Abs(cameraDistance) / 260, 0,
                y + 2 * Constants.CursorSize * Math.Abs(cameraDistance) / 260);
            GL.End();
            GL.PopMatrix();
        }
    }
}
