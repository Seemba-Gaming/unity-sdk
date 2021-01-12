using System.Collections.Generic;
using System.Linq;

namespace SeembaSDK.queue
{
    internal static class Extensions
    {
        internal static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int maxItems)
        {
            return items
                .Select((item, inx) => new { item, inx })
                .GroupBy(x => x.inx / maxItems)
                .Select(g => g.Select(x => x.item));
        }
        public static T GetValueOrDefault<T, TK>(this IDictionary<TK, T> self, TK key)
        {
            T value;
            return self.TryGetValue(key, out value) == false ? default(T) : value;
        }
    }
}