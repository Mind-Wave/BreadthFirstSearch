using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BreadthFirstSearch.Core;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.InputEncoding = Encoding.UTF8     не работает
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.UTF8;

            Console.Write("Enter URL: ");
            string url = Console.ReadLine();

            SearchCore search = new SearchCore();
            search.GetHtmlCode(url);

            //foreach (Match match in search.SearchMatches("Бог"))
            //{
            //    Console.WriteLine(match.Value);
            //}

            foreach (Match match in search.SearchLinks(url))
            {
                Console.WriteLine(match.Value);
            }
        }
    }
}
