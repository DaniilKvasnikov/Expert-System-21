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
            foreach (var result in results)
                LogStringWithoutCheck(result.Key + " : " + result.Value);
            var color = check ? ConsoleColor.Green : ConsoleColor.Red;
            if (DebugMode)
                LogStringWithoutCheck(check + ". Конец!", color);
        }

        public static void LogString(string message, ConsoleColor? color = null)
        {
            if (!Log) return;
            LogStringWithoutCheck(message, color);
        }
        
        public static void LogStringWithoutCheck(string message, ConsoleColor? color = null)
        {
            Console.OutputEncoding = Encoding.UTF8;
            if (color != null)
                Console.ForegroundColor = color.Value;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}