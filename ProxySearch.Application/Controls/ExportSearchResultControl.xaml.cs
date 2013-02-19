using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using ProxySearch.Common;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for ExportDataControl.xaml
    /// </summary>
    public partial class ExportSearchResultControl : System.Windows.Controls.UserControl
    {
        public ExportSearchResultControl()
        {
            InitializeComponent();
        }

        public ExportSettings Settings
        {
            get
            {
                return Context.Get<AllSettings>().ExportSettings;
            }
        }

        private void SelectFolder(object sender, RoutedEventArgs e)
        {
            CreateFolderIfNotExists();

            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = Settings.ExportFolder
            };

            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Settings.ExportFolder = dialog.SelectedPath;
                SelectedFoderTextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateTarget();
            }
        }

        private void ExploreFolder(object sender, RoutedEventArgs e)
        {
            CreateFolderIfNotExists();
        }

        private void CreateFolderIfNotExists()
        {
            if (!Directory.Exists(Settings.ExportFolder))
            {
                Directory.CreateDirectory(Settings.ExportFolder);
            }

            Process.Start(Settings.ExportFolder);
        }
    }
}
