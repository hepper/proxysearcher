using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
    public abstract class ProxyCheckerBase<ProxyDetailsProviderType> : IProxyChecker, IAsyncInitialization
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

        protected ITaskManager TaskManager
        {
            get;
            private set;
        }

        protected IHttpDownloaderContainer HttpDownloaderContainer
        {
            get;
            private set;
        }

        public ProxyCheckerBase()
        {
            DetailsProvider = new ProxyDetailsProviderType();
        }

        public virtual void InitializeAsync(CancellationTokenSource cancellationTokenSource, ITaskManager taskManager, IHttpDownloaderContainer httpDownloaderContainer, IErrorFeedback errorFeedback)
        {
            TaskManager = taskManager;
            HttpDownloaderContainer = httpDownloaderContainer;
            ErrorFeedback = errorFeedback;

            IAsyncInitialization asyncInitialization = DetailsProvider as IAsyncInitialization;

            if (asyncInitialization != null)
                asyncInitialization.InitializeAsync(cancellationTokenSource, taskManager, httpDownloaderContainer, errorFeedback);
        }

        public void CheckAsync(List<Proxy> proxies, IProxySearchFeedback feedback, IGeoIP geoIP, CancellationTokenSource cancellationTokenSource)
        {
            foreach (Proxy proxy in proxies)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                    return;

                Proxy proxyCopy = proxy;

                Task.Run(async () =>
                {
                    if (cancellationTokenSource.IsCancellationRequested)
                        return;

                    using (TaskItem task = TaskManager.Create(Properties.Resources.CheckingProxyIfItWorks))
                    {
                        task.UpdateDetails(string.Format(Resources.ProxyCheckingIfAliveFormat, proxyCopy));
                        BanwidthInfo bandwidth = null;

                        if (await Alive(proxyCopy, task, () => bandwidth = new BanwidthInfo()
                        {
                            BeginTime = DateTime.Now
                        }, lenght =>
                        {
                            task.UpdateDetails(string.Format(Resources.ProxyGotFirstResponseFormat, proxyCopy.AddressPort), Tasks.TaskStatus.Progress);
                            bandwidth.FirstTime = DateTime.Now;
                            bandwidth.FirstCount = lenght * 2;
                        }, lenght =>
                        {
                            bandwidth.EndTime = DateTime.Now;
                            bandwidth.EndCount = lenght * 2;
                        }, cancellationTokenSource))
                        {
                            if (cancellationTokenSource.IsCancellationRequested)
                                return;

                            task.UpdateDetails(string.Format(Resources.ProxyDeterminingProxyType, proxyCopy.AddressPort), Tasks.TaskStatus.GoodProgress);

                            ProxyDetails proxyDetails = new ProxyDetails(await GetProxyDetails(proxyCopy, task, cancellationTokenSource), UpdateProxyDetails);

                            task.UpdateDetails(string.Format(Resources.ProxyDeterminingLocationFormat, proxyCopy.AddressPort), Tasks.TaskStatus.GoodProgress);

                            IPAddress proxyAddress = proxyDetails.Details.OutgoingIPAddress ?? proxyCopy.Address;

                            CountryInfo countryInfo = await geoIP.GetLocation(proxyAddress.ToString());

                            ProxyInfo proxyInfo = new ProxyInfo(proxyCopy)
                            {
                                CountryInfo = countryInfo,
                                Details = proxyDetails
                            };

                            if (bandwidth != null)
                                HttpDownloaderContainer.BandwidthManager.UpdateBandwidthData(proxyInfo, bandwidth);

                            feedback.OnAliveProxy(proxyInfo);
                        }
                    }
                });
            }
        }

        protected abstract Task<bool> Alive(Proxy proxy, TaskItem task, Action begin, Action<int> firstTime, Action<int> end, CancellationTokenSource cancellationToken);

        protected virtual async Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, TaskItem task, CancellationTokenSource cancellationToken)
        {
            return await DetailsProvider.GetProxyDetails(proxy, task, cancellationToken);
        }

        protected virtual Task<ProxyTypeDetails> UpdateProxyDetails(Proxy proxy, TaskItem task, CancellationTokenSource cancellationToken)
        {
            return GetProxyDetails(proxy, task, cancellationToken);
        }
    }
}
