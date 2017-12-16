using System;
using Traffic.World;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using Traffic.Utilities;

namespace Traffic.Graphics
{
    /// <summary>
    /// Class handling graphics of the application
    /// </summary>
    public class GraphicsController : GameWindow
    {

        private readonly Map gameWorld;
        private readonly Action updateWorldHandler;
        private readonly DrawingService drawingService;
        private readonly CameraService cameraService;
        private bool mousePressed;
        private bool isSingleClick;
        public VehicleFinder vehicleFinder;
        

        public GraphicsController(Map world, Action updateWorldHandler)
        {
            this.CursorVisible = false;
            this.isSingleClick = true;
            this.vehicleFinder = new VehicleFinder(world.Vehicles);
            this.gameWorld = world;
            this.updateWorldHandler = updateWorldHandler;
            this.drawingService = new DrawingService();
            this.cameraService = new CameraService(this.Bounds.Left, this.Bounds.Width, this.Bounds.Top, this.Bounds.Height);
            TexturesLoader.InitTextures();
        }

        /// <summary>
        /// Initializing method called after the openGL loads but before it starts rendering
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Title = "Traffic Simulation";
            GL.ClearColor(Color.CornflowerBlue);
            this.cameraService.InitCamera();
        }

        /// <summary>
        /// Event emitted by openGL each time the world state needs to be updated
        /// </summary>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            this.updateWorldHandler();

            if (this.mousePressed == false && this.Focused == true)
            {
                Vector2 cameraTranslationVector =
                    new Vector2((float)(this.cameraService.deltaMousePos.X * Constants.CursorMovementSpeed * this.cameraService.cameraDistance), (float)(this.cameraService.deltaMousePos.Y * Constants.CursorMovementSpeed * this.cameraService.cameraDistance));
                this.cameraService.cameraPosition += cameraTranslationVector;
                this.cameraService.ResetCursor(this.Bounds.Left, this.Bounds.Width, this.Bounds.Top, this.Bounds.Height);
            }
            this.cameraService.UpdateLastMousePosition();
            
        }

        /// <summary>
        /// Main frame rendering method
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);

            
            this.drawingService.GlDrawAxes();

            

            foreach (var street in this.gameWorld.Streets)
                this.drawingService.GlDrawStreet(street);

            foreach (var intersection in this.gameWorld.Intersections)
                this.drawingService.GlDrawIntersection(intersection);

            foreach (var vehicle in this.gameWorld.Vehicles)
                this.drawingService.GlDrawVehicle(vehicle);
            
            this.drawingService.GlDrawCursor(this.cameraService.cameraPosition.X , this.cameraService.cameraPosition.Y , this.cameraService.cameraDistance);
            
            
            this.SwapBuffers();
        }

        /// <summary>
        /// Properly handle window resize
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, this.Width / (float) this.Height, 1.0f, 6400.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        /// <summary>
        /// Moves camera on pressed arrow keys
        /// </summary>
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            this.cameraService.Move(e.Key);
            if (e.Key == Key.Escape)
            {
                this.Exit();
            }
            if (e.Alt && e.Key == Key.Enter)
            {
                this.WindowState = WindowState.Fullscreen;
            }
        }

        /// <summary>
        /// Zooms in and out on mouse scroll event
        /// </summary>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            this.cameraService.Zoom(e.Delta > 0);
        }

        /// <summary>
        /// Enables camera movement via OnMouseMove event
        /// </summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
                this.mousePressed = true;

            if (this.isSingleClick == true)
            {
                this.vehicleFinder.CheckIfClickedOnVehicle(this.cameraService.cameraPosition);
            }

            this.isSingleClick = false;
        }

        /// <summary>
        /// Disables camera movement via OnMouseMove event
        /// </summary>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                this.mousePressed = false;
            }

            this.isSingleClick = true;
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.mousePressed)
                this.cameraService.Move(e);
        }

    }
}
