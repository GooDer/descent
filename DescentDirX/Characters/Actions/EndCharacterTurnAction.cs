using DescentDirX.Scenarios;
using DescentDirX.Gameplay;

namespace DescentDirX.Characters.Actions
{
    class EndCharacterTurnAction : IAction
    {
        public bool CanPerformAction(Character character, Scenario scenario)
        {
            return true;
        }

        public string GetLabel()
        {
            return "End character turn";
        }

        public bool PerformActionOn(Character character, Scenario scenario)
        {
            GameplayProgress.Instance.GetChooseHeroDialog().Show();
            return true;
        }
    }
}
