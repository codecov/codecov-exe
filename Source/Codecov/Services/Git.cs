using System.Text.RegularExpressions;
using Codecov.Program;
using Codecov.Services.Helpers;

namespace Codecov.Services
{
    internal class Git : Service
    {
        public Git(Options options) : base(options)
        {
        }

        public override bool Detect
        {
            get
            {
                var isGit = !string.IsNullOrWhiteSpace(Utils.RunCmd("git", "rev-parse --git-dir"));
                if (!isGit)
                {
                    return false;
                }   
                return true;
            }
        }

        public override void SetQueryParams()
        {
            Log.X("No CI provider detected, using Git.");
            QueryParameters["branch"] = Branch;
            QueryParameters["commit"] = Commit;
            QueryParameters["slug"] = Slug;
        }

        private string Branch
        {
            get
            {
                var branch = RunGit(@"rev-parse --abbrev-ref HEAD");
                if (branch != null && branch.Equals("HEAD"))
                {
                    return string.Empty;
                }

                return !string.IsNullOrWhiteSpace(branch) ? branch : string.Empty;
            }
        }

        private string Commit
        {
            get
            {
                var commit = RunGit(@"rev-parse HEAD");
                return !string.IsNullOrWhiteSpace(commit) ? commit : string.Empty;
            }
        }

        private string Slug
        {
            get
            {
                var remote = RunGit("config --get remote.origin.url");
                var regex = new Regex(@"\b/.*/.*$");
                var match = regex.Match(remote);
                if (match.Success)
                {
                    var m = match.Value;
                    m = m.TrimStart('/');
                    m = m.TrimEnd('t');
                    m = m.TrimEnd('i');
                    m = m.TrimEnd('g');
                    m = m.TrimEnd('.');
                    return System.Net.WebUtility.UrlEncode(m);
                }

                return string.Empty;
            }
        }

        private string RunGit(string commandArguments)
        {
            return Utils.RunCmd("git", $"-C {RepoRoot} {commandArguments}");
        }
    }
}