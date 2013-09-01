using System;
using System.Collections.Generic;
using System.Linq;

namespace ProxySearch.Engine.Parser
{
    public class ParseMethodsProvider : IParseMethodsProvider
    {
        private List<KeyValuePair<string, RegexCompilerMethod>> methods;

        public ParseMethodsProvider(IEnumerable<ParseDetails> parseDetailsCollection)
        {
            methods = parseDetailsCollection.Select(details =>new KeyValuePair<string, RegexCompilerMethod>(details.Url, new RegexCompilerMethod(details))).ToList();
        }

        public IParseMethod GetMethod(Uri uri)
        {
            return methods.Last(pair => uri.ToString().Contains(pair.Key)).Value;
        }
    }
}
