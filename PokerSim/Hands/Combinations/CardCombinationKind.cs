namespace PokerSim
{
    public enum CardCombinationKind
    {
        None = 0,
        OnePair,
        TwoPairs,
        Set,
        Straight,
        Flush,
        FullHouse,
        FourOfKind,
        StraightFlush // No RoyalFlush, because StraightFlush is enough for modeling
    }
}
