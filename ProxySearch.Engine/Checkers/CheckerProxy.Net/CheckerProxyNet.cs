using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ProxySearch.Common;

namespace ProxySearch.Engine.Checkers.CheckerProxy.Net
{
    public class CheckerProxyNet : CheckerProxyBase
    {
        private int Timeout
        {
            get;
            set;
        }

        public CheckerProxyNet(int timeout)
        {
            Timeout = timeout;
        }

        protected override async Task<bool> Alive(ProxyInfo info)
        {
            string proxyType = GetProxyType(await GetInfo(info));

            if (proxyType != null)
            {
                info.Details = new HttpProxyInfo()
                {
                    Type = proxyType
                };
            }

            return info.Details != null;
        }

        private async Task<CheckerProxyNet_ProxyInfo> GetInfo(ProxyInfo proxy)
        {
            try
            {
                HttpContent content = new StringContent(HttpUtility.UrlDecode(BuildData(new ProxyInfo[] { proxy })));
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
                        return ((CheckerProxyNet_ProxiesInfo)serializer.ReadObject(stream)).proxy[0];
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private string BuildData(ProxyInfo[] Ips)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < Ips.Length; i++)
            {
                builder.AppendFormat("proxy[]={0}:{1}:{2}&", i, Ips[i].Address, Ips[i].Port);
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
