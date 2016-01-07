using DescentDirX.Abilities;
using DescentDirX.Dices;
using DescentDirX.Helpers;
using DescentDirX.Items.Weapons.Common;

namespace DescentDirX.Items.Weapons
{
    class ArcaneBolt : Weapon<IRange>, IHasSurgeAbility
    {
        public ArcaneBolt() : base("Arcane Bolt", 0, 25, 2, new WeaponTypeEnum[] { WeaponTypeEnum.Magic, WeaponTypeEnum.Rune })
        {
            AddAttackDices(new AttackDice[] { new BlueDice(), new YellowDice() });
        }

        public SurgeAbility[] GetSurgeAbilities()
        {
            return new SurgeAbility[] { new RangeAbility(1, 1), new PierceAbility(1, 2) };
        }

        public override ImageListEnum GetImage()
        {
            return ImageListEnum.CLASSES_RUNEMASTER_ARCANE_BOLT;
        }

        public override int GetReach()
        {
            return Weapon<IRange>.MAX_REACH;
        }
    }
}
