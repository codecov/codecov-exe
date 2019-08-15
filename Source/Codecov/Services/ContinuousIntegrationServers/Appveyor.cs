using System;
using System.Linq;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class AppVeyor : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_REPO_BRANCH"));
        private readonly Lazy<string> _build = new Lazy<string>(LoadBuild);
        private readonly Lazy<string> _buildUrl = new Lazy<string>(LoadBuildUrl);
        private readonly Lazy<string> _commit = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_REPO_COMMIT"));
        private readonly Lazy<bool> _detecter = new Lazy<bool>(() => CheckEnvironmentVariables("CI", "APPVEYOR"));
        private readonly Lazy<string> _job = new Lazy<string>(LoadJob);
        private readonly Lazy<string> _pr = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_PULL_REQUEST_NUMBER"));
        private readonly Lazy<string> _slug = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_REPO_NAME"));

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string BuildUrl => _buildUrl.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Job => _job.Value;

        public override string Pr => _pr.Value;

        public override string Service => "appveyor";

        public override string Slug => _slug.Value;

        private static string LoadBuild()
        {
            var build = EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_JOB_ID");
            return !string.IsNullOrWhiteSpace(build) ? Uri.EscapeDataString(build) : string.Empty;
        }

        private static string LoadBuildUrl()
        {
            var hostUrl = EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_URL");
            var accountName = EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_ACCOUNT_NAME");
            var slug = EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_PROJECT_SLUG");
            var jobId = EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_JOB_ID");

            if (IsNullOrEmpty(hostUrl, accountName, slug, jobId) || !Uri.TryCreate(hostUrl, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return string.Empty;
            }

            var jobUrl = $"{hostUrl}/project/{accountName}/{slug}/build/job/{jobId}";
            return jobUrl;
        }

        private static string LoadJob()
        {
            var accountName = EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_ACCOUNT_NAME");
            var slug = EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_PROJECT_SLUG");
            var version = EnviornmentVariable.GetEnviornmentVariable("APPVEYOR_BUILD_VERSION");

            if (string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(slug) || string.IsNullOrWhiteSpace(version))
            {
                return string.Empty;
            }

            var job = $"{accountName}/{slug}/{version}";

            return job;
        }

        private static bool IsNullOrEmpty(params string[] parameters)
            => parameters.Any(x => string.IsNullOrEmpty(x));
    }
}
