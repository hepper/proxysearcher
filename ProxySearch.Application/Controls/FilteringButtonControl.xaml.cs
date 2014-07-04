using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ProxySearch.Common;
using ProxySearch.Console.Code.Filters;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for FilteringButtonControl.xaml
    /// </summary>
    public partial class FilteringButtonControl : UserControl
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(IEnumerable<FilterData>), typeof(FilteringButtonControl));

        public FilteringButtonControl()
        {
            InitializeComponent();

            SelectedData.CollectionChanged += (sender, e) =>
            {
                Button.GetBindingExpression(ToggleButton.IsCheckedProperty).UpdateTarget();
            };
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

        public ObservableList<IComparable> SelectedData
        {
            get
            {
                return Popup != null ? Popup.SelectedData : null;
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Popup.IsOpen = true;
            Button.GetBindingExpression(ToggleButton.IsCheckedProperty).UpdateTarget();
        }
    }
}
