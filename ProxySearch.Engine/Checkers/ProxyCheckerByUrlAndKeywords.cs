using System.Threading.Tasks;
using ProxySearch.Common;
using System.Linq;

namespace ProxySearch.Engine.Checkers
{
    public class ProxyCheckerByUrlAndKeywords : ProxyCheckerBase
    {
        private string Url
        {
            get;
            set;
        }

        private string[] Keywords
        {
            get;
            set;
        }

        public ProxyCheckerByUrlAndKeywords(string url, string keywords)
        {
            Url = url;
            Keywords = keywords.Split(' ');
        }

        protected override async Task<bool> Alive(ProxyInfo info)
        {
            string content = await Context.Get<CheckerUtils>().GetContent(Url, info);

            return !Keywords.Any(item => !content.Contains(item));
        }
    }
}
