using DescentDirX.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DescentDirX.UI.Components
{
    class ScaledImage : GameObject
    {
        public ImageListEnum Image { get; private set; }
        private float Scale { get; set; }
        private Texture2D Texture { get; set; }
        private Texture2D GrayScaleTexture { get; set; }

        private bool grayScale = false;
        public bool GrayScale {
            get { return grayScale; }
            set {
                grayScale = value;
                if (value && GrayScaleTexture == null)
                {
                    GrayScaleTexture = ImageProvider.GetGrayscaleImage(Texture);
                }
            }
        }

        public ScaledImage(Vector2 position, ImageListEnum img, float scale = 1) : base(position)
        {
            Image = img;
            Scale = scale;

            Texture = ImageProvider.Instance.GetImage(img);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible()) return;
            var img = GrayScale ? GrayScaleTexture : Texture;
            spriteBatch.Draw(img, Position, scale: new Vector2(Scale, Scale));
        }

        public override void Update(int mouseX, int mouseY)
        {
            
        }

        public int GetHeight()
        {
            return (int)(Texture.Bounds.Height * Scale);
        }

        public int GetWidth()
        {
            return (int)(Texture.Bounds.Width * Scale);
        }
    }
}
