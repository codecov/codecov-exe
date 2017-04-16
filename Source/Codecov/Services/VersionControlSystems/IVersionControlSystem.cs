using System.Collections.Generic;
using Codecov.Factories;

namespace Codecov.Services.VersionControlSystems
{
    internal interface IVersionControlSystem : IRepository, IDetect
    {
        string RepoRoot { get; }

        IEnumerable<string> SourceCode { get; }
    }
}