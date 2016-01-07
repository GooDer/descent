using DescentDirX.Gameplay.Screens;
using DescentDirX.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DescentDirX.UI
{
    abstract class Screen : IGameScreen
    {
        private List<GameButton> buttons;

        public Screen()
        {
            buttons = new List<GameButton>();
        }

        public void AddButton(GameButton button)
        {
            button.Show();
            buttons.Add(button);
        }

        public List<GameButton> GetButtons()
        {
            return buttons;
        }

        public void ClearButtons()
        {
            foreach (var button in buttons)
            {
                button.Hide();
            }
            buttons.Clear();
        }

        public virtual void Update(int MouseX, int MouseY)
        {
            foreach (var button in GetButtons())
            {
                button.Update(MouseX, MouseY);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in GetButtons())
            {
                button.Draw(spriteBatch);
            }
        }

        private GameButton GetFocusedButton()
        {
            foreach (var button in GetButtons())
            {
                if (button.IsFocused())
                {
                    return button;
                }
            }

            return null;
        }

        public abstract Color GetBackgroundColor();

        public void Hide()
        {
            foreach (var button in GetButtons())
            {
                button.Hide();
            }
        }

        public void Show()
        {
            foreach (var button in GetButtons())
            {
                button.Show();
            }
        }
    }
}
