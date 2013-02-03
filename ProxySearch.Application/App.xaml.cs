using System;
using System.Windows;
using System.Windows.Threading;
using ProxySearch.Common;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       public App()
        {
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            this.Startup += new StartupEventHandler(App_Startup);
            this.Exit += new ExitEventHandler(App_Exit);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            new ApplicationInitializer().Initialize();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            new ApplicationInitializer().Deinitialize();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (Context.IsSet<IActionInvoker>() && Application.Current != null && Application.Current.MainWindow.IsVisible)
            {
                Context.Get<IActionInvoker>().SetException(e.Exception);
            }
            else
            {
                if (Application.Current == null)
                    ShowException(null, e.Exception);
                else
                {
                    ShowException(Application.Current.MainWindow, e.Exception);
                    Application.Current.Shutdown();
                }
            }

            e.Handled = true;
        }

        public static void ShowException(Window owner, Exception exception)
        {
            if (owner == null)
                MessageBox.Show(exception.Message, ProxySearch.Console.Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show(owner, exception.Message, ProxySearch.Console.Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
