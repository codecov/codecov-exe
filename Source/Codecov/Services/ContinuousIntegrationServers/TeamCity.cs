using System;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class TeamCity : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch;
        private readonly Lazy<string> _build;
        private readonly Lazy<string> _buildUrl;
        private readonly Lazy<string> _commit;
        private readonly Lazy<bool> _detecter;
        private readonly Lazy<string> _slug;

        public TeamCity(IEnviornmentVariables environmentVariables)
            : base(environmentVariables)
        {
            _branch = new Lazy<string>(() => GetEnvironmentVariable("TEAMCITY_BUILD_BRANCH"));
            _build = new Lazy<string>(() => GetEnvironmentVariable("TEAMCITY_BUILD_ID"));
            _buildUrl = new Lazy<string>(LoadBuildUrl);
            _commit = new Lazy<string>(LoadCommit);
            _detecter = new Lazy<bool>(LoadDetecter);
            _slug = new Lazy<string>(LoadSlug);
        }

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string BuildUrl => _buildUrl.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Service => "teamcity";

        public override string Slug => _slug.Value;

        private string LoadBuildUrl()
        {
            var buildUrl = GetEnvironmentVariable("TEAMCITY_BUILD_URL");
            return !string.IsNullOrWhiteSpace(buildUrl) ? Uri.EscapeDataString(buildUrl) : string.Empty;
        }

        private string LoadCommit()
        {
            var commit = GetEnvironmentVariable("TEAMCITY_BUILD_COMMIT");
            return !string.IsNullOrWhiteSpace(commit) ? commit : GetEnvironmentVariable("BUILD_VCS_NUMBER");
        }

        private bool LoadDetecter()
        {
            var teamCity = GetEnvironmentVariable("TEAMCITY_VERSION");
            return !string.IsNullOrWhiteSpace(teamCity);
        }

        private string LoadSlug()
        {
            var buildRepository = GetEnvironmentVariable("TEAMCITY_BUILD_REPOSITORY");
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
