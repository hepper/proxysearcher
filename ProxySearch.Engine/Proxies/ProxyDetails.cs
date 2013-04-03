using System;
using System.Threading.Tasks;

namespace ProxySearch.Engine.Proxies
{
    public class ProxyDetails
    {
        public ProxyDetails(object details)
        {
            Details = details;
        }

        public ProxyDetails(object details, Func<ProxyInfo, Task<object>> updateMethod)
            : this(details)
        {
            UpdateMethod = updateMethod;
        }

        public object Details
        {
            get;
            private set;
        }

        public Func<ProxyInfo, Task<object>> UpdateMethod
        {
            get;
            private set;
        }
    }
}
