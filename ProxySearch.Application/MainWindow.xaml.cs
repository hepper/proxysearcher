using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using ProxySearch.Common;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Error;
using ProxySearch.Engine.SearchEngines.Google;

namespace ProxySearch.Console
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ICaptchaWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Context.Set<IControlNavigator>(new ControlNavigator(Placeholder));
            Context.Set<IActionInvoker>(ActionInvoker);
            Context.Set<IErrorFeedback>(ActionInvoker);
            Context.Set<ICaptchaWindow>(this);

            WindowState = MainWindowState.IsMaximized ? WindowState.Maximized : WindowState.Normal;

            if (!MainWindowState.IsMaximized)
            {
                Left = MainWindowState.Location.X;
                Top = MainWindowState.Location.Y;
                Width = MainWindowState.Size.Width;
                Height = MainWindowState.Size.Height;
            }
        }

        private MainWindowState MainWindowState
        {
            get
            {
                return Context.Get<AllSettings>().MainWindowState;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            MainWindowState.IsMaximized = WindowState == WindowState.Maximized;
            MainWindowState.Location = new Point(Left, Top);
            MainWindowState.Size = new Size(Width, Height);

            base.OnClosing(e);
        }

        public void ShowControl(UserControl control)
        {
            Context.Get<IControlNavigator>().GoTo(control);
        }

        public void GoToSearch()
        {
            Context.Get<IControlNavigator>().GoToSearch();
        }

        public void Show(string url)
        {
            throw new NotSupportedException();
        }

        public Task<string> GetSolvedContentAsync(string url, int pageNumber, CancellationToken cancellationToken)
        {
            Context.Get<IMessageBox>().Error(Properties.Resources.GoogleDetectsSendingOfAutomaticQueries);
            throw new InvalidOperationException();

            //Add 'Microsoft HTML object library' reference (COM)
            //TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();

            //webBrowser.Navigated += (sender, e) =>
            //{
            //    Dispatcher.Invoke(() =>
            //    {
            //        CaptchaRegion.Visibility = Visibility.Visible;

            //        CaptchaExplanation.Content = string.Format("Page {0}", pageNumber);
            //    });

            //    LoadCompletedEventHandler handler = null;

            //    handler = (sender1, e1) =>
            //    {
            //        IHTMLDocument2 document = webBrowser.Document as IHTMLDocument2;

            //        string content = document.body.outerHTML;
            //        string loweredContent = content.ToLower();

            //        if (!loweredContent.Contains("captcha") && !loweredContent.Contains("redirecting") && !taskCompletionSource.Task.IsCompleted)
            //        {
            //            webBrowser.LoadCompleted -= handler;
            //            Dispatcher.Invoke(() => CaptchaRegion.Visibility = Visibility.Collapsed);
            //            taskCompletionSource.SetResult(content);
            //        }
            //    };

            //    webBrowser.LoadCompleted += handler;
            //};

            //Dispatcher.Invoke(() => webBrowser.Navigate(new Uri(url)));

            //return taskCompletionSource.Task;
        }
    }
}
