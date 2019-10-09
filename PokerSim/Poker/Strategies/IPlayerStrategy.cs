namespace PokerSim
{
    public interface IPlayerStrategy
    {
        PlayerAction GetPlayerAction(PlayerPokerState state);
    }
}
