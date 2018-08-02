using System;
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
            //1251 номер русской локализации
            //Console.InputEncoding = Encoding.UTF8     не работает
            Console.InputEncoding = Encoding.GetEncoding(1251);
            Console.OutputEncoding = Encoding.UTF8;

            Console.Write("Enter URL: ");
            string url = Console.ReadLine();

            SearchCore search = new SearchCore();
            search.GetHtmlCode(url);

            //Console.Write("Enter word to search: ");
            //string SearchingWord = Console.ReadLine();

            //search.SearchMatches(SearchingWord);

            search.SearchLinks(url);
        }
    }
}
