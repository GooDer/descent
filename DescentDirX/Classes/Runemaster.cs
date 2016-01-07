using System.Collections.ObjectModel;
using DescentDirX.Helpers;
using DescentDirX.Items;
using DescentDirX.Items.Weapons;

namespace DescentDirX.Classes
{
    class Runemaster : IMage
    {
        public const string CLASS_NAME = "Runemaster";

        public Collection<Item> getClassEquipment()
        {
            return new Collection<Item>() { new ArcaneBolt() };
        }

        public ImageListEnum GetImage()
        {
            return ImageListEnum.CLASSES_RUNEMASTER;
        }

        public override string ToString()
        {
            return CLASS_NAME;
        }
    }
}
