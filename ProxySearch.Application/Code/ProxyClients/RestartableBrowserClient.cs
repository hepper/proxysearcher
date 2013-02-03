using System;
using System.Diagnostics;
using System.Linq;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Code.ProxyClients
{
    public abstract class RestartableBrowserClient : BrowserClient, IProxyClientRestartable
    {
        private string ProcessName
        {
            get;
            set;
        }

        public RestartableBrowserClient(string name, string image, string clientName, string processName)
            : base(name, image, clientName)
        {
            ProcessName = processName;
        }

        public bool IsRunning
        {
            get
            {
                return Processes.Any();
            }
        }

        public void Close()
        {
            foreach (Process process in Processes)
            {
                process.CloseMainWindow();
                process.WaitForExit();
            }
        }

        public void Open()
        {
            
        }

        private Process[] Processes
        {
            get
            {
                return Process.GetProcesses().Where(process => string.Equals(process.ProcessName, ProcessName, StringComparison.CurrentCultureIgnoreCase)).ToArray();
            }
        }
    }
}
