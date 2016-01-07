using DescentDirX.Helpers;
using DescentDirX.Maps.Tiles.Common;
using Microsoft.Xna.Framework;

namespace DescentDirX.Maps.Tiles
{
    class Tile8A : Tile
    {
        public Tile8A(TileRotation rotate = TileRotation.NONE) : base(rotate)
        {
        }

        public override ImageListEnum GetImageName()
        {
            return ImageListEnum.TILES_8_A;
        }

        protected override void Init()
        {
            TopConnectionPoint = new Vector2(Width / 2, 0);
            RightConnectionPoint = new Vector2(Width, Height / 2);
            CreateFields();
            base.Init();
        }
    }
}
