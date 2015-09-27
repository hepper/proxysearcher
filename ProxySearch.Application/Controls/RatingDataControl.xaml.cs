using System.Windows;
using System.Windows.Controls;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Converters;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Ratings;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for RatingDataControl.xaml
    /// </summary>
    public partial class RatingDataControl : UserControl
    {
        public static readonly DependencyProperty ProxyInfoProperty = DependencyProperty.Register("ProxyInfo", typeof(ProxyInfo), typeof(RatingDataControl));

        public ProxyInfo ProxyInfo
        {
            get
            {
                return (ProxyInfo)GetValue(ProxyInfoProperty);
            }
            set
            {
                SetValue(ProxyInfoProperty, value);
            }
        }

        public RatingDataControl()
        {
            InitializeComponent();
        }

        private async void RatingValueChangedHandler(object sender, RatingControl.RatingValueChangedEventArgs e)
        {
            ProxyInfo.RatingData.State = RatingState.Updating;
            progressBar.IsIndeterminate = true;

            ratingControl.GetBindingExpression(Control.ToolTipProperty).UpdateTarget();

            try
            {
                ProxyInfo.RatingData = await Context.Get<IRatingManager>().UpdateRatingDataAsync(ProxyInfo, e.NewValue != 0 ? e.NewValue : default(int?));
                ratingControl.RatingValue = (int)new RatingValueConverter().Convert(ProxyInfo.RatingData, null, null, null);
            }
            finally
            {
                progressBar.IsIndeterminate = false;
                ratingControl.GetBindingExpression(Control.ToolTipProperty).UpdateTarget();
            }
        }
    }
}