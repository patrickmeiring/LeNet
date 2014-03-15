using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    static class Randomisation
    {
        public static IList<T> Randomise<T>(this IList<T> items)
        {
            Random random = new Random();
            return items.Randomise(random);
        }

        public static IList<T> Randomise<T>(this IList<T> items, int seed)
        {
            return items.Randomise(new Random(seed));
        }

        public static IList<T> Randomise<T>(this IList<T> items, Random random)
        {
            List<T> result = new List<T>(items);
            for (int i = 0; i < result.Count; i++)
            {
                T item = result[i];
                int newPosition = random.Next(result.Count);
                result[i] = result[newPosition];
                result[newPosition] = item;
            }
            return result;
        }
    }
}
