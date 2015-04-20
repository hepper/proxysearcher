using System;
using System.Collections.Generic;
using System.Linq;

namespace ProxySearch.Engine.Tasks
{
    public static class ObservableListExtensions
    {
        public static int Count<T>(this ObservableList<T> list, Func<T, bool> predicate)
        {
            lock (list)
            {
                return ((IEnumerable<T>)list).Count(predicate);
            }
        }
    }
}
