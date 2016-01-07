using Microsoft.Xna.Framework;

namespace DescentDirX.BusEvents.General
{
    class ClickMessage
    {
        public Vector2 MousePosition { get; private set; }

        public ClickMessage(object sender, Vector2 mousePosition)
        {
            MousePosition = mousePosition;
        }
    }
}
