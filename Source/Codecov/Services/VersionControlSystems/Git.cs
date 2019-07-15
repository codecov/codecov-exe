using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codecov.Terminal;
using Codecov.Utilities;

namespace Codecov.Services.VersionControlSystems
{
    internal class Git : VersionControlSystem
    {
        private readonly Lazy<string> _branch;
        private readonly Lazy<string> _commit;
        private readonly Lazy<bool> _detecter;
        private readonly Lazy<string> _repoRoot;
        private readonly Lazy<string> _slug;
        private readonly Lazy<IEnumerable<string>> _sourceCode;

        internal Git(IVersionControlSystemOptions options, ITerminal terminal)
            : base(options, terminal)
        {
            _branch = new Lazy<string>(LoadBranch);
            _commit = new Lazy<string>(LoadCommit);
            _repoRoot = new Lazy<string>(LoadRepoRoot);
            _slug = new Lazy<string>(LoadSlug);
            _sourceCode = new Lazy<IEnumerable<string>>(LoadSourceCode);
            _detecter = new Lazy<bool>(LoadDetecter);
        }

        public override string Branch => _branch.Value;

        public override string Commit => _commit.Value;

        public override bool Detecter => _detecter.Value;

        public override string RepoRoot => _repoRoot.Value;

        public override string Slug => _slug.Value;

        public override IEnumerable<string> SourceCode => _sourceCode.Value;

        private string LoadBranch()
        {
            var branch = RunGit(@"rev-parse --abbrev-ref HEAD");
            return string.IsNullOrWhiteSpace(branch) || branch.Equals("HEAD")
                ? string.Empty
                : branch;
        }

        private string LoadCommit()
        {
            var commit = RunGit(@"rev-parse HEAD");
            return !string.IsNullOrWhiteSpace(commit) ? commit : string.Empty;
        }

        private bool LoadDetecter()
            => !string.IsNullOrWhiteSpace(Terminal.Run("git", "--version"))
                && Directory.Exists(Path.Combine(RepoRoot, ".git"));

        private string LoadRepoRoot()
        {
            if (!Options.RepoRoot.RemoveAllWhiteSpace().Equals(string.Empty))
            {
                return FileSystem.NormalizedPath(Options.RepoRoot);
            }

            var root = Terminal.Run("git", "rev-parse --show-toplevel");
            return string.IsNullOrWhiteSpace(root) ? FileSystem.NormalizedPath(".") : FileSystem.NormalizedPath(root);
        }

        private string LoadSlug()
        {
            var remote = RunGit("config --get remote.origin.url");

            if (string.IsNullOrWhiteSpace(remote))
            {
                return string.Empty;
            }

            var splitColon = remote.Split(':');
            if (splitColon.Length <= 1)
            {
                return string.Empty;
            }

            var splitSlash = splitColon[1].Split('/');
            if (splitSlash.Length <= 1)
            {
                return string.Empty;
            }

            var slug = splitSlash[splitSlash.Length - 2] + "/" + splitSlash[splitSlash.Length - 1].TrimEnd('t').TrimEnd('i').TrimEnd('g').TrimEnd('.');
            return slug;
        }

        private IEnumerable<string> LoadSourceCode()
        {
            var sourceCode = RunGit("ls-tree --full-tree -r HEAD --name-only");
            return string.IsNullOrWhiteSpace(sourceCode) ?
                Enumerable.Empty<string>()
                : sourceCode.Trim('\n').Split('\n').Select(file => FileSystem.NormalizedPath(Path.Combine(RepoRoot, file)));
        }

        private string RunGit(string commandArguments) => Terminal.Run("git", $@"-C ""{RepoRoot}"" {commandArguments}");
    }
}
