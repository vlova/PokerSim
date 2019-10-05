using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    class SetCombination : CardCombination<SetCombination>
    {
        public override CardCombinationKind Kind => CardCombinationKind.Set;

        public CardValue SetValue { get; }

        public SetCombination(IEnumerable<Card> cards) : base(cards)
        {
            this.SetValue = cards.Max(x => x.Value);
        }

        protected override int CompareCombination(SetCombination other) =>
            this.SetValue.CompareTo(other.SetValue);
    }
}
