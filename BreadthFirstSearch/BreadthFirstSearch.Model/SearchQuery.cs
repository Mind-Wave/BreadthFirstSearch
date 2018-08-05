using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadthFirstSearch.Model
{
    public class SearchQuery
    {
        /// <summary>
        /// URL, с которого начинается поиск
        /// </summary>
        public string RootUrl { get; set; }
        /// <summary>
        /// Строка, для которой нужно найти совпадения
        /// </summary>
        public string SearchingString { get; set; }
        /// <summary>
        /// Глубина поиска (количество ссылок, по которым будет произведён поиск)
        /// </summary>
        public int SearchingDeep { get; set; }
        /// <summary>
        /// Количество потоков, заданных пользователем
        /// </summary>
        public int ThreadCount { get; set; }

        public string CurrentUrl { get; set; }

        public List<string> Scanning { get; set; }

        public List<string> ScannedRef { get; set; }

        public List<string> ScanNextRef { get; set; }
    }
}
