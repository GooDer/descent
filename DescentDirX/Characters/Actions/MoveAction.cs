using DescentDirX.Scenarios;

namespace DescentDirX.Characters.Actions
{
    class MoveAction : IAction
    {
        public string GetLabel()
        {
            return "Move";
        }

        public bool PerformActionOn(Character character, Scenario scenario)
        {
            if (!CanPerformAction(character, scenario)) return false;

            scenario.ClearMarkedFields();

            character.UseMovementAction();

            return true;
        }

        public bool CanPerformAction(Character character, Scenario scenario)
        {
            return character.RemainingActions > 0 && !character.Dead;
        }
    }
}
