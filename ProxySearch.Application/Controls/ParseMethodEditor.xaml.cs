using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Error;
using ProxySearch.Engine.Parser;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for ParseMethodEditor.xaml
    /// </summary>
    public partial class ParseMethodEditor : UserControl
    {
        public static readonly DependencyProperty ParseDetailsProperty = DependencyProperty.Register("ParseDetails", typeof(ParseDetails), typeof(ParseMethodEditor));

        public ParseMethodEditor()
        {
            InitializeComponent();
        }

        private ParseDetails parseDetails = null;
        public ParseDetails ParseDetails
        {
            get
            {
                return (ParseDetails)this.GetValue(ParseDetailsProperty);
            }
            set
            {
                parseDetails = value;
                UrlTextBox.IsEnabled = !string.IsNullOrEmpty(value.Url);

                this.SetValue(ParseDetailsProperty, new ParseDetails
                {
                    Url = value.Url,
                    RawRegularExpression = value.RawRegularExpression,
                    Code = value.Code
                });
            }
        }

        public bool IsNew
        {
            get;
            set;
        }

        private Uri TestUri
        {
            get
            {
                return new Uri(TestUrlTextBox.Text);
            }
        }

        private static void GoBack()
        {
            SettingsControl settings = new SettingsControl();
            Context.Get<ISettingsTabNavigator>().OpenAdvancedTab();
            Context.Get<IControlNavigator>().GoTo(settings);
        }

        private void ApplyChanges(object sender, System.Windows.RoutedEventArgs e)
        {
            parseDetails.Url = ParseDetails.Url;
            parseDetails.RawRegularExpression = ParseDetails.RawRegularExpression;
            parseDetails.Code = ParseDetails.Code;

            if (IsNew)
            {
                Context.Get<AllSettings>().ParseDetails.Add(ParseDetails);
            }

            GoBack();
        }

        private void Cancel(object sender, System.Windows.RoutedEventArgs e)
        {
            GoBack();
        }

        private void TestParseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                DoTest();
            }
            catch (Exception exception)
            {
                SetTestResult(exception.Message);
            }
        }

        private void SetTestResult(string message)
        {
            TestResult.Text = message;
        }

        private string DownloadContent()
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = client.GetAsync(TestUri).GetAwaiter().GetResult())
            {
                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
        }

        private void DoTest()
        {
            if (!TestUri.ToString().Contains(ParseDetails.Url))
            {
                throw new InvalidOperationException(Properties.Resources.TestUrlDoesntMatchDefinedUrl);
            }

            Regex regex = new Regex(ParseDetails.RegularExpression);

            string content = DownloadContent();

            IEnumerable<Proxy> proxies = new RegexCompilerMethod(ParseDetails).Parse(content);

            if (proxies.Any())
                SetTestResult(string.Join(Environment.NewLine, proxies));
            else
                SetTestResult(string.Join(Environment.NewLine, Properties.Resources.NoOneProxyHasBeenFound, content));
        }
    }
}