using DescentDirX.Helpers;
using DescentDirX.Maps.Tiles.Common;
using Microsoft.Xna.Framework;

namespace DescentDirX.Maps.Tiles
{
    class Tile16A : Tile
    {
        public Tile16A(TileRotation rotate = TileRotation.NONE) : base(rotate)
        {

        }

        public override ImageListEnum GetImageName()
        {
            return ImageListEnum.TILES_16_A;
        }

        protected override void Init()
        {
            TopConnectionPoint = new Vector2(Width / 2, 0);
            BottomConnectionPoint = new Vector2(Width / 2, Height);
            RightConnectionPoint = new Vector2(Width, Height / 2);
            LeftConnectionPoint = new Vector2(0, Height / 2);
            CreateFields();
            base.Init();
        }
    }
}
