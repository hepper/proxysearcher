using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ProxyCheckerByUrlControl.xaml
    /// </summary>
    public partial class ProxyCheckerByUrlControl : UserControl
    {
        List<object> Arguments
        {
            get;
            set;
        }

        public ProxyCheckerByUrlControl(List<object> arguments)
        {
            Arguments = arguments;

            InitializeComponent();
        }

        public string Url
        {
            get
            {
                return (string)Arguments[0];
            }
            set
            {
                Arguments[0] = value;
            }
        }
        public double Accuracy
        {
            get
            {
                return 1 - (double)Arguments[1];
            }
            set
            {
                Arguments[1] = 1 - value;
            }
        }

    }
}
