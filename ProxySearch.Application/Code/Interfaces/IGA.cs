using System;
using ProxySearch.Console.Code.GoogleAnalytics;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IGA
    {
        void TrackPageViewAsync(string pageName);
        void TrackEventAsync(EventType eventType, string action, object label = null);
        void TrackException(Exception exception);
    }
}
