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
        private string htmlCode;

        private Thread mainThread;

        private readonly ManualResetEvent pauseResetEvent;

        private bool isStop = true;

        private object criticalSection = true;

        private Action<SearchResult> receiveCallbackAction;

        private SearchCore()
        {
            pauseResetEvent = new ManualResetEvent(true);
        }

        public SearchCore(Action<SearchResult> callback) : this()
        {
            receiveCallbackAction = callback;
        }

        /// <summary>
        /// Инициализируем начало поиска
        /// </summary>
        /// <param name="query"></param>
        public void StartSearching(SearchQuery query)
        {
            mainThread = new Thread(SearchingProcess) { IsBackground = false };
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
        private void SearchingProcess(object query)
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


                List<string> scanned = new List<string>();
                List<string> scan = new List<string> { searchQuery.RootUrl };

                while (scan.Count > 0 && scanned.Count <= searchQuery.SearchingDeep)
                {
                    pauseResetEvent.WaitOne();
                    searchQuery.ScannedRef = scanned;
                    searchQuery.Scanning = scan;
                    searchQuery.ScanNextRef = new List<string>();

                    searchQuery.Scanning = ScanLevel(searchQuery, threadsPool);
                }
            }
            catch (ThreadAbortException tae)
            {
                //igonring this shit
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
                StopSearching();
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

        private void Search(object query)
        {
            try
            {
                pauseResetEvent.WaitOne();

                var searchQuery = query as Tuple<string, List<string>, List<string>, string>;

                string currentUrl = searchQuery.Item1;
                List<string> scannedRef = searchQuery.Item2;
                List<string> scanNext = searchQuery.Item3;
                string searchString = searchQuery.Item4;

                string html = GetHtmlCode(currentUrl).ToLower();

                int countMatches = CountMatches(searchString.ToLower());

                SearchResult searchResult = new SearchResult
                {
                    CurrentUrl = currentUrl,
                    CountMatches = countMatches
                };

                lock (criticalSection)
                {
                    scannedRef.Add(currentUrl);
                }

                lock (criticalSection)
                {
                    receiveCallbackAction(searchResult);
                }

                lock (criticalSection)
                {
                    scanNext.AddRange(from string match in SearchLinks(currentUrl)
                                      where !scannedRef.Contains(match)
                                      select match);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetHtmlCode(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                htmlCode = client.DownloadString(url);
            }
            return htmlCode;
        }

        private int CountMatches(string SearchingString)
        {
            Regex regex = new Regex(SearchingString);
            MatchCollection matches = regex.Matches(htmlCode);
            return matches.Count;
        }

        private IList<string> SearchLinks(string url)
        {
            Regex regex = new Regex(@"https?://[\w\d\-_]+(\.[\w\d\-_]+)+[\w\d\-\.,@?^=%&amp;:/~\+#]*");
            MatchCollection matches = regex.Matches(htmlCode);
            return (from Match match in matches
                    select match.Value).ToList();
        }

    }
}
