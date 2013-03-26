using System;
using System.Threading.Tasks;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IActionInvoker
    {
        void StartAsync(Action action);
        void Begin();
        void End();
        void Update(int count);
        void UpdateStatus(string status);
        void SetException(Exception exception);
    }
}