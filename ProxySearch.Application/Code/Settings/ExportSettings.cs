namespace ProxySearch.Console.Code.Settings
{
    public class ExportSettings
    {
        public ExportSettings()
        {
            ExportFolder = Constants.DefaultExportFolder.Location;
        }

        public bool ExportSearchResult
        {
            get;
            set;
        }

        public string ExportFolder
        {
            get;
            set;
        }
    }
}
