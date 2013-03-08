using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProxySearch.Common;
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

        public void Begin(Action action)
        {
            ThreadPool.SetMaxThreads(Context.Get<AllSettings>().MaxThreadCount, Context.Get<AllSettings>().MaxThreadCount);

            SetProgress(true);
            SetInformation(Properties.Resources.WaitUntilCurrentOperationIsFinished);
            ErrorButton.Visibility = Visibility.Hidden;

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

        public void End()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SetProgress(false);
                SetInformation(Properties.Resources.Ready);
                Context.Get<ISearchControl>().Completed();
            }));
        }

        public void Update(int count)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ProgressText.Content = string.Format(Properties.Resources.JobCountFormat, count);
            }));
        }

        public void SetException(Exception exception)
        {
            if (Dispatcher.CheckAccess())
            {
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

        private Timer timer = null;

        private void SetProgress(bool setProgress)
        {
            ProgressBar.IsIndeterminate = setProgress;
            if (setProgress)
            {
                ProgressText.Content = string.Format(Properties.Resources.JobCountFormat, 0);
                timer = new Timer(state => UpdateThreadPoolInfo());
                timer.Change(0, 2000);
            }
            else
            {
                Cancel.Content = Properties.Resources.Cancel;
                ProgressText.Content = null;

                timer.Dispose();
                timer = null;
                ActiveThreads = 0;
            }

            Cancel.IsEnabled = setProgress;
        }

        private void UpdateThreadPoolInfo()
        {
            int workerThreads;
            int competitionPortThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out competitionPortThreads);

            ActiveThreads = (int)(100 * ((double)Context.Get<AllSettings>().MaxThreadCount - workerThreads) / Context.Get<AllSettings>().MaxThreadCount);
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
