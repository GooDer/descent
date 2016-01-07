
using System;
using DescentDirX.Helpers;

namespace DescentDirX.Dices
{
    class RedDice : AttackDice, IPowerDice
    {
        public override void RollAttack()
        {
            switch (Dice.RollSixSideDice())
            {
                case 1:
                    Damage = 1;
                    Surges = 0;
                    Range = 0;
                    ResultImage = ImageListEnum.DICES_RED_SIDE_0;
                    break;
                case 2:
                    Damage = 2;
                    Surges = 0;
                    Range = 0;
                    ResultImage = ImageListEnum.DICES_RED_SIDE_1;
                    break;
                case 3:
                    Damage = 2;
                    Surges = 0;
                    Range = 0;
                    ResultImage = ImageListEnum.DICES_RED_SIDE_1;
                    break;
                case 4:
                    Damage = 2;
                    Surges = 0;
                    Range = 0;
                    ResultImage = ImageListEnum.DICES_RED_SIDE_1;
                    break;
                case 5:
                    Damage = 3;
                    Surges = 0;
                    Range = 0;
                    ResultImage = ImageListEnum.DICES_RED_SIDE_2;
                    break;
                case 6:
                    Damage = 3;
                    Surges = 1;
                    Range = 0;
                    ResultImage = ImageListEnum.DICES_RED_SIDE_3;
                    break;
            }
        }

        public override string ToString()
        {
            return "Red dice - " + base.ToString();
        }

        public override ImageListEnum GetIcon()
        {
            return ImageListEnum.DICES_RED_ICON;
        }
    }
}
