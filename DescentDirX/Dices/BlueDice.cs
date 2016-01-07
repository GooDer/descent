using DescentDirX.Helpers;

namespace DescentDirX.Dices
{
    class BlueDice : AttackDice, IHitDice
    {
        public bool MissedAttack { get; private set; }

        public bool Missed()
        {
            return MissedAttack;
        }

        public override void RollAttack()
        {
            switch (Dice.RollSixSideDice())
            {
                case 1:
                    MissedAttack = true;
                    Damage = 0;
                    Surges = 0;
                    Range = 0;
                    ResultImage = ImageListEnum.DICES_BLUE_SIDE_0;
                    break;
                case 2:
                    MissedAttack = false;
                    Damage = 2;
                    Surges = 1;
                    Range = 2;
                    ResultImage = ImageListEnum.DICES_BLUE_SIDE_1;
                    break;
                case 3:
                    MissedAttack = false;
                    Damage = 2;
                    Surges = 0;
                    Range = 3;
                    ResultImage = ImageListEnum.DICES_BLUE_SIDE_2;
                    break;
                case 4:
                    MissedAttack = false;
                    Damage = 2;
                    Surges = 0;
                    Range = 4;
                    ResultImage = ImageListEnum.DICES_BLUE_SIDE_3;
                    break;
                case 5:
                    MissedAttack = false;
                    Damage = 1;
                    Surges = 0;
                    Range = 5;
                    ResultImage = ImageListEnum.DICES_BLUE_SIDE_4;
                    break;
                case 6:
                    MissedAttack = false;
                    Damage = 1;
                    Surges = 1;
                    Range = 6;
                    ResultImage = ImageListEnum.DICES_BLUE_SIDE_5;
                    break;
            }
        }

        public override string ToString()
        {
            if (Missed())
            {
                return "Blue dice - Missed";
            }

            return "Blue dice - " + base.ToString();
        }

        public override ImageListEnum GetIcon()
        {
            return ImageListEnum.DICES_BLUE_ICON;
        }
    }
}
