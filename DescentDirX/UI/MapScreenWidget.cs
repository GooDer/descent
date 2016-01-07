using DescentDirX.Characters;
using DescentDirX.Characters.Heroes;
using DescentDirX.Gameplay;
using DescentDirX.Helpers;
using DescentDirX.Characters.Monsters;
using DescentDirX.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DescentDirX.BusEvents.GameProgress;
using NGuava;
using DescentDirX.Characters.Actions;
using System.Collections.Generic;

namespace DescentDirX.UI
{
    class MapScreenWidget : GameObject
    {
        public Rectangle Rect { get; private set; }

        private int actualRow = -1;
        private List<GameButton> buttons = new List<GameButton>();

        public MapScreenWidget(Vector2 position) : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, 300, GameCamera.Instance.ScreenViewport.Height);

            MainGame.EVENT_BUS.Register(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ImageProvider.GetColorTexture(Color.White), Rect, new Color(224, 224, 224));

            Character drawCharacter = null;
            if (GameplayProgress.Instance.FocusedCharacter != null)
            {
                drawCharacter = GameplayProgress.Instance.FocusedCharacter;
            }
            else if (GameplayProgress.Instance.ActCharacter != null)
            {
                drawCharacter = GameplayProgress.Instance.ActCharacter;
            }

            if (drawCharacter != null)
            {
                Texture2D image = ImageProvider.Instance.GetImage(drawCharacter.GetPortrait());
                spriteBatch.Draw(image, position: new Vector2(Rect.X, 0), scale: new Vector2((float)Rect.Width / image.Width));
            }

            Hero hero = drawCharacter as Hero;
            Monster monster = drawCharacter as Monster;

            if (hero != null)
            {
                DrawHero(spriteBatch, hero);
            }

            if (monster != null)
            {
                DrawMonster(spriteBatch, monster);
            }

            foreach (var button in buttons)
            {
                button.Draw(spriteBatch);
            }
        }

        private void DrawHero(SpriteBatch spriteBatch, Hero hero, int offset = 0)
        {
            DrawBasicData(spriteBatch, hero, offset);

            int row = nextRow();
            DrawIcon(spriteBatch, ImageProvider.Instance.GetImage(ImageListEnum.TOKENS_FATIGUE_ICON), row + offset);
            DrawString(spriteBatch, hero.FatigueState(), row + offset);

            row = nextRow();
            DrawIcon(spriteBatch, ImageProvider.Instance.GetImage(ImageListEnum.TOKENS_DEFENSE_ICON), row + offset);
            DrawDefense(spriteBatch, hero, row + offset);

            row = nextRow();
            DrawString(spriteBatch, "Remaining actions: " + hero.RemainingActions, row + offset);
        }

        private void DrawMonster(SpriteBatch spriteBatch, Monster monster, int offset = 0)
        {
            DrawBasicData(spriteBatch, monster, offset);

            int row = nextRow();
            DrawIcon(spriteBatch, ImageProvider.Instance.GetImage(ImageListEnum.TOKENS_DEFENSE_ICON), row + offset);
            DrawDefense(spriteBatch, monster, row + offset);
        }

        private void DrawBasicData(SpriteBatch spriteBatch, Character character, int offset = 0)
        {
            actualRow = 0;
            int row = nextRow();
            DrawCenteredString(spriteBatch, character.Name, row + offset);

            row = nextRow();
            DrawIcon(spriteBatch, ImageProvider.Instance.GetImage(ImageListEnum.TOKENS_DAMAGE_ICON), row + offset);
            DrawString(spriteBatch, character.LivesState(), row + offset);

            row = nextRow();
            DrawIcon(spriteBatch, ImageProvider.Instance.GetImage(ImageListEnum.TOKENS_MOVE_ICON), row + offset);
            DrawString(spriteBatch, character.MovementState(), row + offset);
        }

        private void DrawIcon(SpriteBatch spriteBatch, Texture2D icon, int row)
        {
            spriteBatch.Draw(icon, position: new Vector2(Rect.X + 15, 190 + row * 40), scale: new Vector2(0.75f));
        }

        private void DrawString(SpriteBatch spriteBatch, string text, int row)
        {
            spriteBatch.DrawString(FontFactory.Font, text, new Vector2(Rect.X + 60, 197 + row * 40), Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 0);
        }

        private void DrawDefense(SpriteBatch spriteBatch, Character character, int row)
        {
            var i = 0;
            foreach (var dice in character.GetDefenseDices())
            {
                spriteBatch.Draw(ImageProvider.Instance.GetImage(dice.GetIcon()), position: new Vector2(Rect.X + 50 + i * 30, 190 + row * 40));
                i++;
            }
        }

        private void DrawCenteredString(SpriteBatch spriteBatch, string text, int row)
        {
            int posX = (int)(Rect.X + Rect.Width / 2 - FontFactory.Font.MeasureString(text).X / 4);
            spriteBatch.DrawString(FontFactory.Font, text, new Vector2(posX, 200 + row * 40), Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 0);
        }

        private int nextRow()
        {
            return actualRow++;
        }

        [Subscribe]
        public void OnCharacterSelect(CharacterSelectedEvent charEvent)
        {
            ShowPossibleActions(charEvent.Character);
        }

        private void ShowPossibleActions(Character character)
        {
            ClearButtons();

            if (character != null)
            {
                List<IAction> actions = character.GetPossibleActions();

                var i = 0;
                foreach (var action in actions)
                {
                    var labelSize = FontFactory.Font.MeasureString(action.GetLabel());
                    TextGameButton button = new TextGameButton(new Vector2(labelSize.X / 2 + 40, 40), new Vector2(Rect.X + Rect.Width / 2 - (labelSize.X / 2 + 40) / 2, 500 + 50 * i), action.GetLabel());
                    button.Show();
                    button.RegisterCallbackObject(action);
                    button.RegisterOnClick(OnActionClick);
                    buttons.Add(button);
                    i++;
                }
            }
        }

        private void ClearButtons()
        {
            foreach (var button in buttons)
            {
                button.Hide();
            }
            buttons.Clear();
        }

        public override void Update(int mouseX, int mouseY)
        {
            foreach (var button in buttons)
            {
                button.Update(mouseX, mouseY);
            }
        }

        public void OnActionClick(GameButton source, object action)
        {
            IAction choosenAction = action as IAction;

            if (GameplayProgress.Instance.ActCharacter != null && choosenAction != null)
            {
                choosenAction.PerformActionOn(GameplayProgress.Instance.ActCharacter, GameplayProgress.Instance.Scenario);
                ShowPossibleActions(GameplayProgress.Instance.ActCharacter);
            }
        }

        public void OnFocus()
        {
            MainGame.EVENT_BUS.Register(this);
        }

        public void OnUnfocus()
        {
            MainGame.EVENT_BUS.UnRegister(this);
        }
    }
}
