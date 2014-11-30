using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for PagingControl.xaml
    /// </summary>
    public partial class PagingControl : UserControl
    {
        public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(PagingControl));
        public static readonly DependencyProperty PageProperty = DependencyProperty.Register("Page", typeof(int?), typeof(PagingControl));
        public static readonly RoutedEvent PageChangedEvent = EventManager.RegisterRoutedEvent("PageChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PagingControl));

        public PagingControl()
        {
            InitializeComponent();

            DependencyPropertyDescriptor.FromProperty(CountProperty, typeof(PagingControl)).AddValueChanged(this, CountChangedHandler);
            DependencyPropertyDescriptor.FromProperty(PageProperty, typeof(PagingControl)).AddValueChanged(this, PageChangedHandler);
        }

        private void CountChangedHandler(object sender, EventArgs e)
        {
            if (Count == 0)
            {
                Page = null;
            }
            else if (!Page.HasValue)
            {
                Page = 1;
            }
        }

        private void PageChangedHandler(object sender, EventArgs e)
        {
            if (Page.HasValue)
            {
                if (Page < 1)
                {
                    Page = 1;
                    return;
                }

                if (Page.Value > PageCount)
                {
                    Page = PageCount;
                    return;
                }
            }

            RaiseEvent(new RoutedEventArgs(PageChangedEvent));
        }

        public event RoutedEventHandler PageChanged
        {
            add
            {
                AddHandler(PageChangedEvent, value);
            }
            remove
            {
                RemoveHandler(PageChangedEvent, value);
            }
        }

        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((double)Count / Context.Get<AllSettings>().PageSize);
            }
        }

        public int Count
        {
            get
            {
                return (int)this.GetValue(CountProperty);
            }
            set
            {
                this.SetValue(CountProperty, value);
            }
        }

        public int? Page
        {
            get
            {
                return (int?)this.GetValue(PageProperty);
            }
            set
            {
                this.SetValue(PageProperty, value);
            }
        }

        private void GoTop(object sender, RoutedEventArgs e)
        {
            Page = 1;
        }

        private void GoLeft(object sender, RoutedEventArgs e)
        {
            Page--;
        }

        private void GoRight(object sender, RoutedEventArgs e)
        {
            Page++;
        }

        private void GoBottom(object sender, RoutedEventArgs e)
        {
            Page = PageCount;
        }

        private void GoPage(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CurrentPageTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }
    }
}
