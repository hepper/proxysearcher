using System;

namespace ProxySearch.Engine.Error
{
    public class DummyErrorFeedback : IErrorFeedback
    {
        public void SetException(Exception exception)
        {
        }
    }
}
