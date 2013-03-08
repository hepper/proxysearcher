using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Engine.Checkers
{
    public class CheckerUtils
    {
        public async Task<string> GetContent(string url, ProxyInfo proxyInfo)
        {
            IWebProxy proxy = proxyInfo == null ? null : new WebProxy(proxyInfo.Address.ToString(), proxyInfo.Port);

            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    if (proxy != null)
                    {
                        handler.Proxy = proxy;
                    }

                    BanwidthInfo info = new BanwidthInfo()
                    {
                        BeginTime = DateTime.Now
                    };

                    using (HttpClient client = new HttpClient(handler))
                    using (HttpResponseMessage response = await client.GetAsync(url, Context.Get<CancellationTokenSource>().Token))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            return null;
                        }

                        info.FirstTime = DateTime.Now;

                        string content = await response.Content.ReadAsStringAsync();

                        info.FirstCount = content.Length * 2;
                        info.EndTime = info.FirstTime;
                        info.EndCount = info.FirstCount;

                        if (proxyInfo != null)
                            Context.Get<BandwidthManager>().UpdateBandwidthData(proxyInfo, info);

                        return content;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
