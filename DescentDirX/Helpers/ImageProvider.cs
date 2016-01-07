using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DescentDirX.Helpers
{
    /// <summary>
    /// Helper class which register and provides images from disk and base textures for custom rendering
    /// Singleton
    /// </summary>
    class ImageProvider
    {
        /// <summary>
        /// Graphics device for creating textures
        /// </summary>
        public static GraphicsDeviceManager Graphics { get; set; }
        /// <summary>
        /// Default background color used by FF Games on their webpage
        /// </summary>
        public static Color backgroundColor = new Color(224, 224, 224);
        /// <summary>
        /// Default foreground color used by FF Games on their webpage
        /// </summary>
        public static Color foregroundColor = new Color(9, 49, 86);

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static ImageProvider instance;
        /// <summary>
        /// Stored already used textures to prevent memory leaks while called in draw methods
        /// </summary>
        private static Dictionary<Color, Texture2D> colorTextures = new Dictionary<Color, Texture2D>();
        /// <summary>
        /// List of registered images from disk
        /// </summary>
        private Dictionary<ImageListEnum, Texture2D> Images { get; set; }

        public static ImageProvider Instance {
            get {
                if (instance == null)
                {
                    instance = new ImageProvider();
                }
                return instance;
            }
            private set { instance = value; }
        }

        private ImageProvider()
        {
            Images = new Dictionary<ImageListEnum, Texture2D>();
        }

        /// <summary>
        /// Register image from disk with image enum key to help with using
        /// </summary>
        /// <param name="key">image enum help key</param>
        /// <param name="image">texture of image to store</param>
        public void RegisterImage(ImageListEnum key, Texture2D image)
        {
            Images.Add(key, image);
        }

        /// <summary>
        /// Get specific image from internal storage
        /// </summary>
        /// <param name="key">image enum help key</param>
        /// <returns></returns>
        public Texture2D GetImage(ImageListEnum key)
        {
            Texture2D image;
            if (Images.TryGetValue(key, out image))
            {
                return image;
            }

            return null;
        }

        /// <summary>
        /// Returns given image in greyscale colors
        /// </summary>
        /// <param name="key">image enum help key</param>
        /// <returns></returns>
        public Texture2D GetGrayscaleImage(ImageListEnum key)
        {
            return GetGrayscaleImage(GetImage(key));
        }

        /// <summary>
        /// Returns given image with opacity
        /// </summary>
        /// <param name="key">image enum help key</param>
        /// <param name="opaque">A value from RGBA</param>
        /// <returns></returns>
        public Texture2D GetOpaqueImage(ImageListEnum key, int opaque)
        {
            return GetOpaqueImage(GetImage(key), opaque);
        }

        public Texture2D GetOpaqueImage(Texture2D image, int opaque)
        {
            if (image != null)
            {
                int size = image.Width * image.Height;
                Color[] colors = new Color[size];
                Color[] newColors = new Color[size];
                image.GetData<Color>(colors);

                for (int i = 0; i < image.Width * image.Height; i++)
                {
                    newColors[i] = new Color(colors[i].R, colors[i].G, colors[i].B, opaque);
                }
                Texture2D opaqueImage = new Texture2D(image.GraphicsDevice, image.Width, image.Height);
                opaqueImage.SetData(newColors);

                return opaqueImage;
            }

            return image;
        }

        public static Texture2D GetGrayscaleImage(Texture2D image)
        {
            if (image != null)
            {
                int size = image.Width * image.Height;
                Color[] colors = new Color[size];
                Color[] newColors = new Color[size];
                image.GetData<Color>(colors);

                for (int i = 0; i < image.Width * image.Height; i++)
                {
                    int baseColor = (colors[i].R + colors[i].B + colors[i].G) / 3;
                    newColors[i] = new Color(baseColor, baseColor, baseColor);
                }
                Texture2D grayscaleImage = new Texture2D(image.GraphicsDevice, image.Width, image.Height);
                grayscaleImage.SetData(newColors);

                return grayscaleImage;
            }
            return image;
        }

        public static Texture2D GetColorTexture(Color color, Color? baseColor = null)
        {
            Texture2D texture = null;
            if (baseColor == null)
            {
                baseColor = Color.Gray;
            }

            if (colorTextures.ContainsKey(color))
            {
                colorTextures.TryGetValue(color, out texture);
            }
            else
            {
                texture = new Texture2D(Graphics.GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { (Color)baseColor });
                colorTextures.Add(color, texture);
            }
            return texture;
        }
    }
}
