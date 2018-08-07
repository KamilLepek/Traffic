using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Traffic.Utilities;

namespace Traffic.Graphics
{
    /// <summary>
    ///     Handles the behaviour of the camera
    /// </summary>
    internal class CameraService
    {
        private Vector2 lastMousePosition;       

        public CameraService()
        {
            this.CameraDistance = Constants.InitialCameraDistance;
            this.CursorPosition = new Vector2(0, 0);
            this.lastMousePosition = new Vector2(0, 0);
            this.DeltaMousePosition = new Vector2(0, 0);
            this.CameraPosition = new Vector2(0, 0);
        }

        /// <summary>
        /// 	Distance between camera and map
        /// </summary>
        public double CameraDistance { get; private set; }

        /// <summary>
        ///     x and z coordinates used to mapping mouse cursor onto world
        /// </summary>
        public Vector2 CursorPosition { get; set; }

        public Vector2 DeltaMousePosition { get; private set; }

        public Vector2 CameraPosition { get; private set; }

        /// <summary>
        ///     Initializes modelview matrix
        /// </summary>
        public void InitCamera()
        {
            var modelview = Matrix4.LookAt(new Vector3(0.0f, -(float) Constants.InitialCameraDistance, 0.0f),
                Vector3.Zero, -Vector3.UnitZ);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
        }

        /// <summary>
        ///     Zooms in or out
        /// </summary>
        public void Zoom(bool zoomIn)
        {
            var translationVector = zoomIn ? new Vector3(0, (float) Constants.CameraZoomSpeed, 0) : new Vector3(0, -(float) Constants.CameraZoomSpeed, 0);

            GL.MatrixMode(MatrixMode.Modelview);
            if (this.CameraDistance + translationVector.Y * (float)Math.Abs(this.CameraDistance) <= Constants.MinimalCameraDistanceFromSurface &&
                this.CameraDistance + translationVector.Y * (float)Math.Abs(this.CameraDistance) >= Constants.MaximalCameraDistanceFromSurface)
            {
                GL.Translate(translationVector * (float)Math.Abs(this.CameraDistance));
                this.CameraDistance += translationVector.Y * (float)Math.Abs(this.CameraDistance);
            }
        }

        /// <summary>
        ///     Moves the camera as a result of pressing an arrow on keyboard
        /// </summary>
        public void Move(Key keyboardKey)
        {
            Vector3 translationVector;
            float movementSpeed = (float) (Constants.CameraKeysMovementSpeed * this.CameraDistance);
            switch (keyboardKey)
            {
                case Key.Up:
                    translationVector = new Vector3(0, 0, -movementSpeed);
                    break;
                case Key.Down:
                    translationVector = new Vector3(0, 0, movementSpeed);
                    break;
                case Key.Left:
                    translationVector = new Vector3(-movementSpeed, 0, 0);
                    break;
                case Key.Right:
                    translationVector = new Vector3(movementSpeed, 0, 0);
                    break;
                default:
                    return;
            }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(translationVector);
            var cameraTranslationVector = new Vector2(-translationVector.X, -translationVector.Z);
            this.CursorPosition += cameraTranslationVector;
            this.CameraPosition += cameraTranslationVector;
        }

        /// <summary>
        ///     Moves the camera as a result of moving mouse
        /// </summary>
        public void Move(MouseMoveEventArgs eventArgs)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(-eventArgs.XDelta * Constants.CameraMouseMovementSpeed * this.CameraDistance,
                0.0f, -eventArgs.YDelta * Constants.CameraMouseMovementSpeed * this.CameraDistance);
            var cameraTranslationVector =
                new Vector2((float) (-eventArgs.XDelta * Constants.CameraMouseMovementSpeed * this.CameraDistance),
                    (float) (-eventArgs.YDelta * Constants.CameraMouseMovementSpeed * this.CameraDistance));
            this.CameraPosition += -cameraTranslationVector;
        }

        /// <summary>
        ///     Center camera on vehicle that is selected
        /// </summary>
        /// <param name="selectedVehicleCoordinations"></param>
        public void CenterCameraOnVehicle(Vector2 selectedVehicleCoordinations)
        {
            var cameraCenteringVector =
                new Vector3(
                    -Constants.CameraTrackingSmoothness * (selectedVehicleCoordinations.X - this.CameraPosition.X), 0,
                    -Constants.CameraTrackingSmoothness * (selectedVehicleCoordinations.Y - this.CameraPosition.Y));
            if (selectedVehicleCoordinations != new Vector2(0, 0))
            {
                GL.Translate(cameraCenteringVector);
                this.CameraPosition += new Vector2(-cameraCenteringVector.X, -cameraCenteringVector.Z);
                this.CursorPosition += new Vector2(-cameraCenteringVector.X, -cameraCenteringVector.Z);
            }
        }

        /// <summary>
        ///     Determines whether the cursor will be in bounds after translation
        /// </summary>
        /// <param name="translationVector"> vector we translate with </param>
        /// <param name="boundsWidth"> window width  </param>
        /// <param name="boundsHeight"> window height </param>
        /// <returns></returns>
        public bool WillCursorBeInBoundsAfterTranslating(Vector2 translationVector,
            int boundsWidth, int boundsHeight)
        {
            //TODO: currently works in window mode only, reimplement for full screen mode
            return Math.Abs(this.CursorPosition.X + translationVector.X - this.CameraPosition.X) <
                   boundsWidth * Math.Abs(this.CameraDistance) / Constants.WindowModeWidthBoundaryDivider &&
                   Math.Abs(this.CursorPosition.Y + translationVector.Y - this.CameraPosition.Y) <
                   boundsHeight * Math.Abs(this.CameraDistance) / Constants.WindowModeHeightBoundaryDivider;
        }

        #region CursorRelatedMethods

        /// <summary>
        ///     Updates last mouse position between updating frames
        /// </summary>
        public void UpdateLastMousePosition()
        {
            this.DeltaMousePosition = this.lastMousePosition - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            this.lastMousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        /// <summary>
        ///     Resets Cursor to the middle of window
        /// </summary>
        public void ResetCursor(int boundsLeft, int boundsWidth, int boundsTop, int boundsHeight)
        {
            Mouse.SetPosition(boundsLeft + boundsWidth / 2, boundsTop + boundsHeight / 2);
        }

        #endregion
    }
}