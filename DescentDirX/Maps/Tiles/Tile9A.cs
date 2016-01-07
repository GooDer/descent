using DescentDirX.Maps.Tiles.Common;
using Microsoft.Xna.Framework;
using DescentDirX.Helpers;

namespace DescentDirX.Maps.Tiles
{
    class Tile9A : Tile
    {
        public Tile9A(TileRotation rotate = TileRotation.NONE) : base(rotate)
        {
        }

        public override ImageListEnum GetImageName()
        {
            return ImageListEnum.TILES_9_A;
        }

        protected override void Init()
        {
            RightConnectionPoint = new Vector2(Width, Height / 2);
            LeftConnectionPoint = new Vector2(0, Height / 2);
            TopConnectionPoint = new Vector2(Width / 2, 0);
            CreateFields();
            base.Init();
        }
    }
}
