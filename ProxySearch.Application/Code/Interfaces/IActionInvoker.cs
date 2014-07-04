using System;
using System.Threading.Tasks;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IActionInvoker
    {
        void StartAsync(Action action);
        void Begin();
        void Finished(bool setReadyStatus);
        void Cancelled(bool setReadyStatus);
        void SetException(Exception exception);

        string StatusText
        {
            get;
            set;
        }
    }
}