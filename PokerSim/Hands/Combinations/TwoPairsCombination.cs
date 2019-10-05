using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    class TwoPairsCombination : CardCombination<TwoPairsCombination>
    {
        public TwoPairsCombination(IEnumerable<Card> cards)
            : base(cards)
        {
            this.HigherValue = cards.Max(x => x.Value);
            this.LowerValue = cards.Min(x => x.Value);
        }

        public override CardCombinationKind Kind => CardCombinationKind.TwoPairs;

        public CardValue HigherValue { get; }
        public CardValue LowerValue { get; }

        protected override int CompareCombination(TwoPairsCombination other)
        {
            var compareHighest = this.HigherValue.CompareTo(other.HigherValue);
            if (compareHighest != 0)
            {
                return compareHighest;
            }

            var compareLower = this.LowerValue.CompareTo(other.LowerValue);
            return compareLower;
        }
    }
}
