using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Version
{
    public class VersionManager
    {
        public async void Check()
        {
            if (!Context.Get<AllSettings>().CheckUpdates)
            {
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(Resources.HomePageLink))
                {
                    if (!response.IsSuccessStatusCode)
                        return;

                    string document = await response.Content.ReadAsStringAsync();

                    if (Context.Get<IVersionProvider>().Version >= GetVersion(document))
                        return;

                    if (Context.Get<IMessageBox>().YesNoQuestion(Resources.UpdateIsReadyToInstall) != MessageBoxResult.Yes)
                        return;

                    new DownloadNewVersion(GetInstallerUrl(document)).ShowDialog();
                }
            }
            catch
            {
            }
        }

        public int GetVersion(string document)
        {
            try
            {
                Match match = new Regex("<input type=\"hidden\" id=\"currentVersion\" value=\"(?<name>[0-9]+?)\" />").Match(document);
                return int.Parse(match.Groups["name"].Value);
            }
            catch
            {
                return 0;
            }
        }

        public string GetInstallerUrl(string document)
        {
            try
            {
                Match match = new Regex("<input type=\"hidden\" id=\"installationPath\" value=\"(?<name>[^\"]*)\" />").Match(document);
                return match.Groups["name"].Value;
            }
            catch
            {
                throw new InvalidOperationException(Resources.CannotFindInstallationUrl);
            }
        }
    }
}