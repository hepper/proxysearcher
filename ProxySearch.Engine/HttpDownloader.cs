using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine
{
    public class HttpDownloader
    {
        public Task<string> GetContentOrNull(string url, Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return GetContentOrNull(url, proxy, cancellationToken, () => { }, () => { }, length => { });
        }

        public async Task<string> GetContentOrNull(string url, Proxy proxy, CancellationTokenSource cancellationToken, Action begin, Action firstTime, Action<int> end)
        {
            IWebProxy webProxy = proxy == null ? null : new WebProxy(proxy.Address.ToString(), proxy.Port);

            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    if (webProxy != null)
                    {
                        handler.Proxy = webProxy;
                    }

                    begin();

                    using (HttpClient client = new HttpClient(handler))
                    using (HttpResponseMessage response = await client.GetAsync(url, cancellationToken.Token))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            return null;
                        }

                        firstTime();
                        
                        string content = await response.Content.ReadAsStringAsync();
                        end(content.Length);

                        return content;
                    }
                }
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch
            {
                return null;
            }
        }
    }
}
