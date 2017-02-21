using CommandLine;

namespace codecov.Program
{
    public class Options
    {
        [Option('d', "dump", HelpText = "Don't upload and dump to stdin.")]
        public string Dump { get; set; }

        [OptionArray('f', "file", Required = true, HelpText = "Target file(s) to upload.")]
        public string[] File { get; set; }

        [Option('t', "token", HelpText = "Set the private repository token.")]
        public string Token { get; set; }
    }
}