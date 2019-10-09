using PokerSim.Cards;
using System.Collections.Generic;

namespace PokerSim
{
    public class PlayerPokerState
    {
        public decimal CommunityPot { get; set; }

        public IReadOnlyList<Card> PlayerCards { get; set; }

        public IReadOnlyList<Card> CommunityCards { get; set; }
    }
}
