using DescentDirX.Helpers;
using DescentDirX.Maps.Tiles.Common;
using Microsoft.Xna.Framework;

namespace DescentDirX.Maps.Tiles
{
    class TileEdgeA : Tile
    {
        public TileEdgeA(TileRotation rotate = TileRotation.NONE) : base(rotate)
        {
        }

        public override ImageListEnum GetImageName()
        {
            return ImageListEnum.TILES_EDGE_A;
        }

        protected override void Init()
        {
            TopConnectionPoint = new Vector2(Width / 2, 0);
            CreateFields();
            base.Init();
        }
    }
}
