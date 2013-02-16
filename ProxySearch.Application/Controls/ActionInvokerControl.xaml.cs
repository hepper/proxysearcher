using System;
using System.ComponentModel;
using System.IO;
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

            try
            {
                Task.Run(action);
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
            StatusText.Content = ProxySearch.Console.Properties.Resources.ErrorHasHappened;
            StatusText.Foreground = new SolidColorBrush(Colors.Red);

            LastException = exception;
            Details.IsEnabled = true;
        }

        public void UpdateStatus(string status)
        {
            StatusText.Content = status;
        }

        private void SetInformation(string text)
        {
            StatusText.Content = text;
            StatusText.Foreground = new SolidColorBrush(Colors.Black);

            LastException = null;
        }

        private Timer timer = null;

        private void SetProgress(bool setProgress)
        {
            ProgressBar.IsIndeterminate = setProgress;
            if (setProgress)
            {
                Details.Content = Properties.Resources.Cancel;
                Details.Click -= Details_Click;
                Details.Click += Cancel_Click;
                Details.IsEnabled = setProgress;
                ProgressText.Content = string.Format(Properties.Resources.JobCountFormat, 0);
                timer = new Timer(state => UpdateThreadPoolInfo());
                timer.Change(0, 500);
            }
            else
            {
                Details.Content = Properties.Resources.Details;
                Details.Click -= Cancel_Click;
                Details.Click += Details_Click;
                Details.IsEnabled = LastException != null;
                ProgressText.Content = null;

                timer.Dispose();
                timer = null;
                ActiveThreads = 0;
            }
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

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            App.ShowException(Window.GetWindow(this), LastException);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Details.Content = Properties.Resources.Cancelling;
            Details.IsEnabled = false;

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
    }
}
