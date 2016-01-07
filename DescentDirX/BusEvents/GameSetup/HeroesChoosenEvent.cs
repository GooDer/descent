using DescentDirX.Characters.Heroes;
using System.Collections.Generic;

namespace DescentDirX.BusEvents.GameSetup
{
    class HeroesChoosenEvent
    {
        public List<Hero> Heroes { get; private set; }

        public HeroesChoosenEvent(object sender, List<Hero> heroes)
        {
            Heroes = heroes;
        }
    }
}
