using CommandLine;

namespace codecov
{
    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "Target file to upload.")]
        public string FilePath { get; set; }

        [Option('t', "token", HelpText = "Set the private repository token.")]
        public string Token { get; set; }
    }
}