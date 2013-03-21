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

        private string BrowserPath
        {
            get;
            set;
        }

        public RestartableBrowserClient(string name, string image, int order, string clientName, string processName)
            : base(name, image, order, clientName)
        {
            ProcessName = processName;

            RegistryKey browserPath = Registry.LocalMachine.OpenSubKey(string.Format(Constants.Browsers.BrowserPath64Bit, clientName));

            if (browserPath == null)
                browserPath = Registry.LocalMachine.OpenSubKey(string.Format(Constants.Browsers.BrowserPath32Bit, clientName));

            BrowserPath = (string)browserPath.GetValue(null);
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
