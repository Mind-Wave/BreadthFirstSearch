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

        private readonly ManualResetEvent pauseResetEvent;

        private bool isStop = true;

        private object criticalSection = true;

        public string GetHtmlCode(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                htmlCode = client.DownloadString(url);
            }
            return htmlCode;
        }

        public int CountMatches(string SearchingString)
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
        /// Приостановить поиск
        /// </summary>
        public void Pause() => pauseResetEvent.Reset();

        /// <summary>
        /// Продолжить поиск
        /// </summary>
        public void Resume() => pauseResetEvent.Set();

        /// <summary>
        /// Поиск
        /// </summary>
        /// <param name="SearchQuery"></param>
        public void SearchingProcess(object query)
        {
            List<ThreadProccessor> threadsPool = null;
            try
            {
                threadsPool = new List<ThreadProccessor>();

                var searchQuery = (SearchQuery)query;

                if (query == null)
                {
                    throw new Exception("Error: invalid input parameters!");
                }
                isStop = false;
                
                for (int i = 0; i < searchQuery.ThreadCount; i++)
                {
                    threadsPool.Add(new ThreadProccessor(Search));
                }

                while (searchQuery.Scanning.Count > 0 && searchQuery.ScannedRef.Count <= searchQuery.SearchingDeep)
                {
                    pauseResetEvent.WaitOne();
                    searchQuery.Scanning = ScanLevel(searchQuery, threadsPool);
                }
            }
            catch (ThreadAbortException)
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isStop = true;

                if (threadsPool != null)
                {
                    foreach (ThreadProccessor thread in threadsPool)
                    {
                        thread.Stop();
                    }
                }
                mainThread = null;
            }
        }

        private List<string> ScanLevel(object query, List<ThreadProccessor> threadsPool)
        {
            var searchQuery = (SearchQuery)query;

            for (int i = 0; i < searchQuery.Scanning.Count; i++)
            {
                Tuple<string, List<string>, List<string>, string> tupleScanData =
                    new Tuple<string, List<string>, List<string>, string>(searchQuery.Scanning[i], searchQuery.ScannedRef, searchQuery.ScanNextRef, searchQuery.SearchingString);

                pauseResetEvent.WaitOne();

                threadsPool[i % threadsPool.Count].Start(tupleScanData);
            }

            foreach (ThreadProccessor thread in threadsPool)
            {
                thread.Join();
            }

            if (searchQuery.ScanNextRef.Count + searchQuery.ScannedRef.Count > searchQuery.SearchingDeep)
            {
                searchQuery.ScanNextRef.RemoveRange(0, searchQuery.ScanNextRef.Count + searchQuery.ScannedRef.Count - searchQuery.SearchingDeep);
            }

            return searchQuery.ScanNextRef;
        }

        public void Search(object query)
        {
            try
            {
                pauseResetEvent.WaitOne();

                var searchQuery = (SearchQuery)query;

                lock (criticalSection)
                {
                    searchQuery.ScannedRef.Add(searchQuery.CurrentUrl);
                }

                string html = GetHtmlCode(searchQuery.RootUrl).ToLower();

                int countMatches = CountMatches(searchQuery.SearchingString.ToLower());

                lock (criticalSection)
                {

                    //foreach (var match in SearchLinks(searchQuery.RootUrl))
                    //{
                    //    Console.WriteLine("Matches: " + countMatches);

                    //    Console.WriteLine(match);
                    //}
                    
                }

                lock (criticalSection)
                {
                    searchQuery.ScanNextRef.AddRange(SearchLinks(searchQuery.CurrentUrl));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
