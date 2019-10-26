using System;
using System.Collections.Generic;
using System.Text;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class Jenkins : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch = new Lazy<string>(() => EnviornmentVariable.GetFirstExistingEnvironmentVariable("ghprbSourceBranch", "GIT_BRANCH", "BRANCH_NAME"));
        private readonly Lazy<string> _build = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("BUILD_NUMBER"));
        private readonly Lazy<string> _buildUrl = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("BUILD_URL"));
        private readonly Lazy<string> _commit = new Lazy<string>(() => EnviornmentVariable.GetFirstExistingEnvironmentVariable("ghprbActualCommit", "GIT_COMMIT"));
        private readonly Lazy<bool> _detecter = new Lazy<bool>(() => !string.IsNullOrWhiteSpace(EnviornmentVariable.GetEnviornmentVariable("JENKINS_URL")));
        private readonly Lazy<string> _pr = new Lazy<string>(() => EnviornmentVariable.GetFirstExistingEnvironmentVariable("ghprbPullId", "CHANGE_ID"));

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string BuildUrl => _buildUrl.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Pr => _pr.Value;

        public override string Service => "jenkins";
    }
}
