using DescentDirX.BusEvents.GameProgress;
using DescentDirX.Characters;
using DescentDirX.Characters.Heroes;
using DescentDirX.Characters.Monsters;
using DescentDirX.Scenarios;
using DescentDirX.UI.Components;
using Microsoft.Xna.Framework;
using NGuava;
using System.Collections.Generic;
using System.Text;

namespace DescentDirX.Gameplay
{
    delegate int SpendSurges(DefenseResult def, AttackResult att, StringBuilder strBuilder);

    class GameplayProgress
    {
        private static GameplayProgress instance;
        public static GameplayProgress Instance {
            get {
                if (instance == null)
                {
                    instance = new GameplayProgress();
                }

                return instance;
            }

            private set {
                instance = value;
            }
        }

        private static int turn = 1;

        private List<Hero> heroes { get; set; }
        public List<Monster> Monsters { get; private set; }

        private Character actualCharacter;
        public Character ActCharacter {
            get {
                return actualCharacter;
            }
            set {
                actualCharacter = value;
                MainGame.EVENT_BUS.Post(new CharacterSelectedEvent(this, value));
            }
        }
        public Character FocusedCharacter { get; set; }

        public Scenario Scenario { get; set; }

        private Dialog dialog = new Dialog(700, 500, "Choose next hero");

        private GameplayProgress()
        {
            heroes = new List<Hero>();
            Monsters = new List<Monster>();
            MainGame.EVENT_BUS.Register(this);
        }

        [Subscribe]
        public void OnFieldFocus(FieldFocusedEvent fieldEvent)
        {
            Character character = null;
            if (fieldEvent.Field.ObjectsOnField.Count > 0)
            {
                character = fieldEvent.Field.ObjectsOnField[0] as Character;
            }

            FocusedCharacter = character;
        }

        public List<Hero> GetHeroes()
        {
            return heroes;
        }

        public void SetHeroes(List<Hero> heroes)
        {
            this.heroes = heroes;

            int i = 0;
            foreach (var hero in this.heroes)
            {
                var image = new ScaledImage(new Vector2(50 + 50 * i, 50), hero.GetImage(), 0.35f);
                image.AlterPositionX(image.GetWidth() * i);
                ImageGameButton button = new ImageGameButton(image);
                button.RegisterOnClick(this.OnHeroChoose);
                button.RegisterCallbackObject(hero);
                dialog.AddRendable(i, button);
                i++;
            }
        }

        public Dialog GetHeroeChooseDialog()
        {
            return dialog;
        }

        private void OnHeroChoose(GameButton source, object hero)
        {
            source.Disabled = true;
            var actHero = hero as Hero;
            dialog.Hide();
            actHero.Upkeep();
            ActCharacter = actHero;
        }

        private void EndTurn()
        {
            turn++;
            ActCharacter.Upkeep();
        }
    }
}
