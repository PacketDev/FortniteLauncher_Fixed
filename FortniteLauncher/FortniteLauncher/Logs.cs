using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortniteLauncher.FortniteLauncher.Logs
{
    class Log
    {
        public static void Logs(string stuff)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"LOGS: {stuff}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Warning(string stuff)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"WARNING: {stuff}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Error(string stuff)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {stuff}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

