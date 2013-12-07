using System;
using System.IO;
using ProxySearch.Common;
using ProxySearch.Console.Code.GoogleAnalytics;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Properties;
using ProxySearch.Engine;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Http;
using ProxySearch.Engine.Proxies.Socks;

namespace ProxySearch.Console.Code
{
    public class ProxySearchFeedback : IProxySearchFeedback
    {
        private StreamWriter stream = null;
        private bool isProxyFound = false;

        private ExportSettings ExportSettings
        {
            get
            {
                return Context.Get<AllSettings>().ExportSettings;
            }
        }

        public ProxySearchFeedback()
        {
            ExportAllowed = true;
            Context.Get<IGA>().TrackEventAsync(EventType.General, Resources.SearchStarted);
        }

        public bool ExportAllowed
        {
            get;
            set;
        }

        public void OnAliveProxy(ProxyInfo proxyInfo)
        {
            if (ExportAllowed && ExportSettings.ExportSearchResult)
            {
                lock (this)
                {
                    if (stream == null)
                    {
                        stream = CreateFile(GetDirectory(proxyInfo.Details.Details));
                    }

                    stream.WriteLine(proxyInfo.ToString(ExportSettings.ExportCountry, ExportSettings.ExportProxyType));
                }
            }

            Context.Get<ISearchResult>().Add(proxyInfo);
            isProxyFound = true;
        }

        public void UpdateJobCount(TaskType type, int currentCount, int totalCount)
        {
            Context.Get<IActionInvoker>().Update(totalCount);
        }

        public void OnSearchFinished()
        {
            Context.Get<IActionInvoker>().Finished(!isProxyFound);
            CloseFile();
            Context.Get<IGA>().TrackEventAsync(EventType.General, Resources.SearchFinished);
        }

        public void OnSearchCancelled()
        {
            Context.Get<IActionInvoker>().Cancelled(!isProxyFound);
            CloseFile();
            Context.Get<IGA>().TrackEventAsync(EventType.General, Resources.SearchCancelled);
        }

        private void CloseFile()
        {
            lock (this)
            {
                if (stream != null && stream.BaseStream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
            }
        }

        private string GetDirectory(ProxyTypeDetails details)
        {
            if (details is HttpProxyDetails)
            {
                return Context.Get<AllSettings>().ExportSettings.HttpExportFolder;
            }

            if (details is SocksProxyDetails)
            {
                return Context.Get<AllSettings>().ExportSettings.SocksExportFolder;
            }

            throw new NotSupportedException();
        }

        private StreamWriter CreateFile(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string fileName = string.Format(@"{0}\Search Results {1}.txt", directory, DateTime.Now.ToString("HH.mm.ss dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture));
            return new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                AutoFlush = true
            };
        }
    }
}
