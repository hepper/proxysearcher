using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProxySearch.Engine;

namespace ProxySearch.Console.Code.SearchResult
{
    public class ProxyInfoComparer : IComparer<ProxyInfo>
    {
        private string SortMemberPath
        {
            get;
            set;
        }

        private ListSortDirection SortDirection
        {
            get;
            set;
        }

        public ProxyInfoComparer(string sortMemberPath, ListSortDirection sortDirection)
        {
            SortMemberPath = sortMemberPath;
            SortDirection = sortDirection;
        }

        public int Compare(ProxyInfo x, ProxyInfo y)
        {
            IComparable object1 = GetPropertyValue(x, SortMemberPath);
            IComparable object2 = GetPropertyValue(y, SortMemberPath);

            if (SortDirection == ListSortDirection.Descending)
            {
                return object2.CompareTo(object1);
            }

            return object1.CompareTo(object2);
        }

        private IComparable GetPropertyValue(ProxyInfo source, string path)
        {
            switch (path)
            {
                case "AddressString":
                    return source.AddressString;
                case "Port":
                    return source.Port;
                case "CountryInfo.Name":
                    return source.CountryInfo.Name;
                case "Details.Type":
                    return source.Details.Type;
            }

            throw new NotSupportedException(string.Format(Properties.Resources.SortTypeIsNotSupported, path));
        }
    }
}
