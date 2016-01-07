using DescentDirX.Classes;
using DescentDirX.Dices;
using DescentDirX.Helpers;

namespace DescentDirX.Characters.Heroes
{
    class Syndrael : Hero, IWarrior
    {
        public const string HERO_NAME = "Syndrael";

        public Syndrael() : base(HERO_NAME, 12, 4, 4)
        {
            Might = 4;
            Knowledge = 3;
            Willpower = 2;
            Awareness = 2;
            AddDefenseDice(new GreyDice());
        }

        public override ImageListEnum GetImage()
        {
            return ImageListEnum.HEROES_SYNDRAEL;
        }

        public override ImageListEnum GetHeroSheet()
        {
            return ImageListEnum.HEROES_SYNDRAEL_SHEET;
        }

        public override ImageListEnum GetMapToken()
        {
            return ImageListEnum.HEROES_SYNDRAEL_TOKEN;
        }

        public override void SetHeroClass<IWarrior>(IWarrior heroClass)
        {
            base.SetHeroClass(heroClass);
        }

        public override ImageListEnum GetPortrait()
        {
            return ImageListEnum.HEROES_SYNDRAEL_PORTRAIT;
        }
    }
}
