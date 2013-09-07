using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Engine.Parser;

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

        public ParseDetails ParseDetails
        {
            get
            {
                return (ParseDetails)this.GetValue(ParseDetailsProperty);
            }
            set
            {
                UrlTextBox.IsEnabled = !string.IsNullOrEmpty(value.Url);

                this.SetValue(ParseDetailsProperty, new ParseDetails
                {
                    Url = value.Url,
                    RawRegularExpression = value.RawRegularExpression,
                    Code = value.Code
                });
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

        private void DoTest()
        {
            if (!TestUri.ToString().Contains(ParseDetails.Url))
            {
                throw new InvalidOperationException(Properties.Resources.TestUrlDoesntMatchDefinedUrl);
            }

            string document = null;

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = client.GetAsync(TestUri).GetAwaiter().GetResult())
            {
                response.EnsureSuccessStatusCode();

                document = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            SetTestResult(document);
        }

        private Uri TestUri
        {
            get
            {
                return new Uri(TestUrlTextBox.Text);
            }
        }
    }
}
