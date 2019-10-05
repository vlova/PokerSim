using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Hands.CombinationDetectors
{
    class StraigtFlushCombinationDetector : AbstractCombinationDetector
    {
        public override IEnumerable<CardCombination> DetectCombinations(ISet<Card> cards)
        {
            var flushCombinationDetecttor = new FlushCombinationDetector();
            var straightCombinationDetector = new StraightCombinationDetector();
            var flushes = flushCombinationDetecttor.DetectCombinations(cards).OfType<FlushCombination>().ToList();
            return flushes
                .SelectMany(flush => straightCombinationDetector.DetectCombinations(flush.Cards.ToHashSet()))
                .OfType<StraightCombination>()
                .Select(straight => new StraightFlushCombination(straight.Cards));
        }
    }
}
