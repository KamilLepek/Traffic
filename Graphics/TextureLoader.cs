using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using Traffic.Utilities;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Traffic.Graphics
{
    internal static class TextureLoader
    {
        public static List<int> TextureList { get; private set; }

        public static int LoadTexture(string path)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            string pathToFile = @"../../Textures/" + path;
            var bmp = new Bitmap(pathToFile);
            var data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height,
                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int) TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int) TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);

            return id;
        }

        public static void TextureInit()
        {
            TextureList = new List<int>();
            for (int i = 0; i < Constants.NumberOfTextures; i++)
                TextureList.Add(LoadTexture(string.Format("Car{0}.png", i + 1)));
        }
    }
}