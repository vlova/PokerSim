namespace PokerSim
{
    public class FoldAlwaysStategy : IPlayerStrategy
    {
        public PlayerAction GetPlayerAction(PlayerPokerState state)
        {
            return new FoldAction();
        }
    }
}
