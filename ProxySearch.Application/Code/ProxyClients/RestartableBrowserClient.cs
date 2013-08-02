using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Win32;
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

        public RestartableBrowserClient(string proxyType, string name, string image, int order, string clientName, string processName)
            : base(proxyType, name, image, order, clientName)
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
            Process.Start(BrowserPath);
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
