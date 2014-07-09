using System;
using ProxySearch.Console.Code.GoogleAnalytics;
using ProxySearch.Console.Code.GoogleAnalytics.Timing;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IGA
    {
        void TrackPageViewAsync(string pageName);
        void TrackEventAsync(EventType eventType, string action, object label = null);
        void TrackException(Exception exception);
        void TrackException(string exceptionText);
        void StartTrackTiming(TimingCategory category, TimingVariable variable);
        void EndTrackTiming(TimingCategory category, TimingVariable variable, string label = null);
    }
}
