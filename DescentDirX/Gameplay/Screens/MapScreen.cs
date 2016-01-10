using DescentDirX.Scenarios;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DescentDirX.Characters.Monsters;
using DescentDirX.UI;
using DescentDirX.BusEvents.General;
using NGuava;
using DescentDirX.Characters.Actions;
using DescentDirX.Characters.Heroes;
using DescentDirX.UI.Components;

namespace DescentDirX.Gameplay.Screens
{
    class MapScreen : IGameScreen
    {
        private int actHero;
        private Monster actMonsterToPlace;
        private MapScreenWidget widget;
        private Dialog combatDialog;

        public MapScreen()
        {
            widget = new MapScreenWidget(new Vector2(GameCamera.Instance.ScreenViewport.Width - 300, 0));
            GameCamera.Instance.FixedPosition = false;
            GameCamera.Instance.ReduceViewport(widget.Rect);

            OnFocus();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GameplayProgress.Instance.Scenario.Draw(spriteBatch);

            var actField = GameplayProgress.Instance.Scenario.GetFocusedFieldForPick();
            if (GameplayProgress.Instance.Scenario.Stage == ScenarioStageEnum.HERO_SETUP &&
                actField != null && actField.ObjectsOnField.Count == 0)
            {
                GameplayProgress.Instance.GetHeroes()[actHero].Draw(spriteBatch, actField);
            }

            if (GameplayProgress.Instance.Scenario.Stage == ScenarioStageEnum.OVERLORD_SETUP
                && actField != null && actField.ObjectsOnField.Count == 0 && GameplayProgress.Instance.Scenario.HasUnplacedMonster()
                && actMonsterToPlace != null)
            {
                actMonsterToPlace.Draw(spriteBatch, actField);
            }

            widget.Draw(spriteBatch);
            GameplayProgress.Instance.GetChooseHeroDialog().Draw(spriteBatch);

            if (combatDialog != null)
            {
                combatDialog.Draw(spriteBatch);
            }
        }

        public void Update(int mouseX, int mouseY)
        {
            GameplayProgress.Instance.Scenario.Update(mouseX, mouseY);
            widget.Update(mouseX, mouseY);
            GameplayProgress.Instance.GetChooseHeroDialog().Update(mouseX, mouseY);
            if (combatDialog != null)
            {
                combatDialog.Update(mouseX, mouseY);
            }
        }

        [Subscribe]
        public void OnClick(ClickMessage sourceEvent)
        {
            if (!GameCamera.Instance.ScreenViewport.Bounds.Contains(sourceEvent.MousePosition.ToPoint()))
            {
                return;
            }
            
            switch(GameplayProgress.Instance.Scenario.Stage)
            {
                case ScenarioStageEnum.HERO_SETUP:
                    PickHeroesPlace();
                    break;
                case ScenarioStageEnum.OVERLORD_SETUP:
                    PickMonstersPlace();
                    break;
                case ScenarioStageEnum.HERO_TURN:
                    var character = GameplayProgress.Instance.ActCharacter;
                    var focusedField = GameplayProgress.Instance.Scenario.GetFocusedField();

                    if (character != null && focusedField != null)
                    {
                        var movement = character.GetFieldsForMovement();
                        foreach (var field in movement)
                        {
                            if (field == focusedField && character.ActualAction is MoveAction)
                            {
                                GameplayProgress.Instance.Scenario.ClearMarkedFields();
                                character.MoveToField(field);
                            }
                        }

                        if (character.ActualAction is AttackAction)
                        {
                            var enemy = focusedField.GetCharacterOnSelf();
                            if (enemy != null && character.IsEnemy(enemy) && ((Hero)character).CanAttackOn(enemy))
                            {
                                var resolver = new CombatResolver(character, enemy);
                                resolver.DoAttack();
                                combatDialog = new Dialog(400, 200, "Combat result");
                                var a = 0;
                                foreach (var dice in resolver.Attack.AttackDices)
                                {
                                    combatDialog.AddRendable(a, new ScaledImage(new Vector2(10 + a * 40, 10), dice.GetResultImage()));
                                    a++;
                                }

                                var b = 0;
                                foreach (var dice in resolver.Defense.DefenseDices)
                                {
                                    combatDialog.AddRendable(a + b, new ScaledImage(new Vector2(10 + b * 40, 50), dice.GetResultImage()));
                                    b++;
                                }
                                GameButton button = new TextGameButton(new Vector2(80, 30), new Vector2(10, 120), "Close");
                                button.RegisterOnClick(DialogCloseAction);
                                combatDialog.AddRendable(a + b + 1, button);
                                combatDialog.Show();
                                OnUnfocus();
                            }
                        }
                    }
                    break;
            }
        }

        public Color GetBackgroundColor()
        {
            return Color.Black;
        }

        private void PickHeroesPlace()
        {
            var actFields = GameplayProgress.Instance.GetHeroes()[actHero].GetPlaceableFields(GameplayProgress.Instance.Scenario.GetFocusedFieldForPick());

            if (actFields != null && actFields.Count > 0)
            {
                var field = actFields[0];
                if (field != null && field.ObjectsOnField.Count == 0)
                {
                    GameplayProgress.Instance.GetHeroes()[actHero].Position = field;
                    field.ObjectsOnField.Add(GameplayProgress.Instance.GetHeroes()[actHero]);
                    actHero++;
                }

                if (actHero == GameplayProgress.Instance.GetHeroes().Count)
                {
                    GameplayProgress.Instance.Scenario.NextStage();
                    actHero = 0;
                    PrepareMonstersToPlace();
                }
            }
        }

        private void PrepareMonstersToPlace()
        {
            if (!GameplayProgress.Instance.Scenario.HasUnplacedMonster())
            {
                GameplayProgress.Instance.Scenario.PrepareNextMonsterGroup();
            }

            if (actMonsterToPlace == null)
            {
                actMonsterToPlace = GameplayProgress.Instance.Scenario.GetUnplacedMonster();

                if (actMonsterToPlace == null)
                {
                    GameplayProgress.Instance.Scenario.NextStage();
                    GameplayProgress.Instance.GetChooseHeroDialog().Show();
                }
            }

            GameplayProgress.Instance.ActCharacter = actMonsterToPlace;
        }

        private void PickMonstersPlace()
        {
            if (actMonsterToPlace == null)
            {
                return;
            }

            var fieldForPick = GameplayProgress.Instance.Scenario.GetFocusedFieldForPick();
            var actFields = actMonsterToPlace.GetPlaceableFields(fieldForPick);
            if (actFields != null )
            {
                foreach (var field in actFields)
                {
                    if (field != null && field.ObjectsOnField.Count == 0 && GameplayProgress.Instance.Scenario.HasUnplacedMonster())
                    {
                        field.ObjectsOnField.Add(actMonsterToPlace);
                        actMonsterToPlace.Position = fieldForPick;
                    }
                    else if (field != null)
                    {
                        field.ObjectsOnField.Add(actMonsterToPlace);
                    }
                }
                actMonsterToPlace = null;
            }

            PrepareMonstersToPlace();
        }

        public void Hide()
        {
            
        }

        public void Show()
        {

        }

        private void OnUnfocus()
        {
            MainGame.EVENT_BUS.UnRegister(this);
            widget.OnUnfocus();
        }

        private void OnFocus()
        {
            MainGame.EVENT_BUS.Register(this);
            widget.OnFocus();
        }

        public void DialogCloseAction(GameButton source, object sourceObject)
        {
            if (combatDialog != null)
            {
                combatDialog.Hide();
                OnFocus();
            }
        }
    }
}
