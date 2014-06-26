using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console
{
    /// <summary>
    /// Interaction logic for DownloadNewVersion.xaml
    /// </summary>
    public partial class DownloadNewVersion : Window
    {
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();
        private bool downloaded = false;
        private bool closeButtonPressed = false;

        public DownloadNewVersion(string installPath)
        {
            InitializeComponent();

            Begin(installPath);
        }

        private async void Begin(string installPath)
        {
            Application.Current.MainWindow.Hide();
            Focus();

            try
            {
                string file = await DownloadInstallation(installPath);

                Title = Properties.Resources.Uninstalling;

                Process.Start(file);

                downloaded = true;
                Application.Current.Shutdown();
            }
            catch (Exception exception)
            {
                Close();

                if ( !(exception is TaskCanceledException) || !closeButtonPressed)
                {
                    if (Context.Get<IMessageBox>().OkCancelQuestion(Properties.Resources.CannotUpdateProgram) == MessageBoxResult.OK)
                    {
                        Process.Start(Properties.Resources.HomePageLink);
                        Application.Current.Shutdown();
                    }
                }
            }
        }

        private async Task<string> DownloadInstallation(string loadPath)
        {
            string filePath = Path.ChangeExtension(Path.GetTempFileName(), "exe");

            using (HttpClientHandler handler = new HttpClientHandler())
            using (ProgressMessageHandler progressMessageHandler = new ProgressMessageHandler(handler))
            {
                progressMessageHandler.HttpReceiveProgress += UpdateProgress;

                using (HttpClient client = new HttpClient(progressMessageHandler))
                using (HttpResponseMessage response = await client.GetAsync(loadPath, cancellationToken.Token))
                {
                    response.EnsureSuccessStatusCode();
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    using (FileStream file = File.OpenWrite(filePath))
                    {
                        await stream.CopyToAsync(file);
                    }
                }
            }

            return filePath;
        }

        private void UpdateProgress(object sender, HttpProgressEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (e.TotalBytes.HasValue)
                {
                    progressBar.Value = (100 * e.BytesTransferred) / e.TotalBytes.Value;
                }
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Cancel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            closeButtonPressed = true;
            Close();
        }

        private void Cancel()
        {
            if (!downloaded)
            {
                cancellationToken.Cancel();

                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.Focus();
            }
        }
    }
}
