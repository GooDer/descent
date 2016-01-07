using DescentDirX.Gameplay;

namespace DescentDirX.Abilities
{
    class RangeAbility : SurgeAbility
    {
        private int Range { get; set; }

        public RangeAbility(int cost, int range) : base(cost, "for " + cost + " surges add " + range + " range")
        {
            Range = range;
        }

        public override void UseIt(AttackResult attack)
        {
            attack.BonusRange += Range;
        }
    }
}
