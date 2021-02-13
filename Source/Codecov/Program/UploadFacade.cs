using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using Codecov.Coverage.Tool;
using Codecov.Services.ContinuousIntegrationServers;
using Codecov.Services.VersionControlSystems;
using Codecov.Upload;
using Codecov.Url;
using Codecov.Utilities;
using Codecov.Yaml;
using Serilog;

namespace Codecov.Program
{
    [ExcludeFromCodeCoverage]
    internal class UploadFacade
    {
        private readonly IContinuousIntegrationServer _continuousIntegrationServer;
        private readonly IVersionControlSystem _versionControlSystem;
        private readonly IYaml _yaml;
        private readonly ICoverage _coverage;
        private readonly IEnviornmentVariables _enviornmentVariables;
        private readonly IUrl _url;
        private readonly IUpload _upload;

        public UploadFacade(IContinuousIntegrationServer continuousIntegrationServer, IVersionControlSystem versionControlSystem, IYaml yaml, ICoverage coverage, IEnviornmentVariables enviornmentVariables, IUrl url, IUpload upload)
        {
            _continuousIntegrationServer = continuousIntegrationServer;
            _versionControlSystem = versionControlSystem;
            _yaml = yaml;
            _coverage = coverage;
            _enviornmentVariables = enviornmentVariables;
            _url = url;
            _upload = upload;
        }

        private string DisplayUrl
        {
            get
            {
                var url = _url.GetUrl.ToString();
                var regex = new Regex(@"token=\w{8}-\w{4}-\w{4}-\w{4}-\w{12}&");
                return regex.Replace(url, string.Empty);
            }
        }

        public void LogContinuousIntegrationAndVersionControlSystem()
        {
            var ci = _continuousIntegrationServer.GetType().Name;
            if (ci.Equals("ContinuousIntegrationServer"))
            {
                Log.Warning("No CI detected.");
            }
            else if (ci.Equals("TeamCity"))
            {
                Log.Information("{ci} detected.", ci);
                if (string.IsNullOrWhiteSpace(_continuousIntegrationServer.Branch))
                {
                    Log.Warning("Teamcity does not automatically make build parameters available as environment variables.\nAdd the following environment parameters to the build configuration.\nenv.TEAMCITY_BUILD_BRANCH = %teamcity.build.branch%.\nenv.TEAMCITY_BUILD_ID = %teamcity.build.id%.\nenv.TEAMCITY_BUILD_URL = %teamcity.serverUrl%/viewLog.html?buildId=%teamcity.build.id%.\nenv.TEAMCITY_BUILD_COMMIT = %system.build.vcs.number%.\nenv.TEAMCITY_BUILD_REPOSITORY = %vcsroot.<YOUR TEAMCITY VCS NAME>.url%.");
                }
            }
            else
            {
                Log.Information("{ci} detected.", ci);
            }

            var vcs = _versionControlSystem.GetType().Name;
            if (vcs.Equals("VersionControlSystem"))
            {
                Log.Warning("No VCS detected.");
            }
            else
            {
                Log.Information("{vcs} detected.", vcs);
            }

            Log.Information("Project root: {RepoRoot}", _versionControlSystem.RepoRoot);
            if (string.IsNullOrWhiteSpace(_yaml.FileName))
            {
                Log.Information("Yaml not found, that's ok! Learn more at {CodecovUrl}", "https://docs.codecov.io/docs/codecov-yaml");
            }

            Log.Information("Reading reports.");
            Log.Information(string.Join("\n", _coverage.CoverageReports.Select(x => x.File)));

            if (_enviornmentVariables.UserEnvironmentVariables.Any())
            {
                Log.Information("Appending build variables");
                Log.Information(string.Join("\n", _enviornmentVariables.UserEnvironmentVariables.Select(x => x.Key.Trim()).ToArray()));
            }
        }

        public string UploadReports()
        {
            // We warn if the total file size is above 20 MB
            var fileSizes = _coverage.CoverageReports.Sum(x => FileSystem.GetFileSize(x.File));
            if (fileSizes > 20_971_520)
            {
                Log.Warning("Total file size of reports is above 20MB, this may prevent report being shown on {Host}", _url.GetUrl.Host);
                Log.Warning("Reduce the total upload size if this occurs");
            }

            Log.Information("Uploading Reports.");
            Log.Information("url: {Scheme}://{Authority}", _url.GetUrl.Scheme, _url.GetUrl.Authority);
            Log.Information("query: {DisplayUrl}", DisplayUrl);

            var response = _upload.Uploader();
            Log.Verbose("response: {response}", response);
            var splitResponse = response.Split('\n');
            if (splitResponse.Length > 1)
            {
                var s3 = new Uri(splitResponse[1]);
                Log.Information("Uploading to S3 {Scheme}://{Authority}", s3.Scheme, s3.Authority);
            }

            var reportUrl = splitResponse[0];
            Log.Information("View reports at: {reportUrl}", reportUrl);
            return reportUrl;
        }
    }
}
