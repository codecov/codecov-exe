using codecov.Coverage;
using codecov.Services.Utils;

namespace codecov.Program
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var options = Cli.GetOptions(args); // Get options
            Log.IsVerboseMode = options.Verbose; // Set verbose mode
            Log.Message(@"
              _____          _
             / ____|        | |
            | |     ___   __| | ___  ___ _____   __
            | |    / _ \ / _  |/ _ \/ __/ _ \ \ / /
            | |___| (_) | (_| |  __/ (_| (_) \ V /
             \_____\___/ \____|\___|\___\___/ \_/
            ");
            var codeCovYml = new CodecovYml(options.Root);
            var service = ServiceFactory.CreateService; // Get Service
            var report = Report.Reporter(options.File, options.Env, options.Root); // Get Report

            // Only show what would be uploaded
            if (options.Dump)
            {
                Log.ConsoleWriteLine(report);
                return;
            }

            // Upload report using V4 api
            var v4 = new Upload();
            var url = Url.GetUrl(options.Url, codeCovYml.Url, service.CreateQuery(options, codeCovYml));
            v4.Uploader(report, url);
        }
    }
}