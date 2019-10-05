using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    class FourOfKindCombination : CardCombination<FourOfKindCombination>
    {
        private CardValue CardValue;

        public FourOfKindCombination(IEnumerable<Card> cards) : base(cards)
        {
            this.CardValue = cards.Select(c => c.Value).First();
        }

        public override CardCombinationKind Kind => CardCombinationKind.FourOfKind;

        protected override int CompareCombination(FourOfKindCombination other) =>
            this.CardValue.CompareTo(other.CardValue);
    }
}
