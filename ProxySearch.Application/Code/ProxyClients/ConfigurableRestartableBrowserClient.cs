using System.IO;
namespace ProxySearch.Console.Code.ProxyClients
{
    public abstract class ConfigurableRestartableBrowserClient : RestartableBrowserClient
    {
        private string backupPath;

        public ConfigurableRestartableBrowserClient(string name, string image, int order, string clientName, string processName, string backupPath)
            : base(name, image, order, clientName, processName)
        {
            this.backupPath = backupPath;
        }

        protected override SettingsData BackupSettings()
        {
            if (File.Exists(SettingsPath))
            {
                File.Copy(SettingsPath, backupPath, true);
            }

            return new SettingsData();
        }

        protected override void RestoreSettings(SettingsData settings)
        {
            if (File.Exists(backupPath))
            {
                File.Copy(backupPath, SettingsPath, true);
                File.Delete(backupPath);
            }
        }

        protected abstract string SettingsPath { get; }
    }
}
