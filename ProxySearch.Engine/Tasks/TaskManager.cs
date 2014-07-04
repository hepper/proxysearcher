using System;
using ProxySearch.Common;

namespace ProxySearch.Engine.Tasks
{
    public class TaskManager
    {
        private class TaskItemInternal : TaskItem
        {
            private TaskManager Manager
            {
                get;
                set;
            }

            public TaskItemInternal(TaskManager manager, string taskName)
                : base(new TaskData { Name = taskName, Details = null })
            {
                Manager = manager;
                Manager.Started(TaskData);
            }

            public override void Dispose()
            {
                Manager.Completed(TaskData);
            }
        }

        public ObservableList<TaskData> Tasks
        {
            get;
            private set;
        }

        public TaskManager()
        {
            Tasks = new ObservableList<TaskData>();
        }

        public TaskItem Create(string taskName)
        {
            return new TaskItemInternal(this, taskName);
        }

        private void Started(TaskData taskData)
        {
            bool started = false;

            lock (this)
            {
                started = Tasks.Count == 0;
                Tasks.Add(taskData);
            }

            if (started && OnStarted != null)
                OnStarted();
        }

        private void Completed(TaskData taskData)
        {
            lock (this)
            {
                Tasks.Remove(taskData);
            }

            if (Tasks.Count == 0 && OnCompleted != null)
            {
                OnCompleted();
            }
        }

        public event Action OnStarted;
        public event Action OnCompleted;
    }
}
