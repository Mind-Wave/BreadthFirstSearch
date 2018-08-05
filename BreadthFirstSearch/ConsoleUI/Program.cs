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

            //Console.Write("Enter URL: ");
            //string url = Console.ReadLine();

            SearchCore searchCore = new SearchCore();
            SearchQuery searchQuery = new SearchQuery();

            searchQuery.RootUrl = "https://www.wikipedia.org/";
            searchQuery.SearchingDeep = 50;
            searchQuery.ThreadCount = 20;
            searchQuery.SearchingString = "Articles";

            searchCore.StartSearching(searchQuery);
            //searchCore.GetHtmlCode(searchQuery.RootUrl);
            //foreach (var match in searchCore.SearchLinks(searchQuery.RootUrl))
            //{
            //    Console.WriteLine(match);
            //}
            //searchCore.SearchLinks("https://www.wikipedia.org/");

        }
    }
}
