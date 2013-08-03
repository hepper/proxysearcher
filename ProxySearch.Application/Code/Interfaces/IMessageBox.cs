using System.Windows;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IMessageBox
    {
        void Information(string message);
        void Error(string message);
        void Error(Window owner, string message);
        MessageBoxResult YesNoQuestion(string message);
        MessageBoxResult OkCancelQuestion(string message);
    }
}
