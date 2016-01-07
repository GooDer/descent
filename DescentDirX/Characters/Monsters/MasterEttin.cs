using DescentDirX.Dices;
using DescentDirX.Helpers;
using DescentDirX.Maps;

namespace DescentDirX.Characters.Monsters
{
    class MasterEttin : Ettin, IMonsterDrawable, IMonsterMaster
    {
        public MasterEttin(string name = "Master Ettin", int maxSpeed = 3, int maxLives = 8) : base(name, maxSpeed, maxLives)
        {
        }

        protected override void Init()
        {
            AddAttackDices(new AttackDice[] { new BlueDice(), new RedDice() });
            AddDefenseDices(new DefenseDice[] { new GreyDice(), new GreyDice() });
            envs.Add(EnviromentEnum.CAVE);
            envs.Add(EnviromentEnum.MOUNTAIN);
        }

        public override ImageListEnum GetMapToken()
        {
            return ImageListEnum.MONSTERS_ETTIN_MASTER_TOKEN;
        }
    }
}
