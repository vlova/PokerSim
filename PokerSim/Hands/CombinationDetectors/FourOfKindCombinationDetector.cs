using PokerSim.Cards;
using PokerSim.Common;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Hands.CombinationDetectors
{
    class FourOfKindCombinationDetector : AbstractCombinationDetector
    {
        public override IEnumerable<CardCombination> DetectCombinations(ISet<Card> cards)
        {
            return cards.GroupBy(c => c.Value)
                .Where(g => g.Count() >= 4)
                .SelectMany(g => g.GetAllSampleWithSize(4))
                .Select(sample => new FourOfKindCombination(sample));
        }
    }
}
