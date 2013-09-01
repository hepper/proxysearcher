using System;

namespace ProxySearch.Engine.Parser
{
    public interface IParseMethodsProvider
    {
       IParseMethod GetMethod(Uri uri);
    }
}
