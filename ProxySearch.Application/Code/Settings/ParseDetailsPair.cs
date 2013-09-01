using System;
using ProxySearch.Engine.Parser;

namespace ProxySearch.Console.Code.Settings
{
    [Serializable]
    public class ParseDetailsPair
    {
        public string Url { get; set; }
        public ParseDetails Details { get; set; }
    }
}
