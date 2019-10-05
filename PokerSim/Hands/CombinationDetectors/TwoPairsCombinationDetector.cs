using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Hands.CombinationDetectors
{
    class TwoPairsCombinationDetector : AbstractCombinationDetector
    {
        public override IEnumerable<CardCombination> DetectCombinations(ISet<Card> cards)
        {
            var onePairDetector = new OnePairCombinationDetector();
            var onePairCombinations = onePairDetector.DetectCombinations(cards).OfType<OnePairCombination>().ToList();
            foreach (var pair1 in onePairCombinations)
            {
                foreach (var pair2 in onePairCombinations)
                {
                    if (ReferenceEquals(pair1, pair2))
                    {
                        continue;
                    }

                    bool allCardsAreUnique = AllCardsAreUnique(pair1, pair2);

                    if (allCardsAreUnique)
                    {
                        yield return new TwoPairsCombination(pair1.Cards.Concat(pair2.Cards));
                    }
                }
            }
        }

        private static bool AllCardsAreUnique(OnePairCombination pair1, OnePairCombination pair2)
        {
            var cards = pair1.Cards.Concat(pair2.Cards).ToList();
            return cards.Distinct().Count() == cards.Count;
        }
    }
}
