using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ProxySearch.Common;
using ProxySearch.Console.Code.Extensions;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for TaskManagerControl.xaml
    /// </summary>
    public partial class TaskManagerControl : UserControl
    {
        private static readonly DependencyProperty ThreadsCountProperty =
            DependencyProperty.Register("ThreadsCount", typeof(int), typeof(TaskManagerControl));

        private static readonly DependencyProperty CompetitionPortThreadsCountProperty =
           DependencyProperty.Register("CompetitionPortThreadsCount", typeof(int), typeof(TaskManagerControl));

        public IEnumerable Tasks
        {
            get
            {
                return Context.Get<TaskManager>()
                              .Tasks
                              .GroupBy(task => task.Name)
                              .Select(group => new
                              {
                                  Name = group.Key,
                                  Tasks = group.ToArray()
                              })
                              .ToArray();
            }
        }

        private int ThreadsCount
        {
            get
            {
                return (int)GetValue(ThreadsCountProperty);
            }
            set
            {
                SetValue(ThreadsCountProperty, value);
            }
        }

        private int CompetitionPortThreadsCount
        {
            get
            {
                return (int)GetValue(CompetitionPortThreadsCountProperty);
            }
            set
            {
                SetValue(CompetitionPortThreadsCountProperty, value);
            }
        }

        private System.Timers.Timer updatePortsTimer;

        public TaskManagerControl()
        {
            InitializeComponent();

            Context.Get<TaskManager>().Tasks.CollectionChanged += (sender, e) =>
            {
                ((Action)UpdateTaskUI).RunWithDelay(TimeSpan.FromMilliseconds(100));
            };
        }

        private void TaskManager_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                updatePortsTimer = new System.Timers.Timer(100);

                updatePortsTimer.Elapsed += (sender1, e1) => UpdateThreadUI();
                updatePortsTimer.Start();
            }
            else
            {
                updatePortsTimer.Stop();
            }
        }

        private void UpdateThreadUI()
        {
            int workerThreads;
            int competitionPortThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out competitionPortThreads);

            Dispatcher.Invoke(() =>
            {
                ThreadsCount = Context.Get<AllSettings>().MaxThreadCount - workerThreads;
                CompetitionPortThreadsCount = Context.Get<AllSettings>().MaxThreadCount - competitionPortThreads;
            });
        }

        private void UpdateTaskUI()
        {
            Dispatcher.Invoke(() =>
            {
                items.GetBindingExpression(ItemsControl.ItemsSourceProperty).UpdateTarget();
            });
        }
    }
}
