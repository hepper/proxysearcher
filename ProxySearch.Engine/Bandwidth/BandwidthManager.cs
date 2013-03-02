using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Bandwidth
{
    public class BandwidthManager
    {
        public async void Measure(ProxyInfo proxyInfo, CancellationTokenSource cancellationToken)
        {
            BandwidthState previousState = proxyInfo.BandwidthData.State;
            proxyInfo.BandwidthData.Progress = 0;
            proxyInfo.BandwidthData.State = BandwidthState.Progress;

            try
            {
                BanwidthInfo result = await GetBandwidthInfo(proxyInfo, cancellationToken);

                if (result != null)
                {
                    double speed = GetSpeed(result);

                    double respondTime = (result.FirstTime - result.BeginTime).TotalSeconds - result.FirstCount / speed;

                    proxyInfo.BandwidthData.State = BandwidthState.Completed;
                    proxyInfo.BandwidthData.ResponseTime = respondTime < 0 ? 0 : respondTime;
                    proxyInfo.BandwidthData.Bandwidth = 8 * speed / (1024 * 1024);
                }
                else
                {
                    proxyInfo.BandwidthData.State = BandwidthState.Error;
                }
            }
            catch (TaskCanceledException)
            {
                proxyInfo.BandwidthData.State = previousState;
            }
            catch (Exception)
            {
                proxyInfo.BandwidthData.State = BandwidthState.Error;
            }
        }

        private static double GetSpeed(BanwidthInfo result)
        {
            if (result.EndCount != result.FirstCount)
            {
                return (double)(result.EndCount - result.FirstCount) / (result.EndTime - result.FirstTime).TotalSeconds;
            }

            if (result.EndTime != result.BeginTime)
            {
                return (double)result.EndCount / (result.EndTime - result.BeginTime).TotalSeconds;
            }

            return int.MaxValue;
        }

        private async Task<BanwidthInfo> GetBandwidthInfo(ProxyInfo proxyInfo, CancellationTokenSource cancellationToken)
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
