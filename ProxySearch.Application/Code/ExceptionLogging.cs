using System;
using System.Diagnostics;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code
{
    public class ExceptionLogging : IExceptionLogging
    {
        public void Write(Exception exception)
        {
             if (!EventLog.SourceExists(Resources.ProxySearcherVersion))
                 EventLog.CreateEventSource(Resources.ProxySearcherVersion, Resources.EventLogSource);

             EventLog.WriteEntry(Resources.ProxySearcherVersion, exception.ToString(), EventLogEntryType.Error);
        }
    }
}
