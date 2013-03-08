using System;
using System.IO;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine;

namespace ProxySearch.Console.Code
{
    public class ProxySearchFeedback : IProxySearchFeedback
    {
        private StreamWriter stream = null;

        public void OnAliveProxy(ProxyInfo proxyInfo)
        {
            if (Context.Get<AllSettings>().ExportSettings.ExportSearchResult)
            {
                if (stream == null)
                {
                    stream = CreateFile();
                }

                stream.WriteLine(proxyInfo.ToString());
            }

            Context.Get<ISearchResult>().Add(proxyInfo);
        }

        public void UpdateJobCount(TaskType type, int currentCount, int totalCount)
        {
            Context.Get<IActionInvoker>().Update(totalCount);
        }

        public void OnSearchFinished()
        {
            End();
        }

        public void OnSearchCancelled()
        {
            End();
        }

        private void End()
        {
            Context.Get<IActionInvoker>().End();
            if (stream != null)
                stream.Dispose();
        }

        private StreamWriter CreateFile()
        {
            string directory = Context.Get<AllSettings>().ExportSettings.ExportFolder;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string fileName = string.Format("{0}Search Results {1}.txt", directory, DateTime.Now.ToString("HH.mm.ss dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture));
            return new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                AutoFlush = true
            };
        }
    }
}
