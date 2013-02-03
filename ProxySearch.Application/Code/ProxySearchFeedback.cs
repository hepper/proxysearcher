using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProxySearch.Engine;
using System.Windows.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Code
{
    public class ProxySearchFeedback : IProxySearchFeedback
    {
        private Dictionary<string, ProxyInfo> Dictionary { get; set; }

        public ProxySearchFeedback()
        {
            Dictionary = new Dictionary<string, ProxyInfo>();
        }

        public void OnAliveProxy(ProxyInfo proxyInfo)
        {
            Context.Get<ISearchResult>();

            if (!Dictionary.ContainsKey(proxyInfo.ToString()))
            {
                Dictionary.Add(proxyInfo.ToString(), proxyInfo);

                Context.Get<ISearchResult>().Add(proxyInfo);
            }
        }

        public void OnDeadProxy(ProxyInfo proxyInfo)
        {
        }

        public void OnSearchFinished()
        {
            Context.Get<IActionInvoker>().End();
        }

        public void OnSearchCancelled()
        {
            Context.Get<IActionInvoker>().End();
        }

        public void UpdateJobCount(TaskType type, int currentCount, int totalCount)
        {
            Context.Get<IActionInvoker>().Update(totalCount);
        }
    }
}
