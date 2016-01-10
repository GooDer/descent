using DescentDirX.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DescentDirX.UI.Components
{
    class Dialog : GameObject
    {
        private const int TITLE_HEIGHT = 35;

        private Color backgroundColor = ImageProvider.backgroundColor;
        private Color titleBackgroundColor = ImageProvider.foregroundColor;

        private int Width { get; set; }
        private int Height { get; set; }
        private Label Title { get; set; }

        private Rectangle TitlePosition { get; set; }
        private Rectangle DialogPosition { get; set; }
        private Rectangle Bounds { get; set; }

        private SortedList<int, GameObject> rendableList = new SortedList<int, GameObject>();

        public Dialog(int width, int height, string title = null) : base(new Vector2(GameCamera.Instance.ScreenViewport.Width / 2 - width / 2, GameCamera.Instance.ScreenViewport.Height / 2 - height / 2))
        {
            Width = width;
            Height = height;

            TitlePosition = new Rectangle((int)Position.X, (int)Position.Y, Width, TITLE_HEIGHT);
            DialogPosition = new Rectangle((int)Position.X, (int)Position.Y + TITLE_HEIGHT, Width, Height - TITLE_HEIGHT);

            if (title != null) {
                Title = new Label(title, new Vector2(TitlePosition.X + 10, TitlePosition.Y + 10));
                Title.Color = Color.White;
            }

            Bounds = new Rectangle(TitlePosition.X, TitlePosition.Y, TitlePosition.Width, TitlePosition.Height + DialogPosition.Height);
        }

        public void AddRendable(int order, GameObject rendable)
        {
            rendable.PositionRelatedTo(this);
            rendable.AlterPositionY(TITLE_HEIGHT);
            rendableList.Add(order, rendable);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible())
            {
                return;
            }

            spriteBatch.Draw(ImageProvider.GetColorTexture(backgroundColor, Color.White), DialogPosition, backgroundColor);
            spriteBatch.Draw(ImageProvider.GetColorTexture(titleBackgroundColor, Color.White), TitlePosition, titleBackgroundColor);
            spriteBatch.Draw(ImageProvider.GetColorTexture(titleBackgroundColor, Color.White), new Rectangle(TitlePosition.X, TitlePosition.Y, 2, Height), titleBackgroundColor);
            spriteBatch.Draw(ImageProvider.GetColorTexture(titleBackgroundColor, Color.White), new Rectangle(TitlePosition.X + Width, TitlePosition.Y, 2, Height), titleBackgroundColor);
            spriteBatch.Draw(ImageProvider.GetColorTexture(titleBackgroundColor, Color.White), new Rectangle(TitlePosition.X, TitlePosition.Y + Height, Width, 2), titleBackgroundColor);

            if (Title != null)
            {
                Title.Draw(spriteBatch);
            }

            foreach (var rendable in rendableList.Values)
            {
                rendable.Draw(spriteBatch);
            }
        }

        public override void Update(int mouseX, int mouseY)
        {
            foreach (var rendable in rendableList.Values)
            {
                rendable.Update(mouseX, mouseY);
            }
        }

        public override void Show()
        {
            if (IsVisible()) return;

            base.Show();

            if (Title != null)
            {
                Title.Show();
            }

            foreach (var rendable in rendableList.Values)
            {
                rendable.Show();
            }
        }

        public override void Hide()
        {
            base.Hide();

            foreach (var rendable in rendableList.Values)
            {
                rendable.Hide();
            }
        }

        protected IList<GameObject> GetRendables()
        {
            return rendableList.Values;
        }
    }
}
