using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Socks;
using ProxySearch.Engine.Socks.Mentalis;

namespace ProxySearch.Engine.Bandwidth
{
    public class SocksBandwidthManager : BandwidthManagerBase
    {
        protected override async Task<BanwidthInfo> GetBandwidthInfo(ProxyInfo proxyInfo, CancellationTokenSource cancellationToken)
        {
            using (SocksWebClient client = new SocksWebClient(proxyInfo.Address, proxyInfo.Port, ProxyTypes.Socks5, cancellationToken))
            {
                BanwidthInfo result = new BanwidthInfo();
                bool firstResponseTime = true;

                client.DownloadProgressChanged += (sender, e) =>
                {
                    if (firstResponseTime)
                    {
                        firstResponseTime = false;
                        result.FirstTime = DateTime.Now;
                        result.FirstCount = e.BytesReceived;
                    }

                    proxyInfo.BandwidthData.Progress = e.ProgressPercentage;
                };

                result.BeginTime = DateTime.Now;
                result.FirstTime = result.BeginTime;
                result.FirstCount = 0;

                string data = await client.DownloadStringTaskAsync(new Uri(Resources.SpeedTestUrl));

                result.EndTime = DateTime.Now;
                result.EndCount = data.Length;

                return result;
            }
        }
    }
}
