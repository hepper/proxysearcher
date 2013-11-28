using System;
using System.Collections.Generic;
using ProxySearch.Console.Code.Filters;

namespace ProxySearch.Console.Code.SearchResult
{
    public class FilterDataComparer : IComparer<FilterData>
    {
        public int Compare(FilterData x, FilterData y)
        {
            return x.Data.CompareTo(y.Data);
        }
    }
}
