using System.Collections.Generic;
using System.Windows.Controls;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for UrlListPropertyControl.xaml
    /// </summary>
    public partial class UrlListPropertyControl : UserControl
    {
        private List<object> Arguments
        {
            get;
            set;
        }

        public UrlListPropertyControl(List<object> arguments)
        {
            Arguments = arguments;

            InitializeComponent();
        }

        public string UrlList
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
    }
}
