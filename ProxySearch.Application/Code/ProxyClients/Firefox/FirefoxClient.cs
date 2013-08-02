using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ProxySearch.Console.Code.Extensions;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.ProxyClients.Firefox
{
    public class FirefoxClient : ConfigurableRestartableBrowserClient
    {
        private static readonly string proxyTypePref = "network.proxy.type";
        private static readonly string proxyHttpPref = "network.proxy.http";
        private static readonly string proxyPortPref = "network.proxy.http_port";

        public FirefoxClient()
            : base(Resources.HttpProxyType, Resources.Firefox, Resources.Firefox, "/Images/Firefox.png", 1, "FIREFOX.EXE", "firefox", Constants.BackupsLocation.FirefoxSettings)
        {
        }

        protected override void SetProxy(ProxyInfo proxyInfo)
        {
            string content = File.ReadAllText(SettingsPath);

            content = WritePref(content, proxyTypePref, "1");
            content = WritePref(content, proxyHttpPref, string.Format("\"{0}\"", proxyInfo.Address));
            content = WritePref(content, proxyPortPref, proxyInfo.Port.ToString());

            File.WriteAllText(SettingsPath, content);
        }

        protected override ProxyInfo GetProxy()
        {
            if (!File.Exists(SettingsPath))
            {
                return null;
            }

            string content = File.ReadAllText(SettingsPath);

            if (ReadPref(content, proxyTypePref) != "1")
            {
                return null;
            }

            return new ProxyInfo(IPAddress.Parse(ReadPref(content, proxyHttpPref).Trim('"')),
                                 ushort.Parse(ReadPref(content, proxyPortPref)));
        }

        protected override string SettingsPath
        {
            get
            {
                string folder = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"\Mozilla\Firefox\Profiles\");
                string settingFolder = Directory.GetDirectories(folder).Where(path => path.EndsWith(".default")).First();
                string userSettings = string.Concat(settingFolder, @"\user.js");

                if (File.Exists(userSettings))
                {
                    return userSettings;
                }

                return string.Concat(settingFolder, @"\prefs.js");
            }
        }

        private string ReadPref(string content, string name)
        {
            Regex regex = new Regex(GetRegularExpression(name));

            Match match = regex.Match(content);

            if (!match.Success)
                return null;

            return match.Groups["value"].Value;
        }

        private string WritePref(string content, string name, string newValue)
        {
            string oldValue = ReadPref(content, name);

            if (oldValue == null)
            {
                StringBuilder builder = new StringBuilder();

                if (content != string.Empty)
                {
                    builder.Append(content);
                }

                builder.AppendFormat("user_pref(\"{0}\", {1});", name, newValue);
                builder.AppendLine();

                return builder.ToString();
            }

            if (oldValue == newValue)
                return content;

            return new Regex(GetRegularExpression(name)).ReplaceGroup(content, "value", newValue);
        }

        private static string GetRegularExpression(string name)
        {
            return string.Format("user_pref\\(\"{0}\", (?<value>[^\\)]*)\\);", name);
        }
    }
}
