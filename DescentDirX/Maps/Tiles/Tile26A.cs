using DescentDirX.Maps.Tiles.Common;
using DescentDirX.Helpers;
using Microsoft.Xna.Framework;

namespace DescentDirX.Maps.Tiles
{
    class Tile26A : Tile
    {
        public Tile26A(TileRotation rotate = TileRotation.NONE) : base(rotate)
        {
        }

        public override ImageListEnum GetImageName()
        {
            return ImageListEnum.TILES_26_A;
        }

        protected override void Init()
        {
            TopConnectionPoint = new Vector2(Width / 2, 0);
            BottomConnectionPoint = new Vector2(Width / 2, Height);

            CreateFields();
            base.Init();
        }
    }
}
