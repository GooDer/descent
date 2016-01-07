using DescentDirX.Helpers;

namespace DescentDirX.Items
{
    abstract class Item
    {
        public int BuyPrice { get; private set; }
        public int SellPrice { get; private set; }
        public string Name { get; private set; }

        public Item(string name, int buyprice, int sellPrice)
        {
            BuyPrice = buyprice;
            SellPrice = sellPrice;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public abstract ImageListEnum GetImage();
    }
}
