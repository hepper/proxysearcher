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
    /// Interaction logic for GoogleEnginePropertyControl.xaml
    /// </summary>
    public partial class GoogleEnginePropertyControl : UserControl
    {
        private List<object> Arguments
        {
            get;
            set;
        }

        public GoogleEnginePropertyControl(List<object> arguments)
        {
            Arguments = arguments;

            InitializeComponent();            
        }

        public int SearchPageCount
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

        public string SearchKeywords
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
    }
}
