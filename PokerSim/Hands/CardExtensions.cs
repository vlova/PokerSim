using PokerSim.Cards;
using System.Collections.Generic;
using System.Linq;

namespace PokerSim
{
    static class CardExtensions
    {
        public static int CompareCardRanksTo(this IEnumerable<int> myRanks, IEnumerable<int> otherRanks)
        {
            var maxLength = new[] { myRanks.Count(), otherRanks.Count(), 1 }.Max();

            myRanks = myRanks.OrderByDescending(r => r).ExpandToSize(desiredSize: maxLength, defaultValue: 0);
            otherRanks = otherRanks.OrderByDescending(r => r).ExpandToSize(desiredSize: maxLength, defaultValue: 0);

            return myRanks
                .Zip(otherRanks, (myRank, otherRank) => myRank.CompareTo(otherRank))
                .Where(p => p != 0)
                .FirstOrDefault(); // return first comparison or 0 if all ranks are equal
        }

        private static IEnumerable<T> ExpandToSize<T>(this IEnumerable<T> enumerable, int desiredSize, T defaultValue)
        {
            var enumerableSize = enumerable.Count();
            return enumerableSize >= desiredSize
                ? enumerable
                : enumerable.Concat(Enumerable.Range(0, count: desiredSize - enumerableSize).Select(_ => defaultValue));
        }

        public static int CompareCardsTo(this IEnumerable<Card> myCards, IEnumerable<Card> otherCards)
        {
            var myRanks = myCards.Select(m => (int)m.Value);
            var otherRanks = otherCards.Select(m => (int)m.Value);
            return myRanks.CompareCardRanksTo(otherRanks);
        }
    }
}
