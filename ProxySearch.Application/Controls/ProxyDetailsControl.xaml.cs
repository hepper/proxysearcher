using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Console.Code;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for ProxyDetailsControl.xaml
    /// </summary>
    public partial class ProxyDetailsControl : UserControl
    {
        public static readonly DependencyProperty ProxyProperty = DependencyProperty.Register("Proxy", typeof(ProxyInfo), typeof(ProxyDetailsControl));

        public ProxyDetailsControl()
        {
            InitializeComponent();
        }

        public ProxyInfo Proxy
        {
            get
            {
                return (ProxyInfo)this.GetValue(ProxyProperty);
            }
            set
            {
                this.SetValue(ProxyProperty, value);
            }
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            Proxy.Details.IsUpdating = true;

            try
            {
                using (TaskItem task = Context.Get<ITaskManager>().Create(Properties.Resources.ManualUpdateProxyType))
                {
                    Proxy.Details.Details = await Proxy.Details.UpdateMethod(Proxy, task, Proxy.Details.CancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                Proxy.Details.IsUpdating = false;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Proxy.Details.CancellationToken.Cancel();
        }
    }
}
