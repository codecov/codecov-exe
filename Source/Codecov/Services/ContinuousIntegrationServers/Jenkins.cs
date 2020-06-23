using System;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class Jenkins : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch;
        private readonly Lazy<string> _build;
        private readonly Lazy<string> _buildUrl;
        private readonly Lazy<string> _commit;
        private readonly Lazy<bool> _detecter;
        private readonly Lazy<string> _pr;

        public Jenkins(IEnviornmentVariables environmentVariables)
            : base(environmentVariables)
        {
            _branch = new Lazy<string>(() => GetFirstExistingEnvironmentVariable("ghprbSourceBranch", "GIT_BRANCH", "BRANCH_NAME"));
            _build = new Lazy<string>(() => GetEnvironmentVariable("BUILD_NUMBER"));
            _buildUrl = new Lazy<string>(() => GetEnvironmentVariable("BUILD_URL"));
            _commit = new Lazy<string>(() => GetFirstExistingEnvironmentVariable("ghprbActualCommit", "GIT_COMMIT"));
            _detecter = new Lazy<bool>(() => !string.IsNullOrWhiteSpace(GetEnvironmentVariable("JENKINS_URL")));
            _pr = new Lazy<string>(() => GetFirstExistingEnvironmentVariable("ghprbPullId", "CHANGE_ID"));
        }

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string BuildUrl => _buildUrl.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Pr => _pr.Value;

        public override string Service => "jenkins";
    }
}
