using System.Collections.Generic;
using ProxySearch.Console.Code.Filters;

namespace ProxySearch.Console.Code.SearchResult
{
    public class FilterDataComparer : IComparer<FilterData>
    {
        public int Compare(FilterData x, FilterData y)
        {
            if (object.ReferenceEquals(x, y))
                return 0;

            if (x == null || x.Data == null)
                return -1;

            if (y == null || y.Data == null)
                return 1;

            return x.Data.CompareTo(y.Data);
        }
    }
}
