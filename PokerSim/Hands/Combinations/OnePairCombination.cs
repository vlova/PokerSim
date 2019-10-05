using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    class OnePairCombination : CardCombination<OnePairCombination>
    {
        public OnePairCombination(IEnumerable<Card> cards) : base(cards)
        {
            this.PairValue = cards.Max(x => x.Value);
        }

        public override CardCombinationKind Kind => CardCombinationKind.OnePair;

        public CardValue PairValue { get; }

        protected override int CompareCombination(OnePairCombination other) =>
            this.PairValue.CompareTo(other.PairValue);
    }
}
