using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.GoogleAnalytics;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for ActionInvoker.xaml
    /// </summary>
    public partial class ActionInvokerControl : UserControl, IActionInvoker, INotifyPropertyChanged
    {
        private Exception LastException
        {
            get;
            set;
        }

        public ActionInvokerControl()
        {
            InitializeComponent();
        }

        public void StartAsync(Action action)
        {
            ThreadPool.SetMaxThreads(Context.Get<AllSettings>().MaxThreadCount, Context.Get<AllSettings>().MaxThreadCount);

            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception exception)
                    {
                        Dispatcher.Invoke(() => SetException(exception));
                    }
                });
            }
            catch (TaskCanceledException)
            {
            }
        }

        public void Begin()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SetProgress(true);
                SetInformation(Properties.Resources.WaitUntilCurrentOperationIsFinished);
                ErrorButton.Visibility = Visibility.Hidden;
                Context.Get<ISearchResult>().Started();
            }));
        }

        public void Finished(bool setReadyStatus)
        {
            Completed(Context.Get<ISearchResult>().Completed, setReadyStatus);
        }

        public void Cancelled(bool setReadyStatus)
        {
            Completed(Context.Get<ISearchResult>().Cancelled, setReadyStatus);
        }

        private void Completed(Action action, bool setReadyStatus)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SetProgress(false);
                if (setReadyStatus)
                    SetInformation(Properties.Resources.Ready);
                Context.Get<ISearchControl>().Completed();
                action();
            }));
        }

        public void Update(int count)
        {
            string text = string.Format(Properties.Resources.JobCountFormat, count);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                ProgressText.Content = text;
                UpdateThreadPoolInfo();
            }));
        }

        public void SetException(Exception exception)
        {
            //It is a bug of wpf grid. Just ignore it
            if (exception is ArgumentOutOfRangeException && exception.Source == "PresentationFramework")
                return;

            if (Dispatcher.CheckAccess())
            {
                Context.Get<IGA>().TrackException(exception);
                Context.Get<IExceptionLogging>().Write(exception);

                LastException = exception;

                if (Dispatcher.CheckAccess())
                {
                    ErrorButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(() => SetException(exception)));
            }
        }

        public void UpdateStatus(string status)
        {
            StatusText.Content = status;
        }

        private void SetInformation(string text)
        {
            StatusText.Content = text;
        }

        private void SetProgress(bool setProgress)
        {
            ProgressBar.IsIndeterminate = setProgress;
            if (setProgress)
            {
                ProgressText.Content = string.Format(Properties.Resources.JobCountFormat, 0);
            }
            else
            {
                Cancel.Content = Properties.Resources.Cancel;
                ProgressText.Content = null;
                ActiveThreads = 0;
            }

            Cancel.IsEnabled = setProgress;
            UpdateThreadPoolInfo();
        }

        private void UpdateThreadPoolInfo()
        {
            int workerThreads;
            int competitionPortThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out competitionPortThreads);

            int threads = Math.Min(workerThreads, competitionPortThreads);

            ActiveThreads = (int)(100 * ((double)Context.Get<AllSettings>().MaxThreadCount - threads) / Context.Get<AllSettings>().MaxThreadCount);
        }

        private int activeThreads;
        public int ActiveThreads
        {
            get
            {
                return activeThreads;
            }
            set
            {
                activeThreads = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ActiveThreads"));
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Context.Get<IGA>().TrackEventAsync(EventType.ButtonClick, Cancel.Content.ToString());

            Cancel.Content = Properties.Resources.Cancelling;
            Cancel.IsEnabled = false;

            new Thread(CancelOperation).Start();
        }

        private void CancelOperation()
        {
            try
            {
                Context.Get<CancellationTokenSource>().Cancel(false);
            }
            catch (AggregateException)
            {
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Error_Click(object sender, RoutedEventArgs e)
        {
            App.ShowException(Window.GetWindow(this), LastException);
        }
    }
}
