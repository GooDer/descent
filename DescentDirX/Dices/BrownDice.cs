using DescentDirX.Helpers;

namespace DescentDirX.Dices
{
    class BrownDice : DefenseDice
    {
        public override void RollDefense()
        {
            switch (Dice.RollSixSideDice())
            {
                case 1:
                    Defense = 0;
                    ResultImage = ImageListEnum.DICES_BROWN_SIDE_0;
                    break;
                case 2:
                    Defense = 0;
                    ResultImage = ImageListEnum.DICES_BROWN_SIDE_0;
                    break;
                case 3:
                    Defense = 0;
                    ResultImage = ImageListEnum.DICES_BROWN_SIDE_0;
                    break;
                case 4:
                    Defense = 1;
                    ResultImage = ImageListEnum.DICES_BROWN_SIDE_1;
                    break;
                case 5:
                    Defense = 1;
                    ResultImage = ImageListEnum.DICES_BROWN_SIDE_1;
                    break;
                case 6:
                    Defense = 2;
                    ResultImage = ImageListEnum.DICES_BROWN_SIDE_2;
                    break;
            }
        }

        public override string ToString()
        {
            return "Brown dice";
        }

        public override ImageListEnum GetIcon()
        {
            return ImageListEnum.DICES_BROWN_ICON;
        }
    }
}
