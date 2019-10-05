using PokerSim.Cards;
using System.Collections.Generic;

namespace PokerSim
{
    class FlushCombination : CardCombination<FlushCombination>
    {
        public override CardCombinationKind Kind => CardCombinationKind.Flush;

        public FlushCombination(IEnumerable<Card> cards) : base(cards) { }

        protected override int CompareCombination(FlushCombination other) =>
            this.Cards.CompareCardsTo(other.Cards);
    }
}
