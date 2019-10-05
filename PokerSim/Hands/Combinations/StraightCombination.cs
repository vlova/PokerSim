using PokerSim.Cards;
using System.Collections.Generic;

namespace PokerSim
{
    class StraightCombination : StraightBaseCombination<StraightCombination>
    {
        public override CardCombinationKind Kind => CardCombinationKind.Straight;

        /// <param name="cards">Sorted sequence</param>
        public StraightCombination(IEnumerable<Card> cards) : base(cards) { }
    }
}
