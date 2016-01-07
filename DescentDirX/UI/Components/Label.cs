using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DescentDirX.UI.Components
{
    class Label : GameObject
    {
        public string Text { get; private set; }
        public Color Color { get; set; } = Color.Black;
        public float Scale { get; set; } = 0.5f;

        public Label(string text, Vector2 position) : base(position)
        {
            Text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible()) return;
            spriteBatch.DrawString(FontFactory.Font, Text, Position, Color, 0, new Vector2(0, 0), Scale, SpriteEffects.None, 0);
        }

        public override void Update(int mouseX, int mouseY)
        {
        }
    }
}
