using System.Windows;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.UI
{
    public class MessageBoxWrapper : IMessageBox
    {
        public void Information(string message)
        {
            MessageBox.Show(message, Resources.Information, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Error(string message)
        {
            MessageBox.Show(message, Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void Error(Window owner, string message)
        {
            MessageBox.Show(owner, message, Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public MessageBoxResult YesNoQuestion(string message)
        {
            return MessageBox.Show(message, Resources.Question, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public MessageBoxResult OkCancelQuestion(string message)
        {
            return MessageBox.Show(message, Resources.Question, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }
    }
}
