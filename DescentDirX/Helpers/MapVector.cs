using DescentDirX.Maps.Tiles.Common;
using Microsoft.Xna.Framework;

namespace DescentDirX.Helpers
{
    class MapVector
    {
        public static Vector2 GetInversedVector(Vector2 vect)
        {
            var x = vect.X;
            vect.X = vect.Y;
            vect.Y = x;

            return vect;
        }

        public static Vector2 GetInversedVector(Vector2 vect, int width, int height)
        {
            if (vect.X < width)
            {
                vect.X = width - vect.X;
            }

            if (vect.Y < height)
            {
                vect.Y = height - vect.Y;
            }
            return GetInversedVector(vect);
        }

        public static Vector2? GetInversedVector(Vector2? vect, int width, int height, TileRotation rotation)
        {
            Vector2? newVector = null;

            if (vect == null)
            {
                return null;
            }

            switch (rotation)
            {
                case TileRotation.DEGREE_90:
                    newVector = new Vector2(width - vect.Value.Y, vect.Value.X);
                    break;
                case TileRotation.DEGREE_180:
                    newVector = new Vector2(width - vect.Value.X, height - vect.Value.Y);
                    break;
                case TileRotation.DEGREE_270:
                    newVector = new Vector2(vect.Value.Y, height - vect.Value.X);
                    break;
            }

            return newVector;
        }
    }
}
