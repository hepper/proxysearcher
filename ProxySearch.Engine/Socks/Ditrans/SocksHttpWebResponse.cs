using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ProxySearch.Engine.Socks.Ditrans
{
    public class SocksHttpWebResponse : WebResponse
    {
        private WebHeaderCollection _httpResponseHeaders = new WebHeaderCollection();

        public string Content
        {
            get;
            set;
        }

        public HttpStatusCode StatusCode
        {
            get;
            private set;
        }

        public string Location
        {
            get
            {
                return _httpResponseHeaders["Location"];
            }
        }

        public override WebHeaderCollection Headers
        {
            get
            {
                return _httpResponseHeaders;
            }
        }

        public override long ContentLength
        {
            get
            {
                return Content.Length;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public SocksHttpWebResponse(string response)
        {
            string[] lines = response.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            if (!lines.Any())
            {
                ThrowInvalidResponseException();
            }

            StatusCode = GetStatusCode(lines[0]);
            ParseHeaders(lines.Skip(1).TakeWhile(item => item != ""));
            Content = string.Join(Environment.NewLine, lines.SkipWhile(item => item != "").Skip(1));
        }

        public override Stream GetResponseStream()
        {
            return Content.Length == 0 ? Stream.Null : new MemoryStream(Encoding.UTF8.GetBytes(Content));
        }

        public override void Close() { /* the base implementation throws an exception */ }

        private void ParseHeaders(IEnumerable<string> headers)
        {
            foreach (string header in headers)
            {
                string[] headerEntry = header.Split(new[] { ':' });
                Headers.Add(headerEntry[0], string.Join(":", headerEntry.Skip(1).ToArray()).Trim());
            }
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

        private void ThrowInvalidResponseException()
        {
            throw new ArgumentException("Invalid http response from socks proxy");
        }
    }
}