using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DescentDirX.Helpers;
using DescentDirX.UI.Components;
using System.Collections.ObjectModel;
using DescentDirX.Characters.Heroes;
using DescentDirX.Items.Tokens;
using DescentDirX.Characters.Monsters;
using DescentDirX.UI;
using DescentDirX.BusEvents.GameProgress;
using DescentDirX.Characters;

namespace DescentDirX.Maps.Tiles.Common
{
    class Field : IHasFocus
    {
        public const int SIZE = 64;

        public Rectangle Rect { get; set; }
        public Rectangle OriginRect { get; set; }
        public TerrainEnum Terrain { get; set; }

        public bool OpenLeft { get; set; }
        public bool OpenRight { get; set; }
        public bool OpenBottom { get; set; }
        public bool OpenTop { get; set; }

        public Color MainColor { get; set; } = Color.Gray;
        private Color HighlightColor { get; set; } = Color.Green;

        private int offsetX;
        private int offsetY;

        public Tile ParentTile { get; private set; }

        public Collection<object> ObjectsOnField { get; set; }
        public Field TopField { get; set;}
        public Field BottomField { get; set; }
        public Field LeftField { get; set; }
        public Field RightField { get; set; }
        public Field TopRightField { get; set; }
        public Field TopLeftField { get; set; }
        public Field BottomRightField { get; set; }
        public Field BottomLeftField { get; set; }

        public int OffsetX {
            get { return offsetX; }
            set {
                offsetX = value;
                Rect = new Rectangle(OriginRect.X + value, OriginRect.Y + OffsetY, Rect.Width, Rect.Height);
            }
        }

        public int OffsetY {
            get { return offsetY; }
            set {
                offsetY = value;
                Rect = new Rectangle(OriginRect.X + OffsetX, OriginRect.Y + value, Rect.Width, Rect.Height);
            }
        }

        private bool Focused {get; set;}
        private bool LastFocusState { get; set; } = false;
        private int LastMouseX { get; set; }
        private int LastMouseY { get; set; }

        public Field(Tile parentTile, Rectangle rect, TerrainEnum terrain, bool openLeft = true, bool openRight = true, bool openTop = true, bool openBottom = true)
        {
            ParentTile = parentTile;
            Rect = rect;
            OriginRect = rect;
            Terrain = terrain;
            OpenBottom = openBottom;
            OpenLeft = openLeft;
            OpenRight = openRight;
            OpenTop = openTop;

            ObjectsOnField = new Collection<object>();
        }

        public void Update(int mouseX, int mouseY)
        {
            // don't do anything if mouse doesn't move
            if (LastMouseX == mouseX && LastMouseY == mouseY) return;

            if (GameCamera.Instance.GetMovedRectangle(Rect).Contains(mouseX, mouseY))
            {
                Focused = true;

                if (LastFocusState == false)
                {
                    MainGame.EVENT_BUS.Post(new FieldFocusedEvent(this, this));
                }
            }
            else
            {
                Focused = false;
            }

            LastFocusState = Focused;
            LastMouseX = mouseX;
            LastMouseY = mouseY;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ImageProvider.GetColorTexture(Color.Gray), GameCamera.Instance.GetMovedRectangle(Rect), new Color(IsFocused() ? HighlightColor : MainColor, 0.1f));

            if (DebugHelper.DebugOn)
            {
                if (!OpenTop)
                {
                    spriteBatch.Draw(ImageProvider.GetColorTexture(Color.White), GameCamera.Instance.GetMovedRectangle(new Rectangle(Rect.X, Rect.Y, Rect.Width, 3)), Color.Red);
                }

                if (!OpenBottom)
                {
                    spriteBatch.Draw(ImageProvider.GetColorTexture(Color.White), GameCamera.Instance.GetMovedRectangle(new Rectangle(Rect.X, Rect.Y + SIZE, Rect.Width, 3)), Color.Red);
                }

                if (!OpenLeft)
                {
                    spriteBatch.Draw(ImageProvider.GetColorTexture(Color.White), GameCamera.Instance.GetMovedRectangle(new Rectangle(Rect.X, Rect.Y, 3, Rect.Height)), Color.Red);
                }

                if (!OpenRight)
                {
                    spriteBatch.Draw(ImageProvider.GetColorTexture(Color.White), GameCamera.Instance.GetMovedRectangle(new Rectangle(Rect.X + SIZE, Rect.Y, 3, Rect.Height)), Color.Red);
                }

                var text = "[" + GetX() + ", " + GetY() + "]";
                var size = FontFactory.Font.MeasureString(text);
                var debugVect = new Vector2(Rect.Location.ToVector2().X + Field.SIZE / 2 - (size.X * 0.4f) / 2 , Rect.Location.ToVector2().Y + Field.SIZE / 2);

                spriteBatch.DrawString(
                    FontFactory.Font
                    , text
                    , GameCamera.Instance.GetMovedVector(debugVect)
                    , Color.White
                    , 0.0f
                    , new Vector2(0, 0)
                    , 0.4f
                    , SpriteEffects.None
                    , 0);

            }

            foreach (var obj in ObjectsOnField)
            {
                var hero = obj as Hero;
                var search = obj as SearchToken;
                var monster = obj as Monster;

                if (hero != null)
                {
                    hero.Draw(spriteBatch);
                }

                if (search != null)
                {
                    search.Draw(spriteBatch);
                }

                if (monster != null)
                {
                    monster.Draw(spriteBatch);
                }
            }
        }

        public void Rotate(TileRotation rotation, int width, int height)
        {
            FieldHelper.RotateField(this, rotation, width, height);
        }

        public bool IsFocused()
        {
            return Focused;
        }

        public Vector2 GetVector()
        {
            return Rect.Location.ToVector2();
        }

        public int GetTileX()
        {
            return OriginRect.X / SIZE;
        }

        public int GetTileY()
        {
            return OriginRect.Y / SIZE;
        }

        public int GetX()
        {
            return (OriginRect.X + OffsetX) / SIZE;
        }

        public int GetY()
        {
            return (OriginRect.Y + OffsetY) / SIZE;
        }

        public Position GetPosition()
        {
            return new Position(GetX(), GetY());
        }

        public Character GetCharacterOnSelf()
        {
            foreach (var gameObject in ObjectsOnField)
            {
                if (gameObject is Character)
                {
                    return gameObject as Character;
                }
            }

            return null;
        }

        public int DistanceFrom(Field field)
        {
            return (int)Vector2.Distance(GetVector(), field.GetVector()) / 64;
        }

        public override string ToString()
        {
            return "absolute X: " + GetX() + ", absolute Y: " + GetY() + ", " + ParentTile + ", relative X: " + GetTileX() + ", relative Y: " + GetTileY() + ", " + ", Terrain: " + Terrain.ToString();
        }
    }
}
