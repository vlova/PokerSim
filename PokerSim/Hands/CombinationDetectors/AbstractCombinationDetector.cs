using PokerSim.Cards;
using System.Collections.Generic;

namespace PokerSim.Hands.CombinationDetectors
{
    abstract class AbstractCombinationDetector
    {
        public abstract IEnumerable<CardCombination> DetectCombinations(ISet<Card> cards);
    }
}
