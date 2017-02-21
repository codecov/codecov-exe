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

            var service = ServiceFactory.CreateService;
            var report = Report.CreateReport(options.File);
            Uploder.Upload(options, report, service);
        }
    }
}