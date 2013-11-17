using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ProxySearch.Common;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Code.Language
{
    public class LanguageManager
    {
        public List<Language> SupoportedLanguages
        {
            get
            {
                return new List<Language>()
                {
                    new Language
                    {
                        Name = "English",
                        Culture = "en-US"
                    },
                    new Language
                    {
                        Name = "Русский",
                        Culture = "ru-RU"
                    }
                };
            }
        }

        public Language DefaultLanguage
        {
            get 
            {
                return SupoportedLanguages.FirstOrDefault(language => language.Culture == CultureInfo.InstalledUICulture.Name) 
                                          ?? SupoportedLanguages.First();
            }
        }

        public Language CurrentLanguage
        {
            get
            {
                return SupoportedLanguages.FirstOrDefault(language => language.Culture == Context.Get<AllSettings>()
                                                                                                 .SelectedCulture) 
                                          ?? DefaultLanguage;
            }
        }
    }
}
