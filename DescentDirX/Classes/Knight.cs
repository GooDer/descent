using DescentDirX.Items;
using DescentDirX.Items.Weapons;
using System.Collections.ObjectModel;
using DescentDirX.Helpers;
using System;

namespace DescentDirX.Classes
{
    class Knight : IWarrior
    {
        public const string CLASS_NAME = "Knight";

        public Collection<Item> getClassEquipment()
        {
            return new Collection<Item>() { new IronLongsword() };
        }

        public ImageListEnum GetImage()
        {
            return ImageListEnum.CLASSES_KNIGHT;
        }

        public override string ToString()
        {
            return CLASS_NAME;
        }
    }
}
