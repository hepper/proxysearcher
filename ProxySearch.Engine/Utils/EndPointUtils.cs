using System;
using System.Net;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Utils
{
    public class EndPointUtils
    {
        public IPEndPoint UriToIPEndPoint(Uri uri)
        {
            return new IPEndPoint(GetIpAddress(uri.Host), uri.Port);
        }

        public IPAddress GetIpAddress(string host)
        {
            IPAddress ipAddress;

            if (IPAddress.TryParse(host, out ipAddress))
            {
                return ipAddress;
            }

            try
            {
                return Dns.GetHostEntry(host).AddressList[0];
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(string.Format(Resources.UnableToResolveProxyHostnameFormat, host), e);
            }
        }
    }
}
