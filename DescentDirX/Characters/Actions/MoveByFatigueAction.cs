using DescentDirX.Scenarios;
using DescentDirX.Characters.Heroes;

namespace DescentDirX.Characters.Actions
{
    class MoveByFatigueAction : IAction
    {
        public bool CanPerformAction(Character character, Scenario scenario)
        {
            Hero hero = character as Hero;
            return (hero != null && hero.CanSpendFatigue(1));
        }

        public string GetLabel()
        {
            return "Use 1 fatigue to move";
        }

        public bool PerformActionOn(Character character, Scenario scenario)
        {
            Hero hero = character as Hero;
            if (hero != null && hero.CanSpendFatigue(1) && !hero.Dead)
            {
                scenario.ClearMarkedFields();
                hero.UseFatigueToMove(1);
            }

            return false;
        }
    }
}
