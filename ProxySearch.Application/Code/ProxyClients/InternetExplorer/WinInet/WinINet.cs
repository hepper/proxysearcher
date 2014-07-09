using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace ProxySearch.Console.Code.ProxyClients.InternetExplorer.WinInet
{
    public static class WinINet
    {
        [DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool InternetSetOption(IntPtr handle, INTERNET_OPTION optionFlag, IntPtr lpBuffer, int size);

        [DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal extern static bool InternetQueryOption(IntPtr handle, INTERNET_OPTION optionFlag, ref INTERNET_PER_CONN_OPTION_LIST optionList, ref int size);

        public static void SetProxy(bool isProxyUsed, string proxyServer)
        {
            INTERNET_PER_CONN_OPTION[] options = new INTERNET_PER_CONN_OPTION[2];

            options[0] = new INTERNET_PER_CONN_OPTION();
            options[0].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS;
            options[0].Value.dwValue = isProxyUsed ? (int)INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_PROXY : (int)INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_DIRECT;

            options[1] = new INTERNET_PER_CONN_OPTION();
            options[1].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_SERVER;
            options[1].Value.pszValue = Marshal.StringToHGlobalAnsi(proxyServer);

            ExecuteAction(options, (optionList, size) =>
            {
                IntPtr intPtrStruct = Marshal.AllocCoTaskMem(size);
                Marshal.StructureToPtr(optionList, intPtrStruct, true);

                try
                {
                    return InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION, intPtrStruct, size);
                }
                finally
                {
                    Marshal.FreeCoTaskMem(intPtrStruct);
                }
            });

            InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        public static bool IsProxyUsed
        {
            get
            { 
                return (QueryOption(INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS).Value.dwValue & (int)INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_PROXY) == 
                                                                                                           (int)INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_PROXY;
            }
        }

        public static string ProxyIpPort
        {
            get
            {
                INTERNET_PER_CONN_OPTION option = QueryOption(INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_SERVER);

                return Marshal.PtrToStringAnsi(option.Value.pszValue);
            }
        }

        private static INTERNET_PER_CONN_OPTION QueryOption(INTERNET_PER_CONN_OptionEnum optionValue)
        {
            INTERNET_PER_CONN_OPTION[] options = new INTERNET_PER_CONN_OPTION[1];

            options[0].dwOption = (int)optionValue;

            ExecuteAction(options, (optionList, size) =>
            {
                bool result = InternetQueryOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION, ref optionList, ref size);

                if (result)
                {
                    IntPtr current = optionList.pOptions;

                    for (int i = 0; i < options.Length; i++)
                    {
                        options[i] = (INTERNET_PER_CONN_OPTION)Marshal.PtrToStructure(current, typeof(INTERNET_PER_CONN_OPTION));
                        current = (IntPtr)((int)current + Marshal.SizeOf(options[i]));
                    }
                }

                return result;
            });

            INTERNET_PER_CONN_OPTION option = options[0];
            return option;
        }

        private static void ExecuteAction(INTERNET_PER_CONN_OPTION[] options, Func<INTERNET_PER_CONN_OPTION_LIST, int, bool> action)
        {
            IntPtr buffer = Marshal.AllocCoTaskMem(options.Sum(item => Marshal.SizeOf(item)));
            IntPtr current = buffer;

            for (int i = 0; i < options.Length; i++)
            {
                Marshal.StructureToPtr(options[i], current, false);
                current = (IntPtr)((int)current + Marshal.SizeOf(options[i]));
            }

            INTERNET_PER_CONN_OPTION_LIST optionList = new INTERNET_PER_CONN_OPTION_LIST();

            optionList.pOptions = buffer;
            optionList.Size = Marshal.SizeOf(optionList);
            optionList.Connection = IntPtr.Zero;

            optionList.OptionCount = options.Length;
            optionList.OptionError = 0;
            int size = Marshal.SizeOf(optionList);

            try
            {
                if (!action(optionList, size))
                {
                    throw new ApplicationException("WinInet: action failed!");
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(buffer);
            }
        }
    }
}
