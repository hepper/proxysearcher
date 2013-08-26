using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Console.Code.Collections;
using ProxySearch.Console.Code.Filters;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for DataGridHeaderFilteringControl.xaml
    /// </summary>
    public partial class DataGridHeaderFilteringControl : UserControl
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(IEnumerable<FilterData>), typeof(DataGridHeaderFilteringControl));
        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register("HeaderContent", typeof(object), typeof(DataGridHeaderFilteringControl));

        public DataGridHeaderFilteringControl()
        {
            InitializeComponent();
        }

        public IEnumerable<FilterData> Data
        {
            get
            {
                return (IEnumerable<FilterData>)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }

        public object HeaderContent
        {
            get
            {
                return (object)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }

        public ObservableList<IComparable> SelectedData
        {
            get
            {
                return filteringButtonControl.SelectedData;
            }
        }
    }
}
