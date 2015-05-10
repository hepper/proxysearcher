using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Extensions;
using ProxySearch.Console.Code.GoogleAnalytics;
using ProxySearch.Console.Code.Interfaces;
using SHDocVw;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for AdvertisingControl.xaml
    /// </summary>
    public partial class AdvertisingControl : UserControl
    {
        private static readonly string adsUri = "http://proxysearcher.sourceforge.net/Ads.php?interactive=true&v={0}&n={1}";
        private bool isUserClickedOnAdvertising = false;
        private Action updateCursor = null;
        private int delay = 1;
        TimeSpan loadAdvertisingTimeout = TimeSpan.FromSeconds(3);
        Timer timer = new Timer();
        bool isAnimationPlayed = false;
        int updateNumber = 0;

        public TimeSpan RefreshInterval
        {
            get
            {
                return TimeSpan.FromSeconds((3 * 60 + new Random(Environment.TickCount).Next(2 * 60)));
            }
        }

        private List<IProxyClient> IEClients
        {
            get
            {
                return Context.Get<IProxyClientSearcher>().IEClients;
            }
        }

        private bool IsIEUsingProxy
        {
            get
            {
                return IEClients.Any(client => client.Proxy != null);
            }
        }

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

                IEClients.ForEach(client => client.ProxyChanged += () =>
                {
                    if (!isAnimationPlayed)
                    {
                        timer.Interval = 1;
                        timer.Stop();
                        timer.Start();
                    }
                });

                timer.Elapsed += (sender, e) =>
                {
                    updateNumber++;

                    timer.Interval = RefreshInterval.TotalMilliseconds;
                    if (!IsIEUsingProxy)
                    {
                        webBrowser.Navigate(string.Format(adsUri, Context.Get<IVersionProvider>().Version.ToString(), updateNumber));

                        if (!isAnimationPlayed)
                        {
                            isAnimationPlayed = true;
                            Action action = () => Dispatcher.BeginInvoke(new Action(() => PlayAnimation("ExpandControl")));
                            action.RunWithDelay(loadAdvertisingTimeout);
                        }
                    }
                };

                timer.Start();
            }
        }

        private void browser_NavigateError(object pDisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
        {
            Context.Get<IGA>().TrackException(string.Format("Cannot open advertising. Url: {0}, StatusCode: {1}", url, statusCode));
        }

        private void webBrowser_NewWindow3(ref object ppDisp, ref bool cancel, uint flags, string urlContext, string url)
        {
            IEClients.ForEach(client => client.Proxy = null);            
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
                timer.Stop();
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