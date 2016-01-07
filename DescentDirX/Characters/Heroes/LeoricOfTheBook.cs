using System;
using DescentDirX.Classes;
using DescentDirX.Dices;
using DescentDirX.Helpers;

namespace DescentDirX.Characters.Heroes
{
    class LeoricOfTheBook : Hero, IMage
    {
        public const string HERO_NAME = "Leoric of the Book";

        public LeoricOfTheBook() : base(HERO_NAME, 8, 4, 5)
        {
            Might = 1;
            Knowledge = 5;
            Willpower = 2;
            Awareness = 3;
            AddDefenseDice(new GreyDice());
        }

        public override ImageListEnum GetImage()
        {
            return ImageListEnum.HEROES_LEORIC;
        }

        public override ImageListEnum GetHeroSheet()
        {
            return ImageListEnum.HEROES_LEORIC_SHEET;
        }

        public override ImageListEnum GetMapToken()
        {
            return ImageListEnum.HEROES_LEORIC_TOKEN;
        }

        public override void SetHeroClass<IMage>(IMage heroClass)
        {
            base.SetHeroClass(heroClass);
        }

        public override ImageListEnum GetPortrait()
        {
            return ImageListEnum.HEROES_LEORIC_PORTRAIT;
        }
    }
}
