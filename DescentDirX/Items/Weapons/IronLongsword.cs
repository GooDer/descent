using DescentDirX.Dices;
using DescentDirX.Items.Weapons.Common;
using DescentDirX.Abilities;
using DescentDirX.Helpers;
using System;

namespace DescentDirX.Items.Weapons
{
    class IronLongsword : Weapon<IMeele>, IHasSurgeAbility
    {
        public IronLongsword() : base("Iron Longsword", 0, 25, 1, new WeaponTypeEnum[] { WeaponTypeEnum.Blade })
        {
            AddAttackDices(new AttackDice[] { new BlueDice(), new RedDice() });
        }

        public SurgeAbility[] GetSurgeAbilities()
        {
            return new SurgeAbility[] { new ForceRerollDefenseAbility(1, "force defender to reroll one defense dice") };
        }

        public override ImageListEnum GetImage()
        {
            return ImageListEnum.CLASSES_KNIGHT_IRON_LONGSWORD;
        }

        public override int GetReach()
        {
            return 1;
        }
    }
}
