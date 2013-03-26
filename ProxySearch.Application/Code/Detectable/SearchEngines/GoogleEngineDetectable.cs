using System.Collections.Generic;
using ProxySearch.Common;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.SearchEngines.Google;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class GoogleEngineDetectable : DetectableBase<ISearchEngine, GoogleSearchEngine, GoogleEnginePropertyControl>
    {
        public GoogleEngineDetectable()
            : base(Resources.GoogleDotCom, Resources.GoogleEngineDescription, 0, new List<object> { 40, "http proxy list 3128" })
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
