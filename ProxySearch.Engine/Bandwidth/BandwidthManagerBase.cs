using System;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Bandwidth
{
    public abstract class BandwidthManagerBase : IBandwidthManager
    {
        public async void MeasureAsync(ProxyInfo proxyInfo)
        {
            BandwidthState previousState = proxyInfo.BandwidthData.State;
            proxyInfo.BandwidthData.Progress = 0;
            proxyInfo.BandwidthData.State = BandwidthState.Progress;
            proxyInfo.BandwidthData.CancellationToken = new CancellationTokenSource();

            try
            {
                BanwidthInfo result = await GetBandwidthInfo(proxyInfo, proxyInfo.BandwidthData.CancellationToken);

                if (result != null)
                {
                    UpdateBandwidthData(proxyInfo, result);
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

        public void Cancel(ProxyInfo proxyInfo)
        {
            proxyInfo.BandwidthData.CancellationToken.Cancel();
            proxyInfo.BandwidthData.CancellationToken = null;
        }

        public void UpdateBandwidthData(ProxyInfo proxyInfo, BanwidthInfo info)
        {
            double speed = GetSpeed(info);
            double? respondTime = (info.FirstTime != info.EndTime) ? (double?)(info.FirstTime - info.BeginTime).TotalSeconds - info.FirstCount / speed : null;

            proxyInfo.BandwidthData.State = BandwidthState.Completed;
            proxyInfo.BandwidthData.ResponseTime = respondTime < 0 ? 0 : respondTime;
            proxyInfo.BandwidthData.Bandwidth = 8 * speed / (1024 * 1024);
        }

        protected abstract Task<BanwidthInfo> GetBandwidthInfo(ProxyInfo proxyInfo, CancellationTokenSource cancellationToken);

        private double GetSpeed(BanwidthInfo result)
        {
            if (result.EndTime != result.FirstTime)
            {
                return (double)(result.EndCount - result.FirstCount) / (result.EndTime - result.FirstTime).TotalSeconds;
            }

            if (result.EndTime != result.BeginTime)
            {
                return (double)result.EndCount / (result.EndTime - result.BeginTime).TotalSeconds;
            }

            return int.MaxValue;
        }
    }
}
