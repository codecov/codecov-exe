using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Codecov.Coverage.EnvironmentVariables;
using Codecov.Coverage.Report;
using Codecov.Coverage.SourceCode;
using Codecov.Coverage.Tool;
using Codecov.Factories;
using Codecov.Logger;
using Codecov.Services;
using Codecov.Services.ContinuousIntegrationServers;
using Codecov.Services.VersionControlSystems;
using Codecov.Terminal;
using Codecov.Upload;
using Codecov.Url;
using Codecov.Yaml;

namespace Codecov.Program
{
    internal class UploadFacade
    {
        public UploadFacade(CommandLineOptions commandLineOptions)
        {
            CommandLineCommandLineOptions = commandLineOptions;
        }

        private static IDictionary<TerminalName, ITerminal> Terminals => TerminalFactory.Create();

        private static IContinuousIntegrationServer ContinuousIntegrationServer => ContinuousIntegrationServerFactory.Create();

        private ICoverage Coverage => new Coverage.Tool.Coverage(CommandLineCommandLineOptions);

        private IUpload Upload => new Uploads(Url, Report);

        private IUrl Url => new Url.Url(new Host(CommandLineCommandLineOptions), new Route(), new Query(CommandLineCommandLineOptions, Repositories, ContinuousIntegrationServer, Yaml));

        private IYaml Yaml => new Yaml.Yaml(SourceCode);

        private CommandLineOptions CommandLineCommandLineOptions { get; }

        private IEnvironmentVariables EnvironmentVariables => new EnvironmentVariables(CommandLineCommandLineOptions, ContinuousIntegrationServer);

        private IReport Report => new Report(CommandLineCommandLineOptions, EnvironmentVariables, SourceCode, Coverage);

        private IEnumerable<IRepository> Repositories => RepositoryFactory.Create(VersionControlSystem, ContinuousIntegrationServer);

        private ISourceCode SourceCode => new SourceCode(VersionControlSystem);

        private IVersionControlSystem VersionControlSystem => VersionControlSystemFactory.Create(CommandLineCommandLineOptions, Terminals[TerminalName.Generic]);

        private string DisplayUrl
        {
            get
            {
                var url = Url.GetUrl.ToString();
                var regex = new Regex(@"token=\w{8}-\w{4}-\w{4}-\w{4}-\w{12}&");
                return regex.Replace(url, string.Empty);
            }
        }

        public void Uploader()
        {
            var ci = ContinuousIntegrationServer.GetType().Name;
            if (ci.Equals("ContinuousIntegrationServer"))
            {
                Log.Warning("No CI detected.");
            }
            else if (ci.Equals("TeamCity"))
            {
                Log.Information("TeamCity detected.");
                if (string.IsNullOrWhiteSpace(ContinuousIntegrationServer.Branch))
                {
                    Log.Warning("Teamcity does not automatically make build parameters available as environment variables.\nAdd the following environment parameters to the build configuration.\nenv.TEAMCITY_BUILD_BRANCH = %teamcity.build.branch%.\nenv.TEAMCITY_BUILD_ID = %teamcity.build.id%.\nenv.TEAMCITY_BUILD_URL = %teamcity.serverUrl%/viewLog.html?buildId=%teamcity.build.id%.\nenv.TEAMCITY_BUILD_COMMIT = %system.build.vcs.number%.\nenv.TEAMCITY_BUILD_REPOSITORY = %vcsroot.<YOUR TEAMCITY VCS NAME>.url%.");
                }
            }
            else
            {
                Log.Information($"{ci} detected.");
            }

            var vcs = VersionControlSystem.GetType().Name;
            if (vcs.Equals("VersionControlSystem"))
            {
                Log.Warning("No VCS detected.");
            }
            else
            {
                Log.Information($"{vcs} detected.");
            }

            Log.Information($"Project root: {VersionControlSystem.RepoRoot}");
            if (string.IsNullOrWhiteSpace(Yaml.FileName))
            {
                Log.Information("Yaml not found, that's ok! Learn more at http://docs.codecov.io/docs/codecov-yaml");
            }

            Log.Information("Reading reports.");
            Log.Information(string.Join("\n", Coverage.CoverageReports.Select(x => x.File)));

            if (EnvironmentVariables.GetEnvironmentVariables.Any())
            {
                Log.Information("Appending build variables");
                Log.Information(string.Join("\n", EnvironmentVariables.GetEnvironmentVariables.Select(x => x.Key.Trim()).ToArray()));
            }

            if (CommandLineCommandLineOptions.Dump)
            {
                Log.Warning("Skipping upload and dumping contents.");
                Log.Information($"url: {Url.GetUrl}");
                Log.Information(Report.Reporter);
                return;
            }

            Log.Information("Uploading Reports.");
            Log.Information($"url: {Url.GetUrl.Scheme}://{Url.GetUrl.Authority}");
            Log.Verboase($"api endpoint: {Url.GetUrl}");
            Log.Information($"query: {DisplayUrl}");
            Log.Information("Pinging Codecov");

            var response = Upload.Uploader();
            Log.Verboase($"response: {response}");
            var splitResponse = response.Split('\n');
            var s3 = new Uri(splitResponse[1]);
            var reportUrl = splitResponse[0];
            Log.Information($"Uploading to S3 {s3.Scheme}://{s3.Authority}");
            Log.Information($"View reports at: {reportUrl}");
        }
    }
}
