using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Xml;
using ProxySearch.Common;
using System.Threading.Tasks;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Engine
{
    public class ProxySearcher : IProxySearcher
    {
        IProxySearchFeedback feedback;
        IProxyChecker checker;
        IGeoIP geoIP;

        public ProxySearcher(IProxySearchFeedback feedback, IProxyChecker checker, IGeoIP geoIP)
        {
            this.feedback = feedback;
            this.checker = checker;
            this.geoIP = geoIP ?? new TurnOffGeoIP();
        }

        public void BeginSearch(string document)
        {
            string pattern = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?):[0-9]+";
            Regex regEx = new Regex(pattern);

            foreach (Match match in regEx.Matches(document))
            {
                if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                {
                    return;
                }

                Task task = Task.Run(async () =>
                {
                    if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                    {
                        return;
                    }

                    try
                    {
                        using (Context.Get<TaskCounter>().Listen(TaskType.Search))
                        {
                            ProxyInfo info = await GetProxyInfo(match);

                            if (info != null)
                            {
                                if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                                {
                                    return;
                                }

                                if ((await checker.Alive(info)))
                                    feedback.OnAliveProxy(info);
                                else
                                    feedback.OnDeadProxy(info);
                            }
                        }
                    }
                    catch
                    {
                    }
                });
            }
        }

        private async Task<ProxyInfo> GetProxyInfo(Match match)
        {
            string[] data = match.Value.Split(':');

            short port;
            if (!short.TryParse(data[1], out port))
                return null;

            return new ProxyInfo(IPAddress.Parse(data[0]), port, await geoIP.GetLocation(data[0]));
        }
    }
}
