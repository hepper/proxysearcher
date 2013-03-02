using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;

namespace ProxySearch.Engine.Checkers
{
    public class CheckerUtils
    {
        public async Task<string> GetContent(string url, ProxyInfo proxyInfo)
        {
            IWebProxy proxy = proxyInfo == null ? null : new WebProxy(proxyInfo.Address.ToString(), proxyInfo.Port);

            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    if (proxy != null)
                    {
                        handler.Proxy = proxy;
                    }

                    using (HttpClient client = new HttpClient(handler))
                    using (HttpResponseMessage response = await client.GetAsync(url, Context.Get<CancellationTokenSource>().Token))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            return null;
                        }

                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
