using DescentDirX.Helpers;
using DescentDirX.Items;
using System.Collections.ObjectModel;

namespace DescentDirX.Classes
{
    interface IHeroClass
    {
        Collection<Item> getClassEquipment();

        ImageListEnum GetImage();
    }
}
