using DescentDirX.Gameplay;

namespace DescentDirX.Abilities
{
    class SurgeAbility : Ability
    {
        public int Cost { get; private set; }

        public string Desc { get; private set; }

        public SurgeAbility(int cost, string desc)
        {
            Cost = cost;
            Desc = desc;
        }

        public bool CanUse(int surges)
        {
            return surges >= Cost;
        }

        public virtual void UseIt(DefenseResult defense, int diceIndex)
        {
        }

        public virtual void UseIt(AttackResult attack)
        {
        }

        public virtual void UseIt(AttackResult attack, int diceIndex)
        {
        }

        public override string ToString()
        {
            return Desc;
        }
    }
}
