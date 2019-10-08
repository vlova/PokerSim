using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace PokerSim.Common
{
    public static class EnumerableExtensions
    {

        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        private static int GetRandomNumber(int size)
        {
            byte[] data = new byte[4];

            rng.GetBytes(data);

            // well, ok, using of LossyAbs will make bias on random, but it's small
            var number = LossyAbs(BitConverter.ToInt32(data, 0));
            return number % size;
        }

        private static int LossyAbs(int value)
        {
            if (value >= 0) return value;
            if (value == int.MinValue) return int.MaxValue;
            return -value;
        }


        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            var list = new List<T>(collection);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = GetRandomNumber(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static IEnumerable<IEnumerable<T>> GetAllSampleWithSize<T>(this IEnumerable<T> collection, int size)
        {
            IEnumerable<IEnumerable<T>> Samples(IList<T> accumulator, IList<T> leftItems)
            {
                if (accumulator.Count() + leftItems.Count() < size)
                {
                    return Enumerable.Empty<IEnumerable<T>>();
                }

                if (accumulator.Count() == size)
                {
                    return new[] { accumulator };
                }

                return leftItems
                    .SelectMany(
                        (item, index) => Samples(
                            accumulator.Concat(new[] { item }).ToList(),
                            leftItems.Skip(index + 1).ToList()
                        ))
                    .ToList();
            }

            var result = Samples(new List<T> { }, collection.ToList())
                .ToList();

            return result;
        }

        public static List<List<T>> Transpose<T>(this List<List<T>> source)
        {
            var transposed = new List<List<T>>();
            var innerItemsSize = source.First().Count;
            var outerItemsSize = source.Count;
            for (var j = 0; j < innerItemsSize; j++)
            {
                transposed.Add(new List<T>());
                for (var i = 0; i < outerItemsSize; i++)
                {
                    transposed[j].Add(source[i][j]);
                }
            }

            return transposed;
        }
    }
}
