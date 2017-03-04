using System;

namespace codecov.Program
{
    public static class Log
    {
        internal static bool IsVerboseMode { get; set; }

        public static void Arrow(string message)
        {
            Console.WriteLine($"==> {message}");
        }

        public static void Message(string message)
        {
            Console.WriteLine($"    {message}");
        }

        public static void Verbose(string message)
        {
            if (IsVerboseMode)
            {
                Console.WriteLine("\n====");
                Console.WriteLine($"{message}");
                Console.WriteLine("====\n");
            }
        }

        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public static void X(string message)
        {
            Console.WriteLine($"x>  {message}");
        }
    }
}