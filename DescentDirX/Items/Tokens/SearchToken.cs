using DescentDirX.Helpers;
using DescentDirX.Maps.Tiles.Common;
using DescentDirX.UI;
using Microsoft.Xna.Framework.Graphics;

namespace DescentDirX.Items.Tokens
{
    class SearchToken
    {
        public Field Position { get; set; }

        public SearchToken(Field position)
        {
            Position = position;
        }

        public ImageListEnum GetImage()
        {
            return ImageListEnum.TOKENS_SEARCH;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Position != null)
            {
                spriteBatch.Draw(ImageProvider.Instance.GetImage(GetImage()), GameCamera.Instance.GetMovedVector(Position.GetVector()));
            }
        }
    }
}
