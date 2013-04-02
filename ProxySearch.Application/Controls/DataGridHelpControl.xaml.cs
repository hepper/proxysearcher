using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for DataGridHelpControl.xaml
    /// </summary>
    public partial class DataGridHelpControl : UserControl
    {
        public static readonly DependencyProperty ProxyInfoProperty = DependencyProperty.Register("Text", typeof(string), typeof(DataGridHelpControl));

        public DataGridHelpControl()
        {
            InitializeComponent();
        }

        public string Text
        {
            get
            {
                return (string)this.GetValue(ProxyInfoProperty);
            }
            set
            {
                this.SetValue(ProxyInfoProperty, value);
            }
        }
    }
}
