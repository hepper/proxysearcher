using System;
using System.Runtime.InteropServices;

namespace ProxySearch.Console.Code.ProxyClients.InternetExplorer.WinInet
{
    public static class WinINet
    {
        [DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool InternetSetOption(IntPtr hInternet, INTERNET_OPTION dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        [DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal extern static bool InternetQueryOption(IntPtr hInternet, INTERNET_OPTION dwOption, ref INTERNET_PER_CONN_OPTION_LIST OptionList, ref int lpdwBufferLength);

        public static void SetProxy(bool useProxy, string proxyServer)
        {
            INTERNET_PER_CONN_OPTION[] Options = new INTERNET_PER_CONN_OPTION[2];

            Options[0] = new INTERNET_PER_CONN_OPTION();
            Options[0].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS;
            Options[0].Value.dwValue = useProxy ? (int)INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_PROXY : (int)INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_DIRECT;

            Options[1] = new INTERNET_PER_CONN_OPTION();
            Options[1].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_SERVER;
            Options[1].Value.pszValue = Marshal.StringToHGlobalAnsi(proxyServer);

            IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(Options[0]) + Marshal.SizeOf(Options[1]));
            IntPtr current = buffer;

            for (int i = 0; i < Options.Length; i++)
            {
                Marshal.StructureToPtr(Options[i], current, false);
                current = (IntPtr)((int)current + Marshal.SizeOf(Options[i]));
            }

            INTERNET_PER_CONN_OPTION_LIST option_list = new INTERNET_PER_CONN_OPTION_LIST();

            option_list.pOptions = buffer;
            option_list.Size = Marshal.SizeOf(option_list);
            option_list.Connection = IntPtr.Zero;

            option_list.OptionCount = Options.Length;
            option_list.OptionError = 0;
            int size = Marshal.SizeOf(option_list);

            IntPtr intptrStruct = Marshal.AllocCoTaskMem(size);
            Marshal.StructureToPtr(option_list, intptrStruct, true);

            bool bReturn = InternetSetOption(
                IntPtr.Zero,
                INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
                intptrStruct, size);

            Marshal.FreeCoTaskMem(buffer);
            Marshal.FreeCoTaskMem(intptrStruct);

            if (!bReturn)
            {
                throw new ApplicationException(" Set Internet Option Failed!");
            }

            InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }
    }
}
