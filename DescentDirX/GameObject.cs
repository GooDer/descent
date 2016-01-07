using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DescentDirX
{
    abstract class GameObject
    {
        public Vector2 Position { get; set; }
        private bool Visible { get; set; } = false;
        private List<GameObject> children;
        public GameObject Parent { get; protected set; }

        public GameObject(Vector2 position)
        {
            Position = position;
            children = new List<GameObject>();
        }

        public virtual void Show()
        {
            Visible = true;

            foreach (var child in children)
            {
                child.Show();
            }
        }

        public virtual void Hide()
        {
            Visible = false;

            foreach (var child in children)
            {
                child.Hide();
            }
        }

        public bool IsVisible()
        {
            return Visible;
        }

        public virtual void PositionRelatedTo(GameObject gameObject)
        {
            Position = new Vector2(Position.X + gameObject.Position.X, Position.Y + gameObject.Position.Y);
        }

        public void AlterPositionX(int amount)
        {
            Position = new Vector2(Position.X + amount, Position.Y);
        }

        public void AlterPositionY(int amount)
        {
            Position = new Vector2(Position.X, Position.Y + amount);
        }

        public void AddChild(GameObject child, bool setRelativePosition = false)
        {
            child.Parent = this;
            AddChild(child);

            if (setRelativePosition)
            {
                child.PositionRelatedTo(this);
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(int mouseX, int mouseY);
    }
}
