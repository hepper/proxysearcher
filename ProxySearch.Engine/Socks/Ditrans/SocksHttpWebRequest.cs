using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Socks.Mentalis;

namespace ProxySearch.Engine.Socks.Ditrans
{
    public class SocksHttpWebRequest : WebRequest
    {
        #region Properties

        private byte[] requestContentBuffer;

        private Uri requestUri;
        public override Uri RequestUri
        {
            get
            {
                return requestUri;
            }
        }

        public bool AllowAutoRedirect
        {
            get;
            set;
        }

        public int MaxRedirectCount
        {
            get;
            set;
        }

        public CancellationTokenSource CancellationToken
        {
            get;
            set;
        }

        public override IWebProxy Proxy { get; set; }

        private WebHeaderCollection requestHeaders = new WebHeaderCollection();
        public override WebHeaderCollection Headers
        {
            get
            {
                return requestHeaders;
            }
            set
            {
                ThrowIfRequestHasBeenSubmitted();
                requestHeaders = value != null ? value : new WebHeaderCollection();
            }
        }

        public bool RequestSubmitted { get; private set; }

        private string method;
        public override string Method
        {
            get
            {
                return method ?? "GET";
            }
            set
            {
                if (!new StringCollection { "GET", "HEAD", "POST", "PUT", "DELETE", "TRACE", "OPTIONS" }.Contains(value))
                    throw new ArgumentOutOfRangeException("value", string.Format(Resources.IsNotKnownVerbFormat, value));

                method = value;
            }
        }

        public override long ContentLength { get; set; }

        public override string ContentType { get; set; }

        #endregion

        #region Constructors

        private SocksHttpWebRequest(Uri requestUri)
        {
            this.requestUri = requestUri;
            MaxRedirectCount = 10;
        }

        public SocksHttpWebRequest(string requestUri, IWebProxy proxy)
            : this(new Uri(requestUri), proxy) { }

        public SocksHttpWebRequest(Uri requestUri, IWebProxy proxy)
            : this(requestUri)
        {
            Proxy = proxy;
        }

        #endregion

        #region WebRequest Members

        private SocksHttpWebResponse response;

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            var taskCompletionSource = new TaskCompletionSource<WebResponse>(state);

            var task = Task.Run<WebResponse>(() =>
            {
                return GetResponse();
            });

            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    taskCompletionSource.TrySetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    taskCompletionSource.TrySetCanceled();
                else
                    taskCompletionSource.TrySetResult(t.Result);

                callback(taskCompletionSource.Task);
            });

            return taskCompletionSource.Task;
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            return response;
        }

        public override WebResponse GetResponse()
        {
            if (Proxy == null)
            {
                throw new InvalidOperationException(Resources.ProxyPropertyCannotBeNull);
            }

            if (!RequestSubmitted)
            {
                response = InternalGetResponse();
                int redirectsCount = 0;

                while (AllowAutoRedirect && (response.StatusCode == HttpStatusCode.Ambiguous ||
                                            response.StatusCode == HttpStatusCode.Moved ||
                                            response.StatusCode == HttpStatusCode.Redirect ||
                                            response.StatusCode == HttpStatusCode.RedirectMethod ||
                                            response.StatusCode == HttpStatusCode.RedirectKeepVerb))
                {
                    if (redirectsCount > MaxRedirectCount)
                    {
                        throw new InvalidOperationException(Resources.TooManyRedirectsWasRequestedByServer);
                    }

                    requestUri = new Uri(response.Location);
                    response = InternalGetResponse();
                    redirectsCount++;
                }

                RequestSubmitted = true;
            }

            return response;
        }

        public override Stream GetRequestStream()
        {
            ThrowIfRequestHasBeenSubmitted();

            if (requestContentBuffer == null)
            {
                requestContentBuffer = new byte[ContentLength];
            }
            else if (ContentLength == default(long))
            {
                requestContentBuffer = new byte[int.MaxValue];
            }
            else if (requestContentBuffer.Length != ContentLength)
            {
                Array.Resize(ref requestContentBuffer, (int)ContentLength);
            }

            return new MemoryStream(requestContentBuffer);
        }

        #endregion

        #region Methods

        private SocksHttpWebResponse InternalGetResponse()
        {
            var responseBuilder = new StringBuilder();
            using (ProxySocket socksSocket = new ProxySocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var proxyUri = Proxy.GetProxy(RequestUri);
                var ipAddress = GetProxyIpAddress(proxyUri);
                SocksWebProxy socksProxy = Proxy as SocksWebProxy;

                socksSocket.ProxyType = socksProxy == null ? ProxyTypes.Socks4 : socksProxy.ProxyType;
                socksSocket.ProxyEndPoint = new IPEndPoint(ipAddress, proxyUri.Port);
                
                socksSocket.Connect(RequestUri.Host, RequestUri.Port);
                socksSocket.Send(Encoding.UTF8.GetBytes(BuildHttpRequestMessage()));
                var buffer = new byte[1024];
                var bytesReceived = socksSocket.Receive(buffer);
                while (bytesReceived > 0)
                {
                    responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesReceived));
                    bytesReceived = socksSocket.Receive(buffer);
                }
            }

            return new SocksHttpWebResponse(responseBuilder.ToString());
        }

        private string BuildHttpRequestMessage()
        {
            ThrowIfRequestHasBeenSubmitted();

            var message = new StringBuilder();

            message.AppendFormat("{0} {1} HTTP/1.0", Method, RequestUri.PathAndQuery).AppendLine();
            message.AppendFormat("Host: {0}", RequestUri.Host).AppendLine();

            foreach (var key in Headers.Keys)
            {
                message.AppendFormat("{0}: {1}", key, Headers[key.ToString()]).AppendLine();
            }

            if (!string.IsNullOrEmpty(ContentType))
            {
                message.AppendFormat("Content-Type: {0}", ContentType).AppendLine();
            }

            if (ContentLength > 0)
            {
                message.AppendFormat("Content-Length: {0}", ContentLength).AppendLine();
            }

            message.AppendLine();

            if (requestContentBuffer != null && requestContentBuffer.Length > 0)
            {
                using (var stream = new MemoryStream(requestContentBuffer, false))
                using (var reader = new StreamReader(stream))
                {
                    message.Append(reader.ReadToEnd());
                }
            }

            return message.ToString();
        }

        private IPAddress GetProxyIpAddress(Uri proxyUri)
        {
            IPAddress ipAddress;

            if (IPAddress.TryParse(proxyUri.Host, out ipAddress))
            {
                return ipAddress;
            }

            try
            {
                return Dns.GetHostEntry(proxyUri.Host).AddressList[0];
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(string.Format(Resources.UnableToResolveProxyHostnameFormat, proxyUri.Host), e);
            }
        }

        private void ThrowIfRequestHasBeenSubmitted()
        {
            if (RequestSubmitted)
            {
                throw new InvalidOperationException(Resources.ThisOperationCannotBePerformedAfterTheRequestHasBeenSubmitted);
            }
        }

        #endregion
    }
}