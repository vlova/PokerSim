using PokerSim.Cards;
using PokerSim.Common;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Hands.CombinationDetectors
{
    class SetCombinationDetector : AbstractCombinationDetector
    {
        public override IEnumerable<CardCombination> DetectCombinations(ISet<Card> cards)
        {
            return cards.GroupBy(c => c.Value)
                .Where(g => g.Count() >= 3)
                .SelectMany(g => g.GetAllSampleWithSize(3))
                .Select(sample => sample.ToList())
                .Select(sample => new SetCombination(sample));
        }
    }
}
