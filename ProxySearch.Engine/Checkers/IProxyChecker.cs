using System.Threading.Tasks;

namespace ProxySearch.Engine.Checkers
{
    public interface IProxyChecker
    {
        Task<bool> Alive(ProxyInfo info);
    }
}
