using DescentDirX.Gameplay;

namespace DescentDirX.Abilities
{
    class ForceRerollDefenseAbility : SurgeAbility, IPickDefenseDice
    {
        private int DiceIndex { get; set; }

        public ForceRerollDefenseAbility(int cost, string desc) : base(cost, desc)
        {
        }

        public void SetDefenseDice(int index)
        {
            DiceIndex = index;
        }

        public override void UseIt(DefenseResult defense, int diceIndex)
        {
            SetDefenseDice(diceIndex);
            UseAbility(defense);
        }

        private void UseAbility(DefenseResult defense)
        {
            defense.RerollSpecificDice(DiceIndex);
        }
    }
}
