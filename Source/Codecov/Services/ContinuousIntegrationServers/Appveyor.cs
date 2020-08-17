using System;
using System.Linq;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class AppVeyor : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch;
        private readonly Lazy<string> _build;
        private readonly Lazy<string> _buildUrl;
        private readonly Lazy<string> _commit;
        private readonly Lazy<bool> _detecter;
        private readonly Lazy<string> _job;
        private readonly Lazy<string> _pr;
        private readonly Lazy<string> _slug;

        public AppVeyor(IEnviornmentVariables environmentVariables)
            : base(environmentVariables)
        {
            _branch = new Lazy<string>(() => GetEnvironmentVariable("APPVEYOR_REPO_BRANCH"));
            _build = new Lazy<string>(LoadBuild);
            _buildUrl = new Lazy<string>(LoadBuildUrl);
            _commit = new Lazy<string>(() => GetEnvironmentVariable("APPVEYOR_REPO_COMMIT"));
            _detecter = new Lazy<bool>(() => CheckEnvironmentVariables("CI", "APPVEYOR"));
            _job = new Lazy<string>(LoadJob);
            _pr = new Lazy<string>(() => GetEnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER"));
            _slug = new Lazy<string>(() => GetEnvironmentVariable("APPVEYOR_REPO_NAME"));
        }

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string BuildUrl => _buildUrl.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Job => _job.Value;

        public override string Pr => _pr.Value;

        public override string Service => "appveyor";

        public override string Slug => _slug.Value;

        private static bool IsNullOrEmpty(params string[] parameters)
            => parameters.Any(x => string.IsNullOrEmpty(x));

        private string LoadBuild()
        {
            var build = GetEnvironmentVariable("APPVEYOR_JOB_ID");
            return !string.IsNullOrWhiteSpace(build) ? Uri.EscapeDataString(build) : string.Empty;
        }

        private string LoadBuildUrl()
        {
            var hostUrl = GetEnvironmentVariable("APPVEYOR_URL");
            var accountName = GetEnvironmentVariable("APPVEYOR_ACCOUNT_NAME");
            var slug = GetEnvironmentVariable("APPVEYOR_PROJECT_SLUG");
            var jobId = GetEnvironmentVariable("APPVEYOR_JOB_ID");
            var buildId = GetEnvironmentVariable("APPVEYOR_BUILD_ID");

            if (IsNullOrEmpty(hostUrl, accountName, slug, jobId, buildId) || !Uri.TryCreate(hostUrl, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return string.Empty;
            }

            var jobUrl = $"{hostUrl}/project/{accountName}/{slug}/builds/{buildId}/job/{jobId}";
            return jobUrl;
        }

        private string LoadJob()
        {
            var accountName = GetEnvironmentVariable("APPVEYOR_ACCOUNT_NAME");
            var slug = GetEnvironmentVariable("APPVEYOR_PROJECT_SLUG");
            var version = GetEnvironmentVariable("APPVEYOR_BUILD_VERSION");

            if (string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(slug) || string.IsNullOrWhiteSpace(version))
            {
                return string.Empty;
            }

            var job = $"{accountName}/{slug}/{version}";

            return job;
        }
    }
}
