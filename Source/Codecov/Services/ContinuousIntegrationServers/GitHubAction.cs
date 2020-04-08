using System;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class GitHubAction : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch;
        private readonly Lazy<string> _commit;
        private readonly Lazy<bool> _detecter;
        private readonly Lazy<string> _slug;

        public GitHubAction()
        {
            _branch = new Lazy<string>(LoadBranch);
            _commit = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_SHA"));
            _detecter = new Lazy<bool>(() => CheckEnvironmentVariables("GITHUB_ACTIONS") || !string.IsNullOrWhiteSpace(GetEnvironmentVariable("GITHUB_ACTION")));
            _slug = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_REPOSITORY"));
        }

        public override string Branch => _branch.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Service => "github-actions";

        public override string Slug => _slug.Value;

        private string LoadBranch()
        {
            var branch = GetEnvironmentVariable("GITHUB_REF");

            return string.IsNullOrWhiteSpace(branch) ? string.Empty : branch.StartsWith("ref/heads/") ? branch.Substring(10) : branch;
        }
    }
}
