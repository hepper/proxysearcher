﻿using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ProxySearch.Common;
using System.Linq;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Engine.Checkers.CheckerProxy.Net
{
    public class CheckerProxyNet : IProxyChecker
    {
        private int Timeout
        {
            get;
            set;
        }

        private int BatchSize
        {
            get;
            set;
        }

        public CheckerProxyNet(int timeout, int batchSize)
        {
            Timeout = timeout;
            BatchSize = batchSize;
        }

        public async void CheckAsync(List<ProxyInfo> proxies, IProxySearchFeedback feedback, IGeoIP geoIP)
        {
            for (int i = 0; true; i++)
            {
                List<ProxyInfo> batch = proxies.Skip(i * BatchSize).Take(BatchSize).ToList();

                if (!batch.Any())
                    return;

                using (Context.Get<TaskCounter>().Listen(TaskType.Search, batch.Count))
                {
                    await CheckBatchAsync(batch, feedback);
                }
            }
        }

        private async Task CheckBatchAsync(List<ProxyInfo> proxies, IProxySearchFeedback feedback)
        {
            CheckerProxyNet_ProxiesInfo result = await GetInfoOrNull(proxies);

            if (result == null)
                return;

            foreach (CheckerProxyNet_ProxyInfo info in result.proxy)
            {
                if (info.result == 0)
                    continue;

                ProxyInfo proxy = proxies.Single(item => item.AddressPort == info.ipport);

                string proxyType = GetProxyType(info);

                if (proxyType != null)
                {
                    proxy.Details = new HttpProxyInfo()
                    {
                        Type = proxyType
                    };
                }

                proxy.CountryInfo = new CountryInfo
                {
                    Code = info.country_id.ToString(),
                    Name = info.country
                };

                feedback.OnAliveProxy(proxy);
            }
        }

        private async Task<CheckerProxyNet_ProxiesInfo> GetInfoOrNull(List<ProxyInfo> proxies)
        {
            try
            {
                HttpContent content = new StringContent(HttpUtility.UrlDecode(BuildData(proxies.ToArray())));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.PostAsync("http://checkerproxy.net/checker2.php", content, Context.Get<CancellationTokenSource>().Token))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    string result = await response.Content.ReadAsStringAsync();

                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CheckerProxyNet_ProxiesInfo));

                    using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(result)))
                    {
                        return (CheckerProxyNet_ProxiesInfo)serializer.ReadObject(stream);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private string BuildData(ProxyInfo[] proxies)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < proxies.Length; i++)
            {
                builder.AppendFormat("proxy[]={0}:{1}&", i, proxies[i].AddressPort);
            }

            builder.AppendFormat("timeout={0}&step={1}&lang=en&proxy_type={2}&publish_status={3}", Timeout, 25, 0, false);

            return builder.ToString();
        }

        private string GetProxyType(CheckerProxyNet_ProxyInfo info)
        {
            if (info == null)
            {
                return null;
            }

            switch (info.type_2)
            {
                case "<font color=orange><b>Anonymous proxy</b></font>":
                    return HttpProxyInfo.HttpProxyTypes.Anonymous;
                case "<font color=red><b>Transparent proxy</b></font>":
                    return HttpProxyInfo.HttpProxyTypes.Transparent;
                case "<font color=green><b>High anonymous / Elite proxy</b></font>":
                    return HttpProxyInfo.HttpProxyTypes.HighAnonymous;
                default:
                    return null;
            }
        }
    }
}