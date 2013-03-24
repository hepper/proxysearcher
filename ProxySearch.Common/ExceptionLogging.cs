using System;
using System.Diagnostics;
using ProxySearch.Common.Properties;

namespace ProxySearch.Common
{
    public class ExceptionLogging : IExceptionLogging
    {
        public void Write(Exception exception)
        {
            try
            {
                if (!EventLog.SourceExists(Resources.ProxySearcherSource))
                    EventLog.CreateEventSource(Resources.ProxySearcherSource, Resources.EventLogSource);

                EventLog.WriteEntry(Resources.ProxySearcherSource, exception.ToString(), EventLogEntryType.Error);
            }
            catch
            {
            }
        }
    }
}
