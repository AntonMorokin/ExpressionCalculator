using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    /// <summary>
    /// Implementation of bubble sorting.
    /// </summary>
    /// <remarks>
    /// We need to use stable sort to not change relative position of equal members.
    /// Bubble sort - the simplest of stable algorithms.
    /// </remarks>
    public static class BubbleSort
    {
        /// <summary>
        /// Sorts <paramref name="source"/> sequence using Bubble sorting algorithm.
        /// </summary>
        /// <typeparam name="T">Type of sequence elements.</typeparam>
        /// <param name="source">Source sequence for sort.</param>
        /// <param name="comparer">Compare function to compare two elements.</param>
        /// <returns>Sorted sequence.</returns>
        public static IEnumerable<T> Sort<T>(IEnumerable<T> source, Comparison<T> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            var sourceList = source.ToList();

            int n = sourceList.Count;
            int newN = 0;

            do
            {
                newN = 0;

                for (int i = 1; i < n; i++)
                {
                    if (comparer(sourceList[i - 1], sourceList[i]) > 0)
                    {
                        Swap(sourceList, i - 1, i);
                        newN = i;
                    }
                }

                n = newN;
            }
            while (n > 1);

            return sourceList;
        }

        private static void Swap<T>(IList<T> sourceList, int indexOfFirst, int indexOfSecond)
        {
            var buf = sourceList[indexOfFirst];

            sourceList[indexOfFirst] = sourceList[indexOfSecond];
            sourceList[indexOfSecond] = buf;
        }
    }
}
