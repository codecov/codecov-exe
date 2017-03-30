using CommandLine;

namespace Codecov.Program
{
    internal static class Cli
    {
        internal static Options GetOptions(string[] args)
        {
            var result = (Parsed<Options>)Parser.Default.ParseArguments<Options>(args);
            return result.Value;
        }
    }
}