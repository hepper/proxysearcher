using System;

namespace ProxySearch.Common
{
    public interface IExceptionLogging
    {
        void Write(Exception exception);
    }
}
