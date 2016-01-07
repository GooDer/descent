using DescentDirX.Helpers;
using DescentDirX.Maps.Tiles.Common;
using Microsoft.Xna.Framework;

namespace DescentDirX.Maps.Tiles
{
    class TileEnterA : Tile
    {
        public TileEnterA(TileRotation rotate = TileRotation.NONE) : base(rotate)
        {
        }

        public override ImageListEnum GetImageName()
        {
            return ImageListEnum.TILES_ENTER_A;
        }

        protected override void Init()
        {
            RightConnectionPoint = new Vector2(Image.Width, Image.Height / 2);
            CreateFields();
            base.Init();
        }
    }
}
