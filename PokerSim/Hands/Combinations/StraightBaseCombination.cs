using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    abstract class StraightBaseCombination<T> : CardCombination<T> where T : StraightBaseCombination<T>
    {
        /// <param name="cards">Sorted sequence</param>
        public StraightBaseCombination(IEnumerable<Card> cards) : base(cards) { }

        protected IEnumerable<int> CardRanks => this.Cards.Select((card, index) =>
        {
            if (card.Value != CardValue.Ace)
            {
                return (int)card.Value;
            }

            return index == 0
                ? 1
                : (int)CardValue.Ace;
        });

        protected override int CompareCombination(T other) =>
            this.CardRanks.CompareCardRanksTo(other.CardRanks);
    }
}
