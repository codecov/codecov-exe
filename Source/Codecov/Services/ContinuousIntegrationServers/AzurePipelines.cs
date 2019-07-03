using System;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class AzurePipelines : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("BUILD_SOURCEBRANCHNAME"));
        private readonly Lazy<string> _build = new Lazy<string>(LoadBuild);
        private readonly Lazy<string> _commit = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("BUILD_SOURCEVERSION"));
        private readonly Lazy<bool> _detecter = new Lazy<bool>(LoadDetecter);
        private readonly Lazy<string> _job = new Lazy<string>(LoadJob);
        private readonly Lazy<string> _pr = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER"));
        private readonly Lazy<string> _slug = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("BUILD_REPOSITORY_NAME"));

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Job => _job.Value;

        public override string Pr => _pr.Value;

        public override string Service => "custom";

        public override string Slug => _slug.Value;

        private static string LoadBuild()
        {
            var build = EnvironmentVariable.GetEnvironmentVariable("BUILD_BUILDID");
            return !string.IsNullOrWhiteSpace(build) ? Uri.EscapeDataString(build) : string.Empty;
        }

        private static bool LoadDetecter()
        {
            var tfbuild = EnvironmentVariable.GetEnvironmentVariable("TF_BUILD");
            return tfbuild.Equals("True", StringComparison.OrdinalIgnoreCase);
        }

        private static string LoadJob()
        {
            var slug = EnvironmentVariable.GetEnvironmentVariable("BUILD_REPOSITORY_NAME");
            var version = EnvironmentVariable.GetEnvironmentVariable("BUILD_BUILDNUMBER");

            if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(version))
            {
                return string.Empty;
            }

            var job = $"{slug}/{version}";

            return job;
        }
    }
}
