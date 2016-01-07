using DescentDirX.Characters;

namespace DescentDirX.BusEvents.GameProgress
{
    class CharacterSelectedEvent
    {
        public Character Character { get; private set; }

        public CharacterSelectedEvent(object source, Character character)
        {
            Character = character;
        }

    }
}
