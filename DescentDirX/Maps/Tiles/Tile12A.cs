using DescentDirX.Helpers;
using DescentDirX.Maps.Tiles.Common;
using Microsoft.Xna.Framework;

namespace DescentDirX.Maps.Tiles
{
    class Tile12A : Tile
    {
        public Tile12A (TileRotation rotate = TileRotation.NONE) : base(rotate)
        {
        }

        protected override void Init()
        {
            RightConnectionPoint = new Vector2(Image.Width, Image.Height / 5 * 2);
            BottomConnectionPoint = new Vector2(Image.Width / 5 * 2, Image.Height);
            CreateFields();

            GetFieldAt(1, 1).Terrain = TerrainEnum.WATER;
            GetFieldAt(2, 1).Terrain = TerrainEnum.WATER;
            GetFieldAt(3, 1).Terrain = TerrainEnum.WATER;

            GetFieldAt(1, 2).Terrain = TerrainEnum.WATER;
            GetFieldAt(2, 2).Terrain = TerrainEnum.WATER;
            GetFieldAt(3, 2).Terrain = TerrainEnum.WATER;

            GetFieldAt(1, 3).Terrain = TerrainEnum.WATER;
            GetFieldAt(2, 3).Terrain = TerrainEnum.WATER;

            base.Init();
        }

        public override ImageListEnum GetImageName()
        {
            return ImageListEnum.TILES_12_A;
        }
    }
}
