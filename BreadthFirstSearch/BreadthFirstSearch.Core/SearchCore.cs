using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BreadthFirstSearch.Model;

namespace BreadthFirstSearch.Core
{
    public class SearchCore
    {
        //vera77.com
        private string htmlCode;

        private Thread mainThread;

        private bool isStop = true;

        public void GetHtmlCode(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                htmlCode = client.DownloadString(url);
            }
        }

        public int SearchMatches(string SearchingString)
        {
            Regex regex = new Regex(SearchingString);
            MatchCollection matches = regex.Matches(htmlCode);
            return matches.Count;
        }

        public IList<string> SearchLinks(string url)
        {
            Regex regex = new Regex(@"https?://[\w\d\-_]+(\.[\w\d\-_]+)+[\w\d\-\.,@?^=%&amp;:/~\+#]*");
            MatchCollection matches = regex.Matches(htmlCode);
            return (from Match match in matches
                    select match.Value).ToList();
        }
        
        /// <summary>
        /// Инициализируем начало поиска
        /// </summary>
        /// <param name="query"></param>
        public void StartSearching(SearchQuery query)
        {
            mainThread = new Thread(SearchingProcess) { IsBackground = true };
            mainThread.Start(query);
        }

        /// <summary>
        /// Остановка поиска и сброс достигнутого прогресса
        /// </summary>
        public void StopSearching()
        {
            if (mainThread != null)
            {
                mainThread.Abort();
                mainThread = null;
            }
        }
        /// <summary>
        /// Поиск
        /// </summary>
        /// <param name="SearchQuery"></param>
        public void SearchingProcess(object query)
        {
            try
            {
                List<ThreadProccessor> ThreadsPool = new List<ThreadProccessor>();

                var searchQuery = (SearchQuery)query;

                if (query == null)
                {
                    throw new Exception("Error: invalid input parameters!");
                }
                isStop = false;

                for (int i = 0; i < searchQuery.ThreadCount; i++)
                {
                    ThreadsPool.Add(new ThreadProccessor(Search));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Search(object query)
        {

        }
    }
}
