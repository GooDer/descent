using DescentDirX.Scenarios;

namespace DescentDirX.Characters.Actions
{
    class AttackAction : IAction
    {
        public bool CanPerformAction(Character character, Scenario scenario)
        {
            return !character.Dead && character.RemainingActions > 0;
        }

        public string GetLabel()
        {
            return "Attack";
        }

        public bool PerformActionOn(Character character, Scenario scenario)
        {
            return character.UseAttackAction();
        }
    }
}
