using System;
using System.Collections.Generic;
using Expert_System_21.Nodes;

namespace Expert_System_21.Logs
{
    public static class Logger
    {
        public static bool Log;
        public static void PrintResults(Dictionary<char, bool?> results, bool check)
        {
            Log = true;
            foreach (var result in results)
                LogString(result.Key + " : " + result.Value);
            Console.ForegroundColor = check ? ConsoleColor.Green : ConsoleColor.Red;
            LogString(check + ". The end!");
            Console.ResetColor();
            Log = false;
        }

        public static void InitFact(AtomNode atom)
        {
            LogString(atom + " init " + atom.State);
        }

        public static void LogString(string message, int tabCount = 0)
        {
            if (!Log) return;
            while (--tabCount >= 0)
            {
                Console.Write('\t');
            }
            Console.WriteLine(message);
        }
    }
}