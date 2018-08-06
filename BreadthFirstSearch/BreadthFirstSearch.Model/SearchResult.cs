using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadthFirstSearch.Model
{
    public class SearchResult
    {
        /// <summary>
        /// Кол-во найденных совпадений
        /// </summary>
        public int CountMatches { get; set; }
        /// <summary>
        /// URL скнируемый в данный момент
        /// </summary>
        public string CurrentUrl { get; set; }
        /// <summary>
        /// Print error messaging
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
