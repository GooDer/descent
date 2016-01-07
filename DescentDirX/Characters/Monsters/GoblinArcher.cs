using DescentDirX.Dices;
using DescentDirX.Helpers;
using DescentDirX.Items.Weapons.Common;
using DescentDirX.Maps;

namespace DescentDirX.Characters.Monsters
{
    class GoblinArcher : Monster, IMonsterDrawable
    {
        public static int MonsterNum { get; private set; } = 1;

        public GoblinArcher(string name = "Goblin Archer", int maxSpeed = 5, int maxLives = 2, bool attachNumberToName = true) 
            : base(name, maxLives, maxSpeed)
        {
            if (attachNumberToName)
            {
                Name += " " + MonsterNum;
                MonsterNum++;
            }
        }

        protected override void Init()
        {
            AddAttackDices(new AttackDice[] { new BlueDice(), new YellowDice() });
            AddDefenseDice(new GreyDice());
            envs.Add(EnviromentEnum.CAVE);
            envs.Add(EnviromentEnum.BUILDING);
        }

        public ImageListEnum GetMonsterSheetFront()
        {
            return ImageListEnum.MONSTERS_GOBLIN_ARCHER_1_FRONT;
        }

        public ImageListEnum GetMonsterSheetBack()
        {
            return ImageListEnum.MONSTERS_GOBLIN_ARCHER_1_BACK;
        }

        public override ImageListEnum GetMapToken()
        {
            return ImageListEnum.MONSTERS_GOBLIN_TOKEN;
        }

        public override ImageListEnum GetPortrait()
        {
            return ImageListEnum.MONSTERS_GOBLIN_ARCHER_1_PORTRAIT;
        }

        public override int GetReach()
        {
            return Weapon<IRange>.MAX_REACH;
        }
    }
}
