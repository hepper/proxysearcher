using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using ProxySearch.Common;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.UI;

namespace ProxySearch.Console
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool CloseApplication
        {
            get;
            set;
        }

        public App()
        {
            Context.Set<IMessageBox>(new MessageBoxWrapper());

            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            this.Startup += new StartupEventHandler(App_Startup);
            this.Exit += new ExitEventHandler(App_Exit);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Any(item => item == ProxySearch.Console.Properties.Resources.FirstRunArgument))
            {
                try
                {
                    if (File.Exists(Constants.SettingsStorage.Location))
                        File.Delete(Constants.SettingsStorage.Location);
                }
                catch
                {
                }
            }

            CloseApplication = e.Args.Any(item => item == ProxySearch.Console.Properties.Resources.ShutdownArgument);

            if (CloseApplication)
            {
                Shutdown();
            }

            new ApplicationInitializer().Initialize(CloseApplication);
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            if (!CloseApplication)
            {
                new ApplicationInitializer().Deinitialize();
            }
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
                Context.Get<IMessageBox>().Error(exception.Message);
            else
                Context.Get<IMessageBox>().Error(owner, exception.Message);
        }
    }
}
