using DescentDirX.Scenarios;

namespace DescentDirX.BusEvents.GameSetup
{
    class ScenarioChoosenEvent
    {
        public Scenario SelectedScenario { get; private set; }

        public ScenarioChoosenEvent(object sender, Scenario scenario)
        {
            SelectedScenario = scenario;
        }
    }
}
