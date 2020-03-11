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
            var color = check ? ConsoleColor.Green : ConsoleColor.Red;
            LogString(check + ". The end!", color: color);
            Log = false;
        }

        public static void InitFact(AtomNode atom)
        {
            LogString(atom + " init " + atom.State);
        }

        public static void LogString(string message, int tabCount = 0, ConsoleColor? color = null)
        {
            if (!Log) return;
            while (--tabCount >= 0) Console.Write('\t');
            if (color != null)
                Console.ForegroundColor = color.Value;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}