using DescentDirX.Scenarios;

namespace DescentDirX.Characters.Actions
{
    interface IAction
    {
        bool PerformActionOn(Character character, Scenario scenario);
        bool CanPerformAction(Character character, Scenario scenario);
        string GetLabel();
    }
}
