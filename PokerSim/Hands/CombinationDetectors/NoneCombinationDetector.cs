using PokerSim.Cards;
using System.Collections.Generic;

namespace PokerSim.Hands.CombinationDetectors
{
    class NoneCombinationDetector : AbstractCombinationDetector
    {
        public override IEnumerable<CardCombination> DetectCombinations(ISet<Card> cards)
        {
            yield return new NoneCombination();
        }
    }
}
