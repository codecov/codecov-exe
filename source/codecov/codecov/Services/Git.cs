using System.Text.RegularExpressions;
using codecov.Program;
using codecov.Services.Helpers;

namespace codecov.Services
{
    internal class Git : Service
    {
        public Git(Options options) : base(options)
        {
            QueryParameters["branch"] = Branch;
            QueryParameters["commit"] = Commit;
            QueryParameters["slug"] = Slug;
        }

        public override bool Detect
        {
            get
            {
                var isGit = !string.IsNullOrWhiteSpace(RunGit("--version"));
                if (!isGit)
                {
                    return false;
                }
                Log.X("No CI provider detected, using Git.");
                return true;
            }
        }

        public override string RepoRoot => !Utils.RemoveAllWhiteSpaceFromString(Options.Root).Equals(".") ? Utils.NormalizePath(Options.Root) : Utils.RunCmd("git", "rev-parse --show-toplevel");

        public override string SourceCodeFiles
        {
            get
            {
                Log.Message($"Project root: {RepoRoot}");
                return RunGit("ls-tree --full-tree -r HEAD --name-only");
            }
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