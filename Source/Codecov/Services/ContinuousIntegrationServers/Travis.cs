using System;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class Travis : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("TRAVIS_BRANCH"));
        private readonly Lazy<string> _build = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("TRAVIS_JOB_NUMBER"));
        private readonly Lazy<string> _commit = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("TRAVIS_COMMIT"));
        private readonly Lazy<bool> _detecter = new Lazy<bool>(() => CheckEnvironmentVariables("CI", "TRAVIS"));
        private readonly Lazy<string> _job = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("TRAVIS_JOB_ID"));
        private readonly Lazy<string> _pr = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("TRAVIS_PULL_REQUEST"));
        private readonly Lazy<string> _slug = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("TRAVIS_REPO_SLUG"));

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Job => _job.Value;

        public override string Pr => _pr.Value;

        public override string Service => "travis";

        public override string Slug => _slug.Value;
    }
}
