﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreadthFirstSearch.Core;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Console.Write("Enter URL: ");
            string url = Console.ReadLine();

            SearchCore search = new SearchCore();
            search.GetHtmlCode(url);

            Console.Write("Enter word to search: ");
            string SearchingWord = Console.ReadLine();

            search.SearchMatches(SearchingWord);
        }
    }
}
