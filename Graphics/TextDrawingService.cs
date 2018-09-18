using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using Traffic.Utilities;
using Point = Traffic.Utilities.Point;

namespace Traffic.Graphics
{
    public class TextDrawingService
    {

        internal void DisplayText(List<int> texturesID, string textToDisplay, Point dispCoord, CameraService camServ)
        {
            double translation = 0;
            GL.PushMatrix();
            GL.Translate(camServ.CameraPosition.X - Constants.XCoordTranslationOfStatsBox, Math.Abs(camServ.CameraDistance) - 1,
                camServ.CameraPosition.Y - Constants.YCoordTranslationOfStatsBox);
            foreach (char character in textToDisplay)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, texturesID[Convert.ToInt32(character)]);
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(Color.White);
                GL.TexCoord2(0, 0);
                GL.Vertex3(0 + translation + dispCoord.X, 0.0f, 0 + dispCoord.Y);

                GL.TexCoord2(1, 0);
                GL.Vertex3(Constants.DisplayedCharSize + translation + dispCoord.X, 0.0f, 0 + dispCoord.Y);

                GL.TexCoord2(1, 1);
                GL.Vertex3(Constants.DisplayedCharSize + translation + dispCoord.X, 0.0f, Constants.DisplayedCharSize + dispCoord.Y);

                GL.TexCoord2(0, 1);
                GL.Vertex3(0 + translation + dispCoord.X, 0.0f, Constants.DisplayedCharSize + dispCoord.Y);

                translation += Constants.DistanceBetweenChars;

                GL.End();
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.Disable(EnableCap.Texture2D);
            }
            GL.PopMatrix();
        }
    }
}
