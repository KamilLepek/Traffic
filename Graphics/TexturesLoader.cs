using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using Traffic.Utilities;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Traffic.Graphics
{
    /// <summary>
    ///     Handles loading textures
    /// </summary>
    internal static class TexturesLoader
    {
        /// <summary>
        ///     List of unique id of textures
        /// </summary>
        public static List<int> VehiclesTexturesList { get; private set; }
        public static List<int> CharsTextures { get; private set; }

        /// <summary>
        ///     Loads textures from given path
        /// </summary>
        /// <param name="path"> given path </param>
        /// <returns> unique ID in texture loading context </returns>
        public static int LoadVehiclesTexture(string path)
        {
            int textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            string pathToFile = @"../../../Textures/" + path;
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

        public static List<Bitmap> GetCharsFromBitmap()
        {
            List<Bitmap> listOfBitmaps = new List<Bitmap>();
            Bitmap textBitmap = new Bitmap(@"..\..\..\Textures\TextBitmap.bmp");
            for (int i = 0; i <= Constants.WidthHeightOfBitmapChar - 1; i++)
            {
                for (int k = 0; k <= Constants.WidthHeightOfBitmapChar - 1; k++)
                {
                    Rectangle cropRect = new Rectangle(k * Constants.WidthHeightOfBitmapChar, i * Constants.WidthHeightOfBitmapChar,
                        Constants.WidthHeightOfBitmapChar, Constants.WidthHeightOfBitmapChar);
                    Bitmap bmpChar = textBitmap.Clone(cropRect, textBitmap.PixelFormat);
                    listOfBitmaps.Add(bmpChar);
                }
            }
            return listOfBitmaps;
        }

        public static List<int> LoadCharsTextures(List<Bitmap> listOfBitmaps)
        {
            List<int> textureIndexes = new List<int>();
            foreach (Bitmap bitmap in listOfBitmaps)
            {
                int textureId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, textureId);

                Bitmap bmpChar = bitmap;

                var bitmapDataLockedMemory = bmpChar.LockBits(
                    new Rectangle(0, 0, bmpChar.Width, bmpChar.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapDataLockedMemory.Width, bitmapDataLockedMemory.Height,
                    0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapDataLockedMemory.Scan0);

                bmpChar.UnlockBits(bitmapDataLockedMemory);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                    (int)TextureWrapMode.Clamp);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                    (int)TextureWrapMode.Clamp);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int)TextureMinFilter.Linear);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int)TextureMagFilter.Linear);
                textureIndexes.Add(textureId);
            }
            return textureIndexes;
        }

        /// <summary>
        ///     Initializes textures
        /// </summary>
        public static void InitTextures()
        {
            VehiclesTexturesList = new List<int>();
            for (int i = 0; i < Constants.NumberOfVehicleTextures; i++)
                VehiclesTexturesList.Add(LoadVehiclesTexture($"Car{i + 1}.png"));
            VehiclesTexturesList.Add(LoadVehiclesTexture("Cursor.png"));
            List<Bitmap> listOfBmp = GetCharsFromBitmap();
            CharsTextures = LoadCharsTextures(listOfBmp);
        }
    }
}
