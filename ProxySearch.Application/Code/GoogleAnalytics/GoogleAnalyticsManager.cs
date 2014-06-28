using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Code.GoogleAnalytics
{
    public class GoogleAnalyticsManager : IGA
    {
        public async void TrackPageViewAsync(string pageName)
        {
            await Track(HitTypes.AppView, new KeyValuePair<string, string>(GAResources.PageNameKey, pageName));
        }

        public async void TrackEventAsync(EventType eventType, string action, object label = null)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(GAResources.EventCategoryKey, eventType.ToString()),
                new KeyValuePair<string, string>(GAResources.EventActionKey, action)
            };

            if (label != null)
            {
                parameters.Add(new KeyValuePair<string, string>(GAResources.Label, label.ToString()));
            }

            await Track(HitTypes.Event, parameters.ToArray());
        }

        public async void TrackException(Exception exception)
        {
            string data = exception.ToString().Substring(0, 150);
            await Track(HitTypes.Exception, new KeyValuePair<string, string>(GAResources.ExceptionKey, data));
        }

        private Task Track(HitTypes hitType, params KeyValuePair<string, string>[] parameters)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (Context.Get<AllSettings>().ShareUsageStatistic)
                    {
                        List<KeyValuePair<string, string>> allParameters = new List<KeyValuePair<string, string>> 
                        {
                            new KeyValuePair<string, string>(GAResources.ProtocolVersionKey, GAResources.ProtocolVersionFirst),
                            new KeyValuePair<string, string>(GAResources.AccountIdKey, GAResources.AccountId),
                            new KeyValuePair<string, string>(GAResources.ClientIdKey, Context.Get<AllSettings>().RegistrySettings.ClientId),
                            new KeyValuePair<string, string>(GAResources.ApplicationNameKey, GAResources.ApplicationName),
                            new KeyValuePair<string, string>(GAResources.HitTypeKey, hitType.ToString().ToLower()),
                            new KeyValuePair<string, string>(GAResources.ProgramVersionKey, Context.Get<IVersionProvider>().VersionString),
                            new KeyValuePair<string, string>(GAResources.UserLanguage, Context.Get<AllSettings>().SelectedCulture)
                        };

                        string screenResolution = ScreenResolutionOrNull;

                        if (screenResolution != null)
                            allParameters.Add(new KeyValuePair<string, string>(GAResources.ScreenResoultion, ScreenResolutionOrNull));

                        allParameters.AddRange(parameters);

                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.PostAsync(GAResources.CollectUrl, new FormUrlEncodedContent(allParameters));

                            if (response.IsSuccessStatusCode)
                            {
                                await response.Content.ReadAsStringAsync();
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Context.Get<IExceptionLogging>().Write(exception);
                }
            });
        }

        private string ScreenResolutionOrNull
        {
            get
            {
                try
                {
                    return System.Windows.Application.Current.Dispatcher.Invoke<string>(() =>
                    {
                        Window window = System.Windows.Application.Current.MainWindow;
                        if (window == null)
                            return null;

                        Screen activeScreen = Screen.AllScreens.SingleOrDefault(screen => screen.DeviceName == Screen.FromHandle(new WindowInteropHelper(window).Handle).DeviceName);
                        if (activeScreen == null || activeScreen.Bounds == null)
                            return null;

                        return string.Format("{0}x{1}", activeScreen.Bounds.Width, activeScreen.Bounds.Height);
                    });
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
