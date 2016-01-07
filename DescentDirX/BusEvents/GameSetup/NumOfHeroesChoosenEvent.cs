
namespace DescentDirX.BusEvents.GameSetup
{
    class NumOfHeroesChoosenEvent
    {
        public int NumOfHeroes { get; private set; }

        public NumOfHeroesChoosenEvent(object sender, int numOfHeroes)
        {
            NumOfHeroes = numOfHeroes;
        }
    }
}
