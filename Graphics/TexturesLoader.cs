using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using Traffic.Utilities;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Traffic.Graphics
{
    /// <summary>
    /// Handles loading textures
    /// </summary>
    internal static class TexturesLoader
    {
        /// <summary>
        /// List of unique id of textures
        /// </summary>
        public static List<int> TexturesList { get; private set; }

        /// <summary>
        /// Loads textures from given path
        /// </summary>
        /// <param name="path">given path</param>
        /// <returns>unique ID in texture loading context</returns>
        public static int LoadTexture(string path)
        {
            int textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            string pathToFile = @"../../Textures/" + path;
            var bitmap = new Bitmap(pathToFile);
            var bitmapDataLockedMemory = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapDataLockedMemory.Width, bitmapDataLockedMemory.Height,
                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapDataLockedMemory.Scan0);

            bitmap.UnlockBits(bitmapDataLockedMemory);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int) TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int) TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);

            return textureId;
        }

        /// <summary>
        /// Initializes textures
        /// </summary>
        public static void InitTextures()
        {
            TexturesList = new List<int>();
            for (int i = 0; i < Constants.NumberOfTextures; i++)
                TexturesList.Add(LoadTexture(string.Format("Car{0}.png", i + 1)));
        }
    }
}