namespace ProxySearch.Console.Code.Settings
{
    public class ExportSettings
    {
        public ExportSettings()
        {
            HttpExportFolder = Constants.DefaultExportFolder.Http.Location;
            SocksExportFolder = Constants.DefaultExportFolder.Socks.Location;
            ExportCountry = true;
            ExportProxyType = true;
        }

        public bool ExportSearchResult
        {
            get;
            set;
        }

        public string HttpExportFolder
        {
            get;
            set;
        }

        public string SocksExportFolder
        {
            get;
            set;
        }

        public bool ExportCountry
        {
            get;
            set;
        }

        public bool ExportProxyType
        {
            get;
            set;
        }
    }
}
