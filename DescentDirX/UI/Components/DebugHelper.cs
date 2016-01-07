using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

namespace DescentDirX.UI.Components
{
    class DebugHelper
    {
        private static Collection<string> texts = new Collection<string>();
        public static Color ForegroundColor { get; set; }
        public static bool DebugOn { get; set; } = false;

        public static void Clear()
        {
            texts.Clear();
        }

        public static void AddText(string text)
        {
            texts.Add(text);
        }

        public static void RenderDebugText(SpriteBatch spriteBatch)
        {
            int pos = 0;
            foreach (var text in texts)
            {
                spriteBatch.DrawString(FontFactory.Font, text, new Vector2(0, pos), ForegroundColor, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);
                pos += 20;
            }
        }
    }
}
