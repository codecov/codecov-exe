using codecov.Coverage;
using codecov.Services.Utils;

namespace codecov.Program
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            CommandLine.Parser.Default.ParseArgumentsStrict(args, options);

            var service = ServiceFactory.CreateService; // Get Service
            var report = new Report(options); // Get Report

            // Upload report using V4 api
            var v4 = new Upload(options);
            v4.Uploader(report.Reporter, service);
        }
    }
}