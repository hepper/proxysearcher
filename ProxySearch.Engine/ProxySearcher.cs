using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Engine
{
    public class ProxySearcher : IProxySearcher
    {
        IProxySearchFeedback feedback;
        IProxyChecker checker;
        IGeoIP geoIP;

        Hashtable foundIps = new Hashtable();

        public ProxySearcher(IProxySearchFeedback feedback, IProxyChecker checker, IGeoIP geoIP)
        {
            this.feedback = feedback;
            this.checker = checker;
            this.geoIP = geoIP ?? new TurnOffGeoIP();
        }

        public async void BeginSearch(string document)
        {
            await Task.Run(() =>
            {
                string pattern = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?):[0-9]+";
                Regex regEx = new Regex(pattern);

                foreach (Match match in regEx.Matches(document))
                {
                    if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                    {
                        return;
                    }

                    Task.Run(async () =>
                    {
                        if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                        {
                            return;
                        }

                        try
                        {
                            using (Context.Get<TaskCounter>().Listen(TaskType.Search))
                            {
                                ProxyInfo info = GetProxyInfo(match);

                                if (info != null && !foundIps.ContainsKey(info.GetHashCode()))
                                {
                                    foundIps.Add(info.GetHashCode(), info);

                                    info.CountryInfo = await geoIP.GetLocation(info.Address.ToString());

                                    if ((await checker.Alive(info)))
                                        feedback.OnAliveProxy(info);
                                }
                            }
                        }
                        catch
                        {
                        }
                    });
                }
            });
        }

        private ProxyInfo GetProxyInfo(Match match)
        {
            string[] data = match.Value.Split(':');

            ushort port;
            if (!ushort.TryParse(data[1], out port))
                return null;

            return new ProxyInfo(IPAddress.Parse(data[0]), port);
        }
    }
}
