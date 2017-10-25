using System;
using Traffic.World;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Traffic.Utilities;

namespace Traffic.Graphics
{
    /// <summary>
    /// Class handling graphics of the application
    /// </summary>
    public class GraphicsController : GameWindow
    {

        private Map gameWorld;
        private Action updateWorldHandler;
        private DrawingService drawingService;

        public GraphicsController(Map world, Action updateWorldHandler)
        {
            this.gameWorld = world;
            this.updateWorldHandler = updateWorldHandler;
            this.drawingService = new DrawingService();
        }

        /// <summary>
        /// Initializing method called after the openGL loads but before it starts rendering
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Title = "Traffic Simulation";
            GL.ClearColor(Color.CornflowerBlue);

            Matrix4 modelview = Matrix4.LookAt(new Vector3(0.0f, 50.0f, 0.0f), Vector3.Zero, Vector3.UnitZ);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

        }

        /// <summary>
        /// Event emitted by openGL each time the world state needs to be updated
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            updateWorldHandler();
        }

        /// <summary>
        /// Main frame rendering method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);

            foreach (var street in this.gameWorld.Streets)
                this.drawingService.GlDrawStreet(street);

            foreach (var intersection in this.gameWorld.Intersections)
                this.drawingService.GlDrawIntersection(intersection);

            foreach (var vehicle in this.gameWorld.Vehicles)
                this.drawingService.GlDrawVehicle(vehicle);

            SwapBuffers();
        }

        /// <summary>
        /// Properly handle window resize
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 6400.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        /// <summary>
        /// Moves camera on pressed arrow keys
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            Vector3 translationVector;

            switch (e.Key) 
            {
                case OpenTK.Input.Key.Up:
                    translationVector = new Vector3(0, 0, -Constants.CameraMovementSpeed);                    
                    break;
                case OpenTK.Input.Key.Down:
                    translationVector = new Vector3(0, 0, Constants.CameraMovementSpeed);                    
                    break;
                case OpenTK.Input.Key.Left:
                    translationVector = new Vector3(-Constants.CameraMovementSpeed, 0, 0);
                    break;
                case OpenTK.Input.Key.Right:
                    translationVector = new Vector3(Constants.CameraMovementSpeed, 0, 0);
                    break;
                default:
                    return;
            }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(translationVector);
        }

        /// <summary>
        /// Zooms in and out on mouse scroll event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            Vector3 translationVector;
            if (e.Delta > 0)
                translationVector = new Vector3(0, Constants.CameraZoomSpeed, 0);
            else
                translationVector = new Vector3(0, -Constants.CameraZoomSpeed, 0);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(translationVector);
        }

    }
}
