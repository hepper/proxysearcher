using System;
using System.Runtime.InteropServices;

namespace ProxySearch.Console.Code.ProxyClients.InternetExplorer.WinInet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct INTERNET_PER_CONN_OPTION_LIST
    {
        public int Size;
        public IntPtr Connection;        
        public int OptionCount;
        public int OptionError;
        public IntPtr pOptions;
    }
}
