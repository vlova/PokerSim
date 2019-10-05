using PokerSim.Cards;
using System.Collections.Generic;

namespace PokerSim
{
    class StraightFlushCombination : StraightBaseCombination<StraightFlushCombination>
    {
        public override CardCombinationKind Kind => CardCombinationKind.StraightFlush;

        /// <param name="cards">Sorted sequence</param>
        public StraightFlushCombination(IEnumerable<Card> cards) : base(cards) { }
    }
}
