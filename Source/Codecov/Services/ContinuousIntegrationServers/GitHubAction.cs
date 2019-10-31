using System;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class GitHubAction : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch = new Lazy<string>(LoadBranch);
        private readonly Lazy<string> _commit = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("GITHUB_SHA"));
        private readonly Lazy<bool> _detecter = new Lazy<bool>(() => CheckEnvironmentVariables("GITHUB_ACTIONS"));
        private readonly Lazy<string> _slug = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("GITHUB_REPOSITORY"));

        public override string Branch => _branch.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Service => "github-actions";

        public override string Slug => _slug.Value;

        private static string LoadBranch()
        {
            var branch = EnviornmentVariable.GetEnviornmentVariable("GITHUB_REF");

            return string.IsNullOrWhiteSpace(branch) ? string.Empty : branch.StartsWith("ref/heads/") ? branch.Substring(10) : branch;
        }
    }
}
