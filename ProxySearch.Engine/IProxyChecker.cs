using System.Threading.Tasks;

namespace ProxySearch.Engine
{
    public interface IProxyChecker
    {
        Task<bool> Alive(ProxyInfo info);
    }
}
