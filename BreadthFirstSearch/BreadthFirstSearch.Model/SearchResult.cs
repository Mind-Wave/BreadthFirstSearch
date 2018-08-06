﻿using System;
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

        public string CurrentUrl { get; set; }
    }
}
