using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Socks.Mentalis;
using ProxySearch.Engine.Utils;

namespace ProxySearch.Engine.Socks
{
    public class SocksHttpManager
    {
        private static readonly HttpStatusCode[] redirectCodes = new HttpStatusCode[]
        {
            HttpStatusCode.Ambiguous,
            HttpStatusCode.Moved,
            HttpStatusCode.Redirect,
            HttpStatusCode.RedirectMethod,
            HttpStatusCode.RedirectKeepVerb
        };

        public Task<HttpResponseMessage> GetResponse(HttpRequestMessage request, CancellationToken cancellationToken, HttpClientHandler handler,
                                               Action<int, long?> reportRequestProgress = null, Action<int, long?> reportResponseProgress = null)
        {
            if (handler.Proxy == null)
            {
                throw new ArgumentException("HttpClientHandler should have proxy");
            }

            return Task.Run(() =>
            {
                HttpResponseMessage response = GetResponseInternal(request, handler.Proxy.GetProxy(request.RequestUri), reportRequestProgress, reportResponseProgress);
                int redirectsCount = 0;

                while (handler.AllowAutoRedirect && redirectCodes.Contains(response.StatusCode))
                {
                    if (redirectsCount > handler.MaxAutomaticRedirections)
                    {
                        throw new InvalidOperationException(Resources.TooManyRedirectsWasRequestedByServer);
                    }

                    response = GetResponseInternal(request, handler.Proxy.GetProxy(request.RequestUri), reportRequestProgress, reportResponseProgress);
                    redirectsCount++;
                }

                return response;
            });
        }

        private HttpResponseMessage GetResponseInternal(HttpRequestMessage request, Uri proxy, Action<int, long?> reportRequestProgress, Action<int, long?> reportResponseProgress)
        {
            var responseBuilder = new StringBuilder();

            using (ProxySocket socksSocket = new ProxySocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socksSocket.ProxyType = ProxyTypes.Socks5;
                socksSocket.ProxyEndPoint = new EndPointUtils().UriToIPEndPoint(proxy);

                FireEventProgress(reportRequestProgress, 0, null);
                socksSocket.Connect(new EndPointUtils().UriToIPEndPoint(request.RequestUri));

                byte[] requestBytes = Encoding.UTF8.GetBytes(BuildHttpRequestMessage(request));
                socksSocket.Send(requestBytes);
                FireEventProgress(reportRequestProgress, requestBytes.Length, requestBytes.Length);

                long? total = null;
                var buffer = new byte[1024];
                var bytesReceived = socksSocket.Receive(buffer);

                while (bytesReceived > 0)
                {
                    responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesReceived));

                    if (reportResponseProgress != null && !total.HasValue)
                    {
                        total = GetContentLength(responseBuilder.ToString());
                    }

                    if (total.HasValue)
                    {
                        FireEventProgress(reportResponseProgress, responseBuilder.Length, total);
                    }

                    bytesReceived = socksSocket.Receive(buffer);
                }

                FireEventProgress(reportResponseProgress, responseBuilder.Length, responseBuilder.Length);
            }

            return BuildHttpResponseMessage(responseBuilder.ToString());
        }

        private long? GetContentLength(string partialContent)
        {
            try
            {
                HttpResponseMessage response = BuildHttpResponseMessage(partialContent);

                if (!response.Content.Headers.Contains("Content-Length"))
                    return null;

                List<string> values = response.Content.Headers.GetValues("Content-Length").ToList();

                if (values.Count != 1)
                {
                    return null;
                }

                long result = 0;

                if (!long.TryParse(values[0], out result))
                    return null;

                return result;

            }
            catch
            {
                return null;
            }

            //Regex regex = new Regex(@"Content-Length:\s*(?<Length>\d*)\s*");

            //MatchCollection matches = regex.Matches(partialContent);

            //if (matches.Count != 1)
            //    return null;
        }

        private HttpResponseMessage BuildHttpResponseMessage(string response)
        {
            string[] lines = response.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            string content = string.Join(Environment.NewLine, lines.SkipWhile(item => item != "").Skip(1));
            HttpResponseMessage result = new HttpResponseMessage
            {
                StatusCode = GetStatusCode(lines[0]),
                Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(content)))
            };

            foreach (string header in lines.Skip(1).TakeWhile(item => item != ""))
            {
                string[] headerEntry = header.Split(new[] { ':' });
                string value = string.Join(":", headerEntry.Skip(1).ToArray()).Trim();

                try
                {
                    result.Headers.Add(headerEntry[0], value);
                }
                catch(InvalidOperationException)
                {
                    result.Content.Headers.Add(headerEntry[0], value);
                }
            }

            return result;
        }

        private HttpStatusCode GetStatusCode(string firstLine)
        {
            string[] words = firstLine.Split(' ');

            if (words.Length != 3)
            {
                throw new ArgumentException("Invalid http response from socks proxy");
            }

            return (HttpStatusCode)int.Parse(words[1]);
        }

        private string BuildHttpRequestMessage(HttpRequestMessage request)
        {
            var message = new StringBuilder();

            message.AppendFormat("{0} {1} HTTP/{2}", request.Method, request.RequestUri.PathAndQuery, request.Version).AppendLine();
            message.AppendFormat("Host: {0}", request.RequestUri.Host).AppendLine();

            foreach (var header in request.Headers)
            {
                message.AppendFormat("{0}: {1}", header.Key, header.Value).AppendLine();
            }

            message.AppendLine();
            return message.ToString();
        }

        private void FireEventProgress(Action<int, long?> eventHandler, int transfer, long? total)
        {
            if (eventHandler != null)
            {
                eventHandler(transfer, total);
            }
        }
    }
}
