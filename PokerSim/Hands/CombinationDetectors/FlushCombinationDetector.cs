using PokerSim.Cards;
using PokerSim.Common;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Hands.CombinationDetectors
{
    class FlushCombinationDetector : AbstractCombinationDetector
    {
        private const int FlushSize = 5;

        public override IEnumerable<CardCombination> DetectCombinations(ISet<Card> cards)
        {
            return cards
                .GroupBy(c => c.Suit)
                .Where(g => g.Count() >= FlushSize)
                .SelectMany(g => g.GetAllSampleWithSize(FlushSize))
                .Select(g => new FlushCombination(g.OrderBy(c => c.Value)));
        }
    }
}
