using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DescentDirX.UI.Components
{
    class ImageGameButton : GameButton
    {
        private ScaledImage Image { get; set; }

        public ImageGameButton(ScaledImage image) : base(new Vector2(image.GetWidth(), image.GetHeight()), image.Position)
        {
            Image = image;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible()) return;
            Image.Draw(spriteBatch);
        }

        public override void PositionRelatedTo(GameObject gameObject)
        {
            base.PositionRelatedTo(gameObject);
            Image.PositionRelatedTo(gameObject);
        }

        public override void Show()
        {
            base.Show();
            Image.Show();
        }

        public override void Hide()
        {
            base.Hide();
            Image.Hide();
        }

        public override void SetDisabled(bool disabled)
        {
            base.SetDisabled(disabled);
            Image.GrayScale = disabled;
        }
    }
}
