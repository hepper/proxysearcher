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
    /// Interaction logic for CheckerProxyNetPropertyControl.xaml
    /// </summary>
    public partial class CheckerProxyNetPropertyControl : UserControl
    {
        private List<object> Arguments
        {
            get;
            set;
        }

        public CheckerProxyNetPropertyControl(List<object> arguments)
        {
            Arguments = arguments;

            InitializeComponent();
        }

        public int Timeout
        {
            get
            {
                return (int)Arguments[0];
            }

            set
            {
                Arguments[0] = value;
            }
        }
    }
}
