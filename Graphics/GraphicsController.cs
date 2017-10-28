using System;
using Traffic.World;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;

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
        private CameraService cameraService;
        private bool mousePressed = false;

        public GraphicsController(Map world, Action updateWorldHandler)
        {
            this.gameWorld = world;
            this.updateWorldHandler = updateWorldHandler;
            this.drawingService = new DrawingService();
            this.cameraService = new CameraService();
        }

        /// <summary>
        /// Initializing method called after the openGL loads but before it starts rendering
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Title = "Traffic Simulation";
            GL.ClearColor(Color.CornflowerBlue);
            cameraService.InitCamera();
        }

        /// <summary>
        /// Event emitted by openGL each time the world state needs to be updated
        /// </summary>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            updateWorldHandler();
        }

        /// <summary>
        /// Main frame rendering method
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);

            drawingService.GlDrawAxes();

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
        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            cameraService.Move(e.Key);
        }

        /// <summary>
        /// Zooms in and out on mouse scroll event
        /// </summary>
        protected override void OnMouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            cameraService.Zoom(e.Delta > 0);
        }

        /// <summary>
        /// Enables camera movement via OnMouseMove event
        /// </summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
                this.mousePressed = true;
        }

        /// <summary>
        /// Disables camera movement via OnMouseMove event
        /// </summary>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
                this.mousePressed = false;
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.mousePressed)
                cameraService.Move(e);
        }
    }
}
