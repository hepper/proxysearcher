using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Socks.Mentalis;
using ProxySearch.Engine.Utils;

namespace ProxySearch.Engine.Socks
{
    public class SocksHttpClientHandler : HttpClientHandler
    {
        public int MaxRedirectCount
        {
            get;
            set;
        }

        public SocksHttpClientHandler()
        {
            MaxRedirectCount = 10;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                HttpResponseMessage response = GetResponse(request);
                int redirectsCount = 0;

                while (AllowAutoRedirect && (response.StatusCode == HttpStatusCode.Ambiguous ||
                                            response.StatusCode == HttpStatusCode.Moved ||
                                            response.StatusCode == HttpStatusCode.Redirect ||
                                            response.StatusCode == HttpStatusCode.RedirectMethod ||
                                            response.StatusCode == HttpStatusCode.RedirectKeepVerb))
                {
                    ThrowIfMaximumNumbersOfRedirectsReached(redirectsCount);

                    response = GetResponse(request);
                    redirectsCount++;
                }

                return response;
            });
        }

        private HttpResponseMessage GetResponse(HttpRequestMessage request)
        {
            var responseBuilder = new StringBuilder();
            using (ProxySocket socksSocket = new ProxySocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                if (Proxy != null)
                {
                    socksSocket.ProxyType = ProxyTypes.Socks5;
                    socksSocket.ProxyEndPoint = new EndPointUtils().UriToIPEndPoint(Proxy.GetProxy(request.RequestUri));
                }

                socksSocket.Connect(new EndPointUtils().UriToIPEndPoint(request.RequestUri));
                socksSocket.Send(Encoding.UTF8.GetBytes(BuildHttpRequestMessage(request)));
                var buffer = new byte[1024];
                var bytesReceived = socksSocket.Receive(buffer);
                while (bytesReceived > 0)
                {
                    responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesReceived));
                    bytesReceived = socksSocket.Receive(buffer);
                }
            }

            return BuildHttpResponseMessage(responseBuilder.ToString());
        }

        private HttpResponseMessage BuildHttpResponseMessage(string response)
        {
            string[] lines = response.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            if (!lines.Any())
            {
                ThrowInvalidResponseException();
            }

            string content = string.Join(Environment.NewLine, lines.SkipWhile(item => item != "").Skip(1));
            HttpResponseMessage result = new HttpResponseMessage
            {
                StatusCode = GetStatusCode(lines[0]),
                Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(content)))
            };

            foreach (string header in lines.Skip(1).TakeWhile(item => item != ""))
            {
                try
                {
                    string[] headerEntry = header.Split(new[] { ':' });
                    result.Headers.Add(headerEntry[0], string.Join(":", headerEntry.Skip(1).ToArray()).Trim());
                }
                catch
                {
                }
            }

            return result;
        }

        private HttpStatusCode GetStatusCode(string firstLine)
        {
            string[] words = firstLine.Split(' ');

            if (words.Length != 3)
            {
                ThrowInvalidResponseException();
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

        private void ThrowIfMaximumNumbersOfRedirectsReached(int redirectsCount)
        {
            if (redirectsCount > MaxRedirectCount)
            {
                throw new InvalidOperationException(Resources.TooManyRedirectsWasRequestedByServer);
            }
        }

        private void ThrowInvalidResponseException()
        {
            throw new ArgumentException("Invalid http response from socks proxy");
        }
    }
}
