
namespace DescentDirX.Skills
{
    abstract class Skill
    {
        public int RequiredFatugue { get; private set; }

        public Skill(int requiredFatigue)
        {
            RequiredFatugue = requiredFatigue;
        }
    }
}
