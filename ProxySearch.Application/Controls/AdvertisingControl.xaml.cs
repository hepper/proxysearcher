using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ProxySearch.Common;
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
        private static readonly Uri adsUri = new Uri("http://proxysearcher.sourceforge.net/Ads.php");
        private bool hasErrorHappened = false;
        private bool isUserClickedOnAdvertising = false;
        private bool isAnimationPlayed = false;

        private Action updateCursor = null;
        private int delay = 1;

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
                browser.NavigateComplete2 += browser_NavigateComplete2;

                browser.StatusTextChange += browser_StatusTextChange;
                browser.TitleChange += browser_TitleChange;
                webBrowser.Navigate(adsUri);

                IsVisibleChanged += Advertising_IsVisibleChanged;
            }
        }

        private void Advertising_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible && hasErrorHappened)
            {
                hasErrorHappened = false;
                webBrowser.Navigate(adsUri);
            }
        }

        private void browser_NavigateComplete2(object pDisp, ref object url)
        {
            if (adsUri.OriginalString != (string)url && !hasErrorHappened && !isAnimationPlayed)
            {
                isAnimationPlayed = true;
                PlayAnimation("ExpandControl");
            }
        }

        private void browser_NavigateError(object pDisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
        {
            Context.Get<IGA>().TrackException(string.Format("Cannot open advertising. Url: {0}, StatusCode: {1}", url, statusCode));
            hasErrorHappened = true;
        }

        private void webBrowser_NewWindow3(ref object ppDisp, ref bool cancel, uint flags, string urlContext, string url)
        {
            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = url;
            psi.UseShellExecute = true;

            cancel = true;
            isUserClickedOnAdvertising = true;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return;
            }

            Uri uri = new Uri(url);

            if (uri.Scheme != "http" && uri.Scheme != "https")
            {
                return;
            }

            try
            {
                Process.Start(url);
                Context.Get<IGA>().TrackEventAsync(EventType.General, ProxySearch.Console.Properties.Resources.AdvertisingOpenedInBrowser);
            }
            catch (Win32Exception exception)
            {
                Context.Get<IGA>().TrackException(exception);
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