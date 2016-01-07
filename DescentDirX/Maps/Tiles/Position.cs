using System;

namespace DescentDirX.Maps
{
    class Position
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Position)) return false;

            var other = obj as Position;

            return (X == other.X && Y == other.Y);
        }

        public override int GetHashCode()
        {
            return (X + Y).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("X: {0}; Y: {1}", X, Y);
        }

        public int DistanceFromPosition(Position other)
        {
            var diffX = Math.Abs(X - other.X);
            var diffY = Math.Abs(Y - other.Y);

            return Math.Max(diffX, diffY);
        }

        public int MoveTowardPosition(Position other, int movePoints)
        {
            while (movePoints > 0 && (DistanceFromPosition(other) != 1))
            {
                if (X > other.X)
                {
                    X--;
                } else if (X < other.X)
                {
                    X++;
                }

                if (Y > other.Y)
                {
                    Y--;
                } else if (Y < other.Y)
                {
                    Y++;
                }

                movePoints--;
            }

            return movePoints;
        }
    }
}
