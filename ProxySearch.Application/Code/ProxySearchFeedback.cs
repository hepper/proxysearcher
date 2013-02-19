using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProxySearch.Engine;
using System.Windows.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using System.IO;
using System.Threading.Tasks;

namespace ProxySearch.Console.Code
{
    public class ProxySearchFeedback : IProxySearchFeedback
    {
        private Dictionary<string, ProxyInfo> Dictionary { get; set; }

        public ProxySearchFeedback()
        {
            Dictionary = new Dictionary<string, ProxyInfo>();
        }

        public void OnAliveProxy(ProxyInfo proxyInfo)
        {
            Context.Get<ISearchResult>();

            if (!Dictionary.ContainsKey(proxyInfo.ToString()))
            {
                Dictionary.Add(proxyInfo.ToString(), proxyInfo);

                Context.Get<ISearchResult>().Add(proxyInfo);
            }
        }

        public void OnDeadProxy(ProxyInfo proxyInfo)
        {
        }

        public async void OnSearchFinished()
        {
            await ExportList();
            Context.Get<IActionInvoker>().End();
        }

        public async void OnSearchCancelled()
        {
            await ExportList();
            Context.Get<IActionInvoker>().End();
        }

        public void UpdateJobCount(TaskType type, int currentCount, int totalCount)
        {
            Context.Get<IActionInvoker>().Update(totalCount);
        }

        private async Task ExportList()
        {
            if (!Context.Get<AllSettings>().ExportSettings.ExportSearchResult && Dictionary.Count > 0)
                return;

            try
            {
                string directory = Context.Get<AllSettings>().ExportSettings.ExportFolder;

                string fileName = string.Format("{0}Search Results {1}.txt", directory,
                   DateTime.Now.ToString("HH.mm.ss dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture));

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (FileStream stream = File.Create(fileName))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach (ProxyInfo proxyInfo in Dictionary.Values)
                    {
                        await writer.WriteLineAsync(proxyInfo.ToString());
                    }
                }
            }
            catch
            {
            }
        }
    }
}
