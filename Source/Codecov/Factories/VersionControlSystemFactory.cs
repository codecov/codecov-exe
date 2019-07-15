using System.Linq;
using Codecov.Services.VersionControlSystems;
using Codecov.Terminal;

namespace Codecov.Factories
{
    internal static class VersionControlSystemFactory
    {
        public static IVersionControlSystem Create(IVersionControlSystemOptions options, ITerminal terminal)
        {
            var versionControlSystems = new[] { new Git(options, terminal) };
            var vcs = versionControlSystems.FirstOrDefault(x => x.Detecter);
            return vcs ?? new VersionControlSystem(options, terminal);
        }
    }
}
