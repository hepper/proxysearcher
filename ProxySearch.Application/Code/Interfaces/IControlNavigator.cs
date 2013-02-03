using System.Windows.Controls;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IControlNavigator
    {
        void GoTo(UserControl control);
        void GoToSearch();
    }
}
