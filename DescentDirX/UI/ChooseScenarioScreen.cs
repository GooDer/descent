using DescentDirX.BusEvents.GameSetup;
using DescentDirX.Helpers;
using DescentDirX.Scenarios;
using DescentDirX.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DescentDirX.UI
{
    class ChooseScenarioScreen : Screen
    {
        public ChooseScenarioScreen()
        {
            GameButton FirstBloodButton = new TextGameButton(new Vector2(300, 25), new Vector2(GameCamera.GetScreenWidth() / 2 - 150, 400), FirstBloodScenario.NAME);
            FirstBloodButton.RegisterOnClick(ChooseScenario);
            FirstBloodButton.RegisterCallbackObject<Scenario>(new FirstBloodScenario());

            AddButton(FirstBloodButton);
        }

        public override void Update(int mouseX, int mouseY)
        {
            base.Update(mouseX, mouseY);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var logo = ImageProvider.Instance.GetImage(ImageListEnum.LOGO_DESCENT);
            spriteBatch.Draw(logo, new Vector2(GameCamera.GetScreenWidth() / 2 - logo.Width / 2, 20));

            var labelSize = FontFactory.Font.MeasureString("Choose scenario");
            spriteBatch.DrawString(FontFactory.Font, "Choose scenario", new Vector2(GameCamera.GetScreenWidth() / 2 - labelSize.X / 2, 270), ImageProvider.foregroundColor);

            var versionSize = FontFactory.Font.MeasureString(Version.GetVersion());
            spriteBatch.DrawString(FontFactory.Font, Version.GetVersion(), new Vector2(GameCamera.GetScreenWidth() - versionSize.X / 4 - 25, GameCamera.GetScreenHeight() - versionSize.Y / 4 - 10), ImageProvider.foregroundColor, 0, new Vector2(0, 0), 0.35f, SpriteEffects.None, 0);

            base.Draw(spriteBatch);
        }

        private void ChooseScenario(GameButton button, object scenario)
        {
            var sc = scenario as Scenario;
            if (sc != null) {
                MainGame.EVENT_BUS.Post(new ScenarioChoosenEvent(this, sc));
            }
        }

        public override Color GetBackgroundColor()
        {
            return ImageProvider.backgroundColor;
        }
    }
}
