using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace PokerSim.Common
{
    public static class Extensions
    {

        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        private static int GetRandomNumber(int size)
        {
            byte[] data = new byte[4];

            rng.GetBytes(data);

            var number = Math.Abs(BitConverter.ToInt32(data, 0));
            return number % size;
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
    }
}
