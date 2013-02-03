namespace ProxySearch.Console.Code.Interfaces
{
    public interface IProxyClientRestartable
    {
        bool IsRunning
        {
            get;
        }

        void Close();
        void Open();
    }
}
