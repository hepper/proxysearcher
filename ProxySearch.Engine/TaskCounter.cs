using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySearch.Engine
{
    public class TaskCounter
    {
        private class TaskCounterItem : IDisposable
        {
            private TaskCounter Counter
            {
                get;
                set;
            }

            private TaskType Type
            {
                get;
                set;
            }

            private int Count
            {
                get;
                set;
            }

            public TaskCounterItem(TaskCounter counter, TaskType type, int count = 1)
            {
                Counter = counter;
                Type = type;
                Count = count;

                Counter.Started(Type, Count);
            }

            public void Dispose()
            {
                Counter.Completed(Type, Count);
            }
        }

        private int TaskCount
        {
            get;
            set;
        }

        private Dictionary<TaskType, int> Tasks
        {
            get;
            set;
        }

        public TaskCounter()
        {
            Tasks = new Dictionary<TaskType, int>();
        }

        public IDisposable Listen(TaskType type, int count = 1)
        {
            return new TaskCounterItem(this, type, count);
        }

        public void Started(TaskType type, int count = 1)
        {
            int currentCount;
            int currentTypeCount;
            bool started = false;

            lock (this)
            {
                started = TaskCount == 0;
                TaskCount += count;

                if (Tasks.ContainsKey(type))
                {
                    Tasks[type] += count;
                }
                else
                {
                    Tasks.Add(type, 1);
                }

                currentCount = TaskCount;
                currentTypeCount = Tasks[type];
            }

            if (started && OnStarted != null)
                OnStarted();

            FireJobCountChanged(type, currentTypeCount, currentCount);
        }

        public void Completed(TaskType type, int count = 1)
        {
            int currentCount;
            int currentTypeCount;

            lock (this)
            {
                TaskCount -= count;
                Tasks[type] -= count;

                currentCount = TaskCount;
                currentTypeCount = Tasks[type];
            }

            FireJobCountChanged(type, currentTypeCount, currentCount);

            if (currentCount == 0 && OnCompleted != null)
            {
                OnCompleted();
            }
        }

        private void FireJobCountChanged(TaskType type, int currentCount, int totalCount)
        {
            if (JobCountChanged != null)
                JobCountChanged(type, currentCount, totalCount);
        }

        public event Action OnStarted;
        public event Action OnCompleted;
        public event Action<TaskType, int, int> JobCountChanged;
    }
}
