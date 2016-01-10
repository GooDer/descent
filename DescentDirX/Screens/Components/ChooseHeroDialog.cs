using DescentDirX.Characters.Heroes;
using DescentDirX.UI.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DescentDirX.Screens.Components
{
    class ChooseHeroDialog : Dialog
    {
        private List<Hero> heroes;

        public ChooseHeroDialog(int width, int height, List<Hero> heroes) : base(width, height, "Choose hero")
        {
            this.heroes = heroes;

            int i = 0;
            foreach (var character in this.heroes)
            {
                var image = new ScaledImage(new Vector2(50 + 50 * i, 50), character.GetImage(), 0.35f);
                image.AlterPositionX(image.GetWidth() * i);
                ImageGameButton button = new ImageGameButton(image);
                button.RegisterCallbackObject(character);
                AddRendable(i, button);
                i++;
            }
        }

        public void RegisterOnCharacterClick(OnClick callback)
        {
            foreach (var rendable in GetRendables()) {
                var button = rendable as GameButton;
                if (button != null)
                {
                    button.RegisterOnClick(callback);
                }
            }
        }
    }
}
