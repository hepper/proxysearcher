using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Extensions;
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
                return Context.Get<ITaskManager>()
                              .Tasks
                              .GroupBy(task => task.Name)
                              .Select(group => new
                              {
                                  Name = group.Key,
                                  Tasks = NormalizeTaskCount(group).ToArray(),
                                  TotalCount = group.Count()
                              })
                              .ToArray();
            }
        }

        private static IEnumerable<TaskData> NormalizeTaskCount(IEnumerable<TaskData> tasks)
        {
            int maxCount = 30;

            if (tasks.Count() <= maxCount)
                return tasks;

            List<TaskData> result = tasks.Take(maxCount).ToList();

            result.Add(new TaskData 
            {
                Details = string.Format(Properties.Resources.AndMoreFormat, tasks.Count() - maxCount)
            });

            return result;
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

            Context.Get<ITaskManager>().Tasks.CollectionChanged += (sender, e) =>
            {
                ((Action)UpdateTaskUI).RunWithDelay(TimeSpan.FromMilliseconds(100));
            };
        }

        private void TaskManager_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                updatePortsTimer = new System.Timers.Timer(500);

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
            int maxWorkerThreads;
            int maxCompetitionPortThreads;

            ThreadPool.GetAvailableThreads(out workerThreads, out competitionPortThreads);
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompetitionPortThreads);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                ThreadsCount = maxWorkerThreads - workerThreads;
                CompetitionPortThreadsCount = maxCompetitionPortThreads - competitionPortThreads;
            }));
        }

        private void UpdateTaskUI()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                items.GetBindingExpression(ItemsControl.ItemsSourceProperty).UpdateTarget();
            }));
        }
    }
}
