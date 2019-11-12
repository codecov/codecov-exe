using System;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class AzurePipelines : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("BUILD_SOURCEBRANCHNAME"));
        private readonly Lazy<string> _build = new Lazy<string>(LoadBuild);
        private readonly Lazy<string> _buildUrl;
        private readonly Lazy<string> _commit = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("BUILD_SOURCEVERSION"));
        private readonly Lazy<bool> _detecter = new Lazy<bool>(() => CheckEnvironmentVariables("TF_BUILD"));
        private readonly Lazy<string> _job = new Lazy<string>(LoadJob);
        private readonly Lazy<string> _pr = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER"));
        private readonly Lazy<string> _project = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("SYSTEM_TEAMPROJECT"));
        private readonly Lazy<string> _serverUri = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("SYSTEM_TEAMFOUNDATIONSERVERURI"));
        private readonly Lazy<string> _slug = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("BUILD_REPOSITORY_NAME"));

        public AzurePipelines()
        {
            _buildUrl = new Lazy<string>(LoadBuildUrl);
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

        private static string LoadBuild()
        {
            var build = EnviornmentVariable.GetEnviornmentVariable("BUILD_BUILDID");
            return !string.IsNullOrWhiteSpace(build) ? Uri.EscapeDataString(build) : string.Empty;
        }

        private static string LoadJob()
        {
            var slug = EnviornmentVariable.GetEnviornmentVariable("BUILD_REPOSITORY_NAME");
            var version = EnviornmentVariable.GetEnviornmentVariable("BUILD_BUILDNUMBER");

            if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(version))
            {
                return string.Empty;
            }

            var job = $"{slug}/{version}";

            return job;
        }

        private string LoadBuildUrl()
        {
            var serverUri = ServerUri;
            var project = Project;
            var build = Build;
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
