using System;
using codecov.Services.Utils;

namespace codecov.Services
{
    internal class AppVeyor : Service
    {
        public AppVeyor()
        {
            QueryParameters["branch"] = Environment.GetEnvironmentVariable("APPVEYOR_REPO_BRANCH");
            QueryParameters["commit"] = Environment.GetEnvironmentVariable("APPVEYOR_REPO_COMMIT");
            QueryParameters["job"] = Job;
            QueryParameters["build"] = Build;
            QueryParameters["pr"] = Environment.GetEnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER");
            QueryParameters["slug"] = Environment.GetEnvironmentVariable("APPVEYOR_REPO_NAME");
            QueryParameters["service"] = "appveyor";
        }

        public override bool Detect
        {
            get
            {
                var appVeyor = Environment.GetEnvironmentVariable("APPVEYOR");
                var ci = Environment.GetEnvironmentVariable("CI");

                if (string.IsNullOrWhiteSpace(appVeyor) || string.IsNullOrWhiteSpace(ci))
                {
                    return false;
                }

                var isAppveyor = appVeyor.Equals("True") && ci.Equals("True");
                if (!isAppveyor)
                {
                    return false;
                }
                Console.WriteLine("==> Appveyor CI detected.");
                return true;
            }
        }

        private static string Build
        {
            get
            {
                var build = Environment.GetEnvironmentVariable("APPVEYOR_JOB_ID");
                return !string.IsNullOrWhiteSpace(build) ? Uri.EscapeDataString(build) : string.Empty;
            }
        }

        private static string Job
        {
            get
            {
                var accountName = Environment.GetEnvironmentVariable("APPVEYOR_ACCOUNT_NAME");
                var slug = Environment.GetEnvironmentVariable("APPVEYOR_PROJECT_SLUG");
                var version = Environment.GetEnvironmentVariable("APPVEYOR_BUILD_VERSION");

                var job = $"{accountName}/{slug}/{version}";

                return !string.IsNullOrWhiteSpace(job) ? Uri.EscapeDataString(job) : string.Empty;
            }
        }
    }
}