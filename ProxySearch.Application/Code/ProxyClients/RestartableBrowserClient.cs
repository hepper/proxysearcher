using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.ProxyClients
{
    public abstract class RestartableBrowserClient : BrowserClient, IProxyClientRestartable
    {
        private string ProcessName
        {
            get;
            set;
        }

        public RestartableBrowserClient(string proxyType, string name, string settingsKey, string image, int order, string clientName, string processName)
            : base(proxyType, name, settingsKey, image, order, clientName)
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

        public override ProxyInfo Proxy
        {
            get
            {
                return base.Proxy;
            }
            set
            {
                bool restartRequested = false;

                if (IsRunning)
                {
                    if (Context.Get<IMessageBox>().OkCancelQuestion(
                        string.Format(Properties.Resources.DoYouWantToRestartBrowser, Name)) == MessageBoxResult.Cancel)
                    {
                        return;
                    }

                    restartRequested = true;
                }

                if (restartRequested)
                {
                    Close();
                }

                base.Proxy = value;

                if (restartRequested)
                {
                    Open();
                }
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
