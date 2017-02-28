using System;
using System.Reflection;
using codecov.Coverage;
using codecov.Services.Utils;

namespace codecov.Program
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            var kill = false;
            try
            {
                var options = Cli.GetOptions(args);

                Log.IsVerboseMode = options.Verbose; // Set verbose mode
                kill = options.Required;
                Log.Message($@"
              _____          _
             / ____|        | |
            | |     ___   __| | ___  ___ _____   __
            | |    / _ \ / _  |/ _ \/ __/ _ \ \ / /
            | |___| (_) | (_| |  __/ (_| (_) \ V /
             \_____\___/ \____|\___|\___\___/ \_/
                                         exe-{Assembly.GetExecutingAssembly().GetName().Version}
            ");
                var codeCovYml = new CodecovYml(options.Root);
                var service = ServiceFactory.CreateService; // Get Service
                var report = Report.Reporter(options.File, options.Env, options.Root); // Get Report

                // Only show what would be uploaded
                if (options.Dump)
                {
                    Log.ConsoleWriteLine($"\n{report}");
                    return 0;
                }

                // Upload report using V4 api
                var v4 = new Upload();
                var url = Url.GetUrl(options.Url, codeCovYml.Url, service.CreateQuery(options, codeCovYml));
                v4.Uploader(report, url);

                return 0;
            }
            catch (Exception e)
            {
                Log.Verbose(e.Message);
                Log.Verbose(e.StackTrace);
                return kill ? 1 : 0;
            }
        }
    }
}