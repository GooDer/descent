using DescentDirX.Characters.Heroes;
using DescentDirX.Classes;
using DescentDirX.BusEvents.GameSetup;
using DescentDirX.Helpers;
using DescentDirX.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DescentDirX.UI
{
    class ChooseHerosScreen : Screen
    {
        private Hero actualHero = null;
        private SubScreen subScreen = SubScreen.HERO_SCREEN;

        private List<Hero> heroes;

        private int NumOfHeroes { get; set; }

        private Dictionary<Hero, GameButton> heroesButtons;
        private Dictionary<IHeroClass, GameButton> classesButtons;

        private Collection<Texture2D> FocusedImages { get; set; }

        public ChooseHerosScreen(int numOfHeroes)
        {
            NumOfHeroes = numOfHeroes;
            heroesButtons = new Dictionary<Hero, GameButton>();
            classesButtons = new Dictionary<IHeroClass, GameButton>();

            heroesButtons.Add(new Syndrael(), new ImageGameButton(new ScaledImage(new Vector2(20, 20), ImageListEnum.HEROES_SYNDRAEL, 0.5f)));
            heroesButtons.Add(new LeoricOfTheBook(), new ImageGameButton(new ScaledImage(new Vector2(20, 350), ImageListEnum.HEROES_LEORIC, 0.5f)));

            classesButtons.Add(new Knight(), new ImageGameButton(new ScaledImage(new Vector2(20, 370), ImageListEnum.CLASSES_KNIGHT, 0.5f)));
            classesButtons.Add(new Runemaster(), new ImageGameButton(new ScaledImage(new Vector2(20, 310), ImageListEnum.CLASSES_RUNEMASTER, 0.5f)));

            heroes = new List<Hero>();
            FocusedImages = new Collection<Texture2D>();

            InitHeroScreen();
        }

        private void InitHeroScreen()
        {
            ClearButtons();

            foreach (var hero in heroesButtons.Keys)
            {
                GameButton button;
                heroesButtons.TryGetValue(hero, out button);
                button.RegisterOnFocus(OnFocus);
                button.RegisterOnFocusLost(OnFocusLost);
                button.RegisterOnClick(OnHeroChoose);
                button.RegisterCallbackObject(hero);
                AddButton(button);
            }
        }

        private void InitClassScreen()
        {
            ClearButtons();

            foreach (var classes in actualHero.GetPossibleClasses())
            {
                GameButton button = new ImageGameButton(new ScaledImage(new Vector2(20, 150), classes.GetImage(), 0.5f));
                button.RegisterOnFocus(OnFocus);
                button.RegisterOnFocusLost(OnFocusLost);
                button.RegisterOnClick(OnClassChoose);
                button.RegisterCallbackObject(classes);
                AddButton(button);
            }
        }

        public override void Update(int mouseX, int mouseY)
        {
            base.Update(mouseX, mouseY);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var label = "Choose " + (subScreen == SubScreen.HERO_SCREEN ? "hero" : "class for hero") + " number " + (heroes.Count + 1);
            var labelSize = FontFactory.Font.MeasureString(label);
            spriteBatch.DrawString(FontFactory.Font, label, new Vector2(GameCamera.GetScreenWidth() / 2 - labelSize.X / 2, 50), ImageProvider.foregroundColor);

            base.Draw(spriteBatch);

            if (FocusedImages.Count > 0)
            {
                var i = 0;
                foreach (var img in FocusedImages)
                {
                    var posX = GameCamera.GetScreenWidth() / 2 - img.Width / 2 + (i * 300) - ((FocusedImages.Count - 1) * 150);
                    spriteBatch.Draw(img, new Vector2(posX, GameCamera.GetScreenHeight() / 2 - img.Height / 2));
                    i++;
                }
            }
        }

        private void OnHeroChoose(GameButton source, object hero)
        {
            source.Disabled = true;
            heroes.Add((Hero)hero);
            actualHero = (Hero)hero;
            subScreen = SubScreen.CLASS_SCREEN;
            FocusedImages.Clear();
            InitClassScreen();
        }

        private void OnClassChoose(GameButton source, object heroClass)
        {
            source.Disabled = true;
            actualHero.SetHeroClass((IHeroClass)heroClass);

            if (heroes.Count == NumOfHeroes)
            {
                MainGame.EVENT_BUS.Post(new HeroesChoosenEvent(this, heroes));
            }
            else
            {
                subScreen = SubScreen.CLASS_SCREEN;
                FocusedImages.Clear();
                InitHeroScreen();
            }
        }

        private void OnFocus(GameButton source, object sourceObj)
        {
            Hero hero = sourceObj as Hero;
            IHeroClass heroClass = sourceObj as IHeroClass;

            if (hero != null)
            {
                FocusedImages.Add(ImageProvider.Instance.GetImage(hero.GetHeroSheet()));
            }
            else if (heroClass != null)
            {
                foreach (var item in heroClass.getClassEquipment())
                {
                    FocusedImages.Add(ImageProvider.Instance.GetImage(item.GetImage()));
                }
            }
        }

        private void OnFocusLost()
        {
            FocusedImages.Clear();
        }

        public override Color GetBackgroundColor()
        {
            return ImageProvider.backgroundColor;
        }

        private enum SubScreen
        {
            HERO_SCREEN,
            CLASS_SCREEN
        }
    }
}
