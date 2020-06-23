using System;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class Travis : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch;
        private readonly Lazy<string> _build;
        private readonly Lazy<string> _buildUrl;
        private readonly Lazy<string> _commit;
        private readonly Lazy<bool> _detecter;
        private readonly Lazy<string> _job;
        private readonly Lazy<string> _pr;
        private readonly Lazy<string> _slug;
        private readonly Lazy<string> _tag;

        public Travis(IEnviornmentVariables environmentVariables)
            : base(environmentVariables)
        {
            _branch = new Lazy<string>(() => GetEnvironmentVariable("TRAVIS_BRANCH"));
            _build = new Lazy<string>(() => GetEnvironmentVariable("TRAVIS_JOB_NUMBER"));
            _buildUrl = new Lazy<string>(() => GetEnvironmentVariable("TRAVIS_JOB_WEB_URL"));
            _commit = new Lazy<string>(() => GetEnvironmentVariable("TRAVIS_COMMIT"));
            _detecter = new Lazy<bool>(() => CheckEnvironmentVariables("CI", "TRAVIS"));
            _job = new Lazy<string>(() => GetEnvironmentVariable("TRAVIS_JOB_ID"));
            _pr = new Lazy<string>(() => GetEnvironmentVariable("TRAVIS_PULL_REQUEST"));
            _slug = new Lazy<string>(() => GetEnvironmentVariable("TRAVIS_REPO_SLUG"));
            _tag = new Lazy<string>(() => GetEnvironmentVariable("TRAVIS_TAG"));
        }

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string BuildUrl => _buildUrl.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Job => _job.Value;

        public override string Pr => _pr.Value;

        public override string Service => "travis";

        public override string Slug => _slug.Value;

        public override string Tag => _tag.Value;
    }
}
