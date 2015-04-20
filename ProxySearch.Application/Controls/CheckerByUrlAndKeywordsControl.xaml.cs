using System.Collections.Generic;
using System.Windows.Controls;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for CheckerByUrlAndKeywordsControl.xaml
    /// </summary>
    public partial class CheckerByUrlAndKeywordsControl : UserControl
    {
        private List<object> Arguments
        {
            get;
            set;
        }

        public CheckerByUrlAndKeywordsControl(List<object> arguments)
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
        public string Keywords
        {
            get
            {
                return (string)Arguments[1];
            }
            set
            {
                Arguments[1] = value;
            }
        }

        public int MaxAsyncChecks
        {
            get
            {
                return (int)Arguments[2];
            }
            set
            {
                Arguments[2] = value;
            }
        }
    }
}
