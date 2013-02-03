using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProxySearch.Console.Code.ProxyClients.Opera
{
    public class IniFile
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        public static void WriteValue(string path, string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, path);
        }

        public static string ReadValue(string path, string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, string.Empty, temp, 255, path);
            return temp.ToString();
        }        
    }
}
