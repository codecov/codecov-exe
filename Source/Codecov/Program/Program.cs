namespace Codecov.Program
{
    /// <summary>
    /// Main entry point for program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point for program.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>0 if successfull and 1 otherwise.</returns>
        public static int Main(string[] args)
        {
            try
            {
                return Run.Runner(args);
            }
            catch
            {
                return 0;
            }
        }
    }
}
