﻿using System;
using Traffic.World;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using Traffic.Physics;
using Traffic.Utilities;

namespace Traffic.Graphics
{
    /// <summary>
    ///     Class handling graphics of the application
    /// </summary>
    public class GraphicsController : GameWindow
    {
        private readonly Map gameWorld;
        private readonly Action updateWorldHandler;
        private readonly DrawingService drawingService;
        private readonly CameraService cameraService;
        private readonly TextDrawingService textDrawingService;
        private readonly VehicleFinder vehicleFinder;
        private readonly FpsCounter fpsCounter;
        private bool mousePressed;
        
        public GraphicsController(Map world, Action updateWorldHandler)
        {
            this.CursorVisible = false;
            this.gameWorld = world;
            this.updateWorldHandler = updateWorldHandler;
            this.cameraService = new CameraService();
            this.textDrawingService = new TextDrawingService();
            this.fpsCounter = new FpsCounter();
            this.drawingService = new DrawingService(this.cameraService, this.textDrawingService);
            this.vehicleFinder = new VehicleFinder();
            TexturesLoader.InitTextures();
        }

        /// <summary>
        ///     Initializing method called after the openGL loads but before it starts rendering
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Title = "Traffic Simulation";
            GL.ClearColor(Color.CornflowerBlue);
            this.cameraService.InitCamera();
        }

        /// <summary>
        ///     Event emitted by openGL each time the world state needs to be updated
        /// </summary>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            this.updateWorldHandler();

            if (this.mousePressed == false && this.Focused)
            {
                var cameraTranslationVector =
                    new Vector2(
                        (float) (this.cameraService.DeltaMousePosition.X * Constants.CursorMovementSpeed *
                                 this.cameraService.CameraDistance),
                        (float) (this.cameraService.DeltaMousePosition.Y * Constants.CursorMovementSpeed *
                                 this.cameraService.CameraDistance));
                if (this.cameraService.WillCursorBeInBoundsAfterTranslating(cameraTranslationVector, this.Bounds.Width, this.Bounds.Height))
                {
                    this.cameraService.CursorPosition += cameraTranslationVector;
                }              
                this.cameraService.ResetCursor(this.Bounds.Left, this.Bounds.Width, this.Bounds.Top, this.Bounds.Height);
            }
            this.cameraService.UpdateLastMousePosition();
            if (this.vehicleFinder.VehicleWeClickedOn != null)
            {
                this.cameraService.CenterCameraOnVehicle(this.vehicleFinder.VehicleWeClickedOn.GetCoordinates());
            }
        }


        /// <summary>
        ///     Main frame rendering method
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
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            
            foreach (var vehicle in this.gameWorld.Vehicles)
                this.drawingService.GlDrawVehicle(vehicle, this.vehicleFinder.VehicleWeClickedOn != null, this.vehicleFinder.VehicleWeClickedOn);
            
            this.drawingService.GlDrawCursor(this.cameraService.CursorPosition.X , this.cameraService.CursorPosition.Y , this.cameraService.CameraDistance);
            this.drawingService.DrawStatsBox(this.vehicleFinder.VehicleWeClickedOn);
            this.fpsCounter.OnNewFrame();
            this.textDrawingService.DisplayText(TexturesLoader.CharsTextures, "FPS: " + (int)this.fpsCounter.Fps,
                new Utilities.Point(0.94, 0), cameraService);
            this.SwapBuffers();

        }

        /// <summary>
        ///     Properly handle window resize
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, this.Width / (float) this.Height, 0.5f, 6400.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        /// <summary>
        ///     Moves camera on pressed arrow keys
        /// </summary>
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            this.cameraService.Move(e.Key);
            if (e.Key == Key.Escape)
                this.Exit();
            if (e.Alt && e.Key == Key.Enter)
                this.WindowState = WindowState.Fullscreen;
        }

        /// <summary>
        ///     Zooms in and out on mouse scroll event
        /// </summary>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            this.cameraService.Zoom(e.Delta > 0);
        }

        /// <summary>
        ///     Enables camera movement via OnMouseMove event
        /// </summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
                this.mousePressed = true;
            this.vehicleFinder.CheckIfClickedOnVehicle(this.cameraService.CursorPosition, this.gameWorld.Vehicles);
        }

        /// <summary>
        ///     Disables camera movement via OnMouseMove event
        /// </summary>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
                this.mousePressed = false;
        }

        /// <summary>
        ///     Handles camera movement on mouse click and move
        /// </summary>
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.mousePressed)
            {
                var cameraTranslationVector =
                    new Vector2(
                        (float) (-e.XDelta * Constants.CameraMouseMovementSpeed * this.cameraService.CameraDistance),
                        (float) (-e.YDelta * Constants.CameraMouseMovementSpeed * this.cameraService.CameraDistance));
                if (this.cameraService.WillCursorBeInBoundsAfterTranslating(cameraTranslationVector, this.Bounds.Width, this.Bounds.Height))
                {
                    this.cameraService.Move(e);
                }   
            }    
        }
    }
}
