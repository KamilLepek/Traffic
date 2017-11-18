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
        private double cameraDistance;

        public CameraService()
        {
            this.cameraDistance = Constants.InitialCameraDistance;
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
            GL.Translate(translationVector);
            this.cameraDistance += translationVector.Y;
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
    }
}
