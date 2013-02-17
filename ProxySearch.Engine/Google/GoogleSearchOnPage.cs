using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Http;
using ProxySearch.Common;
using System.Threading;
using ProxySearch.Engine.Properties;
using System.Threading.Tasks;

namespace ProxySearch.Engine.Google
{
    public class GoogleSearchOnPage
    {
        private static readonly string urlPrefix = "/url?q=";
        private string pageContent;
        private List<Uri> urls = new List<Uri>();

        public async Task Initialize(Uri googlePage)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(googlePage, Context.Get<CancellationTokenSource>().Token))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                pageContent = await response.Content.ReadAsStringAsync();
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

                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    continue;

                Uri uri = new Uri(url);

                if (!InException(uri))
                    urls.Add(uri);
            }
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
