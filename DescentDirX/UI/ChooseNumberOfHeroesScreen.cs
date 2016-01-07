using DescentDirX.BusEvents.GameSetup;
using DescentDirX.Helpers;
using DescentDirX.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DescentDirX.UI
{
    class ChooseNumberOfHeroesScreen : Screen
    {
        public ChooseNumberOfHeroesScreen()
        {
            GameButton TwoPlayersButton = new TextGameButton(new Vector2(300, 25), new Vector2(GameCamera.GetScreenWidth() / 2 - 150, 400), "2 Heroes");
            GameButton ThreePlayersButton = new TextGameButton(new Vector2(300, 25), new Vector2(GameCamera.GetScreenWidth() / 2 - 150, 450), "3 Heroes");
            GameButton FourPlayersButton = new TextGameButton(new Vector2(300, 25), new Vector2(GameCamera.GetScreenWidth() / 2 - 150, 500), "4 Heroes");

            ThreePlayersButton.Disabled = true;
            FourPlayersButton.Disabled = true;

            AddButton(TwoPlayersButton);
            AddButton(ThreePlayersButton);
            AddButton(FourPlayersButton);

            var i = 2;
            foreach (var button in GetButtons())
            {
                button.RegisterOnClick(ChooseNumberOfHeroes);
                button.RegisterCallbackObject<int>(i);
                i++;
            }
        }

        public override void Update(int mouseX, int mouseY)
        {
            base.Update(mouseX, mouseY);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var logo = ImageProvider.Instance.GetImage(ImageListEnum.LOGO_DESCENT);
            spriteBatch.Draw(logo, new Vector2(GameCamera.GetScreenWidth() / 2 - logo.Width / 2, 20));

            var labelSize = FontFactory.Font.MeasureString("Choose number of heroes");
            spriteBatch.DrawString(FontFactory.Font, "Choose number of heroes", new Vector2(GameCamera.GetScreenWidth() / 2 - labelSize.X / 2, 270), ImageProvider.foregroundColor);

            base.Draw(spriteBatch);
        }

        private void ChooseNumberOfHeroes(GameButton button, object numOfHeroes)
        {
            MainGame.EVENT_BUS.Post(new NumOfHeroesChoosenEvent(this, (int)numOfHeroes));
        }

        public override Color GetBackgroundColor()
        {
            return ImageProvider.backgroundColor;
        }
    }
}
