using System;

namespace Codecov.Program
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
                Log.Verbose(e.Message);
                Log.Verbose(e.StackTrace);
                return kill;
            }
        }
    }
}