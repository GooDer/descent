using DescentDirX.Maps.Tiles.Common;
using DescentDirX.Characters.Monsters;
using DescentDirX.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using DescentDirX.Gameplay;
using DescentDirX.Maps;

namespace DescentDirX.Scenarios
{
    abstract class Scenario
    {
        public string Name { get; private set;}

        private int Width { get; set; }
        private int Height { get; set; }

        private Dictionary<string, Tile> tiles;

        private Dictionary<Position, Field> fieldGrid;
        private int fieldOffsetX;
        private int fieldOffsetY;

        private Tile tileForPick;
        protected Tile TileForPick {
            get { return tileForPick; }
            set {
                tileForPick = value;
                MarkFields();
            }
        }

        public ScenarioStageEnum Stage { get; private set; } = ScenarioStageEnum.HERO_SETUP;

        public List<Monster> ScenarioMonsters { get; protected set; }

        public Scenario(string name)
        {
            tiles = new Dictionary<string, Tile>();
            fieldGrid = new Dictionary<Position, Field>();
            Name = name;
            ScenarioMonsters = new List<Monster>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles)
            {
                tiles[tile.Key].Draw(spriteBatch);
            }
        }

        public void Update(int mouseX, int mouseY)
        {
            foreach (var tile in tiles)
            {
                tiles[tile.Key].Update(mouseX, mouseY);
            }
        }

        public void AddTile(string key, Tile tile)
        {
            Width = tile.Image.Width;
            Height = tile.Image.Height;
            tiles.Add(key, tile);
        }

        protected Tile CreateTile(string key, Tile tile)
        {
            AddTile(key, tile);
            return tile;
        }

        public Dictionary<string, Tile> GetTiles()
        {
            return tiles;
        }

        public Tile GetTile(string key)
        {
            Tile result = null;
            tiles.TryGetValue(key, out result);
            return result;
        }

        public Field GetFocusedField()
        {
            foreach (var tile in tiles)
            {
                foreach (var field in tiles[tile.Key].Fields)
                {
                    if (field.IsFocused())
                    {
                        return field;
                    }
                }
            }

            return null;
        }

        public Field GetFocusedFieldForPick()
        {
            foreach (var tile in tiles)
            {
                foreach (var field in tiles[tile.Key].Fields)
                {
                    if (field.IsFocused() && TileForPick != null && TileForPick.Fields.Contains(field))
                    {
                        return field;
                    }
                }
            }

            return null;
        }

        public void NextStage()
        {
            switch (Stage)
            {
                case ScenarioStageEnum.HERO_SETUP:
                    Stage = ScenarioStageEnum.OVERLORD_SETUP;
                    PrepareSearchTokens();
                    OverlordSetup();
                    MarkFields();
                    break;
                case ScenarioStageEnum.OVERLORD_SETUP:
                    GameplayProgress.Instance.Monsters = ScenarioMonsters;
                    Stage = ScenarioStageEnum.HERO_TURN;
                    break;
                case ScenarioStageEnum.HERO_TURN:
                    Stage = ScenarioStageEnum.OVERLORD_TURN;
                    break;
                case ScenarioStageEnum.OVERLORD_TURN:
                    Stage = ScenarioStageEnum.HERO_TURN;
                    break;
            }
        }

        public int GetNumOfHeros()
        {
            return GameplayProgress.Instance.GetHeroes().Count;
        }

        public Monster GetUnplacedMonster()
        {
            if (!HasUnplacedMonster()) return null;

            foreach (var monster in ScenarioMonsters)
            {
                if (monster.Position == null)
                {
                    return monster;
                }
            }

            return null;
        }

        public bool HasUnplacedMonster()
        {
            foreach (var monster in ScenarioMonsters)
            {
                if (monster.Position == null)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual void MarkFields()
        {
            foreach (var tile in GetTiles())
            {
                foreach (var field in tiles[tile.Key].Fields)
                {
                    if (TileForPick != null && TileForPick.Fields.Contains(field))
                    {
                        field.MainColor = Color.Gray;
                    }
                    else if (TileForPick != null)
                    {
                        field.MainColor = Color.DarkRed;
                    } else
                    {
                        field.MainColor = Color.Transparent;
                    }
                }
            }
        }

        public void ClearMarkedFields()
        {
            TileForPick = null;
        }

        protected void FitMapToScreen()
        {
            var moveY = 0;
            var moveX = 0;

            var minX = 0;
            var minY = 0;
            var maxX = 0;
            var maxY = 0;

            foreach (var tile in tiles)
            {
                var offsetY = (tiles[tile.Key].GetCurrentPos().Y - tiles[tile.Key].Height / 2);
                var offsetX = (tiles[tile.Key].GetCurrentPos().X - tiles[tile.Key].Width / 2);

                if (tiles[tile.Key].GetCurrentPos().Y <= minY)
                {
                    minY = (int)tiles[tile.Key].GetCurrentPos().Y;
                }
                else if (tiles[tile.Key].GetCurrentPos().Y >= maxY)
                {
                    maxY = (int)tiles[tile.Key].GetCurrentPos().Y;
                }

                if (tiles[tile.Key].GetCurrentPos().X <= minX)
                {
                    minX = (int)tiles[tile.Key].GetCurrentPos().X;
                }
                else if (tiles[tile.Key].GetCurrentPos().X >= maxX)
                {
                    maxX = (int)tiles[tile.Key].GetCurrentPos().X;
                }

                if (offsetY < 0 && offsetY < moveY)
                {
                    moveY = (int)offsetY;
                }

                if (offsetX < 0 && offsetX < moveX)
                {
                    moveX = (int)offsetX;
                }
            }

            if (moveY < 0)
            {
                foreach (var tile in tiles)
                {
                    tiles[tile.Key].MoveOffsetY(-moveY, null, false);
                }
            }

            if (moveX < 0)
            {
                foreach (var tile in tiles)
                {
                    tiles[tile.Key].MoveOffsetX(-moveX, null, false);
                }
            }

            GameCamera.Instance.MapBounds = new Rectangle(minX, minY, maxX + Field.SIZE, maxY + Field.SIZE);
            ConnectTilesFields();
        }

        public void ConnectTilesFields()
        {
            foreach (var tile in tiles.Values)
            {
                if (tile.TileOnBottom != null)
                {
                    var bottomFields = tile.GetBottomConnectionFields();
                    var topFields = tile.TileOnBottom.GetTopConnectionFields();

                    if (bottomFields.Count == topFields.Count)
                    {
                        foreach (var index in bottomFields.Keys)
                        {
                            bottomFields[index].BottomField = topFields[index];
                            topFields[index].TopField = bottomFields[index];

                            bottomFields[index].BottomLeftField = topFields[index].LeftField;
                            bottomFields[index].BottomRightField = topFields[index].RightField;

                            topFields[index].TopLeftField = bottomFields[index].LeftField;
                            topFields[index].TopRightField = bottomFields[index].RightField;
                        }
                    }
                }

                if (tile.TileOnTop != null)
                {
                    var topFields = tile.GetTopConnectionFields();
                    var bottomFields = tile.TileOnTop.GetBottomConnectionFields();

                    if (bottomFields.Count == topFields.Count)
                    {
                        foreach (var index in bottomFields.Keys)
                        {
                            bottomFields[index].BottomField = topFields[index];
                            topFields[index].TopField = bottomFields[index];

                            bottomFields[index].BottomLeftField = topFields[index].LeftField;
                            bottomFields[index].BottomRightField = topFields[index].RightField;

                            topFields[index].TopLeftField = bottomFields[index].LeftField;
                            topFields[index].TopRightField = bottomFields[index].RightField;
                        }
                    }
                }

                if (tile.TileOnLeft != null)
                {
                    var leftFields = tile.GetLeftConnectionFields();
                    var rightFields = tile.TileOnLeft.GetRightConnectionFields();

                    if (rightFields.Count == leftFields.Count)
                    {
                        foreach (var index in rightFields.Keys)
                        {
                            rightFields[index].RightField = leftFields[index];
                            leftFields[index].LeftField = rightFields[index];

                            rightFields[index].TopRightField = leftFields[index].TopField;
                            rightFields[index].BottomRightField = leftFields[index].BottomField;

                            leftFields[index].TopLeftField = rightFields[index].TopField;
                            leftFields[index].BottomLeftField = rightFields[index].BottomField;
                        }
                    }
                }

                if (tile.TileOnRight != null)
                {
                    var rightFields = tile.GetRightConnectionFields();
                    var leftFields = tile.TileOnRight.GetLeftConnectionFields();

                    if (rightFields.Count == leftFields.Count)
                    {
                        foreach (var index in rightFields.Keys)
                        {
                            rightFields[index].RightField = leftFields[index];
                            leftFields[index].LeftField = rightFields[index];

                            rightFields[index].TopRightField = leftFields[index].TopField;
                            rightFields[index].BottomRightField = leftFields[index].BottomField;

                            leftFields[index].TopLeftField = rightFields[index].TopField;
                            leftFields[index].BottomLeftField = rightFields[index].BottomField;
                        }
                    }
                }

                foreach (var field in tile.Fields)
                {
                    fieldGrid.Add(new Position(field.GetX(), field.GetY()), field);
                }

                foreach (var field in tile.Fields)
                {
                    var x = field.GetX();
                    var y = field.GetY();

                    Field tmp;
                    fieldGrid.TryGetValue(new Position(x - 1, y  - 1), out tmp);
                    field.TopLeftField = tmp;

                    fieldGrid.TryGetValue(new Position(x + 1, y - 1), out tmp);
                    field.TopRightField = tmp;

                    fieldGrid.TryGetValue(new Position(x + 1, y + 1), out tmp);
                    field.BottomRightField = tmp;

                    fieldGrid.TryGetValue(new Position(x - 1, y + 1), out tmp);
                    field.BottomLeftField = tmp;

                    /*fieldGrid.TryGetValue(new Position(x, y - 1), out tmp);
                    field.TopField = tmp;

                    fieldGrid.TryGetValue(new Position(x, y + 1), out tmp);
                    field.BottomField = tmp;

                    fieldGrid.TryGetValue(new Position(x - 1, y), out tmp);
                    field.LeftField = tmp;

                    fieldGrid.TryGetValue(new Position(x + 1, y), out tmp);
                    field.RightField = tmp;*/
                }
            }
        }

        /// <summary>
        /// Get field by absolute coordinates on map
        /// </summary>
        /// <param name="x">X axis</param>
        /// <param name="y">Y axis</param>
        /// <returns>found field or null</returns>
        public Field GetFieldAt(int x, int y)
        {
            foreach (var tile in GetTiles().Values)
            {
                foreach (var field in tile.Fields)
                {
                    if (field.GetX() == x && field.GetY() == y)
                    {
                        return field;
                    }
                }
            }

            return null;
        }

        public virtual GameStateEnum GetTurnStartSide()
        {
            return GameStateEnum.HERO_TURN;
        }

        protected abstract void PrepareSearchTokens();
        public abstract void PrepareNextMonsterGroup();
        protected abstract void OverlordSetup();
    }
}
