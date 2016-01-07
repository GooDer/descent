using DescentDirX.Dices;
using DescentDirX.Helpers;
using DescentDirX.Maps;

namespace DescentDirX.Characters.Monsters
{
    class MasterGoblinArcher : GoblinArcher, IMonsterMaster, IMonsterDrawable
    {
        public MasterGoblinArcher(string name = "Master Goblin Archer", int maxLives = 4, int maxSpeed = 5) : base(name, maxSpeed, maxLives, false)
        {
        }

        public override ImageListEnum GetMapToken()
        {
            return ImageListEnum.MONSTERS_GOBLIN_MASTER_TOKEN;
        }

        protected override void Init()
        {
            AddAttackDices(new AttackDice[] { new BlueDice(), new YellowDice() });
            AddDefenseDice(new GreyDice());
            envs.Add(EnviromentEnum.CAVE);
            envs.Add(EnviromentEnum.BUILDING);
        }
    }
}
