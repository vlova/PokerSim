using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Simulations
{
    class AllInVersusFoldBadCardsSimulation : AllInSimulation
    {
        protected override List<Player> MakePlayers(SimulationOptions options, int startingPot)
        {
            return Enumerable
                    .Range(0, options.PlayerCount)
                    .Select(i => new Player()
                    {
                        PlayerPot = startingPot,
                        Strategy = i == 0 
                            ? new AllInStrategy()
                            : (IPlayerStrategy)new FoldBadCardsOrAllInStrategy()
                    }).ToList();
        }
    }
}
