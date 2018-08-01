using System;
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

        public void SearchMatches(string SearchingWord)
        {
            Regex regex = new Regex(SearchingWord);
            MatchCollection matches = regex.Matches(htmlCode);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Value);
                }
                Console.WriteLine(matches.Count);
            }
            else
                Console.WriteLine("No matches!");
        }
    }
}
