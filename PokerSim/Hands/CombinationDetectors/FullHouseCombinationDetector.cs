using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Hands.CombinationDetectors
{
    class FullHouseCombinationDetector : AbstractCombinationDetector
    {
        public override IEnumerable<CardCombination> DetectCombinations(ISet<Card> cards)
        {
            var onePairDetector = new OnePairCombinationDetector();
            var setDetector = new SetCombinationDetector();
            var onePairCombinations = onePairDetector.DetectCombinations(cards).OfType<OnePairCombination>();
            var setCombinations = setDetector.DetectCombinations(cards).OfType<SetCombination>();
            foreach (var onePair in onePairCombinations)
            {
                foreach (var set in setCombinations)
                {
                    if (!AllCardsAreUnique(onePair, set))
                    {
                        continue;
                    }

                    yield return new FullHouseCombination(setCards: set.Cards, pairCards: onePair.Cards);
                }
            }
        }

        private static bool AllCardsAreUnique(OnePairCombination onePair, SetCombination set)
        {
            return onePair.Cards.Concat(set.Cards).ToList()
                .Distinct()
                .Count() == 5;
        }
    }
}
