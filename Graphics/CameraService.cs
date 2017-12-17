using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Traffic.Utilities;

namespace Traffic.Graphics
{
    /// <summary>
    /// Handles the behaviour of the camera
    /// </summary>
    internal class CameraService
    {
        public double cameraDistance;

        public Vector2 cursorPosition { get; set; }//x and z coordinates used to mapping mouse cursor onto world

        public Vector2 lastMousePos;

        public Vector2 deltaMousePos;

        public Vector2 cameraPosition;


        public CameraService()
        {
            this.cameraDistance = Constants.InitialCameraDistance;
            this.cursorPosition = new Vector2(0,0);
            this.lastMousePos = new Vector2(0,0); 
            this.deltaMousePos = new Vector2(0,0);
            this.cameraPosition = new Vector2(0,0);
        }

        /// <summary>
        /// Initializes modelview matrix
        /// </summary>
        public void InitCamera()
        {
            Matrix4 modelview = Matrix4.LookAt(new Vector3(0.0f, -(float)Constants.InitialCameraDistance, 0.0f), Vector3.Zero, -Vector3.UnitZ);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
        }

        /// <summary>
        /// Zooms in or out depending on 
        /// </summary>
        public void Zoom(bool zoomIn)
        {
            Vector3 translationVector;
            if (zoomIn)
                translationVector = new Vector3(0, (float)Constants.CameraZoomSpeed, 0);
            else
                translationVector = new Vector3(0, -(float)Constants.CameraZoomSpeed, 0);

            GL.MatrixMode(MatrixMode.Modelview);
            if (this.cameraDistance + translationVector.Y < Constants.MinimalCameraDistanceFromSurface && this.cameraDistance + translationVector.Y > Constants.MaximalCameraDistanceFromSurface)
            {
                GL.Translate(translationVector * (float)(Math.Abs(this.cameraDistance) / 300));
                this.cameraDistance += translationVector.Y * (float)(Math.Abs(this.cameraDistance) / 300);
            }
            if (this.cameraDistance < Constants.MaximalCameraDistanceFromSurface)
            {
                GL.Translate(0, Constants.MaximalCameraDistanceFromSurface - this.cameraDistance, 0);
                this.cameraDistance = Constants.MaximalCameraDistanceFromSurface;
            }
        }

        /// <summary>
        /// Moves the camera as a result of pressing an arrow on keyboard
        /// </summary>
        public void Move(Key keyboardKey)
        {
            Vector3 translationVector;
            var movementSpeed = (float)(Constants.CameraKeysMovementSpeed * this.cameraDistance);
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
            Vector2 cameraTranslationVector = new Vector2(-translationVector.X, -translationVector.Z);
            this.cursorPosition += cameraTranslationVector;
            this.cameraPosition += cameraTranslationVector;
        }

        /// <summary>
        /// Moves the camera as a result of moving mouse
        /// </summary>
        public void Move(MouseMoveEventArgs eventArgs)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(-eventArgs.XDelta * Constants.CameraMouseMovementSpeed * this.cameraDistance,
                0.0f, -eventArgs.YDelta * Constants.CameraMouseMovementSpeed * this.cameraDistance);
            Vector2 cameraTranslationVector = new Vector2((float)(-eventArgs.XDelta * Constants.CameraMouseMovementSpeed * this.cameraDistance), (float)(-eventArgs.YDelta * Constants.CameraMouseMovementSpeed * this.cameraDistance));
            this.cameraPosition += -cameraTranslationVector;
        }

        #region CursorRelatedMethods
        /// <summary>
        /// Updates last mouse position between updating frames 
        /// </summary>

        public void UpdateLastMousePosition()
        {
            this.deltaMousePos = this.lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
            this.lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }
        /// <summary>
        /// Resets Cursor to the middle of window
        /// </summary>
        public void ResetCursor(int boundsLeft, int boundsWidth, int boundsTop, int boundsHeight)
        {
            OpenTK.Input.Mouse.SetPosition(boundsLeft + boundsWidth / 2, boundsTop + boundsHeight / 2);
        }

        #endregion
        /// <summary>
        /// Center camera on vehicle that is selected
        /// </summary>
        /// <param name="selectedVehicleCoordinations"></param>
        public void CenterCameraOnVehicle(Vector2 selectedVehicleCoordinations)
        {
            Vector3 cameraCenteringVector = new Vector3(-Constants.CameraTrackingSmoothness * (selectedVehicleCoordinations.X - this.cameraPosition.X), 0 ,-Constants.CameraTrackingSmoothness * (selectedVehicleCoordinations.Y - this.cameraPosition.Y));
            if (selectedVehicleCoordinations != new Vector2(0,0))
            {
                GL.Translate(cameraCenteringVector);
                this.cameraPosition += new Vector2(-cameraCenteringVector.X, -cameraCenteringVector.Z);
                this.cursorPosition += new Vector2(-cameraCenteringVector.X, -cameraCenteringVector.Z);
            }
        }

        public bool WillCursorBeInBoundsAfterTranslating(Vector2 currentCameraPosition, Vector2 translationVector, Vector2 currentCursorPosition, int BoundsWidth, int BoundsHeight, double currentcameraDistance)
        {
            if (Math.Abs(currentCursorPosition.X + translationVector.X - currentCameraPosition.X) < BoundsWidth * Math.Abs(currentcameraDistance)/ 1180 &&
                Math.Abs(currentCursorPosition.Y + translationVector.Y - currentCameraPosition.Y) < BoundsHeight * Math.Abs(currentcameraDistance)/ 1240)
            {
                return true;
            }
            else
                return false;
        }

    }
}
