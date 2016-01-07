using DescentDirX.Helpers;
using DescentDirX.Maps.Tiles.Common;
using Microsoft.Xna.Framework;

namespace DescentDirX.Maps.Tiles
{
    class TileExitA : Tile
    {
        public TileExitA(TileRotation rotate = TileRotation.NONE) : base(rotate)
        {
        }

        protected override void Init()
        {
            RightConnectionPoint = new Vector2(Image.Width, Image.Height / 2);
            CreateFields();
            base.Init();
        }

        public override ImageListEnum GetImageName()
        {
            return ImageListEnum.TILES_EXIT_A;
        }
    }
}
