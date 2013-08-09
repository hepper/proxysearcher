using System.Threading;
using System.Threading.Tasks;

namespace ProxySearch.Engine.Extended
{
    public class AsyncManualResetEvent
    {
        private volatile TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

        public Task WaitAsync() 
        { 
            return taskCompletionSource.Task; 
        }

        public void Set() 
        { 
            taskCompletionSource.TrySetResult(true);
        }

        public void Reset()
        {
            while (true)
            {
                var tcs = taskCompletionSource;
#pragma warning disable
                if (!tcs.Task.IsCompleted || Interlocked.CompareExchange(ref taskCompletionSource, new TaskCompletionSource<bool>(), tcs) == tcs)
                    return;
#pragma warning enable
            }
        }
    }
}
