using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for SelectFolderControl.xaml
    /// </summary>
    public partial class SelectFolderControl : System.Windows.Controls.UserControl
    {
        public static readonly DependencyProperty SelectedFolderProperty = DependencyProperty.Register("SelectedFolder", typeof(string), typeof(SelectFolderControl));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(SelectFolderControl));
        
        public SelectFolderControl()
        {
            InitializeComponent();
        }

        public string SelectedFolder
        {
            get
            {
                return (string)this.GetValue(SelectedFolderProperty);
            }
            set
            {
                this.SetValue(SelectedFolderProperty, value);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return (bool)this.GetValue(IsReadOnlyProperty);
            }
            set
            {
                this.SetValue(IsReadOnlyProperty, value);
            }
        }

        private void SelectFolder(object sender, RoutedEventArgs e)
        {
            CreateFolderIfNotExists();

            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = SelectedFolder
            };

            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                SelectedFolder = dialog.SelectedPath;
                SelectedFoderTextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateTarget();
            }
        }

        private void ExploreFolder(object sender, RoutedEventArgs e)
        {
            CreateFolderIfNotExists();
            Process.Start(SelectedFolder);
        }

        private void CreateFolderIfNotExists()
        {
            if (!Directory.Exists(SelectedFolder))
            {
                Directory.CreateDirectory(SelectedFolder);
            }            
        }
    }
}
