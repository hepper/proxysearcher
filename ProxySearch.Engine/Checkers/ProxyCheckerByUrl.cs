﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.ProxyDetailsProvider;

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

        private Dictionary<char, int> AnalyzedText
        {
            get;
            set;
        }

        public ProxyCheckerByUrl(string url, double accuracy)
        {
            Url = url;
            Accuracy = accuracy;

            try
            {
                string content = Context.Get<IHttpDownloaderContainer>().HttpDownloader.GetContentOrNull(url, null, Context.Get<CancellationTokenSource>()).GetAwaiter().GetResult();

                if (content == null)
                {
                    throw new InvalidOperationException(string.Format(Resources.CannotDownloadContent, url));
                }

                AnalyzedText = AnalyzeText(content);
            }
            catch (TaskCanceledException)
            {
            }
        }

        protected override async Task<bool> Alive(Proxy proxy, Action begin, Action<int> firstTime, Action<int> end)
        {
            try
            {
                string content = await Context.Get<IHttpDownloaderContainer>().HttpDownloader.GetContentOrNull(Url, proxy, Context.Get<CancellationTokenSource>(), begin, firstTime, end);

                if (content == null)
                {
                    return false;
                }

                return Compare(AnalyzedText, AnalyzeText(content)) <= Accuracy;
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