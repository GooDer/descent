using DescentDirX.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DescentDirX.UI.Components
{
    class TextGameButton : GameButton
    {
        public string Label { get; private set; }
        private Color normalColor = new Color(52, 55, 64);
        private Color focusedColor = ImageProvider.foregroundColor;
        private Color disabledColor = new Color(150, 150, 150);

        public TextGameButton(Vector2 size, Vector2 position, string label) : base(size, position)
        {
            Label = label;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible()) return;

            var color = (Disabled ? disabledColor : (IsFocused() ? focusedColor : normalColor));
            Texture2D texture = ImageProvider.GetColorTexture(color, Color.White);
            
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), color);
            var labelSize = FontFactory.Font.MeasureString(Label);
            var posX = Position.X + Size.X / 2 - labelSize.X / 4;
            var posY = Position.Y + Size.Y / 2 - labelSize.Y / 4;

            spriteBatch.DrawString(FontFactory.Font, Label, new Vector2(posX, posY), Disabled ? Color.DarkGray : Color.White, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 1);
        }
    }
}
