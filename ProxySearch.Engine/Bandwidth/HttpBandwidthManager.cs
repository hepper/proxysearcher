using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Bandwidth
{
    public class HttpBandwidthManager : BandwidthManagerBase
    {
        protected override async Task<BanwidthInfo> GetBandwidthInfo(ProxyInfo proxyInfo, CancellationTokenSource cancellationToken)
        {
            BanwidthInfo result = new BanwidthInfo();
            bool firstResponseTime = true;

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.Proxy = new WebProxy(proxyInfo.Address.ToString(), proxyInfo.Port);
                using (ProgressMessageHandler progressMessageHandler = new ProgressMessageHandler(handler))
                {
                    progressMessageHandler.HttpReceiveProgress += (sender, e) =>
                    {
                        if (firstResponseTime)
                        {
                            firstResponseTime = false;
                            result.FirstTime = DateTime.Now;
                            result.FirstCount = e.BytesTransferred;
                        }

                        proxyInfo.BandwidthData.Progress = (int)((100 * e.BytesTransferred) / e.TotalBytes.Value);
                    };

                    result.BeginTime = DateTime.Now;

                    using (HttpClient client = new HttpClient(progressMessageHandler))
                    using (HttpResponseMessage response = await client.GetAsync(Resources.SpeedTestUrl, cancellationToken.Token))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            return null;
                        }

                        result.EndTime = DateTime.Now;
                        result.EndCount = response.Content.Headers.ContentLength.Value;
                    }
                }
            }

            return result;
        }
    }
}
