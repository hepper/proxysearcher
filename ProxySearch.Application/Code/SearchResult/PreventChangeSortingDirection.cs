using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.SearchResult
{
    public class PreventChangeSortingDirection : IDisposable
    {
        private DataGridColumn SortedColumn
        {
            get;
            set;
        }

        public bool HasSorting
        {
            get
            {
                return SortedColumn != null;
            }
        }

        public string SortMemberPath
        {
            get
            {
                if (!HasSorting)
                {
                    throw new InvalidOperationException(Resources.GridHasNoSorting);
                }

                return SortedColumn.SortMemberPath;
            }
        }

        private ListSortDirection sortDirection;
        public ListSortDirection SortDirection
        {
            get
            {
                if (!HasSorting)
                {
                    throw new InvalidOperationException(Resources.GridHasNoSorting);
                }

                return sortDirection;
            }
        }

        public PreventChangeSortingDirection(DataGrid control)
        {
            SortedColumn = control.Columns.SingleOrDefault(column => column.SortDirection.HasValue);

            if (SortedColumn != null)
            {
                sortDirection = SortedColumn.SortDirection.Value;
            }
        }

        public void Dispose()
        {
            if (HasSorting)
            {
                SortedColumn.SortDirection = sortDirection;
            }
        }
    }
}
