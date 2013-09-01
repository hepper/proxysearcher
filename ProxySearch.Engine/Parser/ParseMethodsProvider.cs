using System;
using System.Collections.Generic;
using System.Linq;

namespace ProxySearch.Engine.Parser
{
    public class ParseMethodsProvider : IParseMethodsProvider
    {
        private Dictionary<string, RegexCompilerMethod> methods;

        public ParseMethodsProvider(IEnumerable<KeyValuePair<string, ParseDetails>> parseDetails)
        {
            methods = parseDetails.ToDictionary(pair => pair.Key, pair => new RegexCompilerMethod(pair.Value));
        }

        public IParseMethod GetMethod(Uri uri)
        {
            return methods.Last(pair => uri.ToString().Contains(pair.Key)).Value;
        }
    }
}
