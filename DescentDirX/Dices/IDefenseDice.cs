using DescentDirX.Helpers;

namespace DescentDirX.Dices
{
    interface IDefenseDice
    {
        void RollDefense();

        int GetDefense(bool getBetterResult = false);

        ImageListEnum GetIcon();

        ImageListEnum GetResultImage();
    }
}
