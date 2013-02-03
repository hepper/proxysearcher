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

            public TaskCounterItem(TaskCounter counter, TaskType type)
            {
                Type = type;

                Counter = counter;
                Counter.Started(Type);
            }
            public void Dispose()
            {
                Counter.Completed(Type);
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

        public IDisposable Listen(TaskType type)
        {
            return new TaskCounterItem(this, type);
        }

        public void Started(TaskType type)
        {
            int currentCount;
            int currentTypeCount;

            lock (this)
            {
                TaskCount++;

                if (Tasks.ContainsKey(type))
                {
                    Tasks[type]++;
                }
                else
                {
                    Tasks.Add(type, 1);
                }

                currentCount = TaskCount;
                currentTypeCount = Tasks[type];
            }

            FireJobCountChanged(type, currentTypeCount, currentCount);
        }

        public void Completed(TaskType type)
        {
            int currentCount;
            int currentTypeCount;

            lock (this)
            {
                TaskCount--;
                Tasks[type]--;

                currentCount = TaskCount;
                currentTypeCount = Tasks[type];
            }

            FireJobCountChanged(type, currentTypeCount, currentCount);

            if (currentCount == 0 && AllCompleted != null)
            {
                AllCompleted();
            }
        }

        private void FireJobCountChanged(TaskType type, int currentCount, int totalCount)
        {
            if (JobCountChanged != null)
                JobCountChanged(type, currentCount, totalCount);
        }

        public event Action AllCompleted;
        public event Action<TaskType, int, int> JobCountChanged;
    }
}
