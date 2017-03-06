using System;

namespace codecov.Program
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            var kill = 0;
            try
            {
                var run = new Run(args);
                kill = run.SuccessIsRequired;
                run.Runner();
                return 0;
            }
            catch (Exception e)
            {
                Log.X("An error occured while running. You can run the program in verbose mode to see it using the flag --verbose.");
                Log.Verbose(e.Message);
                Log.Verbose(e.StackTrace);
                return kill;
            }
        }
    }
}