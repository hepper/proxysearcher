using System.Collections.Generic;
using ProxySearch.Common;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.SearchEngines.Google;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public abstract class GoogleEngineDetectableBase : DetectableBase<ISearchEngine, GoogleSearchEngine, GoogleEnginePropertyControl>
    {
        public GoogleEngineDetectableBase(string supportedProxyType, string defaultKeywords)
            : base(Resources.GoogleDotCom, Resources.GoogleEngineDescription, 0, new string[] { supportedProxyType },
                   new List<object> { 40, defaultKeywords })
        {
        }

        public override List<object> InterfaceSettings
        {
            get
            {
                return new List<object>()
                {
                    Context.Get<ICaptchaWindow>()
                };
            }
        }
    }
}
