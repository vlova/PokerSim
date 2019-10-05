using PokerSim.Cards;
using PokerSim.Common;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim.Hands.CombinationDetectors
{
    class StraightCombinationDetector : AbstractCombinationDetector
    {
        public override IEnumerable<CardCombination> DetectCombinations(ISet<Card> cards)
        {
            return cards
                .GetAllSampleWithSize(5)
                .SelectMany(GetStraights);
        }

        private IEnumerable<StraightCombination> GetStraights(IEnumerable<Card> cards)
        {
            var aces = cards.Where(c => c.Value == CardValue.Ace);
            var noAces = cards.Except(aces).OrderBy(c => c.Value).ToList();
            var hasAces = aces.Any();

            if (hasAces)
            {
                var firstAttempt = aces.Concat(noAces).ToList();
                var secondAttempt = noAces.Concat(aces).ToList();

                if (IsOrdered(firstAttempt))
                {
                    yield return new StraightCombination(firstAttempt);
                }

                if (IsOrdered(secondAttempt))
                {
                    yield return new StraightCombination(secondAttempt);
                }
            }
            else
            {
                if (IsOrdered(noAces))
                {
                    yield return new StraightCombination(noAces);
                }
            }
        }
        private static bool IsOrdered(IList<Card> seq)
        {
            var startPos = 0;
            var straightSequence = new List<Card>() { seq[startPos] };
            for (var currentPos = startPos + 1; currentPos < seq.Count; currentPos++)
            {
                var lastCard = straightSequence.Last();
                var currentCard = seq[currentPos];
                if (CanFollow(lastCard, currentCard))
                {
                    straightSequence.Add(currentCard);
                    continue;
                }
                else if (lastCard.Value == currentCard.Value)
                {
                    break;
                }
                else
                {
                    break;
                }
            }

            return straightSequence.Count == 5;
        }

        private static bool CanFollow(Card prev, Card next)
        {
            if (prev.Value + 1 == next.Value)
            {
                return true;
            }

            if (prev.Value == CardValue.Ace && next.Value == CardValue.Two)
            {
                return true;
            }

            return false;
        }
    }
}
