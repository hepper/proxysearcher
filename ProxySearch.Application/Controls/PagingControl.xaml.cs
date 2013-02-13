using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Engine;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for PagingControl.xaml
    /// </summary>
    public partial class PagingControl : UserControl
    {
        public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(PagingControl));

        public PagingControl()
        {
            InitializeComponent();
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

        public int? CurrentPage
        {
            get;
            set;
        }
    }
}
