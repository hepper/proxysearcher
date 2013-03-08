using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Engine.Checkers
{
    public abstract class CheckerProxyBase : IProxyChecker
    {
        public async void Check(ProxyInfo info, IProxySearchFeedback feedback, IGeoIP geoIP)
        {
            using (Context.Get<TaskCounter>().Listen(TaskType.Search))
            {
                if (await Alive(info))
                {
                    info.CountryInfo = await geoIP.GetLocation(info.Address.ToString());
                    feedback.OnAliveProxy(info);
                }
            }
        }

        protected abstract Task<bool> Alive(ProxyInfo info);
    }
}
