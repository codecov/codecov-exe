namespace codecov.Program
{
    internal static class Cli
    {
        internal static Options GetOptions(string[] args)
        {
            var options = new Options();
            CommandLine.Parser.Default.ParseArgumentsStrict(args, options);
            return options;
        }
    }
}