using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Extensions;
using ProxySearch.Console.Code.GoogleAnalytics;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Engine.Proxies;
using SHDocVw;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for AdvertisingControl.xaml
    /// </summary>
    public partial class AdvertisingControl : UserControl
    {
        private static readonly Uri adsUri = new Uri("http://proxysearcher.sourceforge.net/Ads.php?interactive=true");
        private bool isUserClickedOnAdvertising = false;
        private Action updateCursor = null;
        private int delay = 1;
        Timer timer = new Timer((4 * 60 + new Random(Environment.TickCount).Next(120)) * 1000); // 4-6 minutes 
        TimeSpan loadAdvertisingTimeout = TimeSpan.FromSeconds(3);

        public AdvertisingControl()
        {
            InitializeComponent();

            DWebBrowserEvents2_Event browser = webBrowser.ActiveXInstance as DWebBrowserEvents2_Event;

            //Workaround which allows to minimize showing time of wait cursor when WebBrowser navigating.
            updateCursor = () => Dispatcher.Invoke(Mouse.UpdateCursor);

            if (browser != null)
            {
                browser.NewWindow3 += webBrowser_NewWindow3;
                browser.NavigateError += browser_NavigateError;

                browser.StatusTextChange += browser_StatusTextChange;
                browser.TitleChange += browser_TitleChange;
                NavigateWithoutProxy();

                Action action = () => Dispatcher.BeginInvoke(new Action(() =>
                                                             {
                                                                 PlayAnimation("ExpandControl");
                                                             }));
                action.RunWithDelay(loadAdvertisingTimeout);

                timer.Elapsed += (sender, e) =>
                {
                    NavigateWithoutProxy();
                };

                timer.Start();
            }
        }

        private void NavigateWithoutProxy()
        {
            IProxyClient proxyClient = Context.Get<IProxyClientSearcher>().GetInternetExplorerClientOrNull();
            ProxyInfo proxy = proxyClient != null ? proxyClient.Proxy : null;

            if (proxy != null)
            {
                proxyClient.Proxy = null;
                Dispatcher.Invoke(() => Context.Get<ISearchResult>().UpdatePageData());
            }

            try
            {
                webBrowser.Navigate(adsUri);
            }
            finally
            {
                if (proxy != null)
                {
                    Action action = () =>
                    {
                        if (proxy != null)
                        {
                            proxyClient.Proxy = proxy;
                            Dispatcher.Invoke(() => Context.Get<ISearchResult>().UpdatePageData());
                        }
                    };
                    action.RunWithDelay(loadAdvertisingTimeout);
                }
            }
        }

        private void browser_NavigateError(object pDisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
        {
            Context.Get<IGA>().TrackException(string.Format("Cannot open advertising. Url: {0}, StatusCode: {1}", url, statusCode));
        }

        private void webBrowser_NewWindow3(ref object ppDisp, ref bool cancel, uint flags, string urlContext, string url)
        {
            IProxyClient proxyClient = Context.Get<IProxyClientSearcher>().GetInternetExplorerClientOrNull();
            ProxyInfo proxy = proxyClient != null ? proxyClient.Proxy : null;

            if (proxy != null)
            {
                proxyClient.Proxy = null;
                Dispatcher.Invoke(() => Context.Get<ISearchResult>().UpdatePageData());
            }
        }

        private void PlayAnimation(string name)
        {
            Storyboard storyBoard = (Storyboard)FindResource(name);
            Storyboard.SetTarget(storyBoard, this);
            storyBoard.Begin();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Context.Get<IGA>().TrackEventAsync(EventType.ButtonClick, Buttons.CloseAdvertising.ToString());

            if (isUserClickedOnAdvertising || Context.Get<IMessageBox>()
                       .OkCancelQuestion(ProxySearch.Console.Controls.Resources.AdvertisingControl.CloseAdvertisingQuestion) == MessageBoxResult.OK)
            {
                PlayAnimation("CollapseControl");
                Context.Get<IGA>().TrackEventAsync(EventType.General, Properties.Resources.AdvertisingClosed);
            }
        }

        private void browser_TitleChange(string Text)
        {
            updateCursor.RunWithDelay(TimeSpan.FromMilliseconds(delay));
        }

        private void browser_StatusTextChange(string Text)
        {
            updateCursor.RunWithDelay(TimeSpan.FromMilliseconds(delay));
        }
    }
}