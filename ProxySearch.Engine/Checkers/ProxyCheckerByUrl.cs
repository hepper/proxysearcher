using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.Error;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.ProxyDetailsProvider;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine.Checkers
{
    public class ProxyCheckerByUrl<ProxyDetailsProviderType> : ProxyCheckerBase<ProxyDetailsProviderType>
                                                               where ProxyDetailsProviderType : IProxyDetailsProvider, new()
    {
        private string Url
        {
            get;
            set;
        }

        private double Accuracy
        {
            get;
            set;
        }

        private Dictionary<char, int> analyzedText = null;
        private Task initializatinTask = null;

        public ProxyCheckerByUrl(string url, double accuracy, int maxTasksCount)
            : base(maxTasksCount)
        {
            Url = url;
            Accuracy = accuracy;
        }

        public override void InitializeAsync(CancellationTokenSource cancellationTokenSource,
                                             ITaskManager taskManager,
                                             IHttpDownloaderContainer httpDownloaderContainer,
                                             IErrorFeedback errorFeedback,
                                             IProxySearchFeedback proxySearchFeedback,
                                             IGeoIP geoIP)
        {
            base.InitializeAsync(cancellationTokenSource, taskManager, httpDownloaderContainer, errorFeedback, proxySearchFeedback, geoIP);

            TaskItem taskItem = taskManager.Create(Resources.ConfiguringProxyChecker);

            try
            {
                taskItem.UpdateDetails(string.Format(Resources.DownloadingFormat, Url));

                initializatinTask = httpDownloaderContainer.HttpDownloader.GetContentOrNull(Url, null, cancellationTokenSource)
                                                           .ContinueWith(task =>
                                                           {
                                                               try
                                                               {
                                                                   if (task.Result == null)
                                                                   {
                                                                       errorFeedback.SetException(new InvalidOperationException(string.Format(Resources.CannotDownloadContent, Url)));
                                                                       cancellationTokenSource.Cancel();
                                                                   }
                                                                   else
                                                                   {
                                                                       analyzedText = AnalyzeText(task.Result);
                                                                   }
                                                               }
                                                               finally
                                                               {
                                                                   taskItem.Dispose();
                                                               }
                                                           });
            }
            catch (TaskCanceledException)
            {
            }
        }

        protected override async Task<bool> Alive(Proxy proxy, TaskItem task, Action begin, Action<int> firstTime, Action<int> end, CancellationTokenSource cancellationToken)
        {
            try
            {
                task.UpdateDetails(string.Format(Resources.ProxyDownloadingFormat, proxy, Url));

                string content = await HttpDownloaderContainer.HttpDownloader.GetContentOrNull(Url, proxy, cancellationToken, begin, firstTime, end);

                if (content == null)
                {
                    return false;
                }

                if (!initializatinTask.IsCanceled && !initializatinTask.IsCompleted)
                {
                    task.UpdateDetails(string.Format(Resources.WaitUntilProxyCheckerIsConfiguredFormat, proxy), Tasks.TaskStatus.Slow);
                    await initializatinTask;
                }

                return Compare(analyzedText, AnalyzeText(content)) <= Accuracy;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private Dictionary<char, int> AnalyzeText(string text)
        {
            Dictionary<char, int> result = new Dictionary<char, int>();

            foreach (char symbol in text)
            {
                if (result.ContainsKey(symbol))
                {
                    result[symbol]++;
                }
                else
                {
                    result.Add(symbol, 1);
                }
            }

            return result;
        }

        private double Compare(Dictionary<char, int> dictionary1, Dictionary<char, int> dictionary2)
        {
            int result = 0;

            foreach (char key in dictionary1.Keys.Union(dictionary2.Keys).Distinct().ToList())
            {
                int count1;
                int count2;

                bool get1 = dictionary1.TryGetValue(key, out count1);
                bool get2 = dictionary2.TryGetValue(key, out count2);

                if (get1 && get2)
                {
                    result += Math.Abs(count1 - count2);
                }
                else if (get1)
                {
                    result += count1;
                }
                else if (get2)
                {
                    result += count2;
                }
            }

            return (double)result / dictionary1.Sum(item => item.Value);
        }
    }
}