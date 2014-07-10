using System;
using System.Threading.Tasks;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IActionInvoker
    {
        void StartAsync(Action action);
        void Finished(bool setReadyStatus);
        void Cancelled(bool setReadyStatus);
        string StatusText
        {
            get;
            set;
        }
    }
}