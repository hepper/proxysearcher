using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Bandwidth
{
    public interface IBandwidthManager
    {
        void MeasureAsync(ProxyInfo proxyInfo);
        void Cancel(ProxyInfo proxyInfo);
        void UpdateBandwidthData(ProxyInfo proxyInfo, BanwidthInfo info);
    }
}