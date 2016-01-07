using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DescentDirX.Gameplay.Screens
{
    interface IGameScreen
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(int mouseX, int mouseY);
        Color GetBackgroundColor();
        void Hide();
        void Show();
    }
}
