using System;
using System.Collections.Generic;
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
    public abstract class FirefoxClientBase : ConfigurableRestartableBrowserClient
    {
        private static readonly string proxyTypePref = "network.proxy.type";

        private string ProxyPref
        {
            get;
            set;
        }

        private string ProxyPortPref
        {
            get;
            set;
        }

        public FirefoxClientBase(string proxyType)
            : base(proxyType, Resources.Firefox, Resources.Firefox, "/Images/Firefox.png", 1, "FIREFOX.EXE", "firefox", Constants.BackupsLocation.FirefoxSettings)
        {
            string protocolName = GetProtocolName("http", "socks");

            ProxyPref = string.Concat("network.proxy.", protocolName);
            ProxyPortPref = string.Format("network.proxy.{0}_port", protocolName);
        }


        protected sealed override void SetProxy(ProxyInfo proxyInfo)
        {
            File.WriteAllText(SettingsPath, SetProxy(proxyInfo, File.ReadAllText(SettingsPath)));
        }

        protected virtual string SetProxy(ProxyInfo proxyInfo, string content)
        {
            content = WritePref(content, proxyTypePref, "1");
            content = WritePref(content, ProxyPref, string.Format("\"{0}\"", proxyInfo.Address));
            return WritePref(content, ProxyPortPref, proxyInfo.Port.ToString());
        }

        protected sealed override ProxyInfo GetProxy()
        {
            string content = GetContentOrNull();

            if (content == null || ReadPref(content, proxyTypePref) != "1" || ReadPref(content, ProxyPref) == null)
            {
                return null;
            }

            return new ProxyInfo(IPAddress.Parse(ReadPref(content, ProxyPref).Trim('"')),
                                 ushort.Parse(ReadPref(content, ProxyPortPref)));
        }

        private string GetContentOrNull()
        {
            if (!File.Exists(SettingsPath))
            {
                return null;
            }

            return File.ReadAllText(SettingsPath);
        }

        protected override string SettingsPath
        {
            get
            {
                string settingFolder = ProfileFolderPath;
                string userSettings = string.Concat(settingFolder, @"\user.js");

                if (File.Exists(userSettings))
                {
                    return userSettings;
                }

                return string.Concat(settingFolder, @"\prefs.js");
            }
        }

        public override bool IsInstalled
        {
            get
            {
                if (!base.IsInstalled)
                {
                    return false;
                }

                return ProfileFolderPath != null;
            }
        }

        private string ProfileFolderPath
        {
            get
            {
                string mozilla = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla");

                if (Directory.Exists(mozilla))
                {
                    string firefox = Path.Combine(mozilla, "Firefox");

                    if (Directory.Exists(firefox))
                    {
                        string profileFile = Path.Combine(firefox, "profiles.ini");

                        if (File.Exists(profileFile))
                        {
                            using (StreamReader reader = new StreamReader(profileFile))
                            {
                                string[] lines = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                                string location = lines.First(x => x.Contains("Path=")).Split(new string[] { "=" }, StringSplitOptions.None)[1];

                                return Path.Combine(firefox, location);
                            }
                        }
                    }
                }

                return null;
            }
        }

        protected string ReadPref(string content, string name)
        {
            Regex regex = new Regex(GetRegularExpression(name));

            Match match = regex.Match(content);

            if (!match.Success)
                return null;

            return match.Groups["value"].Value;
        }

        protected string WritePref(string content, string name, string newValue)
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

        protected override bool ImportsInternetExplorerSettings
        {
            get
            {
                string content = GetContentOrNull();

                return content == null ? true : ReadPref(content, proxyTypePref) == null;
            }
        }

        private static string GetRegularExpression(string name)
        {
            return string.Format("user_pref\\(\"{0}\", (?<value>[^\\)]*)\\);", name);
        }
    }
}
