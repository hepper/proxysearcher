using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
using ProxySearch.Engine.Ratings;
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

        protected CancellationTokenSource CancellationTokenSource
        {
            get;
            set;
        }

        protected IProxySearchFeedback ProxySearchFeedback
        {
            get;
            set;
        }

        protected int MaxTasksCount
        {
            get;
            set;
        }

        protected IGeoIP GeoIP
        {
            get;
            set;
        }

        protected IRatingManager RatingManager
        {
            get;
            set;
        }

        private ObservableList<Proxy> ProxyQueue
        {
            get;
            set;
        }

        private Task CheckProxiesTask
        {
            get;
            set;
        }

        private TaskItem InitializationTask
        {
            get;
            set;
        }

        public ProxyCheckerBase(int maxTasksCount)
        {
            ProxyQueue = new ObservableList<Proxy>();
            DetailsProvider = new ProxyDetailsProviderType();
            MaxTasksCount = maxTasksCount;
        }

        public virtual void InitializeAsync(CancellationTokenSource cancellationTokenSource,
                                            ITaskManager taskManager,
                                            IHttpDownloaderContainer httpDownloaderContainer,
                                            IErrorFeedback errorFeedback,
                                            IProxySearchFeedback proxySearchFeedback,
                                            IGeoIP geoIP,
                                            IRatingManager ratingManager)
        {
            CancellationTokenSource = cancellationTokenSource;
            TaskManager = taskManager;
            HttpDownloaderContainer = httpDownloaderContainer;
            ErrorFeedback = errorFeedback;
            ProxySearchFeedback = proxySearchFeedback;
            GeoIP = geoIP;
            RatingManager = ratingManager;

            IAsyncInitialization asyncInitialization = DetailsProvider as IAsyncInitialization;

            if (asyncInitialization != null)
                asyncInitialization.InitializeAsync(cancellationTokenSource, taskManager, httpDownloaderContainer, errorFeedback, proxySearchFeedback, geoIP, ratingManager);

            TaskManager.Tasks.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                    StartCheckProxiesTaskIfRequired();
            };
        }

        public void CheckAsync(List<Proxy> proxies)
        {
            if (CancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            ProxyQueue.AddRange(proxies);
            StartCheckProxiesTaskIfRequired();
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

        private async void StartCheckProxiesTaskIfRequired()
        {
            if (ProxyQueue.Count == 0 || CheckProxiesTask != null)
                return;

            if (InitializationTask == null)
            {
                SetInitializationTaskIfRequired();
            }

            try
            {
                CheckProxiesTask = Task.Run(() =>
                {
                    while (CheckingProxyIfItWorksTasksCount < MaxTasksCount)
                    {
                        if (CancellationTokenSource.IsCancellationRequested)
                            break;

                        Proxy proxy = ProxyQueue.FirstOrDefault();

                        if (proxy == null)
                            break;

                        ProxyQueue.RemoveAt(0);
                        CheckProxyAsync(proxy);
                    }
                });

                await CheckProxiesTask;
            }
            finally
            {
                CheckProxiesTask = null;
                DisposeInitializationTaskIfRequired();
            }
        }

        private int CheckingProxyIfItWorksTasksCount
        {
            get
            {
                return TaskManager.Tasks.Count(taskItem => taskItem.Name == Properties.Resources.CheckingProxyIfItWorks);
            }
        }

        private async void CheckProxyAsync(Proxy proxy)
        {
            using (TaskItem task = TaskManager.Create(Properties.Resources.CheckingProxyIfItWorks))
            {
                DisposeInitializationTaskIfRequired();

                await Task.Run(async () =>
                {
                    if (CancellationTokenSource.IsCancellationRequested)
                    {
                        return;
                    }

                    task.UpdateDetails(string.Format(Resources.ProxyCheckingIfAliveFormat, proxy));
                    BanwidthInfo bandwidth = null;

                    if (await Alive(proxy, task, () => bandwidth = new BanwidthInfo()
                    {
                        BeginTime = DateTime.Now
                    }, lenght =>
                    {
                        task.UpdateDetails(string.Format(Resources.ProxyGotFirstResponseFormat, proxy.AddressPort), Tasks.TaskStatus.Progress);
                        bandwidth.FirstTime = DateTime.Now;
                        bandwidth.FirstCount = lenght * 2;
                    }, lenght =>
                    {
                        bandwidth.EndTime = DateTime.Now;
                        bandwidth.EndCount = lenght * 2;
                    }, CancellationTokenSource))
                    {
                        if (CancellationTokenSource.IsCancellationRequested)
                            return;

                        task.UpdateDetails(string.Format(Resources.ProxyDeterminingProxyType, proxy.AddressPort), Tasks.TaskStatus.GoodProgress);

                        ProxyDetails proxyDetails = new ProxyDetails(await GetProxyDetails(proxy, task, CancellationTokenSource), UpdateProxyDetails);

                        task.UpdateDetails(string.Format(Resources.ProxyDeterminingLocationFormat, proxy.AddressPort), Tasks.TaskStatus.GoodProgress);

                        IPAddress proxyAddress = proxyDetails.Details.OutgoingIPAddress ?? proxy.Address;

                        CountryInfo countryInfo = await GeoIP.GetLocation(proxyAddress.ToString());

                        ProxyInfo proxyInfo = new ProxyInfo(proxy)
                        {
                            CountryInfo = countryInfo,
                            Details = proxyDetails
                        };

                        if (bandwidth != null)
                            HttpDownloaderContainer.BandwidthManager.UpdateBandwidthData(proxyInfo, bandwidth);

                        proxyInfo.RatingData = await RatingManager.GetRatingData(proxyInfo);

                        ProxySearchFeedback.OnAliveProxy(proxyInfo);
                    }
                });
            }
        }

        private void SetInitializationTaskIfRequired()
        {
            lock (TaskManager)
            {
                if (InitializationTask == null)
                {
                    InitializationTask = TaskManager.Create("");
                }
            }
        }

        private void DisposeInitializationTaskIfRequired()
        {
            lock (TaskManager)
            {
                if (InitializationTask != null)
                {
                    InitializationTask.Dispose();
                    InitializationTask = null;
                }
            }
        }
    }
}