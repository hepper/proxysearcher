using System.Collections.Generic;
using System.Windows.Controls;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for FolderSearchEngineControl.xaml
    /// </summary>
    public partial class FolderSearchEngineControl : UserControl
    {
        private List<object> Arguments
        {
            get;
            set;
        }

        public FolderSearchEngineControl(List<object> arguments)
        {
            Arguments = arguments;

            InitializeComponent();
        }

        public string Folder
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
