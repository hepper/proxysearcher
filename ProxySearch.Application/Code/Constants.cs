using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxySearch.Console.Code
{
    public static class Constants
    {
        public static class Working
        {
            static Working()
            {
                if (!System.IO.Directory.Exists(Directory))
                {
                    System.IO.Directory.CreateDirectory(Directory);
                }
            }

            public static readonly string Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ProxySearcher\\";
        }

        public static class SettingsStorage
        {
            public static readonly string Location = Working.Directory + "SettingsStorage.xml";
        }

        public static class UsedProxiesStorage
        {
            public static readonly string Location = Working.Directory + "UsedProxiesStorage.xml";
        }

        public static class ProxySettingsStorage
        {
            public static readonly string Location = Working.Directory + "ProxySettingsStorage.xml";
        }

        public static class DefaultExportFolder
        {
            public static readonly string Location = Working.Directory + "SearchResult\\";
        }

        public static class Browsers
        {
            public static readonly string StartMenuInternet64Bit = @"SOFTWARE\WOW6432Node\Clients\StartMenuInternet";
            public static readonly string StartMenuInternet32Bit = @"SOFTWARE\Clients\StartMenuInternet";

            public static class IE
            {
                public static readonly string Settings = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
                public static readonly string ProxyEnabled = @"ProxyEnable";
                public static readonly string ProxyServer = @"ProxyServer";
            }

            public static class Opera
            {
                public static readonly string Location = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Opera\Opera\operaprefs.ini";
                public static readonly string Section = "Proxy";
            }
        }
    }
}
