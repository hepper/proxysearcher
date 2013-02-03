namespace ProxySearch.Console.Code.ProxyClients.InternetExplorer.WinInet
{
    public enum INTERNET_OPTION
    {
        // Sets or retrieves an INTERNET_PER_CONN_OPTION_LIST structure that specifies
        // a list of options for a particular connection.
        INTERNET_OPTION_PER_CONNECTION_OPTION = 75,

        // Notify the system that the registry settings have been changed so that
        // it verifies the settings on the next call to InternetConnect.
        INTERNET_OPTION_SETTINGS_CHANGED = 39,

        // Causes the proxy data to be reread from the registry for a handle.
        INTERNET_OPTION_REFRESH = 37

    }
}
