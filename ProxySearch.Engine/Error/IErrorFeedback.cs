using System;

namespace ProxySearch.Engine.Error
{
    public interface IErrorFeedback
    {
        void SetException(Exception exception);
    }
}