using System;
using Microsoft.Win32;

namespace ProxySearch.Console.Code.Settings
{
    public class RegistrySettings
    {
        public RegistrySettings()
        {
            if (!IsKeyExists(Constants.RegistrySettings.ClientId))
            {
                SetValue(Constants.RegistrySettings.ClientId, Guid.NewGuid().ToString());
            }
        }

        public string ClientId
        {
            get
            {
                return GetValue<string>(Constants.RegistrySettings.ClientId);
            }
        }

        private bool IsKeyExists(string name)
        {
            using (RegistryKey key = GetOrCreateKey())
            {
                return key.GetValue(name) != null;
            }
        }

        private T GetValue<T>(string name)
        {
            using (RegistryKey key = GetOrCreateKey())
            {
                return (T)key.GetValue(name);
            }
        }

        private void SetValue<T>(string name, T value)
        {
            using (RegistryKey key = GetOrCreateKey())
            {
                key.SetValue(name, value);
            }
        }

        private RegistryKey GetOrCreateKey()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(Constants.RegistrySettings.Location, true);

            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey(Constants.RegistrySettings.Location);
            }

            return key;
        }
    }
}
