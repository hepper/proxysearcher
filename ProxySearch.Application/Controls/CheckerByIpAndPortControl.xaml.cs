using System.Collections.Generic;
using System.Windows.Controls;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for CheckerByIpAndPort.xaml
    /// </summary>
    public partial class CheckerByIpAndPortControl : UserControl
    {
        private List<object> Arguments
        {
            get;
            set;
        }

        public CheckerByIpAndPortControl(List<object> arguments)
        {
             Arguments = arguments;

            InitializeComponent();
        }

        public int MaxAsyncChecks
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