using System;
using System.Collections.Generic;

namespace CalixtoStore.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> itens, Action<T> itemAction)
        {
            foreach (var item in itens)
            {
                itemAction(item);
            }
        }
    }
}