using System.Threading;
using System.Threading.Tasks;

namespace ProxySearch.Engine.SearchEngines.Google
{
    public interface ICaptchaWindow
    {
        Task<string> GetSolvedContentAsync(string url, int pageNumber, CancellationToken cancellationToken);
    }
}
