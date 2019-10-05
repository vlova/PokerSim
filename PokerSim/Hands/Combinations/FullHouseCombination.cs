using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    class FullHouseCombination : CardCombination<FullHouseCombination>
    {
        public FullHouseCombination(IEnumerable<Card> setCards, IEnumerable<Card> pairCards)
            : base(setCards.Concat(pairCards))
        {
            this.SetValue = setCards.Select(s => s.Value).First();
            this.PairValue = pairCards.Select(s => s.Value).First();
        }


        public override CardCombinationKind Kind => CardCombinationKind.FullHouse;

        public CardValue SetValue { get; }
        public CardValue PairValue { get; }


        protected override int CompareCombination(FullHouseCombination other)
        {
            var compareSet = this.SetValue.CompareTo(other.SetValue);
            if (compareSet != 0)
            {
                return compareSet;
            }

            var comparePair = this.PairValue.CompareTo(other.PairValue);
            return comparePair;
        }
    }
}
