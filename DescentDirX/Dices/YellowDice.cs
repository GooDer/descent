using System;
using DescentDirX.Helpers;

namespace DescentDirX.Dices
{
    class YellowDice : AttackDice, IPowerDice
    {
        public override void RollAttack()
        {
            switch (Dice.RollSixSideDice())
            {
                case 1:
                    Damage = 0;
                    Surges = 1;
                    Range = 1;
                    ResultImage = ImageListEnum.DICES_YELLOW_SIDE_0;
                    break;
                case 2:
                    Damage = 1;
                    Surges = 0;
                    Range = 1;
                    ResultImage = ImageListEnum.DICES_YELLOW_SIDE_1;
                    break;
                case 3:
                    Damage = 1;
                    Surges = 0;
                    Range = 2;
                    ResultImage = ImageListEnum.DICES_YELLOW_SIDE_2;
                    break;
                case 4:
                    Damage = 1;
                    Surges = 1;
                    Range = 0;
                    ResultImage = ImageListEnum.DICES_YELLOW_SIDE_3;
                    break;
                case 5:
                    Damage = 2;
                    Surges = 0;
                    Range = 0;
                    ResultImage = ImageListEnum.DICES_YELLOW_SIDE_4;
                    break;
                case 6:
                    Damage = 2;
                    Surges = 1;
                    Range = 0;
                    ResultImage = ImageListEnum.DICES_YELLOW_SIDE_5;
                    break;
            }
        }

        public override string ToString()
        {
            return "Yellow dice - " + base.ToString();
        }

        public override ImageListEnum GetIcon()
        {
            return ImageListEnum.DICES_YELLOW_ICON;
        }
    }
}
