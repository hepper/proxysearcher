using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.GoogleAnalytics;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Error;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for ActionInvoker.xaml
    /// </summary>
    public partial class ActionInvokerControl : UserControl, IActionInvoker, IErrorFeedback
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private static readonly DependencyPropertyKey IsInProgressPropertyKey =
            DependencyProperty.RegisterReadOnly("IsInProgress", typeof(bool), typeof(ActionInvokerControl), new PropertyMetadata(false));

        private static readonly DependencyProperty ActiveThreadsCountProperty =
            DependencyProperty.Register("ActiveThreadsCount", typeof(int), typeof(ActionInvokerControl));

        public static readonly DependencyProperty IsInProgressProperty = IsInProgressPropertyKey.DependencyProperty;

        public static readonly DependencyProperty StatusTextProperty =
            DependencyProperty.Register("StatusText", typeof(string), typeof(ActionInvokerControl));

        public static readonly DependencyProperty IsTopmostProperty = DependencyProperty.Register("IsTopmost", typeof(bool), typeof(ActionInvokerControl),
            new PropertyMetadata(true));

        public bool IsInProgress
        {
            get
            {
                return (bool)GetValue(IsInProgressProperty);
            }
            protected set
            {
                SetValue(IsInProgressPropertyKey, value);
            }
        }

        public string StatusText
        {
            get
            {
                return (string)GetValue(StatusTextProperty);
            }
            set
            {
                SetValue(StatusTextProperty, value);
            }
        }

        private int ActiveThreadsCount
        {
            get
            {
                return (int)GetValue(ActiveThreadsCountProperty);
            }
            set
            {
                SetValue(ActiveThreadsCountProperty, value);
            }
        }

        private bool IsTopmost
        {
            get { return (bool)GetValue(IsTopmostProperty); }
            set { SetValue(IsTopmostProperty, value); }
        }

        private Exception LastException
        {
            get;
            set;
        }

        public ActionInvokerControl()
        {
            InitializeComponent();

            StatusText = Controls.Resources.ActionInvokerControl.Ready;

            Context.Get<ITaskManager>().Tasks.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add ||
                    e.Action == NotifyCollectionChangedAction.Remove ||
                    e.Action == NotifyCollectionChangedAction.Reset)
                {
                    string text = string.Format(Properties.Resources.JobCountFormat, Context.Get<ITaskManager>().Tasks.Count);

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ProgressText.Content = text;
                        UpdateThreadPoolInfo();
                    }));
                }
            };

            Context.Get<ITaskManager>().OnStarted += () => 
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    SetProgress(true);
                    StatusText = Properties.Resources.WaitUntilCurrentOperationIsFinished;
                    ErrorButton.Visibility = Visibility.Collapsed;
                    Context.Get<ISearchResult>().Started();
                }));
            };
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

        public void Finished(bool setReadyStatus)
        {
            Completed(Context.Get<ISearchResult>().Completed, setReadyStatus);
        }

        public void Cancelled(bool setReadyStatus)
        {
            Completed(Context.Get<ISearchResult>().Cancelled, setReadyStatus);
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

        private void Completed(Action action, bool setReadyStatus)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SetProgress(false);
                if (setReadyStatus)
                    StatusText = Properties.Resources.Ready;
                action();
            }));
        }

        private void SetProgress(bool setProgress)
        {
            if (!setProgress)
            {
                Cancel.Content = Properties.Resources.Cancel;
                ProgressText.Content = null;
                ActiveThreadsCount = 0;
            }

            Cancel.IsEnabled = setProgress;
            UpdateThreadPoolInfo();
            IsInProgress = setProgress;
        }

        private void UpdateThreadPoolInfo()
        {
            int workerThreads;
            int competitionPortThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out competitionPortThreads);

            int threads = Math.Min(workerThreads, competitionPortThreads);

            ActiveThreadsCount = (int)(100 * ((double)Context.Get<AllSettings>().MaxThreadCount - threads) / Context.Get<AllSettings>().MaxThreadCount);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Context.Get<IGA>().TrackEventAsync(EventType.ButtonClick, Buttons.CancelSearch.ToString());

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

        private void Error_Click(object sender, RoutedEventArgs e)
        {
            App.ShowException(Window.GetWindow(this), LastException);
        }

        protected void Control_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(toggleButton);

            if (window != null)
            {
                Action updatePopupLocation = () =>
                {
                    popup.HorizontalOffset++;
                    popup.HorizontalOffset--;
                };

                window.LocationChanged += (sender1, e1) => updatePopupLocation();
                window.SizeChanged += (sender1, e1) => updatePopupLocation();

                resizeControl.SizeUpdated += (sender1, e1) =>
                {
                    updatePopupLocation();
                    popup.Width = resizeControl.Width;
                    popup.Height = resizeControl.Height;
                };

                window.Activated += (sender1, e1) =>
                {
                    IsTopmost = true;
                };

                window.Deactivated += (sender1, e1) =>
                {
                    IsTopmost = false;
                };
            }
        }

        protected void Popup_Closed(object sender, EventArgs e)
        {
            if (IsTopmost)
                toggleButton.IsChecked = false;
        }

        private void toggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Context.Get<IGA>().TrackEventAsync(EventType.ButtonClick, Buttons.TaskManager.ToString(), true); 
        }

        private void toggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Context.Get<IGA>().TrackEventAsync(EventType.ButtonClick, Buttons.TaskManager.ToString(), false);
        }
    }
}