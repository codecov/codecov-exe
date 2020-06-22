using System;

namespace Codecov.Services.ContinuousIntegrationServers
{
    internal class GitHubAction : ContinuousIntegrationServer
    {
        private readonly Lazy<string> _branch;
        private readonly Lazy<string> _build;
        private readonly Lazy<string> _commit;
        private readonly Lazy<bool> _detecter;
        private readonly Lazy<string> _pr;
        private readonly Lazy<string> _slug;

        public GitHubAction(IEnviornmentVariables environmentVariables)
            : base(environmentVariables)
        {
            _branch = new Lazy<string>(LoadBranch);
            _build = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_RUN_ID"));
            _commit = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_SHA"));
            _detecter = new Lazy<bool>(() => CheckEnvironmentVariables("GITHUB_ACTIONS") || !string.IsNullOrWhiteSpace(GetEnvironmentVariable("GITHUB_ACTION")));
            _pr = new Lazy<string>(LoadPullRequest);
            _slug = new Lazy<string>(() => GetEnvironmentVariable("GITHUB_REPOSITORY"));
        }

        public override string Branch => _branch.Value;

        public override string Build => _build.Value;

        public override string BuildUrl => LoadBuildUrl();

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string Pr => _pr.Value;

        public override string Service => "github-actions";

        public override string Slug => _slug.Value;

        private static string ExtractSubstring(string text, string prefix, string postfix)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var textLength = text.Length;

            var startIndex = string.IsNullOrEmpty(prefix) ? 0 : text.IndexOf(prefix);
            var endIndex = string.IsNullOrEmpty(postfix) ? textLength : text.IndexOf(postfix);
            if (startIndex == -1)
            {
                startIndex = 0;
            }
            else if (!string.IsNullOrEmpty(prefix))
            {
                startIndex += prefix.Length;
            }

            if (endIndex == -1)
            {
                endIndex = textLength;
            }

            return text.Substring(startIndex, endIndex - startIndex);
        }

        private string LoadBranch()
        {
            var headRef = GetEnvironmentVariable("GITHUB_HEAD_REF");
            if (!string.IsNullOrWhiteSpace(headRef))
            {
                return headRef;
            }

            var branch = GetEnvironmentVariable("GITHUB_REF");

            return ExtractSubstring(branch, "refs/heads/", null);
        }

        private string LoadPullRequest()
        {
            var headRef = GetEnvironmentVariable("GITHUB_HEAD_REF");
            if (string.IsNullOrEmpty(headRef))
            {
                return string.Empty;
            }

            var branchRef = GetEnvironmentVariable("GITHUB_REF");

            return string.IsNullOrEmpty(branchRef)
                ? string.Empty
                : ExtractSubstring(branchRef, "refs/pull/", "/merge");
        }

        private string LoadBuildUrl()
        {
            return (string.IsNullOrWhiteSpace(Slug) || string.IsNullOrWhiteSpace(Build))
                ? string.Empty
                : $"https://github.com/{Slug}/actions/runs/{Build}";
        }
    }
}
