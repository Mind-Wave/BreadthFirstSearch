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
                RootUrl = "https://stackoverflow.com/",
                SearchingDeep = 5,
                ThreadCount = 5,
                SearchingString = "how"
            };

            searchCore.StartSearching(searchQuery);
            Console.ReadKey();
        }

        private static void ResultMethod(SearchResult searchResult)
        {
            Console.WriteLine($"URL: {searchResult.CurrentUrl} Found entries: {searchResult.CountMatches} {searchResult.ErrorMessage ?? string.Empty}");
        }
    }
}
