using System;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class TeamCity : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("TEAMCITY_BUILD_BRANCH"));
        private readonly Lazy<string> _build = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("TEAMCITY_BUILD_ID"));
        private readonly Lazy<string> _buildUrl = new Lazy<string>(LoadBuildUrl);
        private readonly Lazy<string> _commit = new Lazy<string>(LoadCommit);
        private readonly Lazy<bool> _detecter = new Lazy<bool>(LoadDetecter);
        private readonly Lazy<string> _slug = new Lazy<string>(LoadSlug);

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string BuildUrl => _buildUrl.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Service => "teamcity";

        public override string Slug => _slug.Value;

        private static string LoadBuildUrl()
        {
            var buildUrl = EnviornmentVariable.GetEnviornmentVariable("TEAMCITY_BUILD_URL");
            return !string.IsNullOrWhiteSpace(buildUrl) ? Uri.EscapeDataString(buildUrl) : string.Empty;
        }

        private static string LoadCommit()
        {
            var commit = EnviornmentVariable.GetEnviornmentVariable("TEAMCITY_BUILD_COMMIT");
            return !string.IsNullOrWhiteSpace(commit) ? commit : EnviornmentVariable.GetEnviornmentVariable("BUILD_VCS_NUMBER");
        }

        private static bool LoadDetecter()
        {
            var teamCity = EnviornmentVariable.GetEnviornmentVariable("TEAMCITY_VERSION");
            return !string.IsNullOrWhiteSpace(teamCity);
        }

        private static string LoadSlug()
        {
            var buildRepository = EnviornmentVariable.GetEnviornmentVariable("TEAMCITY_BUILD_REPOSITORY");
            if (string.IsNullOrWhiteSpace(buildRepository))
            {
                return string.Empty;
            }

            var temp = buildRepository.Split(':');
            if (temp.Length > 0)
            {
                temp[0] = string.Empty;
            }

            buildRepository = string.Join(string.Empty, temp);

            var splitBuildRepository = buildRepository.Split('/');
            if (splitBuildRepository.Length > 1)
            {
                var repo = splitBuildRepository[splitBuildRepository.Length - 1].Replace(".git", string.Empty);
                var owner = splitBuildRepository[splitBuildRepository.Length - 2];
                return $"{owner}/{repo}";
            }

            return string.Empty;
        }
    }
}
