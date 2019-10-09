using System.Linq;

namespace PokerSim
{
    public class AllInStrategy : IPlayerStrategy
    {
        public PlayerAction GetPlayerAction(PlayerPokerState state)
        {
            return new AllInAction();
        }
    }
}
