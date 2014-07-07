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

        public void UpdateDetails(string details, TaskStatus status = TaskStatus.Normal)
        {
            TaskData.Details = details;
            TaskData.Status = status;
        }

        public abstract void Dispose();
    }
}
