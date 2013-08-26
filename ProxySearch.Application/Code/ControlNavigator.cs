using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Controls;

namespace ProxySearch.Console.Code
{
    public class ControlNavigator : IControlNavigator
    {
        private SearchControl searchControl = new SearchControl();

        private ContentControl Placeholder
        {
            get;
            set;
        }

        public ControlNavigator(ContentControl placeholder)
        {
            Placeholder = placeholder;
            Placeholder.Content = searchControl;
        }

        public void GoTo(UserControl control)
        {
            Placeholder.Content = control;
            Context.Get<IGA>().TrackPageViewAsync(control.GetType().Name);
        }

        public void GoToSearch()
        {
            Placeholder.Content = searchControl;
            Context.Get<IGA>().TrackPageViewAsync(searchControl.GetType().Name);
        }
    }
}
