using PokerSim.Cards;
using PokerSim.Hands.CombinationDetectors;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    public class PokerHandDetector
    {
        private List<AbstractCombinationDetector> combinationDetectors = new List<AbstractCombinationDetector>() {
            new StraigtFlushCombinationDetector(),
            new FourOfKindCombinationDetector(),
            new StraightCombinationDetector(),
            new FullHouseCombinationDetector(),
            new FlushCombinationDetector(),
            new StraightCombinationDetector(),
            new SetCombinationDetector(),
            new TwoPairsCombinationDetector(),
            new OnePairCombinationDetector(),
            new NoneCombinationDetector(),
        };

        public PokerHand DetectTopPokerHand(ISet<Card> cards)
        {
            PokerHand topPokerHand = null;
            foreach (var combinationDetector in combinationDetectors)
            {
                var pokerHands = combinationDetector.DetectCombinations(cards).Select(combination => ToPokerHand(combination, cards));
                foreach (var pokerHand in pokerHands)
                {
                    if (topPokerHand == null)
                    {
                        topPokerHand = pokerHand;
                        continue;
                    }

                    if (topPokerHand.CompareTo(pokerHand) < 0)
                    {
                        topPokerHand = pokerHand;
                    }
                }

                if (topPokerHand != null)
                {
                    // Cuz detectors are sorted from high to low
                    break;
                }
            }

            return topPokerHand;
        }

        private PokerHand ToPokerHand(CardCombination combination, ISet<Card> cards)
        {
            var unusedCardsInCombination = cards.Except(combination.Cards);
            var kickerAmount = 5 - combination.Cards.Count;
            var kickers = unusedCardsInCombination.OrderByDescending(c => c.Value).Take(kickerAmount);
            return new PokerHand(combination, kickers);
        }
    }
}
