using DescentDirX.Helpers;
using DescentDirX.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DescentDirX.Maps.Tiles.Common
{
    abstract class Tile
    {
        private const int EDGE_DETECTION = 15;
        private const int TRANSPARENT_DETECTION = 50;

        public Texture2D Image { get; private set; }
        public TileRotation Rotation { get; private set; }

        private ContentManager TheContentManager { get; set; }

        public Collection<Field> Fields { get; private set; }

        protected Vector2? TopConnectionPoint { get; set; }
        protected Vector2? BottomConnectionPoint { get; set; }
        protected Vector2? LeftConnectionPoint { get; set; }
        protected Vector2? RightConnectionPoint { get; set; }

        private Vector2 CurrentPosition { get; set; }
        private Vector2 Origin { get; set; }
        private float Rotate { get; set; }

        private int offsetX;
        private int offsetY;

        private int OffsetX {
            get { return offsetX; }
            set
            {
                offsetX = value;
                foreach (var field in Fields)
                {
                    field.OffsetX = value;
                }
            }
        }

        private int OffsetY {
            get { return offsetY; }
            set
            {
                offsetY = value;
                foreach (var field in Fields)
                {
                    field.OffsetY = value;
                }
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Tile TileOnLeft { get; set; }
        public Tile TileOnRight { get; set; }
        public Tile TileOnTop { get; set; }
        public Tile TileOnBottom { get; set; }

        public Tile(TileRotation rotate = TileRotation.NONE)
        {
            Image = ImageProvider.Instance.GetImage(GetImageName());
            Rotation = rotate;
            Width = Image.Width;
            Height = Image.Height;
            Fields = new Collection<Field>();
            Init();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, GameCamera.Instance.GetMovedVector(GetCurrentPos()), null, Color.White, Rotate, Origin, 1, SpriteEffects.None, 0);

            foreach (var field in Fields)
            {
                field.Draw(spriteBatch);
            }
        }

        public void Update(int mouseX, int mouseY)
        {
            foreach (var field in Fields)
            {
                field.Update(mouseX, mouseY);
            }
        }

        public Vector2 GetCurrentPos()
        {
            return new Vector2(CurrentPosition.X + OffsetX, CurrentPosition.Y + OffsetY);
        }

        public void ConnectTileToRight(Tile tile)
        {
            if (RightConnectionPoint == null)
            {
                return;
            }

            TileOnRight = tile;
            tile.TileOnLeft = this;

            var diffY = (int)(RightConnectionPoint.Value.Y - tile.LeftConnectionPoint.Value.Y);
            tile.MoveOffsetX((int)RightConnectionPoint.Value.X + OffsetX);
            tile.MoveOffsetY(diffY + OffsetY);
        }

        public void ConnectTileToLeft(Tile tile)
        {
            if (LeftConnectionPoint == null)
            {
                return;
            }

            TileOnLeft = tile;
            tile.TileOnRight = this;

            var diffY = (int)(LeftConnectionPoint.Value.Y - tile.RightConnectionPoint.Value.Y);
            tile.MoveOffsetX((int)LeftConnectionPoint.Value.X + OffsetX - tile.Width, this);
            tile.MoveOffsetY(diffY + OffsetY, this);
        }

        public void ConnectTileToBottom(Tile tile)
        {
            if (BottomConnectionPoint == null)
            {
                return;
            }

            TileOnBottom = tile;
            tile.TileOnTop = this;

            var diffX = (int)(BottomConnectionPoint.Value.X - tile.TopConnectionPoint.Value.X);
            tile.MoveOffsetY((int)BottomConnectionPoint.Value.Y + OffsetY);
            tile.MoveOffsetX(diffX + OffsetX);
        }

        public void ConnectTileToTop(Tile tile)
        {
            if (TopConnectionPoint == null)
            {
                return;
            }

            TileOnTop = tile;
            tile.TileOnBottom = this;

            var diffX = (int)(TopConnectionPoint.Value.X - tile.BottomConnectionPoint.Value.X);
            tile.MoveOffsetY((int)TopConnectionPoint.Value.Y + OffsetY - tile.Height);
            tile.MoveOffsetX(diffX + OffsetX, this);
        }

        public void MoveOffsetX(int x, Tile startingTile = null, bool recursive = true)
        {
            OffsetX += x;

            if (!recursive)
            {
                return;
            }

            if (TileOnRight != null && startingTile != TileOnRight)
            {
                TileOnRight.MoveOffsetX(x, this);
            }

            if (TileOnBottom != null && startingTile != TileOnBottom)
            {
                TileOnBottom.MoveOffsetX(x, this);
            }
        }

        public void MoveOffsetY(int y, Tile startingTile = null, bool recursive = true)
        {
            OffsetY += y;

            if (!recursive)
            {
                return;
            }

            if (TileOnRight != null && startingTile != TileOnRight)
            {
                TileOnRight.MoveOffsetY(y);
            }
        }

        protected virtual void Init()
        {
            SwapSizeOnRotate();

            if (Rotation != TileRotation.NONE)
            {
                RecountConnectionPointsOnRotate();
            }

            Origin = new Vector2(Image.Width / 2, Image.Height / 2);

            switch (Rotation)
            {
                case TileRotation.DEGREE_90:
                    Rotate = (float)Math.PI / 2;
                    break;
                case TileRotation.DEGREE_180:
                    Rotate = (float)Math.PI;
                    break;
                case TileRotation.DEGREE_270:
                    Rotate = (float)Math.PI + (float)Math.PI / 2;
                    break;
            }

            connectFields();
        }

        private void SwapSizeOnRotate()
        {
            switch (Rotation)
            {
                case TileRotation.DEGREE_90:
                    Height = Image.Width;
                    Width = Image.Height;
                    break;
                case TileRotation.DEGREE_270:
                    Height = Image.Width;
                    Width = Image.Height;
                    break;
            }

            CurrentPosition = new Vector2(Width / 2, Height / 2);
        }

        private void RecountConnectionPointsOnRotate()
        {
            var tempRight = RightConnectionPoint;
            var tempLeft = LeftConnectionPoint;
            var tempTop = TopConnectionPoint;
            var tempBottom = BottomConnectionPoint;

            switch (Rotation)
            {
                case TileRotation.DEGREE_90:
                    BottomConnectionPoint = MapVector.GetInversedVector(tempRight, Width, Height, Rotation);
                    LeftConnectionPoint = MapVector.GetInversedVector(tempBottom, Width, Height, Rotation);
                    TopConnectionPoint = MapVector.GetInversedVector(tempLeft, Width, Height, Rotation);
                    RightConnectionPoint = MapVector.GetInversedVector(tempTop, Width, Height, Rotation);
                    break;
                case TileRotation.DEGREE_180:
                    BottomConnectionPoint = MapVector.GetInversedVector(tempTop, Width, Height, Rotation);
                    LeftConnectionPoint = MapVector.GetInversedVector(tempRight, Width, Height, Rotation);
                    TopConnectionPoint = MapVector.GetInversedVector(tempBottom, Width, Height, Rotation);
                    RightConnectionPoint = MapVector.GetInversedVector(tempLeft, Width, Height, Rotation);
                    break;
                case TileRotation.DEGREE_270:
                    BottomConnectionPoint = MapVector.GetInversedVector(tempLeft, Width, Height, Rotation);
                    LeftConnectionPoint = MapVector.GetInversedVector(tempTop, Width, Height, Rotation);
                    TopConnectionPoint = MapVector.GetInversedVector(tempRight, Width, Height, Rotation);
                    RightConnectionPoint = MapVector.GetInversedVector(tempBottom, Width, Height, Rotation);
                    break;
            }

            foreach (var field in Fields)
            {
                field.Rotate(Rotation, Width, Height);
            }
        }

        protected bool IsTransparent(Color[] data)
        {
            var index = 0;
            foreach (var col in data)
            {
                index += col.A;
            }
            return (index / data.Length) < TRANSPARENT_DETECTION;
        }

        protected bool TopClosed(Color[] data)
        {
            int sum = 0;
            int alphaSum = 0;
            for (int x = 0; x < Field.SIZE; x++)
            {
                sum += (data[x].B + data[x].G + data[x].R) / 3;
                alphaSum += data[x].A;
            }

            return (sum / Field.SIZE <= EDGE_DETECTION);
        }

        protected bool BottomClosed(Color[] data)
        {
            int sum = 0;
            int alphaSum = 0;
            for (int x = Field.SIZE * Field.SIZE - Field.SIZE; x < Field.SIZE * Field.SIZE; x++)
            {
                sum += (data[x].B + data[x].G + data[x].R) / 3;
                alphaSum += data[x].A;
            }

            return (sum / Field.SIZE <= EDGE_DETECTION);
        }

        protected bool LeftClosed(Color[] data)
        {
            int sum = 0;
            int alphaSum = 0;
            for (int x = 0; x < (Field.SIZE * Field.SIZE); x = (x + Field.SIZE))
            {
                sum += (data[x].B + data[x].G + data[x].R) / 3;
                alphaSum += data[x].A;
            }

            return (sum / Field.SIZE <= EDGE_DETECTION);
        }

        protected bool RightClosed(Color[] data)
        {
            int sum = 0;
            int alphaSum = 0;
            for (int x = (Field.SIZE - 1); x < Field.SIZE * Field.SIZE; x += Field.SIZE)
            {
                sum += (data[x].B + data[x].G + data[x].R) / 3;
                alphaSum += data[x].A;
            }

            return (sum / Field.SIZE <= EDGE_DETECTION);
        }


        protected void CreateFields()
        {
            var imgDimension = Field.SIZE * Field.SIZE;
            Color[] data = new Color[imgDimension];

            for (var x = 0; x < Width / Field.SIZE; x++)
            {
                var xDimension = x * Field.SIZE;
                for (var y = 0; y < Height / Field.SIZE; y++)
                {
                    var yDimension = y * Field.SIZE;
                    Image.GetData<Color>(0, new Rectangle(xDimension, yDimension, Field.SIZE, Field.SIZE), data, 0, imgDimension);
                    if (!IsTransparent(data))
                    {
                        Fields.Add(new Field(this, new Rectangle(xDimension, yDimension, Field.SIZE, Field.SIZE), TerrainEnum.GROUND
                            , openTop: !TopClosed(data), openBottom: !BottomClosed(data), openLeft: !LeftClosed(data), openRight: !RightClosed(data))
                        );
                    }
                }
            }
        }

        public Field GetFieldAt(int x, int y)
        {
            foreach (var field in Fields)
            {
                if (field.OriginRect.X == x * Field.SIZE && field.OriginRect.Y == y * Field.SIZE)
                {
                    return field;
                }
            }

            return null;
        }

        public SortedList<int, Field> GetBottomConnectionFields()
        {
            SortedList<int, Field> fields = new SortedList<int, Field>();

            var i = 0;
            for (var x = 0; x < Width / Field.SIZE; x++)
            {
                var field = GetFieldAt(x, Height / Field.SIZE - 1);
                if (field != null && field.OpenBottom)
                {
                    fields.Add(i, field);
                    i++;
                }
            }

            return fields;
        }

        public SortedList<int, Field> GetTopConnectionFields()
        {
            SortedList<int, Field> fields = new SortedList<int, Field>();

            var i = 0;
            for (var x = 0; x < Width / Field.SIZE; x++)
            {
                var field = GetFieldAt(x, 0);
                if (field != null && field.OpenTop)
                {
                    fields.Add(i, field);
                    i++;
                }
            }

            return fields;
        }

        public SortedList<int, Field> GetLeftConnectionFields()
        {
            SortedList<int, Field> fields = new SortedList<int, Field>();

            var i = 0;
            for (var y = 0; y < Height / Field.SIZE; y++)
            {
                var field = GetFieldAt(0, y);
                if (field != null && field.OpenLeft)
                {
                    fields.Add(i, field);
                    i++;
                }
            }

            return fields;
        }

        public SortedList<int, Field> GetRightConnectionFields()
        {
            SortedList<int, Field> fields = new SortedList<int, Field>();

            var i = 0;
            for (var y = 0; y < Height / Field.SIZE; y++)
            {
                var field = GetFieldAt(Width / Field.SIZE - 1, y);
                if (field != null && field.OpenRight)
                {
                    fields.Add(i, field);
                    i++;
                }
            }

            return fields;
        }

        private void connectFields()
        {
            for (var x = 0; x < Width / Field.SIZE; x++)
            {
                for (var y = 0; y < Height / Field.SIZE; y++)
                {
                    if (GetFieldAt(x, y) != null) {
                        GetFieldAt(x, y).BottomField = GetFieldAt(x, y + 1);
                        GetFieldAt(x, y).TopField = GetFieldAt(x, y - 1);
                        GetFieldAt(x, y).LeftField = GetFieldAt(x - 1, y);
                        GetFieldAt(x, y).RightField = GetFieldAt(x + 1, y);

                        GetFieldAt(x, y).TopRightField = GetFieldAt(x + 1, y - 1);
                        GetFieldAt(x, y).TopLeftField = GetFieldAt(x - 1, y - 1);
                        GetFieldAt(x, y).BottomRightField = GetFieldAt(x + 1, y + 1);
                        GetFieldAt(x, y).BottomLeftField = GetFieldAt(x - 1, y + 1);
                    }
                }
            }
        }

        public int GetMaxX()
        {
            return Width / Field.SIZE;
        }

        public int GetMaxY()
        {
            return Height / Field.SIZE;
        }

        public abstract ImageListEnum GetImageName();
    }
}
