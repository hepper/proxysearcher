using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySearch.Engine.SearchEngines.Google
{
    public class GoogleSearchOnPage
    {
        private static readonly string urlPrefix = "/url?q=";
        private static readonly string urlSuffix = "&amp;sa=";
        private string pageContent;
        private List<Uri> urls = new List<Uri>();

        public async Task Initialize(Uri googlePage, ICaptchaWindow captchaWindow, int pageNumber, CancellationTokenSource cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(googlePage, cancellationToken.Token))
            {
                pageContent = await GetContent(response, captchaWindow, pageNumber, cancellationToken);
            }

            Regex regex = new Regex("<a[^>]*?href\\s*=\\s*(?<url>[\"']?([^\"'>]+?)['\"])?[^>]*?>");

            foreach (Match match in regex.Matches(pageContent))
            {
                string url = match.Groups["url"].Value;
                url = url.Remove(0, 1);
                url = url.Remove(url.Length - 1, 1);

                if (!url.StartsWith(urlPrefix))
                    continue;

                url = url.Substring(urlPrefix.Length);

                int index = url.IndexOf(urlSuffix);

                if (index != -1)
                    url = url.Substring(0, index);

                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    continue;

                Uri uri = new Uri(url);

                if (!InException(uri))
                {                    
                    urls.Add(uri);
                }
            }
        }

        private async Task<string> GetContent(HttpResponseMessage response, ICaptchaWindow captchaWindow, int pageNumber, CancellationTokenSource cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode != HttpStatusCode.ServiceUnavailable)
                {
                    throw new InvalidOperationException(string.Format("Cannot continue search because engine retrieve error '{0}'", response.StatusCode.ToString()));
                }

                return await captchaWindow.GetSolvedContentAsync(response.RequestMessage.RequestUri.ToString(), pageNumber, cancellationToken.Token);
            }

            return await response.Content.ReadAsStringAsync();
        }

        private bool InException(Uri url)
        {
            return url.ToString().Contains("google");
        }

        public Uri GetNext()
        {
            if (urls.Count == 0)
                return null;

            Uri res = urls[0];

            urls.RemoveAt(0);

            return res;
        }
    }
}
