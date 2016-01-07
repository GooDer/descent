using System;
using DescentDirX.Dices;
using DescentDirX.Helpers;

namespace DescentDirX.Items.Weapons.Common
{
    class Unarmed : Weapon<IMeele>
    {
        public Unarmed() : base("Unarmed", 0, 0, 1, new WeaponTypeEnum[] { WeaponTypeEnum.Hand })
        {
            AddAttackDices(new AttackDice[] { new BlueDice() });
        }

        public override ImageListEnum GetImage()
        {
            throw new Exception("Don't have image");
        }

        public override int GetReach()
        {
            return 1;
        }
    }
}
