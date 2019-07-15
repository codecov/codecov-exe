using System;
using System.Collections.Generic;
using System.Linq;
using Codecov.Services.VersionControlSystems;
using Codecov.Utilities;

namespace Codecov.Coverage.SourceCode
{
    internal class SourceCode : ISourceCode
    {
        private readonly Lazy<string> _directory;
        private readonly IPathFilter _fileFilter;
        private readonly Lazy<IEnumerable<string>> _getAll;
        private readonly Lazy<IEnumerable<string>> _getAllButCodecovIgnored;

        public SourceCode(IVersionControlSystem versionControlSystem)
        {
            VersionControlSystem = versionControlSystem;
            _getAll = new Lazy<IEnumerable<string>>(() => VersionControlSystem.SourceCode.Select(FileSystem.NormalizedPath));
            _getAllButCodecovIgnored = new Lazy<IEnumerable<string>>(LoadGetAllButCodecovIgnored);
            _directory = new Lazy<string>(() => VersionControlSystem.RepoRoot);

            _fileFilter = BuildSourceFilter();
        }

        public string Directory => _directory.Value;

        public IEnumerable<string> GetAll => _getAll.Value;

        public IEnumerable<string> GetAllButCodecovIgnored => _getAllButCodecovIgnored.Value;

        private IVersionControlSystem VersionControlSystem { get; }

        private static IPathFilter BuildSourceFilter()
        {
            return new PathFilterBuilder()
                .PathContains(@"\.git")
                .PathContains(@"\bin\debug")
                .PathContains(@"\bin\release")
                .PathContains(@"\obj\debug")
                .PathContains(@"\obj\release")
                .PathContains(@"\.vscode")
                .PathContains(@"\.vs")
                .PathContains(@"\obj\project.assets.json")
                .PathContains(@"\virtualenv")
                .PathContains(@"\.virtualenv")
                .PathContains(@"\virtualenvs")
                .PathContains(@"\.virtualenvs")
                .PathContains(@"\env")
                .PathContains(@"\.env")
                .PathContains(@"\envs")
                .PathContains(@"\.envs")
                .PathContains(@"\venv")
                .PathContains(@"\.venv")
                .PathContains(@"\venvs")
                .PathContains(@"\.venvs")
                .PathContains(@"\build\lib")
                .PathContains(@"\.egg-info")
                .PathContains(@"\shunit2-2.1.6")
                .PathContains(@"\vendor")
                .PathContains(@"\js\generated\coverage")
                .PathContains(@"\__pycache__")
                .PathContains(@"\__pycache__")
                .PathContains(@"\node_modules")
                .PathContains(@".csproj.nuget.g.targets")
                .PathContains(".csproj.nuget.g.props")
                .FileHasExtension(".dll")
                .FileHasExtension(".exe")
                .FileHasExtension(".gif")
                .FileHasExtension(".jpg")
                .FileHasExtension(".jpeg")
                .FileHasExtension(".md")
                .FileHasExtension(".png")
                .FileHasExtension(".psd")
                .FileHasExtension(".ptt")
                .FileHasExtension(".pptx")
                .FileHasExtension(".numbers")
                .FileHasExtension(".pages")
                .FileHasExtension(".txt")
                .FileHasExtension(".xlsx")
                .FileHasExtension(".docx")
                .FileHasExtension(".doc")
                .FileHasExtension(".pdf")
                .FileHasExtension(".yml")
                .FileHasExtension(".yaml")
                .FileHasExtension(".gitignore")
                .Build();
        }

        private IEnumerable<string> LoadGetAllButCodecovIgnored()
        {
            return GetAll.Where(file => !_fileFilter.Matches(file));
        }
    }
}
