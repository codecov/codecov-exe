﻿using System;
using Codecov.Utilities;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class AppVeyor : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("APPVEYOR_REPO_BRANCH"));
        private readonly Lazy<string> _build = new Lazy<string>(LoadBuild);
        private readonly Lazy<string> _commit = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("APPVEYOR_REPO_COMMIT"));
        private readonly Lazy<bool> _detecter = new Lazy<bool>(LoadDetecter);
        private readonly Lazy<string> _job = new Lazy<string>(LoadJob);
        private readonly Lazy<string> _pr = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER"));
        private readonly Lazy<string> _slug = new Lazy<string>(() => EnvironmentVariable.GetEnvironmentVariable("APPVEYOR_REPO_NAME"));

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Job => _job.Value;

        public override string Pr => _pr.Value;

        public override string Service => "appveyor";

        public override string Slug => _slug.Value;

        private static string LoadBuild()
        {
            var build = EnvironmentVariable.GetEnvironmentVariable("APPVEYOR_JOB_ID");
            return !string.IsNullOrWhiteSpace(build) ? Uri.EscapeDataString(build) : string.Empty;
        }

        private static bool LoadDetecter()
        {
            var appVeyor = EnvironmentVariable.GetEnvironmentVariable("APPVEYOR");
            var ci = EnvironmentVariable.GetEnvironmentVariable("CI");

            if (string.IsNullOrWhiteSpace(appVeyor) || string.IsNullOrWhiteSpace(ci))
            {
                return false;
            }

            return appVeyor.Equals(ci, StringComparison.Ordinal)
                   && appVeyor.Equals("True", StringComparison.OrdinalIgnoreCase);
        }

        private static string LoadJob()
        {
            var accountName = EnvironmentVariable.GetEnvironmentVariable("APPVEYOR_ACCOUNT_NAME");
            var slug = EnvironmentVariable.GetEnvironmentVariable("APPVEYOR_PROJECT_SLUG");
            var version = EnvironmentVariable.GetEnvironmentVariable("APPVEYOR_BUILD_VERSION");

            if (string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(slug) || string.IsNullOrWhiteSpace(version))
            {
                return string.Empty;
            }

            var job = $"{accountName}/{slug}/{version}";

            return job;
        }
    }
}
