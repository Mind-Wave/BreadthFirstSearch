using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BreadthFirstSearch.Core
{
    public class SearchCore
    {
        //vera77.com
        private string htmlCode;

        public void GetHtmlCode(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                htmlCode = client.DownloadString(url);
            }
        }

        public IEnumerable SearchMatches(string SearchingWord)
        {
            Regex regex = new Regex(SearchingWord);
            MatchCollection matches = regex.Matches(htmlCode);
            if (matches.Count > 0)
                return matches;
            else
                return null;
        }

        public IEnumerable SearchLinks(string url)
        {
            Regex regex = new Regex(@"https?://[\w\d\-_]+(\.[\w\d\-_]+)+[\w\d\-\.,@?^=%&amp;:/~\+#]*");
            MatchCollection matches = regex.Matches(htmlCode);
            if (matches.Count > 0)
                return matches;
            else
                return null;
        }
    }
}
