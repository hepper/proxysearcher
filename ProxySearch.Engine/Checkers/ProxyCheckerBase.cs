using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Bandwidth;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.Error;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.ProxyDetailsProvider;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine.Checkers
{
    public abstract class ProxyCheckerBase<ProxyDetailsProviderType> : IProxyChecker, IErrorFeedbackHolder
        where ProxyDetailsProviderType : IProxyDetailsProvider, new()
    {
        public IErrorFeedback ErrorFeedback
        {
            get;
            set;
        }

        protected IProxyDetailsProvider DetailsProvider
        {
            get;
            private set;
        }

        public ProxyCheckerBase()
        {
            DetailsProvider = new ProxyDetailsProviderType();
            ErrorFeedback = new DummyErrorFeedback();
        }

        public void CheckAsync(List<Proxy> proxies, IProxySearchFeedback feedback, IGeoIP geoIP)
        {
            foreach (Proxy proxy in proxies)
            {
                if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                    return;

                Proxy proxyCopy = proxy;

                Task.Run(async () =>
                {
                    if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                        return;

                    using (TaskItem task = Context.Get<TaskManager>().Create(Properties.Resources.CheckingProxyIfItWorks))
                    {
                        task.UpdateDetails(string.Format(Resources.ProxyCheckingIfAliveFormat, proxyCopy));
                        BanwidthInfo bandwidth = null;

                        if (await Alive(proxyCopy, task, () => bandwidth = new BanwidthInfo()
                        {
                            BeginTime = DateTime.Now
                        }, lenght =>
                        {
                            task.UpdateDetails(string.Format(Resources.ProxyGotFirstResponseFormat, proxyCopy), Tasks.TaskStatus.Progress);
                            bandwidth.FirstTime = DateTime.Now;
                            bandwidth.FirstCount = lenght * 2;
                        }, lenght =>
                        {
                            bandwidth.EndTime = DateTime.Now;
                            bandwidth.EndCount = lenght * 2;
                        }))
                        {
                            if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                                return;

                            task.UpdateDetails(string.Format(Resources.ProxyDeterminingProxyType, proxyCopy), Tasks.TaskStatus.GoodProgress);

                            ProxyDetails proxyDetails = new ProxyDetails(await GetProxyDetails(proxy, Context.Get<CancellationTokenSource>()), UpdateProxyDetails);

                            task.UpdateDetails(string.Format(Resources.ProxyDeterminingLocationFormat, proxyCopy), Tasks.TaskStatus.GoodProgress);

                            IPAddress proxyAddress = proxyDetails.Details.OutgoingIPAddress ?? proxyCopy.Address;

                            CountryInfo countryInfo = await geoIP.GetLocation(proxyAddress.ToString());

                            ProxyInfo proxyInfo = new ProxyInfo(proxyCopy)
                            {
                                CountryInfo = countryInfo,
                                Details = proxyDetails
                            };

                            if (bandwidth != null)
                                Context.Get<IHttpDownloaderContainer>().BandwidthManager.UpdateBandwidthData(proxyInfo, bandwidth);

                            feedback.OnAliveProxy(proxyInfo);
                        }
                    }
                });
            }
        }

        protected abstract Task<bool> Alive(Proxy proxy, TaskItem task, Action begin, Action<int> firstTime, Action<int> end);

        protected virtual Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return DetailsProvider.GetProxyDetails(proxy, cancellationToken);
        }

        protected virtual Task<ProxyTypeDetails> UpdateProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return GetProxyDetails(proxy, cancellationToken);
        }
    }
}
