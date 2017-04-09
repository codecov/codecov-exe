using System;
using Codecov.Program;
using Codecov.Services.Helpers;

namespace Codecov.Services
{
    internal class TeamCity : Service
    {
        public TeamCity(Options options) : base(options)
        {
        }

        public override void SetQueryParams()
        {
            QueryParameters["branch"] = Branch;
            QueryParameters["commit"] = Commit;
            QueryParameters["build"] = Environment.GetEnvironmentVariable("TEAMCITY_BUILD_ID");
            QueryParameters["build_url"] = BuildUrl;
            QueryParameters["service"] = "teamcity";
            QueryParameters["slug"] = Slug;
        }

        public override bool Detect
        {
            get
            {
                var teamCity = Environment.GetEnvironmentVariable("TEAMCITY_VERSION");

                if (string.IsNullOrWhiteSpace(teamCity))
                {
                    return false;
                }

                Log.X("TeamCity CI detected.");

                var branch = Environment.GetEnvironmentVariable("TEAMCITY_BUILD_BRANCH");
                if (string.IsNullOrWhiteSpace(branch))
                {
                    Log.Message("Teamcity does not automatically make build parameters available as environment variables.");
                    Log.Message("Add the following environment parameters to the build configuration");
                    Log.Message("env.TEAMCITY_BUILD_BRANCH = %teamcity.build.branch%");
                    Log.Message("env.TEAMCITY_BUILD_ID = %teamcity.build.id%");
                    Log.Message("env.TEAMCITY_BUILD_URL = %teamcity.serverUrl%/viewLog.html?buildId=%teamcity.build.id%");
                    Log.Message("env.TEAMCITY_BUILD_COMMIT = %system.build.vcs.number%");
                    Log.Message("env.TEAMCITY_BUILD_REPOSITORY = %vcsroot.<YOUR TEAMCITY VCS NAME>.url%");
                }

                return true;
            }
        }

        private static string Slug
        {
            get
            {
                var buildRepository = Environment.GetEnvironmentVariable("TEAMCITY_BUILD_REPOSITORY");
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

        private static string Branch
        {
            get
            {
                var branch = Environment.GetEnvironmentVariable("TEAMCITY_BUILD_BRANCH");
                if (!string.IsNullOrWhiteSpace(branch))
                {
                    return branch;
                }

                return string.Empty;
            }
        }

        private static string BuildUrl
        {
            get
            {
                var buildUrl = Environment.GetEnvironmentVariable("TEAMCITY_BUILD_URL");
                return !string.IsNullOrWhiteSpace(buildUrl) ? Uri.EscapeDataString(buildUrl) : string.Empty;
            }
        }

        private static string Commit
        {
            get
            {
                var commit = Environment.GetEnvironmentVariable("TEAMCITY_BUILD_COMMIT");
                return !string.IsNullOrWhiteSpace(commit) ? commit : Environment.GetEnvironmentVariable("BUILD_VCS_NUMBER");
            }
        }
    }
}