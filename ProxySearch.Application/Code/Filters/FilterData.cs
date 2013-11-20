using System;

namespace ProxySearch.Console.Code.Filters
{
    public class FilterData
    {
        public FilterData(IComparable data, int count)
        {
            Data = data;
            Count = count;
        }

        public IComparable Data
        {
            get;
            private set;
        }

        public int Count
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Data, Count);
        }
    }
}
