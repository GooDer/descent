using DescentDirX.Maps.Tiles.Common;

namespace DescentDirX.BusEvents.GameProgress
{
    class FieldFocusedEvent
    {
        public Field Field { get; private set; }

        public FieldFocusedEvent(object source, Field field)
        {
            Field = field;
        }
    }
}
