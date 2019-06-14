using System;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class AzurePipelines : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("BUILD_SOURCEBRANCHNAME"));
        private readonly Lazy<string> _build = new Lazy<string>(LoadBuild);
        private readonly Lazy<string> _commit = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("BUILD_SOURCEVERSION"));
        private readonly Lazy<bool> _detecter = new Lazy<bool>(() => CheckEnvironmentVariables("TF_BUILD"));
        private readonly Lazy<string> _job = new Lazy<string>(LoadJob);
        private readonly Lazy<string> _pr = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER"));
        private readonly Lazy<string> _slug = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("BUILD_REPOSITORY_NAME"));

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
    }
}
