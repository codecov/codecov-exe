using System;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class AzurePipelines : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch;
        private readonly Lazy<string> _build;
        private readonly Lazy<string> _buildUrl;
        private readonly Lazy<string> _commit;
        private readonly Lazy<bool> _detecter;
        private readonly Lazy<string> _job;
        private readonly Lazy<string> _pr;
        private readonly Lazy<string> _project;
        private readonly Lazy<string> _serverUri;
        private readonly Lazy<string> _slug;

        public AzurePipelines()
        {
            _branch = new Lazy<string>(LoadBranch);
            _build = new Lazy<string>(LoadBuild);
            _buildUrl = new Lazy<string>(LoadBuildUrl);
            _commit = new Lazy<string>(() => GetEnvironmentVariable("BUILD_SOURCEVERSION"));
            _detecter = new Lazy<bool>(() => CheckEnvironmentVariables("TF_BUILD"));
            _job = new Lazy<string>(() => GetEnvironmentVariable("BUILD_BUILDID"));
            _pr = new Lazy<string>(() => GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER"));
            _project = new Lazy<string>(() => GetEnvironmentVariable("SYSTEM_TEAMPROJECT"));
            _serverUri = new Lazy<string>(() => GetEnvironmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI"));
            _slug = new Lazy<string>(() => GetEnvironmentVariable("BUILD_REPOSITORY_NAME"));
        }

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string BuildUrl => _buildUrl.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Job => _job.Value;

        public override string Pr => _pr.Value;

        public override string Project => _project.Value;

        public override string ServerUri => _serverUri.Value;

        public override string Service => "azure_pipelines";

        public override string Slug => _slug.Value;

        private string LoadBranch()
        {
            var pullRequestBranch = GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH");
            if (!string.IsNullOrEmpty(pullRequestBranch))
            {
                return pullRequestBranch;
            }

            return GetEnvironmentVariable("BUILD_SOURCEBRANCHNAME");
        }

        private string LoadBuild()
        {
            var build = GetEnvironmentVariable("BUILD_BUILDNUMBER");
            return !string.IsNullOrWhiteSpace(build) ? Uri.EscapeDataString(build) : string.Empty;
        }

        private string LoadBuildUrl()
        {
            var serverUri = ServerUri;
            var project = Project;
            var build = Job;
            if (string.IsNullOrEmpty(serverUri) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(build))
            {
                return string.Empty;
            }

            if (!Uri.TryCreate(serverUri, UriKind.Absolute, out var uri)
                || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return string.Empty;
            }

            return $"{serverUri}{project}/_build/results?buildId={build}";
        }
    }
}
