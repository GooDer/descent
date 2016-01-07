using DescentDirX.BusEvents.General;
using Microsoft.Xna.Framework;
using NGuava;

namespace DescentDirX.UI.Components
{
    delegate void OnClick(GameButton source, object sourceObject);
    delegate void OnFocus(GameButton source, object sourceObject);
    delegate void OnFocusLost();

    abstract class GameButton : GameObject, IHasFocus
    {
        public Vector2 Size { get; private set; }
        private bool Focused { get; set; } = false;
        private bool LastFocusState { get; set; } = false;

        public bool Disabled { get; set; } = false;

        private OnClick ClickCallback { get; set; }
        private OnFocus FocusCallback { get; set; }
        private OnFocusLost FocusLostCallback { get; set; }

        private object SourceObject { get; set; }

        public GameButton(Vector2 size, Vector2 position) : base(position)
        {
            Size = size;
        }

        public override void Update(int mouseX, int mouseY)
        {
            if (!IsVisible()) return;

            DebugHelper.Clear();
            DebugHelper.AddText("focused: " + IsFocused());
            DebugHelper.AddText("Mouse X: " + mouseX);
            DebugHelper.AddText("Mouse Y: " + mouseY);
            DebugHelper.AddText("Position X: " + Position.X);
            DebugHelper.AddText("Position Y: " + Position.Y);
            DebugHelper.AddText("Size X: " + Size.X);
            DebugHelper.AddText("Size Y: " + Size.Y);

            if (mouseX >= Position.X
                && mouseX <= Position.X + Size.X
                && mouseY >= Position.Y
                && mouseY <= Position.Y + Size.Y)
            {
                Focused = true;
                if (FocusCallback != null && LastFocusState == false)
                {
                    FocusCallback(this, SourceObject);
                }
            }
            else
            {
                if (Focused && FocusLostCallback != null)
                {
                    FocusLostCallback();
                }
                Focused = false;
            }

            LastFocusState = Focused;
        }

        [Subscribe]
        public void TriggerClick(ClickMessage sourceEvent)
        {
            if (CanTriggerClick())
            {
                ClickCallback(this, SourceObject);
            }
        }

        private bool CanTriggerClick()
        {
            return Focused && ClickCallback != null && Disabled != true && IsVisible() == true;
        }

        public void RegisterOnClick(OnClick callback)
        {
            ClickCallback = callback;
        }

        public void RegisterCallbackObject<T>(T sourceObject)
        {
            this.SourceObject = sourceObject;
        }

        public bool IsFocused()
        {
            return Focused;
        }

        public void RegisterOnFocus(OnFocus callback)
        {
            FocusCallback = callback;
        }

        public void RegisterOnFocusLost(OnFocusLost callback)
        {
            FocusLostCallback = callback;
        }

        public override void Hide()
        {
            base.Hide();
            MainGame.EVENT_BUS.UnRegister(this);
        }

        public override void Show()
        {
            if (IsVisible()) return;
            base.Show();
            MainGame.EVENT_BUS.Register(this);
        }
    }
}
