using DescentDirX.Helpers;

namespace DescentDirX.Dices
{
    class GreyDice : DefenseDice
    {
        public override void RollDefense()
        {
            switch (Dice.RollSixSideDice())
            {
                case 1:
                    Defense = 0;
                    ResultImage = ImageListEnum.DICES_GREY_SIDE_0;
                    break;
                case 2:
                    Defense = 1;
                    ResultImage = ImageListEnum.DICES_GREY_SIDE_1;
                    break;
                case 3:
                    Defense = 1;
                    ResultImage = ImageListEnum.DICES_GREY_SIDE_1;
                    break;
                case 4:
                    Defense = 1;
                    ResultImage = ImageListEnum.DICES_GREY_SIDE_1;
                    break;
                case 5:
                    Defense = 2;
                    ResultImage = ImageListEnum.DICES_GREY_SIDE_2;
                    break;
                case 6:
                    Defense = 3;
                    ResultImage = ImageListEnum.DICES_GREY_SIDE_3;
                    break;
            }
        }

        public override string ToString()
        {
            return "Grey dice";
        }

        public override ImageListEnum GetIcon()
        {
            return ImageListEnum.DICES_GREY_ICON;
        }
    }
}
