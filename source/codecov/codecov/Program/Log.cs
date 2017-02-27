using System;

namespace codecov.Program
{
    public static class Log
    {
        internal static bool IsVerboseMode { get; set; }

        public static void ConsoleWriteLine(string message)
        {
            Console.WriteLine("{message}");
        }

        public static void Information(string message)
        {
            ConsoleWriteLine($"==>{message}");
        }

        public static void Message(string message)
        {
            ConsoleWriteLine("\t{message}");
        }

        public static void Verbose(string message)
        {
            if (IsVerboseMode)
            {
                Message(message);
            }
        }
    }
}