using System;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IExceptionLogging
    {
        void Write(Exception exception);
    }
}
