using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Extended;

namespace ProxySearch.Engine.Extension
{
    public static class CancellationTokenExtension
    {
        public static Task AsAwaitable(this CancellationToken cancellationToken)
        {
            var manualResetEvent = new AsyncManualResetEvent();
            cancellationToken.Register(manualResetEvent.Set);

            return manualResetEvent.WaitAsync();
        }
    }
}
