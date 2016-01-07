using DescentDirX.Helpers;

namespace DescentDirX.Characters.Monsters
{
    interface IMonsterDrawable
    {
        ImageListEnum GetMonsterSheetFront();

        ImageListEnum GetMonsterSheetBack();

        ImageListEnum GetMapToken();
    }
}
