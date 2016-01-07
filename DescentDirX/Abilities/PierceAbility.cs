using DescentDirX.Gameplay;

namespace DescentDirX.Abilities
{
    class PierceAbility : SurgeAbility
    {
        public int PierceAmount { get; private set; }

        public PierceAbility(int cost, int pierce) : base(cost, "for " + cost + " surges add " + pierce + " pierce")
        {
            PierceAmount = pierce;
        }

        public override void UseIt(AttackResult attack)
        {
            attack.PierceBonus += PierceAmount;
        }
    }
}
