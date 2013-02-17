using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ProxySearch.Console.Code;

namespace ProxySearch.Console
{
    /// <summary>
    /// Interaction logic for DownloadNewVersion.xaml
    /// </summary>
    public partial class DownloadNewVersion : Window
    {
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();
        private bool downloaded = false;

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

                Process uninstall = Process.Start(Environment.SystemDirectory + "\\MsiExec.exe", "/x{EFD8FA84-F3A5-4DF8-999C-7C035BFFD578} /passive");
                uninstall.WaitForExit();

                Process.Start(file);

                downloaded = true;
                Application.Current.Shutdown();
            }
            catch (TaskCanceledException)
            {
                Close();
            }
            catch (Exception)
            {
                Close();
                MessageBox.Show(Properties.Resources.CannotUpdateProgram, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<string> DownloadInstallation(string loadPath)
        {
            string filePath = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), "exe");

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
                progressBar.Value = (100 * e.BytesTransferred) / e.TotalBytes.Value;
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Cancel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
