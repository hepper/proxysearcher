using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using ProxySearch.Engine.Parser;

namespace ProxySearch.Console.Code.Settings
{
    [Serializable]
    public class AllSettings
    {
        public AllSettings()
        {
            TabSettings = new ObservableCollection<TabSettings>();
            GeoIPSettings = new List<ParametersPair>();
            ExportSettings = new ExportSettings();
            PageSize = 20;
            MaxBandwidth = 1;
            RevertUsedProxiesOnExit = true;
            ClientId = Guid.NewGuid();
            ShareUsageStatistic = true;
            ParseDetails = new List<ParseDetails>();
        }

        public bool CheckUpdates
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
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

        public Guid ClientId
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
    }
}
