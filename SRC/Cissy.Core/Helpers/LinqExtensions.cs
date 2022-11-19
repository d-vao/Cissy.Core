using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, Tkey>(this IEnumerable<TSource> source, Func<TSource, Tkey> keySelector)
        {
            HashSet<Tkey> hashSet = new HashSet<Tkey>();
            foreach (TSource item in source)
            {
                if (hashSet.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }
    }
}
