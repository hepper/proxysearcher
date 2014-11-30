using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Console.Code.Filters;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for FilteringControl.xaml
    /// </summary>
    public partial class FilteringPopupControl : UserControl
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(IEnumerable<FilterData>), typeof(FilteringPopupControl));
        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(FilteringPopupControl));
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(FilteringPopupControl));
        public static readonly DependencyProperty SelectedDataProperty = DependencyProperty.Register("SelectedData", typeof(ObservableList<IComparable>), typeof(FilteringPopupControl));

        public FilteringPopupControl()
        {
            InitializeComponent();

            DependencyPropertyDescriptor.FromProperty(DataProperty, typeof(FilteringPopupControl)).AddValueChanged(this, (sender, e) =>
            {
                List<IComparable> toRemove = new List<IComparable>();

                foreach (IComparable selectedItem in SelectedData)
                {
                    if (Data.FirstOrDefault(item => item.Data == selectedItem) == null)
                        toRemove.Add(selectedItem);
                }

                SelectedData.RemoveAll(item => toRemove.Contains(item));
            });

            SelectedData = new ObservableList<IComparable>();
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
                return (ObservableList<IComparable>)GetValue(SelectedDataProperty);
            }
            set
            {
                SetValue(SelectedDataProperty, value);
            }
        }

        public bool IsOpen
        {
            get
            {
                return (bool)this.GetValue(IsOpenProperty);
            }
            set
            {
                SetValue(IsOpenProperty, value);
            }
        }

        public UIElement PlacementTarget
        {
            get
            {
                return (UIElement)GetValue(PlacementTargetProperty);
            }
            set
            {
                SetValue(PlacementTargetProperty, value);
            }
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            SelectedData.Clear();
            DataList.Items.Refresh();
            IsOpen = false;
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            FilterData item = (FilterData)((CheckBox)e.OriginalSource).Tag;

            if (!SelectedData.Contains(item.Data))
                SelectedData.Add(item.Data);
        }

        private void Unhecked(object sender, RoutedEventArgs e)
        {
            FilterData item = (FilterData)((CheckBox)e.OriginalSource).Tag;

            if (SelectedData.Contains(item.Data))
                SelectedData.Remove(item.Data);
        }
    }
}
