using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BreadthFirstSearch.Core;
using BreadthFirstSearch.Model;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.UTF8;

            SearchCore searchCore = new SearchCore(ResultMethod);
            SearchQuery searchQuery = new SearchQuery
            {
                RootUrl = "http://www.vera77.com",
                SearchingDeep = 5,
                ThreadCount = 5,
                SearchingString = "Бог"
            };

            searchCore.StartSearching(searchQuery);
        }

        private static void ResultMethod(SearchResult searchResult)
        {
            Console.WriteLine($"URL: {searchResult.CurrentUrl} Found entries: {searchResult.CountMatches}");
        }
    }
}
