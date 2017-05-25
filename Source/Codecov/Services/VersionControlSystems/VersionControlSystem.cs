using System;
using System.Collections.Generic;
using System.IO;
using Codecov.Terminal;
using Codecov.Utilities;

namespace Codecov.Services.VersionControlSystems
{
    internal class VersionControlSystem : IVersionControlSystem
    {
        private readonly Lazy<string> _branch = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("VCS_BRANCH_NAME"));
        private readonly Lazy<string> _commit = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("VCS_COMMIT_ID"));
        private readonly Lazy<string> _pr = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("VCS_PULL_REQUEST"));
        private readonly Lazy<string> _repoRoot;
        private readonly Lazy<string> _slug = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("VCS_SLUG"));
        private readonly Lazy<IEnumerable<string>> _sourceCode;
        private readonly Lazy<string> _tag = new Lazy<string>(() => EnviornmentVariable.GetEnviornmentVariable("VCS_TAG"));

        internal VersionControlSystem(IVersionControlSystemOptions options, ITerminal terminal)
        {
            Options = options;
            Terminal = terminal;
            _repoRoot = new Lazy<string>(() => !Options.RepoRoot.RemoveAllWhiteSpace().Equals(string.Empty) ? FileSystem.NormalizedPath(Options.RepoRoot) : FileSystem.NormalizedPath("."));
            _sourceCode = new Lazy<IEnumerable<string>>(LoadSourceCode);
        }

        public virtual string Branch => _branch.Value;

        public virtual string Commit => _commit.Value;

        public virtual bool Detecter => false;

        public virtual string Pr => _pr.Value;

        public virtual string RepoRoot => _repoRoot.Value;

        public virtual string Slug => _slug.Value;

        public virtual IEnumerable<string> SourceCode => _sourceCode.Value;

        public virtual string Tag => _tag.Value;

        protected IVersionControlSystemOptions Options { get; }

        protected ITerminal Terminal { get; }

        private IEnumerable<string> LoadSourceCode()
        {
            return Directory.EnumerateFiles(RepoRoot, "*.*", SearchOption.AllDirectories);
        }
    }
}