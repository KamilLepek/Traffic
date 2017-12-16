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
        public Point testingPoint { get; set; }

        public Vector2 cameraPosition { get; set; }//x and z coordinates used to mapping mouse cursor onto world

        public Vector2 lastMousePos;

        public Vector2 deltaMousePos;


        public CameraService(int boundsLeft, int boundsWidth, int boundsTop, int boundsHeight)
        {
            this.cameraDistance = Constants.InitialCameraDistance;
            this.testingPoint = new Point(Constants.InitialTestingPointCoordinateX, Constants.InitialTestingPointCoordinateY);
            this.cameraPosition = new Vector2(0,0);
            this.lastMousePos = new Vector2(0,0); 
            this.deltaMousePos = new Vector2(0,0);
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
        



    }
}
