using DescentDirX.Characters.Heroes;
using DescentDirX.Scenarios;

namespace DescentDirX.Characters.Actions
{
    class RestAction : IAction
    {
        public bool CanPerformAction(Character character, Scenario scenario)
        {
            var hero = character as Hero;

            if (hero == null || hero.TakenFatigue == 0 || hero.UsedRest || hero.RemainingActions == 0)
            {
                return false;
            }

            return true;
        }

        public string GetLabel()
        {
            return "Rest";
        }

        public bool PerformActionOn(Character character, Scenario scenario)
        {
            var hero = character as Hero;

            if (hero == null || !CanPerformAction(character, scenario)) return false;

            return hero.Rest();
        }
    }
}
