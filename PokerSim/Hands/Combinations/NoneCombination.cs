using PokerSim.Cards;
using System.Linq;

namespace PokerSim
{
    class NoneCombination : CardCombination<NoneCombination>
    {
        public NoneCombination() : base(Enumerable.Empty<Card>()) { }

        public override CardCombinationKind Kind => CardCombinationKind.None;

        protected override int CompareCombination(NoneCombination other) => 0;
    }
}
