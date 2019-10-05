using PokerSim.Cards;
using PokerSim.Common;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Hands.CombinationDetectors
{
    class OnePairCombinationDetector : AbstractCombinationDetector
    {
        public override IEnumerable<CardCombination> DetectCombinations(ISet<Card> cards)
        {
            return cards.GroupBy(c => c.Value)
                .Where(g => g.Count() >= 2)
                .SelectMany(g => g.GetAllSampleWithSize(2))
                .Select(sample => new OnePairCombination(sample));
        }
    }
}
