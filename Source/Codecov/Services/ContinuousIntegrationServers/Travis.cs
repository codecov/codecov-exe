using System;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class Travis : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("TRAVIS_BRANCH"));
        private readonly Lazy<string> _build = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("TRAVIS_JOB_NUMBER"));
        private readonly Lazy<string> _commit = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("TRAVIS_COMMIT"));
        private readonly Lazy<bool> _detecter = new Lazy<bool>(LoadDetecter);
        private readonly Lazy<string> _job = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("TRAVIS_JOB_ID"));
        private readonly Lazy<string> _pr = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("TRAVIS_PULL_REQUEST"));
        private readonly Lazy<string> _slug = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("TRAVIS_REPO_SLUG"));

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Job => _job.Value;

        public override string Pr => _pr.Value;

        public override string Service => "travis";

        public override string Slug => _slug.Value;

        private static bool LoadDetecter()
        {
            var travis = EnviornmentVariable.GetEnviornmentVariable("TRAVIS");
            var ci = EnviornmentVariable.GetEnviornmentVariable("CI");

            if (string.IsNullOrWhiteSpace(travis) || string.IsNullOrWhiteSpace(ci))
            {
                return false;
            }

            return travis.Equals(ci, StringComparison.Ordinal)
                   && travis.Equals("True", StringComparison.OrdinalIgnoreCase);
        }
    }
}
