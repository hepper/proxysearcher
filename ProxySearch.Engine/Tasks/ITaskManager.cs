using System;

namespace ProxySearch.Engine.Tasks
{
    public interface ITaskManager
    {
        TaskItem Create(string taskName);
        ObservableList<TaskData> Tasks { get; }

        event Action OnCompleted;
        event Action OnStarted;
    }
}
