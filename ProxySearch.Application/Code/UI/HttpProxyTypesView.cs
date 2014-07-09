using System.Collections.Generic;
using System.Linq;
using ProxySearch.Common;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Proxies.Http;

namespace ProxySearch.Console.Code.UI
{
    public class HttpProxyTypesView
    {
        private static HttpProxyTypes[] IgnoredHttpProxyTypes
        {
            get
            {
                return Context.Get<AllSettings>().IgnoredHttpProxyTypes;
            }
            set
            {
                Context.Get<AllSettings>().IgnoredHttpProxyTypes = value;
            }
        }

        public HttpProxyTypes Type
        {
            get;
            set;
        }

        public bool IsSelected
        {
            get
            {
                return IgnoredHttpProxyTypes.Contains(Type);
            }
            set
            {
                if (value && !IsSelected)
                {
                    List<HttpProxyTypes> list = new List<HttpProxyTypes>(IgnoredHttpProxyTypes);
                    list.Add(Type);
                    IgnoredHttpProxyTypes = list.ToArray();
                }

                if (!value && !IsSelected)
                {
                    List<HttpProxyTypes> list = new List<HttpProxyTypes>(IgnoredHttpProxyTypes);
                    list.Remove(Type);
                    IgnoredHttpProxyTypes = list.ToArray();
                }
            }
        }
    }
}
