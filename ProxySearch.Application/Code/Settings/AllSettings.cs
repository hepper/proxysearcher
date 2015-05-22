using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using ProxySearch.Console.Code.Language;
using ProxySearch.Engine.Parser;
using ProxySearch.Engine.Proxies.Http;

namespace ProxySearch.Console.Code.Settings
{
    [Serializable]
    public class AllSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AllSettings()
        {
            TabSettings = new ObservableCollection<TabSettings>();
            GeoIPSettings = new List<ParametersPair>();
            ExportSettings = new ExportSettings();
            PageSize = DefaultPageSize;
            MaxBandwidth = 1;
            RevertUsedProxiesOnExit = true;
            ShareUsageStatistic = true;
            ParseDetails = new List<ParseDetails>();
            SelectedCulture = new LanguageManager().DefaultLanguage.Culture;
            RegistrySettings = new RegistrySettings();
            IgnoredHttpProxyTypes = new HttpProxyTypes[] 
            {
                HttpProxyTypes.ChangesContent,
                HttpProxyTypes.Transparent
            };

            MainWindowState = new MainWindowState();
            ResultGridColumnWidth = new List<double>();
        }

        private static int DefaultPageSize
        {
            get
            {
                return 20;
            }
        }

        public bool CheckUpdates
        {
            get;
            set;
        }

        private int pageSize;
        public int PageSize
        {
            get
            {
                if (pageSize <= 0)
                    return DefaultPageSize;

                return pageSize;
            }
            set
            {
                pageSize = value;
                FireNotifyPropertyChanged("PageSize");
            }
        }

        public string GeoIPDetectableType
        {
            get;
            set;
        }

        public List<ParametersPair> GeoIPSettings
        {
            get;
            set;
        }

        public int MaxThreadCount
        {
            get;
            set;
        }

        public ExportSettings ExportSettings
        {
            get;
            set;
        }

        public double MaxBandwidth
        {
            get;
            set;
        }

        public bool RevertUsedProxiesOnExit
        {
            get;
            set;
        }

        public bool ShareUsageStatistic
        {
            get;
            set;
        }

        public ObservableCollection<TabSettings> TabSettings
        {
            get;
            set;
        }

        public string SelectedCulture
        {
            get;
            set;
        }

        public HttpProxyTypes[] IgnoredHttpProxyTypes
        {
            get;
            set;
        }

        private Guid selectedTabSettingsId = Guid.Empty;
        public Guid SelectedTabSettingsId
        {
            get
            {
                return SelectedTabSettings.Id;
            }
            set
            {
                selectedTabSettingsId = value;
            }
        }

        public List<ParseDetails> ParseDetails
        {
            get;
            set;
        }

        public MainWindowState MainWindowState
        {
            get;
            set;
        }

        public List<double> ResultGridColumnWidth
        {
            get;
            set;
        }

        [XmlIgnore]
        public TabSettings SelectedTabSettings
        {
            get
            {
                return TabSettings.SingleOrDefault(tab => tab.Id == selectedTabSettingsId) ?? TabSettings.First();
            }
            set
            {
                selectedTabSettingsId = value.Id;
            }
        }

        [XmlIgnore]
        public RegistrySettings RegistrySettings
        {
            get;
            set;
        }

        private void FireNotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
