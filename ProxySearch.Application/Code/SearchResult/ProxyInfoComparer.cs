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

            if (object1 == null && object2 == null)
                return 0;

            if (object1 == null)
            {
                return -1;
            }

            if (object2 == null)
            {
                return 1;
            }

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
                    return source.Address.ToString();
                case "Port":
                    return source.Port;
                case "CountryInfo.Name":
                    if (source.CountryInfo == null)
                        return null;

                    return source.CountryInfo.Name;
                case "Details.Type":
                    if (source.Details == null)
                        return null;

                    return source.Details.Type;
                case "BandwidthData":
                    return source.BandwidthData;
            }

            throw new NotSupportedException(string.Format(Properties.Resources.SortTypeIsNotSupported, path));
        }
    }
}
