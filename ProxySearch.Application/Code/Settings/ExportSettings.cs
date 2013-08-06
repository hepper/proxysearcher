namespace ProxySearch.Console.Code.Settings
{
    public class ExportSettings
    {
        public ExportSettings()
        {
            HttpExportFolder = Constants.DefaultExportFolder.Http.Location;
            SocksExportFolder = Constants.DefaultExportFolder.Socks.Location;
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
    }
}
