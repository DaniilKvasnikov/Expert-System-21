using System;
using System.Collections.Generic;

namespace ExpertSystemTests.ExpertSystem.Log
{
    public static class Log
    {
        public static void PrintResults(Dictionary<char, bool?> results, bool check)
        {
            foreach (var result in results)
                Console.WriteLine(result.Key + " : " + result.Value);
            Console.ForegroundColor = check ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(check + ". The end!");
            Console.ResetColor();;
        }
    }
}