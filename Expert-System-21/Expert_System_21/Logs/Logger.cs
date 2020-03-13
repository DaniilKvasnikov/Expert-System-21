using System;
using System.Collections.Generic;
using System.Text;

namespace Expert_System_21.Logs
{
    public static class Logger
    {
        public static bool Log;
        public static bool DebugMode { get; set; }

        public static void PrintResults(Dictionary<char, bool?> results, bool check)
        {
            Log = true;
            foreach (var result in results)
                LogString(result.Key + " : " + result.Value);
            var color = check ? ConsoleColor.Green : ConsoleColor.Red;
            if (DebugMode)
                LogString(check + ". Конец!", color);
            Log = false;
        }

        public static void LogString(string message, ConsoleColor? color = null)
        {
            if (!Log) return;
            Console.OutputEncoding = Encoding.UTF8;
            if (color != null)
                Console.ForegroundColor = color.Value;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}