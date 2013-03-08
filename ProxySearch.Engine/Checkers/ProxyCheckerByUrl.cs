using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Checkers
{
    public class ProxyCheckerByUrl : CheckerProxyBase
    {
        private string Url
        {
            get;
            set;
        }

        private double Accuracy
        {
            get;
            set;
        }

        private Dictionary<char, int> AnalyzedText
        {
            get;
            set;
        }

        public ProxyCheckerByUrl(string url, double accuracy)
        {
            Url = url;
            Accuracy = accuracy;

            string content = Context.Get<CheckerUtils>().GetContent(Url, null).GetAwaiter().GetResult();

            if (content == null)
            {
                throw new InvalidOperationException(string.Format(Resources.CannotDownloadContent, Url));
            }

            AnalyzedText = AnalyzeText(content);
        }

        protected override async Task<bool> Alive(ProxyInfo info)
        {
            try
            {
                string content = await Context.Get<CheckerUtils>().GetContent(Url, info);

                if (content == null)
                {
                    return false;
                }

                return Compare(AnalyzedText, AnalyzeText(content)) <= Accuracy;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        private Dictionary<char, int> AnalyzeText(string text)
        {
            Dictionary<char, int> result = new Dictionary<char, int>();

            foreach (char symbol in text)
            {
                if (result.ContainsKey(symbol))
                {
                    result[symbol]++;
                }
                else
                {
                    result.Add(symbol, 1);
                }
            }

            return result;
        }

        private double Compare(Dictionary<char, int> dictionary1, Dictionary<char, int> dictionary2)
        {
            int result = 0;

            foreach (char key in dictionary1.Keys.Union(dictionary2.Keys).Distinct().ToList())
            {
                int count1;
                int count2;

                bool get1 = dictionary1.TryGetValue(key, out count1);
                bool get2 = dictionary2.TryGetValue(key, out count2);

                if (get1 && get2)
                {
                    result += Math.Abs(count1 - count2);
                }
                else if (get1)
                {
                    result += count1;
                }
                else if (get2)
                {
                    result += count2;
                }
            }

            return (double)result / dictionary1.Sum(item => item.Value);
        }
    }
}
