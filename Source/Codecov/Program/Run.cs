using Codecov.Coverage;
using Codecov.Services.Helpers;

namespace Codecov.Program
{
    internal class Run
    {
        public Run(string[] args)
        {
            Options = Cli.GetOptions(args);
        }

        public int SuccessIsRequired => Options.Required ? 1 : 0;

        private Options Options { get; }

        public void Runner()
        {
            Log.IsVerboseMode = Options.Verbose;
            DisplayFiglet();
            var serviceFactory = new ServiceFactory(Options);
            var service = serviceFactory.CreateService;
            service.SetQueryParams();
            var report = Report.Reporter(Options.File, Options.Env, service.SourceCodeFiles);
            var uploadFactory = new UploadFactory(Options, service.Query);
            var upload = uploadFactory.CreateUpload;
            upload.Uploader(report);
        }

        private static void DisplayFiglet()
        {
            Log.WriteLine($@"
              _____          _
             / ____|        | |
            | |     ___   __| | ___  ___ _____   __
            | |    / _ \ / _  |/ _ \/ __/ _ \ \ / /
            | |___| (_) | (_| |  __/ (_| (_) \ V /
             \_____\___/ \____|\___|\___\___/ \_/
                                         {Utils.Version}
            ");
        }
    }
}