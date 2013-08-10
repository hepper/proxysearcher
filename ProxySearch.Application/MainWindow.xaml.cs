using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ProxySearch.Common;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;
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
            Context.Set<ICaptchaWindow>(this);
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
