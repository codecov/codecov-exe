using System.Collections.Generic;
using Codecov.Services;
using Codecov.Services.ContinuousIntegrationServers;
using Codecov.Services.VersionControlSystems;

namespace Codecov.Factories
{
    internal static class RepositoryFactory
    {
        public static IEnumerable<IRepository> Create(IVersionControlSystem versionControlSystem, IContinuousIntegrationServer continuousIntegrationServer)
        {
            if (continuousIntegrationServer.Detecter)
            {
                yield return continuousIntegrationServer;
            }

            yield return versionControlSystem;
        }
    }
}
