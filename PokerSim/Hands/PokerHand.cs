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

            if (this.CardCombination.Kind == CardCombinationKind.None && other.CardCombination.Kind == CardCombinationKind.None)
            {
                return this.Kickers.OrderByDescending(r => r.Value).Take(1)
                    .CompareCardsTo(other.Kickers.OrderByDescending(r => r.Value).Take(1));
            }

            return this.Kickers.CompareCardsTo(other.Kickers);
        }
    }
}
