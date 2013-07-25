using System;

namespace ProxySearch.Engine.Socks
{
    public class SocksRequestFailedException : Exception
    {
        public SocksRequestFailedException()
        {
        }

        public SocksRequestFailedException(string description)
            : base(description)
        {
        }
    }
}
