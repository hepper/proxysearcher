using System;

namespace ProxySearch.Engine.Tasks
{
    public abstract class TaskItem : IDisposable
    {
        protected TaskData TaskData
        {
            get;
            private set;
        }

        public TaskItem(TaskData taskData)
        {
            TaskData = taskData;
        }

        public void UpdateDetails(string details)
        {
            TaskData.Details = details;
        }

        public abstract void Dispose();
    }
}
