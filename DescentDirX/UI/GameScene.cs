using DescentDirX.BusEvents.GameSetup;
using DescentDirX.Gameplay;
using DescentDirX.Gameplay.Screens;
using DescentDirX.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NGuava;

namespace DescentDirX.UI
{
    class GameScene
    {
        private int NumOfHeroes { get; set; }
        private GameStateEnum GameState { get; set; }
        private IGameScreen ActualScreen { get; set; }

        public GameScene()
        {
            RestartGame();

            MainGame.EVENT_BUS.Register(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ActualScreen.Draw(spriteBatch);

            DebugHelper.RenderDebugText(spriteBatch);
        }

        public void Update(int mouseX, int mouseY)
        {
            GameCamera.Instance.Update(mouseX, mouseY);

            ActualScreen.Update(mouseX, mouseY);
        }

        public Color GetBackgroundColor()
        {
            return ActualScreen.GetBackgroundColor();
        }

        public void RestartGame()
        {
            if (ActualScreen != null)
            {
                ActualScreen.Hide();
            }
            GameState = GameStateEnum.CHOOSE_SCENARIO;
            ActualScreen = new ChooseScenarioScreen();
            ActualScreen.Show();
        }

        [Subscribe]
        public void ScenarioChoosen(ScenarioChoosenEvent sourceEvent)
        {
            if (ActualScreen != null)
            {
                ActualScreen.Hide();
            }

            GameplayProgress.Instance.Scenario = sourceEvent.SelectedScenario;
            GameState = GameStateEnum.CHOOSE_NUM_OF_HEROES;
            ActualScreen = new ChooseNumberOfHeroesScreen();
            ActualScreen.Show();
        }

        [Subscribe]
        public void NumOfHeroesChoosen(NumOfHeroesChoosenEvent sourceEvent)
        {
            if (ActualScreen != null)
            {
                ActualScreen.Hide();
            }

            NumOfHeroes = sourceEvent.NumOfHeroes;
            GameState = GameStateEnum.PICK_HEROES;
            ActualScreen = new ChooseHerosScreen(NumOfHeroes);
            ActualScreen.Show();
        }

        [Subscribe]
        public void AllHeroesChoosen(HeroesChoosenEvent sourceEvent)
        {
            if (ActualScreen != null)
            {
                ActualScreen.Hide();
            }

            GameplayProgress.Instance.SetHeroes(sourceEvent.Heroes);
            ActualScreen = new MapScreen();
            ActualScreen.Show();
        }
    }
}
