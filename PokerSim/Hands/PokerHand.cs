using PokerSim.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    public class PokerHand : IComparable<PokerHand>
    {
        public CardCombination CardCombination { get; }
        public IEnumerable<Card> Kickers { get; }

        public PokerHand(CardCombination cardCombination, IEnumerable<Card> kickers)
        {
            CardCombination = cardCombination;
            Kickers = kickers.ToList();
        }

        public int CompareTo(PokerHand other)
        {
            var compareCombination = this.CardCombination.CompareTo(other.CardCombination);
            if (compareCombination != 0)
            {
                return compareCombination;
            }

            return this.Kickers.CompareCardsTo(other.Kickers);
        }
    }
}
